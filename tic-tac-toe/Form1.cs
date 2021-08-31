using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace tic_tac_toe {
    enum GameStatus { PAUSE, PLAY }

    public partial class Form1 : Form {
        private GameGrid gameGrid;
        private GameStatus gameStatus = GameStatus.PAUSE;
        private Player startingPlayer;
        private bool OisAI, XisAI;
        private AI playerO, playerX;

        public Form1(bool OisAI, bool XisAI, Player startingPlayer) {
            InitializeComponent();

            this.startingPlayer = startingPlayer;
            gameGrid = new GameGrid(startingPlayer, canvas);
            this.OisAI = OisAI;
            this.XisAI = XisAI;

            gameGrid.Gameover += gameGrid_Gameover;
            gameGrid.SwitchPlayer += gameGrid_SwitchPlayer;

            playerO = new AI(Player.O, gameGrid);
            playerX = new AI(Player.X, gameGrid);

            gameStatus = GameStatus.PLAY;

            if (gameGrid.currentPlayer == Player.O && OisAI) {
                KeyValuePair<int, int> temp = playerO.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }
            }

            if (gameGrid.currentPlayer == Player.X && XisAI) {
                KeyValuePair<int, int> temp = playerX.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e) {
            gameGrid.Draw(e.Graphics);
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            if (gameStatus == GameStatus.PLAY) {                
                gameGrid.Put(e.X / gameGrid.CellWidth(), e.Y / gameGrid.CellHeight());

                evaluationResult.Text = "O: " + playerO.Evaluate(gameGrid.gameBoard, gameGrid.currentPlayer) + "\n" +
                    "X: " + playerX.Evaluate(gameGrid.gameBoard, gameGrid.currentPlayer);
            }
        }

        private void restart_Click(object sender, EventArgs e) {
            gameStatus = GameStatus.PAUSE;

            gameoverLabel.Visible = false;
            gameGrid = new GameGrid(startingPlayer, canvas);
            gameGrid.Gameover += gameGrid_Gameover;
            gameGrid.SwitchPlayer += gameGrid_SwitchPlayer;
            playerO = new AI(Player.O, gameGrid);
            playerX = new AI(Player.X, gameGrid);
            canvas.Invalidate();

            gameStatus = GameStatus.PLAY;

            if (gameGrid.currentPlayer == Player.O && OisAI) {
                KeyValuePair<int, int> temp = playerO.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }
            }

            if (gameGrid.currentPlayer == Player.X && XisAI) {
                KeyValuePair<int, int> temp = playerX.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }
            }
        }
      

        private void gameGrid_Gameover(object sender, GameoverEventArgs e) {          
            gameStatus = GameStatus.PAUSE;
            if (e.Winner == Player.O) gameoverLabel.Text = "Gameover\nO wins!!";            
            else gameoverLabel.Text = "Gameover\nX wins!!";
            gameoverLabel.Visible = true;
        }
        int i = 0;
        private void gameGrid_SwitchPlayer(object sender, EventArgs e) {
            if (gameGrid.currentPlayer == Player.O && OisAI) {
                KeyValuePair<int, int> temp = playerO.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }                    
            }

            if (gameGrid.currentPlayer == Player.X && XisAI) {
                KeyValuePair<int, int> temp = playerX.Move();
                if (temp.Key != -1 && temp.Value != -1) {
                    gameGrid.Put(temp.Key, temp.Value);
                }
            }
        }

    }
}
