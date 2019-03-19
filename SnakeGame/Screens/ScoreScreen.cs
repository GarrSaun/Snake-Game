using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class ScoreScreen : UserControl
    {
        public ScoreScreen()
        {
            InitializeComponent();

            scoreLabel.Text = "";

            foreach(HighScore hs in MainForm.scoreList)
            {
                scoreLabel.Text += "name: " + hs.name + "  score: " + hs.score + "\n";
            }
        }

        private void ScoreScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //when the escape key is pressed the game ends and goes back to the main menu
            if (e.KeyCode == Keys.Escape)
            {
                MainForm.ChangeScreen(this, "MenuScreen");
            }
        }
    }
}
