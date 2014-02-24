using System;
//using System.Collections.Generic;
//using System.Text;

namespace TicTacToeConsole {

    /// <summary>
    /// The game tree, enumerating the possible ways a game board evolves.
    /// </summary>
    class GameTree {
        public GameTreeNode root;

        public GameTree(Board rootBoard, String startingMark) {
            this.root = new GameTreeNode(null, rootBoard, startingMark, 0);
        }

        public void Build() {
            this.root.Build();
        }
    }
    
    /// <summary>
    /// A node in the Tic Tac Toe game tree. It represents a state of the board during the game.
    /// </summary>
    class GameTreeNode {
        public GameTreeNode parent;     //the previous configuration of the board
        public Board status;            //the board now
        public String mark;             //the mark for the next move (indicates who's turn it is)
        public int inputMove;           //what was the move that led to this board configuration
        public bool win;                //true if it's a winning node
        public bool opponentWin;        //true if it's a winning node for the opponent
        public GameTreeNode[] children; //all the possible configurations for all the possible next moves, 
        //0 children if this node is a winning state, 9 children if it's the node with an empty board

        public GameTreeNode(GameTreeNode parent, Board status, String mark, int inputMove) {
            this.parent = parent;
            this.status = status.ObjectCopy();
            this.mark = mark;
            this.inputMove = inputMove;
            this.win = false;
            this.opponentWin = false;
            this.children = null;
        }

        /// <summary>
        /// Builds the subtree starting from this node.
        /// </summary>
        public void Build() {
            GameTreeNode newNode;
            int countEmptyCells = 0;

            // each empty cell leads to a new children
            countEmptyCells = this.status.CountEmptyCells();

            if (countEmptyCells > 0) {
                this.children = new GameTreeNode[countEmptyCells];

                for (int i = 0; i < 9; i++) {

                    if (this.status.layout[i] == Board.CELL_E) {
                        // create a new node, setting up next possible move
                        newNode = new GameTreeNode(this, this.status, this.NextMark(), i + 1);
                        newNode.status.PlayerMove(i + 1, this.mark);
                        newNode.win = newNode.status.CheckForWin(Board.CELL_O); //|| newNode.status.CheckForTie();
                        newNode.opponentWin = newNode.status.CheckForWin(Board.CELL_X);

                        // add the new node to the first empty space in the children array
                        for (int j = 0; j < countEmptyCells; j++) {
                            if (this.children[j] == null) {
                                this.children[j] = newNode;
                                break;
                            }
                        }

                        // go deeper only if the game is not over yet
                        if (!newNode.win && !newNode.opponentWin) {
                            newNode.Build();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Switches from one mark to the other.
        /// </summary>
        /// <returns>The opposite mark of what is currently set on the node.</returns>
        private String NextMark() {
            if (this.mark == Board.CELL_O)
                return Board.CELL_X;
            else
                return Board.CELL_O;
        }

        /// <summary>
        /// Determines if the node is a leaf in the tree.
        /// </summary>
        /// <returns>True if there are no empty cells on the board (useful during building) or there are no children (useful during traversing).</returns>
        public bool IsLeaf() {
            return this.status.CountEmptyCells() == 0 || this.children == null || this.children.Length == 0;
        }

        /// <summary>
        /// Evaluates the node to determine if it's leading to a win or loss, using a simplified MiniMax algorithm.
        /// </summary>
        /// <returns>True if there's a chance for winning, false otherwise.</returns>
        public bool Eval() {

            if (this.IsLeaf()) {
                return this.win || !this.opponentWin; //consider a tie as positive, in case winning is not an option
            } else {
                //this node belongs to skynet
                if (this.mark == Board.CELL_O) {
                    foreach (GameTreeNode child in Utils.Shuffle<GameTreeNode>(this.children)) { //randomness
                        //foreach (GameTreeNode child in this.children) { //deterministic
                        if (child.Eval() == true) return true;
                    }
                    return false;
                } else {
                    foreach (GameTreeNode child in Utils.Shuffle<GameTreeNode>(this.children)) { //randomness
                        //foreach (GameTreeNode child in this.children) { //deterministic
                        if (child.Eval() == false) return false;
                    }
                    return true;
                }

            }

        }

        /// <summary>
        /// Gets a random child from the ones available.
        /// </summary>
        /// <returns></returns>
        public GameTreeNode RandomChild() {
            Random rnd = new Random();

            if (this.children != null) {
                return this.children[rnd.Next(0, this.children.Length - 1)];
            } else {
                return null;
            }

        }

    }


}
