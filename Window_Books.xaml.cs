using App_Library.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// <summary>
    /// Логика взаимодействия для Window_Books.xaml
    /// </summary>
    public partial class Window_Books : Window
    {
        //class StatusToColorConverter : IValueConverter
        //{
        //    //Цвета для выделения строк в базе данных
        //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        if ((DateTime?)value != null)
        //            return new SolidColorBrush(Colors.LightGray);
        //        return new SolidColorBrush(Colors.White);
        //    }


        //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //}
        //Ссылки на датасет и данные
        DataSet1 dataSet1;
        tb_booksTableAdapter tb_BooksTableAdapter;
        pr_booksTableAdapter pr_BooksTableAdapter;

        tb_authorsTableAdapter tb_AuthorsTableAdapter;
        tb_genresTableAdapter tb_GenresTableAdapter;
        tb_publish_housesTableAdapter tb_Publish_HousesTableAdapter;
        public Window_Books()
        {
            InitializeComponent();

            dataSet1 = new DataSet1();
            tb_BooksTableAdapter = new tb_booksTableAdapter();
            tb_BooksTableAdapter.Fill(dataSet1.tb_books);
            pr_BooksTableAdapter = new pr_booksTableAdapter();
            pr_BooksTableAdapter.Fill(dataSet1.pr_books);

            tb_AuthorsTableAdapter = new tb_authorsTableAdapter();
            tb_AuthorsTableAdapter.Fill(dataSet1.tb_authors);
            tb_GenresTableAdapter = new tb_genresTableAdapter();
            tb_GenresTableAdapter.Fill(dataSet1.tb_genres);
            tb_Publish_HousesTableAdapter = new tb_publish_housesTableAdapter();
            tb_Publish_HousesTableAdapter.Fill(dataSet1.tb_publish_houses);

            datagrid.ItemsSource = dataSet1.pr_books.DefaultView;
            datagrid.SelectedValuePath = "ID_Book";
            datagrid.CanUserAddRows = false;
            datagrid.CanUserDeleteRows = false;
            datagrid.SelectionMode = DataGridSelectionMode.Single;

            cb_author.ItemsSource = dataSet1.tb_authors.DefaultView;
            cb_author.SelectedValuePath = "ID_Authors";
            cb_author.SelectedIndex = -1;
            cb_genre.ItemsSource = dataSet1.tb_genres.DefaultView;
            cb_genre.SelectedValuePath = "ID_Genre";
            cb_genre.DisplayMemberPath = "Name_Genre";
            cb_genre.SelectedIndex = -1;
            cb_publish.ItemsSource = dataSet1.tb_publish_houses.DefaultView;
            cb_publish.SelectedValuePath = "ID_Publish_House";
            cb_publish.DisplayMemberPath = "Name_Publish_House";
            cb_publish.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((cb_publish.SelectedIndex != -1) && (cb_genre.SelectedIndex != -1) && (cb_author.SelectedIndex != -1) && (tb_name.Text != "") && (!tb_year.Text.Contains("_")) && (tb_price.Text != "") && (tb_count.Text != ""))
                {
                    DateTime? book_dec = null;
                    if (!tb_dateDec.Text.Contains("_")) book_dec = DateTime.ParseExact(tb_dateDec.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                    int id_genre = (int)cb_genre.SelectedValue;
                    int id_publish = (int)cb_publish.SelectedValue;
                    int id_author = (int)cb_author.SelectedValue;
                    string name = tb_name.Text;
                    int year = Convert.ToInt32(tb_year.Text);
                    decimal price = Convert.ToDecimal(tb_price.Text);
                    int count = Convert.ToInt32(tb_count.Text);
                    if ((price > 1) && (count > 1))
                    {
                        if (book_dec == null)
                        {
                            tb_BooksTableAdapter.Book_Insert(name, year, count, price, id_genre, id_author, id_publish, null);
                            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
                        }
                        else
                        {
                            tb_BooksTableAdapter.Book_Insert(name, year, count, price, id_genre, id_author, id_publish, book_dec);
                            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
                        }
                    }
                    else { MessageBox.Show(@"Ошибка! Проверьте цены и количества!"); }
                }
                else { MessageBox.Show(@"Ошибка! Проверьте ввод данных!"); }
            }
            catch { MessageBox.Show("Error!"); }

        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((datagrid.SelectedItem != null) && (cb_publish.SelectedIndex != -1) && (cb_genre.SelectedIndex != -1) && (cb_author.SelectedIndex != -1) && (tb_name.Text != "") && (!tb_year.Text.Contains("_")) && (tb_price.Text != "") && (tb_count.Text != ""))
                {
                    DateTime? book_dec = null;
                    if (!tb_dateDec.Text.Contains("_")) book_dec = DateTime.ParseExact(tb_dateDec.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                    int id_genre = (int)cb_genre.SelectedValue;
                    int id_publish = (int)cb_publish.SelectedValue;
                    int id_author = (int)cb_author.SelectedValue;
                    string name = tb_name.Text;
                    int year = Convert.ToInt32(tb_year.Text);
                    decimal price = Convert.ToDecimal(tb_price.Text);
                    int count = Convert.ToInt32(tb_count.Text);
                    int id = (int)datagrid.SelectedValue;
                    if ((price > 1) && (count > 1))
                    {
                        if (book_dec == null)
                        {
                            tb_BooksTableAdapter.Book_Update(id, name, year, count, price, id_genre, id_author, id_publish, null);
                            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
                        }
                        else
                        {
                            tb_BooksTableAdapter.Book_Update(id, name, year, count, price, id_genre, id_author, id_publish, book_dec);
                            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
                        }
                    }
                    else { MessageBox.Show(@"Ошибка! Проверьте цены и количества!"); }
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
                MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данные книг?", "Предупреждение!", btnMessageBox, MessageBoxImage.Question);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        {
                            tb_BooksTableAdapter.Book_Delete((int)datagrid.SelectedValue);
                            pr_BooksTableAdapter.Fill(dataSet1.pr_books);
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
                    tb_dateDec.Text = selectedRow.Row.ItemArray[13].ToString();
                    tb_name.Text = selectedRow.Row.ItemArray[1].ToString();
                    tb_year.Text = selectedRow.Row.ItemArray[10].ToString();
                    tb_price.Text = selectedRow.Row.ItemArray[12].ToString();
                    tb_count.Text = selectedRow.Row.ItemArray[11].ToString();


                    cb_genre.SelectedValue = selectedRow.Row.ItemArray[6];
                    cb_publish.SelectedValue = selectedRow.Row.ItemArray[8];
                    cb_author.SelectedValue = selectedRow.Row.ItemArray[2];
                }
            }
            catch { MessageBox.Show(@"Ошибка!"); }
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.Columns[1].Header = "Название";
            datagrid.Columns[0].Visibility = Visibility.Hidden; //Номер 
            datagrid.Columns[2].Visibility = Visibility.Hidden; //Номер автора
            datagrid.Columns[3].Header = "Фамилия";
            datagrid.Columns[4].Header = "Имя";
            datagrid.Columns[5].Header = "Отчество";
            datagrid.Columns[6].Visibility = Visibility.Hidden; //Номер жанра
            datagrid.Columns[7].Header = "Жанр";
            datagrid.Columns[8].Visibility = Visibility.Hidden; //Номер издательства
            datagrid.Columns[9].Header = "Издательство";
            datagrid.Columns[10].Header = "Год издания";
            datagrid.Columns[11].Header = "Количество";
            datagrid.Columns[12].Header = "Стоимость";
            datagrid.Columns[13].Header = "Дата списания";
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
