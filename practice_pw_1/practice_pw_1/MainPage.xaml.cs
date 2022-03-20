using MySql.Data.MySqlClient;
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
using System.Windows.Threading;

namespace practice_pw_1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private MySqlConnection connection;
        private uint numberOfAttempts;
        private DispatcherTimer timer;
        public MainPage()
        {
            InitializeComponent();
            connection = DBUtils.GetDBConnection();
            numberOfAttempts = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
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
            string query = "SELECT * FROM user WHERE login = '" + textBox1.Text + "' AND password = '" + textBox2.Password + "';";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader userData;
            try
            {
                userData = command.ExecuteReader();
                userData.Read();
                _ = userData[4].ToString();
            }
            catch
            {
                MessageBox.Show("Неверный логин или пароль");
                connection.Close();
                numberOfAttempts++;
                if (numberOfAttempts == 3)
                {
                    textBox1.IsEnabled = false;
                    textBox2.IsEnabled = false;
                    button1.IsEnabled = false;
                    timer = new DispatcherTimer();
                    timer.Tick += new EventHandler(TimerTick);
                    timer.Interval = new TimeSpan(0, 0, 5);
                    timer.Start();
                    numberOfAttempts = 0;
                }
                return;
            }
            MessageBox.Show("Здравствуйте, " + userData[4].ToString());
            App.Current.Properties["userRole"] = userData[3].ToString();
            switch (App.Current.Properties["userRole"])
            {
                case "Заказчик":
                    {
                        Window.GetWindow(this).Content = new CustomerMainPage();
                        break;
                    }
                case "Менеджер по продажам":
                    {
                        Window.GetWindow(this).Content = new ManagerMainPage();
                        break;
                    }
                case "Менеджер по закупкам":
                    {
                        Window.GetWindow(this).Content = new PurchasingManagerMainPage();
                        break;
                    }
                case "Мастер":
                    {
                        Window.GetWindow(this).Content = new MasterMainPage();
                        break;
                    }
                case "Директор":
                    {
                        Window.GetWindow(this).Content = new DirectorMainPage();
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Неизвестная роль");
                        break;
                    }
            }
            connection.Close();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;
            button1.IsEnabled = true;
            timer.Stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Registration();
        }
    }
}
