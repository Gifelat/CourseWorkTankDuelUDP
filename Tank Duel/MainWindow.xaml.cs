using System.Windows;
using System.Windows.Controls;

namespace Tank_Duel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ButtonPage());
        }
    }
}
