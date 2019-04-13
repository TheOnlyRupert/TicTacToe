using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using TicTacToe.Source.Reference;
using TicTacToe.Source.ViewModel.Base;

namespace TicTacToe.Source.ViewModel {
    public class MenuVM : BaseViewModel {
        private readonly CrossViewMessenger _crossViewMessenger;

        private string _inputIp, _publicIp, _localIp, _clientName, _displayConnection, _inputPort, _username;

        public MenuVM() {
            ClientName = Dns.GetHostName() + '_' + Environment.UserName;
            Username = "";
            LocalIp = GetLocalIPAddress();
            PublicIp = new WebClient().DownloadString("http://ipv4.icanhazip.com/");
            PublicIp = PublicIp.Remove(PublicIp.Length - 1);

            InputPort = "10000";
            InputIp = HIDDEN.PublicIP;

            _crossViewMessenger = CrossViewMessenger.Instance;
        }

        public string InputIp {
            get => _inputIp;
            set {
                _inputIp = value;
                RaisePropertyChangedEvent("InputIp");
            }
        }

        public string InputPort {
            get => _inputPort;
            set {
                _inputPort = value;
                RaisePropertyChangedEvent("InputPort");
            }
        }

        public string DisplayConnection {
            get => _displayConnection;
            set {
                _displayConnection = value;
                RaisePropertyChangedEvent("DisplayConnection");
            }
        }

        public string ClientName {
            get => _clientName;
            set {
                _clientName = value;
                RaisePropertyChangedEvent("ClientName");
            }
        }


        public string LocalIp {
            get => _localIp;
            set {
                _localIp = value;
                RaisePropertyChangedEvent("LocalIp");
            }
        }

        public string PublicIp {
            get => _publicIp;
            set {
                _publicIp = value;
                RaisePropertyChangedEvent("PublicIp");
            }
        }

        public string Username {
            get => _username;
            set {
                _username = value;
                RaisePropertyChangedEvent("Username");
            }
        }

        public ICommand ButtonCommand => new DelegateCommand(ButtonCommandLogic, true);

        private static string GetLocalIPAddress() {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }

            return null;
        }

        private void ButtonCommandLogic(object obj) {
            switch (obj) {
            case "client":
                if (Username.Length > 2) {
                    try {
                        ReferenceValues.TCP_CLIENT = new TcpClient(PublicIp, int.Parse(InputPort));
                        ReferenceValues.NETWORK_STREAM = ReferenceValues.TCP_CLIENT.GetStream();

                        ReferenceValues.CurrentModule = "Source/Modules/Game.xaml";
                        _crossViewMessenger.PushMessage("SwitchCurrentModule", null);
                    } catch (Exception) {
                        //nothing
                    }
                }

                break;
            }
        }
    }
}