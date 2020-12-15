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
    public partial class Form1 : Form {
        private GameGrid gameGrid;

        public Form1() {
            InitializeComponent();
            gameGrid = new GameGrid(canvas);
            gameGrid.Gameover += gameGrid_Gameover;
        }

        private void canvas_Paint(object sender, PaintEventArgs e) {
            gameGrid.Draw(e.Graphics);
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            gameGrid.Put(e.X / gameGrid.CellWidth(), e.Y / gameGrid.CellHeight());
        }

        private void gameGrid_Gameover(object sender, GameoverEventArgs e) {
            if (e.Winner == Player.O) gameoverLabel.Text = "Gameover\nO wins!!";
            else gameoverLabel.Text = "Gameover\nX wins!!";
            gameoverLabel.Visible = true;
        }
    }
}
