using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameSystemServices;
using System.Xml;

namespace SnakeGame
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            MainForm.ChangeScreen(this, "GameScreen");
        }

        private void scoresButton_Click(object sender, EventArgs e)
        {
            MainForm.ChangeScreen(this, "ScoreScreen");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            XmlWriter writer = XmlWriter.Create("Resources/Scores.xml");

            //Write the root element
            writer.WriteStartElement("HighScores");

            foreach(HighScore hs in MainForm.scoreList)
            {
                //Start an element
                writer.WriteStartElement("Score");

                //Write sub-elements
                writer.WriteElementString("name", hs.name);
                writer.WriteElementString("score", hs.score.ToString());

                // end the element
                writer.WriteEndElement();
            }

            // end the root element
            writer.WriteEndElement();

            //Write the XML to file and close the writer
            writer.Close();

            Application.Exit();
        }

        private void button_Enter(object sender, EventArgs e)
        {
            //start by changing all the buttons to the default colour
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                    c.BackColor = Color.White;
            }

            //change the current button to the active colour
            Button btn = (Button)sender;
            btn.BackColor = Color.Gold;
        }
    }
}
