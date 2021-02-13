using BookshelfLib;
using BookshelfLib.Models;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace BookshelfApp
{
    /// <summary>
    /// Logika interakcji dla klasy ManageShelfsWindow.xaml
    /// </summary>
    public partial class ManageShelfsWindow : Window
    {
        private DBConnect dB = new DBConnect();

        public ManageShelfsWindow()
        {
            InitializeComponent();

            ShelfsDataGrid.DataContext = dB.GetShelfs();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddButton.IsEnabled = false;
            AddShelfTextBox.IsEnabled = false;

            try
            {
                dB.AddShelf(AddShelfTextBox.Text);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania nowej półki.", "Błąd podczas dodawania półki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania nowej półki.\nUpewnij się, że półka, którą próbujesz dodać nie została już dodana.", "Błąd podczas dodawania półki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Nie można dodać półki bez nazwy.", "Błąd podczas dodawania półki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);
            }

            AddShelfTextBox.Clear();

            ShelfsDataGrid.DataContext = dB.GetShelfs();

            AddButton.IsEnabled = true;
            AddShelfTextBox.IsEnabled = true;
        }

        private void RemoveButton(object sender, RoutedEventArgs e)
        {
            try
            {
                dB.RemoveShelf((Shelf)ShelfsDataGrid.SelectedItem);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Wystąpił błąd podczas próby usunięcia półki.\nUpewnij się, że do półki, którą chcesz usunąć nie ma przypisanej żadej książki, a następnie spróbuj ponownie.", "Błąd podczas usuwania półki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);
            }

            ShelfsDataGrid.DataContext = dB.GetShelfs();
        }
    }
}
