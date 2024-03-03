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
using System.Data;
using System.Data.SqlClient;

namespace Projekt_Programowanie_obiektowe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-AB94PO7\SQLEXPRESS;Initial Catalog=Serwis;Integrated Security=True");
        public void clearData()
        {
            idnaprawy_txt.Clear();
            model_txt.Clear();
            opusterki_txt.Clear();
            dane_txt.Clear();
        }
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from Klienci", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {

            clearData();
        }
        public bool isValid()
        {
            if (model_txt.Text == string.Empty)
            {
                MessageBox.Show("Wpisz model", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (opusterki_txt.Text == string.Empty)
            {
                MessageBox.Show("Wpisz opis usterki", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (dane_txt.Text == string.Empty)
            {
                MessageBox.Show("Wpisz dane klienta", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Klienci VALUES (@Model,@Opis, @Adres)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Model", model_txt.Text);
                cmd.Parameters.AddWithValue("@Opis", opusterki_txt.Text);
                cmd.Parameters.AddWithValue("@Adres", dane_txt.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                LoadGrid();
                MessageBox.Show("Zapisano");

                clearData();

            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Klienci where ID = " + idnaprawy_txt.Text + " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Wpis usunięty", "Usunięty", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Wpis nie usunięty" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Klienci set Model = '" + model_txt.Text + "'Opis ='" + opusterki_txt.Text + "', Dane = '" + dane_txt.Text + "' WHERE ID ='" + idnaprawy_txt.Text + "' ", con);
            try
            {

                cmd.ExecuteNonQuery();
                MessageBox.Show("Wiresz zaaktualizoway", "Zaaktualizowano", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }
        }
    }
}
