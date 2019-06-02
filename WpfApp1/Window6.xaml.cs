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
    /// Window6.xaml 的互動邏輯
    /// </summary>
    public partial class Window6 : Window
    {
        OracleConnection con = null;
        public Window6()
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
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "select*from player_level order by player_level" ;
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
            String sql = "select count(*) from player_level where player_level=:player_level";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("player_level", OracleDbType.NChar).Value = player_level_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow > 0)
            {
                String msg = "armor_id已被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
             sql = "insert into player_level(player_level,hp_base,mp_base,pda_base,mda_base,pde_base,mde_base)" +
                "values(:player_level,:hp_base,:mp_base,:pda_base,:mda_base,:pde_base,:mde_base)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from player_level where player_level=:player_level";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("player_level", OracleDbType.NChar).Value = player_level_txbx.Text.ToString();

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
                sql = "update player_level set hp_base = :hp_base ," +
                    " mp_base= :mp_base , pda_base = :pda_base , " +
                    "mda_base = :mda_base , pde_base = :pde_base, mde_base = :mde_base  " +
                    "where player_level = :player_level";
                this.AUD(sql, 1);

            }
        }
        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from player_level where player_level=:player_level";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("player_level", OracleDbType.NChar).Value = player_level_txbx.Text.ToString();

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
                sql = "delete from  player_level " +
                "where player_level = :player_level";
                this.AUD(sql, 2);
                this.resetAll();
            }
        }
        private void resetAll()
        {
            player_level_txbx.Text = "";
            hp_base_txbx.Text = "";
            mp_base_txbx.Text = "";
            pda_base_txbx.Text = "";
            mda_base_txbx.Text = "";
            pde_base_txbx.Text = "";
            mde_base_txbx.Text = "";

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
                    cmd.Parameters.Add("player_level", OracleDbType.Int32, 3).Value = Int32.Parse(player_level_txbx.Text);
                    cmd.Parameters.Add("hp_base", OracleDbType.Int32, 5).Value = Int32.Parse(hp_base_txbx.Text);
                    cmd.Parameters.Add("mp_base", OracleDbType.Int32, 5).Value = Int32.Parse(mp_base_txbx.Text);
                    cmd.Parameters.Add("pda_base", OracleDbType.Int32, 5).Value = Int32.Parse(pda_base_txbx.Text);
                    cmd.Parameters.Add("mda_base", OracleDbType.Int32, 5).Value = Int32.Parse(mda_base_txbx.Text);
                    cmd.Parameters.Add("pde_base", OracleDbType.Int32, 4).Value = Int32.Parse(pde_base_txbx.Text);
                    cmd.Parameters.Add("mde_base", OracleDbType.Int32, 4).Value = Int32.Parse(mde_base_txbx.Text);

                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("hp_base", OracleDbType.Int32,5 ).Value = Int32.Parse(hp_base_txbx.Text);
                    cmd.Parameters.Add("mp_base", OracleDbType.Int32, 5).Value = Int32.Parse(mp_base_txbx.Text);
                    cmd.Parameters.Add("pda_base", OracleDbType.Int32, 5).Value = Int32.Parse(pda_base_txbx.Text);
                    cmd.Parameters.Add("mda_base", OracleDbType.Int32, 5).Value = Int32.Parse(mda_base_txbx.Text);
                    cmd.Parameters.Add("pde_base", OracleDbType.Int32, 4).Value = Int32.Parse(pde_base_txbx.Text);
                    cmd.Parameters.Add("mde_base", OracleDbType.Int32, 4).Value = Int32.Parse(mde_base_txbx.Text);

                    cmd.Parameters.Add("player_level", OracleDbType.Int32, 3).Value = Int32.Parse(player_level_txbx.Text);


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("player_level", OracleDbType.Varchar2, 16).Value = player_level_txbx.Text;

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
                player_level_txbx.Text = dr["player_level"].ToString();
                hp_base_txbx.Text = dr["hp_base"].ToString();
                mp_base_txbx.Text = dr["mp_base"].ToString();
                pda_base_txbx.Text = dr["pda_base"].ToString();
                mda_base_txbx.Text = dr["mda_base"].ToString();
                pde_base_txbx.Text = dr["pde_base"].ToString();
                mde_base_txbx.Text = dr["mde_base"].ToString();
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
    }
}
