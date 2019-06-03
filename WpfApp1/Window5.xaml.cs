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
    /// Window5.xaml 的互動邏輯
    /// </summary>
    public partial class Window5 : Window
    {
        OracleConnection con = null;
       private String sql =   "select*from armor order by armor_id";
        public Window5()
        {
            this.setConnection();
            InitializeComponent();
     
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private void updateDataGrid()
        {
            OracleCommand cmd = new OracleCommand(sql, con);


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
            String sql = "select count(*) from armor where armor_id=:armor_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("armor_id", OracleDbType.NChar).Value = armor_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow > 0)
            {
                String msg = "armor_id已被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
             sql = "insert into armor(armor_id,armor_name,pd_weighted,md_weighted,speed_weighted,dodge_weighted,hp_plus,mp_plus,durability)" +
                "values(:armor_id,:armor_name,:pd_weighted,:md_weighted,:speed_weighted,:dodge_weighted,:hp_plus,:mp_plus,:durability)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
             sql = "select count(*) from armor where armor_id=:armor_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("armor_id", OracleDbType.NChar).Value = armor_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此armor_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "update armor set armor_name = :armor_name ," +
                     " pd_weighted= :pd_weighted , md_weighted = :md_weighted , " +
                     "speed_weighted = :speed_weighted , dodge_weighted = :dodge_weighted, " +
                       "hp_plus = :hp_plus , mp_plus = :mp_plus ," + "durability = :durability " +
                     "where armor_id = :armor_id";
                this.AUD(sql, 1);
            }

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from armor where armor_id=:armor_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("armor_id", OracleDbType.NChar).Value = armor_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此armor_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "delete from  armor " +
               "where armor_id = :armor_id";
                this.AUD(sql, 2);
                this.resetAll();
            }
        }

        private void resetAll()
        {
            armor_id_txbx.Text = "";
            armor_name_txbx.Text = "";
            pd_weighted_txbx.Text = "";
            md_weighted_txbx.Text = "";
            speed_weighted_txbx.Text = "";
            dodge_weighted_txbx.Text = "";
            hp_plus_txbx.Text = "";
            mp_plus_txbx.Text = "";
            durability_txbx.Text = "";

            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            this.resetAll();
            sql = "select*from armor order by armor_id";
            updateDataGrid();
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
                    cmd.Parameters.Add("armor_id", OracleDbType.Int32, 4).Value = Int32.Parse(armor_id_txbx.Text);
                    cmd.Parameters.Add("armor_name", OracleDbType.Varchar2, 30).Value = armor_name_txbx.Text;
                    cmd.Parameters.Add("pd_weighted", OracleDbType.Varchar2, 5).Value = pd_weighted_txbx.Text;
                    cmd.Parameters.Add("md_weighted", OracleDbType.Varchar2, 5).Value = md_weighted_txbx.Text;
                    cmd.Parameters.Add("speed_weighted", OracleDbType.Varchar2, 5).Value = speed_weighted_txbx.Text;
                    cmd.Parameters.Add("dodge_weighted", OracleDbType.Varchar2, 5).Value = dodge_weighted_txbx.Text;
                    cmd.Parameters.Add("hp_plus", OracleDbType.Int32, 5).Value = Int32.Parse(hp_plus_txbx.Text);
                    cmd.Parameters.Add("mp_plus", OracleDbType.Int32, 5).Value = Int32.Parse(mp_plus_txbx.Text);
                    cmd.Parameters.Add("durability", OracleDbType.Int32, 3).Value = Int32.Parse(durability_txbx.Text);
                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("armor_name", OracleDbType.Varchar2, 20).Value = armor_name_txbx.Text;
                    cmd.Parameters.Add("pd_weighted", OracleDbType.Varchar2, 5).Value = pd_weighted_txbx.Text;
                    cmd.Parameters.Add("md_weighted", OracleDbType.Varchar2, 5).Value = md_weighted_txbx.Text;
                    cmd.Parameters.Add("speed_weighted", OracleDbType.Varchar2, 5).Value = speed_weighted_txbx.Text;
                    cmd.Parameters.Add("dodge_weighted", OracleDbType.Varchar2, 5).Value = dodge_weighted_txbx.Text;
                    cmd.Parameters.Add("hp_plus", OracleDbType.Int32, 5).Value = Int32.Parse(hp_plus_txbx.Text);
                    cmd.Parameters.Add("mp_plus", OracleDbType.Int32, 5).Value = Int32.Parse(mp_plus_txbx.Text);
                    cmd.Parameters.Add("durability", OracleDbType.Int32, 3).Value = Int32.Parse(durability_txbx.Text);


                    cmd.Parameters.Add("armor_id", OracleDbType.Int32,4).Value = Int32.Parse(armor_id_txbx.Text);


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("armor_id", OracleDbType.Varchar2, 16).Value = armor_id_txbx.Text;

                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();

                if (n > 0)
                {

                    MessageBox.Show(msg);
                    sql = "select*from armor order by armor_id";
                    this.updateDataGrid();
                }
            }
            catch (Exception expe)
            {
                MessageBox.Show("Error");
                add_btn.IsEnabled = true;
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                armor_id_txbx.Text = dr["armor_id"].ToString();
                armor_name_txbx.Text = dr["armor_name"].ToString();
                pd_weighted_txbx.Text = dr["pd_weighted"].ToString();
                md_weighted_txbx.Text = dr["md_weighted"].ToString();
                speed_weighted_txbx.Text = dr["speed_weighted"].ToString();
                dodge_weighted_txbx.Text = dr["dodge_weighted"].ToString();
                hp_plus_txbx.Text = dr["hp_plus"].ToString();
                mp_plus_txbx.Text = dr["mp_plus"].ToString();
                durability_txbx.Text = dr["durability"].ToString();
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
            new Window5().ShowDialog();
        }

        private void Player_level_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
        }

        private void Player_Character_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.ShowDialog();
        }
        private void Weapon_class_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window7().ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window8().ShowDialog();



        }
        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            sql = "select count(*) from armor where armor_id=:armor_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("armor_id", OracleDbType.NChar).Value = armor_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此armor_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "select * from armor where armor_id=:armor_id";

                 cmd = new OracleCommand(sql, con);

                cmd.Parameters.Add("armor_id", OracleDbType.NChar).Value = armor_id_txbx.Text.ToString();
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                myDataGrid.ItemsSource = dt.DefaultView;
                dr.Close();
                MessageBox.Show("search ok");
            }
        }

        private void Hp_plus_txbx_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
