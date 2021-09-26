using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace SolveText
{

    public partial class MainForm : Form
    {
        private void MeasureCharacterRangesRegions(PaintEventArgs e)
        {

            // Set up string. 
            string measureString = "First and Second ranges";
            Font stringFont = new Font("Times New Roman", 16.0F);

            // Set character ranges to "First" and "Second".
            CharacterRange[] characterRanges = { new CharacterRange(0, 2), new CharacterRange(10, 6) };

            // Create rectangle for layout. 
            float x = 50.0F;
            float y = 50.0F;
            float width = 200.0F;
            float height = 40.0F;
            RectangleF layoutRect = new RectangleF(x, y, width, height);

            // Set string format.
            StringFormat stringFormat = new StringFormat();
            //   stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            stringFormat.SetMeasurableCharacterRanges(characterRanges);

            // Draw string to screen.
            e.Graphics.DrawString(measureString, stringFont, Brushes.Black, x, y, stringFormat);

            // Measure two ranges in string.
            Region[] stringRegions = e.Graphics.MeasureCharacterRanges(measureString,
        stringFont, layoutRect, stringFormat);

            for (int n = 0; n < stringRegions.Length; n++)
            {
                RectangleF[] recs = stringRegions[n].GetRegionScans(new Matrix());
                for (int i = 0; i < recs.Length; i++)
                {
                    //  e.Graphics.DrawRectangle(new Pen(Color.Black), Rectangle.Ceiling(recs[i]));
                }
            }
            string stng = "WWO";
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(stng, this.Font.FontFamily, 0, 10, new Point(0, 0), new StringFormat());
            gp.CloseAllFigures();
            // Draw rectangle for first measured range.
            RectangleF measureRect1 = stringRegions[0].GetBounds(e.Graphics);
            // e.Graphics.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(measureRect1));

            // Draw rectangle for second measured range.
            RectangleF measureRect2 = stringRegions[1].GetBounds(e.Graphics);
            //    e.Graphics.DrawRectangle(new Pen(Color.Blue, 1), Rectangle.Round(measureRect2));
        }
        public void GetRegionDataExample(PaintEventArgs e)
        {
            MeasureCharacterRangesRegions(e);
            Graphics g = e.Graphics;
            Region[] regs = g.MeasureCharacterRanges("A B S", this.Font, new RectangleF(0, 0, 100, 100), new StringFormat());



            for (int n = 0; n < regs.Length; n++)
            {
                RectangleF[] recs = regs[n].GetRegionScans(new Matrix());
                for (int i = 0; i < recs.Length; i++)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black), Rectangle.Ceiling(recs[i]));
                }
            }
        }
        // THIS IS A HELPER FUNCTION FOR GetRegionData. 
        public void DisplayRegionData(PaintEventArgs e,
            int len,
            RegionData dat)
        {

            // Display the result. 
            int i;
            float x = 20, y = 140;
            Font myFont = new Font("Arial", 8);
            SolidBrush myBrush = new SolidBrush(Color.Black);
            e.Graphics.DrawString("myRegionData = ",
                myFont,
                myBrush,
                new PointF(x, y));
            y = 160;
            for (i = 0; i < len; i++)
            {
                if (x > 300)
                {
                    y += 20;
                    x = 20;
                }
                e.Graphics.DrawString(dat.Data[i].ToString(),
                    myFont,
                    myBrush,
                    new PointF(x, y));
                x += 30;
            }

        }

        ThreadStart thrdst;
        Thread t;

        ParameterizedThreadStart thrdstart_loadeqs;
        Thread thrd_loadeqs;
        public MainForm()
        {
            InitializeComponent();



        }
        bool istreadalive(Thread t)
        {
            if (t == null)
            {
                return false;
            }
            else if (t.IsAlive)
            {
                return true;
            }
            else { return false; }
        }
        private bool iseditmode()
        { return kscale_editmode.Value == 0 ? false : true; }
        [Serializable()]
        public class ModlesCollections
        {
            public List<Model3D> Models = new List<Model3D>();
            public List<EquationFile> Equations = new List<EquationFile>();
            private int selectedIndex = -1;

            public int SelectedIndex
            {
                get { return selectedIndex; }
                set
                {
                    selectedIndex = value;
                    for (int i = 0; i < this.Models.Count; i++)
                    {
                        if (this.Models[i] != null)
                        {
                            if (i == value)
                            {
                                this.Models[value].isselected = true;
                            }
                            else
                            {
                                this.Models[i].isselected = false;
                            }
                        }
                    }

                }
            }
            public Model3D SelectedModel
            {
                get { if (selectedIndex < 0) { return null; } { return this.Models[selectedIndex]; } }
                set
                {
                    this.SelectedIndex = this.Models.IndexOf(value);
                }
            }
            public int Count()
            {
                if (this.Equations.Count == this.Models.Count)
                {
                    return this.Models.Count;
                }
                else
                { return -1; }
            }
            public ModlesCollections()
            { }
            public ModlesCollections(int count)
            {
                this.Models.AddRange(new Model3D[count]);
                this.Equations.AddRange(new EquationFile[count]);

            }
            public void Add(EquationFile equation, Model3D mdl)
            {
                this.Models.Add(mdl);
                this.Equations.Add(equation);
            }
            public void Add(Model3D mdl)
            {
                this.Models.Add(mdl);
                this.Equations.Add(null);
            }
            public void Add(EquationFile equation)
            {
                this.Models.Add(null);
                this.Equations.Add(equation);
            }
            public void RemoveAt(int indx)
            {

                this.Equations.RemoveAt(indx);
                this.Models.RemoveAt(indx);
            }
            public void Remove(EquationFile eqf)
            {
                int indx = this.Equations.IndexOf(eqf);
                this.Equations.RemoveAt(indx);
                this.Models.RemoveAt(indx);
            }
            public void Remove(Model3D mdl)
            {
                int indx = this.Models.IndexOf(mdl);
                this.Equations.RemoveAt(indx);
                this.Models.RemoveAt(indx);
            }
        }
        private void UpdateEquations()
        {
            for (int i = 0; i < Modles.Count(); i++)
            {
                EquationForm eq = (EquationForm)flowLayoutPanel_modles.Controls[i];
                eq.updateequationfile();
                Modles.Equations[i] = eq.Equationfilevalue;
            }
        }
        private void newequation_modelvisible_changed(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel_modles.Controls.Count; i++)
            {
                EquationForm eqform = flowLayoutPanel_modles.Controls[i] as EquationForm;
                if (Modles.Models[i] != null)
                {
                    Modles.Models[i].IsVisible = eqform.ismodelvisible;
                }
            }
            Regraph();
        }
        private void AddEquation(EquationFile eqfile)
        {

            EquationForm eqform = new EquationForm();
            eqform.TopLevel = false;
            eqform.FormClosing += new FormClosingEventHandler(equationform_closing);
            eqform.draw += new EventHandler(newequationform_draw_Click);
            eqform.ismodelvisible_changed += new EventHandler(newequation_modelvisible_changed);
            Modles.Add(eqfile);
            flowLayoutPanel_modles.Controls.Add(eqform);

            eqform.Show();

            if (eqfile != null)
            {
                eqform.Equationfilevalue = eqfile;
            }

        }
        private void AddEquation()
        {
            EquationForm eqform = new EquationForm();
            eqform.TopLevel = false;
            eqform.FormClosing += new FormClosingEventHandler(equationform_closing);
            eqform.draw += new EventHandler(newequationform_draw_Click);
            eqform.ismodelvisible_changed += new EventHandler(newequation_modelvisible_changed);

            eqform.updateequationfile();
            Modles.Add(eqform.Equationfilevalue);
            flowLayoutPanel_modles.Controls.Add(eqform);

            eqform.Show();


        }
        public void AddModels(string addres)
        {

            Models3Dlist mdls = Models3Dlist.FromFile(addres);
            for (int i = 0; i < mdls.Models.Count; i++)
            {

                EquationForm eqform = new EquationForm();
                eqform.TopLevel = false;
                eqform.FormClosing += new FormClosingEventHandler(equationform_closing);
                eqform.draw += new EventHandler(newequationform_draw_Click);
                eqform.ismodelvisible_changed += new EventHandler(newequation_modelvisible_changed);
                eqform.LoadModel(addres, i);
                Modles.Add(eqform.Equationfilevalue);
                flowLayoutPanel_modles.Controls.Add(eqform);
                eqform.Show();
            }
        }
        public void AddModelsformresources(int resourcesindx)
        {
            string modelname = "";
            switch (resourcesindx)
            {
                case 0:
                    modelname = "electorns_super_ball_levels";
                    break;
                case 1:
                    modelname = "f16";
                    break;
            }
            Models3Dlist mdls = EquationForm.Getmodelfromresources(modelname);
            for (int i = 0; i < mdls.Models.Count; i++)
            {

                EquationForm eqform = new EquationForm();
                eqform.TopLevel = false;
                eqform.FormClosing += new FormClosingEventHandler(equationform_closing);
                eqform.draw += new EventHandler(newequationform_draw_Click);
                eqform.ismodelvisible_changed += new EventHandler(newequation_modelvisible_changed);
                eqform.LoadModel(EquationForm.res + modelname, i, mdls);
                Modles.Add(eqform.Equationfilevalue);
                flowLayoutPanel_modles.Controls.Add(eqform);
                eqform.Show();


            }
        }


        public ModlesCollections Modles = new ModlesCollections();

        private void analzform_colsed(object sender, FormClosingEventArgs e)
        {
            showcalculations = false;
        }
        Label starting_lbl = new Label();
        bool is_form_loaded = false;
        bool is_firstmodel_generated = false;
        private void picterbox1_mousewheel(object sender, MouseEventArgs e)
        {
            if (Modles.SelectedModel != null)
            {
                ActionThread(ref rotate3dthread, null, null, false, true);

                float sign = Math.Sign(e.Delta * SystemInformation.MouseWheelScrollLines / 1000f);
                float scale = 1;
                if (sign == 1)
                {
                    scale = 1.2f;
                }
                else if (sign == -1)
                { scale = 0.8f; }

                Model3D mdl = Modles.SelectedModel;
                this.Text = scale.ToString();
                mdl.Resize(mdl.Size_With_Out_Rotation * scale);
                Regraph();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseWheel += new MouseEventHandler(picterbox1_mousewheel);
            starting_lbl.Text = "Welcome " + Environment.UserName + ",Please wait a seconds while we finish processing ...";
            starting_lbl.AutoSize = false;
            starting_lbl.Size = this.Size;
            starting_lbl.Location = new Point(0, 0);
            starting_lbl.TextAlign = ContentAlignment.MiddleCenter;
            starting_lbl.Font = new Font(this.Font.FontFamily, 20);
            starting_lbl.ForeColor = Color.Blue;

            this.Controls.Add(starting_lbl);
            starting_lbl.BringToFront();
            kpnl_cam.IsOpened = false;
            kDynamkPanel_edit_selectedpoints.IsOpened = false;
            Bitmap b = new Bitmap(25, 25);
            Graphics g = Graphics.FromImage(b);
            g.Clear(pictureBox1.BackColor);
            kgriditem10.Centerimage = b;

            colorPicker_lightxy.Value = new Point(0, 0);
            kscale_record.Value = 0;
            drawfont = new Font(this.Font.FontFamily, 12);
            thrdstart_loadeqs = new ParameterizedThreadStart(h);
            thrd_loadeqs = new Thread(thrdstart_loadeqs);
            AddTextureModle();
            string s = "";
            try
            {
                s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\equationgrapherfile2.ico";

                if (File.Exists(s) == false)
                {

                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SolveText.Resources.equationgrapherfile2.ico");
                    FileStream fileStream = new FileStream(s, FileMode.Create);
                    for (int i = 0; i < stream.Length; i++)
                        fileStream.WriteByte((byte)stream.ReadByte());
                    fileStream.Close();
                }
            }
            catch
            {

            }
            try
            {
                fnc.Selraliz.EstablishNewFileType(Application.ExecutablePath, "veq", s, "Vector Equation file");
            }
            catch
            {

            }
            try
            {
                fnc.Selraliz.EstablishNewFileType(Application.ExecutablePath, "mdls", s, "Vector Equation file");
           
            }
            catch
            {

            }
            string startup = fnc.Selraliz.getCommandstartup();
            bool ismodeladded = false;
            if (startup != "")
            {
                if (startup.ToLower().EndsWith("mdls"))
                {
                    try
                    {
                        AddModels(startup);
                        ismodeladded = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to read :" + ex.Message);
                    }
                }
                else if (startup.ToLower().EndsWith(".veq"))
                {
                    try
                    {
                        AddEquation(fnc.Selraliz.ReadFromBinaryFile<EquationFile>(startup));
                        ismodeladded = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to read :" + ex.Message);
                    }
                }
            }
            if (!ismodeladded)
            {
                AddEquation();

            }
            Modles.SelectedIndex = 0;

            button_Param_Click(null, null);
            is_form_loaded = true;
            if (starting_lbl != null && is_firstmodel_generated)
            {
                this.Controls.Remove(starting_lbl);
                starting_lbl = null;

            }
        }


        TextureModel GetDefultTextureModle()
        {
            Form_TextureModle f = (Form_TextureModle)flowLayoutPanel_textures.Controls[0];
            return f.TextureModleValue;
        }
        TextureModel[] Defulttextures;
        TextureModel[] GetDefulttextures()
        {
            TextureModel[] tms = new TextureModel[flowLayoutPanel_textures.Controls.Count];
            for (int i = 0; i < flowLayoutPanel_textures.Controls.Count; i++)
            {
                tms[i] = ((Form_TextureModle)flowLayoutPanel_textures.Controls[i]).TextureModleValue;
            }
            Defulttextures = tms;
            return tms;
        }

        int milly = 0;
        public bool GetRotateAll()
        { return kscale_rotatemode.Value == 0 ? false : true; }
        public bool GetDrawLines()
        { return this.kscale_fillarea.Value == 1 || this.kscale_fillarea.Value == 3 ? true : false; }
        public bool GetDrawbox()
        { return kscale_drawbox.Value == 0 ? false : true; }
        public int GetDrawaxes()
        { return kscale_axes.Value; }
        public bool Getrecord()
        { return this.kscale_record.Value == 0 ? false : true; }

        public bool GetColorLines()
        { return kscale_colorlines.Value == 0 ? false : true; }

        public bool GetFillPlans()
        { return this.kscale_fillarea.Value == 2 || this.kscale_fillarea.Value == 3 ? true : false; }

        public bool GetDrawPoints()
        { return this.kscale_drawpoints.Value == 0 ? false : true; }

        public float GetDiffuse()
        { return this.kscale_lightstrong.ValueF / 100f; }

        public float GetSpecular()
        { return this.kscale_specular.Value; }

        public bool GetDrawNormals()
        { return this.kscale_norms.Value == 0 ? false : true; }

        public bool GetDraweyeray()
        { return this.kscale_eyeray.Value == 0 ? false : true; }

        public bool GetReflectedRey()
        { return this.kscale_Reflectiveray.Value == 0 ? false : true; }

        public bool GetLightRay()
        { return this.kscale_lightray.Value == 0 ? false : true; }

        public bool GetDrawFrontFace()
        { return this.kscale_frontface.Value == 0 ? false : true; }

        public bool GetDrawBackFace()
        { return this.kscale_backface.Value == 0 ? false : true; }

        public bool GetDrawShadow()
        { return this.kscale_shadow.Value == 0 ? false : true; }

        public Color GetBackFaceColor()
        { return this.kscale_backfacecolor.SelectedColor; }

        public Color GetFrontFaceColor()
        { return this.kscale_frontfacecolor.SelectedColor; }
        public PointF GetPalnDomain()
        {
            return new PointF(kscale_plan.MinValue / 100f, kscale_plan.MaxValue / 100f);
        }
        public PointF GetGridDomain()
        {
            return new PointF(kscale_grid.MinValue / 100f, kscale_grid.MaxValue / 100f);
        }
        public Vector3 GetLightPosition()
        { return new Vector3(colorPicker_lightxy.Value.X * 500, colorPicker_lightxy.Value.Y * 500, 0); }

        public Render GetdefultRender()
        {
            Render v = new Render();

            v.colorlines = GetColorLines();
            v.diffuse = GetDiffuse();
            v.eyeray = GetDraweyeray();
            v.fillplans = GetFillPlans();
            v.backfacecolor = GetBackFaceColor();
            v.frontfacecolor = GetFrontFaceColor();
            v.frontface = GetDrawFrontFace();
            v.backface = GetDrawBackFace();
            v.lightposition = GetLightPosition();
            v.lightray = GetLightRay();
            v.lines = GetDrawLines();
            v.normalvector = GetDrawNormals();
            v.points = GetDrawPoints();
            v.reflectedray = GetReflectedRey();
            v.shadow = GetDrawShadow();
            v.specular = GetSpecular();
            v.plandomain = GetPalnDomain();
            v.griddomain = GetGridDomain();
            v.drawbox = GetDrawbox();
            v.drawaxes = GetDrawaxes();
            v.drawboxplans = kscale_drawwalls.Value;
            v.drawinfo = kscale_info.Value == 0 ? false : true;
            v.highlight_selectedpoints = iseditmode();
   
            v.Dark_Render_engine = kscale_lightengine.Value == 0 ? false : true;
            return v;
        }











        List<Bitmap> bbs = new List<Bitmap>();
        bool exitrortatthrad = true;
        void rotate3dthread_void(object speedpoint)
        {
            while (exitrortatthrad == false)
            {
                Vector3 minp = new Vector3();
                Vector3 maxp = new Vector3();
                Point rotspeed = (Point)speedpoint;
                rotspeed.X = rotspeed.X; rotspeed.Y = rotspeed.Y;
                if (GetRotateAll())
                {
                    for (int i = 0; i < Modles.Count(); i++)
                    {
                        if (Modles.Models[i] != null)
                        {
                            Modles.Models[i].MuliplyRotate(rotspeed);
                        }
                    }
                }
                else if (Modles.SelectedModel != null)
                {
                    Modles.SelectedModel.MuliplyRotate(rotspeed);
                }
                if (exitrortatthrad == false)
                {
                    pictureBox1.Invoke(new Action(() => Regraph()));
                }

            }

        }


        void startrotatethread(object rotspeed, bool start = true)
        {
            System.Threading.ParameterizedThreadStart pts = new ParameterizedThreadStart(rotate3dthread_void);

            if (rotate3dthread == null)
            {

                if (start)
                {
                    exitrortatthrad = false;
                    rotate3dthread = new Thread(pts);
                    rotate3dthread.Start(rotspeed);
                }
            }
            else if (rotate3dthread.IsAlive)
            {
                if (start == false)
                {
                    exitrortatthrad = true;
                }
            }
            else //thread !=null but stoped
            {
                if (start)
                {
                    exitrortatthrad = false;
                    rotate3dthread = new Thread(pts);
                    rotate3dthread.Start(rotspeed);
                }
            }
        }
        void ActionThread(ref Thread t, ParameterizedThreadStart ts, object param, bool start, bool specialforrotate = false)
        {

            if (t == null)
            {
                if (start)
                {
                    t = new Thread(ts);

                    t.Start(param);

                }
            }
            else if (t.IsAlive)
            {
                if (start == false)
                {
                    if (specialforrotate)
                    {
                        exitrortatthrad = true;
                    }
                    else
                    {
                        t.Abort();
                    }
                }
            }
            else //thread !=null but stoped
            {
                if (start)
                {
                    t = new Thread(ts);
                    t.Start(param);

                }
            }
        }
        void ActionThread(ref Thread t, ThreadStart ts, bool start = true)
        {

            if (t == null)
            {
                if (start)
                {
                    t = new Thread(ts);

                    t.Start();

                }
            }
            else if (t.IsAlive)
            {
                if (start == false)
                {

                    t.Abort();
                }
            }
            else //thread !=null but stoped
            {
                if (start)
                {
                    t = new Thread(ts);
                    t.Start();

                }
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (rotate3dthread != null)
            {
                rotate3dthread.Abort();
            }
            if (thrd_loadeqs != null)
            {
                thrd_loadeqs.Abort();
            }
        }









        private void kgriditem35_Click(object sender, EventArgs e)
        {
            AddEquation();
        }


        void cfg()
        {
            for (int s = 0; s <= 2 * 100; s++)
            {

                notfcform.Invoke(new Action(() =>
                {
                    notfcform.Opacity = 2f - (((float)s / (1f * 100f)) * 1f);
                }));

                System.Threading.Thread.Sleep(1000 / 100);
            }
            notfcform.Invoke(new Action(() =>
           {
               notfcform.Dispose();
           }));
        }
        ThreadStart ts2;
        Thread t2;
        Notfic notfcform;

        void startt2()
        {
            if (t2 == null)
            {
                notfcform = new Notfic();
                Size scn = Screen.PrimaryScreen.WorkingArea.Size;
                notfcform.StartPosition = FormStartPosition.Manual;
                notfcform.Location = new Point(scn.Width - notfcform.Width - 10, scn.Height - notfcform.Height - 10);
                notfcform.Show();
                this.Activate();
                t2 = new Thread(ts2);
                t2.Start();

            }
            else if (t2.IsAlive)
            {
                t2.Abort();
                notfcform.Dispose();

            }
            else
            {
                if (notfcform != null)
                {

                    if (notfcform.IsDisposed)
                    {
                        notfcform = new Notfic();
                    }
                }
                else
                { notfcform = new Notfic(); }
                Size scn = Screen.PrimaryScreen.WorkingArea.Size;
                notfcform.StartPosition = FormStartPosition.Manual;
                notfcform.Location = new Point(scn.Width - notfcform.Width - 10, scn.Height - notfcform.Height - 10);
                notfcform.Show();

                t2 = new Thread(ts2);
                t2.Start();

            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            startt2();
        }




        private void kgriditem37_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/mohamedsalah.elkony");
        }

        private void kgriditem36_Click_1(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }








        //----------------------------
        private void kgriditem42_Click(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "Vector Equation(veq)|*.veq";
            dv.Multiselect = true;
            if (dv.ShowDialog() == DialogResult.OK)
            {

                for (int i = 0; i < dv.FileNames.Length; i++)
                {
                    try
                    {
                        EquationFile eq = fnc.Selraliz.ReadFromBinaryFile<EquationFile>(dv.FileNames[i]);
                        AddEquation(eq);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to read :" + ex.Message);
                    }
                }
            }
        }







        Bitmap Get3DCordsLines(Vector3 rotateangle, int sz)
        {
            Bitmap b = new Bitmap(sz * 2, sz * 2);
            Graphics g = Graphics.FromImage(b);
            g.TranslateTransform(sz, sz);
            Vector3[] px = new Vector3[2] { new Vector3(-sz, 0, 0), new Vector3(sz, 0, 0) };
            Vector3[] py = new Vector3[2] { new Vector3(0, -sz, 0), new Vector3(0, sz, 0) };
            Vector3[] pz = new Vector3[2] { new Vector3(0, 0, -sz), new Vector3(0, 0, sz) };


            Rotate3D.RotateVectors(rotateangle.TOPointF(), px);
            Rotate3D.RotateVectors(rotateangle.TOPointF(), py);
            Rotate3D.RotateVectors(rotateangle.TOPointF(), pz);

            Pen p = new Pen(Color.Red, 4f); p.DashStyle = DashStyle.Dash; p.EndCap = p.StartCap = LineCap.ArrowAnchor;
            g.DrawLine(p, px[0].TOPointF(), px[1].TOPointF());
            p.Color = Color.Green;
            g.DrawLine(p, py[0].TOPointF(), py[1].TOPointF());
            p.Color = Color.Blue;
            g.DrawLine(p, pz[0].TOPointF(), pz[1].TOPointF());

            return b;
        }
        Bitmap Get3DCordsLines(Vector3[][] pnts, int sz)
        {
            return Get3DCordsLines(pnts, false, 5, true, sz);
        }


        Bitmap Get3DCordsLines(Vector3[][] pnts, bool dash, float wid, bool drawstring, int sz)
        {
            Bitmap b = new Bitmap(sz, sz);
            Graphics g = Graphics.FromImage(b);
            g.TranslateTransform(sz / 2, sz / 2);
            PointF xloc = pnts[0][1].TOPointF();
            PointF yloc = pnts[1][1].TOPointF();
            PointF zloc = pnts[2][1].TOPointF();

            Pen p = new Pen(Color.Red, wid);
            if (dash)
            { p.DashStyle = DashStyle.Dash; }
            p.EndCap = LineCap.ArrowAnchor;// p.StartCap
                                           //x
            g.DrawLine(p, pnts[0][0].TOPointF(), xloc);
            //y
            p.Color = Color.Green;
            g.DrawLine(p, pnts[1][0].TOPointF(), yloc);
            //z
            p.Color = Color.Blue;
            g.DrawLine(p, pnts[2][0].TOPointF(), zloc);

            if (drawstring)
            {
                Font sf = new Font(this.Font.FontFamily, 15);
                xloc.X += 2; xloc.Y += 2;
                g.DrawString("X", sf, new SolidBrush(Color.Red), xloc);
                yloc.X += 2; yloc.Y += 2;
                g.DrawString("Y", sf, new SolidBrush(Color.Green), yloc);
                zloc.X += 2; zloc.Y += 2;
                g.DrawString("Z", sf, new SolidBrush(Color.Blue), zloc);
            }
            return b;
        }



        public Bitmap Get3DImg(Model3D mdl, Render rend)
        {
            return Get3DImg(new Model3D[] { mdl }, pictureBox1.Size, rend, new Face[][] { mdl.GetFaces(rend.griddomain, rend.plandomain) }, 0);
        }
        public Bitmap Get3DImg(Model3D mdl)
        {
            return Get3DImg(new Model3D[] { mdl }, pictureBox1.Size, new Render(), new Face[][] { mdl.GetFaces() }, 0);
        }
        public Bitmap Get3DImgDflt(Model3D mdl)
        {
            Render rend = GetdefultRender();
            return Get3DImg(new Model3D[] { mdl }, pictureBox1.Size, rend, new Face[][] { mdl.GetFaces(rend.griddomain, rend.plandomain) }, 0);
        }
        public Bitmap Get3DImgs(Model3D[] mdls, Size imgsiz, Render rnd)
        {
             
             frame_start_render_time = Environment.TickCount;
              if (imgsiz.Width<=0|imgsiz.Height<=0)
              {
                  imgsiz = new Size(1, 1);
              }
            Bitmap b = new Bitmap(imgsiz.Width, imgsiz.Height); Graphics g = Graphics.FromImage(b);
            List<Face[]> areas = new List<Face[]>();
            for (int i = 0; i < mdls.Length; i++)
            {
                if (mdls[i] != null)
                {
                    if (mdls[i].IsVisible)
                    {
                        areas.Add(mdls[i].GetFaces(rnd.griddomain, rnd.plandomain));

                        if (rnd.drawboxplans != 0)
                        {
                          //  areas.Add(mdls[i].GetPlans(new PointF(0, 1), new PointF(0, 1), mdls[i].planx));
                         //   areas.Add(mdls[i].GetPlans(new PointF(0, 1), new PointF(0, 1), mdls[i].plany));
                         //   areas.Add(mdls[i].GetPlans(new PointF(0, 1), new PointF(0, 1), mdls[i].planz));
                        }
                    }


                }
            }

            if (mdls.Length > 0)
            {
                if (mdls[0] != null)
                {
                    g.DrawImage(Get3DImg(mdls, imgsiz, rnd, areas.ToArray(), 0), 0, 0);
                }
            }
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; 
            float highest_text_drawn = 0;
            float text_hight = 0;
            if (Modles.SelectedModel != null)
            {
                Model3D mdl = Modles.SelectedModel;
                int grd = Modles.SelectedModel.OrginPoints.Count;
                int pln = 0;
                if (mdl.EditedPoints.Count > 0)
                {
                    pln = mdl.OrginPoints[0].Count;
                } 
                  
                if (rnd.drawinfo)
                {
                    string s = "Vectors= " + grd + "X" + pln + " =" + (grd * pln).ToString();
                    g.DrawString(s, drawfont, new SolidBrush(Color.Red), 0, 0);
                 text_hight =g.MeasureString(s, drawfont).Height;
                 highest_text_drawn += text_hight + 1;
                   
                    s="Faces = " + mdl.FacesCount+" X 4^"+mdl.SubDivisionLevel+" = "+(mdl.FacesCount*(Math.Pow(4,mdl.SubDivisionLevel)));
                    g.DrawString(s, drawfont, new SolidBrush(Color.Red), 0, highest_text_drawn);
                    highest_text_drawn += text_hight + 1;
                    
                    s = "X=" + mdl.rotateangleww.X + ",Y=" + mdl.rotateangleww.Y + ",Z=" + mdl.rotateangleww.Z;                 
                    g.DrawString(s, drawfont, new SolidBrush(Color.Red), 0, highest_text_drawn);
                    highest_text_drawn += text_hight + 1;

                  
                    if (Getrecord())
                    {s="Recording,Time: {" + (int)((Environment.TickCount - milly) / 1000f) + "}Seconds, {" + bbs.Count + "}Frames";
                    g.DrawString(s, drawfont, new SolidBrush(Color.Red), 0, highest_text_drawn);
                    highest_text_drawn += text_hight + 1;
                    }
                } 
               
                string metext = "M.El-Kony C# Programmer 01118979196";
                //  Font markfnt = new Font(drawfont.FontFamily, 12);
                SizeF metextsz = g.MeasureString(metext, drawfont);
                PointF meloc = new PointF(imgsiz.Width - metextsz.Width, imgsiz.Height - metextsz.Height);
                /*  ColorBlend cb = new ColorBlend(7);
                 cb.Positions = new float[] { 0.0f, 0.15f, 0.35f, 0.5f, 0.65f, 0.8f, 1.0f };
                 int alp = 250;
                 cb.Colors = new Color[] { Color.FromArgb(alp, Color.Red), Color.FromArgb(alp, Color.Yellow), Color.FromArgb(alp, Color.Green), Color.FromArgb(alp, Color.Cyan), Color.FromArgb(alp, Color.Blue), Color.FromArgb(alp, Color.Purple), Color.FromArgb(alp, Color.Red) };
               LinearGradientBrush lgb = new LinearGradientBrush(new RectangleF(meloc, metextsz), Color.Red, Color.Red, 0f);
                 lgb.InterpolationColors = cb;
                 */
                g.DrawString(metext, drawfont, new SolidBrush(Color.Red), meloc);       
            }
            int now = Environment.TickCount;
           float frame_time=(float)(now - frame_start_render_time);
            if (frame_time==0){frame_time=15;}
            float fps =(1000f/frame_time);
        //    fps = fnc.cutfloat(fps,1);
            if (fps < 0) fps = 0;
            frame_start_render_time = now;
            g.DrawString("FBS = " +(int)fps, drawfont, new SolidBrush(Color.Red), 0, highest_text_drawn);
              
            return b;
        }
        Font drawfont;

        public Bitmap Get3DImgsDflt(Model3D[] mdls)
        {

            return Get3DImgs(mdls, pictureBox1.Size, GetdefultRender());
        }
        public Bitmap Get3DImg(Model3D[] mdls, Size imgsiz, Render rnd, Face[][] araes, int selecttedmodel_indx = 0)
        {

            KokMap b = new KokMap(imgsiz.Width, imgsiz.Height); //Graphics g = Graphics.FromImage(b);

           b.TranslateTransform=new PointF(imgsiz.Width / 2f, imgsiz.Height / 2f);

         //   g.CompositingQuality = CompositingQuality.HighQuality;
       

            Vector3 lightsource = rnd.lightposition;
            List<Face> area = new List<Face>();
            for (int i = 0; i < araes.Length; i++)
            {
                for (int n = 0; n < araes[i].Length; n++)
                {
                    area.Add(araes[i][n]);
                }
            }

         //   area = new List<Face>(area.OrderBy(d => d.TopLeft.Z));
         
            if (rnd.drawaxes != 0)
            {
                Vector3[][] selected_cordes = mdls[selecttedmodel_indx].cords;
                PointF xloce = selected_cordes[0][1].TOPointF();
                PointF yloce = selected_cordes[1][1].TOPointF();
                PointF zloce = selected_cordes[2][1].TOPointF();
                PointF xlocs = selected_cordes[0][0].TOPointF();
                PointF ylocs = selected_cordes[1][0].TOPointF();
                PointF zlocs = selected_cordes[2][0].TOPointF();
                Pen pn = new Pen(Color.Red, 3f);
                if (rnd.drawaxes == 1)
                {
                    xloce = yloce = zloce = new PointF(0, 0);
                }
                else
                {
                    pn.StartCap = LineCap.ArrowAnchor;
                }

                pn.EndCap = LineCap.ArrowAnchor;
                //x
                //g.DrawLine(pn, xlocs, xloce);
                //y
          //      pn.Color = Color.Green;
             //   g.DrawLine(pn, ylocs, yloce);
                //z
                pn.Color = Color.Blue;
            //    g.DrawLine(pn, zlocs, zloce);


                Font sf = new Font(this.Font.FontFamily, 15);
                xlocs.X += 2; xlocs.Y += 2;
             //   g.DrawString("X", sf, new SolidBrush(Color.Red), xlocs);
                ylocs.X += 2; ylocs.Y += 2;
             //   g.DrawString("Y", sf, new SolidBrush(Color.Green), ylocs);
                zlocs.X += 2; zlocs.Y += 2;
            //    g.DrawString("Z", sf, new SolidBrush(Color.Blue), zlocs);



            }
            for (int i = 0; i < mdls.Length; i++)
            {
                Model3D mdl = mdls[i];
                if (mdl == null) continue;
                if (mdl.IsVisible == false) continue;
                if (mdl.isselected)
                {
                    if (Modles.Count() == 1)
                    {

                   //     drawbox(ref g, mdl, false, rnd, Color.Black);
                    }
                    else
                    {
                 //       drawbox(ref g, mdl, false, rnd, Color.DarkRed);

                    }
                }
                else if (rnd.drawbox)
                {

               //     drawbox(ref g, mdl, false, rnd, Color.Black);

                }
            }
            Vector3 viewdirection = new Vector3(-1 * lightsource.X, lightsource.Y, 5000000);
            for (int i = 0; i < area.Count; i += 1)
            {
                Face face = area[i];
                PointF[] vecs2d = new PointF[3];
                      Vector3 normal1=Vector3.CrossProduct(face.TopRight - face.TopLeft,face.TopLeft- face.BottomLeft);
                      Vector3 normal3 = Vector3.CrossProduct( face.TopRight - face.BottomRight,face.BottomLeft - face.BottomRight);
                 Vector3 v1 = face.TopLeft; ;
                    Vector3 v3=face.BottomRight;
                   
                for (int tringleidx=0;tringleidx<2;tringleidx++)
                {
                    Vector3 v2 ;                 
                     Vector3 normal2;
                    if (tringleidx==0)
                    {
                        v2 = face.BottomLeft;
                     
                      normal2 = Vector3.CrossProduct(face.BottomRight -face.BottomLeft, face.TopLeft - face.BottomLeft);
                  
                    }
                    else
                    {
                        v2 = face.TopRight;

                        normal2 = Vector3.CrossProduct(face.TopLeft - face.TopRight, face.BottomRight - face.TopRight);
                      
                    }
               
              
               

             


                  

                //  Vector3D cnt= Vector3D.GetCenterVect(vecs);       
                //  Vector3D f1 = vec1 + ((vec2 - vec1) / 2f); 
                //   Vector3D f2= vec1 + ((vec4 - vec1) / 2f);


                 // (vecs[0].Normal + vecs[1].Normal + vecs[2].Normal) / 3;/// Vector3.CrossProduct(vec2 - vec1, vec3 - vec1).Normalized();
                //       Vector3D normalvetcorc =Vector3D.CrossProduct(f1 - cnt, f2 - cnt).Normalized();

                if (normal2.Z < 0)
                {
                    lightsource.Z = -5000;
                }
                else
                {
                    lightsource.Z = 5000;
                }

                Vector3 reallightposc = lightsource - v1;
                float diffuse = Vector3.DotProduct(reallightposc.Normalized(), normal2.Normalized());
                diffuse = Math.Max(0, diffuse);
                Vector3 refct = Vector3.Reflect((normal2).Normalized(), reallightposc.Normalized());

                Vector3 halfvector = reallightposc + viewdirection;
                float spec = Vector3.DotProduct(normal2.Normalized(), halfvector.Normalized());
                int idealspecpower = (int)rnd.specular;

                if (idealspecpower % 2 == 1)
                { idealspecpower += 1; }

                float specular = (float)Math.Max(0, Math.Pow(spec, idealspecpower));
                //so user be able to rempove specualr light
                if (idealspecpower > 100) { specular = 0; }

               
                int cntercolor_r = (int)((v1.color.R + v2.color.R + v3.color.R) /3f);
                int cntercolor_g = (int)((v1.color.G + v2.color.G + v3.color.G) /3f);
                int cntercolor_b = (int)((v1.color.B + v2.color.B + v3.color.B)/3f);
              Color  c = Color.FromArgb(v1.color.A, cntercolor_r, cntercolor_g, cntercolor_b);
      //        c = vec1.color;
                if (rnd.frontface)
                {
                    if (normal2.Z >= 0)
                    {
                        c = rnd.frontfacecolor;
                    }
                }
                if (rnd.backface)
                {
                    if (normal2.Z < 0)
                    {
                        c = rnd.backfacecolor;
                    }
                }
specular = 1f * 255f * specular;
                float crc = 0;
                float cgc = 0;
                float cbc = 0;
                if (rnd.Dark_Render_engine)
                {   float diif2 = 30 * diffuse;           
                    diffuse = rnd.diffuse * 2f * diffuse;
                   crc  = c.R * diffuse + diif2 + specular;
                  cgc  = c.G * diffuse + diif2 + specular;
              cbc = c.B * diffuse + diif2 + specular;
                }
                else
                {
                    diffuse = rnd.diffuse * 255f * diffuse;
                    crc = c.R+ diffuse + specular;
                    cgc = c.G + diffuse  + specular;
                    cbc = c.B+ diffuse + specular;

                }

                if (crc > 255) { crc = 255; } else if (crc < 0) { crc = 0; }
                if (cgc > 255) { cgc = 255; } else if (cgc < 0) { cgc = 0; }
                if (cbc > 255) { cbc = 255; } else if (cbc < 0) { cbc = 0; }
                Color cc = Color.FromArgb(c.A, (int)crc, (int)cgc, (int)cbc);
                if ((v1).Z > 280)
                {
                    continue;
                } Color linecolor = Color.Black;
                if (v1.ismodelwall)
                {
                    if (rnd.drawboxplans == 1 || rnd.drawboxplans == 3)
                    {
                        if (rnd.colorlines)
                        { linecolor = cc; }
                        b.DrawLine(Color.FromArgb(cc.A, linecolor), v1, v2);
                        //  g.DrawLine(new Pen(Color.FromArgb(cc.A, linecolor), 1), vec2.TOPointF(), vec3.TOPointF());
                    }
                }
                else if (rnd.lines)
                {
                    if (rnd.colorlines)
                    { linecolor = cc; }
                    //  g.DrawLines(new Pen(Color.FromArgb(cc.A, linecolor), 1), vecs2d);
                    Vector3 up=new Vector3(0,0,-1);
                    b.DrawLine(Color.FromArgb(cc.A, linecolor), v1+up, v2+up);
                    //  g.DrawLine(new Pen(Color.FromArgb(cc.A, linecolor), 1), vec2.TOPointF(), vec3.TOPointF());

                }
               // if (normal2.Z < 0)
                {




                    if (rnd.normalvector)
                    { b.DrawLine(Color.Blue, v1, (normal2.Normalized().HitBy(50) + v1)); }
                    if (rnd.reflectedray)
                    { b.DrawLine(Color.Green, v1.TOPointF(), (refct.HitBy(50) + v1).TOPointF()); }
                    if (rnd.lightray)
                    { b.DrawLine(Color.Black, v1.TOPointF(), (lightsource / 200f).TOPointF()); }
                    if (rnd.eyeray)
                    { b.DrawLine(Color.Red, v1.TOPointF(), (viewdirection / 200f).TOPointF()); }
                    if (v1.ismodelwall)
                    {
                        if (rnd.drawboxplans == 2 || rnd.drawboxplans == 3)
                        {
                          //  g.FillPolygon(new SolidBrush(cc), vecs2d);
                        }
                    }
                    else if (rnd.fillplans)
                    {

                        b.DrawTriangle(new Vertex(v1, normal1), new Vertex(v2, normal2), new Vertex(v3, normal3), c);
                       // g.FillPolygon(new SolidBrush(cc), vecs2d);
                    }

                }
              /*  else
                {
                    if (vec1.ismodelwall)
                    {
                        if (rnd.drawboxplans == 2 || rnd.drawboxplans == 3)
                        {
                            g.FillPolygon(new SolidBrush(cc), vecs2d);
                        }
                    }
                    else if (rnd.fillplans)
                    {
                        g.FillPolygon(new SolidBrush(cc), vecs2d);
                    }
                    if (rnd.normalvector)
                    { g.DrawLine(new Pen(Color.Blue, 2f), vec1.TOPointF(), (normal2.HitBy(50) + vec1).TOPointF()); }
                    if (rnd.reflectedray)
                    { g.DrawLine(new Pen(Color.Green, 2f), vec1.TOPointF(), (refct.HitBy(50) + vec1).TOPointF()); }
                    if (rnd.lightray)
                    { g.DrawLine(new Pen(Color.Black, 2f), vec1.TOPointF(), (lightsource / 200f).TOPointF()); }
                    if (rnd.eyeray)
                    { g.DrawLine(new Pen(Color.Red, 2f), vec1.TOPointF(), (viewdirection / 200f).TOPointF()); }
                }*/
                    
                                    Color pointcolor = Color.Black;
                                    if (rnd.highlight_selectedpoints)
                                    {
                                        for (int k = 0; k < mdls.Length; k++)
                                        {
                                            if (mdls[k] == null) { continue; }
                                            for (int kk = 0; kk < mdls[k].selectionindxs.Count; kk++)
                                            {
                                                int[] idxs = mdls[k].selectionindxs[kk];
                                                if (mdls[k].EditedPoints[idxs[0]][idxs[1]] == v1 && v1.isdeleted == false)
                                                {

                                                    pointcolor = Color.FromArgb(230, 50, 0);
                                                    if (kk == 0)
                                                    {
                                                        pointcolor = Color.Blue;
                          /*                              g.TranslateTransform(-1 * imgsiz.Width / 2f, -1 * imgsiz.Height / 2f);

                                                        GraphCords(Get3dpointcords(mdls[k].cords, new SizeF3D(500, 500, 500), vec1), 500 * 2, mdls[k], false, 2, true, g);
                                                        g.TranslateTransform(imgsiz.Width / 2f, imgsiz.Height / 2f);
*/
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (rnd.points || rnd.highlight_selectedpoints)
                                    {
                                      //  if (vec1.isdeleted == false)
                                        {
                                            b.DrawLine(pointcolor, v1.X - 2, v1.Y, v1.X + 2, v1.Y);
                                                    b.DrawLine(pointcolor, v1.X , v1.Y-2, v1.X, v1.Y+2);
                                        //    g.FillEllipse(new SolidBrush(pointcolor), vec1.X - 2, vec1.Y - 2, 4, 4);
                                        }
                                    }
                                        
                    //   g.SmoothingMode = SmoothingMode.AntiAlias;
               

          
              //      g.SmoothingMode = SmoothingMode.None;
              


            } 
            }

            
            for (int i = 0; i < mdls.Length; i++)
            {
 Color pointcolor = Color.Black;
           
                if (rnd.highlight_selectedpoints)
                {

                    if (mdls[i] != null)
                    {
                        if (mdls[i].selectionindxs.Count > 0)
                        {
                            int[] idxs = mdls[i].selectionindxs[0];
                            Vector3 pnt = mdls[i].EditedPoints[idxs[0]][idxs[1]];
                            if (pnt.isdeleted == false)
                            {
                                pointcolor = Color.FromArgb(230, 50, 0);

                            //    g.TranslateTransform(-1 * imgsiz.Width / 2f, -1 * imgsiz.Height / 2f);

                           //     GraphCords(Get3dpointcords(mdls[i].cords, new SizeF3D(500, 500, 500), pnt), 500 * 2, mdls[i], false, 2, true, g);
                          //      g.TranslateTransform(imgsiz.Width / 2f, imgsiz.Height / 2f);
                            }



                        }
                    }
                }

                if (rnd.points)
                {
                    for (int kk = 0; kk < mdls[i].EditedPoints.Count; kk++)
                    {
                        for (int kkk = 0; kkk < mdls[i].EditedPoints[kk].Count; kkk++)
                        {
                            Vector3 pnt = mdls[i].EditedPoints[kk][kkk];
                          
                            if (pnt.isdeleted == false)
                            {
                                b.DrawLine(pointcolor, pnt.X - 2, pnt.Y, pnt.X + 2, pnt.Y);
                                b.DrawLine(pointcolor, pnt.X , pnt.Y-2, pnt.X, pnt.Y+2);
                        
                             //  g.FillEllipse(new SolidBrush(pointcolor), pnt.X - 2, pnt.Y - 2, 4, 4);
                            }
                        }
                    }
                }
//-------------------






                Model3D mdl = mdls[i];
                if (mdl == null) continue;
                if (mdl.IsVisible == false) continue;
                if (mdl.isselected)
                {
                    if (Modles.Count() == 1)
                    {

                     //   drawbox(ref g, mdl, true, rnd, Color.Black);
                    }
                    else
                    {
                 //       drawbox(ref g, mdl, true, rnd, Color.DarkRed);

                    }
                }
                else if (rnd.drawbox)
                {

               //     drawbox(ref g, mdl, true, rnd, Color.Black);

                }
            }

            return b.ToBitmap();

        }
        void drawbox(ref Graphics g, Model3D mdl, bool front, Render rnd, Color linecol)
        {
            for (int w = 0; w < mdl.Boxedges.Length; w++)
            {
                Color c = linecol;
                Pen pp = new Pen(c);


                if (w == 11)
                {
                    c = Color.Red; pp.Width = 4; pp.EndCap = LineCap.ArrowAnchor;
                }
                else if (w == 5)
                {
                    c = Color.Green; pp.Width = 4; pp.StartCap = LineCap.ArrowAnchor;
                }
                else if (w == 6)
                {
                    c = Color.Blue; pp.Width = 4; pp.EndCap = LineCap.ArrowAnchor;

                }

                if (rnd.drawbox)
                {
                    bool draw = false;
                    if (front)
                    {

                        if (mdl.Boxedges[w][0].Z + mdl.Boxedges[w][1].Z / 2f >= 0)
                        {
                            draw = true;

                        }
                    }
                    else
                    {
                        if (mdl.Boxedges[w][0].Z + mdl.Boxedges[w][1].Z / 2f < 0)
                        {
                            draw = true;

                        }
                    }
                    if (draw == true)
                    {
                        pp.Color = c;

                        g.DrawLine(pp, mdl.Boxedges[w][0].TOPointF(), mdl.Boxedges[w][1].TOPointF());
                    }
                }
            }
        }


        public Bitmap DrawImages(Bitmap[] objs, Size sz)
        {

            Bitmap b = new Bitmap(sz.Width, sz.Height);
            Graphics g = Graphics.FromImage(b);
            for (int i = 0; i < objs.Length; i++)
            {
                g.DrawImage(objs[i], 0, 0);
            }
            return b;
        }


        private void GraphCords(Model3D mdl, bool dash, float wid, bool drawstring, Graphics g)
        {
            g.DrawImage(Get3DCordsLines(mdl.cords, dash, wid, drawstring, (int)(mdl.CordsLenght * 2)), centerdrawlocation.X - mdl.CordsLenght, centerdrawlocation.Y - mdl.CordsLenght);
        }
        private void GraphCords(Model3D mdl, Graphics g)
        {
            g.DrawImage(Get3DCordsLines(mdl.cords, (int)(mdl.CordsLenght * 2)), centerdrawlocation.X - mdl.CordsLenght, centerdrawlocation.Y - mdl.CordsLenght);
        }
        private void GraphCords(Vector3[][] cords, float cordslenght, Model3D mdl, bool dash, float wid, bool drawstring, Graphics g)
        {
            g.DrawImage(Get3DCordsLines(cords, dash, wid, drawstring, (int)(cordslenght * 2)), centerdrawlocation.X - cordslenght, centerdrawlocation.Y - cordslenght);
        }
        private void GraphCords(Vector3[][] cords, float cordslenght, Model3D mdl, Graphics g)
        {

            g.DrawImage(Get3DCordsLines(cords, (int)(cordslenght * 2)), centerdrawlocation.X - cordslenght, centerdrawlocation.Y - cordslenght);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        int sizescale = 280;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Render rnd = GetdefultRender();
            Model3D mdl = this.Modles.SelectedModel;
            if (istreadalive(thrd_loadeqs)) return;
            if (mdl == null) return;

            if (e.Button == MouseButtons.Right && isonselection == false)
            {
                if (mdl != null)
                {

                    Modles.SelectedModel.Locate(new PointF3D(e.Location.X - pictureBox1.Width / 2f, e.Location.Y - pictureBox1.Height / 2f, 0));

                    PointF3D loc = new PointF3D();
                    loc.X = e.Location.X;
                    loc.Y = e.Location.Y;
                    //   Modles.SelectedModel.Locate(loc);

                    Regraph();
                }
                return;
            }

            if (is_moving_points)
            {//move selected points
                if (isonselection)
                {

                    PointF trns = centerdrawlocation;



                    Point pd = new Point(e.Location.X - cp.X, e.Location.Y - cp.Y);


                    //========
                    for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                    {
                        int[] indxs = mdl.selectionindxs[i];
                        Vector3 unedit_targ = (Vector3)mdl.selection_unedited_pnts[i].ToV();
                        Vector3 unedit_orgn_targ = (Vector3)mdl.selection_unedited_orgn_pnts[i].ToV();
                        Vector3[][] orgn_cords = new Vector3[3][];
                        orgn_cords[0] = new Vector3[2]; orgn_cords[1] = new Vector3[2]; orgn_cords[2] = new Vector3[2];
                        orgn_cords[0][0] = new Vector3(-100, 0, 0);
                        orgn_cords[0][1] = new Vector3(100, 0, 0);
                        orgn_cords[1][0] = new Vector3(0, -100, 0);
                        orgn_cords[1][1] = new Vector3(0, 100, 0);
                        orgn_cords[2][0] = new Vector3(0, 0, -100);
                        orgn_cords[2][1] = new Vector3(0, 0, 100);

                        Vector3 moved_unedit_targ = Move3DPoint(mdl.cords, new float[3] { pd.X, pd.Y, 0 }, unedit_targ);
                        Vector3 moved_unedit_orgn_targ = Move3DPoint(orgn_cords, new float[3] { pd.X / mdl.Size.Width * mdl.OrginRec.Width, pd.Y / mdl.Size.Height * mdl.OrginRec.Height, 0 }, unedit_orgn_targ);


                        Vector3 targ = mdl.EditedPoints[indxs[0]][indxs[1]];
                        targ.X = moved_unedit_targ.X;
                        targ.Y = moved_unedit_targ.Y;
                        targ.Z = moved_unedit_targ.Z;
                        Vector3 orgn_targ = mdl.OrginPoints[indxs[0]][indxs[1]];

                        orgn_targ.X = moved_unedit_orgn_targ.X;
                        orgn_targ.Y = moved_unedit_orgn_targ.Y;
                        orgn_targ.Z = moved_unedit_orgn_targ.Z;



                    }
                    mdl.SubDivisionLevel = mdl.SubDivisionLevel;
                    Regraph(true);



                }

            }
            else if (iseditmode())
            {//draw selction rec and selcet points
                if (isdrag)
                {
                    centerdrawlocation = new PointF(pictureBox1.Width / 2f, pictureBox1.Height / 2f);
                    Point crntp = e.Location;
                    if (crntp.X < 0) crntp.X = 0;
                    if (crntp.Y < 0) crntp.Y = 0;
                    if (crntp.X > this.Width) crntp.X = this.Width;
                    if (crntp.Y > this.Height) crntp.Y = this.Height;

                    if (crntp.X > cp.X)
                    {
                        selectionrec.X = cp.X; selectionrec.Width = crntp.X - cp.X;
                    }
                    else
                    {
                        selectionrec.X = crntp.X; selectionrec.Width = cp.X - crntp.X;
                    }

                    if (crntp.Y > cp.Y)
                    {
                        selectionrec.Y = cp.Y; selectionrec.Height = crntp.Y - cp.Y;
                    }
                    else
                    {
                        selectionrec.Y = crntp.Y; selectionrec.Height = cp.Y - crntp.Y;
                    }

                    Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(b);

                    g.DrawImage(Get3DImgsDflt(Modles.Models.ToArray()), 0, 0);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 250, 250)), selectionrec);
                    g.DrawRectangle(new Pen(Color.FromArgb(150, 0, 250, 250)), Rectangle.Round(selectionrec));

                    g.TranslateTransform(centerdrawlocation.X, centerdrawlocation.Y);

                    for (int i = (int)rnd.griddomain.X; i < (int)(mdl.EditedPoints.Count * rnd.griddomain.Y); i++)
                    {
                        for (int n = (int)rnd.plandomain.X; n < (int)(mdl.EditedPoints[i].Count * rnd.plandomain.Y); n++)
                        {
                            Vector3 p = mdl.EditedPoints[i][n];
                            if (p.isdeleted) continue;
                            Color c = Color.FromArgb(200, 0, 0, 0);
                            if (selectionrec.Contains(p.X + centerdrawlocation.X, p.Y + centerdrawlocation.Y))
                            {
                                isonselection = true;
                                c = Color.FromArgb(230, 50, 0);
                            }
                            g.FillEllipse(new SolidBrush(c), p.X - 3f, p.Y - 3f, 6f, 6f);
                        }
                    }





                    pictureBox1.BackgroundImage = b;
                    pictureBox1.Refresh();
                }
                else
                {//dectet point while moving                  
                }
            }
            else if (isdrag)
            {
                lp = cp; cp = e.Location;
                PointF rotatep = new PointF(cp.X - lp.X, cp.Y - lp.Y);
                ismove = true;
                if (GetRotateAll())
                {
                    for (int i = 0; i < Modles.Count(); i++)
                    {
                        if (Modles.Models[i] != null)
                        {
                            Modles.Models[i].MuliplyRotate(rotatep);
                        }
                    }
                }
                else if (mdl != null)
                {
                    mdl.MuliplyRotate(rotatep);
                }
                Point p = new Point((int)mdl.rotateangleww.X, (int)mdl.rotateangleww.Y);
                if (p.X >= 0 && p.X <= 360)
                {
                    anglex.Value = p.Y;
                }
                if (p.Y >= 0 && p.Y <= 360)
                {
                    angley.Value = p.X;
                }
                angles_prevalue = new PointF3D(p.Y, p.X, anglez.Value);
                Regraph();

            }
            else //select active modle
            {
                if (istreadalive(rotate3dthread) == false)
                {
                    bool isslected = false;

                    PointF p = new PointF(e.Location.X - pictureBox1.Width / 2f, e.Location.Y - pictureBox1.Height / 2f);

                    Model3D[] mdls = Modles.Models.ToArray();

                    List<Face[]> areas = new List<Face[]>();
                    List<int> areasmodel = new List<int>();
                    for (int d = 0; d < mdls.Length; d++)
                    {
                        if (mdls[d] != null)
                        {
                            if (mdls[d].IsVisible)
                            {
                                areas.Add(mdls[d].GetFaces(rnd.griddomain, rnd.plandomain));
                            }
                        }
                    }
                    List<Plan> area = new List<Plan>();
                    for (int i = 0; i < areas.Count; i++)
                    {
                        for (int n = 0; n < areas[i].Length; n++)
                        {
                           // Vector3[] m = areas[i][n];
                         //   Plan pln = new Plan(m, i);
                          //  area.Add(pln);
                        }
                    }
                    area = new List<Plan>(area.OrderBy(d => Vector3.GetCenterVect(d.vectors).Z));
                    PointF[][] pps = new PointF[area.Count][];
                    for (int d = area.Count - 1; d >= 0; d--)
                    {
                        Plan pln = area[d];
                        pps[d] = new PointF[3] { pln.vectors[0].TOPointF(), pln.vectors[1].TOPointF(), pln.vectors[2].TOPointF() };
                        GraphicsPath gp = new GraphicsPath();
                        gp.AddPolygon(pps[d]);
                        if (gp.IsVisible(p))
                        {
                            Modles.SelectedIndex = pln.indx;
                            Model3D selectedmodel = Modles.SelectedModel;

                            colorPicker_locate.HangValueChangingEvent = colorPicker_locate.HangValueChangedEvent = true;
                            colorPicker_locate.Value = new Point((int)selectedmodel.TranslatedLoc.X, (int)selectedmodel.TranslatedLoc.Y);
                            colorPicker_locate.HangValueChangingEvent = colorPicker_locate.HangValueChangedEvent = false;
                            kscale_relocate_z.Hangvaluechangedevent = true;
                            kscale_relocate_z.ValueF = mdl.TranslatedLoc.Z;
                            kscale_relocate_z.Hangvaluechangedevent = false;

                            this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = true;
                            this.kscale_edit_width.ForcedSetValue(mdl.Size_With_Out_Rotation.Width);
                            this.kscale_edit_height.ForcedSetValue(mdl.Size_With_Out_Rotation.Height);
                            this.kscale_edit_thick.ForcedSetValue(mdl.Size_With_Out_Rotation.Thick);

                            this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = false;

                            Regraph();
                            isslected = true;
                            break;
                        }

                    }

                }
            }
        }

        PointF centerdrawlocation = new PointF();
        bool isonselection = false;
        RectangleF selectionrec = new RectangleF();

        PointF mainrotatepoint = new PointF();
        Point lp = new Point();
        Point cp = new Point();
        Thread rotate3dthread;
        bool is_moving_points = false;
        bool isdrag = false;
        bool ismove = false;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (istreadalive(thrd_loadeqs)) return;
            if (mdl == null) return;


            startrotatethread(new Point(), false);
            isdrag = true;
            cp = e.Location;
            is_moving_points = false;
            if (iseditmode())
            {
                for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                {
                    PointF p = new PointF(mdl.selection_unedited_pnts[i].X, mdl.selection_unedited_pnts[i].Y);
                    p.X += centerdrawlocation.X; p.Y += centerdrawlocation.Y;

                    if (e.Location.X < p.X + 3 && e.Location.X > p.X - 3)
                    {
                        if (e.Location.Y < p.Y + 3 && e.Location.Y > p.Y - 3)
                        {
                            is_moving_points = true;
                        }

                    }
                }
            }



        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (istreadalive(thrd_loadeqs)) return;
            if (mdl == null) return;


            isonselection = false;
            pictureBox1.ContextMenuStrip = null;


            if (iseditmode() == true && isdrag == true)
            {

                mdl.selectionindxs.Clear();
                mdl.selection_unedited_pnts.Clear();
                mdl.selection_unedited_orgn_pnts.Clear();
                Render rnd = GetdefultRender();


                for (int i = (int)(mdl.EditedPoints.Count * rnd.griddomain.X); i < (int)(mdl.EditedPoints.Count * rnd.griddomain.Y); i++)
                {
                    for (int n = (int)(mdl.EditedPoints.Count * rnd.plandomain.X); n < (int)(mdl.EditedPoints[i].Count * rnd.plandomain.Y); n++)
                    {
                        Vector3 p = mdl.EditedPoints[i][n];
                        if (p.isdeleted) continue;
                        if (selectionrec.Contains(p.X + centerdrawlocation.X, p.Y + centerdrawlocation.Y))
                        {
                            pictureBox1.ContextMenuStrip = contextMenuStrip3;
                            isonselection = true;
                            mdl.selection_unedited_pnts.Add(p.TOPointF3SS());
                            mdl.selection_unedited_orgn_pnts.Add(mdl.OrginPoints[i][n].TOPointF3SS());

                            mdl.selectionindxs.Add(new int[2] { i, n });

                        }

                    }
                }



                Regraph(true);
            }
            else if (iseditmode() == false && e.Button == MouseButtons.Left && ismove)
            {
                Point rotspeed = new Point(cp.X - lp.X, cp.Y - lp.Y);
                if (rotspeed.X > 2 || rotspeed.X < -2 || rotspeed.Y > 2 || rotspeed.Y < -2)
                {
                    int max = 10;
                    if (rotspeed.X > max)
                    { rotspeed.X = max; }
                    else if (rotspeed.X < -max)
                    { rotspeed.X = -max; }

                    if (rotspeed.Y > max)
                    { rotspeed.Y = max; }
                    else if (rotspeed.Y < -max)
                    { rotspeed.Y = -max; }
                    startrotatethread(rotspeed, true);
                }
                else
                {
                    startrotatethread(rotspeed, false);
                }
            }
            else if (iseditmode() && e.Button == MouseButtons.Left)
            {
                Point pd = new Point(e.Location.X - cp.X, e.Location.Y - cp.Y);

                kscale_editx.SafeSetValue(pd.X);
                kscale_edity.SafeSetValue(pd.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {

            }
            ismove = false;
            is_moving_points = false;
            isdrag = false;
        }

        void h(object modlesdomain)
        {
            this.Invoke(new Action(() =>
            {
                this.Text = "Konk 3D Equation Render" + " , " + "Generating...";
                this.Cursor = Cursors.AppStarting;
            }));

            int matherrors = 0;
            List<string> errors = new List<string>();
            Point rang = (Point)modlesdomain;
            for (int i = rang.X; i < rang.X + rang.Y; i++)
            {
                float nowprog = 0;
                EquationForm eqform = flowLayoutPanel_modles.Controls[i] as EquationForm;

                switch (eqform.Genetarion_Type)
                {
                    case EquationForm.GenerationType.Model:
                        try
                        {
                            Model3D loadedmdl = eqform.GetLoadModel().Models[eqform.Innermodelindex];
                            if (loadedmdl != null)
                            {
                                Modles.Models[i] = loadedmdl;
                            }
                            else
                            {
                                MessageBox.Show("Error in reading model number(" + i + ")");

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error in reading model number(" + i + ") from hard \n details:" + ex.Message);
                        }
                        break;
                    case EquationForm.GenerationType.Desigen:
                         PointF[] pnts2 = eqform.DesigenPoints.ToArray();
                        if (pnts2.Length<2)
                        {
                            MessageBox.Show("Must be at least 2 points to create model");
                            continue;
                        } 
                        if (eqform.Desigen_generatefromline==false)
                         {
                             GraphicsPath gp2 = new GraphicsPath();
                             gp2.AddCurve(pnts2);
                             pnts2 = gp2.PathPoints;
                         }
                       pnts2 = KGrphs.resizepoints(pnts2, new SizeF(1, 1),false);
                      
                        Model3D md2 = new Model3D();
                        PointF3D pmin2 = new PointF3D(); PointF3D pmax2 = new PointF3D();
                        RectangleF bnds2 = KGrphs.Recofpnts(pnts2);
                        EquationFile eqf2 = eqform.GetDesigenEquationFile();
                        List<int[][]> list_back_closed_faces = new List<int[][]>();
                        if (eqform.Desigen_back_closed_face)
                        {
                            for (int n = 0; n < eqform.DesigenClosedFaces.Count; n++)
                            {
                                List<Vector3> lst1 = new List<Vector3>();
                                List<Vector3> lst2 = new List<Vector3>();
                                PointF[] cpnts = new PointF[4];
                                cpnts[0] = pnts2[eqform.DesigenClosedFaces[n][0]];
                                cpnts[1] = pnts2[eqform.DesigenClosedFaces[n][1]];
                                cpnts[2] = pnts2[eqform.DesigenClosedFaces[n][2]];
                                cpnts[3] = pnts2[eqform.DesigenClosedFaces[n][3]];

                                cpnts = cpnts.OrderBy(d => d.X).ToArray().Reverse().ToArray();
                                PointF p11 = cpnts[0].Y > cpnts[1].Y ? cpnts[0] : cpnts[1];
                                PointF p12 = cpnts[0].Y > cpnts[1].Y ? cpnts[1] : cpnts[0];
                                PointF p21 = cpnts[2].Y > cpnts[3].Y ? cpnts[2] : cpnts[3];
                                PointF p22 = cpnts[2].Y > cpnts[3].Y ? cpnts[3] : cpnts[2];

                                float lsatz = eqf2.DomainV.X;

                                lst1.Add(new Vector3(p11.X - bnds2.Width / 2f, p11.Y - bnds2.Height / 2f, lsatz));
                                lst1.Add(new Vector3(p12.X - bnds2.Width / 2f, p12.Y - bnds2.Height / 2f, lsatz));
                                lst1.Add(new Vector3(p22.X - bnds2.Width / 2f, p22.Y - bnds2.Height / 2f, lsatz));
  lst1.Add(new Vector3(p21.X - bnds2.Width / 2f, p21.Y - bnds2.Height / 2f, lsatz));
                              
                                md2.OrginPoints.Add(lst1);
                         //       md2.OrginPoints.Add(lst2);


                            }
                            for (int a = 0; a < md2.OrginPoints.Count; a +=1)
                            {
                                List<int[][]> list_faces_indxes = new List<int[][]>();

                                int[][] faces_indexs = new int[4][];
                                faces_indexs[0] = new int[2] { a, 0 };
                                faces_indexs[1] = new int[2] { a , 1 };
                                faces_indexs[2] = new int[2] { a ,2 };
                                faces_indexs[3] = new int[2] { a,3 };

                                list_faces_indxes.Add(faces_indexs);

                                md2.FacesIndex.Add(list_faces_indxes);

                            }}
                            bool vlaststepdone2 = false;
                            for (float v = eqf2.DomainV.X; v <= eqf2.DomainV.Y; v += eqf2.Stepv)
                            {
                                List<Vector3> inps = new List<Vector3>();

                                for (int u = 0; u < pnts2.Length; u += 1)
                                {
                                    Vector3 f = new Vector3(); f.uv = new PointF(u, v);
                                    f.X = pnts2[u].X; f.Y = pnts2[u].Y;
                                    f.Z = EquationSolver.SolveEQ(eqf2.ParamZ, new string[] { "x", "y", "v" }, new float[] { f.X, f.Y, v }, false);
                                    f.Y = EquationSolver.SolveEQ(eqf2.ParamY, new string[] { "x", "y", "v" }, new float[] { f.X, f.Y, v }, false);
                                    f.X = EquationSolver.SolveEQ(eqf2.ParamX, new string[] { "x", "y", "v" }, new float[] { f.X, f.Y, v }, false);
                                    f.X -= bnds2.Width / 2f;
                                    f.Y -= bnds2.Height / 2f;
                                    inps.Add(f);
                                    if (f.X > pmax2.X) { pmax2.X = f.X; }
                                    if (f.X < pmin2.X) { pmin2.X = f.X; }
                                    if (f.Y > pmax2.Y) { pmax2.Y = f.Y; }
                                    if (f.Y < pmin2.Y) { pmin2.Y = f.Y; }
                                    if (f.Z > pmax2.Z) { pmax2.Z = f.Z; }
                                    if (f.Z < pmin2.Z) { pmin2.Z = f.Z; }

                                }
                                if (inps.Count > 0)
                                {
                                    md2.OrginPoints.Add(inps);
                                }
                                if (v + eqf2.Stepv > eqf2.DomainV.Y && vlaststepdone2 == false)
                                {
                                    v = eqf2.DomainV.Y - eqf2.Stepv; vlaststepdone2 = true;
                                }
                            }
                            int back_faces_count = eqform.DesigenClosedFaces.Count ;
                            if (eqform.Desigen_back_closed_face == false) { back_faces_count = 0; }
                            for (int a = back_faces_count; a < md2.OrginPoints.Count; a++)
                            {
                                List<int[][]> list_faces_indxes = new List<int[][]>();
                                for (int b = 0; b < md2.OrginPoints[a].Count; b++)
                                {
                                    int[][] faces_indexs = new int[4][];
                                    faces_indexs[0] = new int[2] { a, b };
                                    if (md2.OrginPoints.Count > a + 1)
                                    {
                                        if (md2.OrginPoints[a + 1].Count > b)
                                        {
                                            faces_indexs[1] = new int[2] { a + 1, b };
                                        }
                                        else { continue; }

                                        if (md2.OrginPoints[a + 1].Count > b + 1)
                                        {
                                            faces_indexs[2] = new int[2] { a + 1, b + 1 };
                                        }
                                        else { continue; }

                                    }
                                    else { continue; }
                                    if (md2.OrginPoints[a].Count > i + 1)
                                    {
                                        faces_indexs[3] = new int[2] { a, b + 1 };
                                    }
                                    else { continue; }
                                    if (a + 2 >= 51)
                                    {

                                    }
                                    list_faces_indxes.Add(faces_indexs);
                                }

                                if (list_faces_indxes.Count > 0)
                                {
                                    md2.FacesIndex.Add(list_faces_indxes);
                                }
                            }

                            if (eqform.Desigen_front_closed_face)
                            {
                                for (int n = 0; n < eqform.DesigenClosedFaces.Count; n++)
                                {
                                    List<Vector3> lst1 = new List<Vector3>();
                                    List<Vector3> lst2 = new List<Vector3>();
                                    PointF[] cpnts = new PointF[4];
                                    cpnts[0] = pnts2[eqform.DesigenClosedFaces[n][0]];
                                    cpnts[1] = pnts2[eqform.DesigenClosedFaces[n][1]];
                                    cpnts[2] = pnts2[eqform.DesigenClosedFaces[n][2]];
                                    cpnts[3] = pnts2[eqform.DesigenClosedFaces[n][3]];

                                    cpnts = cpnts.OrderBy(d => d.X).ToArray();
                                    PointF p11 = cpnts[0].Y > cpnts[1].Y ? cpnts[0] : cpnts[1];
                                    PointF p12 = cpnts[0].Y > cpnts[1].Y ? cpnts[1] : cpnts[0];
                                    PointF p21 = cpnts[2].Y > cpnts[3].Y ? cpnts[2] : cpnts[3];
                                    PointF p22 = cpnts[2].Y > cpnts[3].Y ? cpnts[3] : cpnts[2];

                                    float lsatz = eqf2.DomainV.Y;

                                    lst1.Add(new Vector3(p11.X - bnds2.Width / 2f, p11.Y - bnds2.Height / 2f, lsatz));
                                    lst1.Add(new Vector3(p12.X - bnds2.Width / 2f, p12.Y - bnds2.Height / 2f, lsatz));
                                    lst2.Add(new Vector3(p21.X - bnds2.Width / 2f, p21.Y - bnds2.Height / 2f, lsatz));
                                    lst2.Add(new Vector3(p22.X - bnds2.Width / 2f, p22.Y - bnds2.Height / 2f, lsatz));

                                    md2.OrginPoints.Add(lst1);
                                    md2.OrginPoints.Add(lst2);


                                } int front_faces_count = eqform.DesigenClosedFaces.Count * 2;
                                if (eqform.Desigen_front_closed_face == false) { front_faces_count = 0; }
                          
                                for (int a = md2.OrginPoints.Count - front_faces_count ; a < md2.OrginPoints.Count; a += 2)
                                {
                                    List<int[][]> list_faces_indxes = new List<int[][]>();

                                    int[][] faces_indexs = new int[4][];
                                    faces_indexs[0] = new int[2] { a, 0 };
                                    faces_indexs[1] = new int[2] { a + 1, 0 };
                                    faces_indexs[2] = new int[2] { a + 1, 0 + 1 };
                                    faces_indexs[3] = new int[2] { a, 0 + 1 };

                                    list_faces_indxes.Add(faces_indexs);

                                    md2.FacesIndex.Add(list_faces_indxes);
                                }
                            
                        }
                    

                         md2.OrginRec = new RectangleF3D(pmin2, new SizeF3D(pmax2.X - pmin2.X, pmax2.Y - pmin2.Y, pmax2.Z - pmin2.Z));

                        float maxmax2 = Math.Max(md2.OrginRec.Width, Math.Max(md2.OrginRec.Height, md2.OrginRec.Thick));
                        if (maxmax2 == 0) { maxmax2 = 1; }
                        SizeF3D realscalesz_t = new SizeF3D(
                                (md2.OrginRec.Width / maxmax2) * sizescale,
                               (md2.OrginRec.Height / maxmax2) * sizescale,
                              (md2.OrginRec.Thick / maxmax2) * sizescale);


                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = true;
                        this.kscale_edit_width.Invoke(new Action(() => this.kscale_edit_width.ForcedSetValue(realscalesz_t.Width)));
                        this.kscale_edit_height.Invoke(new Action(() => this.kscale_edit_height.ForcedSetValue(realscalesz_t.Height)));
                        this.kscale_edit_thick.Invoke(new Action(() => this.kscale_edit_thick.ForcedSetValue(realscalesz_t.Thick)));
                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = false;

                        md2.Resize(realscalesz_t);
                        md2.ReColor(Defulttextures);



                        Modles.Models[i] = md2;
                        break;
                    case EquationForm.GenerationType.Text:
                        string stng = eqform.Text_to_generate;
                        GraphicsPath gp = new GraphicsPath();
                        StringFormat sf = new StringFormat(); sf.LineAlignment = sf.Alignment = StringAlignment.Center;
                        Size sz = new Size(); pictureBox1.Invoke(new Action(() => { sz = pictureBox1.Size; }));
                        Font fnt = eqform.Font_of_text_to_generate;
                        gp.AddString(stng, fnt.FontFamily, (int)fnt.Style, fnt.Size, new PointF(0, 0), sf);



                        PointF[] pnts1 = gp.PathPoints;
                        Model3D md1 = new Model3D();
                        PointF3D pmin1 = new PointF3D(); PointF3D pmax1 = new PointF3D();
                        RectangleF bnds1 = gp.GetBounds();
                        for (float v = -1f; v <= 1f; v += 0.5f)
                        {

                            List<Vector3> inps = new List<Vector3>();

                            for (int u = 0; u < pnts1.Length; u += 1)
                            {
                                Vector3 f = new Vector3(); f.uv = new PointF(u, v);
                                f.X = pnts1[u].X; f.Y = pnts1[u].Y;
                                f.Z = EquationSolver.SolveEQ(eqform.Z_equation_of_Text_to_generate, new string[] { "u", "v" }, new float[] { u, v }, false);
                                f.X -= bnds1.Width / 2f;
                                f.Y -= bnds1.Height / 2f;
                                inps.Add(f);
                                if (f.X > pmax1.X) { pmax1.X = f.X; }
                                if (f.X < pmin1.X) { pmin1.X = f.X; }
                                if (f.Y > pmax1.Y) { pmax1.Y = f.Y; }
                                if (f.Y < pmin1.Y) { pmin1.Y = f.Y; }
                                if (f.Z > pmax1.Z) { pmax1.Z = f.Z; }
                                if (f.Z < pmin1.Z) { pmin1.Z = f.Z; }
                            }
                            if (inps.Count > 0)
                            {
                                md1.OrginPoints.Add(inps);
                            }
                        }

                        for (int a = 0; a < md1.OrginPoints.Count; a++)
                        {
                            List<int[][]> list_faces_indxes = new List<int[][]>();
                            for (int b = 0; b < md1.OrginPoints[a].Count; b++)
                            {
                                int[][] faces_indexs = new int[4][];
                                faces_indexs[0] = new int[2] { a, b };
                                if (md1.OrginPoints.Count > a + 1)
                                {
                                    if (md1.OrginPoints[a + 1].Count > b)
                                    {
                                        faces_indexs[1] = new int[2] { a + 1, b };
                                    }
                                    else { continue; }

                                    if (md1.OrginPoints[a + 1].Count > b + 1)
                                    {
                                        faces_indexs[2] = new int[2] { a + 1, b + 1 };
                                    }
                                    else { continue; }

                                }
                                else { continue; }
                                if (md1.OrginPoints[a].Count > i + 1)
                                {
                                    faces_indexs[3] = new int[2] { a, b + 1 };
                                }
                                else { continue; }
                                list_faces_indxes.Add(faces_indexs);
                            }
                            md1.FacesIndex.Add(list_faces_indxes);
                        }

                        md1.OrginRec = new RectangleF3D(pmin1, new SizeF3D(pmax1.X - pmin1.X, pmax1.Y - pmin1.Y, pmax1.Z - pmin1.Z));

                        float maxmax1 = Math.Max(md1.OrginRec.Width, Math.Max(md1.OrginRec.Height, md1.OrginRec.Thick));
                        if (maxmax1 == 0) { maxmax1 = 1; }
                        SizeF3D realscalesz_t1 = new SizeF3D(
                                (md1.OrginRec.Width / maxmax1) * sizescale,
                               (md1.OrginRec.Height / maxmax1) * sizescale,
                              (md1.OrginRec.Thick / maxmax1) * sizescale);


                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = true;
                        this.kscale_edit_width.Invoke(new Action(() => this.kscale_edit_width.ForcedSetValue(realscalesz_t1.Width)));
                        this.kscale_edit_height.Invoke(new Action(() => this.kscale_edit_height.ForcedSetValue(realscalesz_t1.Height)));
                        this.kscale_edit_thick.Invoke(new Action(() => this.kscale_edit_thick.ForcedSetValue(realscalesz_t1.Thick)));
                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = false;

                        md1.Resize(realscalesz_t1);
                        md1.ReColor(Defulttextures);



                        Modles.Models[i] = md1;
                        break;
                    case EquationForm.GenerationType.equation:

                        EquationFile eqf = Modles.Equations[i];
                        //****************
                        EquationSolver eqsolver = new EquationSolver(false);
                        if (eqf.IsExplicit)
                        {
                            float[] helpingvalues = new float[eqf.ExplicitHelpingvariblesequations.Length];
                            string[] vstring = new string[0];
                            float[] vfloat = new float[0];
                            string worng_in_help_equation_msg = null;
                            int worng_in_help_equation_indx = -1;
                            if (helpingvalues.Length == 0)
                            {
                                vstring = new string[2] { "x", "y" };
                                vfloat = new float[2] { eqf.DomainU.X, eqf.DomainV.X };

                            }
                            else
                            {
                                for (int s = 0; s < helpingvalues.Length; s++)
                                {
                                    vstring = new string[2 + s]; vstring[0] = "x"; vstring[1] = "y";
                                    vfloat = new float[2 + s]; vfloat[0] = 1; vfloat[1] = 1;

                                    for (int r = 0; r < s; r++)
                                    {
                                        vstring[r + 2] = "c" + (r + 1); vfloat[r + 2] = helpingvalues[r];
                                    }
                                    try
                                    {
                                        helpingvalues[s] = EquationSolver.SolveEQ(eqf.ExplicitHelpingvariblesequations[s], vstring, vfloat, false, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        worng_in_help_equation_msg = ex.Message;
                                        worng_in_help_equation_indx = s + 1;
                                    }
                                }
                            }
                            if (worng_in_help_equation_indx != -1)
                            {
                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Helping Varible 'c" + worng_in_help_equation_indx + "',Please Coorect it \n Details :" + worng_in_help_equation_msg);
                                continue;
                            }


                            List<float> allfloat = new List<float>(); allfloat.AddRange(new float[2] { eqf.DomainX.X, eqf.DomainY.X }); allfloat.AddRange(helpingvalues);
                            string[] allstring = eqf.GetAllSymbols();
                            try
                            {
                                EquationSolver.SolveEQ(eqf.Expliciteqaution, allstring, allfloat.ToArray(), false, true);
                            }
                            catch (MathException mx)
                            {

                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Explicit Equation,Please Coorect it \n Details :" + ex.Message);
                                continue;
                            }
                        }
                        else
                        {
                            float[] helpingvalues = new float[eqf.ParamHelpingvariblesequations.Length];
                            string[] vstring = new string[0];
                            float[] vfloat = new float[0];
                            string worng_in_help_equation_msg = null;
                            int worng_in_help_equation_indx = -1;
                            if (helpingvalues.Length == 0)
                            {
                                vstring = new string[2] { "u", "v" };
                                vfloat = new float[2] { eqf.DomainU.X, eqf.DomainV.X };

                            }
                            else
                            {
                                for (int s = 0; s < helpingvalues.Length; s++)
                                {
                                    vstring = new string[2 + s]; vstring[0] = "u"; vstring[1] = "v";
                                    vfloat = new float[2 + s]; vfloat[0] = 1; vfloat[1] = 1;

                                    for (int r = 0; r < s; r++)
                                    {
                                        vstring[r + 2] = "c" + (r + 1); vfloat[r + 2] = helpingvalues[r];
                                    }
                                    try
                                    {
                                        helpingvalues[s] = EquationSolver.SolveEQ(eqf.ParamHelpingvariblesequations[s], vstring, vfloat, false, true);
                                    }
                                    catch (MathException ex)
                                    {
                                    }
                                    catch (Exception ex)
                                    {
                                        worng_in_help_equation_msg = ex.Message;
                                        worng_in_help_equation_indx = s + 1;
                                    }
                                }
                            }
                            if (worng_in_help_equation_indx != -1)
                            {
                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Helping Varible 'c" + worng_in_help_equation_indx + "',Please Coorect it \n Details :" + worng_in_help_equation_msg);
                                continue;
                            }

                            //because of text of vars(eqf.Getallsymbols) are form max to min 
                            helpingvalues = helpingvalues.Reverse().ToArray();
                            List<float> allfloat = new List<float>(); allfloat.AddRange(new float[2] { eqf.DomainV.X, eqf.DomainU.X }); allfloat.AddRange(helpingvalues);
                            string[] allstring = eqf.GetAllSymbols();
                            try
                            {
                                EquationSolver.SolveEQ(eqf.ParamX, allstring, allfloat.ToArray(), false, true);
                            }
                            catch (MathException ex)
                            {

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Equation X,Please Coorect it \n Details :" + ex.Message);
                                continue;
                            }


                            try
                            {
                                EquationSolver.SolveEQ(eqf.ParamY, allstring, allfloat.ToArray(), false, true);
                            }
                            catch (MathException ex)
                            {

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Equation Y,Please Coorect it \n Details :" + ex.Message);
                                continue;
                            }

                            try
                            {
                                EquationSolver.SolveEQ(eqf.ParamZ, allstring, allfloat.ToArray(), false, true);
                            }
                            catch (MathException ex)
                            {

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error in Model " + (i + 1).ToString() + " in Equation Z,Please Coorect it \n Details :" + ex.Message);
                                continue;
                            }

                        }


                        //*****************************     
                        PointF3D pmin = new PointF3D(); PointF3D pmax = new PointF3D();
                        Modles.Models[i] = new Model3D();
                        Model3D mdl = Modles.Models[i];
                        mdl.OrginPoints.Clear();

                        if (eqf.IsExplicit)
                        {
                            bool xlaststepdone = false; bool ylaststepdone = false;
                            for (float x = eqf.DomainX.X; x <= eqf.DomainX.Y; x += eqf.Stepu)
                            {
                                ylaststepdone = false;
                                List<Vector3> inps = new List<Vector3>();
                                for (float y = eqf.DomainY.X; y <= eqf.DomainY.Y; y += eqf.Stepv)
                                {
                                    float[] helpingvalues = new float[eqf.ExplicitHelpingvariblesequations.Length];
                                    string[] vstring = new string[0];
                                    float[] vfloat = new float[0];
                                    string worng_in_help_equation = null;
                                    if (helpingvalues.Length == 0)
                                    {
                                        vstring = new string[2] { "x", "y" };
                                        vfloat = new float[2] { x, y };

                                    }
                                    else
                                    {
                                        for (int s = 0; s < helpingvalues.Length; s++)
                                        {
                                            vstring = new string[2 + s]; vstring[0] = "x"; vstring[1] = "y";
                                            vfloat = new float[2 + s]; vfloat[0] = x; vfloat[1] = y;

                                            for (int r = 0; r < s; r++)
                                            {
                                                vstring[r + 2] = "c" + (r + 1); vfloat[r + 2] = helpingvalues[r];
                                            }
                                            try
                                            {
                                                helpingvalues[s] = EquationSolver.SolveEQ(eqf.ExplicitHelpingvariblesequations[s], vstring, vfloat, false, true);
                                            }
                                            catch (Exception ex)
                                            {
                                                worng_in_help_equation = ex.Message;
                                            }
                                        }
                                    }
                                    if (worng_in_help_equation != null)
                                    {
                                        errors.Add(worng_in_help_equation);
                                        matherrors++;
                                        continue;
                                    }
                                    List<float> allfloat = new List<float>(); allfloat.AddRange(new float[2] { x, y }); allfloat.AddRange(helpingvalues);
                                    string[] allstring = eqf.GetAllSymbols();




                                    Vector3 f = new Vector3();
                                    f.X = x;
                                    f.Y = y;
                                    f.uv = new PointF(x, y);
                                    try
                                    {
                                        if (showcalculations && anlz_form.IsDisposed == false)
                                        {
                                            anlz_form.Invoke(new Action(() =>
                                        {

                                            anlz_form.rtb_x.Text += "\n-----------------------------------";
                                            anlz_form.rtb_x.Text += "\n" + f.X.ToString();
                                            anlz_form.rtb_x.SelectionStart = anlz_form.rtb_x.Text.Length;
                                            anlz_form.rtb_x.ScrollToCaret();

                                            anlz_form.rtb_y.Text += "\n-----------------------------------";
                                            anlz_form.rtb_y.Text += "\n" + f.Y.ToString();
                                            anlz_form.rtb_y.SelectionStart = anlz_form.rtb_y.Text.Length;
                                            anlz_form.rtb_y.ScrollToCaret();

                                        }));
                                        }
                                    }
                                    catch { }


                                    try
                                    {
                                        f.Z = EquationSolver.NumricE(eqsolver.SolveEquation(eqf.Expliciteqaution, allstring, allfloat.ToArray(), true));
                                        if (showcalculations && anlz_form.IsDisposed == false)
                                        {
                                            try
                                            {

                                                anlz_form.Invoke(new Action(() =>
                                                {

                                                    if (showcalculations && anlz_form.IsDisposed == false)
                                                    {
                                                        anlz_form.rtb_z.Text += "\n-----------------------------------";
                                                        foreach (string s in eqsolver.Steps)
                                                        {
                                                            if (showcalculations && anlz_form.IsDisposed == false)
                                                            {
                                                                anlz_form.rtb_z.Text += "\n" + s;

                                                            }
                                                        }
                                                        if (eqsolver.Steps.Count == 0)
                                                        {
                                                            anlz_form.rtb_z.Text += "\n" + f.Z.ToString();
                                                        }
                                                        anlz_form.rtb_z.SelectionStart = anlz_form.rtb_z.Text.Length;
                                                        anlz_form.rtb_z.ScrollToCaret();
                                                    }
                                                }

                                                    ));
                                            }
                                            catch { }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.Message);
                                        matherrors++;
                                        continue;
                                    }

                                    if (f.X > pmax.X) { pmax.X = f.X; }
                                    if (f.X < pmin.X) { pmin.X = f.X; }
                                    if (f.Y > pmax.Y) { pmax.Y = f.Y; }
                                    if (f.Y < pmin.Y) { pmin.Y = f.Y; }
                                    if (f.Z > pmax.Z) { pmax.Z = f.Z; }
                                    if (f.Z < pmin.Z) { pmin.Z = f.Z; }
                                    if (y + eqf.Stepv > eqf.DomainY.Y && ylaststepdone == false)
                                    {
                                        y = eqf.DomainY.Y - eqf.Stepv; ylaststepdone = true;
                                    }
                                    inps.Add(f);
                                    if (showcalculations)
                                    {
                                        anlz_form.prog2.ValueF = (y - eqf.DomainY.X) / (eqf.DomainY.Y - eqf.DomainY.X) * 100f;
                                    }
                                }
                                eqform.InvokeProgress = nowprog = (x - eqf.DomainX.X) / (eqf.DomainX.Y - eqf.DomainX.X) * 99f;
                                if (showcalculations)
                                {
                                    anlz_form.prog1.ValueF = nowprog;
                                }

                                float allprog_eqs = (nowprog + 100 * i) / (Modles.Count());
                                if (allprog_eqs == 100)
                                { allprog_eqs = 99; }

                                kg_redraw.Invoke(new Action(() => kg_redraw.FillPercent = allprog_eqs));
                                if (showcalculations)
                                {
                                    anlz_form.progall.ValueF = allprog_eqs;
                                }

                                if (inps.Count > 0)
                                {
                                    mdl.OrginPoints.Add(inps);
                                }
                                if (x + eqf.Stepu > eqf.DomainX.Y && xlaststepdone == false)
                                {
                                    x = eqf.DomainX.Y - eqf.Stepu; xlaststepdone = true;
                                }
                            }
                        }
                        else // param equation
                        {
                            bool ulaststepdone = false; bool vlaststepdone = false;
                            for (float v = eqf.DomainV.X; v <= eqf.DomainV.Y; v += eqf.Stepv)
                            {
                                ulaststepdone = false;
                                List<Vector3> inps = new List<Vector3>();

                                for (float u = eqf.DomainU.X; u <= eqf.DomainU.Y; u += eqf.Stepu)
                                {

                                    float[] helpingvalues = new float[eqf.ParamHelpingvariblesequations.Length];
                                    string[] vstring = new string[0];
                                    float[] vfloat = new float[0];
                                    string worng_in_help_equation = null;
                                    if (helpingvalues.Length == 0)
                                    {
                                        vstring = new string[2] { "u", "v" };
                                        vfloat = new float[2] { u, v };

                                    }
                                    else
                                    {
                                        for (int s = 0; s < helpingvalues.Length; s++)
                                        {
                                            vstring = new string[2 + s]; vstring[0] = "u"; vstring[1] = "v";
                                            vfloat = new float[2 + s]; vfloat[0] = u; vfloat[1] = v;

                                            for (int r = 0; r < s; r++)
                                            {
                                                vstring[r + 2] = "c" + (r + 1); vfloat[r + 2] = helpingvalues[r];
                                            }
                                            try
                                            {
                                                helpingvalues[s] = EquationSolver.SolveEQ(eqf.ParamHelpingvariblesequations[s], vstring, vfloat, false, true);
                                            }
                                            catch (Exception ex)
                                            {
                                                worng_in_help_equation = ex.Message;
                                            }
                                        }
                                    }
                                    if (worng_in_help_equation != null)
                                    {
                                        errors.Add(worng_in_help_equation);
                                        matherrors++;
                                        continue;
                                    }

                                    List<float> allfloat = new List<float>(); allfloat.AddRange(new float[2] { u, v }); allfloat.AddRange(helpingvalues);
                                    string[] allstring = eqf.GetAllSymbols();


                                    Vector3 f = new Vector3();
                                    f.uv = new PointF(u, v);
                                    try
                                    {
                                        f.X = EquationSolver.NumricE(eqsolver.SolveEquation(eqf.ParamX, allstring, allfloat.ToArray(), true));
                                        if (showcalculations && anlz_form.IsDisposed == false)
                                        {
                                            try
                                            {

                                                anlz_form.Invoke(new Action(() =>
                                                    {

                                                        if (showcalculations && anlz_form.IsDisposed == false)
                                                        {
                                                            anlz_form.rtb_x.Text += "\n-------------------------";
                                                            foreach (string s in eqsolver.Steps)
                                                            {
                                                                if (showcalculations && anlz_form.IsDisposed == false)
                                                                {
                                                                    anlz_form.rtb_x.Text += "\n" + s;

                                                                }
                                                            }
                                                            if (eqsolver.Steps.Count == 0)
                                                            {
                                                                anlz_form.rtb_x.Text += "\n" + f.X.ToString();
                                                            }
                                                            anlz_form.rtb_x.SelectionStart = anlz_form.rtb_x.Text.Length;
                                                            anlz_form.rtb_x.ScrollToCaret();
                                                        }
                                                    }

                                                    ));
                                            }
                                            catch { }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.Message);
                                        matherrors++;
                                        continue;
                                    }
                                    try
                                    {
                                        f.Y = EquationSolver.NumricE(eqsolver.SolveEquation(eqf.ParamY, allstring, allfloat.ToArray(), true));
                                        if (showcalculations && anlz_form.IsDisposed == false)
                                        {
                                            try
                                            {

                                                anlz_form.Invoke(new Action(() =>
                                                {

                                                    if (showcalculations && anlz_form.IsDisposed == false)
                                                    {
                                                        anlz_form.rtb_y.Text += "\n-------------------------";
                                                        foreach (string s in eqsolver.Steps)
                                                        {
                                                            if (showcalculations && anlz_form.IsDisposed == false)
                                                            {
                                                                anlz_form.rtb_y.Text += "\n" + s;

                                                            }
                                                        }
                                                        if (eqsolver.Steps.Count == 0)
                                                        {
                                                            anlz_form.rtb_y.Text += "\n" + f.Y.ToString();
                                                        }
                                                        anlz_form.rtb_y.SelectionStart = anlz_form.rtb_y.Text.Length;
                                                        anlz_form.rtb_y.ScrollToCaret();
                                                    }
                                                }

                                                    ));
                                            }
                                            catch { }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.Message);
                                        matherrors++;
                                        continue;
                                    }
                                    try
                                    {
                                        f.Z = EquationSolver.NumricE(eqsolver.SolveEquation(eqf.ParamZ, allstring, allfloat.ToArray(), true));
                                        if (showcalculations && anlz_form.IsDisposed == false)
                                        {
                                            try
                                            {

                                                anlz_form.Invoke(new Action(() =>
                                                {

                                                    if (showcalculations && anlz_form.IsDisposed == false)
                                                    {
                                                        anlz_form.rtb_z.Text += "\n-----------------------------------";
                                                        foreach (string s in eqsolver.Steps)
                                                        {
                                                            if (showcalculations && anlz_form.IsDisposed == false)
                                                            {
                                                                anlz_form.rtb_z.Text += "\n" + s;

                                                            }
                                                        }
                                                        if (eqsolver.Steps.Count == 0)
                                                        {
                                                            anlz_form.rtb_z.Text += "\n" + f.Z.ToString();
                                                        }
                                                        anlz_form.rtb_z.SelectionStart = anlz_form.rtb_z.Text.Length;
                                                        anlz_form.rtb_z.ScrollToCaret();
                                                    }
                                                }

                                                    ));
                                            }
                                            catch { }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.Message);
                                        matherrors++;
                                        continue;
                                    }
                                    if (f.X > pmax.X) { pmax.X = f.X; }
                                    if (f.X < pmin.X) { pmin.X = f.X; }
                                    if (f.Y > pmax.Y) { pmax.Y = f.Y; }
                                    if (f.Y < pmin.Y) { pmin.Y = f.Y; }
                                    if (f.Z > pmax.Z) { pmax.Z = f.Z; }
                                    if (f.Z < pmin.Z) { pmin.Z = f.Z; }

                                    if (u + eqf.Stepu > eqf.DomainU.Y && ulaststepdone == false)
                                    {
                                       u = eqf.DomainU.Y - eqf.Stepu;
                                        ulaststepdone = true;
                                    }
                          
                                    inps.Add(f);


                                    if (showcalculations)
                                    {
                                        anlz_form.prog2.ValueF = (u - eqf.DomainU.X) / (eqf.DomainU.Y - eqf.DomainU.X) * 100f;
                                    }


                                }
                                if (inps.Count > 0)
                                {
                                    mdl.OrginPoints.Add(inps);
                                }
                                nowprog = (v - eqf.DomainV.X) / (eqf.DomainV.Y - eqf.DomainV.X) * 99f;
                                if (float.IsNaN(nowprog))
                                { nowprog = 100; }
                                if (showcalculations)
                                {
                                    anlz_form.prog1.ValueF = nowprog;
                                }

                                eqform.InvokeProgress = nowprog;
                                float allprog_eqs = (nowprog + 100 * i) / (Modles.Count());
                                if (allprog_eqs == 100)
                                { allprog_eqs = 99; }

                                kg_redraw.Invoke(new Action(() => kg_redraw.FillPercent = allprog_eqs));
                                if (showcalculations)
                                {
                                    anlz_form.progall.ValueF = allprog_eqs;
                                }

                                if (v + eqf.Stepv > eqf.DomainV.Y && vlaststepdone == false)
                                {
                                   v = eqf.DomainV.Y - eqf.Stepv; vlaststepdone = true;
                                }

                            }

                        }
                        if (showcalculations)
                        {
                            anlz_form.Invoke(new Action(() => anlz_form.Complete()));
                        }
                        if (matherrors > 0)
                        {
                            MessageBox.Show("Found " + matherrors + " math errors that were skiped\n First error:" + errors[0], "error");
                        }

                        for (int a = 0; a < mdl.OrginPoints.Count; a++)
                        {
                            List<int[][]> list_faces_indxes = new List<int[][]>();
                            for (int b = 0; b < mdl.OrginPoints[a].Count; b++)
                            {
                                int[][] faces_indexs = new int[4][];
                                faces_indexs[0] = new int[2] { a, b };
                                if (mdl.OrginPoints.Count > a + 1)
                                {
                                    if (mdl.OrginPoints[a + 1].Count > b)
                                    {
                                        faces_indexs[1] = new int[2] { a + 1, b };
                                    }
                                    else { continue; }

                                    if (mdl.OrginPoints[a + 1].Count > b + 1)
                                    {
                                        faces_indexs[2] = new int[2] { a + 1, b + 1 };
                                    }
                                    else { continue; }

                                }
                                else { continue; }
                                if (mdl.OrginPoints[a].Count > i + 1)
                                {
                                    faces_indexs[3] = new int[2] { a, b + 1 };
                                }
                                else { continue; }
                                list_faces_indxes.Add(faces_indexs);
                            }
                            if (list_faces_indxes.Count > 0)
                            {
                                mdl.FacesIndex.Add(list_faces_indxes);
                            }
                        }

                        mdl.OrginRec = new RectangleF3D(pmin, new SizeF3D(pmax.X - pmin.X, pmax.Y - pmin.Y, pmax.Z - pmin.Z));

                        float maxmax = Math.Max(mdl.OrginRec.Width, Math.Max(mdl.OrginRec.Height, mdl.OrginRec.Thick));
                        if (maxmax == 0) { maxmax = 1; }
                        SizeF3D realscalesz = new SizeF3D(
                                (mdl.OrginRec.Width / maxmax) * sizescale,
                               (mdl.OrginRec.Height / maxmax) * sizescale,
                              (mdl.OrginRec.Thick / maxmax) * sizescale);
                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = true;
                        this.kscale_edit_width.Invoke(new Action(() => this.kscale_edit_width.ForcedSetValue(realscalesz.Width)));
                        this.kscale_edit_height.Invoke(new Action(() => this.kscale_edit_height.ForcedSetValue(realscalesz.Height)));
                        this.kscale_edit_thick.Invoke(new Action(() => this.kscale_edit_thick.ForcedSetValue(realscalesz.Thick)));
                        this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = false;
                      
                   
                        mdl.Resize(realscalesz);
                            mdl.ReColor(Defulttextures);
                        centerdrawlocation = new PointF(pictureBox1.Width / 2f, pictureBox1.Height / 2f);

                        mdl.IsVisible = eqform.ismodelvisible;
                        if (showcalculations)
                        {
                            anlz_form.prog1.ValueF = 100;
                        }
                        eqform.InvokeProgress = 100;
                        float allproges = (nowprog + 100 * i) / (Modles.Count());
                        kg_redraw.Invoke(new Action(() => kg_redraw.FillPercent = allproges));

                      
                            break;

                }
                float allprog = (100 * (i + 1)) / (Modles.Count());
                if (allprog == 100)
                { allprog = 99; }
                kg_redraw.Invoke(new Action(() => kg_redraw.FillPercent = allprog));
                if (showcalculations)
                {
                    anlz_form.progall.ValueF = allprog;
                }
            }

            this.Invoke(new Action(() =>
       {
           try
           {
               iscangraph = true;
               Regraph();
           }
           catch
           {
               iscangraph = false;
           }
       }));



            float milly = Environment.TickCount - genratestartingtime;

            float seconds = milly / 1000f;
            float mints = seconds / 60f;

            seconds = (mints - (float)Math.Floor(mints)) * 60f;
            milly = (seconds - (float)Math.Floor(seconds)) * 1000f;
            this.Invoke(new Action(() =>
            {
                this.Text = "Konk 3D Equation Render" + " , " + "Generating completed in (" + ((int)mints).ToString() + ") minutes,(" + (int)seconds + ") seconds,(" + (int)milly + ") millysecondes";
                if (iscangraph == false)
                {
                    this.Text = "Konk 3D Equation Render" + " Error in Rendering !";
                }
                this.Cursor = Cursors.Default;
                if (starting_lbl != null && is_form_loaded)
                {
                    this.Controls.Remove(starting_lbl);
                    starting_lbl = null;
                    is_firstmodel_generated = true;
                }
            }));

            kg_redraw.Invoke(new Action(() => kg_redraw.FillPercent = 0));

        }
        bool iscangraph = false;
        int genratestartingtime = 0;
        private void Generate(Point domain, bool showcalcs = false)
        {
            showcalculations = showcalcs;

            if (showcalcs)
            {
                anlz_form = new AnalzingForm();
                anlz_form.FormClosing += new FormClosingEventHandler(analzform_colsed);
                anlz_form.Show();
            }

            genratestartingtime = Environment.TickCount;
            ActionThread(ref thrd_loadeqs, thrdstart_loadeqs, null, false);
            startrotatethread(new Point(), false);
            resetselection();
            UpdateEquations();
            GetDefulttextures();
            ActionThread(ref thrd_loadeqs, thrdstart_loadeqs, domain, true);

        }
        private void button_Param_Click(object sender, EventArgs e)
        {

            Generate(new Point(0, Modles.Count()));
        }
        private void newequationform_draw_Click(object sender, EventArgs e)
        {
            Control sendercont = sender as Control;
            EquationForm neweqform = (EquationForm)sendercont.FindForm();
            int d = this.flowLayoutPanel_modles.Controls.IndexOf(neweqform);
            Generate(new Point(d, 1));

        }
        private void kgriditem1_Click(object sender, EventArgs e)
        {
            new CalculatorForm().ShowDialog();
        }





        private void kswitch3_ValueChanged(object sender, booleventargs e)
        {

        }


        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void kgrid1_Selecteditemchanged(object sender, kgrideventargs e)
        {

        }



        private void kscale11_ValueChanged(object sender, Kscaleeventargs e)
        {


        }

        private void button9_Click_1(object sender, EventArgs e)
        {


        }

        private void kscale6_ValueChanged(object sender, Kscaleeventargs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {

        }



        private void kscale7_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kscale10_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kscale9_ValueChanged(object sender, Kscaleeventargs e)
        {

        }
        private Vector3 getangle(Vector3[][] cords, Vector3 p)
        {
            Vector3 pdf = new Vector3();
            //x
            float xx_x = cords[0][0].X - cords[0][1].X;
            float yy_x = cords[0][0].Y - cords[0][1].Y;
            float angle_x = (float)(Math.Atan(Math.Abs(yy_x) / Math.Abs(xx_x))); angle_x = fnc.Topi(angle_x, false);
            angle_x = fnc.correctquart(angle_x, xx_x, yy_x);
            float xx_y = cords[1][0].X - cords[1][1].X;
            float yy_y = cords[1][0].Y - cords[1][1].Y;
            float angle_y = (float)(Math.Atan(Math.Abs(yy_y) / Math.Abs(xx_y))); angle_y = fnc.Topi(angle_y, false);
            angle_y = fnc.correctquart(angle_y, xx_y, yy_y);

            //   pdf.Z = (float)(distance[1]);

            //z
            float xxz = cords[2][0].X - cords[2][1].X;
            float yyz = cords[2][0].Y - cords[2][1].Y;
            float anglez = (float)(Math.Atan(Math.Abs(yyz) / Math.Abs(xxz))); anglez = fnc.Topi(anglez, false);
            anglez = fnc.correctquart(anglez, xxz, yyz);

            pdf.X = angle_x;
            pdf.Y = angle_y;
            pdf.Z = anglez;
            return pdf;
        }
        public static Vector3 Move3DPoint(Vector3[][] cords, float[] distance, Vector3 p)
        {
            Vector3 pdf = new Vector3();
            //x:
            float xx_x = cords[0][1].X - cords[0][0].X;
            float yy_x = cords[0][1].Y - cords[0][0].Y;
            float angle_x = (float)(Math.Atan(Math.Abs(yy_x) / Math.Abs(xx_x))); angle_x = fnc.Topi(angle_x, false);
            angle_x = fnc.correctquart(angle_x, xx_x, yy_x);
            pdf.X += (float)(distance[0] * 1 * Math.Cos(fnc.Topi((angle_x))));
            pdf.Y += (float)(distance[0] * Math.Sin(fnc.Topi((angle_x))));
            //  pdf.Z += (float)(distance[0] );

            //y:
            float xx_y = cords[1][1].X - cords[1][0].X;
            float yy_y = cords[1][1].Y - cords[1][0].Y;
            float angle_y = (float)(Math.Atan(Math.Abs(yy_y) / Math.Abs(xx_y))); angle_y = fnc.Topi(angle_y, false);
            angle_y = fnc.correctquart(angle_y, xx_y, yy_y);

            pdf.X += (float)(distance[1] * Math.Cos(fnc.Topi((angle_y))));
            pdf.Y += (float)(distance[1] * 1 * Math.Sin(fnc.Topi((angle_y))));
            //   pdf.Z +=(float)(distance[1] * Math.Sin(fnc.Topi((angle_y)))); ;

            //z:
            float xx_z = cords[2][1].X - cords[2][0].X;
            float yy_z = cords[2][1].Y - cords[2][0].Y;
            float anglez = (float)(Math.Atan(Math.Abs(yy_z) / Math.Abs(xx_z))); anglez = fnc.Topi(anglez, false);
            anglez = fnc.correctquart(anglez, xx_z, yy_z);

            pdf.X += (float)(distance[2] * Math.Cos(fnc.Topi((anglez))));
            pdf.Y += (float)(distance[2] * Math.Sin(fnc.Topi((anglez))));
            pdf.Z += (float)(distance[2]);


            pdf.X += p.X;
            pdf.Y += p.Y;
            pdf.Z += p.Z;
            return pdf;
        }
        private void kscale7_ValueChanging(object sender, Kscaleeventargs e)
        {
           
            Model3D mdl = Modles.SelectedModel;

            if (iseditmode() == false) return;
            if (mdl == null) return;
            if (isonselection)
            {
                Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(b);





                //========
                for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                {
                    int[] indxs = mdl.selectionindxs[i];
                    Vector3 unedit_targ = (Vector3)mdl.selection_unedited_pnts[i].ToV();
                    Vector3 unedit_orgn_targ = (Vector3)mdl.selection_unedited_orgn_pnts[i].ToV();

                    Vector3 moved_unedit_targ = Move3DPoint(mdl.cords, new float[3] { kscale_editx.ValueF, kscale_edity.ValueF, kscale_editz.ValueF }, unedit_targ);
                    Vector3[][] orgn_cords = new Vector3[3][];
                    orgn_cords[0] = new Vector3[2]; orgn_cords[1] = new Vector3[2]; orgn_cords[2] = new Vector3[2];
                    orgn_cords[0][0] = new Vector3(-100, 0, 0);
                    orgn_cords[0][1] = new Vector3(100, 0, 0);
                    orgn_cords[1][0] = new Vector3(0, -100, 0);
                    orgn_cords[1][1] = new Vector3(0, 100, 0);
                    orgn_cords[2][0] = new Vector3(0, 0, -100);
                    orgn_cords[2][1] = new Vector3(0, 0, 100);

                    Vector3 moved_unedit_orgn_targ = Move3DPoint(orgn_cords, new float[3] {
                        kscale_editx.ValueF / mdl.Size.Width * mdl.OrginRec.Width,
                        kscale_edity.ValueF / mdl.Size.Height * mdl.OrginRec.Height,
                        kscale_editz.ValueF / mdl.Size.Thick * mdl.OrginRec.Thick }
                    , unedit_orgn_targ);


                    Vector3 targ = mdl.EditedPoints[indxs[0]][indxs[1]];
                    targ.X = moved_unedit_targ.X;
                    targ.Y = moved_unedit_targ.Y;
                    targ.Z = moved_unedit_targ.Z;
                    Vector3 orgn_targ = mdl.OrginPoints[indxs[0]][indxs[1]];

                    orgn_targ.X = moved_unedit_orgn_targ.X;
                    orgn_targ.Y = moved_unedit_orgn_targ.Y;
                    orgn_targ.Z = moved_unedit_orgn_targ.Z;



                }

                Regraph(true);
                mdl.SubDivisionLevel = mdl.SubDivisionLevel;
            }

        }
        Vector3[][] Get3dpointcords(Vector3[][] cords, SizeF3D size, Vector3 p)
        {
            Vector3[][] value = new Vector3[3][];
            value[0] = new Vector3[2]; value[1] = new Vector3[2]; value[2] = new Vector3[2];

            value[0][0] = Move3DPoint(cords, new float[3] { -size.Width, 0, 0 }, p);
            value[0][1] = Move3DPoint(cords, new float[3] { size.Width, 0, 0 }, p);
            value[1][0] = Move3DPoint(cords, new float[3] { 0, -size.Height, 0 }, p);
            value[1][1] = Move3DPoint(cords, new float[3] { 0, size.Height, 0 }, p);
            value[2][0] = Move3DPoint(cords, new float[3] { 0, 0, -size.Thick }, p);
            value[2][1] = Move3DPoint(cords, new float[3] { 0, 0, size.Thick }, p);
            return value;

        }
        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void kscale_edit_width_ValueChanged(object sender, Kscaleeventargs e)
        {

        }



        private void kscale_edit_height_ValueChanging(object sender, Kscaleeventargs e)
        {
            if (Modles.SelectedModel != null)
            {
                Modles.SelectedModel.Resize(new SizeF3D(kscale_edit_width.ValueF, kscale_edit_height.ValueF, kscale_edit_thick.ValueF));
                Regraph();
            }
        }

        private void textBox_r_Leave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.White;

        }

        private void textBox_r_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Yellow;

        }





        private void kgriditem8_Click(object sender, EventArgs e)
        {
            kgriditem8.Cursor = Cursors.WaitCursor;
            TextureModel[] tms = new TextureModel[flowLayoutPanel_textures.Controls.Count];
            for (int i = 0; i < flowLayoutPanel_textures.Controls.Count; i++)
            {
                tms[i] = ((Form_TextureModle)flowLayoutPanel_textures.Controls[i]).TextureModleValue;
            }
            if (kscale_color_models.Value == 0)
            {
                if (Modles.SelectedModel != null)
                {
                    Modles.SelectedModel.ReColor(tms);
                }
            }
            else
            {
                for (int i = 0; i < Modles.Models.Count; i++)
                {
                    if (Modles.Models[i] != null)
                    {
                        Modles.Models[i].ReColor(tms);
                    }
                }

            }

            Regraph();
            kgriditem8.Cursor = Cursors.Default;

        }

        private void kscale_editmode_ValueChanged(object sender, Kscaleeventargs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (kscale_editmode.Value == 1)
            {
                if (mdl == null) { kscale_editmode.Value = 0; MessageBox.Show("No model had been generated yet to be edited!"); return; }
            }
            else
            {
               resetselection();
                kscale_editx.ValueF = kscale_edity.ValueF = kscale_editz.ValueF = 0;
                angle_edit_rotate_x.Value = angle_edit_rotate_y.Value = angle_edit_rotate_z.Value = 0;
            }
            //to avoid over repate to due set value to zero when model=null
            if (mdl == null) return;

            kDynamkPanel_edit_selectedpoints.IsOpened = iseditmode();

            startrotatethread(new Point(1, 1), false);
            Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(b);
            Render r = new Render();
            Regraph(true);

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Modles.SelectedIndex = 0;
        }

        private void kscale2_ValueChanging_1(object sender, Kscaleeventargs e)
        {
            Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(Get3DImgDflt(Modles.SelectedModel), 0, 0);
            pictureBox1.BackgroundImage = b;
            pictureBox1.Refresh();
        }
        void resetselection()
        {
            for (int i = 0; i < Modles.Count(); i++)
            {
                Model3D mdl = Modles.Models[i];
                if (mdl == null) continue;
                mdl.selectionindxs.Clear();
                if (mdl.selection_unedited_pnts != null)
                {
                    mdl.selection_unedited_pnts.Clear();
                }
                if (mdl.selection_unedited_orgn_pnts != null)
                {
                    mdl.selection_unedited_orgn_pnts.Clear();
                }

                selectionrec = new RectangleF();
                isonselection = false;
            }
        }
        void deleteselection()
        {
            Model3D mdl = Modles.SelectedModel;
            for (int i = 0; i < mdl.selectionindxs.Count; i++)
            {
                int[] indxs = mdl.selectionindxs[i];

                int n1 = indxs[0]; int n2 = indxs[1];
                mdl.EditedPoints[n1][n2].isdeleted = true;
                mdl.OrginPoints[n1][n2].isdeleted = true;
            }
            resetselection();



        }
        private void deletSelectedPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isonselection)
            {
                deleteselection();

                Regraph();
            }
        }

        private void kscale9_ValueChanged_1(object sender, Kscaleeventargs e)
        {


        }

        private void kscale9_ValueChanging(object sender, Kscaleeventargs e)
        {

        }

        private void colorPicker2_ValueChanging(ColorPicker sender, ColorPickereventargs e)
        {

        }

        private void colorPicker2_ValueChanged(ColorPicker sender, ColorPickereventargs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (mdl == null) return;
            Color c = Color.FromArgb((int)(255 - (kscale_transparensy.ValueF / 100f * 255f)), e.SelectedColor);

            if (isonselection)
            {


                for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                {
                    Vector3 targ = (Vector3)mdl.EditedPoints[mdl.selectionindxs[i][0]][mdl.selectionindxs[i][1]];
                    Vector3 orgntarg = (Vector3)mdl.OrginPoints[mdl.selectionindxs[i][0]][mdl.selectionindxs[i][1]];
                    orgntarg.color = c;
                    targ.color = c;
                }
                Regraph();
            }

            else { MessageBox.Show("No vectors selecred turn on edit mode and select vectors to recolor"); }
        }

        private void kPictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "all files|*.*|png|*.png|jpg|*.jpg|bmp|*.bmp";
            if (dv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ((PictureBox)sender).BackgroundImage = Image.FromFile(dv.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to read :" + ex.Message);
                }
            }
        }

        private void kPictureBox1_Click(object sender, EventArgs e)
        {

        }
        void filllayes_Click(object sender, EventArgs e)
        {

            this.flowLayoutPanel_modles.Controls.Remove(((Control)sender).Parent);
        }


        private void textbox_focs(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Yellow;
        }
        private void textbox_lostfocs(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.White;
        }

        private void kcomboboximgexmples_SelectedIndexChanged(object sender, EventArgs e)
        {
            kcombobox c = (kcombobox)sender;


            RectangleF cq = (RectangleF)((comboitem)c.SelectedItem).Tag;
            ((kPictureBox)c.Parent.Controls["SkPictureBox1"]).Selectionrec = cq;

        }



        private void button11_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(300, 200);
            // b.GetPixel(200,300);
        }


        private void kDynamkPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

            //   d.MdiParent = this;


            //    d.Show();
            // pictureBox1.BackgroundImage=Get3DImgo(fnc.GetPlot3DObject(), pictureBox1.Size);
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel_modles.Controls.Count; i++)
            {
                EquationForm eqform = (EquationForm)flowLayoutPanel_modles.Controls[i];
                eqform.Text = "Model" + " {" + (i + 1).ToString() + "}";
            }


        }

        public void AddTextureModle(TextureModel tm = null)
        {
            Form_TextureModle v = new Form_TextureModle();
            v.TopLevel = false;
            v.FormClosing += new FormClosingEventHandler(Texturemodleform_closing);
            flowLayoutPanel_textures.Controls.Add(v);
            v.Show();
            if (tm != null)
            {
                v.TextureModleValue = tm;
            }
        }
        private void Texturemodleform_closing(object sender, FormClosingEventArgs e)
        {
            if (flowLayoutPanel_textures.Controls.Count == 1)
            {
                MessageBox.Show("Must be at least one texture modle!");
                e.Cancel = true;
            }
        }
        private void equationform_closing(object sender, FormClosingEventArgs e)
        {
            if (flowLayoutPanel_modles.Controls.Count == 1)
            {
                MessageBox.Show("Must be at least one equation!");
                e.Cancel = true;
            }
            else
            {
                EquationForm neweqform = (EquationForm)sender;
                int d = this.flowLayoutPanel_modles.Controls.IndexOf(neweqform);
                Modles.SelectedIndex = 0;
                Modles.RemoveAt(d);
            }
        }
        private void kgriditem4_Click(object sender, EventArgs e)
        {
            AddTextureModle();
        }

        private void kcombobox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void kscale5_ValueChanging(object sender, Kscaleeventargs e)
        {
            colorPicker_editcolor.Centerhslcirclelight = (int)((100 - e.Value) * 2.5f);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (Getrecord())
            {
                try
                {
                    bbs.Add(pictureBox1.BackgroundImage as Bitmap);
                }
                catch (Exception ex)
                {
                    startrotatethread(null, false);
                    MessageBox.Show("error So many frames no memory :" + ex.Message);
                }
            }
        }
        private Vector3 pre_editmodel_loaction = new Vector3();
        private Vector3 crnt_editmodel_loaction = new Vector3();



        private void kscale5_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kscale3_ValueChanging(object sender, Kscaleeventargs e)
        {
            int transpercy = 255 - (int)(kscale_transparensy.Value / 100f * 255f);
            kscale_transparensy.Maincolor = Color.FromArgb(transpercy, kscale_transparensy.BackColor);

        }


        private void kscale3_ValueChanged_1(object sender, Kscaleeventargs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (mdl == null) return;
            int transpercy = 255 - (int)(kscale_transparensy.Value / 100f * 255f);

            if (isonselection)
            {

                //========
                for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                {
                    Vector3 targ = (Vector3)mdl.EditedPoints[mdl.selectionindxs[i][0]][mdl.selectionindxs[i][1]];
                    Vector3 orgntarg = (Vector3)mdl.OrginPoints[mdl.selectionindxs[i][0]][mdl.selectionindxs[i][1]];
                    orgntarg.color = Color.FromArgb(transpercy, targ.color);
                    targ.color = Color.FromArgb(transpercy, targ.color);
                }
                Regraph();
            }

            else { MessageBox.Show("No vectors selecred turn on edit mode and select vectors to recolor"); }
        }

        private void kscale2_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kgriditem6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://plus.google.com/u/0/115727285984134923577/posts");
        }
        int framescount = 0;
        int frame_start_render_time = 0;
        public void Regraph(bool highlightselectedpoints = false)
        {
            if (iscangraph == false) return;
            Render rnd = GetdefultRender();

            pictureBox1.BackgroundImage = Get3DImgs(Modles.Models.ToArray(), pictureBox1.Size, rnd);

            pictureBox1.Refresh();

            if (kpnl_cam.IsOpened)//drawing anothr view
            {
                List<Model3D> cmdls = new List<Model3D>(Modles.Count());

                for (int u = 0; u < Modles.Models.Count; u++)
                {
                    if (Modles.Models[u] != null)
                    {
                        cmdls.Add(Modles.Models[u].Clone());
                        cmdls[cmdls.Count - 1].MuliplyRotate(new PointF(angle_anotherview_xy.Value, angle_anotherview_z.Value));
                    }
                }
                if (cmdls.Count == 0)
                {
                    return;
                }

                rnd.drawinfo = false;
                pictureBox2.BackgroundImage = Get3DImgs(cmdls.ToArray(), pictureBox1.Size, rnd);
                pictureBox2.Refresh();

            }
          
        }
        private void kscale_backface_ValueChanged(object sender, Kscaleeventargs e)
        {
            Regraph();
        }

        private void colorPicker_lightxy_ValueChanged(ColorPicker sender, ColorPickereventargs e)
        {
            Regraph();
        }

        private void kscale_backfacecolor_ValueChanging(object sender, Kscaleeventargs e)
        {
            Regraph();

        }

        private void colorPicker_lightxy_ValueChanging(ColorPicker sender, ColorPickereventargs e)
        {
            Regraph();
        }



        private void kscale2_ValueChanging_2(object sender, Kscaleeventargs e)
        {
            pictureBox1.BackColor = e.SelectedColor;
        }

        private void kscale_record_ValueChanged(object sender, Kscaleeventargs e)
        {
            if (e.Value == 1)
            {
                bbs.Clear();
                milly = Environment.TickCount;
            }
            else
            {

                milly = 0;
            }
        }

        private void kgriditem5_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCkC5aj719JWTTeWRmaFUEHg");
        }

        private void kgriditem9_Click(object sender, EventArgs e)
        {


        }

        private void kscale_editx_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kgriditem10_Click(object sender, EventArgs e)
        {
            KColorDialog kcd = new KColorDialog();
            kcd.DefultColor = pictureBox1.BackColor;
            if (kcd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = kcd.ColorValue;
                Bitmap b = new Bitmap(25, 25);
                Graphics g = Graphics.FromImage(b);
                g.Clear(kcd.ColorValue);
                kgriditem10.Centerimage = b;

            }
        }

        private void kgriditem9_Click_1(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();
        }



        private void angle1_ValueChanged(object sender, Inteventaregs e)
        {


        }
        private void angle1_ValueChanging(object sender, Inteventaregs e)
        {
            Model3D mdl = Modles.SelectedModel;
            if (mdl == null) return;
            if (iseditmode() == false) return;
            PointF3D rotateangle = new PointF3D(angle_edit_rotate_x.Value, angle_edit_rotate_y.Value, angle_edit_rotate_z.Value);

            if (isonselection)
            {

                //========
                for (int i = 0; i < mdl.selection_unedited_pnts.Count; i++)
                {
                    int[] indxs = mdl.selectionindxs[i];

                    Vector3 unedit_targ = (Vector3)mdl.selection_unedited_pnts[i].ToV();
                    Vector3 unedit_orgn_targ = (Vector3)mdl.selection_unedited_orgn_pnts[i].ToV();
                    /*
                    Vector3D rotated_unedit_targ = Orginrot(unedit_targ, rotateangle);
                    Vector3D rotated_unedit_orgn_targ = Orginrot(unedit_orgn_targ, rotateangle);
                    */

                    Vector3 rotated_unedit_targ = Rotate3D.RotateVector_Clone(rotateangle, unedit_targ);
                    Vector3 rotated_unedit_orgn_targ = Rotate3D.RotateVector_Clone(rotateangle, unedit_orgn_targ);



                    Vector3 targ = mdl.EditedPoints[indxs[0]][indxs[1]];
                    targ.X = rotated_unedit_targ.X;
                    targ.Y = rotated_unedit_targ.Y;
                    targ.Z = rotated_unedit_targ.Z;
                    Vector3 orgn_targ = mdl.OrginPoints[indxs[0]][indxs[1]];

                    orgn_targ.X = rotated_unedit_orgn_targ.X;
                    orgn_targ.Y = rotated_unedit_orgn_targ.Y;
                    orgn_targ.Z = rotated_unedit_orgn_targ.Z;


                }
                Regraph();
            }


        }

        private void kscale_edit_z_ValueChanging(object sender, Kscaleeventargs e)
        {
        }

        private void kscale_relocate_z_ValueChanging(object sender, Kscaleeventargs e)
        {

        }
        List<List<Vector3>> locrt = new List<List<Vector3>>();
        PointF3D Preloc = new PointF3D();


        private void kscale_relocate_x_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kscale_relocate_x_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kscale_relocate_z_ValueChanged(object sender, Kscaleeventargs e)
        {

        }

        private void kDynamkPanel_edit_openingclosing(object sender, EventArgs e)
        {
            kDynamkPanel4.Height = flowLayoutPanel_custiomize_and_texture.Height - kDynamkPanel_edit.Height - 5;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //   GetRegionDataExample(e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void kDynamkPanel2_openingclosing(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(kDynamkPanel2.Right + 5, pictureBox1.Location.Y);
            pictureBox1.Width = flowLayoutPanel_custiomize_and_texture.Left - kDynamkPanel2.Right - 10;

            kDynamkPane_graphic_option.Location = new Point(kDynamkPanel2.Right + 5, kDynamkPane_graphic_option.Location.Y);
            kDynamkPane_graphic_option.Width = flowLayoutPanel_custiomize_and_texture.Left - kDynamkPanel2.Right - 10;
        }

        private void kgriditem11_Click(object sender, EventArgs e)
        {
            kgriditem11.ShowTasks();




        }

        private void kgriditem11_TextChanged(object sender, EventArgs e)
        {

        }

        private void kgriditem11_SelectedTaskChanged(object sender, kgrideventargs e)
        {
            if (Modles.SelectedModel == null) { MessageBox.Show("You havn't generated any model yet to save!"); return; }
            int indx = kgriditem11.SelectedTaskIndx;
            if (indx == 0)//photo
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "png|*.png|jpg|*.jpg|bmp|*.bmp";
                RectangleF rec = Modles.SelectedModel.Rec.To2D();
                rec.Offset(centerdrawlocation);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap b = (Bitmap)pictureBox1.BackgroundImage.Clone();
                        b.Save(sfd.FileName);
                        MessageBox.Show("Succesfully saved");
                    }
                    catch (Exception ex)
                    { MessageBox.Show("An Error had happend during saving :" + ex.Message); }
                }

            }
            else if (indx == 1)// croped photo
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "png|*.png|jpg|*.jpg|bmp|*.bmp";
                RectangleF rec = Modles.SelectedModel.Rec.To2D();
                rec.Offset(centerdrawlocation);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap b = ((Bitmap)pictureBox1.BackgroundImage).Clone(rec, pictureBox1.BackgroundImage.PixelFormat);
                        b.Save(sfd.FileName);
                        MessageBox.Show("Succesfully saved");
                    }
                    catch (Exception ex)
                    { MessageBox.Show("An Error had happend during saving :" + ex.Message); }
                }

            }

            else if (indx == 2)//gif
            {
                if (bbs.Count > 1)
                {
                    startrotatethread(null, false);
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "gif|*.gif";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            fnc.GifConvert.ConvertToGIF(bbs.ToArray(), sfd.FileName, pictureBox1.Size, Color.Transparent, 0);
                            MessageBox.Show("Succesfully saved");
                        }
                        catch
                        {
                            MessageBox.Show("An Error had happend during saving");
                        }
                    }
                }
                else { MessageBox.Show("NO Animation has Done yet!"); }
            }
            else if (indx == 3)//equation
            {
                SaveFileDialog dv = new SaveFileDialog(); dv.Filter = "Vector Equation(veq)|*.veq";
                if (dv.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (Modles.SelectedIndex != -1)
                        {
                            EquationForm eqform = flowLayoutPanel_modles.Controls[Modles.SelectedIndex] as EquationForm;
                            eqform.updateequationfile();
                            EquationFile eqf = Modles.Equations[Modles.SelectedIndex];
                            eqf.AutoDetectStep = false;
                            fnc.Selraliz.WriteToBinaryFile(dv.FileName, eqf);
                            MessageBox.Show("Succesfully saved");
                        }
                        else
                        {
                            MessageBox.Show("No equation had been generated yet");
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Failed to Save file :" + x.Message);
                    }
                }
            }


            else if (indx == 4)//models
            {
                SaveFileDialog dv = new SaveFileDialog(); dv.Filter = "Models(mdls)|*.mdls";
                if (dv.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Models3Dlist mdls = new Models3Dlist();
                        for (int i = 0; i < Modles.Models.Count; i++)
                        {
                            mdls.Models.Add(Modles.Models[i]);
                        }

                        mdls.Save(dv.FileName);
                        MessageBox.Show("Succesfully saved");
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Failed to Save file :" + x.Message);
                    }
                }
            }
        }

        private void kgriditem4_SelectedTaskChanged(object sender, kgrideventargs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int indx = kgriditem4.SelectedTaskIndx;
            if (indx == 1)
            {
                TextureModel tm1 = new TextureModel();
                TextureModel tm2 = new TextureModel();
                TextureModel tm3 = new TextureModel();
                TextureModel tm61 = new TextureModel();
                TextureModel tm62 = new TextureModel();
                TextureModel tm4 = new TextureModel();
                TextureModel tm5 = new TextureModel();

                tm1.IsImageFill = tm2.IsImageFill = tm3.IsImageFill = tm4.IsImageFill = tm5.IsImageFill = tm61.IsImageFill = tm62.IsImageFill = true;

                tm1.GridDomain = new Point(13, 38); tm1.PlanDomain = new Point(25, 75);
                tm2.GridDomain = new Point(38, 63); tm2.PlanDomain = new Point(25, 75);
                tm3.GridDomain = new Point(63, 88); tm3.PlanDomain = new Point(25, 75);

                tm61.GridDomain = new Point(0, 13); tm61.PlanDomain = new Point(25, 75); tm61.ImageSelectionRec = new RectangleF(0, 0, 50, 100);
                tm62.GridDomain = new Point(88, 100); tm62.PlanDomain = new Point(25, 75); tm62.ImageSelectionRec = new RectangleF(50, 0, 50, 100);


                tm4.GridDomain = new Point(0, 100); tm4.PlanDomain = new Point(0, 25);
                tm5.GridDomain = new Point(0, 100); tm5.PlanDomain = new Point(75, 100);
                flowLayoutPanel_textures.Controls.Clear();

                AddTextureModle(tm1);
                AddTextureModle(tm2);
                AddTextureModle(tm3);
                AddTextureModle(tm61);
                AddTextureModle(tm62);
                AddTextureModle(tm4);
                AddTextureModle(tm5);
            }
            else if (indx == 0)
            {
                TextureModel tm1 = new TextureModel();
                TextureModel tm2 = new TextureModel();
                TextureModel tm3 = new TextureModel();
                TextureModel tm4 = new TextureModel();

                tm1.GridDomain = new Point(0, 50); tm1.PlanDomain = new Point(0, 50);
                tm2.GridDomain = new Point(0, 50); tm2.PlanDomain = new Point(50, 100);
                tm3.GridDomain = new Point(50, 100); tm3.PlanDomain = new Point(0, 50);
                tm4.GridDomain = new Point(50, 100); tm4.PlanDomain = new Point(50, 100);

                tm1.colorequation = new ColorEquation("100", "0", "0");
                tm2.colorequation = new ColorEquation("0", "100", "0");
                tm3.colorequation = new ColorEquation("0", "0", "100");
                tm4.colorequation = new ColorEquation("100", "50", "0");

                flowLayoutPanel_textures.Controls.Clear();

                AddTextureModle(tm1);
                AddTextureModle(tm2);
                AddTextureModle(tm3);
                AddTextureModle(tm4);

            }
            this.Cursor = Cursors.Default;
        }



        private void kscale_relocate_x_ValueChanged_1(object sender, Kscaleeventargs e)
        {

        }

        private void colorPicker1_ValueChanging(ColorPicker sender, ColorPickereventargs e)
        {
            if (Modles.SelectedModel != null)
            {
                Modles.SelectedModel.Locate(new PointF3D(colorPicker_locate.Value.X, colorPicker_locate.Value.Y, kscale_relocate_z.Value));
                Regraph();
            }
        }

        private void kscale_relocate_x_ValueChanging_1(object sender, Kscaleeventargs e)
        {

        }

        private void kscale_relocate_z_ValueChanging_1(object sender, Kscaleeventargs e)
        {
            Modles.SelectedModel.Locate(new PointF3D(colorPicker_locate.Value.X, colorPicker_locate.Value.Y, kscale_relocate_z.Value));
            Regraph();
        }

        private void kscale_relocate_z_ValueChanged_1(object sender, Kscaleeventargs e)
        {

        }

        private void kgriditem7_Click_1(object sender, EventArgs e)
        {
            deleteselection();
            Regraph();
        }
        bool showcalculations = false;
        AnalzingForm anlz_form = new AnalzingForm();
        private void kg_redraw_SelectedTaskChanged(object sender, kgrideventargs e)
        {
            Generate(new Point(0, Modles.Count()), true);

        }
        PointF3D angles_prevalue = new PointF3D();
        PointF3D angles_crntvalue = new PointF3D();
        private void anglexy_ValueChanging(object sender, Inteventaregs e)
        {
            if (rotate3dthread != null)
            { if (rotate3dthread.IsAlive) { return; } }

            lp = cp; cp = new Point(this.anglex.Value, this.anglez.Value);


            // PointF rotatep = new PointF(cp.X - lp.X, cp.Y - lp.Y);
            angles_prevalue = angles_crntvalue;
            angles_crntvalue = new PointF3D(anglex.Value, angley.Value, anglez.Value);

            PointF3D rotatep = angles_crntvalue - angles_prevalue;
            if (GetRotateAll())
            {
                for (int i = 0; i < Modles.Count(); i++)
                {
                    if (Modles.Models[i] != null)
                    {
                        Modles.Models[i].MuliplyRotate(rotatep);
                    }
                }
            }
            else if (Modles.SelectedModel != null)
            {
                Modles.SelectedModel.MuliplyRotate(rotatep);
            }
            Regraph();
        }

        private void kgriditem12_Click(object sender, EventArgs e)
        {

        }

        private void kgriditem13_Click(object sender, EventArgs e)
        {
            kgriditem13.ShowTasks();
        }

        private void kgriditem13_SelectedTaskChanged(object sender, kgrideventargs e)
        {
            int indx = kgriditem13.SelectedTaskIndx;
            this.Cursor = Cursors.WaitCursor; 
            if (indx == 0)// open equation file
            {
                OpenFileDialog dv = new OpenFileDialog();
                dv.Filter = "Vector Equation(veq)|*.veq";
                dv.Multiselect = true;
                if (dv.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < dv.FileNames.Length; i++)
                    {
                        try
                        {
                            EquationFile eq = fnc.Selraliz.ReadFromBinaryFile<EquationFile>(dv.FileNames[i]);
                            AddEquation(eq);
                        }
                        catch (Exception ex)
                        {
                            this.Cursor = Cursors.Default; MessageBox.Show("Failed to read :" + ex.Message);
                        }
                        MessageBox.Show("Succesfuly added! ,click on regenerate all to graph");
                      
                    }
                }
            }
            else if (indx == 1)//open models file
            {
                OpenFileDialog dv = new OpenFileDialog();
                dv.Filter = "Modles File(mdls)|*.mdls";
                dv.Multiselect = false;
                if (dv.ShowDialog() == DialogResult.OK)
                {
                 
                    try
                    {
                        AddModels(dv.FileName);
                        MessageBox.Show("Succesfuly added! ,click on regenerate all to graph");
                    }
                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default; MessageBox.Show("Failed to read :" + ex.Message);
                    }

                }
            }
            this.Cursor = Cursors.Default;
        }

        private void kgriditem12_Click_1(object sender, EventArgs e)
        {
            

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            kDynamkPanel4.Height = flowLayoutPanel_custiomize_and_texture.Height - kDynamkPanel_edit.Height - 5;

        }

        private void kgriditem12_Click_2(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/profile/view?id=349019198&trk=nav_responsive_tab_profile");
        }

        private void kgriditem14_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://3dkonk.wix.com/3d-equation-render");
        }

        private void kscale_resizeall_ValueChanging(object sender, Kscaleeventargs e)
        {
            if (Modles.SelectedModel != null)
            {
                Model3D mdl = Modles.SelectedModel;
                float maxmax = Math.Max(mdl.OrginRec.Width, Math.Max(mdl.OrginRec.Height, mdl.OrginRec.Thick));
                if (maxmax == 0) { maxmax = 1; }
                SizeF3D newsize = new SizeF3D(e.Value, e.Value, e.Value);
                SizeF3D realscalesz = new SizeF3D(
                        (mdl.OrginRec.Width / maxmax) * newsize.Width,
                       (mdl.OrginRec.Height / maxmax) * newsize.Height,
                      (mdl.OrginRec.Thick / maxmax) * newsize.Thick);

                this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = true;
                this.kscale_edit_width.ForcedSetValue(realscalesz.Width);
                this.kscale_edit_height.ForcedSetValue(realscalesz.Height);
                this.kscale_edit_thick.ForcedSetValue(realscalesz.Thick);
                mdl.Resize(realscalesz);
                this.kscale_edit_width.Hangvaluechangedevent = this.kscale_edit_height.Hangvaluechangedevent = this.kscale_edit_thick.Hangvaluechangedevent = false;


                Regraph();
            }
        }

        private void angle_anotherview_xy_ValueChanging(object sender, Inteventaregs e)
        {
            Regraph();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            centerdrawlocation = new PointF(pictureBox1.Width / 2f, pictureBox1.Height / 2f);

        }

        private void kDynamkPanel_edit_selectedpoints_openingclosing(object sender, EventArgs e)
        {

            this.panel1.Location = new Point(panel1.Location.X, kDynamkPanel_edit_selectedpoints.Location.Y + kDynamkPanel_edit_selectedpoints.Height + 4);
            kDynamkPanel_edit.Height = panel1.Location.Y + panel1.Height + 5;

            kDynamkPanel4.Height = flowLayoutPanel_custiomize_and_texture.Height - kDynamkPanel_edit.Height - 5;



        }

        private void kDynamkPanel_edit_selectedpoints_Click(object sender, EventArgs e)
        {
            if (kDynamkPanel_edit_selectedpoints.IsOpened == false)
            {
                MessageBox.Show("Enable edit mode switch to open control panel");
            }
        }



        private void kgriditem35_SelectedTaskChanged(object sender, kgrideventargs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.flowLayoutPanel_modles.Enabled = false;
            int index = kgriditem35.SelectedTaskIndx;
            if (index >= 0)
            {
                AddModelsformresources(index);
            }
            this.flowLayoutPanel_modles.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void kscale_resizeall_MouseDown(object sender, MouseEventArgs e)
        {
            ActionThread(ref rotate3dthread, null, null, false, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "Modles File(mdls)|*.mdls";
            dv.Multiselect = false;
            if (dv.ShowDialog() == DialogResult.OK)
            {


               try
                {
                    AddModels(dv.FileName);
                    MessageBox.Show("Succesfuly added!");
                }
                catch (Exception ex)
              {
                  MessageBox.Show("Failed to read :" + ex.Message);
                }
            }


        }

        private void kscale1_ValueChanged(object sender, Kscaleeventargs e)
        {
           this.Modles.SelectedModel.SubDivisionLevel = e.Value;
            Regraph();
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(b);
            Vector3[] vecs = new Vector3[]{new Vector3(50, 0, 0),new Vector3(150,0,0),new Vector3(150,150,0),new Vector3(50,150,0),new Vector3(50,0,0)};
            Vector3[] crvs = fnc.GetBezirPoints(vecs);
            for (int i = 0; i < crvs.Length-1; i++)
            {
               
               
                    g.DrawLine(new Pen(Color.Red, 2), crvs[i].TOPointF(), crvs[i + 1].TOPointF());            
            }
            g.TranslateTransform(0, 200);
            PointF[] pps=new PointF[5];
         for(int i=0;i<5;i++)
            {
              pps[i].X=vecs[i].X;
             pps[i].Y=vecs[i].Y;
            }
         g.DrawCurve(new Pen(Color.Blue, 2), pps);
         g.TranslateTransform(200,0);
       
    //     g.DrawBeziers(new Pen(Color.Green, 2), pps);
            pictureBox1.BackgroundImage = b;
            pictureBox1.Refresh();
        }
    }

    //----------------------------

    [Serializable()]
    public class TextureModel
    {
        public TextureModel()
        {

        }
        public ColorEquation colorequation = new ColorEquation();
        public Bitmap Image;
        public RectangleF ImageSelectionRec = new RectangleF(0, 0, 100, 100);
        public bool IsImageFill = false;
        public Point PlanDomain = new Point(0, 100);
        public Point GridDomain = new Point(0, 100);

        public Color backfacecolor = Color.Blue;

        public Color frontfacecolor = Color.Red;

    }

    public struct Plan
    {
        public int indx ;
        public Vector3[] vectors;
      
        public Plan(Vector3[] vecs, int idex)
        {
            vectors = vecs;
            indx = idex;
        }

    }
    public enum Axes { X, Y, Z, XY };
    public struct Face
    {
        public static Face Empty()
        {
            Face f = new Face();
            f.TopLeft = f.TopRight = f.BottomLeft = f.BottomRight = f.Center = f.TopMid = f.LeftMid = f.RightMid = f.BottomMid = new Vector3();
            f.isempty = true;
            return f;
        }
        public Face Value { get { return this; } }
        public Vector3 TopLeft;
       public Vector3 TopRight;
       public Vector3 BottomLeft;
       public Vector3 BottomRight;

       public Vector3 Center;

       public Vector3 TopMid;
        public Vector3 LeftMid;
        public Vector3 RightMid;
        public Vector3 BottomMid;
        public bool isempty;
        public int[] Index;
        public void UpDate()
        {
            this.Center =( TopLeft + TopRight + BottomLeft + BottomRight )/ 4f;
            this.TopMid = (TopRight + TopLeft) / 2f;
            this.LeftMid = (TopLeft + BottomLeft) / 2f;
            this.RightMid = (BottomRight + TopRight) / 2f;
            this.BottomMid = (BottomRight + BottomLeft) / 2f;
        }
    }
    [Serializable()]
    public class Model3D
    {

      
        public static Model3D FromFile(string address)
        {
            return fnc.Selraliz.ReadFromBinaryFile<Model3D>(address);
        }
        public bool Save(string address)
        {
            try
            {
                fnc.Selraliz.WriteToBinaryFile(address, this);
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public List<int[]> selectionindxs = new List<int[]>();
        public List<PointF3D> selection_unedited_pnts = new List<PointF3D>();
        public List<PointF3D> selection_unedited_orgn_pnts = new List<PointF3D>();
        private int facescount = 0;
        public int FacesCount
        {
            get { return facescount; }
        }
        public bool IsVisible = true;
        public SizeF3D Size { get { return Rec.Size; } set { Rec.Size = value; } }
        public PointF3D Location { get { return Rec.Location; } set { Rec.Location = value; } }
        public RectangleF3D Rec = new RectangleF3D();
        private SizeF3D size_with_out_rotation=new SizeF3D();
        public SizeF3D Size_With_Out_Rotation { get {return size_with_out_rotation; } }
       public PointF3D rotateangleww = new PointF3D();
       
        public PointF3D TranslatedLoc = new PointF3D();

        public Vector3[][] cords = new Vector3[3][];
        private float cordslength = 0;
        public float CordsLenght { get { return cordslength; } }

        public List<List<int[][]>> FacesIndex = new List<List<int[][]>>();
        public Face[][] FinalFaces =new Face[0][];
   
        public List<List<Vector3>> FinalPoints = new List<List<Vector3>>();
        public List<List<int[][]>> FinalFacesIndex = new List<List<int[][]>>();  
        public List<List<Vector3>> EditedPoints = new List<List<Vector3>>();
        public List<List<Vector3>> OrginPoints = new List<List<Vector3>>();
        public List<List<Vector3>> planx = new List<List<Vector3>>();
        public List<List<Vector3>> plany = new List<List<Vector3>>();
        public List<List<Vector3>> planz = new List<List<Vector3>>();
        public RectangleF3D OrginRec;
       
        private int subdivisionlevel = 0;

        public int SubDivisionLevel
        {
            get { return subdivisionlevel; }
            set {
               // 
              
                subdivisionlevel = value;
                Face[][] oldfaces = GetMainPlans();
                facescount = 0;
                for (int i = 0; i < oldfaces.Length;i++)                   
                {
                    for (int n=0;n<oldfaces[i].Length;n++)
                    {
                        facescount += 1;
                    }
                }
                 
                Face[][] subfaces = oldfaces;

                for (int i = 0; i < value; i++)
                {
                    subfaces = GetSubFaces(subfaces);
                }
                this.FinalFaces =subfaces;
               
            }
        }
        public bool isselected = false;

        private Vector3[] BoxVects = new Vector3[8];
        public Vector3[][] Boxedges
        {
            get
            {
                Vector3[][] v = new Vector3[12][];
                //  [0, 1], [1, 3], [3, 2], [2, 0], [4, 5], [5, 7], [7, 6], [6, 4],[0, 4], [1, 5], [2, 6], [3, 7]];
                v[0] = new Vector3[2] { BoxVects[0], BoxVects[1] };
                v[1] = new Vector3[2] { BoxVects[1], BoxVects[3] };
                v[2] = new Vector3[2] { BoxVects[3], BoxVects[2] };
                v[3] = new Vector3[2] { BoxVects[2], BoxVects[0] };
                v[4] = new Vector3[2] { BoxVects[4], BoxVects[5] };
                v[5] = new Vector3[2] { BoxVects[5], BoxVects[7] };
                v[6] = new Vector3[2] { BoxVects[7], BoxVects[6] };
                v[7] = new Vector3[2] { BoxVects[6], BoxVects[4] };
                v[8] = new Vector3[2] { BoxVects[0], BoxVects[4] };
                v[9] = new Vector3[2] { BoxVects[1], BoxVects[5] };
                v[10] = new Vector3[2] { BoxVects[2], BoxVects[6] };
                v[11] = new Vector3[2] { BoxVects[3], BoxVects[7] };
                return v;
            }
        }
        public Plan[] Plans;
        public Model3D()
        {
            cords[0] = new Vector3[2];
            cords[1] = new Vector3[2];
            cords[2] = new Vector3[2];
            int l = 230;
            this.SizeCords(new RectangleF3D(-l, -l, -l, l, l, l));

        }
        public Model3D Clone()
        {
            Model3D m = new Model3D();
            m = (Model3D)this.MemberwiseClone();
            List<List<Vector3>> pnts = new List<List<Vector3>>();
            for (int i = 0; i < this.EditedPoints.Count; i++)
            {
                pnts.Add(new List<Vector3>());
                for (int n = 0; n < this.EditedPoints[i].Count; n++)
                {
                    pnts[i].Add(new Vector3());
                    pnts[i][n] = this.EditedPoints[i][n].Clone();
                }
            }
            m.EditedPoints = pnts;
            Vector3[][] cloned_cords = new Vector3[cords.Length][];
            for (int i=0;i<cords.Length;i++)
            {
                cloned_cords[i] = new Vector3[2];
                cloned_cords[i][0] = cords[i][0].Clone();
                cloned_cords[i][1] = cords[i][1].Clone();
            }
            m.cords = cloned_cords;
            Vector3[] colned_boxvects = new Vector3[BoxVects.Length];
            for (int i=0;i<BoxVects.Length;i++)
            {
                colned_boxvects[i] = BoxVects[i].Clone();
            }
            m.BoxVects = colned_boxvects;

            List<List<Vector3>> clonde_planx = new List<List<Vector3>>();
            for (int i = 0; i < this.planx.Count; i++)
            {
                clonde_planx.Add(new List<Vector3>());
                for (int n = 0; n < this.planx[i].Count; n++)
                {
                    clonde_planx[i].Add(new Vector3());
                    clonde_planx[i][n] = this.planx[i][n].Clone();
                }
            }
            m.planx = clonde_planx;

            List<List<Vector3>> clonde_plany = new List<List<Vector3>>();
            for (int i = 0; i < this.plany.Count; i++)
            {
                clonde_plany.Add(new List<Vector3>());
                for (int n = 0; n < this.plany[i].Count; n++)
                {
                    clonde_plany[i].Add(new Vector3());
                    clonde_plany[i][n] = this.plany[i][n].Clone();
                }
            }
            m.plany = clonde_plany;

            List<List<Vector3>> clonde_planz = new List<List<Vector3>>();
            for (int i = 0; i < this.planz.Count; i++)
            {
                clonde_planz.Add(new List<Vector3>());
                for (int n = 0; n < this.planz[i].Count; n++)
                {
                    clonde_planz[i].Add(new Vector3());
                    clonde_planz[i][n] = this.planz[i][n].Clone();
                }
            }
            m.planz = clonde_planz;
            m.rotates = new List<PointF3D>(this.rotates);
            return m;
        }
        public void SizeCords(RectangleF3D rec)
        {
            this.cords[0][0] = new Vector3(rec.X, 0, 0); this.cords[0][1] = new Vector3(rec.Width, 0, 0);
            this.cords[1][0] = new Vector3(0, rec.Y, 0); this.cords[1][1] = new Vector3(0, rec.Height, 0);
            this.cords[2][0] = new Vector3(0, 0, rec.Z); this.cords[2][1] = new Vector3(0, 0, rec.Thick);
            this.cordslength = (float)(Math.Max(rec.Width - rec.X, Math.Max(rec.Thick - rec.Z, rec.Height - rec.Y)));
        }
        public void SizeBox(RectangleF3D rec)
        {
            this.BoxVects[0] = new Vector3(rec.X, rec.Y, rec.Z);
            this.BoxVects[1] = new Vector3(rec.X, rec.Y, rec.Z + rec.Thick);
            this.BoxVects[2] = new Vector3(rec.X, rec.Y + rec.Height, rec.Z);
            this.BoxVects[3] = new Vector3(rec.X, rec.Y + rec.Height, rec.Z + rec.Thick);
            this.BoxVects[4] = new Vector3(rec.X + rec.Width, rec.Y, rec.Z);
            this.BoxVects[5] = new Vector3(rec.X + rec.Width, rec.Y, rec.Z + rec.Thick);
            this.BoxVects[6] = new Vector3(rec.X + rec.Width, rec.Y + rec.Height, rec.Z);
            this.BoxVects[7] = new Vector3(rec.X + rec.Width, rec.Y + rec.Height, rec.Z + rec.Thick);

        }
        public void GenerateXYZPlans()
        {
            planz.Clear(); plany.Clear(); planx.Clear();
            bool xlaststepdone = false; bool ylaststepdone = false; bool zlaststepdone = false;


            float xstep = this.Rec.Width / 5f;
            float ystep = this.Rec.Height / 5f;
            float zstep = this.Rec.Thick / 5f;
            if (xstep == 0) xstep = 1; if (ystep == 0) ystep = 1; if (zstep == 0) zstep = 1;

            float xdmn = this.Rec.X + this.Rec.Width; float ydmn = this.Rec.Y + this.Rec.Height; float zdmn = this.Rec.Z + this.Rec.Thick;
            for (float x = this.Rec.X; x <= xdmn; x += xstep)
            {
                List<Vector3> ps = new List<Vector3>();
                ylaststepdone = false;
                for (float y = this.Rec.Y; y <= ydmn; y += ystep)
                {

                    Vector3 vec=new Vector3(x, y, this.Rec.Z - 5f);
                    vec.ismodelwall = true;vec.color = Color.White;
                    ps.Add(vec);
                    if (y + ystep > ydmn && ylaststepdone == false)
                    {
                        y = ydmn - ystep;
                        ylaststepdone = true;
                    }
                }

                if (ps.Count > 0)
                {
                    planz.Add(ps);
                }
                if (x + xstep > xdmn && xlaststepdone == false)
                {
                    x = xdmn - xstep;
                    xlaststepdone = true;
                }
            }
            xlaststepdone = false; ylaststepdone = false; zlaststepdone = false;
            for (float z = this.Rec.Z; z <= zdmn; z += zstep)
            {
                xlaststepdone = false;
                List<Vector3> ps = new List<Vector3>();
                for (float x = this.Rec.X; x <= xdmn; x += xstep)
                {
                    Vector3 vec = new Vector3(x, this.Rec.Y - 5, z);

                    vec.ismodelwall = true; vec.color = Color.White;
                    ps.Add(vec );
                    if (x + xstep > xdmn && xlaststepdone == false)
                    {
                        x = xdmn - xstep;
                        xlaststepdone = true;
                    }
                }
                if (ps.Count > 0)
                {
                    plany.Add(ps);
                }
                if (z + zstep > zdmn && zlaststepdone == false)
                {
                    z = zdmn - zstep;
                    zlaststepdone = true;
                }
            }
            xlaststepdone = false; ylaststepdone = false; zlaststepdone = false;
            for (float y = this.Rec.Y; y <= ydmn; y += ystep)
            {
                zlaststepdone = false;
                List<Vector3> ps = new List<Vector3>();
                for (float z = this.Rec.Z; z <= zdmn; z += zstep)
                {
                    Vector3 vec = new Vector3(this.Rec.X - 5, y, z);
                    vec.ismodelwall = true; vec.color = Color.White;
                    ps.Add(vec);
                    if (z + zstep > zdmn && zlaststepdone == false)
                    {
                        z = zdmn - zstep;
                        zlaststepdone = true;
                    }
                }
                if (ps.Count > 0)
                {
                    planx.Add(ps);
                }
                if (y + ystep > ydmn && ylaststepdone == false)
                {
                    y = ydmn - ystep;
                    ylaststepdone = true;
                }
            }
        }
        private Vertex[][] GetTringlesOfPlan(Face f)
        {
            return GetTringlesOfPlan(f.TopLeft, f.TopRight, f.BottomRight, f.BottomLeft);
        }
        private Vertex[][] GetTringlesOfPlan(Vector3[] vecs)
        { return GetTringlesOfPlan(vecs[0], vecs[1], vecs[2], vecs[3]); }
        private Vertex[][] GetTringlesOfPlan(Vector3 vec1,Vector3 vec2,Vector3 vec3,Vector3 vec4)
        {
            Vertex[] trng1 = new Vertex[3];
            Vertex[] trng2 = new Vertex[3];           
            trng1[0].Coordinates =vec1 ;                      
            trng1[1].Coordinates = trng2[1].Coordinates = vec2;
            trng2[0].Coordinates = vec3;
            trng1[2].Coordinates =trng2[2].Coordinates =vec4;
            Vertex[][] result = new Vertex[2][];
           result[0]=(trng1);
           result[1] = (trng2);



           trng1[0].Normal = Vector3.CrossProduct(vec1 - vec2, vec4 - vec1);
           trng1[1].Normal = Vector3.CrossProduct(vec1 - vec2, vec4 - vec2);
           trng1[2].Normal = Vector3.CrossProduct(vec2 - vec4, vec1 - vec4);


           trng2[0].Normal = Vector3.CrossProduct(vec3 - vec4, vec2 - vec3);// *new Vector3();
           trng2[1].Normal = Vector3.CrossProduct(vec4 - vec2, vec3 - vec1);// *new Vector3();       
           trng2[2].Normal = Vector3.CrossProduct(vec3 - vec4, vec2 - vec4);// *new Vector3();
            
           return result;

        }
   
        public Face[][] GetMainPlans()
        {
            return this.GetMainPlans(new PointF(0, 1), new PointF(0, 1));
        }
        public Face[][] GetMainPlans(PointF grid, PointF plan)
        { return this.GetMainPlans(grid, plan,this.FacesIndex,this.EditedPoints); }
            
        public Face[][] GetMainPlans(PointF grid, PointF plan, List<List<int[][]>> indexs,List<List<Vector3>> vectorslist)
        {
            List<Face[]> area = new List<Face[]>();           
            for (int n = (int)(grid.X * indexs.Count); n < indexs.Count / 1f * grid.Y; n ++)
            {
                List<Face> listfaces = new List<Face>();
                for (int i = (int)(plan.X * indexs[n].Count); i < indexs[n].Count / 1f * plan.Y; i ++)
                {
                    
                        int[][] faces_index = indexs[n][i];
                        Face f = new Face();
                        f.TopLeft = vectorslist[faces_index[0][0]][faces_index[0][1]].Clone();
                        f.TopRight = vectorslist[faces_index[1][0]][faces_index[1][1]].Clone();
                        f.BottomRight = vectorslist[faces_index[2][0]][faces_index[2][1]].Clone();
                        f.BottomLeft = vectorslist[faces_index[3][0]][faces_index[3][1]].Clone();
                       f.UpDate();
                        listfaces.Add(f);
                    }
                   
                
                if (listfaces.Count > 0)
                {
                    area.Add(listfaces.ToArray());
                }
            }
           return area.ToArray();
        }

        public Face[][] GetSubFaces(Face[][] oldfaces)
        {
            
            
            List<List<int[][]>> indexs = new List<List<int[][]>>();
            for (int i = 0; i < FacesIndex.Count; i++)
            {
                List<int[][]>left_indexs = new List<int[][]>();
                List<int[][]> right_indexs = new List<int[][]>();
                for (int n = 0; n < FacesIndex[i].Count; n++)
                {
                    int[][] orgn = FacesIndex[i][n];
                    int[][] topleft_face = new int[4][];
                    topleft_face[0] = new int[2] { orgn[0][0] * 2, orgn[0][1] * 2 };
                    topleft_face[1] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 };
                    topleft_face[2] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 1 };
                    topleft_face[3] = new int[2] { orgn[0][0] * 2, orgn[0][1] * 2 + 1 };

                    int[][] topright_face = new int[4][];
                    topright_face[0] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 };
                    topright_face[1] = new int[2] { orgn[0][0] * 2 + 2, orgn[0][1] * 2 };
                    topright_face[2] = new int[2] { orgn[0][0] * 2 + 2, orgn[0][1] * 2 + 1 };
                    topright_face[3] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 1 };

                    int[][] bottomleft_face = new int[4][];
                    bottomleft_face[0] = new int[2] { orgn[0][0] * 2, orgn[0][1] * 2 + 1 };
                    bottomleft_face[1] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 1 };
                    bottomleft_face[2] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 2 };
                    bottomleft_face[3] = new int[2] { orgn[0][0] * 2, orgn[0][1] * 2 + 2 };

                    int[][] bottomright_face = new int[4][];
                    bottomright_face[0] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 1 };
                    bottomright_face[1] = new int[2] { orgn[0][0] * 2 + 2, orgn[0][1] * 2 + 1 };
                    bottomright_face[2] = new int[2] { orgn[0][0] * 2 + 2, orgn[0][1] * 2 + 2 };
                    bottomright_face[3] = new int[2] { orgn[0][0] * 2 + 1, orgn[0][1] * 2 + 2 };

                    left_indexs.Add(topleft_face);
                    left_indexs.Add(bottomleft_face);
                    right_indexs.Add(topright_face);
                    right_indexs.Add(bottomright_face);
                }
                indexs.Add(left_indexs); indexs.Add(right_indexs);
            }
            this.FinalFacesIndex = indexs;
            this.FinalPoints.Clear();
             List<Face[]> subfaces = new List<Face[]>(oldfaces.Length * 4);
             for (int i = 0; i < oldfaces.Length; i++)
             {
                 List<Face> curent_lefttsubfaces = new List<Face>(oldfaces[i].Length * 4);
                 List<Face> curent_rightsubfaces = new List<Face>(oldfaces[i].Length * 4);

                 List<Vector3> left_list_vectors = new List<Vector3>();
                 List<Vector3> mid_list_vectors = new List<Vector3>();
                 List<Vector3> right_list_vectors = new List<Vector3>();

                 for (int n = 0; n < oldfaces[i].Length; n++)
                 {
                   Face face_this = oldfaces[i][n];
                  
                   Face ?face_midleft = null;
                   Face ?face_topleft = null;
                   Face ?face_bottomleft = null;

                   Face ?face_topmid = null;
                   Face ?face_bottommid = null;

                   Face ?face_topright = null;
                   Face ?face_midright = null;
                   Face ?face_bottomright = null;

                     int istopleft =1;
                     int ismidleft = 1;
                     int isbottomleft = 1;

                     int istopright = 1;
                     int ismidright = 1;
                     int isbottomright = 1;

                     int istopmid = 1;
                     int isbottommid = 1;
                  
if (i==0)
{
    if (oldfaces.Length > i + 1)
    {
        face_midright = oldfaces[i + 1][n];
    }
    int left_list_index = oldfaces.Length - 1;
    Face next_midleft = oldfaces[left_list_index][n];
    if (next_midleft.TopRight.IsSameLocation(face_this.TopLeft))
    { face_midleft = next_midleft;}
           
        
   
        //-----------------------------------------------
    if (n == 0)//(i==0)&&(n==0)
    {
       if (oldfaces[i].Length > n+1)
        {
            face_bottommid = oldfaces[i][n + 1];
        }
       if (oldfaces.Length > i + 1)
       {
           if (oldfaces[i + 1].Length > n + 1)
           {
               face_bottomright = oldfaces[i + 1][n + 1];
           }
       }
       if (oldfaces[left_list_index].Length > n+1)
        {
            Face next_bottomleft = oldfaces[left_list_index][n + 1];

            if (next_bottomleft.TopRight.IsSameLocation(face_this.BottomLeft))
            { face_bottomleft = next_bottomleft; }
        }

        int next_topindex = oldfaces[i].Length - 1;
        Face next_topleft = oldfaces[left_list_index][next_topindex];
        if (next_topleft.BottomRight.IsSameLocation(face_this.TopLeft))
        {face_topleft = next_topleft; }

        Face next_topmid = oldfaces[i][next_topindex];
        if (next_topmid.BottomLeft.IsSameLocation(face_this.TopLeft) || (next_topmid.BottomRight.IsSameLocation(face_this.TopRight)))
        {face_topmid = next_topmid; }
        
        if (oldfaces.Length > i + 1)
        {
            Face next_topright = oldfaces[i + 1][next_topindex];
            if (next_topright.BottomLeft.IsSameLocation(face_this.TopRight))
            { face_topright = next_topright; }
        }
    }
    else if (n == oldfaces[i].Length-1)//(n==oldfaces[i].Lenght) && (i==0)
    {
      
            if (oldfaces[i].Length > n - 1)
            {
                face_topmid = oldfaces[i][n - 1];
            }
        
        if (oldfaces.Length > i + 1)
        {
            if (oldfaces[i + 1].Length > n - 1)
            {
                face_topright = oldfaces[i + 1][n - 1];
            }
        }
        Face next_topleft = oldfaces[left_list_index][n - 1];
        if (next_topleft.BottomRight.IsSameLocation(face_this.TopLeft))
        {
            face_topleft = next_topleft;
        }

        Face next_bottomleft = oldfaces[left_list_index][0];
        if (next_bottomleft.TopRight.IsSameLocation(face_this.BottomLeft))
        { face_bottomleft = next_bottomleft; }

        Face next_bottommid = oldfaces[i][0];
        if (next_bottommid.TopRight.IsSameLocation(face_this.BottomRight))
     
        { face_bottommid = next_bottommid; }
        if (oldfaces.Length > i + 1)
        {
            Face next_bottomright = oldfaces[i + 1][0];
            if (next_bottomright.TopLeft.IsSameLocation(face_this.BottomRight))
            { face_bottomright = next_bottomright; }
        }
    }
    else //(n>0&&n<oldfaces[i].Lenght-1) && (i==0)
    {
        face_topmid = oldfaces[i][n - 1];
        face_bottommid = oldfaces[i][n + 1];
        if (oldfaces.Length > i + 1)
        {
            face_topright = oldfaces[i + 1][n - 1];
            face_bottomright = oldfaces[i + 1][n + 1];
        }
        Face next_topleft = oldfaces[left_list_index][n - 1];
        if (next_topleft.BottomRight.IsSameLocation(face_this.TopLeft))
        {
          face_topleft = next_topleft;
        }   
        Face next_bottomleft = oldfaces[left_list_index][n + 1];
        if (next_bottomleft.TopRight.IsSameLocation(face_this.BottomLeft))
        {
          face_bottomleft = next_bottomleft;
        }   
        //-------------------------------------------------
    }
}
    //---------------------------------------
else if (i==oldfaces.Length-1)
{
    int right_list_index = 0; 
    if (oldfaces[i-1].Length > n)
    {
        face_midleft = oldfaces[i - 1][n];
    }
    if (oldfaces[right_list_index].Length > n )
    {
        Face next_midright = oldfaces[right_list_index][n];
        if (next_midright.BottomLeft.IsSameLocation(face_this.BottomRight))
        { face_midright = next_midright; }
    }
    if (n == 0)//(n==0) && (i==oldfaces.Lenght-1)
    {
        if (oldfaces[i - 1].Length > n+1)
        {
            face_bottomleft = oldfaces[i - 1][n + 1];
        }
       if (oldfaces[i].Length > n+1)
        {
            face_bottommid = oldfaces[i][n + 1];
        }
        if (oldfaces[right_list_index].Length > n+1)
        {
            Face next_bottomright = oldfaces[right_list_index][n + 1];
            if (next_bottomright.TopLeft.IsSameLocation(face_this.BottomRight))
            { face_bottomright = next_bottomright; }
        }
        int next_topindex = oldfaces[i].Length - 1;
        if (oldfaces[right_list_index].Length > next_topindex)
        {
            Face next_topright = oldfaces[right_list_index][next_topindex];
            if (next_topright.BottomLeft.IsSameLocation(face_this.TopRight))
            { face_topright = next_topright; }
        }
        if (oldfaces[i-1].Length > next_topindex)
        {
            Face next_topleft = oldfaces[i - 1][next_topindex];
            if (next_topleft.BottomRight.IsSameLocation(face_this.TopLeft))
            { face_topleft = next_topleft; }
        }
        if (oldfaces[i].Length > next_topindex)
        {
            Face next_topmid = oldfaces[i][next_topindex];
            if (next_topmid.BottomLeft.IsSameLocation(face_this.TopLeft) || (next_topmid.BottomRight.IsSameLocation(face_this.TopRight)))
            { face_topmid = next_topmid; }
        }
    }
    else if (n == oldfaces[i].Length-1)//(n==oldfaces[i].Lenght-1) && (i>==oldfaces.Lenght-1)
    {
        if (oldfaces[i-1].Length > n-1)
        {
            face_topleft = oldfaces[i - 1][n - 1];
        } 
        if (oldfaces[i].Length > n-1)
        {
            face_topmid = oldfaces[i][n - 1];
        }
        if (oldfaces[right_list_index].Length > n-1)
        {
            Face next_topright = oldfaces[right_list_index][n - 1];
            if (next_topright.BottomLeft.IsSameLocation(face_this.TopRight))
            { face_topright = next_topright; }
        }
       
            Face next_bottomright = oldfaces[right_list_index][0];
            if (next_bottomright.TopLeft.IsSameLocation(face_this.BottomRight))
            { face_bottomright = next_bottomright; }
        

        Face next_bottommid = oldfaces[i][0];
        if (next_bottommid.TopLeft.IsSameLocation(face_this.BottomLeft)||(next_bottommid.TopRight.IsSameLocation(face_this.BottomRight)))
        { face_bottommid = next_bottommid; }

        Face next_bootomleft = oldfaces[i-1][0];
        if (next_bootomleft.TopRight.IsSameLocation(face_this.BottomLeft))
        { face_bottomleft = next_bootomleft; }
 
    }
    else //(n>0&&n<oldfaces[i].Lenght) && (i==oldfaces.Lenght-1)
    {
        if (oldfaces[i-1].Length > n-1)
        {
            face_topleft = oldfaces[i - 1][n - 1];
        }
        if (oldfaces[i-1].Length > n+1)
    {
        face_bottomleft = oldfaces[i - 1][n + 1];
      }
        if (oldfaces[i].Length > n-1)
        {
            face_topmid = oldfaces[i][n - 1];
        }
        if (oldfaces[i].Length > n+1)
        {
            face_bottommid = oldfaces[i][n + 1];
        }
        if (oldfaces[right_list_index].Length > n-1)
        {
            Face next_topright = oldfaces[right_list_index][n - 1];
            if (next_topright.BottomLeft.IsSameLocation(face_this.TopRight))
            { face_topright = next_topright; }
        }
        if (oldfaces[right_list_index].Length > n+1)
        {
            Face next_bottomright = oldfaces[right_list_index][n + 1];
            if (next_bottomright.TopLeft.IsSameLocation(face_this.BottomRight))
            { face_bottomright = next_bottomright; }
        }
    }
}
    //-----------------------------------------
else//i>0&&i<oldfaces.Lenght
{
    if (oldfaces[i - 1].Length > n)
    {
        face_midleft = oldfaces[i - 1][n];
    }
    if (oldfaces[i + 1].Length > n)
    {
        face_midright = oldfaces[i + 1][n];
    }
  if (n == 0)//(n==0) && (i>0&&i<oldfaces.Lenght)
    { 
        int next_topindex = oldfaces[i].Length - 1;

       if (oldfaces[i-1].Length > n+1)
        {
            face_bottomleft = oldfaces[i - 1][n + 1]; 
         }
        if (oldfaces[i - 1].Length > next_topindex)
        {
            Face next_topleft = oldfaces[i - 1][next_topindex];
            if (next_topleft.BottomRight.IsSameLocation(face_this.TopLeft))
            { face_topleft = next_topleft; }
        }
       
       if (oldfaces[i].Length > n+1)
        {
            face_bottommid = oldfaces[i][n + 1]; 
        }

        if (oldfaces[i].Length > next_topindex)
        {
            Face next_topmid = oldfaces[i][next_topindex];
            if (next_topmid.BottomLeft.IsSameLocation(face_this.TopLeft) || (next_topmid.BottomRight.IsSameLocation(face_this.TopRight)))
            { face_topmid = next_topmid; }
        }
        
      if (oldfaces[i+1].Length > n+1)
         
        {
            face_bottomright = oldfaces[i + 1][n + 1];
      }
      if (oldfaces[i+1].Length > next_topindex)
      {
          Face next_topright = oldfaces[i + 1][next_topindex];
          if (next_topright.BottomLeft.IsSameLocation(face_this.TopRight))
          { face_topright = next_topright; }
      }
       
      
     
        
    }
    else if (n == oldfaces[i].Length - 1)//(n==oldfaces.Lenght-1) && (i>0&&i<oldfaces.Lenght)
    {
        if (oldfaces[i -1].Length > n-1)
        {
            face_topleft = oldfaces[i - 1][n - 1];
        }
        if (oldfaces[i ].Length > n-1)
        {
            face_topmid = oldfaces[i][n - 1];
        } 
        if (oldfaces[i + 1].Length > n-1)
        {
            face_topright = oldfaces[i + 1][n - 1];
        }
        Face next_bottomleft = oldfaces[i - 1][0];
        if (next_bottomleft.TopRight.IsSameLocation(face_this.BottomLeft))
        { face_bottomleft = next_bottomleft; }

        Face next_bottommid = oldfaces[i][0];
        if (next_bottommid.TopRight.IsSameLocation(face_this.BottomRight) || (next_bottommid.TopLeft.IsSameLocation(face_this.BottomLeft)))
        { face_bottommid = next_bottommid; }


        Face next_bottomright = oldfaces[i + 1][0];
        if (next_bottomright.TopLeft.IsSameLocation(face_this.BottomRight))
        { face_bottomright = next_bottomright; }
    }
    else //(n>0&&n<oldfaces[i].Lenght) && (i>0&&i<oldfaces.Lenght)
    {
       if (oldfaces[i - 1].Length > n-1)
        {
            face_topleft = oldfaces[i - 1][n - 1];
        }
        if (oldfaces[i - 1].Length > n+1)
        {
            face_bottomleft = oldfaces[i - 1][n + 1];
        }
        face_topmid = oldfaces[i][n - 1];
        face_bottommid = oldfaces[i][n + 1];

        if (oldfaces[i + 1].Length > n-1)
        {          
            face_topright = oldfaces[i + 1][n - 1];
        }  
        if (oldfaces[i +1].Length > n + 1)
            {
                face_bottomright = oldfaces[i + 1][n + 1];
            }
        
    }
}

if (face_topleft== null) { istopleft = 0; face_topleft = Face.Empty(); }
if (face_midleft == null) { ismidleft = 0; face_midleft = Face.Empty(); }
if (face_bottomleft == null) { isbottomleft = 0; face_bottomleft = Face.Empty(); }
if (face_topmid == null) { istopmid = 0; face_topmid = Face.Empty(); }
if (face_bottommid == null) { isbottommid = 0; face_bottommid = Face.Empty(); }
if (face_topright == null ) { istopright = 0; face_topright = Face.Empty(); }
if (face_midright == null) { ismidright = 0; face_midright = Face.Empty(); }
if (face_bottomright == null) { isbottomright = 0; face_bottomright = Face.Empty(); }

                   
                     Vector3 topleft_edge_avg = (face_this.TopMid + face_this.LeftMid + face_topmid.Value.LeftMid + face_midleft.Value.TopMid) / (1 * 2 + ismidleft +istopmid);
                     Vector3 topleft_center_avg = (face_this.Center + face_topleft.Value.Center + face_topmid.Value.Center + face_midleft.Value.Center) / (1 + istopleft + istopmid + ismidleft);
                     Vector3 newtopleft = (topleft_center_avg / 4f + topleft_edge_avg / 2f + face_this.TopLeft / 4f);
                  
                  
                     Vector3 topright_edge_avg = (face_this.TopMid + face_this.RightMid +face_topmid.Value.RightMid + face_midright.Value.TopMid) / (1 *2+ istopmid+ismidright);
                     Vector3 topright_center_avg = (face_this.Center + face_topright.Value.Center + face_topmid.Value.Center + face_midright.Value.Center) / (1 + istopright + istopmid + ismidright);
                     Vector3 newtopright = topright_center_avg / 4f + topright_edge_avg / 2f + face_this.TopRight / 4f;
                   

                     Vector3 bottomleft_edge_avg = (face_this.LeftMid + face_this.BottomMid + face_bottommid.Value.LeftMid + face_midleft .Value.BottomMid) / (1 *2 + isbottommid +ismidleft);
                     Vector3 bottomleft_center_avg = (face_this.Center + face_bottomleft.Value.Center + face_bottommid.Value.Center + face_midleft.Value.Center) / (1 + isbottomleft + isbottommid + ismidleft);
                     Vector3 newbottomleft = bottomleft_center_avg / 4f + bottomleft_edge_avg / 2f + face_this.BottomLeft / 4f;
                   

                     Vector3 bottomright_edge_avg = (face_this.RightMid + face_this.BottomMid + face_bottommid.Value.RightMid + face_midright.Value.BottomMid) / (1 *2 + isbottommid+ismidright); 
                     Vector3 bottomright_center_avg = (face_this.Center + face_bottomright.Value.Center + face_bottommid.Value.Center + face_midright.Value.Center) / (1 + isbottomright + isbottommid + ismidright); ;
                     Vector3 newbottomright = bottomright_center_avg / 4f + bottomright_edge_avg / 2f + face_this.BottomRight / 4f;
                    
                     Face newface = new Face();
                     newface.TopLeft = newtopleft;
                     newface.TopRight = newtopright;
                     newface.BottomLeft = newbottomleft;
                     newface.BottomRight = newbottomright;
                     
                     newface.UpDate();

                    
                     Color4 c1 = (Color4)face_this.TopLeft.color;
                     Color4 c2 = (Color4)face_this.TopRight.color;
                     Color4 c3 = (Color4)face_this.BottomRight.color;
                     Color4 c4 = (Color4)face_this.BottomLeft.color;

                     Color4 center_color=(c1+c2+c3+c4)/4f;

                     Color4 topmid_color = (c1 + c2) / 2f;
                     Color4 leftmid_color = (c1 + c4) / 2f;
                     Color4 rightmid_color = ( c2 + c3 ) / 2f;
                     Color4 bottommid_color = ( c3 + c4) / 2f;

                     newface.TopLeft.color = face_this.TopLeft.color;
                     newface.BottomLeft.color = face_this.BottomLeft.color;
                     newface.TopRight.color = face_this.TopRight.color;
                     newface.BottomRight.color = face_this.BottomRight.color;

                     newface.TopMid.color = (Color)topmid_color;
                     newface.LeftMid.color = (Color)leftmid_color;
                     newface.RightMid.color = (Color)rightmid_color;
                     newface.BottomMid.color = (Color)bottommid_color;

                     newface.Center.color = (Color)center_color;


                     Face f1 = new Face();
                     Face f2 = new Face();
                     Face f3 = new Face();
                     Face f4 = new Face();

                     f1.TopLeft = newface.TopLeft.Clone();
                     f1.TopRight = newface.TopMid.Clone();
                     f1.BottomRight = newface.Center.Clone();
                     f1.BottomLeft = newface.LeftMid.Clone();

                     f2.TopLeft = newface.TopMid.Clone();
                     f2.TopRight = newface.TopRight.Clone();
                     f2.BottomRight = newface.RightMid.Clone();
                     f2.BottomLeft = newface.Center.Clone();
                
                     f3.TopLeft = newface.Center.Clone();
                     f3.TopRight = newface.RightMid.Clone();
                     f3.BottomRight = newface.BottomRight.Clone();
                     f3.BottomLeft = newface.BottomMid.Clone();

                     f4.TopLeft = newface.LeftMid.Clone();
                     f4.TopRight = newface.Center.Clone();
                     f4.BottomRight = newface.BottomMid.Clone();
                     f4.BottomLeft = newface.BottomLeft.Clone();
                     
                     f1.UpDate();
                     f2.UpDate();
                     f3.UpDate();
                     f4.UpDate();
                    
                     
                      curent_lefttsubfaces.Add(f1);
                     curent_rightsubfaces.Add(f2);
                     curent_rightsubfaces.Add(f3);
                     curent_lefttsubfaces.Add(f4);
                     
                 }
                
             subfaces.Add(curent_lefttsubfaces.ToArray());
                 subfaces.Add(curent_rightsubfaces.ToArray());
             }
             return subfaces.ToArray();
     } 
      
         public Face[] GetFaces(PointF grid,PointF plan)
         {
            
             List<Face> result = new List<Face>(FinalFaces.Length * FinalFaces.Length);
             for (int n = (int)(grid.X * FinalFaces.Length); n < FinalFaces.Length / 1f * grid.Y; n++)
            {
                List<Face> listfaces = new List<Face>();
                for (int i = (int)(plan.X * FinalFaces[n].Length); i < FinalFaces[n].Length / 1f * plan.Y; i++)
                {
                    result.Add(FinalFaces[n][i]);                 
                }
            }    
         return result.ToArray();
         }
         public Face[] GetFaces()
         {
             return GetFaces(new PointF(0,1), new PointF(0,1));
         }

        public void Resize(SizeF3D newsize)
        {


            Vector3 incrs = new Vector3(
              this.OrginRec.Width / 2f - newsize.Width / 2f,
              this.OrginRec.Height / 2f - newsize.Height / 2f,
              this.OrginRec.Thick / 2f - newsize.Thick / 2f);
            this.EditedPoints.Clear();

            for (int i = 0; i < this.OrginPoints.Count; i++)
            {
                this.EditedPoints.Add(new List<Vector3>());

                this.EditedPoints[i] = new List<Vector3>(KGrphs.resize3dpoints(this.OrginPoints[i].ToArray(), newsize, this.OrginRec));
                this.EditedPoints[i] = new List<Vector3>(KGrphs.multiplaypoints(this.EditedPoints[i].ToArray(), incrs.TOPointF3SS()));
            
            }


            Vector3 minp = new Vector3(this.OrginRec.X + incrs.X, this.OrginRec.Y + incrs.Y, this.OrginRec.Z + incrs.Z);
            this.Rec = new RectangleF3D(minp, newsize);
            this.size_with_out_rotation = newsize;

            int l = 230;
            this.SizeCords(new RectangleF3D(-l, -l, -l, l, l, l));

            this.SizeBox(this.Rec);

            this.GenerateXYZPlans();

            PointF3D tempcenterloc = this.TranslatedLoc;
            this.TranslatedLoc = new PointF3D();
            this.Locate(tempcenterloc);
            List<PointF3D> temprots = new List<PointF3D>(rotates);
            rotates.Clear();
            foreach (PointF3D p in temprots)
            {
                this.MuliplyRotate(p);
            }
            this.rotateangleww = new PointF3D();
            this.SubDivisionLevel = subdivisionlevel;





        }
        public void Locate(PointF3D newlocation)
        {


            PointF3D smin = new PointF3D();
            PointF3D smax = new PointF3D();




            PointF3D value = newlocation - this.TranslatedLoc;
            for (int i = 0; i < this.EditedPoints.Count; i++)
            {
                for (int n = 0; n < this.EditedPoints[i].Count; n++)
                {

                    Vector3 vec = this.EditedPoints[i][n];
                    PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();

                    vec.X = rv.X;
                    vec.Y = rv.Y;
                    vec.Z = rv.Z;

                    if (i == 0 && n == 0)
                    {
                        smin = rv;
                        smax = rv;
                    }

                    if (rv.X < smin.X)
                    { smin.X = rv.X; }
                    if (rv.Y < smin.Y)
                    { smin.Y = rv.Y; }
                    if (rv.Z < smin.Z)
                    { smin.Z = rv.Z; }

                    //=======================

                    if (rv.X > smax.X)
                    { smax.X = rv.X; }
                    if (rv.Y > smax.Y)
                    { smax.Y = rv.Y; }
                    if (rv.Z > smax.Z)
                    { smax.Z = rv.Z; }
                }
            }

            SizeF3D sz = new SizeF3D();
            sz.Width = smax.X - smin.X;
            sz.Height = smax.Y - smin.Y;
            sz.Thick = smax.Z - smin.Z;
            this.Rec = new RectangleF3D(smin, sz);
            this.TranslatedLoc = newlocation;

           
          

            for (int n = 0; n < this.cords.Length; n++)
            {

                Vector3 vec = this.cords[n][0];
                PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();
                vec.X = rv.X;
                vec.Y = rv.Y;
                vec.Z = rv.Z;

                Vector3 vec2 = this.cords[n][1];
                PointF3D rv2 = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec2).TOPointF3SS();
                vec2.X = rv2.X;
                vec2.Y = rv2.Y;
                vec2.Z = rv2.Z;

            }

            for (int n = 0; n < this.BoxVects.Length; n++)
            {

                Vector3 vec = this.BoxVects[n];
                PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();
                vec.X = rv.X;
                vec.Y = rv.Y;
                vec.Z = rv.Z;
            }

            for (int i = 0; i < this.planx.Count; i++)
            {
                for (int n = 0; n < this.planx[i].Count; n++)
                {

                    Vector3 vec = this.planx[i][n];
                    PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();

                    vec.X = rv.X;
                    vec.Y = rv.Y;
                    vec.Z = rv.Z;
                }
            }

            for (int i = 0; i < this.plany.Count; i++)
            {
                for (int n = 0; n < this.plany[i].Count; n++)
                {

                    Vector3 vec = this.plany[i][n];
                    PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();

                    vec.X = rv.X;
                    vec.Y = rv.Y;
                    vec.Z = rv.Z;
                }
            }
            for (int i = 0; i < this.planz.Count; i++)
            {
                for (int n = 0; n < this.planz[i].Count; n++)
                {

                    Vector3 vec = this.planz[i][n];
                    PointF3D rv = MainForm.Move3DPoint(this.cords, new float[3] { value.X, value.Y, value.Z }, vec).TOPointF3SS();

                    vec.X = rv.X;
                    vec.Y = rv.Y;
                    vec.Z = rv.Z;
                }
            }

         //   this.SubDivisionLevel = subdivisionlevel;
        }
        public void ReColor(TextureModel[] tmd, bool clear = true)
        {
            //make it black 
            if (clear)
            {
                this.ReColor(new TextureModel());
            }
            for (int i = 0; i < tmd.Length; i++)
            {
                this.ReColor(tmd[i]);
            }
        }
        public void ReColor(TextureModel tmd)
        {

            PointF gridrng = new PointF(tmd.GridDomain.X / 100f, tmd.GridDomain.Y / 100f);
            PointF planrng = new PointF(tmd.PlanDomain.X / 100f, tmd.PlanDomain.Y / 100f);
            PointF vdomn = new PointF((this.EditedPoints.Count-1) * gridrng.X, (this.EditedPoints.Count-1) * gridrng.Y);

            if (tmd.IsImageFill)
            {
                if (tmd.Image == null) return;
                Size imgsz = System.Drawing.Size.Round(new SizeF(this.OrginPoints.Count * (gridrng.Y - gridrng.X), this.OrginPoints[0].Count * (planrng.Y - planrng.X)));
                Bitmap thebit = new Bitmap(imgsz.Width, imgsz.Height);
                SizeF texture_img_size = tmd.Image.Size;
                Graphics g = Graphics.FromImage(thebit);
                RectangleF slectedrec = tmd.ImageSelectionRec;
                Bitmap todraw = tmd.Image;
                slectedrec.X = todraw.Width / 100f * slectedrec.X;
                slectedrec.Y = todraw.Height / 100f * slectedrec.Y;
                slectedrec.Width = todraw.Width / 100f * slectedrec.Width;
                slectedrec.Height = todraw.Height / 100f * slectedrec.Height;
                slectedrec = Rectangle.Ceiling(slectedrec);
                if (slectedrec.Height+slectedrec.Y >= texture_img_size.Height)
                {
                    slectedrec.Height = texture_img_size.Height-slectedrec.Y - 1;
                }
                if (slectedrec.Width+slectedrec.X >= texture_img_size.Width)
                { slectedrec.Width = texture_img_size.Width-slectedrec.X - 1; }
                   g.DrawImage((Bitmap)todraw.Clone(slectedrec, todraw.PixelFormat), 0, 0, imgsz.Width, imgsz.Height);

                LockBitmap lc = new LockBitmap(thebit);
                lc.LockBits();
                bool vlaststep = false;
                for (int v = (int)(vdomn.X); v <= (int)(vdomn.Y); v++)
                {
                    PointF udomn = new PointF((this.EditedPoints[v].Count - 1) * planrng.X, (this.EditedPoints[v].Count - 1) * planrng.Y);
                    bool ulaststep = false;

                    for (int u = (int)(udomn.X); u <= (int)(udomn.Y); u++)
                    {

                        Vector3 pf = this.EditedPoints[v][u];
                        Vector3 pfmn = this.OrginPoints[v][u];

                        pf.color = Color.White;



                        Point getlc = Point.Round(new PointF(v - (this.EditedPoints.Count * gridrng.X), u - (this.EditedPoints[v].Count * planrng.X)));
                        if (getlc.X < 0) getlc.X = 0;
                        if (getlc.X >= lc.Width) getlc.X = lc.Width - 1;
                        if (getlc.Y < 0) getlc.Y = 0;
                        if (getlc.Y >= lc.Height) getlc.Y = lc.Height - 1;

                        pf.color = pfmn.color = lc.GetPixel(getlc.X, getlc.Y);
                        if (u + 1f > udomn.Y &&ulaststep==false)
                        {
                            u = (int)(udomn.Y - 1f);
                            ulaststep = true;
                        }
                    }
                    if (v + 1f > vdomn.Y &&vlaststep==false)
                    {
                        v = (int)(vdomn.Y - 1f);
                        vlaststep = true;
                    }
                }
                lc.UnlockBits();
            }
            else
            {

                int matherrors = 0;
                List<string> errors = new List<string>();

                bool vlaststep = false;
                for (int v = (int)(vdomn.X); v <= (int)(vdomn.Y); v++)
                {
                    PointF udomn = new PointF((this.EditedPoints[v].Count-1) * planrng.X, (this.EditedPoints[v].Count-1) * planrng.Y);
                    string[] vars = new string[10] { "x%", "y%", "z%", "x", "y", "z", "u%", "v%", "u", "v" };
                    bool ulaststep = false;
                    for (int u = (int)(udomn.X); u <= (int)(udomn.Y); u++)
                    {

                        Vector3 pf = this.EditedPoints[v][u];
                        Vector3 pfmn = this.OrginPoints[v][u];

                        float xcnt = (100f / this.OrginRec.Width) * (pfmn.X - this.OrginRec.X);
                        float ycnt = (100f / this.OrginRec.Height) * (pfmn.Y - this.OrginRec.Y);
                        float zcnt = (100f / this.OrginRec.Thick) * (pfmn.Z - this.OrginRec.Z);
                        if (OrginRec.Width == 0) { xcnt = 0; }
                        if (OrginRec.Height == 0) { ycnt = 0; }
                        if (OrginRec.Thick == 0) { zcnt = 0; }
                        float ucnt = (100f / udomn.Y) * (u - udomn.X);
                        if (udomn.Y == 0) { ucnt = 0; };
                        float vcnt = (100f / vdomn.Y) * (v - vdomn.X);
                        if (vdomn.Y == 0) { vcnt = 0; }
                        float[] vals = new float[10] { xcnt, ycnt, zcnt, pf.X, pf.Y, pf.Z, ucnt,vcnt , pf.uv.X, pf.uv.Y };
                        Color fc = Color.White;
                        int fcr = 255; int fcg = 255; int fcb = 255; int fca = 255;
                        try
                        {
                            fcr = (int)(EquationSolver.SolveEQ(tmd.colorequation.Red, vars, vals, false) * 2.55f);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                            matherrors++;
                        }
                        try
                        {
                            fcg = (int)(EquationSolver.SolveEQ(tmd.colorequation.Green, vars, vals, false) * 2.55f);

                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                            matherrors++;

                        }
                        try
                        {

                            fcb = (int)(EquationSolver.SolveEQ(tmd.colorequation.Blue, vars, vals, false) * 2.55f);

                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                            matherrors++;

                        }
                        try
                        {
                            fca = (int)(EquationSolver.SolveEQ(tmd.colorequation.Alpha, vars, vals, false) * 2.55f);

                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.Message);
                            matherrors++;
                        }
                        if (u + 1f > udomn.Y && ulaststep==false )
                        {
                            u = (int)(udomn.Y - 1f);
                            ulaststep = true;
                        }
                        if (fcr < 0) { fcr = 0; } else if (fcr > 255) { fcr = 255; }
                        if (fcg < 0) { fcg = 0; } else if (fcg > 255) { fcg = 255; }
                        if (fcb < 0) { fcb = 0; } else if (fcb > 255) { fcb = 255; }
                        if (fca < 0) { fca = 0; } else if (fca > 255) { fca = 255; }

                        pf.color = pfmn.color = Color.FromArgb(fca, fcr, fcg, fcb);


                    }
                    if (v + 1f > vdomn.Y && vlaststep==false)
                    {
                        v = (int)(vdomn.Y - 1f);
                        vlaststep = true;
                    }
                }
                if (matherrors > 0)
                {
                    MessageBox.Show("Error in Coloring ,it had been found" + matherrors + " math errors that were set to white \n First error:" + errors[0], "error");
                }
            }
      
            this.SubDivisionLevel = subdivisionlevel;
        }
        List<PointF3D> rotates = new List<PointF3D>();
        public void MuliplyRotate(PointF rotp)
        {
            for (int i = 0; i < this.EditedPoints.Count; i++)
            {
                Rotate3D.RotateVectors(rotp, this.EditedPoints[i].ToArray());
            }
              RectangleF3D rec3d = Rotate3D.RotateFaces_inform(rotp, this.FinalFaces);

              this.Rec = rec3d;
            for (int i = 0; i < this.cords.Length; i++)
            {             
                    Rotate3D.RotateVectors(rotp, cords[i]);
 
            }

            this.rotateangleww = new PointF3D(rotp.X + rotateangleww.X, rotp.Y + rotateangleww.Y,rotateangleww.Z);
            if (rotateangleww.X > 360) { rotateangleww.X = rotateangleww.X - 360; }
            else if (rotateangleww.X < 0) { rotateangleww.X = rotateangleww.X + 360; }

           if (rotateangleww.Y > 360) { rotateangleww.Y = rotateangleww.Y - 360; }
           else if (rotateangleww.Y < 0) { rotateangleww.Y = rotateangleww.Y + 360; }
           //old models have rotates null
            if (rotates == null) rotates = new List<PointF3D>();
          rotates.Add(new PointF3D(rotp.Y,rotp.X,0));
            if (rotates.Count > 100)
            { rotates.Clear(); }


            Rotate3D.RotateVectors(rotp, this.BoxVects);
            this.MuliplyRotate(rotp, planx);
            this.MuliplyRotate(rotp, plany);
            this.MuliplyRotate(rotp, planz);
        }

        public void MuliplyRotate(PointF3D rotp)
        {
            for (int i = 0; i < this.EditedPoints.Count; i++)
            {
                Rotate3D.RotateVectors(rotp, this.EditedPoints[i].ToArray());
            }
                 RectangleF3D rec3d = Rotate3D.RotateFaces_inform(rotp, this.FinalFaces);

                 this.Rec = rec3d;
            for (int i = 0; i < this.cords.Length; i++)
            {             
                    Rotate3D.RotateVectors(rotp, cords[i]);
            }

            this.rotateangleww += rotp.X;
            if (rotateangleww.X > 360) { rotateangleww.X = rotateangleww.X - 360; }
            else if (rotateangleww.X < 0) { rotateangleww.X = rotateangleww.X + 360; }

            if (rotateangleww.Y > 360) { rotateangleww.Y = rotateangleww.Y - 360; }
            else if (rotateangleww.Y < 0) { rotateangleww.Y = rotateangleww.Y + 360; }
         
            //old models have rotates null
            if (rotates == null) rotates = new List<PointF3D>();
            rotates.Add(rotp);
            if (rotates.Count>100)
            { rotates.Clear(); }


            Rotate3D.RotateVectors(rotp, this.BoxVects);
            this.MuliplyRotate(rotp, planx);
            this.MuliplyRotate(rotp, plany);
            this.MuliplyRotate(rotp, planz);
        }

        public void MuliplyRotate(PointF rotp, List<List<Vector3>> lst)
        {
            PointF3D minp = new PointF3D();
            PointF3D maxp = new PointF3D();

            for (int i = 0; i < lst.Count; i++)
            {    
                Rotate3D.RotateVectors(rotp, lst[i].ToArray());
            }
       }
        public void MuliplyRotate(PointF3D rotp, List<List<Vector3>> lst)
        {
            PointF3D minp = new PointF3D();
            PointF3D maxp = new PointF3D();

            for (int i = 0; i < lst.Count; i++)
            {
                Rotate3D.RotateVectors(rotp, lst[i].ToArray());
            }
        }

    }
    [Serializable()]
    public class Models3Dlist
    {
        public static Models3Dlist FromStream(Stream strm)
        {
         
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Models3Dlist v =(Models3Dlist) binaryFormatter.Deserialize(strm);    
            return v;
        }
    
        private List<Model3D> models = new List<Model3D>();
        public List<Model3D> Models { get { return models; } }
        public Models3Dlist()
        {

        }
        public Models3Dlist(List<Model3D> mdls)
        {
            this.models = mdls;
        }
        public static Models3Dlist FromFile(string address)
        {
            return fnc.Selraliz.ReadFromBinaryFile<Models3Dlist>(address);
        }

        public void SaveAsXML(string addres)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Models3Dlist));
            using (StreamWriter streamWriter = System.IO.File.CreateText(addres))
            {
                xmlSerializer.Serialize(streamWriter, this);
            }
        }
        public static  Models3Dlist FromXML(string addres)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Models3Dlist));
            FileStream readStream = new FileStream(addres, FileMode.Open,FileAccess.Read);
            return (Models3Dlist)xmlSerializer.Deserialize(readStream);
        }
        public bool Save(string address)
        {
            try
            {
                fnc.Selraliz.WriteToBinaryFile(address, this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
    

