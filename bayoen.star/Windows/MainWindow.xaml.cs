﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = $"{Config.AssemblyTitle}";
        }
    }
}
