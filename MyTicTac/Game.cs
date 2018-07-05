using System;
using System.Collections.Generic;

namespace MyTicTac
{
    class Game
    {
        enum SpaceHolder
        {
            O,
            X,
            None
        }

        enum Player
        {
            player1,
            computer1,
            tie
        }

        private SpaceHolder[] board;

        private Player turn;

        private bool isGameEnd;

        private Player winner;

        public Game()
        {
            board = new SpaceHolder[9];
        }

        public void Run()
        {
            Initialize();
            Draw();
            while (!isGameEnd)
            {
                Update();
                Draw();
            }
            FinalizeGame();
        }

        private void Initialize()
        {
            #region 보드를 초기화합니다.
            for (int i = 0; i < 9; i++)
            {
                board[i] = SpaceHolder.None;
            }
            #endregion

            #region 시작 플레이어를 정합니다.
            Random rnd = new Random();
            int t = rnd.Next(2);
            if (t == 1)
            {
                turn = Player.player1;
                Console.WriteLine("당신의 선입니다!");
            }
            else
            {
                turn = Player.computer1;
                Console.WriteLine("컴퓨터의 선입니다!");
            }
            #endregion

            #region 게임 루프 탈출용 스위치를 설정합니다.
            isGameEnd = false;
            #endregion
        }

        private void Update()
        {
            switch (turn)
            {
                case Player.player1:
                    #region 착수 가능한 위치를 수집합니다.
                    List<int> movableSpace = new List<int>();
                    for (int i = 0; i < 9; i++)
                    {
                        if (board[i] == SpaceHolder.None)
                        {
                            movableSpace.Add(i);
                        }
                    }
                    #endregion

                    #region 올바른 착수 위치를 입력할 때까지 착수 가능한 위치를 표시하고 플레이어의 입력을 기다립니다.
                    int read = -1;
                    do
                    {

                        Console.Write("착수할 위치를 고르세요 - 착수 가능 위치 = ");
                        foreach (int i in movableSpace)
                        {
                            Console.Write("{0} ", i);
                        }
                        Console.Write(">> ");
                        read = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Read: {0}", read);

                    } while (!movableSpace.Contains(read));
                    #endregion

                    #region 입력된 위치에 말을 둡니다.
                    board[read] = SpaceHolder.O;
                    #endregion

                    #region 게임이 끝났는지 확인하고 아니라면 컴퓨터에게 턴을 넘깁니다.
                    if (IsWin(board, SpaceHolder.O))
                    {
                        isGameEnd = true;
                        winner = Player.player1;
                    }
                    else if (IsTie(board))
                    {
                        isGameEnd = true;
                        winner = Player.tie;
                    }
                    else
                    {
                        turn = Player.computer1;
                    }
                    #endregion
                    break;
                case Player.computer1:
                    #region 착수 가능한 위치를 수집합니다.
                    movableSpace = new List<int>();
                    for (int i = 0; i < 9; i++)
                    {
                        if (board[i] == SpaceHolder.None)
                        {
                            movableSpace.Add(i);
                        }
                    }
                    #endregion

                    int next = AiLogic(movableSpace);

                    #region 계산된 위치에 말을 둡니다.
                    board[next] = SpaceHolder.X;
                    #endregion

                    #region 게임이 끝났는지 확인하고 아니라면 플레이어에게 턴을 넘깁니다.
                    if (IsWin(board, SpaceHolder.X))
                    {
                        isGameEnd = true;
                        winner = Player.computer1;
                    }
                    else if (IsTie(board))
                    {
                        isGameEnd = true;
                        winner = Player.tie;
                    }
                    else
                    {
                        turn = Player.player1;
                    }
                    #endregion
                    break;
                default:
                    throw new ArgumentException("그런 플레이어는 없습니다.");
            }
        }

