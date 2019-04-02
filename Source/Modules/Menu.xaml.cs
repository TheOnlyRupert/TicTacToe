using System.Windows.Controls;
using TicTacToe.Source.ViewModel;

namespace TicTacToe.Source.Modules {
    public partial class Menu : Page {
        public Menu() {
            InitializeComponent();
            DataContext = new MenuVM();
        }
    }
}