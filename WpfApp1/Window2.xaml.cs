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
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Window2.xaml 的互動邏輯
    /// </summary>
    public partial class Window2 : Window
    {
        OracleConnection con = null;
        public Window2()
        {
            this.setConnection();
            InitializeComponent();
            this.updateDataGrid();
            specialization_btn.IsEnabled =false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "select*from specialization";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }
        private void setConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch { }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.updateDataGrid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            con.Close();
        }
        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "insert into player_info(spec_id,password,email,tel)" +
                "values(:player_id,:password,:email,:tel)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "update player_character set password = :password ," +
                " email= :email , tel = :tel  " +
                "where player_id = :player_id";
            this.AUD(sql, 1);

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "delete from  player_character " +
                "where player_id = :player_id";
            this.AUD(sql, 2);
            this.resetAll();
        }
        private void resetAll()
        {
            Player_Id_txbx.Text = "";
            Password_txbx.Text = "";
            email_txbx.Text = "";
            tel_txbx.Text = "";


            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            this.resetAll();
        }
        private void AUD(String sql_stmt, int statue)
        {
            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (statue)
            {
                case 0:
                    msg = "Row Inserted Successfully!";
                    cmd.Parameters.Add("Player_Id", OracleDbType.Varchar2, 16).Value = Player_Id_txbx.Text;
                    cmd.Parameters.Add("Password", OracleDbType.Varchar2, 16).Value = Password_txbx.Text;
                    cmd.Parameters.Add("email", OracleDbType.Varchar2, 50).Value = email_txbx.Text;
                    cmd.Parameters.Add("tel", OracleDbType.Varchar2, 20).Value = tel_txbx.Text;


                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("Password", OracleDbType.Varchar2, 16).Value = Password_txbx.Text;
                    cmd.Parameters.Add("email", OracleDbType.Varchar2, 50).Value = email_txbx.Text;
                    cmd.Parameters.Add("tel", OracleDbType.Varchar2, 20).Value = tel_txbx.Text;


                    cmd.Parameters.Add("Player_Id", OracleDbType.Varchar2, 16).Value = Player_Id_txbx.Text;


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("Player_Id", OracleDbType.Varchar2, 16).Value = Player_Id_txbx.Text;

                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {

                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }
            }
            catch (Exception expe)
            {

            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                Player_Id_txbx.Text = dr["player_id"].ToString();
                Password_txbx.Text = dr["Password"].ToString();
                email_txbx.Text = dr["email"].ToString();
                tel_txbx.Text = dr["tel"].ToString();

                add_btn.IsEnabled = false;
                update_btn.IsEnabled = true;
                delete_btn.IsEnabled = true;
            }
        }
        private void Player_info_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window1().Show();
        }

        private void Player_Character_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mw = new MainWindow();
            mw.ShowDialog();
        }
        private void Specialization_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window2().ShowDialog();
        }

        private void Weapen_type_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window3().ShowDialog();
        }

        private void Weapen_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window4().ShowDialog();
        }

        private void Armor_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

    }
}
