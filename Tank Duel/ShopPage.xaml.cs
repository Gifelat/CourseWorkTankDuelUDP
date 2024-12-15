using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UDPProtocol;
using TankLibrary;
using System.Threading.Tasks;

namespace Tank_Duel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        private ISocket _socket;
        private Dictionary<int, int> _bulletCount;
        private float _gas;
        private int _armor;
        private double _money;
        private float _gasCount;

        private bool _isDone = false;
        private Player _player;

        public ShopPage(ISocket socket)
        {
            _socket = socket;

            InitializeComponent();

            if (_socket is Client client)
            {
                start.Content = "Не готов";
            }
            else if (_socket is Server server)
            {
                start.Content = "Начать игру";
            }

            _money = 1000;
            _bulletCount = new Dictionary<int, int>();

            _gas = 50;
            _armor = 0;

            _bulletCount.Add(1, 0);
            _bulletCount.Add(2, 0);
            _bulletCount.Add(3, 0);

            labelMoney.Content = _money.ToString();
            labelGas.Content = _gas.ToString();
            armorCount.Content = _armor.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_socket is Client client)
            {
                _isDone = !_isDone;
                if (_isDone)
                {
                    start.Content = "Готов";
                    Task.Run(ExpectationofGame);
                }
                else
                {
                    start.Content = "Не готов";
                }

                client.SendMessageAsync($"ClientReady {_isDone}");
            }
            else if (_socket is Server server)
            {
                if (server.ClientReady)
                {
                    server.SendMessageAsync($"GameTrue");
                    StartGame();
                }
            }
        }

        private void StartGame()
        {
            _player = new Player(_bulletCount, _gas, _armor);

            TankLibrary.App game = new TankLibrary.App(_socket, _player);

            Application.Current.MainWindow.IsEnabled = false;
            Application.Current.MainWindow.Visibility = Visibility.Collapsed;

            game.Run();
            game.Dispose();
        }

        private async void ExpectationofGame()
        {
            if (_socket is Client client)
            {
                while (_isDone)
                {
                    if (client.GameStart)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            _isDone = false;
                            StartGame();
                        });

                        break;
                    }
                }
            }
        }

        private void lightAmmoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_money >= 10)
            {
                _money -= 10;
                ++_bulletCount[1];
                label.Content = $"{_bulletCount[1]} {_bulletCount[2]} {_bulletCount[3]}";
                labelMoney.Content = _money.ToString();
                start.IsEnabled = true;
            }
        }

        private void normalAmmoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_money >= 20)
            {
                _money -= 20;
                ++_bulletCount[2];
                label.Content = $"{_bulletCount[1]} {_bulletCount[2]} {_bulletCount[3]}";
                labelMoney.Content = _money.ToString();
                start.IsEnabled = true;
            }
        }

        private void heavyAmmoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_money >= 20)
            {
                _money -= 20;
                ++_bulletCount[3];
                label.Content = $"{_bulletCount[1]} {_bulletCount[2]} {_bulletCount[3]}";
                labelMoney.Content = _money.ToString();
                start.IsEnabled = true;
            }
        }

        private void Gas1(object sender, RoutedEventArgs e)
        {
            if (_money >= 100)
            {
                _money -= 100;
                _gas += 200;
                labelMoney.Content = _money.ToString();
                labelGas.Content = _gas.ToString();
            }
        }


        private void armorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_money >= 100)
            {
                _money -= 100;
                _armor += 20;
                armorCount.Content = _armor.ToString();
            }
        }
    }
}
