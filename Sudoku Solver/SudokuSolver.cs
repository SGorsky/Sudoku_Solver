using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class frmSudokuSolver : Form
    {
        MaskedTextBox[,] textbox = new MaskedTextBox[9, 9];
        const byte BOARD_SIZE = 9;
        public frmSudokuSolver()
        {
            InitializeComponent();

            textbox[0, 0] = mtb00;
            textbox[0, 1] = mtb01;
            textbox[0, 2] = mtb02;
            textbox[0, 3] = mtb03;
            textbox[0, 4] = mtb04;
            textbox[0, 5] = mtb05;
            textbox[0, 6] = mtb06;
            textbox[0, 7] = mtb07;
            textbox[0, 8] = mtb08;

            textbox[1, 0] = mtb10;
            textbox[1, 1] = mtb11;
            textbox[1, 2] = mtb12;
            textbox[1, 3] = mtb13;
            textbox[1, 4] = mtb14;
            textbox[1, 5] = mtb15;
            textbox[1, 6] = mtb16;
            textbox[1, 7] = mtb17;
            textbox[1, 8] = mtb18;

            textbox[2, 0] = mtb20;
            textbox[2, 1] = mtb21;
            textbox[2, 2] = mtb22;
            textbox[2, 3] = mtb23;
            textbox[2, 4] = mtb24;
            textbox[2, 5] = mtb25;
            textbox[2, 6] = mtb26;
            textbox[2, 7] = mtb27;
            textbox[2, 8] = mtb28;

            textbox[3, 0] = mtb30;
            textbox[3, 1] = mtb31;
            textbox[3, 2] = mtb32;
            textbox[3, 3] = mtb33;
            textbox[3, 4] = mtb34;
            textbox[3, 5] = mtb35;
            textbox[3, 6] = mtb36;
            textbox[3, 7] = mtb37;
            textbox[3, 8] = mtb38;

            textbox[4, 0] = mtb40;
            textbox[4, 1] = mtb41;
            textbox[4, 2] = mtb42;
            textbox[4, 3] = mtb43;
            textbox[4, 4] = mtb44;
            textbox[4, 5] = mtb45;
            textbox[4, 6] = mtb46;
            textbox[4, 7] = mtb47;
            textbox[4, 8] = mtb48;

            textbox[5, 0] = mtb50;
            textbox[5, 1] = mtb51;
            textbox[5, 2] = mtb52;
            textbox[5, 3] = mtb53;
            textbox[5, 4] = mtb54;
            textbox[5, 5] = mtb55;
            textbox[5, 6] = mtb56;
            textbox[5, 7] = mtb57;
            textbox[5, 8] = mtb58;

            textbox[6, 0] = mtb60;
            textbox[6, 1] = mtb61;
            textbox[6, 2] = mtb62;
            textbox[6, 3] = mtb63;
            textbox[6, 4] = mtb64;
            textbox[6, 5] = mtb65;
            textbox[6, 6] = mtb66;
            textbox[6, 7] = mtb67;
            textbox[6, 8] = mtb68;

            textbox[7, 0] = mtb70;
            textbox[7, 1] = mtb71;
            textbox[7, 2] = mtb72;
            textbox[7, 3] = mtb73;
            textbox[7, 4] = mtb74;
            textbox[7, 5] = mtb75;
            textbox[7, 6] = mtb76;
            textbox[7, 7] = mtb77;
            textbox[7, 8] = mtb78;

            textbox[8, 0] = mtb80;
            textbox[8, 1] = mtb81;
            textbox[8, 2] = mtb82;
            textbox[8, 3] = mtb83;
            textbox[8, 4] = mtb84;
            textbox[8, 5] = mtb85;
            textbox[8, 6] = mtb86;
            textbox[8, 7] = mtb87;
            textbox[8, 8] = mtb88;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            Board board;
            string input = "";

            //Read in the board from each of the masked textboxes
            //The board is read in as a string/
            //If a box has a number then add that number to the string
            //If a box is empty then add 0 to the string
            #region Read in Board

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    if (textbox[i, j].Text != String.Empty)
                    {
                        input += textbox[i, j].Text;
                        textbox[i, j].ForeColor = Color.Black;
                    }
                    else
                    {
                        input += "0";
                        textbox[i, j].ForeColor = Color.Green;
                    }
                }
            }

            board = new Board(input);

            #endregion
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            board.Solve();

            sw.Stop();
            double swDuration = (sw.ElapsedTicks * 1000.0) / System.Diagnostics.Stopwatch.Frequency;
            MessageBox.Show("StopWatch:\t" + swDuration + " ms");
            
            //Output the board to the masked textboxes
            #region Output Board

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    textbox[i, j].Text = board.board[i, j].ToString();
                }
            }

            #endregion
        }
 
        /// <summary>
        /// Clear all the numbers from the masked textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < textbox.GetLength(0); ++i)
            {
                for (int j = 0; j < textbox.GetLength(1); ++j)
                {
                    textbox[i, j].Clear();
                    textbox[i, j].ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Control the focus of the masked textboxes when you press the arrow keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSudokuSolver_KeyDown(object sender, KeyEventArgs e)
        {
            int xIndex = -1;
            int yIndex = 0;
            for (int i = 0; i < textbox.GetLength(0); ++i)
            {
                for (int j = 0; j < textbox.GetLength(1); ++j)
                {
                    if (textbox[i, j].Focused)
                    {
                        xIndex = j;
                        yIndex = i;
                    }
                }
            }

            if (xIndex != -1)
            {
                switch (e.KeyData)
                {
                    case Keys.Up:
                        if (yIndex > 0)
                        {
                            --yIndex;
                        }
                        break;
                    case Keys.Down:
                        if (yIndex < 8)
                        {
                            ++yIndex;
                        }
                        break;
                    case Keys.Left:
                        if (xIndex > 0)
                        {
                            --xIndex;
                        }
                        break;
                    case Keys.Right:
                        if (xIndex < 8)
                        {
                            ++xIndex;
                        }
                        break;
                }

                textbox[yIndex, xIndex].Focus();
            }
        }
    }
}