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

namespace Tank_Duel
{
    /// <summary>
    /// Interaction logic for MultiPlayerPage.xaml
    /// </summary>
    public partial class ButtonPage : Page
    {
        public ButtonPage()
        {
            InitializeComponent();
        }

        private void createlobby_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateLobbyPage());
        }

        private void joinlobby_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JoinLobbyPage());
        }

        private void heavyAmmoButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
