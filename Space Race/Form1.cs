using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Space_Race
{
    public partial class Form1 : Form
    {
        String gameState = "waiting";

        int stopwatch = 500;

        SoundPlayer xplode = new SoundPlayer(Properties.Resources.engine);
        SoundPlayer point = new SoundPlayer(Properties.Resources.Point);


        Random randGen = new Random();

        int p1Score; //to track player1 scores.
        int p2Score; //to track player2 scores

        int rocketHeight = 40; //to track the rockets height
        int rocketWidth = 10; //to track the rockets width.

        int yP1rocket = 750; // to track player1 Y value.
        int yP2rocket = 750; // to track player2 Y value.

        int xP1rocket = 200; // to track player1 X value.
        int xP2rocket = 600; // to track player2 X value.

        int rocketSpeed = 15; //tracks speed rockets move at.
        int asteroidSize = 10; //tracks size of asteroids.

        bool wDown = false;
        bool sDown = false;
        bool downArrowDown = false;
        bool upArrowDown = false;

        List<int> asteroidX = new List<int>(new int[] { }); //List AsteroidX value.
        List<int> asteroidY = new List<int>(new int[] { }); //List AsteroidY value.
        List<int> direction = new List<int>(new int[] { }); //List Direction
        List<int> asteroidSpeed = new List<int>(); //List Asteroid Speed.

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush redBrush = new SolidBrush(Color.Red);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)

        {
            if (gameState == "waiting")

            {
                titleLabel.Text = "Space Race";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "p1Wins")
            {
                titleLabel.Text = "Player 1 Wins!";
                subTitleLabel.Text = $"Your final score was {p1Score}";
                subTitleLabel.Text += "\nPress Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "p2Wins")
            {
                titleLabel.Text = "Player 2 Wins!";
                subTitleLabel.Text = $"Your final score was {p2Score}";
                subTitleLabel.Text += "\nPress Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "tie")
            {
                titleLabel.Text = "TIE!";
                subTitleLabel.Text = $"Your final score was {p2Score}";
                subTitleLabel.Text += "\nPress Space Bar to Start or Escape to Exit";
            }
            e.Graphics.FillRectangle(redBrush, xP1rocket, yP1rocket, rocketWidth, rocketHeight);
            e.Graphics.FillRectangle(blueBrush, xP2rocket, yP2rocket, rocketWidth, rocketHeight);

            for (int i = 0; i < asteroidX.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, asteroidX[i], asteroidY[i], asteroidSize, asteroidSize);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (gameState == "running")
            {
                //move rockets
                if (wDown == true)
                {
                    yP1rocket -= rocketSpeed;
                }

                if (upArrowDown == true)
                {
                    yP2rocket -= rocketSpeed;
                }
                if (sDown == true)
                {
                    yP1rocket += rocketSpeed;
                }
                if (downArrowDown == true)
                {
                    yP2rocket += rocketSpeed;
                }
                //stopwatch
                stopwatch--;
                //establish asteroids
                if (asteroidY.Count == 0)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        asteroidX.Add(randGen.Next(0, 800));
                        asteroidY.Add(randGen.Next(100, 700));
                        asteroidSpeed.Add(randGen.Next(1, 10));
                        int temporary = randGen.Next(0, 2);
                        if (temporary == 1)
                        {
                            direction.Add(asteroidSpeed[i]);
                        }
                        else
                        {
                            direction.Add(asteroidSpeed[i] * -1);
                        }

                    }

                }
                //asteroid movement
                for (int i = 0; i < asteroidX.Count; i++)
                {
                    asteroidX[i] += direction[i];
                    if (asteroidX[i] > 0)
                    {
                        direction[i] = direction[i] * -1;
                    }
                    if (asteroidX[i] < 800)
                    {
                        direction[i] = direction[i] * -1;
                    }
                }

                //collisions
                Rectangle p1Rocket = new Rectangle(xP1rocket, yP1rocket, rocketWidth, rocketHeight);
                Rectangle p2Rocket = new Rectangle(xP2rocket, yP2rocket, rocketWidth, rocketHeight);

                for (int i = 0; i < 50; i++)
                {
                    Rectangle asteroidBox = new Rectangle(asteroidX[i], asteroidY[i], asteroidSize, asteroidSize);

                    if (p1Rocket.IntersectsWith(asteroidBox))
                    {
                        yP1rocket = 750;
                        xplode.Play();
                    }
                    if (p2Rocket.IntersectsWith(asteroidBox))
                    {
                        yP2rocket = 750;
                        xplode.Play();

                    }
                }

                //score counters
                if (yP1rocket < 0)
                {
                    point.Play();
                    yP1rocket = 750;
                    p1Score++;
                }
                if (yP2rocket < 0)
                {
                    point.Play();
                    yP2rocket = 750;
                    p2Score++;
                }

                if (stopwatch == 0)
                {
                    gameTimer.Enabled = false;
                    if (p1Score > p2Score)
                    {
                        gameState = "p1Wins";
                    }
                    else if (p2Score > p1Score)
                    {
                        gameState = "p2Wins";
                    }
                    else if (p1Score == p2Score)
                    {
                        gameState = "tie";
                    }
                }

                Refresh();
                p1ScoreLabel.Text = $"{p1Score}";
                p2ScoreLabel.Text = $"{p2Score}";
            }

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Space:

                    if (gameState == "waiting" || gameState == "p1Wins" || gameState == "tie" || gameState == "p2Wins")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "p1Wins" || gameState == "tie" || gameState == "p2Wins")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }
        public void GameInitialize()

        {
            p1Score = 0;
            p2Score = 0;
            yP1rocket = 750;
            yP2rocket = 750;
            stopwatch = 500;
            asteroidX.Clear();
            asteroidY.Clear();
            asteroidSpeed.Clear();

            titleLabel.Text = "";

            subTitleLabel.Text = "";

            gameTimer.Enabled = true;

            gameState = "running";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
