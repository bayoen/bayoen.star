using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.global.XAMLMaker
{
    public class Config
    {
        /// <summary>
        /// Seed for localization
        /// 0: Key
        /// 1: Value
        /// </summary>
        public const string StringItemSeed = "    <system:String x:Key=\"{0}\">{1}</system:String>";
        public const string StringPreserveSpaceItemSeed = "    <system:String x:Key=\"{0}\" xml:space=\"preserve\">{1}</system:String>";

        public const string ExportFileNameSeed = "Localization.{0}.xaml"; // {0} will be language code

        public static readonly List<string> LanguageCodeList = new List<string>()
        {
            "en",
            "ko",
            "ja",
            "es",
            "fr",
            "pt",
        };

        public const int EnglishIndex = 0;
        public const string ControlColumnLetter = "H";
        public const string ColumnStartLetter = "J";
        public const int ColumnOffsetUnit = 3;

        // ControlStrings
        public const string PreserveSpaceControlString = "PreserveSpace";
    }
}
