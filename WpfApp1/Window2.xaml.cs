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
            String sql = "insert into specialization(spec_id,spec_name,hp_weighted,mp_weighted,phy_damage_weighted,magic_damage_weighted,phy_defense_weighted,magic_defense_weighted,weapon_type_id) "  +
                "values( :spec_id, :spec_name, :hp_weighted, :mp_weighted, :phy_damage_weighted, :magic_damage_weighted, :phy_defense_weighted, :magic_defense_weighted, :weapon_type_id)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "update specialization set spec_name = :spec_name ," +
                " hp_weighted= :hp_weighted , mp_weighted = :mp_weighted , " + " phy_damage_weighted = :phy_damage_weighted, magic_damage_weighted = :magic_damage_weighted, phy_defense_weighted = :phy_defense_weighted,magic_defense_weighted = :magic_defense_weighted,weapon_type_id = :weapon_type_id " +
                " where spec_id = :spec_id " ;
            this.AUD(sql, 1);

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "delete from  specialization " +
                "where spec_id = :spec_id";
            this.AUD(sql, 2);
            this.resetAll();
        }
        private void resetAll()
        {
            spec_id_txbx.Text = "";
            spec_name_txbx.Text = "";
            hp_weight_txbx.Text = "";
            mp_weight_txbx.Text = "";
            phy_damage_txbx.Text = "";
            magic_damage_txbx.Text = "";
            phy_defense_txbx.Text = "";
            magic_defense_txbx.Text = "";
            weapon_type_txbx.Text = "";
          


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
                    cmd.Parameters.Add("spec_id", OracleDbType.Varchar2, 16).Value = spec_id_txbx.Text;
                    cmd.Parameters.Add("spec_name", OracleDbType.Varchar2, 16).Value = spec_name_txbx.Text;
                    cmd.Parameters.Add("hp_weighted", OracleDbType.Varchar2, 50).Value =hp_weight_txbx.Text;
                    cmd.Parameters.Add("mp_weighted", OracleDbType.Varchar2, 20).Value = mp_weight_txbx.Text;
                    cmd.Parameters.Add("phy_damage_weighted", OracleDbType.Varchar2, 20).Value = phy_damage_txbx.Text;
                    cmd.Parameters.Add("magic_damage_weighted", OracleDbType.Varchar2, 20).Value = magic_damage_txbx.Text;
                    cmd.Parameters.Add("phy_defense_weighted", OracleDbType.Varchar2, 20).Value = phy_defense_txbx.Text;
                    cmd.Parameters.Add("magic_defense_weighted", OracleDbType.Varchar2, 20).Value = magic_defense_txbx.Text;
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Varchar2, 20).Value = weapon_type_txbx.Text;


                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("spec_name", OracleDbType.Varchar2, 16).Value = spec_name_txbx.Text;
                    cmd.Parameters.Add("hp_weighted", OracleDbType.Varchar2, 50).Value = hp_weight_txbx.Text;
                    cmd.Parameters.Add("mp_weighted", OracleDbType.Varchar2, 20).Value = mp_weight_txbx.Text;
                    cmd.Parameters.Add("phy_damage_weighted", OracleDbType.Varchar2, 20).Value = phy_damage_txbx.Text;
                    cmd.Parameters.Add("magic_damage_weighted", OracleDbType.Varchar2, 20).Value = magic_damage_txbx.Text;
                    cmd.Parameters.Add("phy_defense_weighted", OracleDbType.Varchar2, 20).Value = phy_defense_txbx.Text;
                    cmd.Parameters.Add("magic_defense_weighted", OracleDbType.Varchar2, 20).Value = magic_defense_txbx.Text;
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Varchar2, 20).Value = weapon_type_txbx.Text;

                    cmd.Parameters.Add("spec_id", OracleDbType.Varchar2, 16).Value = spec_id_txbx.Text;


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("spec_id", OracleDbType.Varchar2, 16).Value = spec_id_txbx.Text;

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
                spec_id_txbx.Text = dr["spec_id"].ToString();
                spec_name_txbx.Text = dr["spec_name"].ToString();
                hp_weight_txbx.Text = dr["hp_weighted"].ToString();
                mp_weight_txbx.Text = dr["mp_weighted"].ToString();
                phy_damage_txbx.Text = dr["phy_damage_weighted"].ToString();
                magic_damage_txbx.Text = dr["magic_damage_weighted"].ToString();
                phy_defense_txbx.Text = dr["phy_defense_weighted"].ToString();
                magic_defense_txbx.Text = dr["magic_defense_weighted"].ToString();
                weapon_type_txbx.Text = dr["weapon_type_id"].ToString();

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
            mw.ShowDialog();
            this.Hide();
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
        private void Player_level_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
        }
    }
}
