using TicTacToe.Source.ViewModel.Base;

namespace TicTacToe.Source.ViewModel {
    public class GameVM : BaseViewModel {
        private string _currentPlayer;

        public GameVM() {
            CurrentPlayer = "Current Player:\nTheOnlyRupert";
        }

        public string CurrentPlayer {
            get => _currentPlayer;
            set {
                _currentPlayer = value;
                RaisePropertyChangedEvent("CurrentPlayer");
            }
        }
    }
}