        private void Draw()
        {
            // 현재의 보드 상태를 그립니다.
            char[] boardString = new char[9];
            for (int i = 0; i < 9; i++)
            {
                switch (board[i])
                {
                    case SpaceHolder.None:
                        boardString[i] = Convert.ToString(i)[0];
                        break;
                    case SpaceHolder.O:
                        boardString[i] = 'O';
                        break;
                    case SpaceHolder.X:
                        boardString[i] = 'X';
                        break;
                    default:
                        throw new ArgumentException("보드에 잘못된 기호가 있습니다.");
                }
            }
            Console.WriteLine();
            Console.WriteLine("   |   |   ");
            Console.WriteLine(" {0} | {1} | {2} ", boardString[6], boardString[7], boardString[8]);
            Console.WriteLine("   |   |   ");
            Console.WriteLine("---|---|---");
            Console.WriteLine("   |   |   ");
            Console.WriteLine(" {0} | {1} | {2} ", boardString[3], boardString[4], boardString[5]);
            Console.WriteLine("   |   |   ");
            Console.WriteLine("---|---|---");
            Console.WriteLine("   |   |   ");
            Console.WriteLine(" {0} | {1} | {2} ", boardString[0], boardString[1], boardString[2]);
            Console.WriteLine("   |   |   ");
            Console.WriteLine();
        }

        private void FinalizeGame()
        {
            if (winner == Player.player1)
            {
                Console.WriteLine("당신의 승리입니다!!!");
            }
            else if (winner == Player.computer1)
            {
                Console.WriteLine("캄퓨터의 승리입니다...");
            }
            else if (winner == Player.tie)
            {
                Console.WriteLine("아쉽게도 비겼습니다..!");
            }
        }

        private bool IsWin(SpaceHolder[] board, SpaceHolder mark)
        {
            return ((board[0] == mark && board[1] == mark && board[2] == mark) ||
                    (board[3] == mark && board[4] == mark && board[5] == mark) ||
                    (board[6] == mark && board[7] == mark && board[8] == mark) ||
                    (board[0] == mark && board[3] == mark && board[6] == mark) ||
                    (board[1] == mark && board[4] == mark && board[7] == mark) ||
                    (board[2] == mark && board[5] == mark && board[8] == mark) ||
                    (board[0] == mark && board[4] == mark && board[8] == mark) ||
                    (board[2] == mark && board[4] == mark && board[6] == mark));
        }

        private bool IsTie(SpaceHolder[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == SpaceHolder.None)
                {
                    return false; // 하나라도 빈 곳이 있으면 비기지 않았습니다.
                }
            }
            return true;
        }

        private int AiLogic(List<int> movableSpace)
        {
            SpaceHolder[] boardCopy = new SpaceHolder[9];

            // 이길 수 있는 곳이 있다면 거기에 둡니다.
            foreach (int i in movableSpace)
            {
                boardCopy = CopyBoard(board);
                boardCopy[i] = SpaceHolder.X;
                if (IsWin(boardCopy, SpaceHolder.X))
                {
                    return i;
                }
            }

            // 상대방이 다음에 이길 수 있는 곳이 있다면 거기에 두어서 막습니다.
            foreach (int i in movableSpace)
            {
                boardCopy = CopyBoard(board);
                boardCopy[i] = SpaceHolder.O;
                if (IsWin(boardCopy, SpaceHolder.O))
                {
                    return i;
                }
            }

            // 우선순위에 따라서 둡니다. 중앙 -> 변 -> 코너
            if (board[4] == SpaceHolder.None)
            {
                return 4;
            }

            int[] sides = new int[] { 1, 3, 5, 7 };
            foreach (int i in sides)
            {
                if (board[i] == SpaceHolder.None)
                {
                    return i;
                }
            }

            int[] corners = new int[] { 0, 2, 6, 8 };
            foreach (int i in corners)
            {
                if (board[i] == SpaceHolder.None)
                {
                    return i;
                }
            }

            // 그 외의 경우는 없습니다.
            throw new ArgumentException("예상치못한 경로에 들어갔습니다.");
        }

        private SpaceHolder[] CopyBoard(SpaceHolder[] board)
        {
            SpaceHolder[] boardCopy = new SpaceHolder[9];
            for (int i = 0; i < 9; i++)
            {
                boardCopy[i] = board[i];
            }
            return boardCopy;
        }
    }
}
