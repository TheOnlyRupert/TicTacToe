using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using TicTacToe.Source.ViewModel.Base;

namespace TicTacToe.Source.ViewModel
{
    public class MainWindowVM : BaseViewModel
    {
        private string _ipAddress, _connectionStatus, _publicIP, _portIP;

        public MainWindowVM()
        {
            PublicIP = "Public IP: " + new WebClient().DownloadString("http://ipv4.icanhazip.com/");
            PortIP = "Connection Port: " + "9999";
        }

        public ICommand ButtonCommand => new DelegateCommand(ButtonCommandLogic, true);

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                RaisePropertyChangedEvent("IpAddress");
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                RaisePropertyChangedEvent("ConnectionStatus");
            }
        }
        
        public string PublicIP
        {
            get => _publicIP;
            set
            {
                _publicIP = value;
                RaisePropertyChangedEvent("PublicIP");
            }
        }
        
        public string PortIP
        {
            get => _portIP;
            set
            {
                _portIP = value;
                RaisePropertyChangedEvent("PortIP");
            }
        }

        private void ButtonCommandLogic(object obj)
        {
            try
            {
                switch (obj)
                {
                    case "client":
                        var client = new TcpClient(IpAddress, 9999);

                        byte[] sendData = Encoding.ASCII.GetBytes(obj.ToString() + ';');

                        var stream = client.GetStream();
                        stream.Write(sendData, 0, sendData.Length);
                        stream.Close();

                        client.Close();
                        break;

                    case "server":
                        break;
                }
            }
            catch (Exception e)
            {
                //continue...
            }
        }
    }
}