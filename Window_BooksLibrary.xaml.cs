using App_Library.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App_Library
{
    public partial class Window_BooksLibrary : Window
    {
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_books_libraryTableAdapter tb_Books_LibraryTableAdapter;
        pr_books_libraryTableAdapter pr_Books_LibraryTableAdapter;

        tb_sectionsTableAdapter tb_SectionsTableAdapter;
        pr_booksTableAdapter pr_BooksTableAdapter;
        public Window_BooksLibrary()
        {
            InitializeComponent();
            dataSet1 = new DataSet1();
            tb_Books_LibraryTableAdapter = new tb_books_libraryTableAdapter();
            tb_Books_LibraryTableAdapter.Fill(dataSet1.tb_books_library);
            pr_Books_LibraryTableAdapter = new pr_books_libraryTableAdapter();
            pr_Books_LibraryTableAdapter.Fill(dataSet1.pr_books_library);

            tb_SectionsTableAdapter = new tb_sectionsTableAdapter();
            tb_SectionsTableAdapter.Fill(dataSet1.tb_sections);
            pr_BooksTableAdapter = new pr_booksTableAdapter();
            pr_BooksTableAdapter.Fill(dataSet1.pr_books);

            datagrid.ItemsSource = dataSet1.pr_books_library.DefaultView;
            datagrid.SelectedValuePath = "ID_Book_Library";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_section.ItemsSource = dataSet1.tb_sections.DefaultView;
            cb_section.SelectedValuePath = "ID_Section";
            cb_section.DisplayMemberPath = "Name_Section";
            cb_section.SelectedIndex = -1;
            cb_book.ItemsSource = dataSet1.pr_books.DefaultView;
            cb_book.SelectedValuePath = "ID_Book";
            cb_book.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_count.Text == "") tb_count.Text = "1";
                if ((cb_section.SelectedIndex != -1) && (cb_book.SelectedIndex != -1) && (tb_count.Text != "") && (!tb_shelf.Text.Contains("_")) && (!tb_rack.Text.Contains("_")))
                {
                    int id_book = (int)cb_book.SelectedValue;
                    int id_section = (int)cb_section.SelectedValue;
                    int count = Convert.ToInt32(tb_count.Text);
                    int shelf = Convert.ToInt32(tb_shelf.Text);
                    int rack = Convert.ToInt32(tb_rack.Text);
                    if (count > 0)
                    {
                        tb_Books_LibraryTableAdapter.BookLibrary_Insert(rack, shelf, id_book, id_section, count);
                        pr_Books_LibraryTableAdapter.Fill(dataSet1.pr_books_library);
                    }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }
            catch { MessageBox.Show("Erroe!"); }

        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_count.Text == "") tb_count.Text = "1";
                if ((datagrid.SelectedItem != null) && (cb_section.SelectedIndex != -1) && (cb_book.SelectedIndex != -1) && (tb_count.Text != "") && (!tb_shelf.Text.Contains("_")) && (!tb_rack.Text.Contains("_")))
                {
                    int id_book = (int)cb_book.SelectedValue;
                    int id_section = (int)cb_section.SelectedValue;
                    int count = Convert.ToInt32(tb_count.Text);
                    int shelf = Convert.ToInt32(tb_shelf.Text);
                    int rack = Convert.ToInt32(tb_rack.Text);
                    int id = (int)datagrid.SelectedValue;
                    if (count > 0)
                    {
                        tb_Books_LibraryTableAdapter.BookLibrary_Update(id,rack, shelf, id_book, id_section, count);
                        pr_Books_LibraryTableAdapter.Fill(dataSet1.pr_books_library);
                    }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные книг в библиотеке?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            tb_Books_LibraryTableAdapter.BookLibrary_Delete((int)datagrid.SelectedValue);
                            pr_Books_LibraryTableAdapter.Fill(dataSet1.pr_books_library);
                            break;
                        }
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)datagrid.SelectedItem;
                if (selectedRow != null)
                {
                    tb_count.Text = selectedRow.Row.ItemArray[11].ToString();
                    tb_shelf.Text = selectedRow.Row.ItemArray[2].ToString();
                    tb_rack.Text = selectedRow.Row.ItemArray[1].ToString();

                    cb_book.SelectedValue = selectedRow.Row.ItemArray[5];
                    cb_section.SelectedValue = selectedRow.Row.ItemArray[3];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[1].Header = "Номер стеллажа";
            datagrid.Columns[0].Visibility = Visibility.Hidden; //Номер 
            datagrid.Columns[2].Header = "Номер полки";
            datagrid.Columns[3].Visibility = Visibility.Hidden; //индекс секции
            datagrid.Columns[4].Header = "Секция";
            datagrid.Columns[5].Visibility = Visibility.Hidden; //индекс книги
            datagrid.Columns[6].Header = "Название";
            datagrid.Columns[7].Header = "Фамилия";
            datagrid.Columns[8].Header = "Имя";
            datagrid.Columns[9].Header = "Отчество";
            datagrid.Columns[10].Visibility = Visibility.Hidden; //дата списания
            datagrid.Columns[11].Header = "Количество";
            datagrid.Columns[12].Visibility = Visibility.Hidden; //индекс жанра
            datagrid.Columns[13].Visibility = Visibility.Hidden; //индекс автора
            datagrid.Columns[14].Visibility = Visibility.Hidden; //индекс издательства
        }
        private void btn_catalog_Click(object sender, RoutedEventArgs e)
        {
            Window_CatalogBook window = new Window_CatalogBook();
            window.Show();
            this.Close();
        }

        private void btn_profile_Click(object sender, RoutedEventArgs e)
        {
            Window_Profile window = new Window_Profile();
            window.Show();
            this.Close();
        }

        private void btn_authexit_Click(object sender, RoutedEventArgs e)
        {
            projectHelper.func_Auth();
        }

        private void btn_admin_Click(object sender, RoutedEventArgs e)
        {
            Window_MenuAdmin window = new Window_MenuAdmin();
            window.Show();
            this.Close();
        }

        private void btn_mtolibrary_Click(object sender, RoutedEventArgs e)
        {
            Window_MTOLibrary window = new Window_MTOLibrary();
            window.Show();
            this.Close();
        }
        private void btn_deliverymto_Click(object sender, RoutedEventArgs e)
        {
            Window_DeliveryMTO window = new Window_DeliveryMTO();
            window.Show();
            this.Close();
        }
        private void btn_applicants_Click(object sender, RoutedEventArgs e)
        {
            Window_Applicants window = new Window_Applicants();
            window.Show();
            this.Close();
        }
        private void btn_employees_Click(object sender, RoutedEventArgs e)
        {
            Window_Employees window = new Window_Employees();
            window.Show();
            this.Close();
        }
        private void btn_readers_Click(object sender, RoutedEventArgs e)
        {
            Window_Readers window = new Window_Readers();
            window.Show();
            this.Close();
        }
        private void btn_librarycards_Click(object sender, RoutedEventArgs e)
        {
            Window_LibraryCards window = new Window_LibraryCards();
            window.Show();
            this.Close();
        }
        private void btn_books_Click(object sender, RoutedEventArgs e)
        {
            Window_Books window = new Window_Books();
            window.Show();
            this.Close();
        }
        private void btn_bookslibrary_Click(object sender, RoutedEventArgs e)
        {
            Window_BooksLibrary window = new Window_BooksLibrary();
            window.Show();
            this.Close();
        }
        private void btn_extraditionbooks_Click(object sender, RoutedEventArgs e)
        {
            Window_ExtraditionBooks window = new Window_ExtraditionBooks();
            window.Show();
            this.Close();
        }
    }
}
