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
    /// Window8.xaml 的互動邏輯
    /// </summary>
    public partial class Window8 : Window
    {
        OracleConnection con = null;
        private string sql = " select * from player_character order by player_id";

        public Window8()
        {
            this.setConnection();
            InitializeComponent();
              this.updateDataGrid();
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
            datagrid.ItemsSource = dt.DefaultView;
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
       

        private void Print_Click(object sender, RoutedEventArgs e)
        {

             System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                datagrid.Measure(pageSize);
                datagrid.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(datagrid, Title);
            }
        }

        private void Player_character_data_Click(object sender, RoutedEventArgs e)
        {
            sql = "select*from player_character order by player_id";
            updateDataGrid();
        }

        private void Player_info_Click(object sender, RoutedEventArgs e)
        {
            sql = "select * from player_info order by player_id";
            updateDataGrid();
        }

        private void Specialization_Click(object sender, RoutedEventArgs e)
        {
            sql = " select* from specialization order by spec_id";
            updateDataGrid();
        }

        private void Weapon_type_Click(object sender, RoutedEventArgs e)
        {
            sql = " select*from weapon_type order by weapon_type_id";
            updateDataGrid();
        }

        private void Weapon_Click(object sender, RoutedEventArgs e)
        {
            sql = "select*from weapon order by weapon_id";
            updateDataGrid();
        }

        private void Armor_Click(object sender, RoutedEventArgs e)
        {
            sql = "select * from armor order by armor_id";
            updateDataGrid();
        }

        private void Player_Level_Click(object sender, RoutedEventArgs e)
        {
            sql = "select*from player_level order by player_level";
            updateDataGrid();
        }

        private void Weapon_class_Click(object sender, RoutedEventArgs e)
        {
            sql = "select*from weapon_class order by weapon_class_id";
            updateDataGrid();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new MainWindow().ShowDialog();
        }
    }
}
