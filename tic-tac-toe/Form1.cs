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
        }

        private void canvas_Paint(object sender, PaintEventArgs e) {
            gameGrid.Draw();
            gameoverLabel.Text = "Test\n123";
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            gameGrid.Put(e.X / gameGrid.CellWidth(), e.Y / gameGrid.CellHeight());
        }
    }
}
