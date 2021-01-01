using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac_toe {
    enum GameStatus { PAUSE, PLAY }

    public partial class Form1 : Form {
        private GameGrid gameGrid;
        private GameStatus gameStatus = GameStatus.PAUSE;
        AI playerO, playerX;

        public Form1() {
            InitializeComponent();
            gameGrid = new GameGrid(canvas);
            gameGrid.Gameover += gameGrid_Gameover;
            playerO = new AI(Player.O, GameGrid.WIN_CONDITION, gameGrid.gameBoard);
            playerX = new AI(Player.X, GameGrid.WIN_CONDITION, gameGrid.gameBoard);

            gameStatus = GameStatus.PLAY;
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
            gameGrid = new GameGrid(canvas);
            gameGrid.Gameover += gameGrid_Gameover;
            canvas.Invalidate();

            gameStatus = GameStatus.PLAY;
        }

        private void gameGrid_Gameover(object sender, GameoverEventArgs e) {          
            gameStatus = GameStatus.PAUSE;
            if (e.Winner == Player.O) gameoverLabel.Text = "Gameover\nO wins!!";            
            else gameoverLabel.Text = "Gameover\nX wins!!";
            gameoverLabel.Visible = true;
        }

    }
}
