using System;
//using System.Collections.Generic;
//using System.Text;

namespace TicTacToeConsole {

    /// <summary>
    /// Base class for all players, humans and machines.
    /// </summary>
    class Player {
        public String name;
        public String mark;

        public Player(String mark) {
            this.mark = mark;
        }

        public virtual GameTreeNode NextMove(Board board, GameTreeNode currentNode) { return null; }
    }

    /// <summary>
    /// AI player. Impossible to beat.
    /// </summary>
    class Skynet : Player {

        public Skynet(String mark)
            : base(mark) {
            this.name = "Skynet";
        }

        public override GameTreeNode NextMove(Board board, GameTreeNode currentNode) {
            //int winningMove;

            //priorities:
            //1. crush humans
            //2. prevent humans victory

            //1. just win the game
            //winningMove = board.GoingToWin(Board.CELL_O);
            //if (winningMove != 0) {
            //    Console.WriteLine(this.name + ": You have been terminated.");
            //    //do the move
            //    board.PlayerMove(winningMove, this.mark);
            //    //and look for the node to return
            //    foreach (GameTreeNode child in currentNode.children) {
            //        if (child.status.Equals(board)) {
            //            return child;
            //        }
            //    }
            //}

            ////2. counter the opponents winning chance
            //winningMove = board.GoingToWin(Board.CELL_X);
            //if (winningMove != 0) {
            //    Console.WriteLine(this.name + ": You will pay for your insolence!");
            //    //do the move
            //    board.PlayerMove(winningMove, this.mark);
            //    //and look for the node to return
            //    foreach (GameTreeNode child in currentNode.children) {
            //        if (child.status.Equals(board)) {
            //            return child;
            //        }
            //    }
            //}

            //3. play a move toward the victory
            foreach (GameTreeNode child in Utils.Shuffle<GameTreeNode>(currentNode.children)) {
                //foreach (GameTreeNode child in currentNode.children) {
                if (child.Eval() == true) {
                    Console.WriteLine(this.name + ": I will crush you all!");
                    board.PlayerMove(child.inputMove, this.mark);
                    return child;
                }
            }

            //4. there's no winning chance: we take a random available move
            Console.WriteLine(this.name + ": The board is a lie!");

            //board.Move(currentNode.children[0].inputMove, this.mark);
            //return currentNode.children[0];

            //random selection of next node
            GameTreeNode nextNode = currentNode.RandomChild();
            board.PlayerMove(nextNode.inputMove, this.mark);
            return nextNode;
        }

    }

    /// <summary>
    /// The player.
    /// </summary>
    class Human : Player {

        public Human(String mark)
            : base(mark) {
            this.name = "Human";
        }

        public override GameTreeNode NextMove(Board board, GameTreeNode currentNode) {
            int movePos = 0;

            //keep asking for a reasonable move to input:
            //1 2 3
            //4 5 6
            //7 8 9
            while (!board.PlayerMove(movePos, this.mark)) {
                Console.WriteLine("What is your next move? [1..9]");
                ConsoleKeyInfo kinfo = Console.ReadKey(false);
                movePos = (int)(kinfo.KeyChar);
                movePos = movePos - 48; //subtract the char code for '0'
                Console.WriteLine();
            }

            //look for the child node corresponding to the chosen move and return it
            foreach (GameTreeNode child in currentNode.children) {
                if (child.status.Equals(board)) {
                    return child;
                }
            }

            Console.WriteLine("[Error] Something's wrong with the game tree.");
            return null;
        }
    }

}
