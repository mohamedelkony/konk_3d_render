using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace SolveText
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
        }
        EquationSolver eqs = new EquationSolver();
        float ans=0;
        private void kgriditem21_Click(object sender, EventArgs e)
        {

            if (vars!=null)
            {
                equationvalue = this.richBox_Sum.Text;
                 this.DialogResult = DialogResult.OK;
               return;
            }

            this.richTextBox1.Text = "";

            float v = float.NaN;
            try
            {
                v = eqs.SolveEquation(this.richBox_Sum.Text.ToLower(),"ans",ans);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error solving make sure you type correctly \n Details:"+ex.Message+" \n Error Text:"+eqs.ErrorText);
                return;
            }
            if (eqs.ErrorText != "")
            {
                if (eqs.ErrorIndex != -1)
                {
                    richBox_Sum.SelectionStart = eqs.ErrorIndex;
                    richBox_Sum.SelectionLength = 1;
                }
                else
                {
                    MessageBox.Show("Error solving make sure you type correct " + "error in +" + eqs.ErrorText);
                    richBox_Sum.SelectedText = eqs.ErrorText;
                }
              

            }
            else
            {

               this.TextBox_Result.Text = v.ToString();
                ans = v;
            
                foreach (string s in eqs.Steps)
                { this.richTextBox1.Text += s + "\n"; }
            }
        }

        private void kgriditem34_Click(object sender, EventArgs e)
        {
            richBox_Sum.Text = "";
        }

        private void kgriditem19_Click(object sender, EventArgs e)
        {string txt=((Control)sender).Tag.ToString();
            int start = 0;
             if (richBox_Sum.SelectionStart==0)
        {
            richBox_Sum.Text += txt;
            start = richBox_Sum.TextLength;
        }
            else
        {
            start = richBox_Sum.SelectionStart + txt.Length;
 
   richBox_Sum.Text=    richBox_Sum.Text.Insert(richBox_Sum.SelectionStart, txt);
     
        }


             richBox_Sum.Focus();
            richBox_Sum.SelectionLength = 0;
            richBox_Sum.SelectionStart = start;
                 
           richBox_Sum.Refresh();
        }

        private void kgriditem_Del_Click(object sender, EventArgs e)
        {
       
            int ss=richBox_Sum.SelectionStart;
            int lenth = richBox_Sum.SelectionLength;
            int start = 0;
       
            if (ss > 0&&lenth==0)
                    {
                        richBox_Sum.Text = richBox_Sum.Text.Remove(ss - 1, 1);
                     start = richBox_Sum.SelectionStart;
                richBox_Sum.Focus();              
                richBox_Sum.SelectionLength = 0;
               richBox_Sum.SelectionStart = ss - 1;
               
                richBox_Sum.Refresh();
  
            }         
                else
                {
                    richBox_Sum.Text = richBox_Sum.Text.Remove(ss,  lenth);
                   richBox_Sum.Focus();
                   richBox_Sum.SelectionLength = 0;
                   richBox_Sum.SelectionStart = ss;
                   richBox_Sum.Refresh();
  
                }
           
            
           
           
        }
        private string[] vars = null;

        public string[] Vars
        {
            get { return vars; }
            set { vars = value; }
        }
        public string equationvalue = "";
        private void Calculator_Load(object sender, EventArgs e)
        {
            richBox_Sum.ContextMenuStrip = textcontext.make();
    
           richBox_Sum.Text = equationvalue;
            if (vars!=null)
            {
                richBox_Sum.Multiline = true;
                richBox_Sum.Height = 66;
                this.kDynamkPanel2.Visible = false;
                kgriditem38.Enabled = false;
                TextBox_Result.Visible = false;
                this.kDynamkPanel1.Visible = true;
                kswitch_degreessystem.Value = false;
                kswitch_degreessystem.Enabled = false;
                kgriditem21.Text = "OK";
                kgriditem21.Font = this.Font;
                for (int i = 0; i < vars.Length; i++)
                {
                    kgriditem kgi = new kgriditem(vars[i]);
                    kgi.Size = new Size(60, 50);
                    kgi.ShowImage = false;
                    kgi.Selectable = false;
                    kgi.Theme = KitemThemes.light;
                    kgi.Paintstyle = KitemPaintStyle.DoubleFlat;
                    kgi.Tag = kgi.Text;
                    kgi.Font = kgriditem12.Font;
                    kgi.Click += new EventHandler(kgriditem19_Click);
                    kgrid1.Items.Add(kgi);
                }
            }
        }
      
        private void kswitch2_ValueChanged(object sender, booleventargs e)
        {
            eqs.Isdegreee = e.Value;
        }

        private void textBoxfx_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

      

       

        private void kgriditem39_Click(object sender, EventArgs e)
        {
          
           this.DialogResult = DialogResult.Cancel;
            this.Close(); 
            if (!this.IsDisposed)
            {
         //     this.Dispose();
            }
        }

        private void kgriditem40_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        bool   ndrag;
   int  nmousex ;
   int nmousey;


  
        private void label4_MouseDown(object sender, MouseEventArgs e)
        {

              ndrag =true;
        nmousex = MousePosition.X - this.Left;
        nmousey = MousePosition.Y - this.Top;
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
 if (ndrag == true)
       {
           this.Top = MousePosition.Y - nmousey;
           this.Left = MousePosition.X - nmousex;
       }
        }

        private void label4_MouseUp(object sender, MouseEventArgs e)
        {
            ndrag = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox_Sum_Enter(object sender, EventArgs e)
        {
          
        }

        private void textBox_Sum_Leave(object sender, EventArgs e)
        {
           
        }

        private void textBox_Sum_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void textBox_Sum_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
          fnc.ColorMathEquation(sender as RichTextBox, vars);
            RichTextBox rtb = richBox_Sum;
            bool isright = true;
            try
            {
                if (vars != null)
                {
                    float[] vals = new float[vars.Length];
                    for (int i = 0; i < vars.Length; i++)
                    { vals[i] = 1; }
                   
                    EquationSolver.SolveEQ(rtb.Text, Vars, vals, false);
                }
                else
                {
                    EquationSolver.SolveEQ(rtb.Text, false);

                }
                isright = true;
            }
            catch (MathException me)
            {
                isright = true;
            }
            catch (Exception ex)
            {
                isright = false;
              
            }
            if (isright)
            {
                if (rtb.Focused)
                {
                    rtb.BackColor = Color.Yellow;
                }
                else
                {
                    rtb.BackColor = Color.White;
                }
            }
            else
            {
                rtb.BackColor = Color.Pink;
            }
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2_TextChanged(null, null);
        }

        private void TextBox_Result_TextChanged(object sender, EventArgs e)
        {

        }

      
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
