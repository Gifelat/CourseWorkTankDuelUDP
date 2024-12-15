using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using UDPProtocol;

namespace Tank_Duel
{
    public partial class CreateLobbyPage : Page
    {
        private ISocket _server; 
        public CreateLobbyPage()
        {
            InitializeComponent();
            DisplayLocalIPAddress();
        }

        private void DisplayLocalIPAddress()
        {
            try
            {
                string localIP = Dns.GetHostAddresses(Dns.GetHostName())
                                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?
                                    .ToString();
                IpDisplay.Text = localIP ?? "Не удалось определить IP";
            }
            catch (Exception ex)
            {
                IpDisplay.Text = "Ошибка: " + ex.Message;
            }
        }

        private void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            string portInput = PortInput.Text.Trim();
            if (int.TryParse(portInput, out int port) && port > 0 && port <= 65535)
            {
                try
                {
                    string localIP = IpDisplay.Text;
                    _server = Server.GetInstance(localIP, port, freePort: false);

                   /* MessageBox.Show($"Лобби создано на IP {localIP} и порту {port}!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);*/

                    LogMessage($"Сервер запущен: {_server.GetInfo()}");

                    NavigationService.Navigate(new ShopPage(_server));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании лобби: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный порт (1–65535).",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_server != null)
            {
                _server.StopReceive();
                _server.ClearInstance();
                _server = null;

                MessageBox.Show("Лобби остановлено.",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LogMessage("Сервер остановлен.");
            }
            else
            {
                MessageBox.Show("Сервер не был запущен.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LogMessage(string message)
        {
            Logs.Text += $"{DateTime.Now}: {message}\n";
        }
    }
}
