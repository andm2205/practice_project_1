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
    /// Логика взаимодействия для IngredientsDecorationsPage.xaml
    /// </summary>
    public partial class IngredientsDecorationsPage : Page
    {
        private MySqlConnection connection;
        static string[] roles1 = { "Директор", "Менеджер по закупкам"};
        static string[] roles2 = { "Заказчик"};
        static string[] roles3 = { "Менеджер по работе с клиентами", "Мастер" };
        public IngredientsDecorationsPage()
        {
            InitializeComponent();
            connection = DBUtils.GetDBConnection();
            
            if(roles1.Contains(App.Current.Properties["userRole"]))
            {
                changeButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
                printButton.IsEnabled = true;
            }
            else if (roles2.Contains(App.Current.Properties["userRole"]))
            {
                
            }
            else if (roles3.Contains(App.Current.Properties["userRole"]))
            {
                printButton.IsEnabled = true;
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new DirectorMainPage();
        }

        private void ingredientPrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Ошибка подключения к БД");
                return;
            }
            string query = $"SELECT * FROM ingredient;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            List<IngredientData> list = new List<IngredientData>();
            while (dataReader.Read())
            {
                list.Add(new IngredientData((IDataRecord)dataReader));
            }
            dataGrid.ItemsSource = list;
            connection.Close();
        }

        class IngredientData
        {
            public IngredientData(string id, string vendorCode, string name, string unit, string count, string mainProvider, string ingredientType, string purchasePrice, string gost, string packing, string characteristic)
            {
                this.Id = id;
                this.VendorCode = vendorCode;
                this.Name = name;
                this.Unit = unit;
                this.Count = count;
                this.MainProvider = mainProvider;
                this.IngredientType = ingredientType;
                this.PurchasePrice = purchasePrice;
                this.Gost = gost;
                this.Packing = packing;
                this.Characteristic = characteristic;
            }
            public IngredientData(IDataRecord dataRecord) : this(Convert.ToString(dataRecord[0]), Convert.ToString(dataRecord[1]), Convert.ToString(dataRecord[2]), Convert.ToString(dataRecord[3]), Convert.ToString(dataRecord[4]), Convert.ToString(dataRecord[5]), Convert.ToString(dataRecord[7]), Convert.ToString(dataRecord[8]), Convert.ToString(dataRecord[9]), Convert.ToString(dataRecord[10]), Convert.ToString(dataRecord[11]))
            { }
            public string Id { get; set; }
            public string VendorCode { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Count { get; set; }
            public string MainProvider { get; set; }
            public string IngredientType { get; set; }
            public string PurchasePrice { get; set; }
            public string Gost { get; set; }
            public string Packing { get; set; }
            public string Characteristic { get; set; }
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
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
            string query = $"UPDATE ingredient SET ";
            int b = query.Length;
            string[] sj = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text };
            string[] svalues = { textBox1.Text, $"'{textBox2.Text}'", $"'{textBox3.Text}'", textBox4.Text, textBox5.Text, $"'{textBox6.Text}'", textBox7.Text, $"'{textBox8.Text}'", $"'{textBox9.Text}'", $"'{textBox10.Text}'" };
            string[] scolumns = { "vendor_code", "name", "unit", "count", "main_provider", "ingredient_type", "purchase_price", "GOST", "packing", "characteristic" };
            for(int a = 0; a < svalues.Length; a++)
                if(!String.IsNullOrWhiteSpace(sj[a]))
                    query += $"{scolumns[a]} = {svalues[a]}, ";
            if (b == query.Length)
                return;
            query = query.Substring(0, query.Length - 2) + $" WHERE id = {textBox11.Text};";
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Ошибка изменения таблицы");
                connection.Close();
                return;
            }
            MessageBox.Show("Успешно изменено");
            connection.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Подтвердите удаление", "", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            try
            {
                connection.Open();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к БД");
                return;
            }
            string query = $"SELECT count FROM ingredient WHERE id = {textBox11.Text};";
            MySqlCommand command = new MySqlCommand(query, connection);
            string queryResult;
            try
            {
                queryResult = Convert.ToString(command.ExecuteScalar());
            }
            catch
            {
                MessageBox.Show("Ошибка проверки количества");
                connection.Close();
                return;
            }
            if (!(queryResult == null || queryResult == "0" || String.IsNullOrEmpty(queryResult)))
            {
                MessageBox.Show("Количество не равно нулю");
                return;
            }
            query = $"DELETE FROM ingredient WHERE id = {textBox11.Text};";
            command = new MySqlCommand(query, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Ошибка удаления");
                connection.Close();
                return;
            }
            MessageBox.Show("Успешно удалено");
            connection.Close();
        }
    }
}
