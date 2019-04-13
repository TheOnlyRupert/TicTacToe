# 2 Player Online TicTacToe Game

#### Features
1. Easy server start right from within the client.
2. Currently does not have any AI for winning the game.
3. Currently does not have any AI for singleplayer.

#### Downloads
Download the latest release from [release page](https://github.com/TheOnlyRupert/TicTacToe/releases).

#### Requirements
Microsoft [.NET Framework 4.6.2](https://www.microsoft.com/en-US/download/details.aspx?id=53344 "Microsoft's download page") or higher and Microsoft [Visual C++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=53840 "Microsoft's download page").

#### Technical
1. Uses TCP/IP protocol to establish 2 connections on 2 different threads.
2. "Gameboard State" is polled from the server every 50 miliseconds.
3. Uses port 10000 by default.

#### To Do List
1. High score leader-boards.
2. Singleplayer functionality with AI.
3. Be able to actually win the game.