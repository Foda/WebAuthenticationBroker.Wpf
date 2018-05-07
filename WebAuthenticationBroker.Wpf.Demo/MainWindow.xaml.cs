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

namespace WebAuthenticationBroker.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            //Key     3SEANy8Ur7rSsNEPWD
            //Secret  A6LNqwA8RjnhCnWGwPMerqm7FHKNGwmw
            //http://localhost:58293/

            var startUri = new Uri(
                string.Format("https://bitbucket.org/site/oauth2/authorize?client_id={0}&response_type=token",
                "3SEANy8Ur7rSsNEPWD"));

            var endUri = new Uri("http://localhost:58293/");

            var result = await WebAuthenticationBroker.Desktop.WebAuthenticationBroker.AuthenticateAsync(
                startUri, endUri, this);

            Console.WriteLine("Done");
        }
    }
}
