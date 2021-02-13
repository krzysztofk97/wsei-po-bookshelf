using System;
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
using BookshelfLib;
using BookshelfLib.Models;
using Microsoft.EntityFrameworkCore;

namespace BookshelfApp
{
    public partial class MainWindow : Window
    {
        private DBConnect dB = new DBConnect();

        public MainWindow()
        {
            InitializeComponent();

            BooksDataGrid.DataContext = dB.GetBooks();
        }

        private void AddBookButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ManageShelfsButtonClick(object sender, RoutedEventArgs e)
        {
            ManageShelfsWindow manageShelfsWindow = new ManageShelfsWindow();

            manageShelfsWindow.Show();
        }
    }
}
