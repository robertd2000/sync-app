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
using Client.ServiceReference1;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IService1Callback
    {
        bool isConnected = false;
        Service1Client client;
        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectUser()
        {
            if (!isConnected)
            {
                client = new Service1Client(new System.ServiceModel.InstanceContext(this));

                ID = client.Connect(tbUserName.Text);

                tbUserName.IsEnabled = false;
                bConnect.Content = "Отключиться";
                isConnected = true;
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Msg()
        {
            Random ran = new Random();
            string msg;
            int id = ID;
            msg = ran.Next(1, 100).ToString();

            string formattedMsg = " с id " + ID.ToString() + " " + msg;

            popup.IsOpen = Convert.ToInt32(msg) > 50 && Convert.ToInt32(msg) < 70 && id == ID;

            if (client != null)
            {
                client.SendMsg(formattedMsg, ID);
            }

        }

        private void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.IsEnabled = true;
                bConnect.Content = "Подключиться";
                isConnected = false;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
            int i = 0;
            while (isConnected)
            {
                await Task.Delay(10000);
                i++;
                Msg();
            }
        }

        public void MessageCallback(string msg)
        {
            _ = lbMessages.Items.Add(msg);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void lbMessages_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.SendMsg("Режим A активирован", ID);
            }

            popup.IsOpen = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.SendMsg("Отказ оператора в активации режима А", ID);
            }

            popup.IsOpen = false;
        }
    }
}
