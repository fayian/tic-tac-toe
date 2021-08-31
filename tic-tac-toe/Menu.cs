using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tic_tac_toe {
    public partial class Menu : Form {
        Form1 mainGame;
        public Menu() {
            InitializeComponent();

            comboBoxO.Items.Add("Player");
            comboBoxO.Items.Add("AI");
            comboBoxO.SelectedItem = comboBoxO.Items[0];
            comboBoxX.Items.Add("Player");
            comboBoxX.Items.Add("AI");
            comboBoxX.SelectedItem = comboBoxX.Items[0];
        }

        private void comboBoxO_SelectedIndexChanged(object sender, EventArgs e) {
            comboBoxO.Text = comboBoxO.SelectedItem.ToString();
        }

        private void comboBoxX_SelectedIndexChanged(object sender, EventArgs e) {
            comboBoxX.Text = comboBoxX.SelectedItem.ToString();
        }

        private void startButton_Click(object sender, EventArgs e) {
            mainGame = new Form1(comboBoxO.SelectedItem == comboBoxO.Items[1],
                                                          comboBoxX.SelectedItem == comboBoxX.Items[1],
                                                          OGoesFirst.Checked?Player.O:Player.X);
            mainGame.Show();
        }
    }
}
