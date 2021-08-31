using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace tic_tac_toe {
    public enum Player { NONE, O, X };

    class GameoverEventArgs {
        public Player  Winner { get; }
        public GameoverEventArgs(Player winner) {
            Winner = winner;
        }
    }

    class GameGrid {
        private const int MARGIN_COEFFICIENT = 8;
        internal const int WIN_CONDITION = 3;
        public const int CELL_COUNT = 3;

        private Panel canvas;
        internal List<List<Player>> gameBoard;
        private int movesLeft = CELL_COUNT* CELL_COUNT;

       public Player currentPlayer { get; private set; }

        public delegate void GameoverEventHandler(object sender, GameoverEventArgs e);
        public event GameoverEventHandler Gameover;
        public event EventHandler SwitchPlayer;

        private void DrawCircle(int cellX, int cellY, Pen pen, Graphics g) {
            int marginX = Math.Max(1, CellWidth() / MARGIN_COEFFICIENT);
            int marginY = Math.Max(1, CellHeight() / MARGIN_COEFFICIENT);
            g.DrawEllipse(pen, cellX * CellWidth() + marginX, cellY * CellHeight() + marginY,
                                                 CellWidth() - 2 * marginX, CellHeight() - 2 * marginY);
        }
        private void DrawCross(int cellX, int cellY, Pen pen, Graphics g) {
            int marginX = Math.Max(1, CellWidth() / MARGIN_COEFFICIENT);
            int marginY = Math.Max(1, CellHeight() / MARGIN_COEFFICIENT);
            int left = CellWidth() * cellX + marginX;
            int right = CellWidth() * (cellX + 1) - marginX;
            int top = CellHeight() * cellY + marginY;
            int bottom = CellHeight() * (cellY + 1) - marginY;
            g.DrawLine(pen, left, top, right, bottom);
            g.DrawLine(pen, left, bottom, right, top);
        }

        public GameGrid(Player startingPlayer, Panel canvas) {
            this.canvas = canvas;
            currentPlayer = startingPlayer;

            gameBoard = new List<List<Player>>(CELL_COUNT);
            for (int i = 0; i < CELL_COUNT; i++) gameBoard.Add(new List<Player>(CELL_COUNT));
            foreach (List<Player> list in gameBoard) 
                for(int i = 0; i < CELL_COUNT; i++) 
                    list.Add(Player.NONE);
        }

        public int CellWidth() { return canvas.Size.Width / CELL_COUNT;  }
        public int CellHeight() { return canvas.Size.Height / CELL_COUNT; }
        public int MovesLeft() { return movesLeft; }

        public void Draw(Graphics g) {
            int cellWidth = canvas.Size.Width / CELL_COUNT;
            int cellHeight = canvas.Size.Height / CELL_COUNT;

            using (Pen pen = new Pen(Color.Black, 2)) {
                //draw grid
                for (int i = 0; i <= CELL_COUNT; i++) {
                    g.DrawLine(pen, 0, cellHeight * i, canvas.Size.Width, cellHeight * i); //horizontal
                    g.DrawLine(pen, cellWidth * i, 0, cellWidth * i, canvas.Size.Height); //vertical
                }
                
                //draw O and X            
                for (int x = 0; x < gameBoard.Count; x++) {
                    for (int y = 0; y < gameBoard.Count; y++) {
                        if (gameBoard[x][y] != Player.NONE) {
                            if (gameBoard[x][y] == Player.X) DrawCross(x, y, pen, g);
                            else DrawCircle(x, y, pen, g);
                        }
                    }
                }                
            }
        }
    
        // put on the xth cell counting from left to right
        //                     yth cell counting from top to bottom
        public void Put(int cellX, int cellY) {
            if(gameBoard[cellX][cellY] == Player.NONE) {
                movesLeft--;
                if (currentPlayer == Player.O) {
                    gameBoard[cellX][cellY] = Player.O;
                } else {
                    gameBoard[cellX][cellY] = Player.X;
                }
                canvas.Invalidate(new Rectangle(cellX * CellWidth(), cellY * CellHeight(), CellWidth(), CellHeight()));

                //check if the player wins                
                int length;
                int x, y;

                sbyte[] deltaX = { 0, 1,  1, 1 };
                sbyte[] deltaY = { 1, 0, -1, 1 };
                //[0]: vertical
                //[1]: horizontal
                //[2]: top-left to bot-right
                //[3]: top-right to bot-left                

                for(int i = 0; i < 4; i++) {
                    length = 0;
                    for(int j = 1, k = 1; j <= WIN_CONDITION && j != -1 || k <= WIN_CONDITION && k != -1;) {
                        if(j != -1) {
                            x = cellX + deltaX[i] * j;
                            y = cellY + deltaY[i] * j;
                            if(x >= 0 && x < CELL_COUNT && y >= 0 && y < CELL_COUNT && gameBoard[x][y] == currentPlayer) {
                                length++;
                                j++;
                            } else {
                                j = -1;
                            }
                        }

                        if (k != -1) {
                            x = cellX - deltaX[i] * k;
                            y = cellY - deltaY[i] * k;
                            if (x >= 0 && x < CELL_COUNT && y >= 0 && y < CELL_COUNT && gameBoard[x][y] == currentPlayer) {
                                length++;
                                k++;
                            } else {
                                k = -1;
                            }
                        }
                    }

                    if (length + 1 >= WIN_CONDITION) {
                        Gameover?.Invoke(this, new GameoverEventArgs(currentPlayer));
                        break;
                    }
                }
                

                //switch current player
                if (currentPlayer == Player.O) currentPlayer = Player.X;
                else if (currentPlayer == Player.X) currentPlayer = Player.O;
                SwitchPlayer?.Invoke(this, new EventArgs());
            }
        }
    
    }
}
