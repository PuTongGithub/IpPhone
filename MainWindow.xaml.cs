using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IpPhone.Something;

namespace IpPhone
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller;
        private List<string> clients = new List<string>();
        private string client_ip;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller(this);
        }

        public void set_local_ip(string ip)
        {
            ipBlock.Text += ' ' + ip;
        }

        public void add_client(string ip)
        {
            clients.Add(ip);
            myListBox.Items.Add(ip);
        }

        public void be_called(string call_ip)
        {
            client_ip = call_ip;
            statusBlock.Text = Command.BE_CALLED;
            myGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD3, 0xD3));
            ipBlock.Text = "目标ip地址 : " + client_ip;
            beCalledButton.Visibility = Visibility.Visible;
            callButton.Visibility = Visibility.Hidden;
        }

        public void answer_call(string call_ip)
        {
            client_ip = call_ip;
            statusBlock.Text = Command.CONNECTED;
            myGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC1, 0xD6, 0xE9));
            ipBlock.Text = "目标ip地址 : " + client_ip;
            beCalledButton.Visibility = Visibility.Visible;
            callButton.Visibility = Visibility.Hidden;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            myListBox.Items.Clear();
            controller.clients_refresh();
        }

        private void callButton_Click(object sender, RoutedEventArgs e)
        {
            if(client_ip != null && statusBlock.Text == Command.NOT_CONNECTED)
            {
                statusBlock.Text = Command.CALLING;
                myGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0xFF, 0xAF));
                controller.call_client(client_ip);
            }
        }

        private void myListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            client_ip = (string)myListBox.SelectedItem;
            statusBlock.Text = Command.NOT_CONNECTED;
            myGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            ipBlock.Text = "目标ip地址 : " + client_ip;
            beCalledButton.Visibility = Visibility.Hidden;
            callButton.Visibility = Visibility.Visible;
        }

        private void beCalledButton_Click(object sender, RoutedEventArgs e)
        {
            if (statusBlock.Text == Command.BE_CALLED)
            {
                statusBlock.Text = Command.CONNECTED;
                myGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC1, 0xD6, 0xE9));
                controller.answer_call(client_ip);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.close();
        }
    }
}
