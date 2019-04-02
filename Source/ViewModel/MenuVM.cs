using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TicTacToe.Source.Reference;
using TicTacToe.Source.ViewModel.Base;

namespace TicTacToe.Source.ViewModel {
    public class MenuVM : BaseViewModel {
        private readonly CrossViewMessenger _crossViewMessenger;

        private string _inputIp,
            _publicIp,
            _localIp,
            _clientName,
            _displayConnection,
            _inputPort;

        private bool isServerRunning, isTryingToConnect;

        public MenuVM() {
            ClientName = Dns.GetHostName() + '_' + Environment.UserName;
            LocalIp = GetLocalIPAddress();
            PublicIp = new WebClient().DownloadString("http://ipv4.icanhazip.com/");
            PublicIp = PublicIp.Remove(PublicIp.Length - 1);

            DisplayConnection = "Not Connected";
            InputPort = "9999";

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

        public ICommand ButtonCommand => new DelegateCommand(ButtonCommandLogic, true);

        private static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            throw new Exception("[ERROR] No network adapters with an IPv4 address in the system");
        }

        private void ButtonCommandLogic(object obj) {
            switch (obj) {
                case "client":
                    if (!isTryingToConnect) {
                        isTryingToConnect = true;
                        DisplayConnection = "Connecting...";
                        Client();

                        ReferenceValues.CurrentModule = "Source/Modules/Game.xaml";
                        _crossViewMessenger.PushMessage("SwitchCurrentModule", null);
                    } else {
                        Console.WriteLine("[CHILL] Client is trying to connect");
                    }

                    break;

                case "server":
                    if (!isServerRunning)
                        Server();
                    else
                        Console.WriteLine("[CHILL] Server is already running");

                    break;
            }
        }

        private void Client() {
            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                try {
                    var client = new TcpClient(InputIp, int.Parse(InputPort));
                    var sendData = Encoding.ASCII.GetBytes("test;");
                    var stream = client.GetStream();

                    stream.Write(sendData, 0, sendData.Length);
                    DisplayConnection = "Connected";

                    stream.Close();
                    client.Close();
                } catch (Exception e) {
                    DisplayConnection = "Not Connected";
                    isTryingToConnect = false;

                    if (e is SocketException)
                        Console.WriteLine(@"[ERROR] Connection refused on port " + InputPort +
                                          "\n    Server may not be running or port not forwarded");
                    else if (e is ArgumentNullException || e is FormatException)
                        Console.WriteLine("[ERROR] IP Address must be correct format");
                    else
                        Console.WriteLine(e);
                }
            }).Start();
        }

        private void Server() {
            /* If user is starting server, they're probably going to want to connect to it */
            InputIp = GetLocalIPAddress();

            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;

                try {
                    var ipHost = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
                    var server = new TcpListener(ipHost, int.Parse(InputPort));

                    try {
                        server.Start();
                        isServerRunning = true;
                        Console.WriteLine(@"[INFO] Server did start");
                    } catch (Exception) {
                        Console.WriteLine(@"[ERROR] Server cannot start");
                        return;
                    }

                    while (true) {
                        var client = server.AcceptTcpClient();
                        var receivingBuffer = new byte[64];
                        var stream = client.GetStream();

                        stream.Read(receivingBuffer, 0, receivingBuffer.Length);
                        var message = new StringBuilder();
                        foreach (var b in receivingBuffer) {
                            if (b.Equals(59)) break;

                            message.Append(Convert.ToChar(b).ToString());
                        }

                        Console.WriteLine(message);
                    }
                } catch (Exception e) {
                    if (e is FormatException)
                        Console.WriteLine("[ERROR] Port must be in correct format!");
                    else
                        Console.WriteLine(e);
                }
            }).Start();
        }
    }
}