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

namespace practice_pw_1
{
    /// <summary>
    /// Логика взаимодействия для DirectorPage.xaml
    /// </summary>
    public partial class DirectorMainPage : Page
    {
        public DirectorMainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new MainPage();
        }

        private void Button_Open_Registration_Page(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new ToolsRegistrationPage();
        }

        private void Button_Open_Ingredients_Decorations_Page_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new IngredientsDecorationsPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new DecorationsPage();
        }
    }
}
