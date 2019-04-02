using TicTacToe.Source.ViewModel;

namespace TicTacToe {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainWindowVM();
        }
    }
}