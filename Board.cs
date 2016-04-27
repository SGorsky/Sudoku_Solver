using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class Board
    {
        const byte BOARD_SIZE = 9;
        public bool invalidBoard;
        public Num[,] board = new Num[BOARD_SIZE, BOARD_SIZE];
        byte emptyCount;

        public Board(string input)
        {
            emptyCount = 0;
            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    board[i, j] = new Num(input[i * BOARD_SIZE + j].ToString());

                    if (board[i, j].Value == 0)
                    {
                        ++emptyCount;
                    }
                }
            }

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    if (board[i, j].Value != 0)
                    {
                        EvaluatePossibilities(j, i);
                    }
                }
            }
        }

        /// <summary>
        /// Converts the board into its string representation
        /// </summary>
        /// <param name="b">The Sudoku board (A 2D Num array) that will be converted into its string representation</param>
        /// <returns>The text representation of parameter b</returns>
        private string ExportBoard(Num[,] b)
        {
            string output = "";

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    output += b[i, j].Value;
                }
            }
            
            return output;
        }

        /// <summary>
        /// Solves the Sudoku board
        /// </summary>
        public void Solve()
        {
            BasicSolver();
            if (emptyCount > 0)
            {
                board = Guess(board);
            }
        }

        private Num[,] Guess(Num[,] b)
        {
            #region Find Box With Min Possibilities

            int minPossibilities = 2;
            int x = -1;
            int y = 0;
            while (x == -1)
            {
                for (int i = 0; i < BOARD_SIZE; ++i)
                {
                    for (int j = 0; j < BOARD_SIZE; ++j)
                    {
                        if (b[i, j].Value == 0 && b[i, j].possibilities.Count == minPossibilities)
                        {
                            x = j;
                            y = i;
                            break;
                        }
                    }

                    if (x != -1)
                        break;
                }

                if (x == -1)
                    ++minPossibilities;
                if (minPossibilities == 9)
                {
                    invalidBoard = true;
                    return b;
                }
            }

            #endregion

            string s = ExportBoard(b);
            Board tempBoard = new Board(s);
            tempBoard.BasicSolver();

            for (int i = 0; i < b[y, x].possibilities.Count; ++i)
            {
                tempBoard = new Board(s);
                tempBoard.board[y, x].Value = b[y, x].possibilities[i];
                tempBoard.EvaluatePossibilities(x, y);
                tempBoard.BasicSolver();

                if (!tempBoard.invalidBoard)
                {
                    if (tempBoard.emptyCount == 0)
                    {
                        return tempBoard.board;
                    }
                    else
                    {
                        tempBoard.board = Guess(tempBoard.board);
                        if (tempBoard.EmptyCount() == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (tempBoard.EmptyCount() == 0)
            {
                return tempBoard.board;
            }
            else
            {
                return b;
            }
        }

        public void BasicSolver()
        {
            int empty = EmptyCount();
            int prevCount = empty;
            bool continueSolving = true;

            while (empty > 0 && continueSolving)
            {
                #region Linear Check

                for (int i = 0; i < BOARD_SIZE && continueSolving; ++i)
                {
                    for (int j = 0; j < BOARD_SIZE; ++j)
                    {
                        if (board[i, j].Value == 0)
                        {
                            List<Num> lineNumbers = GetLineNums(j, i);

                            foreach (Num n in lineNumbers)
                            {
                                board[i, j].possibilities.Remove(n.Value);
                            }

                            if (board[i, j].possibilities.Count == 1)
                            {
                                board[i, j].Value = board[i, j].possibilities[0];
                                --empty;
                                EvaluatePossibilities(j, i);
                                break;
                            }
                            else if (board[i, j].possibilities.Count == 0)
                            {
                                continueSolving = false;
                                invalidBoard = true;
                                break;
                            }
                        }
                    }
                }

                #endregion

                #region Square Check I

                for (int i = 0; i < BOARD_SIZE && continueSolving; ++i)
                {
                    for (int j = 0; j < BOARD_SIZE; ++j)
                    {
                        if (board[i, j].Value == 0)
                        {
                            List<Num> squareNums = GetSquareNums(j, i);

                            foreach (Num n in squareNums)
                            {
                                if (n.Value != 0)
                                {
                                    board[i, j].possibilities.Remove(n.Value);
                                }
                            }

                            if (board[i, j].possibilities.Count == 1)
                            {
                                board[i, j].Value = board[i, j].possibilities[0];
                                --empty;
                                EvaluatePossibilities(j, i);
                                break;
                            }
                            else if (board[i, j].possibilities.Count == 0)
                            {
                                continueSolving = false;
                                invalidBoard = true;
                                break;
                            }
                        }
                    }
                }

                #endregion

                if (prevCount == empty)
                {
                    break;
                }
                else
                {
                    prevCount = empty;
                }
            }

            emptyCount = EmptyCount();
        }

        /// <summary>
        /// Calculates the number of empty numbers in the board
        /// </summary>
        /// <returns>The number of empty numbers in the board</returns>
        private byte EmptyCount()
        {
            emptyCount = 0;
            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    if (board[i, j].Value == 0)
                    {
                        ++emptyCount;
                    }
                }
            }
            return emptyCount;
        }
        
        /// <summary>
        /// Evaluate the possibilities of all other empty numbers in the same square or on the same vertical and horiontal line
        /// </summary>
        /// <param name="x">The x coordinate of the number in the board</param>
        /// <param name="y">The y coordinate of the number in the board</param>
        private void EvaluatePossibilities(int x, int y)
        {
            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                if (i != x && board[y, i].Value == 0)
                {
                    board[y, i].possibilities.Remove(board[y, x].Value);
                }
                if (i != y && board[i, x].Value == 0 && board[i, x].possibilities.Contains(board[y, x].Value))
                {
                    board[i, x].possibilities.Remove(board[y, x].Value);
                }
            }

            if ((x <= 2 && x >= 0) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 0
                board[0, 0].possibilities.Remove(board[y, x].Value);
                board[0, 1].possibilities.Remove(board[y, x].Value);
                board[0, 2].possibilities.Remove(board[y, x].Value);
                board[1, 0].possibilities.Remove(board[y, x].Value);
                board[1, 1].possibilities.Remove(board[y, x].Value);
                board[1, 2].possibilities.Remove(board[y, x].Value);
                board[2, 0].possibilities.Remove(board[y, x].Value);
                board[2, 1].possibilities.Remove(board[y, x].Value);
                board[2, 2].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 5 && x >= 3) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 1
                board[0, 3].possibilities.Remove(board[y, x].Value);
                board[0, 4].possibilities.Remove(board[y, x].Value);
                board[0, 5].possibilities.Remove(board[y, x].Value);
                board[1, 3].possibilities.Remove(board[y, x].Value);
                board[1, 4].possibilities.Remove(board[y, x].Value);
                board[1, 5].possibilities.Remove(board[y, x].Value);
                board[2, 3].possibilities.Remove(board[y, x].Value);
                board[2, 4].possibilities.Remove(board[y, x].Value);
                board[2, 5].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 8 && x >= 6) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 2
                board[0, 6].possibilities.Remove(board[y, x].Value);
                board[0, 7].possibilities.Remove(board[y, x].Value);
                board[0, 8].possibilities.Remove(board[y, x].Value);
                board[1, 6].possibilities.Remove(board[y, x].Value);
                board[1, 7].possibilities.Remove(board[y, x].Value);
                board[1, 8].possibilities.Remove(board[y, x].Value);
                board[2, 6].possibilities.Remove(board[y, x].Value);
                board[2, 7].possibilities.Remove(board[y, x].Value);
                board[2, 8].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 2 && x >= 0) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 0
                board[3, 0].possibilities.Remove(board[y, x].Value);
                board[3, 1].possibilities.Remove(board[y, x].Value);
                board[3, 2].possibilities.Remove(board[y, x].Value);
                board[4, 0].possibilities.Remove(board[y, x].Value);
                board[4, 1].possibilities.Remove(board[y, x].Value);
                board[4, 2].possibilities.Remove(board[y, x].Value);
                board[5, 0].possibilities.Remove(board[y, x].Value);
                board[5, 1].possibilities.Remove(board[y, x].Value);
                board[5, 2].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 5 && x >= 3) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 1
                board[3, 3].possibilities.Remove(board[y, x].Value);
                board[3, 4].possibilities.Remove(board[y, x].Value);
                board[3, 5].possibilities.Remove(board[y, x].Value);
                board[4, 3].possibilities.Remove(board[y, x].Value);
                board[4, 4].possibilities.Remove(board[y, x].Value);
                board[4, 5].possibilities.Remove(board[y, x].Value);
                board[5, 3].possibilities.Remove(board[y, x].Value);
                board[5, 4].possibilities.Remove(board[y, x].Value);
                board[5, 5].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 8 && x >= 6) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 2
                board[3, 6].possibilities.Remove(board[y, x].Value);
                board[3, 7].possibilities.Remove(board[y, x].Value);
                board[3, 8].possibilities.Remove(board[y, x].Value);
                board[4, 6].possibilities.Remove(board[y, x].Value);
                board[4, 7].possibilities.Remove(board[y, x].Value);
                board[4, 8].possibilities.Remove(board[y, x].Value);
                board[5, 6].possibilities.Remove(board[y, x].Value);
                board[5, 7].possibilities.Remove(board[y, x].Value);
                board[5, 8].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 2 && x >= 0) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 0
                board[6, 0].possibilities.Remove(board[y, x].Value);
                board[6, 1].possibilities.Remove(board[y, x].Value);
                board[6, 2].possibilities.Remove(board[y, x].Value);
                board[7, 0].possibilities.Remove(board[y, x].Value);
                board[7, 1].possibilities.Remove(board[y, x].Value);
                board[7, 2].possibilities.Remove(board[y, x].Value);
                board[8, 0].possibilities.Remove(board[y, x].Value);
                board[8, 1].possibilities.Remove(board[y, x].Value);
                board[8, 2].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 5 && x >= 3) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 1
                board[6, 3].possibilities.Remove(board[y, x].Value);
                board[6, 4].possibilities.Remove(board[y, x].Value);
                board[6, 5].possibilities.Remove(board[y, x].Value);
                board[7, 3].possibilities.Remove(board[y, x].Value);
                board[7, 4].possibilities.Remove(board[y, x].Value);
                board[7, 5].possibilities.Remove(board[y, x].Value);
                board[8, 3].possibilities.Remove(board[y, x].Value);
                board[8, 4].possibilities.Remove(board[y, x].Value);
                board[8, 5].possibilities.Remove(board[y, x].Value);
            }
            else if ((x <= 8 && x >= 6) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 2
                board[6, 6].possibilities.Remove(board[y, x].Value);
                board[6, 7].possibilities.Remove(board[y, x].Value);
                board[6, 8].possibilities.Remove(board[y, x].Value);
                board[7, 6].possibilities.Remove(board[y, x].Value);
                board[7, 7].possibilities.Remove(board[y, x].Value);
                board[7, 8].possibilities.Remove(board[y, x].Value);
                board[8, 6].possibilities.Remove(board[y, x].Value);
                board[8, 7].possibilities.Remove(board[y, x].Value);
                board[8, 8].possibilities.Remove(board[y, x].Value);
            }
        }
        
        /// <summary>
        /// Returns the numbers that are on the same horizontal or vertical line as the number at a given x and y
        /// </summary>
        /// <param name="x">The x coordinate of the number in the board</param>
        /// <param name="y">The y coordinate of the number in the board</param>
        /// <returns>A list of Nums which contains all unique non-empty numbers on the same line
        /// as the number at x and y</returns>
        private List<Num> GetLineNums(int x, int y)
        {
            List<Num> nums = new List<Num>();

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                if (i != x && board[y, i].Value != 0)
                {
                    if (nums.Any(n => n.Value == board[y, i].Value) == false)
                    {
                        nums.Add(board[y, i]);
                    }
                }
                if (i != y && board[i, x].Value != 0)
                {
                    if (nums.Any(n => n.Value == board[i, x].Value) == false)
                    {
                        nums.Add(board[i, x]);
                    }
                }
            }

            return nums;
        }
        
        /// <summary>
        /// Returns the other numbers that share the same 3*3 square as the number at a given x and y
        /// </summary>
        /// <param name="x">The x coordinate of the number in the board</param>
        /// <param name="y">The y coordinate of the number in the board</param>
        /// <returns>A list of Nums which contains all other non-empty numbers in the same 3*3 square 
        /// as the number at x and y</returns>
        private List<Num> GetSquareNums(int x, int y)
        {
            /*
             * Box: 0, 0     0: 0-2      1: 0-2      2: 0-2
             * Box: 0, 1     0: 3-5      1: 3-5      2: 3-5
             * Box: 0, 2     0: 6-8      1: 6-8      2: 6-8
             * Box: 1, 0     3: 0-2      4: 0-2      5: 0-2
             * Box: 1, 1     3: 3-5      4: 3-5      5: 3-5
             * Box: 1, 2     3: 6-8      4: 6-8      5: 6-8
             * Box: 2, 0     6: 0-2      7: 0-2      8: 0-2
             * Box: 2, 1     6: 3-5      7: 3-5      8: 3-5
             * Box: 2, 2     6: 6-8      7: 6-8      8: 6-8
             */

            List<Num> squareNums = new List<Num>();

            if ((x <= 2 && x >= 0) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 0
                squareNums.Add(board[0, 0]);
                squareNums.Add(board[0, 1]);
                squareNums.Add(board[0, 2]);
                squareNums.Add(board[1, 0]);
                squareNums.Add(board[1, 1]);
                squareNums.Add(board[1, 2]);
                squareNums.Add(board[2, 0]);
                squareNums.Add(board[2, 1]);
                squareNums.Add(board[2, 2]);
            }
            else if ((x <= 5 && x >= 3) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 1
                squareNums.Add(board[0, 3]);
                squareNums.Add(board[0, 4]);
                squareNums.Add(board[0, 5]);
                squareNums.Add(board[1, 3]);
                squareNums.Add(board[1, 4]);
                squareNums.Add(board[1, 5]);
                squareNums.Add(board[2, 3]);
                squareNums.Add(board[2, 4]);
                squareNums.Add(board[2, 5]);
            }
            else if ((x <= 8 && x >= 6) && (y == 0 || y == 1 || y == 2))
            {
                //Box 0, 2
                squareNums.Add(board[0, 6]);
                squareNums.Add(board[0, 7]);
                squareNums.Add(board[0, 8]);
                squareNums.Add(board[1, 6]);
                squareNums.Add(board[1, 7]);
                squareNums.Add(board[1, 8]);
                squareNums.Add(board[2, 6]);
                squareNums.Add(board[2, 7]);
                squareNums.Add(board[2, 8]);
            }
            else if ((x <= 2 && x >= 0) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 0
                squareNums.Add(board[3, 0]);
                squareNums.Add(board[3, 1]);
                squareNums.Add(board[3, 2]);
                squareNums.Add(board[4, 0]);
                squareNums.Add(board[4, 1]);
                squareNums.Add(board[4, 2]);
                squareNums.Add(board[5, 0]);
                squareNums.Add(board[5, 1]);
                squareNums.Add(board[5, 2]);
            }
            else if ((x <= 5 && x >= 3) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 1
                squareNums.Add(board[3, 3]);
                squareNums.Add(board[3, 4]);
                squareNums.Add(board[3, 5]);
                squareNums.Add(board[4, 3]);
                squareNums.Add(board[4, 4]);
                squareNums.Add(board[4, 5]);
                squareNums.Add(board[5, 3]);
                squareNums.Add(board[5, 4]);
                squareNums.Add(board[5, 5]);
            }
            else if ((x <= 8 && x >= 6) && (y == 3 || y == 4 || y == 5))
            {
                //Box 1, 2
                squareNums.Add(board[3, 6]);
                squareNums.Add(board[3, 7]);
                squareNums.Add(board[3, 8]);
                squareNums.Add(board[4, 6]);
                squareNums.Add(board[4, 7]);
                squareNums.Add(board[4, 8]);
                squareNums.Add(board[5, 6]);
                squareNums.Add(board[5, 7]);
                squareNums.Add(board[5, 8]);
            }
            else if ((x <= 2 && x >= 0) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 0
                squareNums.Add(board[6, 0]);
                squareNums.Add(board[6, 1]);
                squareNums.Add(board[6, 2]);
                squareNums.Add(board[7, 0]);
                squareNums.Add(board[7, 1]);
                squareNums.Add(board[7, 2]);
                squareNums.Add(board[8, 0]);
                squareNums.Add(board[8, 1]);
                squareNums.Add(board[8, 2]);
            }
            else if ((x <= 5 && x >= 3) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 1
                squareNums.Add(board[6, 3]);
                squareNums.Add(board[6, 4]);
                squareNums.Add(board[6, 5]);
                squareNums.Add(board[7, 3]);
                squareNums.Add(board[7, 4]);
                squareNums.Add(board[7, 5]);
                squareNums.Add(board[8, 3]);
                squareNums.Add(board[8, 4]);
                squareNums.Add(board[8, 5]);
            }
            else if ((x <= 8 && x >= 6) && (y == 6 || y == 7 || y == 8))
            {
                //Box 2, 2
                squareNums.Add(board[6, 6]);
                squareNums.Add(board[6, 7]);
                squareNums.Add(board[6, 8]);
                squareNums.Add(board[7, 6]);
                squareNums.Add(board[7, 7]);
                squareNums.Add(board[7, 8]);
                squareNums.Add(board[8, 6]);
                squareNums.Add(board[8, 7]);
                squareNums.Add(board[8, 8]);
            }

            squareNums.Remove(board[y, x]);
            squareNums.RemoveAll(n => n.Value == 0);
            return squareNums;
        }
    }
}