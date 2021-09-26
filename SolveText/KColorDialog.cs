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
    public partial class KColorDialog : Form
    {
        public KColorDialog()
        {
            InitializeComponent();
        }
        public Color[] customcolors(int rat)
        {

            bool brk = false;
            List<Color> ss = new List<Color>();
            for (int r = 0; r <= 255; r += 0)
            {
                for (int g = 0; g <= 255; g += 0)
                {
                    for (int b = 0; b <= 255; b += 0)
                    {
                        Color col = Color.FromArgb(r, g, b);
                        ss.Add(col);


                        r += rat;
                        if (r >= 255)
                        {
                            r = 0;
                            g += rat;
                            if (g >= 255)
                            {
                                g = 0;
                                b += rat;
                                if (b >= 255)
                                {
                                    b = 0;
                                    brk = true;
                                    break;
                                }
                            }
                        }

                    }
                    if (brk) break;
                }
                if (brk) break;
            }

            List<Color> ddd = new List<Color>(ss.OrderBy(d => d.GetHue()));

            return ddd.ToArray();

        }
        private void sd(Color[] c, Control owner, Size btnsize, KitemShapes ks)
        {

            for (int i = 0; i < c.Length; i += 1)
            {
                kgriditem nbtn = new kgriditem();
                Color col = c[i];
                nbtn.Size = btnsize;

                nbtn.Theme = KitemThemes.sky;

                nbtn.Paintstyle = KitemPaintStyle.Flat;
                nbtn.BackColor = col;
                nbtn.Text = (col.IsNamedColor ? "" : "#") + col.Name;
                if (ks == KitemShapes.grid)
                { nbtn.ShowText = false; }
                nbtn.ShowSelectedTrangle = false;
                Bitmap bb = new Bitmap(20, 20);

                for (int x = 0; x < bb.Width; x++)
                {
                    for (int y = 0; y < bb.Height; y++)
                    {
                        bb.SetPixel(x, y, c[i]);
                    }
                }
                nbtn.CenterImageLayout = ImageLayout.Stretch;
                nbtn.Centerimage = bb;
                nbtn.ImgEffect = kgriditem.ImgEffectss.none;
                nbtn.Shape = ks;

                nbtn.Click += new System.EventHandler(colorbtn_click);

                owner.Controls.Add(nbtn);

            }
        }
        private void colorbtn_click(object sender, EventArgs e)
        {
            ColorValue = ((kgriditem)sender).BackColor;

        }
        private void KColorDialog_Load(object sender, EventArgs e)
        {
            sd(customcolors(50), kgrid1, new Size(14, 20), KitemShapes.grid);
            if (DefultColor != null)
            {
                ColorValue = DefultColor;
            }
            else
            {
                ColorValue = Color.Black;
            }
            Bitmap b = new Bitmap(20, 20);
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                { b.SetPixel(x, y, DefultColor); }
            }
            kgriditem4.Centerimage = b;

            IEnumerable<Color> colors = Enum.GetValues(typeof(KnownColor))
                   .Cast<KnownColor>()
                 .Select(kc => Color.FromKnownColor(kc))
               .OrderBy(c => c.GetHue());

            sd(colors.ToArray<Color>(), kgrid2, new Size(kgrid2.Width - 40, 20), KitemShapes.tile);
        }

        private Color colorvalue=Color.Black;
        private void sethsv(ColorHandler.HSV hsvcolor)
        {
            Color value = ColorHandler.HSVtoRGB(hsvcolor);
            colorvalue = value;
            Bitmap b = new Bitmap(20, 20);
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                { b.SetPixel(x, y, value); }
            }
            bclr.Centerimage = b;
            if (255 - sbright_gdi.Value != hsvcolor.value)
            {
                sbright_gdi.Smoothcolor = value;
            }
            shue.Value = hsvcolor.Hue;
            ssat.Value = hsvcolor.Saturation;

            sred.Value = value.R;
            sgreen.Value = value.G;
            sblue.Value = value.B;



            textBox1.Text = (colorvalue.IsNamedColor ? "" : "#") + colorvalue.Name;




            if (colorPicker_id3.SelectedColor != colorvalue)
            {
                colorPicker_id3.EnterdColor = value;
            }

        }

        public Color ColorValue
        {
            get { return colorvalue; }
            set
            {
                colorvalue = value;
                Bitmap b = new Bitmap(20, 20);
                for (int x = 0; x < b.Width; x++)
                {
                    for (int y = 0; y < b.Height; y++)
                    { b.SetPixel(x, y, value); }
                }
                bclr.Centerimage = b;

                sbright_gdi.Smoothcolor = value;

                sred.Value = value.R;
                sgreen.Value = value.G;
                sblue.Value = value.B;

                ColorHandler.HSV hsvcolor = ColorHandler.RGBtoHSV(value);
                shue.Value = hsvcolor.Hue;
                ssat.Value = hsvcolor.Saturation;


                salpha.Value = value.A;

                textBox1.Text = (colorvalue.IsNamedColor ? "" : "#") + colorvalue.Name;



                if (colorPicker_id3.SelectedColor != colorvalue)
                {
                    colorPicker_id3.EnterdColor = value;
                }

            }
        }



        public Color DefultColor;



        private void kgriditem1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void kgriditem5_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }







        private void sblue_ValueChanging(object sender, Kscaleeventargs e)
        {
            if (((Control)sender) == salpha)
            {
                salpha.Maincolor = Color.FromArgb(e.Value, salpha.Maincolor);
            }
            if (((kscale)sender).IsOnValueChanging)
            {
                ColorValue = Color.FromArgb(salpha.Value, sred.Value, sgreen.Value, sblue.Value);
                colorPicker_hsl_cicle.SelectedColor = colorvalue;
            }
        }



        private void kgriditem3_Click(object sender, EventArgs e)
        {
            ColorValue = ControlPaint.Light(ColorValue, .19f);



        }

        private void kscale1_ValueChanging(object sender, Kscaleeventargs e)
        {
            ColorValue = e.SelectedColor;

        }


        private void buttonX1_Click(object sender, EventArgs e)
        {
            kscale_max.MaxgradientColors = new Color[] { Color.Black, Color.Green, Color.Yellow, Color.Red, Color.Blue, Color.Fuchsia, Color.Orange, Color.Cyan };
          
            kscale_max.MakeMaxGradientPostions();
        }



        private void kscale1_ValueChanging_2(object sender, Kscaleeventargs e)
        {

            ColorHandler.HSV hsv = ColorHandler.RGBtoHSV(e.SelectedColor);
            hsv.value = 255 - sbright_gdi.Value;
            sbright_gdi.Smoothcolor = e.SelectedColor;

            sethsv(hsv);

        }


        private void colorPicker1_ValueChanging_1(ColorPicker sender, ColorPickereventargs e)
        {
            ColorValue = e.SelectedColor;

        }

        private void colorPicker2_ValueChanging(ColorPicker sender, ColorPickereventargs e)
        {
            if (sender.onmoving)
            {
                ColorHandler.HSV hsv = ColorHandler.RGBtoHSV(e.SelectedColor);
                hsv.value = 255 - sbright_gdi.Value;
                sbright_gdi.Smoothcolor = e.SelectedColor;

                sethsv(hsv);
            }
        }



        private void kgriditem2_Click(object sender, EventArgs e)
        {
            ColorValue = ControlPaint.Dark(ColorValue, 0.2f);


        }


        private void hs_ValueChanging(object sender, Kscaleeventargs e)
        {
            if (((kscale)sender).IsOnValueChanging)
            {
                sethsv(new ColorHandler.HSV(shue.Value, ssat.Value, 255 - sbright_gdi.Value));
                colorPicker_hsl_cicle.SelectedColor = colorvalue;
                sbright_gdi.Smoothcolor = colorvalue;
            }
        }

        private void ssat_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void sbright_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void sbright_gdi_ValueChanging(object sender, Kscaleeventargs e)
        {
            MessageBox.Show(":");
            //   ColorValue = ColorPicker.ColorHandler.HSVtoRGB(shue.Value, ssat.Value, e.Value);

        }

        private void kscale2_ValueChanging(object sender, Kscaleeventargs e)
        {
            MessageBox.Show(":");

        }

        private void sbright_gdi_ValueChanging_1(object sender, Kscaleeventargs e)
        {
            sethsv(new ColorHandler.HSV(shue.Value, ssat.Value, 255 - e.Value));
        }

        private void kscale3_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ss = textBox1.Text;

                if (!ss.StartsWith("#") && !colorvalue.IsNamedColor)
                {

                    ss = ss.Insert(0, "#");

                }
                try
                {
                    Color v = (Color)new ColorConverter().ConvertFromString(ss);
                    textBox1.Text = ss;
                    ColorValue = v;
                }
                catch
                {
                    MessageBox.Show("Error '" + textBox1.Text + "' Is InCorrect Format ");
                    textBox1.Text = colorvalue.Name;
                }
            }
        }

        private void kgriditem7_Click(object sender, EventArgs e)
        {

        }



        private void kscale3_ValueChanged_1(object sender, Kscaleeventargs e)
        {

        }

        private void bclr_Click(object sender, EventArgs e)
        {

        }

        private void kgriditem4_Click(object sender, EventArgs e)
        {

        }










    }
}
