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
using System.Windows.Navigation;
using System.Windows.Shapes;
using p4_projektwpf.ViewModels;

namespace p4_projektwpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy BooksView.xaml
    /// </summary>
    public partial class BooksView : UserControl
    {
        public BooksView()
        {
            InitializeComponent();
            DataContext = new BooksViewModel();
        }
    }
}
