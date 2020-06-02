using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace clickout
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        int cursorX = 5;
        int cursorY = 5;

        int score = 0;

        public Form1()
        {
            InitializeComponent();
            labelPause.Visible = false;
            Cursor.Hide();
        }

        private void win()
        {
            timer1.Stop();
            Cursor.Show();
            Cursor.Clip = Rectangle.Empty;
            MessageBox.Show("You Won!\nScore: " + score);
        }

        private void lost()
        {
            timer1.Stop();
            Cursor.Show();
            Cursor.Clip = Rectangle.Empty;
            MessageBox.Show("You Lost :(\nScore: " + score);
            Application.Restart();
        }

        private void pause()
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                labelPause.Visible = true;
                Cursor.Show();
                Cursor.Clip = Rectangle.Empty;
            }
            else
            {
                Cursor.Position = new Point(player.Location.X, Cursor.Position.Y);
                timer1.Start();
                labelPause.Visible = false;
                Cursor.Hide();
                Cursor.Clip = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            }
        }

        private void Check(Control control)
        {
            (control as CheckBox).Checked = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cursor.Left += cursorX;
            cursor.Top += cursorY;

            labelScore.Text = "Score: " + score;

            if (cursor.Left + cursor.Width > ClientSize.Width || cursor.Left < 0)
            {
                cursorX = -cursorX;
            }

            if (cursor.Top < 0 || cursor.Bounds.IntersectsWith(player.Bounds))
            {
                cursorY = -cursorY;
            }

            if (cursor.Top + cursor.Height > ClientSize.Height)
            {
                lost();
            }

            foreach (Control x in this.Controls)
            {
                if (x is CheckBox && x.Tag == "check")
                {
                    if (cursor.Bounds.IntersectsWith(x.Bounds))
                    {
                        Check(x);
                        Thread.Sleep(50);
                        this.Controls.Remove(x);
                        cursorY = -cursorY;
                        score++;
                    }
                }
            }

            if (score > 251)
            {
                win();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape | e.KeyData == Keys.Space)
            {
                pause();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled)
            {
                Point pos = this.PointToClient(Cursor.Position);
                player.Location = new Point(pos.X, 481);
            }
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Clip = this.Bounds;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/TassiloBalbo/clickout");
        }
    }
}
