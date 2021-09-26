using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveText
{
    public partial class AnalzingForm : Form
    {
        public AnalzingForm()
        {
            InitializeComponent();
        }
        public RichTextBox rtb_x { get { return richTextBox1; } }
        public RichTextBox rtb_y { get { return richTextBox2; } }
        public RichTextBox rtb_z { get { return richTextBox3; } }
        public kscale prog1 { get { return kscale_progress1; } }
        public kscale prog2 { get { return kscale_progress2; } }
        public kscale progall { get { return kscale1; } }

        private void AnalzingForm_Load(object sender, EventArgs e)
        {

        }

        private void AnalzingForm_Deactivate(object sender, EventArgs e)
        {
        
        }

        private void AnalzingForm_Leave(object sender, EventArgs e)
        {
          
        }
        public void Complete()
        {
            label1.Text = (10000*cleardtimes.X).ToString()+ " text were cleared from momery to save speed";
            label2.Text = (10000 * cleardtimes.Y).ToString() + " text were cleared from momery to save speed";
            label3.Text = (10000 * cleardtimes.Z).ToString() + " text were cleared from momery to save speed";

            button1.Text = "Completed hide now";
            richTextBox1.Enabled = richTextBox2.Enabled = richTextBox3.Enabled = true;
        }
        PointF3D cleardtimes = new PointF3D();
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            if (t.TextLength > 12000)
            {
                t.Text = t.Text.Substring(980, 0);
                label1.Text = "10000 text were cleared from momery to save speed";
                cleardtimes.X += 1;
            }
            else if (t.Text.Length>6000){ label1.Text = ""; }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            if (t.TextLength > 12000)
            {
                t.Text = t.Text.Substring(980, 0);
                label2.Text = "10000 text were cleared from momery to save speed";
                cleardtimes.Y += 1;
            }
            else if (t.Text.Length > 6000) { label2.Text = ""; }
        }

        private void richTextBox3_TextChanged_1(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            if (t.TextLength > 12000)
            {
                t.Text = t.Text.Substring(980, 0);
                label3.Text = "10000 text were cleared from momery to save speed";
                cleardtimes.Z += 1;
            }
            else if (t.Text.Length > 6000) { label3.Text = ""; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
