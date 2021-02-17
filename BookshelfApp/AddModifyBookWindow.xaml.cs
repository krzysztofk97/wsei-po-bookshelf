﻿using BookshelfLib;
using BookshelfLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookshelfApp
{
    public partial class AddModifyBookWindow : Window
    {
        private DBConnect dB = new DBConnect();
        private Book bookToModify = null;

        public AddModifyBookWindow(Book book = null)
        {
            bookToModify = book;

            InitializeComponent();

            ShelfsComboBox.ItemsSource = dB.GetShelfs();
            GeneresComboBox.ItemsSource = dB.GetGeneres();

            if (bookToModify != null)
            {
                TitleTextBox.Text = bookToModify.Title;
                PurchaseDateCalendar.SelectedDate = bookToModify.PurchaseDate;

                AddButton.Content = "Modyfikuj";
                this.Title = "Modyfikuj książkę";
            }
        }

        private void ManageShelfsButtonClick(object sender, RoutedEventArgs e)
        {
            ManageShelfsWindow manageShelfsWindow = new ManageShelfsWindow();
            manageShelfsWindow.Closing += ManageShelfsWindowClosing;
            manageShelfsWindow.ShowDialog();
        }

        private void ManageShelfsWindowClosing(object sender, EventArgs e)
        {
            ShelfsComboBox.ItemsSource = dB.GetShelfs();
        }

        private void ManageGeneresButtonClick(object sender, RoutedEventArgs e)
        {
            ManageGeneresWindow manageGeneresWindow = new ManageGeneresWindow();
            manageGeneresWindow.Closing += ManageGeneresWindowClosing;
            manageGeneresWindow.ShowDialog();
        }

        private void ManageGeneresWindowClosing(object sender, EventArgs e)
        {
            GeneresComboBox.ItemsSource = dB.GetGeneres();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            TitleTextBox.IsEnabled = false;
            PurchaseDateCalendar.IsEnabled = false;
            GeneresComboBox.IsEnabled = false;
            ManageGeneresButton.IsEnabled = false;
            ShelfsComboBox.IsEnabled = false;
            ManageShelfsButton.IsEnabled = false;

            if (bookToModify != null)
                ModifyAction();
            else
                AddAction();

            TitleTextBox.IsEnabled = true;
            PurchaseDateCalendar.IsEnabled = true;
            GeneresComboBox.IsEnabled = true;
            ManageGeneresButton.IsEnabled = true;
            ShelfsComboBox.IsEnabled = true;
            ManageShelfsButton.IsEnabled = true;

        }

        private void AddAction()
        {
            bool bookAddSuccess = true;

            try
            {
                dB.AddBook(
                    TitleTextBox.Text,
                    (DateTime)PurchaseDateCalendar.SelectedDate,
                    (Genere)GeneresComboBox.SelectedItem,
                    (Shelf)ShelfsComboBox.SelectedItem
                    );
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Nie wszystkie dane zotsały uzupełnione.",
                                "Błąd podczas dodawania książki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);

                bookAddSuccess = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Data zakupu książki nie może być z przyszłości.\nPopraw datę i spróbuj ponownie.",
                                "Błąd podczas dodawania książki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);

                bookAddSuccess = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + "\n\n" + ex.Message,
                                "Błąd podczas dodawania książki",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                bookAddSuccess = false;
            }

            if (bookAddSuccess)
                this.Close();
        }

        private void ModifyAction()
        {
            bool bookModifySuccess = true;

            try
            {
                dB.ModifyBook(
                    bookToModify,
                    TitleTextBox.Text,
                    (DateTime)PurchaseDateCalendar.SelectedDate,
                    (Genere)GeneresComboBox.SelectedItem,
                    (Shelf)ShelfsComboBox.SelectedItem
                    );
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Nie wszystkie dane zotsały uzupełnione.",
                                "Błąd podczas modyfikacji książki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);

                bookModifySuccess = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Data zakupu książki nie może być z przyszłości.\nPopraw datę i spróbuj ponownie.",
                                "Błąd podczas modyfikacji książki",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);

                bookModifySuccess = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + "\n\n" + ex.Message,
                                "Błąd podczas modyfikacji książki",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                bookModifySuccess = false;
            }

            if (bookModifySuccess)
                this.Close();
        }
    }
}