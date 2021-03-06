﻿using System;
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
    /// Window3.xaml 的互動邏輯
    /// </summary>
    public partial class Window3 : Window
    {
        OracleConnection con = null;
       private String sql =  "select*from weapon_type order by weapon_type_id";
        public Window3()
        {
            this.setConnection();
            InitializeComponent();
            this.updateDataGrid();
            weapon_type_btn.IsEnabled = false;
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
            String sql = "select count(*) from weapon_type where weapon_type_id=:weapon_type_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_type_id", OracleDbType.NChar).Value = weapon_type_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow > 0)
            {
                String msg = "weapon_type_id已被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            sql = "insert into weapon_type(weapon_type_id, weapon_class_id ,weapon_type_name)" +
                "values(:weapon_type_id,:weapon_class_id,:weapon_type_name)";
            this.AUD(sql, 0);
            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
            delete_btn.IsEnabled = true;


        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
             sql = "select count(*) from weapon_type where weapon_type_id=:weapon_type_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_type_id", OracleDbType.NChar).Value = weapon_type_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此weapon_type_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "update weapon_type set  weapon_class_id = : weapon_class_id ,weapon_type_name = :weapon_type_name " +
                    "where weapon_type_id = :weapon_type_id";
                this.AUD(sql, 1);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            String sql = "select count(*) from weapon_type where weapon_type_id=:weapon_type_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_type_id", OracleDbType.NChar).Value = weapon_type_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此weapon_type_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "delete from  weapon_type " +
               "where weapon_type_id = :weapon_type_id";
                this.AUD(sql, 2);
                this.resetAll();
            }
        }
        private void resetAll()
        {
            weapon_type_id_txbx.Text = "";
            weapon_class_id_txbx.Text = "";
            weapon_type_name_txbx.Text = "";
     

            add_btn.IsEnabled = true;
            update_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            this.resetAll();
            sql = "select*from weapon_type order by weapon_type_id";
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
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_type_id_txbx.Text);
                    cmd.Parameters.Add("weapon_class_id", OracleDbType.Int32, 3).Value = Int32.Parse(weapon_class_id_txbx.Text);
                    cmd.Parameters.Add("weapon_type_name", OracleDbType.Varchar2, 20).Value = weapon_type_name_txbx.Text;
                

                    break;
                case 1:
                    msg = "Row Update Successfully!";
                    cmd.Parameters.Add("weapon_class_id", OracleDbType.Int32, 3).Value = Int32.Parse(weapon_class_id_txbx.Text);
                    cmd.Parameters.Add("weapon_type_name", OracleDbType.Varchar2, 20).Value = weapon_type_name_txbx.Text;
                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_type_id_txbx.Text);


                    break;
                case 2:
                    msg = "Row Delete Successfully!";

                    cmd.Parameters.Add("weapon_type_id", OracleDbType.Int32, 4).Value = Int32.Parse(weapon_type_id_txbx.Text);

                    break;

            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {

                    MessageBox.Show(msg);
                    sql = "select*from weapon_type order by weapon_type_id";
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
                weapon_type_id_txbx.Text = dr["weapon_type_id"].ToString();
                weapon_class_id_txbx.Text = dr["weapon_class_id"].ToString();
                weapon_type_name_txbx.Text = dr["weapon_type_name"].ToString();
            
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window8().ShowDialog();



        }
        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            sql = "select count(*) from weapon_type where weapon_type_id=:weapon_type_id";

            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add("weapon_type_id", OracleDbType.NChar).Value = weapon_type_id_txbx.Text.ToString();

            int datarow = Convert.ToInt32(cmd.ExecuteScalar());
            if (datarow == 0)
            {
                String msg = "此weapon_type_id未被使用";
                MessageBox.Show(msg);
                cmd.Cancel();


                return;
            }
            else
            {
                sql = "select * from weapon_type where weapon_type_id=:weapon_type_id";

                cmd = new OracleCommand(sql, con);

                cmd.Parameters.Add("weapon_type_id", OracleDbType.NChar).Value = weapon_type_id_txbx.Text.ToString();
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                myDataGrid.ItemsSource = dt.DefaultView;
                dr.Close();
                MessageBox.Show("search ok");
            }
        }
    }
}

