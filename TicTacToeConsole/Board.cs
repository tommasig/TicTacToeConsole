using System;
//using System.Collections.Generic;
//using System.Text;

namespace TicTacToeConsole {

    class Board {
        public String[] layout;
        public const String CELL_X = "X";
        public const String CELL_O = "O";
        public const String CELL_E = " ";

        public Board() {
            this.Clear();
        }

        /// <summary>
        /// Init the board with empty cells.
        /// </summary>
        public void Clear() {
            this.layout = new String[9];

            for (int i = 0; i < 9; i++) {
                this.layout[i] = CELL_E;
            }
        }

        public override String ToString() {
            String strOut = "";
            String strCell = "";

            for (int i = 0; i < 9; i++) {
                if (this.layout[i] == CELL_E) {
                    strCell = "_";
                } else {
                    strCell = this.layout[i];
                }
                strOut += "[" + strCell + "]";
                if (i % 3 == 2) {
                    strOut += System.Environment.NewLine;
                }
            }

            return strOut;
        }

        /// <summary>
        /// Applies a mark in a specified position on the board
        /// </summary>
        /// <param name="pos">Position on the board, 1-based</param>
        /// <param name="mark">Mark to write in that position</param>
        /// <returns>True if position was empty and the mark could be written, false otherwise</returns>
        public bool PlayerMove(int pos, String mark) {
            bool done = false;

            //checking for boundaries
            if (pos < 1 || pos > 9) return done;

            //empty cell? then we write and it's done
            if (this.layout[pos - 1] == CELL_E) {
                this.layout[pos - 1] = mark;
                done = true;
            }

            return done;
        }

        /// <summary>
        /// Checks for victory of a specific mark 
        /// </summary>
        /// <returns>True if one row, one column or one diagonal are in a winning state, false otherwise</returns>
        public bool CheckForWin(String mark) {

            for (int i = 0; i < 3; i++) {

                //3 in a row
                //or
                //3 in a column
                if ((this.layout[i * 3] == this.layout[i * 3 + 1] &&
                     this.layout[i * 3 + 1] == this.layout[i * 3 + 2] &&
                     this.layout[i * 3] == mark)
                    ||
                    (this.layout[i] == this.layout[i + (3 * 1)] &&
                     this.layout[i + (3 * 1)] == this.layout[i + (3 * 2)] &&
                     this.layout[i] == mark)) {

                    return true;
                }

            }

            //3 in a diagonal
            if ((this.layout[0] == this.layout[4] && this.layout[4] == this.layout[8] && this.layout[0] == mark) ||
                (this.layout[2] == this.layout[4] && this.layout[4] == this.layout[6] && this.layout[2] == mark)) {

                return true;
            }

            return false;
        }

        public bool CheckForTie() {
            if (this.CheckForWin(Board.CELL_O) == false &&
                this.CheckForWin(Board.CELL_X) == false &&
                this.CountEmptyCells() == 0) {

                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Checks if the specified mark is going to win on the next move (2 in a row)
        /// </summary>
        /// <param name="mark">The mark to check</param>
        /// <returns>The first winning move found on the board (1-based)</returns>
        public int GoingToWin(String mark) {
            int noMove = 0; //in case the mark is not going to win, return an impossible move
            int row;        //row of the empty cell to check (0,1,2)
            int col;        //column of the empty cell to check (0,1,2)
            String line;    //string representation of a row, column or diagonal
            int countMarks; //how many marks of the speficied kind are there on the line

            //for every empty cell...
            for (int i = 0; i < 9; i++) {

                if (this.layout[i] == CELL_E) {
                    row = i / 3;
                    col = i % 3;

                    //... check its row
                    line = "";
                    for (int j = 0; j < 3; j++) {
                        line += this.layout[row * 3 + j];
                    }
                    countMarks = line.Length - line.Replace(mark, "").Length;
                    if (countMarks == 2) return i + 1;

                    //... check its column
                    line = "";
                    for (int j = 0; j < 3; j++) {
                        line += this.layout[col + 3 * j];
                    }
                    countMarks = line.Length - line.Replace(mark, "").Length;
                    if (countMarks == 2) return i + 1;

                    //... check its diagonals (if needed)
                    if (i == 0 || i == 8) {
                        //check NW - SE diagonal
                        line = "";
                        for (int j = 0; j < 3; j++) {
                            line += this.layout[j * 3 + j]; //0, 4, 8
                        }
                        countMarks = line.Length - line.Replace(mark, "").Length;
                        if (countMarks == 2) return i + 1;

                    } else if (i == 2 || i == 6) {
                        //check NE - SW diagonal
                        line = "";
                        for (int j = 0; j < 3; j++) {
                            line += this.layout[2 - j + 3 * j]; //2, 4, 6
                        }
                        countMarks = line.Length - line.Replace(mark, "").Length;
                        if (countMarks == 2) return i + 1;

                    } else if (i == 4) {
                        //check NW - SE
                        line = "";
                        for (int j = 0; j < 3; j++) {
                            line += this.layout[j * 3 + j];
                        }
                        countMarks = line.Length - line.Replace(mark, "").Length;
                        if (countMarks == 2) return i + 1;

                        //check NE - SW
                        line = "";
                        for (int j = 0; j < 3; j++) {
                            line += this.layout[2 - j + 3 * j];
                        }
                        countMarks = line.Length - line.Replace(mark, "").Length;
                        if (countMarks == 2) return i + 1;
                    }
                }
            }

            //not going to win
            return noMove;
        }

        /// <summary>
        /// How many empty cells on board?
        /// </summary>
        /// <returns>Integer number between 0 and 8</returns>
        public int CountEmptyCells() {
            int count = 0;

            for (int i = 0; i < 9; i++) {
                if (this.layout[i] == CELL_E) count++;
            }

            return count;
        }

        /// <summary>
        /// Creates a copy of the board.
        /// </summary>
        /// <returns>A new Board instance.</returns>
        public Board ObjectCopy() {
            Board newBoard = new Board();

            for (int i = 0; i < 9; i++) {
                newBoard.layout[i] = this.layout[i];
            }

            return newBoard;
        }

        /// <summary>
        /// Compares a board to the current one.
        /// </summary>
        /// <param name="b">Board instance to compare to the current one.</param>
        /// <returns>True if the layouts of the two boards are exactly the same, false otherwise</returns>
        public bool Equals(Board b) {
            for (int i = 0; i < 9; i++) {
                if (this.layout[i] != b.layout[i]) return false;
            }
            return true;
        }

    }

}
