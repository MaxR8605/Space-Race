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
    public partial class SpaceRace : Form
    {
        Rectangle player1 = new Rectangle(175, 0, 25, 40);
        Rectangle player2 = new Rectangle(550, 0, 25, 40);

        List<Rectangle> obstacles = new List<Rectangle>();
        List<int> type = new List<int>();
        List<int> speed = new List<int>();
        int size = 0;

        bool wDown = false;
        bool sDown = false;
        bool upDown = false;
        bool downDown = false;
        bool moveable = false;

        int p1Score = 0;
        int p2Score = 0;
        int timeLeft = 2000;

        SolidBrush smallColour = new SolidBrush(Color.PaleTurquoise);
        SolidBrush medColour = new SolidBrush(Color.PaleGreen);
        SolidBrush bigColour = new SolidBrush(Color.Khaki);
        SolidBrush p1Colour = new SolidBrush(Color.DodgerBlue);
        SolidBrush p2Colour = new SolidBrush(Color.Red);
        SolidBrush white = new SolidBrush(Color.White);
        SolidBrush black = new SolidBrush(Color.Black);

        Random random = new Random();
        int randValue = 0;

        SoundPlayer hitSound = new SoundPlayer(Properties.Resources.hit);
        SoundPlayer pointSound = new SoundPlayer(Properties.Resources.point);

        public SpaceRace()
        {
            InitializeComponent();
            player1.Y = this.Height - 50;
            player2.Y = this.Height - 50;

            titleLabel.Text = "Space Race";
        }

        private void SpaceRace_KeyDown(object sender, KeyEventArgs e)
        {
            if (moveable == true)
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
                        upDown = true;
                        break;
                    case Keys.Down:
                        downDown = true;
                        break;
                }
            }
        }

        private void SpaceRace_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move player 1
            
            if (wDown == true)
            {
                player1.Y -= 8;

                if (player1.Y < 0 - player1.Height)
                {
                    player1.Y = this.Height;
                    p1Score++;
                    p1ScoreLabel.Text = p1Score.ToString();
                    pointSound.Play();
                }
            }
            if (sDown == true)
            {
                if (player1.Y < this.Height - player1.Height)
                {
                    player1.Y += 8;
                }
            }

            // Move player 2

            if (upDown == true)
            {
                player2.Y -= 8;

                if (player2.Y < 0 - player2.Height)
                {
                    player2.Y = this.Height;
                    p2Score++;
                    p2ScoreLabel.Text = p2Score.ToString();
                    pointSound.Play();
                }
            }
            if (downDown == true)
            {
                if (player2.Y < this.Height - player2.Height)
                {
                    player2.Y += 8;
                }
            }

            // Move obstacles

            for (int i = 0; i < obstacles.Count(); i++)
            {
                int x = obstacles[i].X + speed[i];

                obstacles[i] = new Rectangle(x, obstacles[i].Y, obstacles[i].Width, obstacles[i].Height);
            }

            // Generate a random value

            randValue = random.Next(1, 101);

            // Generate an obstacle if it is time

            randValue = random.Next(1, 101);
            if (randValue < 5)
            {
                type.Add(random.Next(5, 7));
                size = random.Next(1, 3) * 5;
            }
            else if (randValue < 30)
            {
                type.Add(random.Next(3, 5));
                size = random.Next(3, 5) * 5;
            }
            else
            {
                type.Add(random.Next(1, 3));
                size = random.Next(5, 7) * 5;
            }

            if (randValue < 16)
            {
                obstacles.Add(new Rectangle(-30, random.Next(0, this.Height - 80), size, size));
                speed.Add(random.Next(3, 6));
            }
            else if (randValue < 31)
            {
                obstacles.Add(new Rectangle(this.Width, random.Next(0, this.Height - 80), size, size));
                speed.Add(random.Next(-6, -3));
            }

            // Remove obstacle if it goes off the screen

            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].X > this.Width + 100 || obstacles[i].X < -100)
                {
                    obstacles.RemoveAt(i);
                    speed.RemoveAt(i);
                    type.RemoveAt(i);

                    moveable = true;
                }

                if (player1.IntersectsWith(obstacles[i]))
                {
                    player1.Y = this.Height - 50;

                    obstacles.RemoveAt(i);
                    speed.RemoveAt(i);
                    type.RemoveAt(i);

                    hitSound.Play();
                }

                if (player2.IntersectsWith(obstacles[i]))
                {
                    player2.Y = this.Height - 50;

                    obstacles.RemoveAt(i);
                    speed.RemoveAt(i);
                    type.RemoveAt(i);

                    hitSound.Play();
                }
            }

            if (moveable == true)
            {
                timeLeft--;
            }

            if (p1Score == p2Score && timeLeft < 0 && timeLeft > -100)
            {
                titleLabel.Text = "Overtime!";
                titleLabel.Visible = true;
            }
            else
            {
                titleLabel.Visible = false;
            }

            if (timeLeft <= 0 && p1Score != p2Score)
            {
                gameTimer.Stop();
                startButton.Enabled = true;
                startButton.Visible = true;
                titleLabel.Visible = true;
                
                pointSound.Play();

                if (p1Score > p2Score)
                {
                    titleLabel.Text = "Player One Wins";
                }
                else
                {
                    titleLabel.Text = "Player Two Wins";
                }

                startButton.Text = "Play Again";
            }
            else if (timeLeft < 0)
            {
                this.BackColor = Color.Maroon;
                black.Color = Color.Maroon;
            }

            Refresh();
        }

        private void SpaceRace_Paint(object sender, PaintEventArgs e)
        {
            // Draw players
            e.Graphics.FillRectangle(p1Colour, player1);
            e.Graphics.FillRectangle(p1Colour, player1.X - 2, player1.Y + 20, 10, 26);
            e.Graphics.FillRectangle(p1Colour, player1.X + 17, player1.Y + 20, 10, 26);
            e.Graphics.FillEllipse(black, player1.X, player1.Y + 35, 25, 25);
            e.Graphics.FillEllipse(black, player1.X - 15, player1.Y - 10, 24, 24);
            e.Graphics.FillEllipse(black, player1.X + 15, player1.Y - 10, 24, 24);
            e.Graphics.FillEllipse(black, player1.X + 5, player1.Y + 10, 14, 14);

            e.Graphics.FillRectangle(p2Colour, player2);
            e.Graphics.FillRectangle(p2Colour, player2.X - 2, player2.Y + 20, 10, 26);
            e.Graphics.FillRectangle(p2Colour, player2.X + 17, player2.Y + 20, 10, 26);
            e.Graphics.FillEllipse(black, player2.X, player2.Y + 35, 25, 25);
            e.Graphics.FillEllipse(black, player2.X - 15, player2.Y - 10, 24, 24);
            e.Graphics.FillEllipse(black, player2.X + 15, player2.Y - 10, 24, 24);
            e.Graphics.FillEllipse(black, player2.X + 5, player2.Y + 10, 14, 14);

            // Draw obstacles

            for (int i = 0; i < obstacles.Count(); i++)
            {
                switch (type[i])
                {
                    case 1:
                        e.Graphics.FillRectangle(smallColour, obstacles[i].X, obstacles[i].Y, 5, 5);
                        break;
                    case 2:
                        e.Graphics.FillRectangle(smallColour, obstacles[i].X, obstacles[i].Y, 10, 10);
                        break;
                    case 3:
                        e.Graphics.FillRectangle(medColour, obstacles[i].X, obstacles[i].Y, 15, 15);
                        break;
                    case 4:
                        e.Graphics.FillRectangle(medColour, obstacles[i].X, obstacles[i].Y, 20, 20);
                        break;
                    case 5:
                        e.Graphics.FillRectangle(bigColour, obstacles[i].X, obstacles[i].Y, 25, 25);
                        break;
                    case 6:
                        e.Graphics.FillRectangle(bigColour, obstacles[i].X, obstacles[i].Y, 30, 30);
                        break;
                }
            }

            // Draw time bar
            float barHeight = (float)timeLeft / 2000 * this.Height;
            if (timeLeft < 251)
            {
                e.Graphics.FillRectangle(bigColour, this.Width / 2 - 10, this.Height - barHeight, 20, barHeight);
            }
            else if (timeLeft < 501)
            {
                e.Graphics.FillRectangle(medColour, this.Width / 2 - 10, this.Height - barHeight, 20, barHeight);
            }
            else if (timeLeft < 1001)
            {
                e.Graphics.FillRectangle(smallColour, this.Width / 2 - 10, this.Height - barHeight, 20, barHeight);
            }
            else
            {
                e.Graphics.FillRectangle(white, this.Width / 2 - 10, this.Height - barHeight, 20, barHeight);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            titleLabel.Visible = false;
            startButton.Visible = false;
            startButton.Enabled = false;
            gameTimer.Enabled = true;
            p1ScoreLabel.Visible = true;
            p2ScoreLabel.Visible = true;
            moveable = false;

            p1Score = 0;
            p2Score = 0;
            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";
            timeLeft = 2000;
            player1.Y = this.Height - 50;
            player2.Y = this.Height - 50;

            obstacles.Clear();
            type.Clear();
            speed.Clear();

            this.BackColor = Color.Black;
            black.Color = Color.Black;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
