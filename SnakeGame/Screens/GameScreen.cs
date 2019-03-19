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

namespace SnakeGame
{
    public partial class GameScreen : UserControl
    {
        Random randGen = new Random();


        Rectangle food; // stores the size and location of food pellet

        List<Wall> walls = new List<Wall>(); //list containing the 4 walls
        List<Body> bodyParts = new List<Body>(); //list of all body parts
        List<Point> beenPoints = new List<Point>(); //list of all the places the snake head has been to


        string direction = "right";  //current snake head direction
        string directionLast = "";  // stores the previous to check for valid next move

        int snakeSize = 20;
        int snakeSpeed = 20;
        int snakeLength = 0;

        //various graphics objects used to draw on screen
        SolidBrush snakeBrush = new SolidBrush(Color.White);
        Pen pen = new Pen(Color.Red);
        Font font = new Font("Courier New", 12);

        public GameScreen()
        {
            InitializeComponent();
            InitializeGameValues();
        }

        public void InitializeGameValues()
        {
            // setup walls for game
            walls.Add(new Wall(0, 40, this.Width, 10)); //top wall
            walls.Add(new Wall(0, 40, 10, this.Height)); //left wall
            walls.Add(new Wall(0, this.Height - 10, this.Width, 10)); //bottom wall
            walls.Add(new Wall(this.Width - 10, 40, 10, this.Height)); //right wall

            //add the first body part
            bodyParts.Add(new Body(100, 100, snakeSize));
            snakeLength++;

            //add the inital food location
            food = new Rectangle(350, 105, 10, 10);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // opens a pause screen is escape is pressed. Depending on what is pressed
            // on pause screen the program will either continue or exit to main menu
            if (e.KeyCode == Keys.Escape && gameTimer.Enabled)
            {
                gameTimer.Enabled = false;

                DialogResult result = PauseForm.Show();

                if (result == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (result == DialogResult.Abort)
                {
                    MainForm.ChangeScreen(this, "MenuScreen");
                }
            }

            directionLast = direction; //store the previous direction to determine if new move is ok

            switch (e.KeyCode)
            {    
                case Keys.Left:
                    direction = "left";
                    break;
                case Keys.Down:
                    direction = "down";
                    break;
                case Keys.Right:
                    direction = "right";
                    break;
                case Keys.Up:
                    direction = "up";
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // checks to see if the new move goes back on the body of the snake
            // and moves the snake if it does not
            if (directionLast == "right" && direction == "left" ||
                directionLast == "down" && direction == "up" ||
                directionLast == "left" && direction == "right" ||
                directionLast == "up" && direction == "down")
            {
                GameOver();
            }
            else
            {
                // move the head of the snake and add the new location for 
                // the head of the snake to a list of previous locations
                bodyParts[0].MoveHead(direction, snakeSpeed); 
                beenPoints.Add(new Point(bodyParts[0].x, bodyParts[0].y));

                // only keep as many previous positions as there are body parts
                if (beenPoints.Count > bodyParts.Count) { beenPoints.RemoveAt(0); }

                // move the body parts to the position that was occupied by the previous part
                for (int i = 1; i < bodyParts.Count; i++)
                {
                    bodyParts[i].Move(beenPoints[i - 1]);
                }
            }
            
            // checks if the head ate a food pellet
            if (bodyParts[0].Collision(food))
            {
                bodyParts.Add(new Body(beenPoints[beenPoints.Count - 1].X, beenPoints[beenPoints.Count - 1].Y, snakeSize));
                snakeLength++;

                food.X = randGen.Next(20, Width - 40);
                food.Y = randGen.Next(60, Height - 40);
            }

            // check if head ran into a wall
            foreach(Wall w in walls)
            {
                if(bodyParts[0].Collision(w) )
                {
                    GameOver();
                    break;
                }
            }

            //checks for collision of head with other body parts. Start at index 1 to skip checking head against itself,
            //and also skip last one as when a new part is created it initially has the same x,y as the head
            for (int i = 1; i < bodyParts.Count -2; i++)
            {
                if (bodyParts[0].Collision(bodyParts[i]))
                {
                    GameOver();
                    break;
                }
            }
            //calls the GameScreen_Paint method to draw the screen.
            Refresh();
        }

        public void GameOver()
        {
            //stop the game and display the screen briefly before moving to score screen
            gameTimer.Enabled = false;
            Refresh();
            System.Threading.Thread.Sleep(1000);           

            //add current score to scorelist
            HighScore hs = new HighScore("temp", snakeLength);
            MainForm.scoreList.Add(hs);

            //sort the scores list and then add reverse order to show highest first
            MainForm.scoreList.Sort((x, y) => x.score.CompareTo(y.score));
            MainForm.scoreList.Reverse();

            //only keep top 5 scores
            if (MainForm.scoreList.Count == 6)
                MainForm.scoreList.RemoveAt(5); 

            MainForm.ChangeScreen(this, "ScoreScreen");
        }

        //Everything that is to be drawn on the screen should be done here
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw score
            e.Graphics.DrawString("SNAKE LENGTH - " + snakeLength, font, snakeBrush, 10, 10);

            //draw high score
            if (MainForm.scoreList[0].score > snakeLength)
                e.Graphics.DrawString("HIGH SCORE - " + MainForm.scoreList[0].score, font, snakeBrush, 550, 10);
            else
                e.Graphics.DrawString("HIGH SCORE - " + snakeLength, font, snakeBrush, 550, 10);
            
            //draw body parts
            foreach(Body b in bodyParts)
            {
                e.Graphics.FillRectangle(snakeBrush, b.x, b.y, b.size, b.size);
                e.Graphics.DrawRectangle(pen, b.x, b.y, b.size, b.size);
            }

            //draw walls
            foreach(Wall w in walls)
            {
                e.Graphics.FillRectangle(snakeBrush, w.x, w.y, w.width, w.height);
            }

            //draw food
            e.Graphics.FillEllipse(snakeBrush, food);
        }
    }

}
