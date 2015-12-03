using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFApp.ViewModel;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        MainViewModel viewmodel = new MainViewModel();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = viewmodel;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.BrowseForFolder();
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.ModifyINFFiles();
        }
    }
}
