using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace tic_tac_toe {
    class GameGrid {
        private Panel gameBoard;
        private int cellCount = 3;

        public GameGrid(Panel gameBoard) {
            this.gameBoard = gameBoard;
        }

        public void Draw() {
            int cellWidth = gameBoard.Size.Width / cellCount;
            int cellHeight = gameBoard.Size.Height / cellCount;
            Pen pen = new Pen(Color.Black, 2);

            using (Graphics g = gameBoard.CreateGraphics()) {
                for (int i = 0; i <= cellCount; i++) {
                    g.DrawLine(pen, 0, cellHeight * i, gameBoard.Size.Width, cellHeight * i); //horizontal
                    g.DrawLine(pen, cellWidth * i, 0, cellWidth * i, gameBoard.Size.Height); //vertical
                }
            }
        }
    }
}
