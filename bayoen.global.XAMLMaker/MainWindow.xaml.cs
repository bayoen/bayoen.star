using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

using ex = Microsoft.Office.Interop.Excel;

namespace bayoen.global.XAMLMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.CodeListBlock.Text = $"Support: {string.Join(", ", Config.LanguageCodeList)}";
        }


        public List<string> DictionaryList { get; private set; }

        private bool _isXlsxLoaded = false;
        public bool IsXlsxLoaded
        {
            get => this._isXlsxLoaded;
            set
            {
                if (this._isXlsxLoaded == value) return;

                this.ConvertButton.IsEnabled = value;
                this.ClearButton.IsEnabled = value;

                this._isXlsxLoaded = value;
            }
        }

        private ex::Application _excelApp;
        public ex::Application ExcelApp => this._excelApp ?? (this._excelApp = new ex::Application());
        public ex::Workbook ExcelWorkbook { get; private set; }

        private OpenFileDialog _openXlsxDialog;
        public OpenFileDialog OpenXlsxDialog => this._openXlsxDialog ?? (this._openXlsxDialog = new OpenFileDialog()
        {
            Filter = "Microsoft Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*",
        });

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.StatusBlock.Text = "Select Excel file to convert";
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Helper.ReleaseExcelObject(this.ExcelApp);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            bool? result = this.OpenXlsxDialog.ShowDialog();

            if (result.Value)
            {
                if (this.OpenXlsxDialog.CheckFileExists)
                {
                    this.ExcelWorkbook = this.ExcelApp.Workbooks.Open(this.OpenXlsxDialog.FileName, null, true);
                    this.IsXlsxLoaded = true;

                    this.StatusBlock.Text = "Loaded!";
                }
                else
                {
                    System.Media.SystemSounds.Hand.Play();
                    this.StatusBlock.Text = "File Missing";
                }
            }
            else
            {
                System.Media.SystemSounds.Hand.Play();
                this.StatusBlock.Text = "Dialog Broken";
            }
        }
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialze
            this.DictionaryList = new List<string>();
            DateTime now = DateTime.Now;
            for (int languageIndex = 0; languageIndex < Config.LanguageCodeList.Count; languageIndex++)
            {
                string code = Config.LanguageCodeList[languageIndex];
                this.DictionaryList.Add("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""
                + Environment.NewLine + "                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\""
                + Environment.NewLine + "                    xmlns:system=\"clr-namespace:System;assembly=mscorlib\">"
                + Environment.NewLine
                + Environment.NewLine + $"    <!-- Localization.{code}: {(new CultureInfo(code)).EnglishName}   {now:yyyy-MM-dd HH:mm} KST -->");
            }

            foreach (ex::Worksheet tempSheet in this.ExcelWorkbook.Worksheets)
            {
                string name = tempSheet.Name;

                for (int languageIndex = 0; languageIndex < Config.LanguageCodeList.Count; languageIndex++)
                {
                    this.DictionaryList[languageIndex] += Environment.NewLine + Environment.NewLine + $"    <!-- for {name} -->";
                }


                int columnStartNumber = Helper.AlphabetToNumber(Config.ColumnStartLetter);
                List<string> ColumnList = Enumerable.Range(0, Config.LanguageCodeList.Count).ToList()
                    .ConvertAll(x => Helper.NumberToAlphabet(columnStartNumber + x * Config.ColumnOffsetUnit));

                for (int sheetRow = 2; sheetRow < 1000; sheetRow++)
                {
                    string rangeString = $"A{sheetRow}:{ColumnList.Last()}{sheetRow}";
                    ex::Range tempRange = tempSheet.Range[rangeString];

                    int order;
                    try
                    {
                        order = (int)(tempRange.Cells[1, 1] as ex::Range).Value;
                    }
                    catch
                    {
                        break;
                    }
                    
                    string middleKey = "";
                    for (int keyLocation = 3; keyLocation <=6; keyLocation++)
                    {
                        string tokenString = (tempRange.Cells[1, keyLocation] as ex::Range).Value as string;

                        if (tokenString != null)
                        {
                            middleKey += $"-{tokenString}";
                        }
                        else
                        {
                            try
                            {
                                middleKey += $"-{(int)(tempRange.Cells[1, keyLocation] as ex::Range).Value}";
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    if (middleKey.Length == 0) continue;

                    string fullKey = $"{name}{middleKey}-String";
                    string englishEval = null;
                    for (int languageIndex = 0; languageIndex < Config.LanguageCodeList.Count; languageIndex++)
                    {
                        if (fullKey.Length == 0)
                        {
                            // line break
                            this.DictionaryList[languageIndex] += Environment.NewLine;
                        }
                        else
                        {
                            bool missingFlag = false;
                            string eval = (tempRange.Cells[1, Helper.AlphabetToNumber(ColumnList[languageIndex])] as ex::Range).Value as string;

                            if (eval == null) missingFlag = true;
                            else if (eval.Length == 0) missingFlag = true;

                            if (languageIndex == Config.EnglishIndex)
                            {
                                englishEval = eval;
                            }
                            else
                            {
                                if (missingFlag)
                                {
                                    if (englishEval == null) missingFlag = true;
                                    else if (englishEval.Length == 0) missingFlag = true;
                                    else missingFlag = false;

                                    eval = englishEval;
                                }
                            }

                            if (missingFlag)
                            {
                                this.DictionaryList[languageIndex] += Environment.NewLine + $"    <!-- '{fullKey}' is empty -->";
                            }
                            else
                            {
                                bool preserveFlag = false;
                                string control = (tempRange.Cells[1, Helper.AlphabetToNumber(Config.ControlColumnLetter)] as ex::Range).Value as string;

                                if (control == null) preserveFlag = false;
                                else if (control.Length == 0) preserveFlag = false;
                                else if (control.Contains(Config.PreserveSpaceControlString)) preserveFlag = true;

                                this.DictionaryList[languageIndex] += Environment.NewLine + string.Format(preserveFlag ? Config.StringPreserveSpaceItemSeed : Config.StringItemSeed, fullKey, eval);
                            }
                        }
                    }

                }

                Helper.ReleaseExcelObject(tempSheet);
            }

            for (int languageIndex = 0; languageIndex < Config.LanguageCodeList.Count; languageIndex++)
            {
                this.DictionaryList[languageIndex] += Environment.NewLine + "</ResourceDictionary>";

                File.WriteAllText(
                    string.Format(Config.ExportFileNameSeed, Config.LanguageCodeList[languageIndex]),
                    this.DictionaryList[languageIndex]);
            }

            this.ExcelWorkbook.Close(false);
            this.ExcelApp.Workbooks.Close();
            this.ExcelApp.Quit();

            Helper.ReleaseExcelObject(this.ExcelWorkbook);

            System.Media.SystemSounds.Exclamation.Play();
            this.StatusBlock.Text = "Converted!";
        }
        private void CleartButton_Click(object sender, RoutedEventArgs e)
        {            
            this.ExcelWorkbook.Close(false);
            this.ExcelApp.Workbooks.Close();
            this.ExcelApp.Quit();

            this.IsXlsxLoaded = false;

            this.StatusBlock.Text = "Cleared";
        }
    }
}
