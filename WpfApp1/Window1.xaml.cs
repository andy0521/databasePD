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
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        OracleConnection con = null;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        public Window1()
        {
            this.setConnection();
            InitializeComponent();
            this.updateDataGrid();
            Player_info_btn.IsEnabled = false;
        }
        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from player_info";
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

            String sql = "select count(*) from player_info where player_id=:playerid";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("playerid", OracleDbType.NChar).Value = Player_Id_txbx.Text.ToString();
         
            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow>0)
            {
                String msg = "player_id已被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            sql = "insert into player_info(player_id,password,email,tel)" +
                "values(:player_id,:password,:email,:tel)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from player_character where player_id=:playerid";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("playerid", OracleDbType.NChar).Value = Player_Id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此player_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "update player_character set nickname = :nickname ," +
                   " player_level= :player_level , specialization_id = :specialization_id , " +
                   "weapon_id = :weapon_id , armor_id = :armor_id  " +
                   "where player_id = :player_id";
                this.AUD(sql, 1);
            }
            

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from player_character where player_id=:playerid";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("playerid", OracleDbType.NChar).Value = Player_Id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此player_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "delete from  player_info " +
                    "where player_id = :player_id";
                this.AUD(sql, 2);
                this.resetAll();
            }
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

                this.resetAll();
                MessageBox.Show("Error");
                

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
            
            MainWindow mw = new MainWindow();
            this.Hide();
            mw.Show();
            

        }

        private void Specialization_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window2().ShowDialog();
        }

        private void Weapon_type_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window3().ShowDialog();
        }

        private void Weapon_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window4().ShowDialog();
        }

        private void Armor_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window5().ShowDialog();
        }

        private void Player_level_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
        }

        private void Player_Id_txbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
  
        }
    }
}
