using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpPhone.Something
{
    class Controller
    {
        private NetWork netWork;
        private MainWindow mainWindow;

        public Controller(MainWindow window)
        {
            mainWindow = window;
            netWork = new NetWork(this);
            app_start();
        }

        public void app_start()
        {
            netWork.server_start();
            netWork.send_broadcast(Command.CLIENT_LADING);
            set_local_ip();

        }

        public void set_local_ip()
        {
            Action<string> action = new Action<string>(mainWindow.set_local_ip);
            mainWindow.Dispatcher.BeginInvoke(action, netWork.get_local_ip());
        }

        public void clients_refresh()
        {
            netWork.send_broadcast(Command.FIND_CLIENTS);
        }

        public void call_client(string client_ip)
        {
            netWork.send_message(Command.CALL_CLIENT, client_ip);
        }

        public void answer_call(string client_ip)
        {
            netWork.send_message(Command.ANSWER_CLIENT, client_ip);
        }

        public void message_process(string message, string client_ip)
        {
            if (message.Equals(Command.CLIENT_LADING))
            {
                Action<string> action = new Action<string>(mainWindow.add_client);
                mainWindow.Dispatcher.BeginInvoke(action, client_ip);
                netWork.send_message(Command.CLIENT_ONLINE, client_ip);
            }
            else if (message.Equals(Command.FIND_CLIENTS))
            {
                netWork.send_message(Command.CLIENT_ONLINE, client_ip);
            }
            else if (message.Equals(Command.CLIENT_ONLINE))
            {
                Action<string> action = new Action<string>(mainWindow.add_client);
                mainWindow.Dispatcher.BeginInvoke(action, client_ip);
            }
            else if (message.Equals(Command.CALL_CLIENT))
            {
                Action<string> action = new Action<string>(mainWindow.be_called);
                mainWindow.Dispatcher.BeginInvoke(action, client_ip);
            }
            else if (message.Equals(Command.ANSWER_CLIENT))
            {
                Action<string> action = new Action<string>(mainWindow.answer_call);
                mainWindow.Dispatcher.BeginInvoke(action, client_ip);
            }
        }
        
        public void close()
        {
            netWork.close();
        }
    }
}
