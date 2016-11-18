using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MouseMover
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Move Cursor 1 Pixel (x,y)
            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
            
            Application.Run();
        }

        //Key Pressed
        private void KeyPress(object sender, KeyEventArgs e)
        {
            //Crtl + W: pressed
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
            {

            }
        }
    }
}
