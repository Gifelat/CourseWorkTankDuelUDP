using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UDPProtocol;

namespace Tank_Duel
{
    public partial class JoinLobbyPage : Page
    {
        private ISocket _socket;
        public JoinLobbyPage()
        {
            InitializeComponent();
        }

        // Обработчик кнопки "Подключиться"
        private async void JoinLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = IpInput.Text.Trim();
            string portInput = PortInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(portInput))
            {
                MessageBox.Show("Введите IP-адрес и порт!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(portInput, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Введите корректный порт (1–65535).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _socket = Client.GetInstance("192.168.0.182", 8080, false);

                if (_socket is Client client)
                {
                    client.Connection(ipAddress, port, $"ping {client.GetAddress()}");
                    await Task.Delay(1000);
                    Task.Run(CheckConnection);
                }

                //MessageBox.Show("Не удалось подключиться: Сервер не отвечает.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Не удалось подключиться: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void CheckConnection()
        {
            int step = 0;
            if (_socket is Client client)
            {
                while (step < 15)
                {
                    if (client.IsConnection)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            NavigationService.Navigate(new ShopPage(_socket));
                        });
                        break;
                    }

                    step++;
                    await Task.Delay(1000);
                }

                return;
            }
        }

       /* private void IpInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void PortInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            bool isIpValid = System.Net.IPAddress.TryParse(IpInput.Text.Trim(), out _);
            bool isPortValid = int.TryParse(PortInput.Text.Trim(), out int port) && port > 0 && port <= 65535;

            JoinLobbyButton.IsEnabled = isIpValid && isPortValid;

            // Опционально: визуальная индикация валидности
            IpInput.BorderBrush = isIpValid ?
                System.Windows.Media.Brushes.Green :
                System.Windows.Media.Brushes.Red;

            PortInput.BorderBrush = isPortValid ?
                System.Windows.Media.Brushes.Green :
                System.Windows.Media.Brushes.Red;
        }*/

    }
}
