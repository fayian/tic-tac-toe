using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace tic_tac_toe {
    enum Player { NONE, O, X };

    class GameoverEventArgs {
        public Player  Winner { get; }
        public GameoverEventArgs(Player winner) {
            Winner = winner;
        }
    }

    class GameGrid {
        private const int MARGIN_COEFFICIENT = 8;
        private const int WIN_CONDITION = 3;

        private Panel canvas;
        private Pen pen = new Pen(Color.Black, 2);
        private int cellCount = 3;
        private List<List<Player>> gameBoard;

        public Player currentPlayer = Player.O;        

        public delegate void GameoverEventHandler(object sender, GameoverEventArgs e);
        public event GameoverEventHandler Gameover;

        private void DrawCircle(int cellX, int cellY, Graphics g) {
            int marginX = Math.Max(1, CellWidth() / MARGIN_COEFFICIENT);
            int marginY = Math.Max(1, CellHeight() / MARGIN_COEFFICIENT);
            g.DrawEllipse(pen, cellX * CellWidth() + marginX, cellY * CellHeight() + marginY,
                                                 CellWidth() - 2 * marginX, CellHeight() - 2 * marginY);
        }
        private void DrawCross(int cellX, int cellY, Graphics g) {
            int marginX = Math.Max(1, CellWidth() / MARGIN_COEFFICIENT);
            int marginY = Math.Max(1, CellHeight() / MARGIN_COEFFICIENT);
            int left = CellWidth() * cellX + marginX;
            int right = CellWidth() * (cellX + 1) - marginX;
            int top = CellHeight() * cellY + marginY;
            int bottom = CellHeight() * (cellY + 1) - marginY;
            g.DrawLine(pen, left, top, right, bottom);
            g.DrawLine(pen, left, bottom, right, top);
        }

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

        public void Draw(Graphics g) {
            int cellWidth = canvas.Size.Width / cellCount;
            int cellHeight = canvas.Size.Height / cellCount;

            //draw grid
            for (int i = 0; i <= cellCount; i++) {
                g.DrawLine(pen, 0, cellHeight * i, canvas.Size.Width, cellHeight * i); //horizontal
                g.DrawLine(pen, cellWidth * i, 0, cellWidth * i, canvas.Size.Height); //vertical
            }

            //draw O and X            
            for (int x = 0; x < gameBoard.Count; x++) {
                for (int y = 0; y < gameBoard.Count; y++) {
                    if(gameBoard[x][y] != Player.NONE) {
                        if (gameBoard[x][y] == Player.X) DrawCross(x, y, g);
                        else DrawCircle(x, y, g);
                    }
                }
            }
        }
    
        // put on the xth cell counting from left to right
        //                     yth cell counting from top to bottom
        public void Put(int cellX, int cellY) {
            if(gameBoard[cellX][cellY] == Player.NONE) {
                using (Graphics g = canvas.CreateGraphics()) {
                    if (currentPlayer == Player.O) {
                        DrawCircle(cellX, cellY, g);
                        gameBoard[cellX][cellY] = Player.O;                        
                    } else {
                        DrawCross(cellX, cellY, g);
                        gameBoard[cellX][cellY] = Player.X;
                    }
                }

                //check if the player wins
                byte possiblility = 0xFF;
                //start from top, clockwise
                sbyte[] deltaX = { 0, 1, 1,  1,  0, -1, -1, -1 };
                sbyte[] deltaY = { 1, 1, 0, -1, -1, -1,  0,  1 };
                for (int i = 1; i < WIN_CONDITION; i++) {
                    for(byte bin = 0x1, j = 0; j < 8; bin <<= 1, j++) {
                        if ((possiblility & bin) != 0) {
                            if (cellX + i * deltaX[j] >= cellCount || cellX + i * deltaX[j] < 0 ||
                                 cellY + i * deltaY[j] >= cellCount || cellY + i * deltaY[j] < 0)
                                possiblility ^= bin;
                            else if (gameBoard[ cellX + i * deltaX[j] ][ cellY + i * deltaY[j] ] != currentPlayer)
                                possiblility ^= bin;
                        }
                    }
                }
                if (possiblility != 0) Gameover?.Invoke(this, new GameoverEventArgs(currentPlayer));


                //switch current player
                if (currentPlayer == Player.O) currentPlayer = Player.X;
                else if (currentPlayer == Player.X) currentPlayer = Player.O;
            }
        }
    }
}
