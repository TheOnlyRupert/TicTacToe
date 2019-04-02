using System.Windows.Controls;
using TicTacToe.Source.ViewModel;

namespace TicTacToe.Source.Modules {
    public partial class Game : Page {
        public Game() {
            InitializeComponent();
            DataContext = new GameVM();
        }
    }
}