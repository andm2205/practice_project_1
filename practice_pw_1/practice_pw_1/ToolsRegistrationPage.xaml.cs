using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace practice_pw_1
{
    /// <summary>
    /// Логика взаимодействия для ToolsRegistrationPage.xaml
    /// </summary>
    public partial class ToolsRegistrationPage : Page
    {
        private MySqlConnection connection;
        public ToolsRegistrationPage()
        {
            InitializeComponent();
            connection = DBUtils.GetDBConnection();
        }

        private void Button_Add_Tool_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к БД");
                return;
            }

            string query = $"SELECT id FROM provider WHERE name = '{provider.Text}';";
            MySqlCommand command = new MySqlCommand(query, connection);
            int id;
            try
            {
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                MessageBox.Show("Такого поставщика не существует");
                connection.Close();
                return;
            }
            query = $"INSERT INTO tools (name, description, tool_type, wear_rate, provider, purchase_date, amount) VALUES ('{name.Text}', '{description.Text}', '{toolType.Text}', '{wearRate.Text}', '{id}', '{purchaseDate.Text}', {amount.Text});";
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Инструмент успешно добавлен");
            printButton_Click(sender, e);
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к БД");
                return;
            }
            string query = $"SELECT * FROM tools;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            List<DataTable> list = new List<DataTable>();
            while (dataReader.Read())
            {
                list.Add(new DataTable((IDataRecord)dataReader));
            }
            dataGrid.ItemsSource = list;
            connection.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new DirectorMainPage();
        }

        class DataTable
        {
            public DataTable(string id, string name, string description, string toolType, string wearRate, string provider, string purchaseDate, string amount)
            {
                this.Id = id;
                this.Name = name;
                this.Description = description;
                this.ToolType = toolType;
                this.WearRate = wearRate;
                this.Provider = provider;
                this.PurchaseDate = purchaseDate;
                this.Amount = amount;
            }
            public DataTable(IDataRecord dataRecord) : this(Convert.ToString(dataRecord[0]), Convert.ToString(dataRecord[1]), Convert.ToString(dataRecord[2]), Convert.ToString(dataRecord[3]), Convert.ToString(dataRecord[4]), Convert.ToString(dataRecord[5]), Convert.ToString(dataRecord[6]), Convert.ToString(dataRecord[7]))
            { }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ToolType { get; set; }
            public string WearRate { get; set; }
            public string Provider { get; set; }
            public string PurchaseDate { get; set; }
            public string Amount { get; set; }
        }
    }
}
