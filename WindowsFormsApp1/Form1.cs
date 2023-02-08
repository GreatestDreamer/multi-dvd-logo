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


namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {

        List<DVD> DVDs = new List<DVD>();
        DVD selectedDVD;

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void updateCountOfDVDs() {
            label2.Text = Convert.ToString(DVDs.Count);
        }


        public bool inRange(int i, int min, int max) {
            return (i >= min && i <= max);
        }

        private bool checkAvailability(Point p, bool itSelf = false) {
            if (DVDs == null)
                return true;

            foreach (DVD i in DVDs) {
                if (
                    (inRange(p.X, i.Location.X, i.Location.X + 255) || inRange(p.X + 255, i.Location.X, i.Location.X + 255))
                    &&
                    (inRange(p.Y, i.Location.Y, i.Location.Y + 126) || inRange(p.Y + 126, i.Location.Y, i.Location.Y + 126))
                    )
                    if (itSelf)
                        itSelf = false;
                    else
                        return false;
            }
            return true;
        }

        private Point getPointWithoutColizion() {
            Random rnd = new Random();

            Point p;
            p = new Point(rnd.Next(0, 855), rnd.Next(24, 429));

            for (int i = 0; i != 15; i++) //tries 15 times
            {

                if (checkAvailability(p))
                    return p;
                else
                    p = new Point(rnd.Next(0, 855), rnd.Next(24, 429));

            }
            return new Point(-1000, -1000);
        }

        private void selectDVD(object sender, EventArgs e) {
            
            if (selectedDVD != null)
                selectedDVD.BackColor = Color.FromArgb(0,0,0);
            
            selectedDVD = (DVD) sender;

            selectedDVD.BackColor = Color.FromArgb(36,36,36);
            startToolStripMenuItem.Enabled = !selectedDVD.mooving;
            stopToolStripMenuItem.Enabled = selectedDVD.mooving;
            destroyToolStripMenuItem.Enabled = true;
        }

        public void spawnDVD(PictureBox originalPictureBox)
        {
            DVD pb = new DVD();
            //Copy properties 
            pb.Size = new Size(255, 126);
            pb.BackColor = originalPictureBox.BackColor;
            pb.BackgroundImage = originalPictureBox.BackgroundImage;
            pb.Location = getPointWithoutColizion(); // min y = 24; min x = 0; max y = 429; max x = 855; 

            if (pb.Location.X < 0) {
                MessageBox.Show(
                    "Мы наткнулись на коллизию объектов и не смогли заспавнить DVD лого :(",
                    "Ошибка при спавне",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            pb.Click += new System.EventHandler(selectDVD);

            DVDs.Add(pb);
            this.Controls.Add(pb);
            updateCountOfDVDs();
        }

        public void destroyDVD(DVD d) {
            DVDs.Remove(d);
            this.Controls.Remove(d);
            updateCountOfDVDs();
        }

        private DVD findDVDByPoint(Point p) {
            
            foreach (DVD i in DVDs)
            {
                if (i.skip)
                    continue;
                if (
                    (inRange(p.X, i.Location.X, i.Location.X + 255) || inRange(p.X + 255, i.Location.X, i.Location.X + 255))
                    &&
                    (inRange(p.Y, i.Location.Y, i.Location.Y + 126) || inRange(p.Y + 126, i.Location.Y, i.Location.Y + 126))
                    ) return i;
            }
            return new DVD();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DVD d in DVDs)
            {
                if (!d.mooving)
                    continue;
                d.moveEvent();
                d.skip = true;

                Point p = d.Location;

                if (!checkAvailability(p, true) && checkAvailability(new Point(p.X - ((d.xRightMove) ? 1 : -1), p.Y), true))
                {
                    Console.WriteLine("get hited X");
                    d.xRightMove = !d.xRightMove;
                    
                    DVD hitedDVD = findDVDByPoint(p);
                    hitedDVD.xRightMove = !hitedDVD.xRightMove;

                }

                if (!checkAvailability(p, true) && checkAvailability(new Point(p.X, p.Y - ((d.yUpMove) ? -1 : 1)), true))
                {
                    Console.WriteLine("get hited Y");
                    d.yUpMove = !d.yUpMove;

                    DVD hitedDVD = findDVDByPoint(p);
                    hitedDVD.yUpMove= !hitedDVD.yUpMove;

                }
                d.skip = false;

            }
        }


        private void spawnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spawnDVD(pictureBox1);
        }

        private void destroyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = false;
            destroyToolStripMenuItem.Enabled = false;
            destroyDVD(selectedDVD);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedDVD.mooving = true;
            startToolStripMenuItem.Enabled = !selectedDVD.mooving;
            stopToolStripMenuItem.Enabled = selectedDVD.mooving;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedDVD.mooving = false;
            startToolStripMenuItem.Enabled = !selectedDVD.mooving;
            stopToolStripMenuItem.Enabled = selectedDVD.mooving;
        }

        private void destroyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DVD i in DVDs.ToArray())
                destroyDVD(i);


            updateCountOfDVDs();
        }

        private void startAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DVD i in DVDs)
                i.mooving = true;
        }
    }
}
