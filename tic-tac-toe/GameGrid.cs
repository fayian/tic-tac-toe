using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace tic_tac_toe {
    enum Player { NONE, O, X};
    class GameGrid {
        private const int MARGIN_COEFFICIENT = 8;

        private Panel canvas;
        private int cellCount = 3;
        Player currentPlayer = Player.O;  //true: O, false: X
        List<List<Player>> gameBoard;

        public GameGrid(Panel canvas) {
            this.canvas = canvas;

            gameBoard = new List<List<Player>>(cellCount);
            for (int i = 0; i < cellCount; i++) gameBoard.Add(new List<Player>(cellCount));
            foreach (List<Player> list in gameBoard) 
                for(int i = 0; i < cellCount; i++) 
                    list.Add(Player.NONE);
        }

        public int CellWidth() { return canvas.Size.Width / cellCount;  }
        public int CellHeight() { return canvas.Size.Height / cellCount; }

        public void Draw() {
            int cellWidth = canvas.Size.Width / cellCount;
            int cellHeight = canvas.Size.Height / cellCount;
            Pen pen = new Pen(Color.Black, 2);

            using (Graphics g = canvas.CreateGraphics()) {
                for (int i = 0; i <= cellCount; i++) {
                    g.DrawLine(pen, 0, cellHeight * i, canvas.Size.Width, cellHeight * i); //horizontal
                    g.DrawLine(pen, cellWidth * i, 0, cellWidth * i, canvas.Size.Height); //vertical
                }
            }
        }
    
        // put on the xth cell counting from left to right
        //                     yth cell counting from top to bottom
        public void Put(int x, int y) {
            if(gameBoard[x][y] == Player.NONE) {
                int marginX = Math.Max(1, CellWidth() / MARGIN_COEFFICIENT);
                int marginY = Math.Max(1, CellHeight() / MARGIN_COEFFICIENT);
                Pen pen = new Pen(Color.Black, 2);

                using (Graphics g = canvas.CreateGraphics()) {
                    if (currentPlayer == Player.O) {
                        g.DrawEllipse(pen, x * CellWidth() + marginX, y * CellHeight() + marginY,
                                                 CellWidth() - 2 * marginX, CellHeight() - 2 * marginY);

                        gameBoard[x][y] = Player.O;
                        currentPlayer = Player.X;
                    } else {
                        int left = CellWidth() * x + marginX;
                        int right = CellWidth() * (x + 1) - marginX;
                        int top = CellHeight() * y + marginY;
                        int bottom = CellHeight() * (y + 1) - marginY;
                        g.DrawLine(pen, left, top, right, bottom);
                        g.DrawLine(pen, left, bottom, right, top);

                        gameBoard[x][y] = Player.X;
                        currentPlayer = Player.O;
                    }
                }
            }
        }
    }
}
