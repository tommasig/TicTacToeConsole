using System;

namespace TicTacToeConsole {

    class Utils {

        public static Random rnd = new Random();

        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        /// <remarks></remarks>
        public static T[] Shuffle<T>(T[] array) {
            Random random = rnd;
            for (int i = array.Length; i > 1; i--) {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
            return array;
        }
    }

    class Program {

        public static Board board;
        public static Player[] turns;
        public static GameTree gameTree;

        static void Main(string[] args) {
            int moves = 0;
            Player turn;
            GameTreeNode currentNode;
            
            //setting up the board
            board = new Board();

            // and the players with their turns
            turns = new Player[2];
            turns[0] = new Skynet(Board.CELL_O);
            turns[1] = new Human(Board.CELL_X);

            // we want the first turn to be assigned randomly
            Random rnd = new Random();
            moves = rnd.Next(0, 10);

            // let's decide who's to go first
            turn = turns[moves % 2];

            // create the game tree to drive Skynet's strategy
            gameTree = new GameTree(board, turn.mark);
            gameTree.Build();
            currentNode = gameTree.root;

            while (!board.CheckForWin(turn.mark) && !board.CheckForTie()) {
                //whose turn is this?
                turn = turns[moves % 2];

                //print the board status
                Console.WriteLine(board.ToString());

                //your move
                currentNode = turn.NextMove(board, currentNode);

                moves++;
            }

            Console.WriteLine(board.ToString());
            if (board.CheckForTie()) {
                //it's a tie!
                Console.WriteLine("It's a tie.");
            } else {
                //somebody won!
                Console.WriteLine(turn.name + " won!");
            }

            Console.Write("Press enter to exit...");
            Console.ReadLine();
        }

    }
}
