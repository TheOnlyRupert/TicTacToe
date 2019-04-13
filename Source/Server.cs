using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TicTacToe.Source.Reference;

namespace TicTacToe.Source {
    public static class Server {
        public static void Start() {
            TcpListener server = new TcpListener(IPAddress.Any, 10000);
            ReferenceValues.gameBoard = new[] {0, 0, 0, 0, 0, 0, 0, 0, 0};
            ReferenceValues.currentPlayer = 1;

            server.Start();
            Console.WriteLine("Server Started");

            int counter = 0;
            while (true) {
                counter += 1;
                TcpClient clientSocket = server.AcceptTcpClient();
                Console.WriteLine("CLIENT_" + counter + " Connected");
                HandleClient client = new HandleClient();
                client.startClient(clientSocket, counter);
            }
        }
    }

    public class HandleClient {
        private TcpClient clientSocket;

        public void startClient(TcpClient inClientSocket, int clientID) {
            if (clientID > 2) {
                throw new Exception();
            }

            clientSocket = inClientSocket;
            Thread thread = new Thread(clientCommandThread);
            thread.Start(clientID);
        }

        private void clientCommandThread(object clientID) {
            byte[] bytes = new byte[256];
            int i;

            NetworkStream networkStream = clientSocket.GetStream();

            while ((i = networkStream.Read(bytes, 0, bytes.Length)) != 0) {
                string data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("[CLIENT_" + clientID + "] Received: " + data);

                string gameBoard = GetGameBoardData(data, clientID);
                byte[] msg = Encoding.ASCII.GetBytes(gameBoard);

                // Send game board data back to client
                networkStream.Write(msg, 0, msg.Length);
                Console.WriteLine("[CLIENT_" + clientID + "] Sent: " + gameBoard);
            }
        }

        private static string GetGameBoardData(string tableID, object clientID) {
            int location = 0, client = 0;
            string returnable = "";

            try {
                location = int.Parse(tableID);
                client = int.Parse(clientID.ToString());
            } catch (Exception) {
                // nothing yet
            }

            if (ReferenceValues.currentPlayer == client) {
                if (ReferenceValues.gameBoard[location - 1] == 0) {
                    ReferenceValues.gameBoard[location - 1] = client;

                    // Switch current player
                    ReferenceValues.currentPlayer = ReferenceValues.currentPlayer == 1 ? 2 : 1;
                }
            }

            bool newGame = true;
            foreach (int i in ReferenceValues.gameBoard) {
                returnable += i;
                if (i == 0) {
                    newGame = false;
                }
            }

            // If game board full, start new game
            return newGame ? "000000000" : returnable;
        }
    }
}