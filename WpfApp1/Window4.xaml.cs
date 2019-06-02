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
    /// Window4.xaml 的互動邏輯
    /// </summary>
    public partial class Window4 : Window
    {
        OracleConnection con = null;
        public Window4()
        {
            this.setConnection();
            InitializeComponent();
            this.updateDataGrid();
            weapon_btn.IsEnabled = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "select*from weapon";
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
            String sql = "select count(*) from weapon where weapon_id=:weapon_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_id", OracleDbType.NChar).Value = weapon_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow > 0)
            {
                String msg = "weapon_id已被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
          sql = "insert into weapon(weapon_id,weapon_type_id,weapon_name,atk)" +
                "values(:weapon_id,:weapon_type_id,:weapon_name,:atk)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from weapon where weapon_id=:weapon_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_id", OracleDbType.NChar).Value = weapon_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此weapon_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "update weapon set weapon_type_id = :weapon_type_id ," +
                    " weapon_name= :weapon_name , atk = :atk  " +
                    "where weapon_id = :weapon_id";
                this.AUD(sql, 1);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from weapon where weapon_id=:weapon_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_id", OracleDbType.NChar).Value = weapon_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此weapon_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "delete from  weapon " +
                "where weapon_id = :weapon_id";
                this.AUD(sql, 2);
                this.resetAll();
            }
        }
        private void resetAll()
        {
            weapon_id_txbx.Text = "";
            weapon_type_id_txbx.Text = "";
            weapon_name_txbx.Text = "";
            atk_txbx.Text = "";


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
                    cmd.Parameters.Add("weapon_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_id_txbx.Text);
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_type_id_txbx.Text);
                    cmd.Parameters.Add("weapon_name", OracleDbType.Varchar2,20).Value = weapon_name_txbx.Text;
                    cmd.Parameters.Add("atk", OracleDbType.Int32, 4).Value = Int32.Parse(atk_txbx.Text);


                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_type_id_txbx.Text);
                    cmd.Parameters.Add("weapon_name", OracleDbType.Varchar2, 20).Value = weapon_name_txbx.Text;
                    cmd.Parameters.Add("atk", OracleDbType.Int32, 4).Value = Int32.Parse(atk_txbx.Text);


                    cmd.Parameters.Add("weapon_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_id_txbx.Text);


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("weapon_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_id_txbx.Text);

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
                weapon_id_txbx.Text = dr["weapon_id"].ToString();
                weapon_type_id_txbx.Text = dr["weapon_type_id"].ToString();
                weapon_name_txbx.Text = dr["weapon_name"].ToString();
                atk_txbx.Text = dr["atk"].ToString();

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
            this.Hide();
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
            new Window5().ShowDialog();
        }

        private void Player_level_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
        }
        private void Weapon_class_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window7().ShowDialog();
        }
    }
}

