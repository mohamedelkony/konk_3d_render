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
    public partial class Form_TextureModle : Form
    {
        public Form_TextureModle()
        {
            InitializeComponent();
        }   List<ColorEquation> colorexpmles = new List<ColorEquation>();
        List<RectangleF> imgexpmles = new List<RectangleF>();
     
        public TextureModel TextureModleValue
        {
            get
            {
                TextureModel v = new TextureModel();
                v.Image =(Bitmap) kPictureBox1.BackgroundImage;
                v.ImageSelectionRec = kPictureBox1.Selectionrec;
                v.IsImageFill = tabControl1.SelectedIndex == 0 ? false : true;
                v.GridDomain = new Point((int)kscale_griddomain.MinValue,(int) kscale_griddomain.MaxValue);
                v.PlanDomain = new Point((int)kscale_plandomain.MinValue,(int) kscale_plandomain.MaxValue);
                v.colorequation=new ColorEquation(textBox_r.Text,textBox_g.Text,textBox_b.Text,textbox_a.Text);
               
                return v;
            }
            set
            {
                if (value.Image!=null)
                {
                    kPictureBox1.BackgroundImage = value.Image;
                }
                kPictureBox1.Selectionrec = value.ImageSelectionRec;
                kscale_griddomain.Pointers[0].ValueF = value.GridDomain.X; kscale_griddomain.Pointers[1].ValueF = value.GridDomain.Y;
                kscale_plandomain.Pointers[0].ValueF = value.PlanDomain.X; kscale_plandomain.Pointers[1].ValueF = value.PlanDomain.Y;
                textBox_r.Text = value.colorequation.Red;
                textBox_g.Text = value.colorequation.Green;
                textBox_b.Text = value.colorequation.Blue;
                textbox_a.Text = value.colorequation.Alpha;
                tabControl1.SelectedIndex = value.IsImageFill == false ? 0 : 1;
            }
        }
       

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox_r.ContextMenuStrip = textBox_g.ContextMenuStrip = textBox_b.ContextMenuStrip = this.textbox_a.ContextMenuStrip = textcontext.make();
           
            kDynamkPanel3.Location = new Point();
            this.Size = kDynamkPanel3.Size;
            ColorEquation cq1 = new ColorEquation("x%", "y%", "z%");
            ColorEquation cq15 = new ColorEquation("(sin(2*u)+1)*50", "abs((sin(2*v))*100)", "(cos(2*u)+1)*50");
            ColorEquation cq16 = new ColorEquation("(sin(u)+1)*50", "abs((sin(v)+1)*50)", "(cos(u)+1)*50");          
            ColorEquation cq2 = new ColorEquation("(sin(4*u)+1)*50", "(cos(4*u)+1)*50", "(cos(4*v)+1)*25");
            ColorEquation cq17 = new ColorEquation("(sin(2*u)+1)*50", "(sin(v)+1)*50", "(cos(2*u)+1)*50");
            ColorEquation cq18 = new ColorEquation("(sin(12*u)+1)*50", "(cos(12*u)+1)*50", "0");
            ColorEquation cq3 = new ColorEquation("(sin(u)+1)*50", "(cos(u)+1)*50", "0");
            ColorEquation cq4 = new ColorEquation("(sin(u/2)+1)*50", "(cos(v*4)+1)*50", "(cos(u/2)+1)*50");
            ColorEquation cq5 = new ColorEquation("(sin(2*u)+1)*50", "(sin(u-v)+1)*50", "(sin(u)+1)*25");
            ColorEquation cq6 = new ColorEquation("0", "(cos(8*v)+1.2)*40", "0");
            ColorEquation cq7 = new ColorEquation("(sin(u/2)+1)*50", "(cos(v)+1)*50", "(sin(u/2-v)+1)*50");
            ColorEquation cq8 = new ColorEquation("(sin(u*2)+1)*50", "(sin(v*2)+1)*50", "0");
            ColorEquation cq9 = new ColorEquation("(sin(u/4-v)+1)*50", "(cos(v)+1)*50", "(cos(u/4+v)+1)*50");
            ColorEquation cq10 = new ColorEquation("(sin(u*2)+1)*50", "(sin(v)+1)*50", "(cos(u*2)+1)*50");
            ColorEquation cq11 = new ColorEquation("(sin(u/4)+1)*50", "(cos(v*4+4*u)+1)*50", "(cos(u/4)+1)*50");
            ColorEquation cq12 = new ColorEquation("(sin(u-v)+1)*50", "(cos(v)+1)*50", "(sin(u+v)+1)*50");         
            ColorEquation cq13 = new ColorEquation("abs(u)*20", "abs(v)*20", "0"); ;
            ColorEquation cq19 = new ColorEquation("(sin(5/3 * u)+1)*50*sin(v)", "(cos(5/3 * u)+1)*50*sin(v)", "(cos(5/3 * u + 3))*50*sin(v)");
            ColorEquation cq20 = new ColorEquation("100", "100", "100"); ;
            ColorEquation cq14 = new ColorEquation("100", "30", "0"); ;
            ColorEquation cq21 = new ColorEquation("(cos(8*u)+1.2)*40", "(cos(8*v)+1.2)*40", "0"); ;



            colorexpmles.AddRange(new ColorEquation[] { cq1,cq15,cq16, cq2, cq3, cq4, cq5,cq17,cq18, cq6, cq7, cq8, cq9, cq10, cq11, cq12, cq13, cq19,cq20,cq14 ,cq21});
            imgexpmles.AddRange(new RectangleF[] {new RectangleF(0,0,100,100),new RectangleF(25,25,50,50), new RectangleF(0,0,50,50),new RectangleF(0,50,50,50),new 
            RectangleF(50,0,50,50),new RectangleF(10,10,80,80),new RectangleF(30,30,35,35)});
       
            for (int i = 0; i < colorexpmles.Count; i++)
            {
                Bitmap b = null;
              switch(i)
                {
                    case 0:
                        b = EquationGrapher3D.Properties.Resources.cq1;
                        break;
                    case 1:
                        b = EquationGrapher3D.Properties.Resources.cq2;
                        break;
                    case 2:
                        b = EquationGrapher3D.Properties.Resources.cq3;
                        break;
                    case 3:
                        b = EquationGrapher3D.Properties.Resources.cq4;
                        break;
                    case 4:
                        b = EquationGrapher3D.Properties.Resources.cq5;
                        break;
                    case 5:
                        b = EquationGrapher3D.Properties.Resources.cq6;
                        break;
                    case 6:
                        b = EquationGrapher3D.Properties.Resources.cq7;
                        break;
                    case 7:
                        b = EquationGrapher3D.Properties.Resources.cq8;
                        break;
                    case 8:
                        b = EquationGrapher3D.Properties.Resources.cq9;
                        break;
                    case 9:
                        b = EquationGrapher3D.Properties.Resources.cq10;
                        break;
                    case 10:
                        b = EquationGrapher3D.Properties.Resources.cq11;
                        break;
                    case 11:
                        b = EquationGrapher3D.Properties.Resources.cq12;
                        break;
                    case 12:
                        b = EquationGrapher3D.Properties.Resources.cq13;
                        break;
                    case 13:
                        b = EquationGrapher3D.Properties.Resources.cq14;
                        break;
                    case 14:
                        b = EquationGrapher3D.Properties.Resources.cq15;
                        break;
                    case 15:
                        b = EquationGrapher3D.Properties.Resources.cq16;
                        break;
                    case 16:
                        b = EquationGrapher3D.Properties.Resources.cq17;
                        break;
                    case 17:
                        b = EquationGrapher3D.Properties.Resources.cq18;
                        break;
                    case 18:
                        b = EquationGrapher3D.Properties.Resources.cq19;
                        break;
                    case 19:
                        b = EquationGrapher3D.Properties.Resources.cq20;
                        break;
                    case 20:
                        b = EquationGrapher3D.Properties.Resources.cq21;
                        break;                 
                }
                kcombobox1.Items.Add(new comboitem("", colorexpmles[i],b));
            }
            for (int i = 0; i < imgexpmles.Count; i++)
            {
                kcombobox2.Items.Add(new comboitem("P"+(i+1).ToString(), imgexpmles[i]));
            }
      this.kcombobox_images.Items.Add(new comboitem("flat grady", EquationGrapher3D.Properties.Resources.images_6));
              this.kcombobox_images.Items.Add(new comboitem("WorldMap", EquationGrapher3D.Properties.Resources.highrelotinedworldmap));
            this.kcombobox_images.Items.Add(new comboitem("grady2", EquationGrapher3D.Properties.Resources.images_12));
      
         this.kcombobox_images.Items.Add(new comboitem("Egypt", EquationGrapher3D.Properties.Resources.egypt_flagicon));
      
    this.kcombobox_images.Items.Add(new comboitem("grady15", EquationGrapher3D.Properties.Resources.stars_image_backgr));
        this.kcombobox_images.Items.Add(new comboitem("Obama", EquationGrapher3D.Properties.Resources.face_PNG5660));
      this.kcombobox_images.Items.Add(new comboitem("grady6", EquationGrapher3D.Properties.Resources.images_37));
       this.kcombobox_images.Items.Add(new comboitem("Face", EquationGrapher3D.Properties.Resources.m01_32_gr));
  
       this.kcombobox_images.Items.Add(new comboitem("grady9", EquationGrapher3D.Properties.Resources.images_55));
        this.kcombobox_images.Items.Add(new comboitem("HSl", EquationGrapher3D.Properties.Resources.HSLL128));
       

    
     this.kcombobox_images.Items.Add(new comboitem("grady1", EquationGrapher3D.Properties.Resources.images_11));
  
      
          
   
       this.kcombobox_images.Items.Add(new comboitem("grady4", EquationGrapher3D.Properties.Resources.images_25));


            this.kcombobox_images.Items.Add(new comboitem("Sun ", EquationGrapher3D.Properties.Resources.w1));
            this.kcombobox_images.Items.Add(new comboitem("Sky ", EquationGrapher3D.Properties.Resources.w2));
            this.kcombobox_images.Items.Add(new comboitem("Sun 2", EquationGrapher3D.Properties.Resources.w3));
            this.kcombobox_images.Items.Add(new comboitem("Wood", EquationGrapher3D.Properties.Resources.w4));

        
            this.kcombobox_images.Items.Add(new comboitem("Glass", EquationGrapher3D.Properties.Resources.w6));
            this.kcombobox_images.Items.Add(new comboitem("Sky 2", EquationGrapher3D.Properties.Resources.w7));
            this.kcombobox_images.Items.Add(new comboitem("Sky 3", EquationGrapher3D.Properties.Resources.w8));

            this.kcombobox_images.Items.Add(new comboitem("Glass 2", EquationGrapher3D.Properties.Resources.w9));
            this.kcombobox_images.Items.Add(new comboitem("Dark Sky", EquationGrapher3D.Properties.Resources.w10));
            this.kcombobox_images.Items.Add(new comboitem("Glass 3", EquationGrapher3D.Properties.Resources.w11));
            this.kcombobox_images.Items.Add(new comboitem("Brick", EquationGrapher3D.Properties.Resources.w12));

            this.kcombobox_images.Items.Add(new comboitem("Wood 3", EquationGrapher3D.Properties.Resources.w13));
            this.kcombobox_images.Items.Add(new comboitem("Brick 2", EquationGrapher3D.Properties.Resources.w14));
            this.kcombobox_images.Items.Add(new comboitem("Brick 3", EquationGrapher3D.Properties.Resources.w15));
            this.kcombobox_images.Items.Add(new comboitem("Glass 4", EquationGrapher3D.Properties.Resources.w16));

            this.kcombobox_images.Items.Add(new comboitem("Sun 3", EquationGrapher3D.Properties.Resources.w17));
            this.kcombobox_images.Items.Add(new comboitem("Wood 2", EquationGrapher3D.Properties.Resources.w18));
            this.kcombobox_images.Items.Add(new comboitem("Brick 4", EquationGrapher3D.Properties.Resources.w19));

            this.kcombobox_images.Items.Add(new comboitem("Brick 5", EquationGrapher3D.Properties.Resources.w20));
            this.kcombobox_images.Items.Add(new comboitem("Grady", EquationGrapher3D.Properties.Resources.w21));
            this.kcombobox_images.Items.Add(new comboitem("Grady", EquationGrapher3D.Properties.Resources.w22));
            this.kcombobox_images.Items.Add(new comboitem("Color interance", EquationGrapher3D.Properties.Resources.w23));

            this.kcombobox_images.Items.Add(new comboitem("roops loop", EquationGrapher3D.Properties.Resources.w24));
            this.kcombobox_images.Items.Add(new comboitem("night", EquationGrapher3D.Properties.Resources.w25));
           
     

       
        
            kcombobox1.SelectedIndex = 0;
            kcombobox2.SelectedIndex = 0;
            kcombobox_images.SelectedIndex = 0;
        }

        private void kDynamkPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kDynamkPanel3_openingclosing(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => this.Size = new Size(kDynamkPanel3.Width, kDynamkPanel3.Height)));     
        }

        private void kgriditem9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void kgriditem_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void kPictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "all files|*.*|png|*.png|jpg|*.jpg|bmp|*.bmp";
            if (dv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    (kPictureBox1).BackgroundImage = Image.FromFile(dv.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to read :" + ex.Message);
                }
            }
            kPictureBox1.Selectionrec = new RectangleF(0, 0, 100, 100);
        }

        private void kcombobox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorEquation cq = (ColorEquation)((comboitem)((kcombobox)sender).SelectedItem).Tag;
            textBox_r.Text = cq.Red;
            textBox_g.Text = cq.Green;
            textBox_b.Text = cq.Blue;
            textbox_a.Text = cq.Alpha;
        }

        private void kcombobox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            RectangleF rec = (RectangleF)((comboitem)((kcombobox)sender).SelectedItem).Tag;
            kPictureBox1.Selectionrec = rec;
        }

        private void kgriditem5_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.Vars = new string[8] { "x%", "y%", "z%", "x", "y", "z","u","v" };
            cf.equationvalue = textBox_r.Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                textBox_r.Text = cf.equationvalue;
            }
        }

        private void kgriditem7_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.Vars = new string[8] { "x%", "y%", "z%", "x", "y", "z", "u", "v" };
            cf.equationvalue = textBox_g.Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                textBox_g.Text = cf.equationvalue;
            }
        }
        private void kgriditem6_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.Vars = new string[8] { "x%", "y%", "z%", "x", "y", "z", "u", "v" };
            cf.equationvalue = textBox_b.Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                textBox_b.Text = cf.equationvalue;
            }
        }

       

        private void textBox_g_Enter(object sender, EventArgs e)
        {
            Control t = sender as Control;
            if (t.BackColor != Color.Pink)
            {
                t.BackColor = Color.Yellow;
            }
        }

        private void textBox_g_Leave(object sender, EventArgs e)
        {
            Control t = sender as Control;
            if (t.BackColor != Color.Pink)
            {
                t.BackColor = Color.White;
            }
        }

        private void textBox_g_TextChanged(object sender, EventArgs e)
        {
                  RichTextBox t = sender as RichTextBox;
           string[] vars = new string[10] { "x%", "y%", "z%", "x", "y", "z", "u%", "v%","u","v" };
            fnc.ColorMathEquation(t,vars);

            EquationSolver eqs = new EquationSolver(false);
            bool isright = true;
            try
            {

                EquationSolver.SolveEQ(t.Text, vars, new float[10]{0,0,0,0,0,0,0,0,0,0},false );
                isright = true ;
              
            }
            catch (MathException me)
            { isright = true; }
            catch (Exception ex)
            {
                isright = false;
               
            }
            if (isright)
            {
                if (t.Focused)
                {
                    t.BackColor = Color.Yellow;
                }
                else
                {
                    t.BackColor = Color.White;
                }
            }
            else
            {
                t.BackColor = Color.Pink;
            }
        }

        private void kcombobox_images_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboitem c = ((comboitem)((kcombobox)sender).SelectedItem);
          kPictureBox1.BackgroundImage = (Bitmap)c.Image;
          kPictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "all files|*.*|png|*.png|jpg|*.jpg|bmp|*.bmp";
            if (dv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    (kPictureBox1).BackgroundImage = Image.FromFile(dv.FileName);
               }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to read :" + ex.Message);
                }
            }
        }

        private void kgriditem1_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.Vars = new string[8] { "x%", "y%", "z%", "x", "y", "z", "u", "v" };
            cf.equationvalue = textbox_a.Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                textbox_a.Text = cf.equationvalue;
            }
        }

        private void kscale_backfacecolor_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kscale_backfacecolor_ValueChanging(object sender, Kscaleeventargs e)
        {
            Color c = e.SelectedColor;
            textbox_a.Text = ((int)(c.A / 2.55f)).ToString();
            textBox_b.Text = ((int)(c.B/2.55f)).ToString();
            textBox_r.Text = ((int)(c.R / 2.55f)).ToString();
            textBox_g.Text = ((int)(c.G / 2.55f)).ToString();
        }

        private void kscale_griddomain_ValueChanged(object sender, Kscaleeventargs e)
        {

        }
    }
}
