using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DVD : PictureBox
    {

        public bool xRightMove;
        public bool yUpMove;
        public bool mooving;
        public bool skip;

        public DVD() {
            Random rnd = new Random();

            xRightMove = Convert.ToBoolean(System.DateTime.Now.Millisecond % 2);
            yUpMove = Convert.ToBoolean(System.DateTime.Now.Millisecond % 7 > 3);
            mooving = false;
        }

        public void moveEvent() {
            //855, 429
            if (this.Top >= 429)
                yUpMove = true;

            if (this.Top <= 24)
                yUpMove = false;

            if (this.Left >= 855)
                xRightMove = false;

            if (this.Left <= 0)
                xRightMove = true;


            this.Left += ((xRightMove) ? 1 : -1) * 1;
            this.Top += ((yUpMove) ? -1 : 1) * 1;
        }

    }


    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
