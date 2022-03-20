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
using MySql.Data.MySqlClient;

namespace practice_pw_1
{
    public partial class Registration : Page
    {
        private MySqlConnection connection;
        public Registration()
        {
            InitializeComponent();
            connection = DBUtils.GetDBConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox3.Text) || String.IsNullOrWhiteSpace(textBox4.Password) ||
            String.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Одно из полей пустое");
                return;
            }
            if (!Validation(textBox4.Password, textBox5.Text))
            {
                MessageBox.Show("Пароль должен содержать от 5 до 20 символов, не должен содержать логин, должны встречаться заглавные буквы,должны встречаться маленькие буквы");
                return;
            }
            connection.Open();
            string query = $"SELECT * FROM user WHERE login = '{textBox3.Text}';";
            MySqlCommand command = new MySqlCommand(query, connection);
            if (command.ExecuteScalar() != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                connection.Close();
                return;
            }
            query = $"INSERT INTO user (login, password, role, full_name) VALUES ('{textBox3.Text}', '{textBox4.Password}', 'Заказчик', '{textBox5.Text}');";
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Пользователь успешно добавлен");
        }
        private bool Validation(string password, string login)
        {
            bool result = true;
            result &= password.Length <= 20 && password.Length >= 5;
            result &= !(password.Contains(login));
            int count = 0;
            foreach (char a in password)
                if (Char.IsUpper(a))
                {
                    count = 1;
                    break;
                }
            result &= count == 1;
            count = 0;
            foreach (char a in password)
                if (Char.IsDigit(a))
                {
                    count = 1;
                    break;
                }
            result &= count == 1;
            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new MainPage();
        }
    }
}
