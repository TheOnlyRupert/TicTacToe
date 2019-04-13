using System;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TicTacToe.Source.Reference;
using TicTacToe.Source.ViewModel.Base;

namespace TicTacToe.Source.ViewModel {
    public class GameVM : BaseViewModel {
        private readonly string[] gameBoard = {"0", "0", "0", "0", "0", "0", "0", "0", "0"};

        private string _currentPlayer, _button1State, _button2State, _button3State, _button4State, _button5State,
            _button6State, _button7State, _button8State, _button9State;

        public GameVM() {
            CurrentPlayer = "Current Player:\nTheOnlyRupert";
            Button1State = Button2State = Button3State = Button4State =
                Button5State = Button6State = Button7State = Button8State = Button9State = "";

            Thread thread = new Thread(serverListenThread);
            thread.Start();
        }

        public string CurrentPlayer {
            get => _currentPlayer;
            set {
                _currentPlayer = value;
                RaisePropertyChangedEvent("CurrentPlayer");
            }
        }

        public string Button1State {
            get => _button1State;
            set {
                _button1State = value;
                RaisePropertyChangedEvent("Button1State");
            }
        }

        public string Button2State {
            get => _button2State;
            set {
                _button2State = value;
                RaisePropertyChangedEvent("Button2State");
            }
        }

        public string Button3State {
            get => _button3State;
            set {
                _button3State = value;
                RaisePropertyChangedEvent("Button3State");
            }
        }

        public string Button4State {
            get => _button4State;
            set {
                _button4State = value;
                RaisePropertyChangedEvent("Button4State");
            }
        }

        public string Button5State {
            get => _button5State;
            set {
                _button5State = value;
                RaisePropertyChangedEvent("Button5State");
            }
        }

        public string Button6State {
            get => _button6State;
            set {
                _button6State = value;
                RaisePropertyChangedEvent("Button6State");
            }
        }

        public string Button7State {
            get => _button7State;
            set {
                _button7State = value;
                RaisePropertyChangedEvent("Button7State");
            }
        }

        public string Button8State {
            get => _button8State;
            set {
                _button8State = value;
                RaisePropertyChangedEvent("Button8State");
            }
        }

        public string Button9State {
            get => _button9State;
            set {
                _button9State = value;
                RaisePropertyChangedEvent("Button9State");
            }
        }

        public ICommand ButtonCommand => new DelegateCommand(ButtonCommandLogic, true);

        private void serverListenThread() {
            while (true) {
                byte[] inStream = new byte[256];
                ReferenceValues.NETWORK_STREAM.Write(inStream, 0, inStream.Length);
                ReferenceValues.NETWORK_STREAM.Read(inStream, 0, inStream.Length);
                string data = Encoding.ASCII.GetString(inStream);
                Console.WriteLine("Data from Server: " + data);

                for (int i = 0; i < gameBoard.Length; i++) {
                    gameBoard[i] = data[i].ToString();
                }

                Button1State = gameBoard[0];
                Button2State = gameBoard[1];
                Button3State = gameBoard[2];
                Button4State = gameBoard[3];
                Button5State = gameBoard[4];
                Button6State = gameBoard[5];
                Button7State = gameBoard[6];
                Button8State = gameBoard[7];
                Button9State = gameBoard[8];

                Thread.Sleep(50);
            }
        }

        private void ButtonCommandLogic(object obj) {
            ReferenceValues.NETWORK_STREAM.Write(
                Encoding.ASCII.GetBytes(obj.ToString()), 0, Encoding.ASCII.GetBytes(obj.ToString()).Length
            );
        }
    }
}