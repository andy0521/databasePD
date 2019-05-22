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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        OracleConnection con = null;
        public MainWindow()
        {
            this.setConnection();
            InitializeComponent();
            Player_Character_btn.IsEnabled = false;
        }
        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
     
            cmd.CommandText = "select*from player_character";
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
            try {
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
            String sql = "insert into player_character(player_id,nickname,player_level,specialization_id,weapon_id,armor_id)" +
                "values(:player_id,:nickname,:player_level,:specialization_id,:weapon_id,:armor_id)";
            this.AUD(sql,0);
            add_btn.IsEnabled =  false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {

            String sql = "update player_character set nickname = :nickname ," +
                " player_level= :player_level , specialization_id = :specialization_id , " +
                "weapon_id = :weapon_id , armor_id = :armor_id  " +
                "where player_id = :player_id";
            this.AUD(sql,1);

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql =  "delete from  player_character " +
                "where player_id = :player_id";
            this.AUD(sql,2);
            this.resetAll();
        }
        private void resetAll()
        {
            Player_Id_txbx.Text = "";
            Nickname_txbx.Text = "";
            Player_Level_txbx.Text = "";
            Specialization_Id_txbx.Text = "";
            Weapon_Id_txbx.Text = "";
            Armor_Id_txbx.Text = "";

            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            this.resetAll();
        }
        private void AUD (String sql_stmt, int statue)
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
                    cmd.Parameters.Add("Nickname", OracleDbType.Varchar2, 12).Value = Nickname_txbx.Text;
                    cmd.Parameters.Add("Player_Level", OracleDbType.Int32,2).Value = Int32.Parse(Player_Level_txbx.Text);
                    cmd.Parameters.Add("Specialization_id", OracleDbType.Int32, 2).Value = Int32.Parse(Specialization_Id_txbx.Text);
                    cmd.Parameters.Add("Weapon_Id", OracleDbType.Int32, 3).Value = Int32.Parse(Weapon_Id_txbx.Text);
                    cmd.Parameters.Add("Armor_id", OracleDbType.Int32, 3).Value = Int32.Parse(Armor_Id_txbx.Text);

                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("Nickname", OracleDbType.Varchar2, 12).Value = Nickname_txbx.Text;
                    cmd.Parameters.Add("Player_Level", OracleDbType.Int32, 2).Value = Int32.Parse(Player_Level_txbx.Text);
                    cmd.Parameters.Add("Specialization_id", OracleDbType.Int32, 2).Value = Int32.Parse(Specialization_Id_txbx.Text);
                    cmd.Parameters.Add("Weapon_Id", OracleDbType.Int32, 3).Value = Int32.Parse(Weapon_Id_txbx.Text);
                    cmd.Parameters.Add("Armor_id", OracleDbType.Int32, 3).Value = Int32.Parse(Armor_Id_txbx.Text);

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

                if(n  > 0)
                {

                MessageBox.Show(msg);
                this.updateDataGrid();
                }
            }
            catch(Exception expe)    {
                MessageBox.Show("Error");
                add_btn.IsEnabled = true;
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if(dr!= null)
            {
                Player_Id_txbx.Text = dr["player_id"].ToString();
                Nickname_txbx.Text = dr["nickname"].ToString();
                Player_Level_txbx.Text = dr["Player_Level"].ToString();
                Specialization_Id_txbx.Text = dr["Specialization_Id"].ToString();
                Weapon_Id_txbx.Text = dr["Weapon_Id"].ToString();
                Armor_Id_txbx.Text = dr["Armor_Id"].ToString();
                add_btn.IsEnabled = false;
                update_btn.IsEnabled = true;
                delete_btn.IsEnabled = true;
            }
        }

        private void Player_info_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window1().ShowDialog();
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
        }
    }
}
