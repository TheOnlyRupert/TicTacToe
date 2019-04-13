using System.Net.Sockets;

namespace TicTacToe.Source.Reference {
    public static class ReferenceValues {
        public static string CurrentModule { get; set; }
        public static NetworkStream NETWORK_STREAM { get; set; }
        public static TcpClient TCP_CLIENT { get; set; }
    }
}