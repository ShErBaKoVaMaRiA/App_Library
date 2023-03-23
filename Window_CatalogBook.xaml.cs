using App_Library.DataSet1TableAdapters;
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

namespace App_Library
{
    /// <summary>
    /// Логика взаимодействия для Window_CatalogBook.xaml
    /// </summary>
    public partial class Window_CatalogBook : Window
    {
        DataSet1 dataSet1;
        tb_books_libraryTableAdapter tb_Books_LibraryTableAdapter;
        pr_books_libraryTableAdapter pr_Books_LibraryTableAdapter;

        tb_genresTableAdapter tb_GenresTableAdapter;
        tb_authorsTableAdapter tb_AuthorsTableAdapter;
        tb_publish_housesTableAdapter tb_Publish_HousesTableAdapter;

        public Window_CatalogBook()
        {
            InitializeComponent();

            dataSet1 = new DataSet1();
            tb_Books_LibraryTableAdapter = new tb_books_libraryTableAdapter();
            tb_Books_LibraryTableAdapter.Fill(dataSet1.tb_books_library);
            pr_Books_LibraryTableAdapter = new pr_books_libraryTableAdapter();
            pr_Books_LibraryTableAdapter.Fill(dataSet1.pr_books_library);

            tb_GenresTableAdapter = new tb_genresTableAdapter();
            tb_GenresTableAdapter.Fill(dataSet1.tb_genres);
            tb_AuthorsTableAdapter = new tb_authorsTableAdapter();
            tb_AuthorsTableAdapter.Fill(dataSet1.tb_authors);
            tb_Publish_HousesTableAdapter = new tb_publish_housesTableAdapter();
            tb_Publish_HousesTableAdapter.Fill(dataSet1.tb_publish_houses);

            dg_bookslibrary.ItemsSource = dataSet1.pr_books_library.DefaultView;
            dg_bookslibrary.SelectedValuePath = "ID_Book_Library";
            dg_bookslibrary.CanUserAddRows = false;
            dg_bookslibrary.CanUserDeleteRows = false;
            dg_bookslibrary.SelectionMode = DataGridSelectionMode.Single;

            cb_genre.ItemsSource = dataSet1.tb_genres.DefaultView;
            cb_genre.SelectedValuePath = "ID_Genre";
            cb_genre.DisplayMemberPath = "Name_Genre";
            cb_genre.SelectedIndex = -1;
            cb_publish.ItemsSource = dataSet1.tb_publish_houses.DefaultView;
            cb_publish.SelectedValuePath = "ID_Publish_House";
            cb_publish.DisplayMemberPath = "Name_Publish_House";
            cb_publish.SelectedIndex = -1;
            cb_author.ItemsSource = dataSet1.tb_authors.DefaultView;
            cb_author.SelectedValuePath = "ID_Authors";
            cb_author.SelectedIndex = -1;
        }
        ProjectHelper projectHelper = new ProjectHelper();
        private void cb_filter_Click(object sender, RoutedEventArgs e)
        {
            int id_publish = cb_publish.SelectedIndex;
            int id_genre = cb_genre.SelectedIndex;
            int id_author = cb_author.SelectedIndex;
            string filter = "";
            if (id_publish > -1)
                filter = "Publish_House_ID = " + (int)cb_publish.SelectedValue;
            if ((id_genre > -1)&& (filter!=""))
                filter += " AND Genre_ID = " + (int)cb_genre.SelectedValue;
            else if ((id_genre > -1) && (filter == "")) filter = "Genre_ID = " + (int)cb_genre.SelectedValue;
            if ((id_author > -1) && (filter != ""))
                filter += " AND Authors_ID = " + (int)cb_author.SelectedValue;
            else if ((id_author > -1) && (filter == "")) filter = "Authors_ID = " + (int)cb_author.SelectedValue;
            if (filter != "")
                dataSet1.pr_books_library.DefaultView.RowFilter = filter;

        }

        private void cb_clearfilter_Click(object sender, RoutedEventArgs e)
        {
            cb_publish.SelectedIndex = -1;
            cb_genre.SelectedIndex = -1;
            cb_author.SelectedIndex = -1;
            dataSet1.pr_books_library.DefaultView.RowFilter = "ID_Book_Library = ID_Book_Library";
        }
        //Отображение таблицы БД
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dg_bookslibrary.Columns[1].Header = "Номер стеллажа";
            dg_bookslibrary.Columns[0].Visibility = Visibility.Hidden; //Номер 
            dg_bookslibrary.Columns[2].Header = "Номер полки";
            dg_bookslibrary.Columns[3].Visibility = Visibility.Hidden; //индекс секции
            dg_bookslibrary.Columns[4].Header = "Секция";
            dg_bookslibrary.Columns[5].Visibility = Visibility.Hidden; //индекс книги
            dg_bookslibrary.Columns[6].Header = "Название";
            dg_bookslibrary.Columns[7].Header = "Фамилия";
            dg_bookslibrary.Columns[8].Header = "Имя";
            dg_bookslibrary.Columns[9].Header = "Отчество";
            dg_bookslibrary.Columns[10].Visibility = Visibility.Hidden; //дата списания
            dg_bookslibrary.Columns[11].Header = "Количество";
            dg_bookslibrary.Columns[12].Visibility = Visibility.Hidden; //индекс жанра
            dg_bookslibrary.Columns[13].Visibility = Visibility.Hidden; //индекс автора
            dg_bookslibrary.Columns[14].Visibility = Visibility.Hidden; //индекс издательства
        }
        private void btn_analitic_Click(object sender, RoutedEventArgs e)
        {
            Window_Analitic window = new Window_Analitic();
            window.Show();
            this.Close();
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
