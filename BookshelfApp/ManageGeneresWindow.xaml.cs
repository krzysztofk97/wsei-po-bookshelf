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
    public partial class ManageGeneresWindow : Window
    {
        private DBConnect dB = new DBConnect();

        public ManageGeneresWindow()
        {
            InitializeComponent();

            GeneresDataGrid.DataContext = dB.GetGeneres();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddButton.IsEnabled = false;
            AddGenereTextBox.IsEnabled = false;

            try
            {
                dB.AddGenere(AddGenereTextBox.Text);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania nowego gatunku.", "Błąd podczas dodawania gatunku",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania nowego gatunku.\nUpewnij się, że gatunek, który próbujesz dodać nie został już dodany.", "Błąd podczas dodawania gatunku",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Nie można dodać gatunku bez nazwy.", "Błąd podczas dodawania gatunku",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + "\n\n" + ex.Message,
                                "Błąd podczas dodawania gatunku",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            AddGenereTextBox.Clear();

            GeneresDataGrid.DataContext = dB.GetGeneres();

            AddButton.IsEnabled = true;
            AddGenereTextBox.IsEnabled = true;
        }

        private void RemoveButton(object sender, RoutedEventArgs e)
        {
            try
            {
                dB.RemoveGenere((Genere)GeneresDataGrid.SelectedItem);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Wystąpił błąd podczas próby usunięcia gatunku.\nUpewnij się, że do gatunku, który chcesz usunąć nie ma przypisanej żadej książki, a następnie spróbuj ponownie.", 
                                "Błąd podczas usuwania gatunku",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + "\n\n" + ex.Message,
                                "Błąd podczas usuwania gatunku",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            GeneresDataGrid.DataContext = dB.GetGeneres();
        }
    }
}
