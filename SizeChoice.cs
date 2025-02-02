using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Of_Life
{
   
    public partial class SizeChoice : Form
    {
        public static int univerSize = 0;
        public static int timeClicked = 0;
        string information1 = "Choose the size of your Universe: 1 corresponds to a 1x1 grid relative to the screen size, and 10 corresponds to a 4x4 grid relative to the screen size.";
        string information2 = @"                               The cells of the grid can be filled using the mouse. 
                              ""The (+) key fills the cells randomly, while the (-) key removes all living cells to clear the grid. ""
                              ""Ctrl + Mouse Wheel for zooming and De-zooming ""
                                The event starts and stops by pressing the spacebar. Have fun :)";
        public SizeChoice()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                comboBox1.Items.Add(i);
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timeClicked < 1)
            { this.Close(); }
            timeClicked--;
            this.label1.Text = information1;
            this.button1.Text = "Not Gonna,bye";
            this.button2.Text = "Yeah yeah..";
            comboBox1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timeClicked++;
            comboBox1.Visible = false;
            this.label1.Text = information2;
            this.button1.Text = "Go back to size selection";
            this.button2.Text = "Understood, Let's go";
            if (timeClicked > 1) 
            { this.Close(); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            univerSize = comboBox1.SelectedIndex +1;
           
        }
    }
}
