using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace SolveText
{
    public partial class EquationForm : Form
    {
        public  static Models3Dlist Getmodelfromresources(string name)
        {           
            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resources = thisExe.GetManifestResourceNames();
            System.IO.Stream file =thisExe.GetManifestResourceStream("EquationGrapher3D.Resources."+name+".mdls");
            Models3Dlist m=null;
         
                m = Models3Dlist.FromStream(file);
          
        
            return m;
          }
    public EquationForm()
        {
            InitializeComponent();
        }

        private bool small = false;

   
        List<EquationFile> parmeqsexpmles = new List<EquationFile>();
        List<EquationFile> explicitexpmles = new List<EquationFile>();
        List<EquationFile> desigenexpmles = new List<EquationFile>();
       
        List<string> modelslist = new List<string >();
        

        public float InvokeProgress
        {
            get { return kscale_progress.ValueF; }
            set {  if (value >= 0 && value <= 100) { kscale_progress.Invoke(new Action(() => kscale_progress.ValueF = value)); } }
        }

        private void neweq_Load(object sender, EventArgs e)
        {
            kscale_draw_model.Value = 1;
       

            textbox_x.ContextMenuStrip = textbox_y.ContextMenuStrip = textbox_z.ContextMenuStrip = textBox_explicit.ContextMenuStrip= textcontext.make();

            EquationFile dq1 = new EquationFile();
            EquationFile dq2 = new EquationFile();
            dq1.Title = "Rotation";
            dq1.ParamX = "x*sin(v)";
            dq1.ParamY = "y";
           dq1.ParamZ = "x*cos(v)";
            dq1.DomainV = new PointF(-3.14f, 3.14f);
         
            dq2.Title = "extened";
            dq2.ParamX = "x";
            dq2.ParamY = "y";
            dq2.ParamZ = "v";
            dq2.DomainV = new PointF(-0.1f, 0.1f);
            desigenexpmles.AddRange(new EquationFile[] { dq1, dq2 });
            for (int i = 0; i < desigenexpmles.Count; i++)
            {
                this.kcombobox_dex.Items.Add(new comboitem(desigenexpmles[i].Title, desigenexpmles[i]));
            }
            kDynamkPanel1.Location = new Point();
            this.Size = kDynamkPanel1.Size;
            EquationFile eq1 = new EquationFile(); EquationFile eq26 = new EquationFile();
            EquationFile eq2 = new EquationFile(); EquationFile eq27 = new EquationFile();
            EquationFile eq3 = new EquationFile(); EquationFile eq28 = new EquationFile();
            EquationFile eq4 = new EquationFile();
            EquationFile eq5 = new EquationFile();
            EquationFile eq6 = new EquationFile();
            EquationFile eq7 = new EquationFile();
            EquationFile eq8 = new EquationFile();
            EquationFile eq9 = new EquationFile();
            EquationFile eq10 = new EquationFile();
            EquationFile eq11 = new EquationFile();
            EquationFile eq12 = new EquationFile();
            EquationFile eq13 = new EquationFile();
            EquationFile eq14 = new EquationFile();
            EquationFile eq15 = new EquationFile();
            EquationFile eq16 = new EquationFile();
            EquationFile eq17 = new EquationFile();
            EquationFile eq18 = new EquationFile();
            EquationFile eq19 = new EquationFile();
            EquationFile eq20 = new EquationFile();
            EquationFile eq21 = new EquationFile();
            EquationFile eq22 = new EquationFile();
           
            EquationFile eq24 = new EquationFile();
            EquationFile eq25 = new EquationFile();

            eq1.Title = "Klein";
            eq1.ParamX = "(c1*(1+sin(v)) + c3*c4*c5)*cos(v)";
            eq1.ParamY = "(c2+c3*c4*c5)*sin(v)";
            eq1.ParamZ = "-c3*c4*sin(u)";
            eq1.DomainU = new PointF(0, 2 * 3.14f);
            eq1.DomainV = new PointF(0, 2 * 3.14f);
            eq1.Image = EquationGrapher3D.Properties.Resources.klein;
            eq1.ParamHelpingvariblesequations = new string[]
            {
                "3",
                "4",
                "2",
                    "1-cos(v)/c3",
            "cos(u)","1"
              };

            eq2.Title = "Tuors";
            eq2.ParamX = "c3*cos(v)";
            eq2.ParamY = "c1*sin(u)";
            eq2.ParamZ = "c3*sin(v)";
            eq2.DomainU = new PointF(0, 2 * 3.14f);
            eq2.DomainV = new PointF(0, 2 * 3.14f);
            eq2.Image = EquationGrapher3D.Properties.Resources.trous1;
            eq2.ParamHelpingvariblesequations = new string[]
            {
                "0.5",
                "1",
                "c2+ c1*cos(u)"
              };

            eq3.Title = "Cosinus";
            eq3.ParamX = "u";
            eq3.ParamY = "sin(pi*((u)^c2+(v)^c2))/c1";
            eq3.ParamZ = "v";
            eq3.DomainU = new PointF(-1, 1);
            eq3.DomainV = new PointF(-1, 1);
            eq3.Image = EquationGrapher3D.Properties.Resources.cosinus;
            eq3.ParamHelpingvariblesequations = new string[]
            {
                "2",
                "2",
              };
            eq4.Title = "Sphere";
            eq4.ParamX = "cos(u)*cos(v)";
            eq4.ParamY = "sin(u)";
            eq4.ParamZ = "cos(u)*sin(v)";
            eq4.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq4.DomainV = new PointF(0, 2 * 3.14f);
            eq4.Image = EquationGrapher3D.Properties.Resources.sphere;

            eq5.Title = "Drop";
            eq5.ParamX = "u*cos(v)";
            eq5.ParamY = "exp(-u*u)*(sin(c1*pi*u) - u*cos(c2*v))";
            eq5.ParamZ = "u*sin(v)";
            eq5.DomainU = new PointF(0, 2);
            eq5.DomainV = new PointF(0, 2 * 3.14f);
            eq5.Image = EquationGrapher3D.Properties.Resources.drop_vimg;
            eq5.ParamHelpingvariblesequations = new string[]
            {
                "3",
                "3",
              };

            eq6.Title = "Heart";
            eq6.ParamX = "sin(u)*c4";
            eq6.ParamY = "cos(u)*c4";
            eq6.ParamZ = "v";
            eq6.DomainU = new PointF(-3.14f, 3.14f);
            eq6.DomainV = new PointF(-1, 1);
            eq6.Image = EquationGrapher3D.Properties.Resources.herat_vimg;
            eq6.ParamHelpingvariblesequations = new string[]
            {
                "1",
             "2",
             "4",
                 "c3*sqrt(1-v^c2)*sin(abs(u))^abs(c1*u)",
              };

            eq7.Title = "Cube";
            eq7.ParamX = "cos(u)*cos(v)*c5";
            eq7.ParamY = "sin(u)*c4";
            eq7.ParamZ = "cos(u)*sin(v)*c5";
            eq7.ParamHelpingvariblesequations = new string[] 
            {"4/4"
            ,"4/4"
            ,"100"
            , "(abs(cos(u*c1))^c3 + abs(sin(u*c1))^c3)^(-1/c3)",
             "c4*(abs(cos(v*c2))^c3 + abs(sin(v*c2))^c3)^(-1/c3)" };

            eq7.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq7.DomainV = new PointF(0, 2 * 3.14f);
            eq7.Image = EquationGrapher3D.Properties.Resources.Cubeimg;

            eq8.Title = "Star";
            eq8.ParamX = "cos(u)*cos(v)*c9";
            eq8.ParamY = "sin(u)*c8";
            eq8.ParamZ = "cos(u)*sin(v)*c9";
            eq8.DomainU = new PointF(-3.14f / 2, 3.14f / 2);
            eq8.DomainV = new PointF(0, 2 * 3.14f);
            eq8.Image = EquationGrapher3D.Properties.Resources.asdwe;
            eq8.ParamHelpingvariblesequations = new string[]
            {
                "0.25",
                "0.5",
                "0.3",
                "1.25",
                "1.7"
                ,"0.3"
                ,"0.1"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c6)",
                "c8*(abs(cos(v*c4))^c5 + abs(sin(v*c4))^c5)^(-1/c7)"
            };

            eq9.Title = "Prisim";      
            eq9.ParamX = "cos(u)*cos(v)*c7";
            eq9.ParamY = "sin(u)*c6";
            eq9.ParamZ = "cos(u)*sin(v)*c7";
            eq9.DomainU = new PointF(-3.14f / 2, 3.14f / 2);
            eq9.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "200",
                "3/4",
                "500",
                "260"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"

           };
            eq9.DomainV = new PointF(0, 2 * 3.14f);
            eq9.Image = EquationGrapher3D.Properties.Resources.prisim;


            eq10.Title = "Plan";
            eq10.ParamX = "u";
            eq10.ParamY = "v";
            eq10.ParamZ = "0";
            eq10.DomainU = new PointF(-1f, 1f);
            eq10.DomainV = new PointF(-1f, 1f);
          eq10.Image = EquationGrapher3D.Properties.Resources.plan ;

            eq11.Title = "Shell";
            eq11.ParamX = "c2*(c4 *sin(v))";
            eq11.ParamY = "c2*(c3*cos(u))";
            eq11.ParamZ = "c2*(c4 *cos(v))";
            eq11.ParamHelpingvariblesequations = new string[] {
                "1.2",
                "c1^v",
                "sin(u)",
                "c3^2" };
            eq11.DomainU = new PointF(-1.57f, 1.57f);
            eq11.DomainV = new PointF(-3.14f/4f, 5f*3.14f/2f);
            eq11.Image = EquationGrapher3D.Properties.Resources.shell;

            eq12.Title = "Hexaedron";
            eq12.ParamX = "cos(v)^c1*c2";
            eq12.ParamY = "sin(u)^c1";
            eq12.ParamZ = "sin(v)^c1*c2";
            eq12.DomainU = new PointF(-1.3f, 1.3f);
            eq12.DomainV = new PointF(0, 2f * 3.14f);
            eq12.Image = EquationGrapher3D.Properties.Resources.hexaedro;
            eq12.ParamHelpingvariblesequations = new string[] {
                "3",
                "cos(u)^c1",
              };

            eq13.Title = "Cone";
            eq13.ParamX = "u*cos(v)";
            eq13.ParamY = "u";
            eq13.ParamZ = "u*sin(v)";
            eq13.DomainU = new PointF(-1, 1);
            eq13.DomainV = new PointF(0, 3.14f * 2f);
          eq13.Image = EquationGrapher3D.Properties.Resources.Cone;
           

            eq14.Title = "Helx";
            eq14.ParamX = "c5*cos(u)";
            eq14.ParamY = "c1*(sin(v) + u/c2 -c4)";
            eq14.ParamZ = "c5*sin(u)";
            eq14.DomainU = new PointF(0f, 4f * 3.14f);
            eq14.DomainV = new PointF(0f, 2 * 3.14f);
            eq14.Image = EquationGrapher3D.Properties.Resources.helx;
            eq14.ParamHelpingvariblesequations = new string[] {
                "0.1",
                "1.7",
                "1",
                "10"
                ,"c3-c1*cos(v)"
            };

            eq15.Title = "Toupie";
            eq15.ParamX = "c3*cos(v)";
            eq15.ParamY = "u";
            eq15.ParamZ = "c3*sin(v)";
            eq15.DomainU = new PointF(-1, 1);
            eq15.DomainV = new PointF(0, 2 * 3.14f);
            eq15.Image = EquationGrapher3D.Properties.Resources.toupi;
            eq15.ParamHelpingvariblesequations = new string[] {
                "1",
                "2",
                "(abs(u)-1)^2",
            };

            eq16.Title = "Bonbon";
            eq16.ParamX = "u";
            eq16.ParamY = "cos(u)*sin(v)";
            eq16.ParamZ = "cos(u)*cos(v)";
            eq16.DomainU = new PointF(0, 2 * 3.14f);
            eq16.DomainV = new PointF(0, 2 * 3.14f);
            eq16.Image = EquationGrapher3D.Properties.Resources.bonbon;

            eq17.Title = "Catenoid";
            eq17.ParamX = "c2*cos(u)";
            eq17.ParamY = "v";
            eq17.ParamZ = "c2*sin(u)";
            eq17.DomainU = new PointF(-3.14f, 3.14f);
            eq17.DomainV = new PointF(-3.14f, 3.14f);
            eq17.Image = EquationGrapher3D.Properties.Resources.catrnod;
            eq17.ParamHelpingvariblesequations = new string[] {
                "2",
                "2*cosh(v/c1)",
            };

            eq18.Title = "Snake";
            eq18.ParamX = "c2*cos(c3)*c5+ c4*cos(c3)";
            eq18.ParamY = "c6*v/c1+c2*sin(u)";
            eq18.ParamZ = "c2*sin(c3)*c5 + c4*sin(c3)";
            eq18.DomainU = new PointF(0, 2f * 3.14f);
            eq18.DomainV = new PointF(0, 2f * 3.14f);
            eq18.Image = EquationGrapher3D.Properties.Resources.snake;
            eq18.ParamHelpingvariblesequations = new string[] {
              "2*pi",  "1.2*(1 -v/c1)",
                "3*v",
                "3",
                "(1 + cos(u))",            
                "9"
            };

            eq19.Title = "Hexagon";
            eq19.ParamX = "cos(u)*cos(v)*c8";
            eq19.ParamY = "sin(u)*c7";
            eq19.ParamZ = "cos(u)*sin(v)*c8";          
            eq19.ParamHelpingvariblesequations = new string[] {
                "4/4","300","300","6/4","400","1000",
                "(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c3)",
                "c7*(abs(cos(v*c4))^c5 + abs(sin(v*c4))^c5)^(-1/c6)"
            };
            eq19.DomainU = new PointF(-1.57f, 1.57f);
            eq19.DomainV = new PointF(0, 6.28f);
            eq19.Image = EquationGrapher3D.Properties.Resources.heaxgon;

            eq20.Title = "Humicane";

            eq20.ParamX = "(c8*c10-c7*c9)";
               eq20.ParamY = "c2*v";
            eq20.ParamZ = "(c8*c9+c7*c10)";

         eq20.ParamHelpingvariblesequations = new string[] {
             "2.1","2*c1","0.35","13.2","2*cosh(v/2)","(c2*v+c4)*c3","c1*(c5*sin(u))","c5*cos(u)","sin(c6)","cos(c6)"
           };
            eq20.DomainU = new PointF(-3.14f, 3.14f);
            eq20.DomainV = new PointF(-3.14f, 3.14f);
            eq20.Image = EquationGrapher3D.Properties.Resources.humance;


            eq21.Title = "DNA";
            eq21.ParamX = "c4*cos(v)";
            eq21.ParamY = " c1 * (c5 * sin(c6) + c2*sin(u) * cos(c6))";
            eq21.ParamZ = "c1 * (c5 * cos(c6) - c2*sin(u) * sin(c6))  ";
            eq21.ParamHelpingvariblesequations = new string[]
           {
               "0.632","0.5","2",
               "1+ c2*cos(u)",   "c4*sin(v) ",
                "(c4*cos(v) +1.5)*c3",           
           };
            eq21.DomainU = new PointF(0, 2 * 3.14f);
            eq21.DomainV = new PointF(0, 2 * 3.14f);
            eq21.Image = EquationGrapher3D.Properties.Resources.DNA;

            eq22.Title = "EightSurface";
            eq22.ParamX = "cos(u)*sin(2*v)";
            eq22.ParamY = "sin(v)";
            eq22.ParamZ = "sin(u)*sin(2*v)";
            eq22.DomainU = new PointF(0, 2 * 3.14f);
            eq22.DomainV = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq22.Image = EquationGrapher3D.Properties.Resources.eightsurface;

            EquationFile eq23 = new EquationFile("Star_7",
             "cos(u)*cos(v)*c9"
         , "sin(u)*c7"
        , "cos(u)*sin(v)*c9"
        , new PointF(-3.14f/2, 3.14f/2), new PointF(0, 3.14f*2f)
        , EquationGrapher3D.Properties.Resources.star_7);
            eq23.ParamHelpingvariblesequations = new string[]

            {"7/4","1.7","0.2","7/4","1.7","0.2"
    ,"(abs(cos(u*c1))^c2+ abs(sin(u*c1))^c2)^(-1/c3)"
    ,"(abs(cos(v*c4))^c5+abs(sin(v*c4))^c5)^(-1/c6)"
    ,"c7*c8"
            };       

            eq24.Title = "Limpet_Tuors";
            eq24.ParamX = "cos(u) / c2";
            eq24.ParamY = "1 / (c1 + cos(v))";
            eq24.ParamZ = "sin(u) / c2";
            eq24.ParamHelpingvariblesequations = new string[] { "sqrt(2)", "c1+sin(v)" };
            eq24.DomainU = new PointF(0, 2 * 3.14f);
            eq24.DomainV = new PointF(0, 5f);
           eq24.Image = EquationGrapher3D.Properties.Resources.limput_trous;

            eq25.Title = "Spehere_dashed";
            eq25.ParamX = "cos(u)*cos(v)*c5";
            eq25.ParamY = "sin(u)*c4";
            eq25.ParamZ = "cos(u)*sin(v)*c5";
            eq25.ParamHelpingvariblesequations = new string[] { "0.1", "2.5", "100", "(abs(cos(u*c1))^c3 + abs(sin(u*c1))^c3)^(-1/c3)", "c4*(abs(cos(v*c2))^c3 + abs(sin(v*c2))^c3)^(-1/c3)" };
            eq25.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq25.DomainV = new PointF(0, 2 * 3.14f);
            eq25.Image = EquationGrapher3D.Properties.Resources.sphere_dashed;

            eq26.Title = "Diamond";
            eq26.ParamX = "cos(u)*cos(v)*c5";
            eq26.ParamY = "sin(u)*c4";
            eq26.ParamZ = "cos(u)*sin(v)*c5";
            eq26.ParamHelpingvariblesequations = new string[] { "4/4","1","1","(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c3)", "c4*(abs(cos(v*c1))^c2 + abs(sin(v*c1))^c2)^(-1/c3)" };

            eq26.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq26.DomainV = new PointF(0, 2 * 3.14f);
            eq26.Image = EquationGrapher3D.Properties.Resources.Diamond;

            eq27.Title = "Pesudo_Hexagon";
            eq27.ParamX = "c10*(cos(u)*cos(v)*c9)";
            eq27.ParamY = "c13 * ((cos(u)*sin(v)*c9) * sin(c11) +c12*(sin(u)*c7) * cos(c11))";
            eq27.ParamZ = "c13 * ((cos(u)*sin(v)*c9) * cos(c11) -c12*(sin(u)*c7) * sin(c11))  ";
            eq27.ParamHelpingvariblesequations = new string[]
            {
              "4/4"
              ,"300"
              ,"300"
              ,"6/4"
              ,"400"
               , "1000"
               ,"(abs(cos(u*c1))^c2+ abs(sin(u*c1))^c2)^(-1/c3)"
               ,"(abs(cos(v*c4))^c5+ abs(sin(v*c4))^c5)^(-1/c6)"
               ,"c7*c8"
               ,"1.7"
               ,"(c10*(cos(u)*cos(v)*c9)+c10)*0.55"
               ,"0.7"
               ,"0.75"
            };

            eq27.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq27.DomainV = new PointF(0, 2 * 3.14f);
            eq27.Image = EquationGrapher3D.Properties.Resources.Pesudo_h;

            eq28.Title = "Shape_8";
            eq28.ParamX = "cos(u)*cos(v)*c9";
            eq28.ParamY = "sin(u)*c7";
            eq28.ParamZ = "cos(u)*sin(v)*c9";
            eq28.DomainU = new PointF(-3.14f / 2f, 3.14f / 2f);
            eq28.DomainV = new PointF(0, 2 * 3.14f);
            eq28.Image = EquationGrapher3D.Properties.Resources.shape_8;
            eq28.ParamHelpingvariblesequations = new string[]
           {
              "3/4"
              ,"100"
              ,"100"
              ,"2/4"
              ,"0.3"
               , "0.7"
               ,"(abs(cos(u*c1))^c2+ abs(sin(u*c1))^c2)^(-1/c3)"
               ,"(abs(cos(v*c4))^c5+ abs(sin(v*c4))^c5)^(-1/c6)"
               ,"c7*c8"

           };
            EquationFile eq29 = new EquationFile("2D Star", "c5*(sin(0-(u*c1)))+(c3*sin(u))", "c5*(cos(0-(u*c2)))+(c4*cos(u))", "v", new PointF(-3 * 3.14f, 3 * 3.14f), new PointF(-1, 1));
            eq29.ParamHelpingvariblesequations = new string[]
    {
              "2/3","2/3"
              ,"2"
              ,"3"
              ,"6"
    };
            eq29.Image = EquationGrapher3D.Properties.Resources._2dstar;

            EquationFile eq30 = new EquationFile("Pesudo_cone"
              , "u*cos(v)"
              , "c1*(u*sin(v)*sin(c4)+u*cos(c4))"
              , "c1*(u*sin(v)*cos(c4)-u*sin(c4)) "
              , new PointF(-1, 1), new PointF(0, 2 * 3.14f)
              , EquationGrapher3D.Properties.Resources.pesudo_cone);
            eq30.ParamHelpingvariblesequations = new string[]
{
              "0.84","pi","1"
              ,"(u*cos(v)+c3)*c2"

};
            EquationFile eq31 = new EquationFile("Kidney"
        , "cos(u) *(3  *cos(v) - cos(3  *v))"
        , "3  *sin(v) - sin(3 * v)"
        , "sin(u)  *(3  *cos(v) - cos(3 * v))"
        , new PointF(0, 2 * 3.14f), new PointF(-3.14f / 2f, 3.14f / 2f)
        , EquationGrapher3D.Properties.Resources.kidney);

            EquationFile eq32 = new EquationFile("Folium"
               , "cos(u) *(2*v/pi - tanh(v))"
               , "cos(u - 2*pi / 3) / cosh(v)"
               , "cos(u + 2*pi / 3) / cosh(v)"
               , new PointF(-3.14f, 3.14f), new PointF(-3.14f, 3.14f)
               , EquationGrapher3D.Properties.Resources.folium);

            EquationFile eq33 = new EquationFile("epicycloid"
            , "c4*sin(V)"
            , "c5"
            , "c4*cos(v)"
            , new PointF(-1.57f ,1.57f), new PointF(-3.14f, 3.14f)
            , EquationGrapher3D.Properties.Resources.epicycloid1);
            eq33.ParamHelpingvariblesequations = new string[]
            {
                "0.5","10","5","c3*cos(u)-c1*cos(c2*u)","c3*sin(u)-c1*sin(c2*u)"
            };

            EquationFile eq34 = new EquationFile("Cup"
            , "c1*(v-pi)"
            , "c4*sin(u)"
            , "c4*cos(u)"
            , new PointF(0, 2f * 3.14f), new PointF(0, 2f * 3.14f)
            , EquationGrapher3D.Properties.Resources.cup_vimg);
            eq34.ParamHelpingvariblesequations = new string[]
            {
                "2",
                "7/8",
                "3",
                "(c1* sin(v + pi*c2) + c3)",                         
            };


            EquationFile eq35 = new EquationFile("Horn"
         , "c4*sin(c3)"
         , "u *sin(v)"
         , "c4*cos(c3) + c1*u"
         , new PointF(0, 1), new PointF(0, 2f * 3.14f)
         , EquationGrapher3D.Properties.Resources.horn_vimg);
            eq35.ParamHelpingvariblesequations = new string[]
            {
                "2",
                "2",
                "c2*pi*u",
                "c1+ u*cos(v)",                         
            };

            EquationFile eq36 = new EquationFile("2D Heart"
       , "4*sin(u)^3"
       , "0.25*(13*cos(u)-5*cos(2*u)-2*cos(3*u)-cos(4*u))"
       , "v"
       , new PointF(0, 6.28f), new PointF(0, 2f)
     , EquationGrapher3D.Properties.Resources._2dheartimgimg);
        

            EquationFile eq37 = new EquationFile("Barrel"
     , "2*(sin(v)-pi)*cos(v)"
     , "c1*sin(u)"
     , "c1*cos(u)"
     , new PointF(0, 6.28f), new PointF(0, 6.28f)
     , EquationGrapher3D.Properties.Resources.barrelimg);
            eq37.ParamHelpingvariblesequations = new string[]
           {
                "(2*sin(v+pi*7/8)+3)",
           };
            EquationFile eq38 = new EquationFile("BentHorns"
     , "(2+cos(u))*(v/3 -sin(v))"
     , "(2+cos(u-2*PI/3))*(cos(v)-1)"
     , "(2+cos(u+2*PI/3))*(cos(v)-1)"
     , new PointF(-3, 3), new PointF(-6, 6)
     , EquationGrapher3D.Properties.Resources.BentHornsimg);

            EquationFile eq39 = new EquationFile("Circle Loops"
     , "c4*cos(u)"
     , "c4*sin(u)"
     , "c1*sin(c3)+c2*sin(v)"
     , new PointF(-12.56f, 12.56f), new PointF(-3.14f, 3.14f)
     , EquationGrapher3D.Properties.Resources.Circle_Loops_IIimg);
           eq39.ParamHelpingvariblesequations = new string[]
            {
                "5",
                "1",
                "u/4",
                "c1*cos(c3)+c2*cos(v)",                         
            };
        
            EquationFile eq40 = new EquationFile("Elliptical Torus"
     , "(1.5+cos(v))*cos(u)"
     , "(1.5+cos(v))*sin(u)"
     , "sin(v)+cos(v)"
     , new PointF(0, 6.28f), new PointF(0, 6.28f)
     , EquationGrapher3D.Properties.Resources.EllipticalTorusimg);

            EquationFile eq41 = new EquationFile("interiotion"
     , "c1*sin(v)"
     , "c2"
     , "c1*cos(v)"
     , new PointF(-3.14f, 3.14f), new PointF(-3.14f, 3.14f)
     , EquationGrapher3D.Properties.Resources.interiotionimg);

            eq41.ParamHelpingvariblesequations = new string[]
            {
                "2*(sin(u)+1*sin(2*u))"
                ,"2*(cos(u)-1*cos(2*u))"
            };

            EquationFile eq42 = new EquationFile("Klein Cycloid"
     , "cos(u/2) * cos(u/3) * (6 + cos(v)) + sin(u/3) * sin(v) * cos(v)"
     , "sin(u/2) * cos(u/3) * (6 + cos(v)) + sin(u/3) * sin(v) * cos(v)"
     , "-sin(u/3) * (6 + cos(v)) + cos(u/3) * sin(v) * cos(v)"
     , new PointF(-25f, 25f), new PointF(0f, 25f)
     , EquationGrapher3D.Properties.Resources.KleinCycloid_unsafedomanimg);

            EquationFile eq43 = new EquationFile("Kuens"
   , "2*(cos(u)+u*sin(u))*sin(v)/(1+u*u*sin(v)*sin(v))"
   , "2*(sin(u)-u*cos(u))*sin(v)/(1+u*u*sin(v)*sin(v))"
   , "log(tan(v/2))+2*cos(v)/(1+u*u*sin(v)*sin(v))"
   , new PointF(-5, 4), new PointF(0.05f, 3.14f)
   , EquationGrapher3D.Properties.Resources.KuensSurfaceimg);

            EquationFile eq44 = new EquationFile("Stiletto"
 , "(2 + cos(u))*cos(v)^3 *sin(v)"
 , "(2 + cos(u+2*pi/3))*cos(v+2*pi/3)^2*sin(v+2*pi/3)^2"
 , "-(2 + cos(u-2*pi/3))*cos(v+2*pi/3)^2*sin(v+2*pi/3)^2"
 , new PointF(0, 6.28f), new PointF(0, 6.28f)
 , EquationGrapher3D.Properties.Resources.Stilettoimg);

            EquationFile eq45 = new EquationFile("Twisted Horn"
   , "2*(1-exp(u/(6*pi)))*cos(u)*cos(v/2)^2"
   , "2*(-1+exp(u/(6*pi)))*sin(u)*cos(v/2)^2"
   , "1-exp(u/(3*pi))-sin(v)+exp(u/(6*pi))*sin(v)"
   , new PointF(0, 12.56f), new PointF(0, 6.28f)
   , EquationGrapher3D.Properties.Resources.tiwstedhormimg);

            EquationFile eq46 = new EquationFile("TopTrous"
 , "1*cos(2*u)*cos(u)*cos(v)"
 , "1*cos(2*u)*cos(u)*sin(v)"
 , "1*cos(2*u)*sin(u)"
 , new PointF(-1.57f, 1.57f), new PointF(-3.14f, 3.14f)
 , EquationGrapher3D.Properties.Resources.TopTrousimg);
       
            EquationFile eq47 = new EquationFile("Trefoil"
   , "c4*cos(u)+(c2*(c9*cos(v)*c6-sin(v)*c5*c7)/(c8*c9))"
   , "c4*sin(u)-(c2*(c9*cos(v)*c5+sin(v)*c6*c7)/(c8*c9))"
   , "c4*sin(sin(c3))+(c2*sin(v)*c8/c9)"
   , new PointF(0, 12.56f), new PointF(-3.14f, 3.14f)
   , EquationGrapher3D.Properties.Resources.Trefoilimg);
         
            eq47.ParamHelpingvariblesequations = new string[]
            {
                "10",
                "2",
                "3*u/2",
                "c1*(cos(c3)+3)/4",
                "-c4*sin(u)-3*c1*sin(c3)*cos(u)/8",
                "c4*cos(u)-3*c1*sin(c3)*sin(u)/8",
                "(3*c4*cos(sin(c3))*cos(c3))/2-(3*c1*sin(sin(c3))*sin(c3))/8",
                "sqrt(c5^2+c6^2)",
                "sqrt(c5^2+c6^2+c7^2)"

            };
            EquationFile eq48 = new EquationFile("Triangle Tube"
  , "c3*sin(v)"
  , "c3*cos(v)"
  , "c4"
  , new PointF(-3.14f, 3.14f), new PointF(-3.14f,3.14f)
  , EquationGrapher3D.Properties.Resources.TriangleTube);
            eq48.ParamHelpingvariblesequations = new string[]
        {"2","2",
                "c1*cos(u) + cos(c2*u)",
                "c1*sin(u) - sin(c2*u)"
        };

            EquationFile eq49 = new EquationFile("Twisted Coil"
 , "c1*cos(u)"
 , "c1*sin(u)"
 , "sin(v) + 2*sin(5/3 * u)"
 , new PointF(-3.14f, 18), new PointF(0, 6.28f)
 , EquationGrapher3D.Properties.Resources.Twisted_Coilimg);
            eq49.ParamHelpingvariblesequations = new string[]
           {
                "7 + 2*cos(5/3 * u) + cos(v)",              
           };

            EquationFile eq50 = new EquationFile("Spiral Snake"
, "(((1/(2*pi))*u)+((1/(1.5*pi))*pi/2)*cos(v))*cos(u)"
, "(((1/(2*pi))*u)+((1/(1.5*pi))*pi/2)*cos(v))*sin(u)"
, "((1/(1.5*pi))*pi/1.5)*sin(v)"
, new PointF(0, 25), new PointF(-3.14f, 3.14f)
, EquationGrapher3D.Properties.Resources.SpiralSnakeimg);

            EquationFile eq51 = new EquationFile("electrons levels",
                "c4*cos(u)+(c10*cos(v)*c6-sin(v)*c5*c7)/(c9*c10)"
            , "c4*sin(u)+(-c10*cos(v)*c5-sin(v)*c6*c7)/(c9*c10)"
           , "c1*sin(c2*u)+sin(v)"
           , new PointF(-3.14f, 3.14f), new PointF(-3.14f, 3.14f)
           , EquationGrapher3D.Properties.Resources.SpringTorusimg);

            eq51.AutoDetectStep = false;
            eq51.Stepu = 0.02f;
            eq51.Stepv = 0.5f;
            eq51.ParamHelpingvariblesequations = new string[]
            {
                "20",
                "15",
                "c2*u",
                "14+c1*cos(c3)",
                "-c4*sin(u)-c1*c2*cos(u)*sin(c3)",
                "c4*cos(u)-c1*c2*sin(u)*sin(c3)",
                "c1*c2*cos(c3)",
                "c5^2+c6^2",
                "sqrt(c8)",
                "sqrt(c9+c7^2)"
            };
        
            EquationFile eq52 = new EquationFile("3Ploes"
 , "cos(u)*cos(v)*c9"
 , "sin(u)*c8"
 , "cos(u)*sin(v)*c9"
 , new PointF(-1.57f, 1.57f), new PointF(0, 6.28f)
 , EquationGrapher3D.Properties.Resources._3ploes);

            eq52.ParamHelpingvariblesequations = new string[]
            {
                "1",
                "4",
                "u",
                "5",
                "3",
                "1.7",
                "0.1",
                "(abs(cos(c1*u/c2))^c3 + abs(sin(c1*u/c2))^c3)^(-1/c4)",
                "c8*(abs(cos(c5*v/c2))^c6 + abs(sin(c5*v/c2))^c6)^(-1/c7)"

            };

 
            EquationFile eq53 = new EquationFile("Bullet"
, "c5*cos(v)"
, "c6*1.5"
, "c5*sin(v)"
, new PointF(0, 5.6f), new PointF(-3.14f, 3.14f)
, EquationGrapher3D.Properties.Resources.bullet);

            eq53.ParamHelpingvariblesequations = new string[]
            {
                "6",
                "3/2",
                "1",
                "3",
                "c1*(sin(0-(u/c2)))+(c3*sin(u))",
                "c1*(cos(0-(u/c2)))+(c4*cos(u))",     
           };

            EquationFile eq54 = new EquationFile("CakePlate"
, "c1*sin(v)"
, "c2"
, "c1*cos(v)"
, new PointF(2, 3.14f), new PointF(0, 6.28f)
, EquationGrapher3D.Properties.Resources.cakeplate);

            eq54.ParamHelpingvariblesequations = new string[]
            {
                "2*(sin(u)+2*sin(10*u))",
                "2*(cos(u)-2*cos(10*u))",    
            };
           EquationFile eq55 = new EquationFile("Closed Cylinder"
, "cos(u)*cos(v)*c7"
, "sin(u)*c6"
, "cos(u)*sin(v)*c7"
, new PointF(-1.57f, 1.57f), new PointF(0,6.28f)
, EquationGrapher3D.Properties.Resources.ClosedCylinderimg);

            eq55.ParamHelpingvariblesequations = new string[]
            {
                "1",
                "100",
                "3/4"
                ,"100"
                ,"1000"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"
           };


            EquationFile eq56 = new EquationFile("Dram"
, "cos(u)*cos(v)*c2"
, "sin(u)*c1"
, "cos(u)*sin(v)*c2"
, new PointF(-1.57f, 1.57f), new PointF(0f, 6.28f)
, EquationGrapher3D.Properties.Resources.dram);

            eq56.ParamHelpingvariblesequations = new string[]
            {
                "(abs(cos(4*u/4))^100 + abs(sin(4*u/4))^100)^(-1/100)",
                "c1^2",
            };
         
            EquationFile eq57 = new EquationFile("Filledcontainer"
, "c5*cos(v)"
, "c6"
, "c5*sin(v)"
, new PointF(-9.28f, 9.28f), new PointF(0f, 3.14f)
, EquationGrapher3D.Properties.Resources.filledcontainer);

            eq57.ParamHelpingvariblesequations = new string[]
            {
                "1",
                "5",
                "2",
                "3",
                "6*c1*(sin(0-(u/c2)))+(c3*sin(u))",
                "6*c1*(cos(0-(u/c2)))+(c4*cos(u))"
            };
   
            EquationFile eq58 = new EquationFile("Klein Bottel"
, "6*cos(u)*(1+sin(u))+c4*c3*cos(u)*cos(v)+c5*c3*cos(v+pi)"
, "16*sin(u)+c4*c3*sin(u)*cos(v)"
, "c3*sin(v)"
, new PointF(0, 6.28f), new PointF(0f, 6.28f)
, EquationGrapher3D.Properties.Resources.Kline_Bottleomg);

            eq58.ParamHelpingvariblesequations = new string[]
            {
                "sign(u-pi)",
                "2",
                "4*(1-cos(u)/2)",
                "(-1*c1)>0",
                "c1>0",
                "c4*cos(u)-c1*c2*sin(u)*sin(c3)",
                "c1*c2*cos(c3)",
                "c5^2+c6^2",
                "sqrt(c8)",
                "sqrt(c9+c7^2)"

            };
          
            EquationFile eq59 = new EquationFile("Pear"
, "c1*cos(v)"
, "c5"
, "c1*sin(v)"
, new PointF(0, 3.14f), new PointF(0f, 6.3f)
, EquationGrapher3D.Properties.Resources.pear);

            eq59.ParamHelpingvariblesequations = new string[]
            {
                "4*sin(u)^3",
                "13",
                "5",
                "3",
                "0.3*(c2*cos(u)-c3*cos(2*u)-c4*cos(3*u)-cos(4*u))",           
            };
  
            EquationFile eq60 = new EquationFile("Vlc"
, "c5*sin(v)"
, "c6"
, "c5*cos(v)"
, new PointF(0,6.28f), new PointF(0, 6.28f)
, EquationGrapher3D.Properties.Resources.vlc);

            eq60.ParamHelpingvariblesequations = new string[]
            {
                "6",
                "10/5",
                "2",
                "3",
                "c1*(sin(0-(u/c2)))+(c3*sin(u))",
                "c1*(cos(0-(u/c2)))+(c4*cos(u))",
             };
        
            EquationFile eq61 = new EquationFile("Winged Bullet"
, "c5*sin(v)"
, "c6"
, "c5*cos(v)"
, new PointF(-9.42f, 9.42f), new PointF(0f, 3.14f)
, EquationGrapher3D.Properties.Resources.wingedbullet);

            eq61.ParamHelpingvariblesequations = new string[]
            {
                "6",
                "3/2",
                "2",
                "3",
                "c1*(sin(0-(u/c2)))+(c3*sin(u))",
                "c1*(cos(0-(u/c2)))+(c4*cos(u))",
             }; 
          
            EquationFile eq62 = new EquationFile("Fourth Polygen"
, "cos(u)*cos(v)*c2"
, "sin(u)*c1"
, "cos(u)*sin(v)*c2"
, new PointF(0, 12.56f), new PointF(-3.14f, 3.14f)
, EquationGrapher3D.Properties.Resources.fourthpolgen);

            eq62.ParamHelpingvariblesequations = new string[]
            {
                "(abs(cos(4*u/4))^300 + abs(sin(4*u/4))^300)^(-1/100)",
                "c1*(abs(cos(4*v/4))^300 + abs(sin(4*v/4))^300)^(-1/100)",                
            };
          
            EquationFile eq63 = new EquationFile("Loop"
, "c4*cos(u)"
, "c1*(sin(v) + u/c3)"
, "c4*sin(u)"
, new PointF(0, 6.28f), new PointF(0, 2f * 3.14f)
, EquationGrapher3D.Properties.Resources.loop);
            eq63.ParamHelpingvariblesequations = new string[]
            {
                "0.1",
                "1",
                "100",
                "(c2-c1*cos(v))",
            };
            EquationFile eq64 = new EquationFile("Eight flower");          
            eq64.ParamX = "cos(u)*cos(v)*c7";
            eq64.ParamY = "sin(u)*c6";
            eq64.ParamZ = "cos(u)*sin(v)*c7";
            eq64.DomainU = new PointF(-3.14f / 2, 3.14f / 2);
            eq64.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "200",
                "8/4",
                "500",
                "50"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"

           };
            eq64.DomainV = new PointF(0, 2 * 3.14f);
            eq64.Image = EquationGrapher3D.Properties.Resources.eight_flower;

            EquationFile eq65 = new EquationFile("fifth flower",
             "cos(u)*cos(v)*c7",
          "sin(u)*c6",
            "cos(u)*sin(v)*c7",
           new PointF(-3.14f / 2, 3.14f / 2), new PointF(0, 2 * 3.14f),
 EquationGrapher3D.Properties.Resources.fifth_flower);

            eq65.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "200",
                "5/4",
                "500",
                "50"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"

           };

            EquationFile eq66 = new EquationFile("Five pump",
       "u",
    "c3*sin(v)",
      "c3*cos(v)",
     new PointF(0, 2), new PointF(0, 2 * 3.14f),
EquationGrapher3D.Properties.Resources.third_pump);

            eq66.ParamHelpingvariblesequations = new string[]
           {
                "3",
                "5",
                "exp(-u*u)*(sin(c1*pi*u) - u*cos(c2*v))",
          };

            EquationFile eq67 = new EquationFile("Pressed ball",
       "cos(u)*cos(v)*c5",
    "sin(u)*c4",
      "cos(u)*sin(v)*c5",
     new PointF(-1.57f, 1.57f), new PointF(0, 2 * 3.14f),
EquationGrapher3D.Properties.Resources.pressed_balls);

            eq67.ParamHelpingvariblesequations = new string[]
           {
                "2.5",
                "2",
                "abs(3*u)"
                ,"(abs(cos(u*c1))^c3 + abs(sin(u*c1))^c3)^(-1/c3)"
                ,"c4*(abs(cos(v*c2))^c3 + abs(sin(v*c2))^c3)^(-1/c3)"
          };

            EquationFile eq68 = new EquationFile("DashPrisim",
   "c7*sin(u)*c14*cos(v)",
"c7*cos(u)*c14*cos(v)",
  "c14*sin(v)",
new PointF(-3.14f, 3.14f) ,new PointF(-1.57f, 1.57f) ,
EquationGrapher3D.Properties.Resources.dash_prisim);

            eq68.ParamHelpingvariblesequations = new string[]
           {
                "5.7/4",
                "1",
                "1"
                ,"1"
                ,"2.5"
                ,"0.5"
                ,"(abs(cos(c1*u)/c2)^c3+abs(sin(c1*u)/c4)^c5)^(-1/c6)"
                ,"10/4"
                ,"1"
                ,"0.2"
                ,"1"
                ,"1"
                ,"3"
                ,"(abs(cos(c8*v)/c9)^c10+abs(sin(c8*v)/c11)^c12)^(-1/c13)"
          };

            EquationFile eq69 = new EquationFile("Circular Heart",
"c7*sin(u)*c14*cos(v)",
"c7*cos(u)*c14*cos(v)",
"c14*sin(v)",
new PointF(-3.14f, 3.14f), new PointF(-1.57f, 1.57f),
EquationGrapher3D.Properties.Resources.circular_heart);

            eq69.ParamHelpingvariblesequations = new string[]
           {
                "2/4",
                "1",
                "97.67"
                ,"1"
                ,"-0.439"
                ,"1"
                ,"(abs(cos(c1*u)/c2)^c3+abs(sin(c1*u)/c4)^c5)^(-1/c6)"
                ,"7/4"
                ,"1"
                ,"-0.0807"
                ,"1"
                ,"93"
                ,"-8.11"
                ,"(abs(cos(c8*v)/c9)^c10+abs(sin(c8*v)/c11)^c12)^(-1/c13)"
          };
            EquationFile eq70 = new EquationFile("Looped Sphere",
"c7*sin(u)*c14*cos(v)",
"c7*cos(u)*c14*cos(v)",
"c14*sin(v)",
new PointF(-3.14f, 3.14f), new PointF(-1.57f, 1.57f),
EquationGrapher3D.Properties.Resources.looped_sphere);

            eq70.ParamHelpingvariblesequations = new string[]
           {
                "2/4",
                "1",
                "10"
                ,"1"
                ,"-0.439"
                ,"1"
                ,"(abs(cos(c1*u)/c2)^c3+abs(sin(c1*u)/c4)^c5)^(-1/c6)"
                ,"7/4"
                ,"1"
                ,"15"
                ,"1"
                ,"93"
                ,"-8.11"
                ,"(abs(cos(c8*v)/c9)^c10+abs(sin(c8*v)/c11)^c12)^(-1/c13)"
          };

            EquationFile eq71 = new EquationFile("Scaled Star",
"c7*sin(u)*c14*cos(v)",
"c7*cos(u)*c14*cos(v)",
"c14*sin(v)",
new PointF(-3.14f, 3.14f), new PointF(-1.57f, 1.57f),
EquationGrapher3D.Properties.Resources.scaled_star);

            eq71.ParamHelpingvariblesequations = new string[]
           {
                "6/4",
                "1",
                "55"
                ,"1"
                ,"1000"
                ,"60"
                ,"(abs(cos(c1*u)/c2)^c3+abs(sin(c1*u)/c4)^c5)^(-1/c6)"
                ,"6/4"
                ,"1"
                ,"100"
                ,"1"
                ,"100"
                ,"250"
                ,"(abs(cos(c8*v)/c9)^c10+abs(sin(c8*v)/c11)^c12)^(-1/c13)"
          };

            EquationFile eq72 = new EquationFile("Marceds",
"cos(u)*cos(v)*c7",
"sin(u)*c6",
"cos(u)*sin(v)*c7",
new PointF(-3.14f/2f, 3.14f/2f), new PointF(0, 6.28f),
EquationGrapher3D.Properties.Resources.marcedes);

            eq72.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "200",
                "3/4"
                ,"260"
                ,"50"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"
          };

            EquationFile eq73 = new EquationFile("Engine Trous",
"cos(u)*cos(v)*c7",
"sin(u)*c6/3",
"cos(u)*sin(v)*c7",
new PointF(-3.14f / 2f, 3.14f / 2f), new PointF(0, 6.28f),
EquationGrapher3D.Properties.Resources.engine);

            eq73.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "100",
                "10/4"
                ,"200"
                ,"100"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"
          };
            EquationFile eq74 = new EquationFile("Five Star",
"cos(u)*cos(v)*c7",
"sin(u)*c6/2",
"cos(u)*sin(v)*c7",
new PointF(-3.14f / 2f, 3.14f / 2f), new PointF(0, 6.28f),
EquationGrapher3D.Properties.Resources.five_star);

            eq74.ParamHelpingvariblesequations = new string[]
           {
                "1",
                "100",
                "5/4"
                ,"200"
                ,"100"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"
          };

            EquationFile eq75 = new EquationFile("Trangle",
        "cos(u)*cos(v)*c7",
        "sin(u)*c6/4",
        "cos(u)*sin(v)*c7",
        new PointF(-3.14f / 2f, 3.14f / 2f), new PointF(0, 6.28f),
        EquationGrapher3D.Properties.Resources.trangle);

            eq75.ParamHelpingvariblesequations = new string[]
           {
                "3/4",
                "50",
                "3/4"
                ,"100"
                ,"50"
                ,"(abs(cos(u*c1))^c2 + abs(sin(u*c1))^c2)^(-1/c2)"
                ,"c6*(abs(cos(v*c3))^c4 + abs(sin(v*c3))^c4)^(-1/c5)"
          };
            //====================================================================================================================================

            parmeqsexpmles.AddRange(new EquationFile[] { eq6, eq2, eq3,eq51, eq4, eq5, eq1, eq34, eq35, eq7, eq8, eq9, eq10, eq11, eq12, eq13, eq14, eq15, eq16, eq17, eq18, eq19, eq20, eq21, eq22, eq23, eq24, eq25, eq26, eq27, eq28, eq29,eq63 ,eq30, eq31, eq32, eq33, eq36, eq37, eq38, eq39, eq40, eq41, eq42, eq43, eq44, eq45, eq46, eq47, eq48, eq49, eq50,eq52,eq53,eq54,eq55,eq56,eq57,eq58,eq59,eq60,eq61,eq62,eq64,eq65,eq66,eq67,eq68,eq69,eq70,eq72,eq73,eq74,eq75,eq71});

           
            for (int i = 0; i < parmeqsexpmles.Count; i++)
            {
                this.kcombobox_parm_empls.Items.Add(new comboitem(parmeqsexpmles[i].Title, parmeqsexpmles[i], parmeqsexpmles[i].Image));
            }


         modelslist.AddRange(new string [] {"earthmodel", "pokeman", "path","book","chair" });
            for (int i = 0; i < modelslist.Count; i++)
            {
             comboitem c=   new comboitem("", null,null);
                if (i == 0)
                {
                    c.Text = "Earth";
                    c.Image = EquationGrapher3D.Properties.Resources.earth;
                }
                else if (i == 1)
                {
                    c.Text = "PokeMan";
                    c.Image = EquationGrapher3D.Properties.Resources.pokeman;
                }
                else if (i==2)
                {
                    c.Text = "Path";
                    c.Image = EquationGrapher3D.Properties.Resources.path;
                }
                else if (i==3)
                {
                    c.Text = "Book";
                    c.Image = EquationGrapher3D.Properties.Resources.book;
                }
                else if (i==4)
                {
                    c.Text = "Chair";
                    c.Image = EquationGrapher3D.Properties.Resources.chair;
                }
                this.kcombobox1.Items.Add(c);
            }
            EquationFile xeq1 = new EquationFile();
            EquationFile xeq2 = new EquationFile();
            EquationFile xeq3 = new EquationFile();
            EquationFile xeq4 = new EquationFile();
            EquationFile xeq5 = new EquationFile();
            EquationFile xeq6 = new EquationFile();
            EquationFile xeq7 = new EquationFile();
            EquationFile xeq8 = new EquationFile();
            EquationFile xeq9 = new EquationFile();
            EquationFile xeq10 = new EquationFile();

            xeq1.Title = "nebula";
            xeq1.Expliciteqaution = "sin(10(x^2+y^2))/10";
            xeq1.DomainX = new PointF(-1, 1f);
            xeq1.DomainY = new PointF(-1, 1f);
            xeq1.Image = EquationGrapher3D.Properties.Resources.nebula;

            xeq2.Title = "hill in a valley";
            xeq2.Expliciteqaution = "(sqrt(x^2+y^2)-2)^2";
            xeq2.DomainX = new PointF(-3, 3f);
            xeq2.DomainY = new PointF(-3, 3f);
            xeq2.Image = EquationGrapher3D.Properties.Resources.hillinvally;

            xeq3.Title = "Stairs";
            xeq3.Expliciteqaution = "(sign(-.65-x)+sign(-.35-x)+sign(-.05-x)+sign(.25-x)+sign(.55-x))/7";
            xeq3.DomainX = new PointF(-1, 1f);
            xeq3.DomainY = new PointF(-1, 1f);
            xeq3.Image = EquationGrapher3D.Properties.Resources.Stairs;

            xeq4.Title = "low rolling plains";
            xeq4.Expliciteqaution = "sin(x)+sin(y)";
            xeq4.DomainX = new PointF(-10, 10f);
            xeq4.DomainY = new PointF(-10, 10f);
            xeq4.Stepu = 1;
            xeq4.Image = EquationGrapher3D.Properties.Resources.lowrollinglpans;

            xeq5.Title = "sewing snap";
            xeq5.Expliciteqaution = "cos(abs(x) + abs(y)) * (abs(x) + abs(y))/4";
            xeq5.DomainX = new PointF(-10, 10f);
            xeq5.DomainY = new PointF(-10, 10f);
            xeq5.Stepu = 1;
            xeq5.Image = EquationGrapher3D.Properties.Resources.sewingsnap;

            xeq6.Title = "blossoming flower";
            xeq6.Expliciteqaution = "abs((cos(x) + cos(y))) ^(1/2)";
            xeq6.DomainX = new PointF(-4, 4f);
            xeq6.DomainY = new PointF(-4, 4f);
            xeq6.Image = EquationGrapher3D.Properties.Resources.blomissngflower; ;

            xeq7.Title = "pair of mountains and valleys";
            xeq7.Expliciteqaution = "5*x*y*exp(-0.5*(x*x+y*y))";
            xeq7.DomainX = new PointF(-4, 4f);
            xeq7.DomainY = new PointF(-4, 4f);
            xeq7.Image = EquationGrapher3D.Properties.Resources.pairofmousnanesandvallyes;

            xeq8.Title = "volcano";
            xeq8.Expliciteqaution = "sqrt(1-(sqrt(x^2+y^2)-2)^2)";
            xeq8.DomainX = new PointF(-4, 4f);
            xeq8.DomainY = new PointF(-4, 4f);
            xeq8.Image = EquationGrapher3D.Properties.Resources.vlocano;

            xeq9.Title = "Pyramid";
            xeq9.Expliciteqaution = "1-abs(x+y)-abs(y-x)";
            xeq9.DomainX = new PointF(-3, 3f);
            xeq9.DomainY = new PointF(-3, 3f);
            xeq9.Image = EquationGrapher3D.Properties.Resources.pyramid;

            xeq10.Title = "Roof";
            xeq10.Expliciteqaution = "1-abs(y)";
            xeq10.DomainX = new PointF(-4, 4f);
            xeq10.DomainY = new PointF(-4, 4f);
            xeq10.Image = EquationGrapher3D.Properties.Resources.roof;

            EquationFile xeq11 = new EquationFile("Cone", "(x^2+y^2)^0.5", new PointF(-2, 2f), new PointF(-2, 2f));
            xeq11.Image = EquationGrapher3D.Properties.Resources.Cone1;

            EquationFile xeq12 = new EquationFile("Top Hat", "(sign(0.2-(x^2+y^2)) + sign(0.2-(x^2/3+y^2/3)))/3-1", new PointF(-1, 1f), new PointF(-1, 1f));
            xeq12.Image = EquationGrapher3D.Properties.Resources.Tophat;
            EquationFile xeq13 = new EquationFile("Intersecting Fences", "0.75/exp((x*5)^2*(y*5)^2)", new PointF(-1, 1f), new PointF(-1, 1f));
            xeq13.Image = EquationGrapher3D.Properties.Resources.intersectingfenes;
     

            EquationFile xeq15 = new EquationFile("Cross", ".1 - sign(sign((x*12)^2-9)-1 + sign((y*12)^2-9)-1)/2", new PointF(-1, 1f), new PointF(-1, 1f));
            xeq15.Image = EquationGrapher3D.Properties.Resources.cross;
            EquationFile xeq16 = new EquationFile("V", "sign(x-1+abs(y*2))/3 + sign(x-.5+abs(y*2))/3", new PointF(-1, 1f), new PointF(-1, 1f));
            xeq16.Image = EquationGrapher3D.Properties.Resources.v;

            EquationFile xeq17 = new EquationFile("O", "(-sign(0.2-(x^2+y^2)) + sign(0.2-(x^2/3+y^2/3)))/9", new PointF(-1, 1f), new PointF(-1, 1f));
            xeq17.Image = EquationGrapher3D.Properties.Resources.o;
            EquationFile xeq19 = new EquationFile("rush", "20*cos((x^2 +y^2)/4) / (x^2 +y^2 + pi)", new PointF(-5, 5f), new PointF(-5, 5f));
            xeq19.Image = EquationGrapher3D.Properties.Resources.rush;
            EquationFile xeq20 = new EquationFile("waver", "sqrt(x*x+y*y)+3*cos(sqrt(x*x+y*y))+5", new PointF(-10, 10f), new PointF(-10, 10f));
            xeq20.Image = EquationGrapher3D.Properties.Resources.Waver;

            EquationFile xeq21 = new EquationFile("Obsts", "sin(x^2+y^2)/(abs(x*y)+1)", new PointF(-2.5f, 2.5f), new PointF(-2.5f, 2.5f));
            xeq21.Image = EquationGrapher3D.Properties.Resources.obsts;

            EquationFile xeq22 = new EquationFile("Curve", "(cos(sqrt(((x+0)^2)+((y+0)^2))) + cos(sqrt(((x+.913*2pi)^2)+((y+0)^2))) + cos(sqrt(((x-.913*2pi)^2)+((y+0)^2))))*4", new PointF(-3, 3f), new PointF(-3, 3f));
            xeq22.Image = EquationGrapher3D.Properties.Resources.curve;


            EquationFile xeq23 = new EquationFile("Bond", "c1*sin((x^2+y^2-x*y)/c2)", new PointF(-10, 10f), new PointF(-10, 10f));
            xeq23.Image = EquationGrapher3D.Properties.Resources.Hat;

            xeq23.ExplicitHelpingvariblesequations = new string[]
          {
                "2",
                "16",
          };
            explicitexpmles.AddRange(new EquationFile[] { xeq1, xeq2, xeq3, xeq4, xeq5, xeq6, xeq7, xeq8, xeq9, xeq10, xeq11, xeq12, xeq13,xeq15, xeq16, xeq17, xeq19, xeq20, xeq21, xeq22,xeq23 });
            for (int i = 0; i < explicitexpmles.Count; i++)
            {
                explicitexpmles[i].IsExplicit = true;
                this.kcombobox_explict_examples.Items.Add(new comboitem(explicitexpmles[i].Title, explicitexpmles[i], explicitexpmles[i].Image));
            }
            this.kcombobox_explict_examples.SelectedIndex = 0; this.kcombobox_parm_empls.SelectedIndex = 0;
            this.kcombobox_dex.SelectedIndex = 0;

        }
        public void MakeDefultEquation()
        {
            this.kcombobox_parm_empls.SelectedIndex = 0;
            this.kcombobox_explict_examples.SelectedIndex = 0;
        }
        private EquationFile equationfilevalue;

        public EquationFile Equationfilevalue
        {
            get { return equationfilevalue; }
            set
            {
                equationfilevalue = value;

                LoadEquation(value);
                tabControl_eqs.SelectedIndex = value.IsExplicit == true ? 1 : 0;


            }
        }
        private void LoadEquation(EquationFile value)
        {

            equationfilevalue = value;



           
            if (value.IsExplicit)
            { clearconstants(true );
                addconstants(value.ExplicitHelpingvariblesequations,true);

                this.kscale_xdomain.ForcedSetValue(value.DomainX.X, 0); this.kscale_xdomain.ForcedSetValue(value.DomainX.Y, 1);
                this.kscale_ydomain.ForcedSetValue(value.DomainY.X, 0); this.kscale_ydomain.ForcedSetValue(value.DomainY.Y, 1);
                if (kscale_xdomain.Maxmum > 12.56f && kscale_xdomain.MaxValue <= 12.56f)
                {
                    kscale_xdomain.Maxmum = 12.56f;
                }
                if (kscale_xdomain.Minimum < -12.56f && kscale_xdomain.MinValue >= -12.56f)
                {
                    kscale_xdomain.Minimum = -12.56f;
                }
                if (kscale_ydomain.Maxmum > 12.56f && kscale_ydomain.MaxValue <= 12.56f)
                {
                    kscale_ydomain.Maxmum = 12.56f;
                }
                if (kscale_ydomain.Minimum < -12.56f && kscale_ydomain.MinValue >= -12.56f)
                {
                    kscale_ydomain.Minimum = -12.56f;
                }
                this.kscale_xdomain.makepointerlocation(); this.kscale_ydomain.makepointerlocation();
                this.kscale_xdomain.Invalidate(); this.kscale_ydomain.Invalidate();
                this.textBox_explicit.Text = value.Expliciteqaution;
                if (value.ExplicitHelpingvariblesequations.Length == 0)
                {
                    addconstant("1", true );
                }
            }
            else
            {
                clearconstants(false );
                addconstants(value.ParamHelpingvariblesequations,false);

                if (value.ParamHelpingvariblesequations.Length==0)
            {
                addconstant("1",false);
            }
                this.kscale_udomain.ForcedSetValue(value.DomainU.X, 0); this.kscale_udomain.ForcedSetValue(value.DomainU.Y, 1);
                this.kscale_vdomain.ForcedSetValue(value.DomainV.X, 0); this.kscale_vdomain.ForcedSetValue(value.DomainV.Y, 1);
                if (kscale_udomain.Maxmum > 12.56f && kscale_udomain.MaxValue <= 12.56f)
                {
                    kscale_udomain.Maxmum = 12.56f;
                }
                if (kscale_udomain.Minimum < -12.56f && kscale_udomain.MinValue >= -12.56f)
                {
                    kscale_udomain.Minimum = -12.56f;
                }
                if (kscale_vdomain.Maxmum > 12.56f && kscale_vdomain.MaxValue <= 12.56f)
                {
                    kscale_vdomain.Maxmum = 12.56f;
                }
                if (kscale_vdomain.Minimum < -12.56f && kscale_vdomain.MinValue >= -12.56f)
                {
                    kscale_vdomain.Minimum = -12.56f;
                }
                this.kscale_udomain.makepointerlocation(); this.kscale_vdomain.makepointerlocation();
                this.kscale_udomain.Invalidate(); this.kscale_vdomain.Invalidate();

                this.textbox_x.Text = value.ParamX;
                this.textbox_y.Text = value.ParamY;
                this.textbox_z.Text = value.ParamZ;

            }

    
            
            float quality = 50;
            if (value.AutoDetectStep)
            {
                if (value.IsExplicit)
                {
                    float lenghtx = value.DomainX.Y - value.DomainX.X;
                    float idealstepx = lenghtx / quality;
                    if (idealstepx == 0) { idealstepx = 0.01f; }
                    kscale_esx.ForcedSetValue(idealstepx);
                    float lenghty = value.DomainY.Y - value.DomainY.X;
                    float idealstepy = lenghty / quality;
                    if (idealstepy == 0) { idealstepy = 0.01f; }
                    kscale_esy.ForcedSetValue(idealstepy);
                }
                else
                {
                    float lenghtu = value.DomainU.Y - value.DomainU.X;
                    float idealstepu = lenghtu / quality;
                    if (idealstepu == 0) { idealstepu = 0.01f; }
                    kscale_psu.ForcedSetValue(idealstepu);
                    float lenghtv = value.DomainV.Y - value.DomainV.X;
                    float idealstepv = lenghtv / quality;
                    if (idealstepv == 0) { idealstepv = 0.01f; }
                    kscale_psv.ForcedSetValue(idealstepv);
                }
            }
            else
            {
                if (value.IsExplicit)
                {                  
                    kscale_esx.ForcedSetValue(value.Stepu);                   
                    kscale_esy.ForcedSetValue(value.Stepv);
                }
                else
                {                  
                    kscale_psu.ForcedSetValue(value.Stepu);                  
                    kscale_psv.ForcedSetValue(value.Stepv);
                }
            }
        }
        private void LoadDesigenEquation(EquationFile value)
        {


            this.kscale_dvdomain.ForcedSetValue(value.DomainV.X, 0); this.kscale_dvdomain.ForcedSetValue(value.DomainV.Y, 1);
            if (kscale_dvdomain.Maxmum > 12.56f && kscale_dvdomain.MaxValue <= 12.56f)
                {
                    kscale_dvdomain.Maxmum = 12.56f;
                }
            if (kscale_dvdomain.Minimum < -12.56f && kscale_dvdomain.MinValue >= -12.56f)
                {
                    kscale_dvdomain.Minimum = -12.56f;
                }
            this.kscale_dvdomain.makepointerlocation(); this.kscale_dvdomain.Invalidate();
               

                this.rtb_dx.Text = value.ParamX;
                this.rtb_dy.Text = value.ParamY;
                this.rtb_dz.Text = value.ParamZ;

            float quality = 50;
            if (value.AutoDetectStep)
            {                                  
                    float lenghtv = value.DomainV.Y - value.DomainV.X;
                    float idealstepv = lenghtv / quality;
                    if (idealstepv == 0) { idealstepv = 0.01f; }
                    kscale_dvstep.ForcedSetValue(idealstepv);             
            }
            else
            {                                
                    kscale_psv.ForcedSetValue(value.Stepv);             
            }
        }

    

        public bool isExplicit() { bool value = tabControl_eqs.SelectedIndex == 1 ? true : false; equationfilevalue.IsExplicit = value; return value; }
        public bool updateequationfile()
        {
           
            PointF udomain = new PointF((float)Math.Min(kscale_udomain.Pointers[0].ValueF, kscale_udomain.Pointers[1].ValueF), (float)Math.Max(kscale_udomain.Pointers[0].ValueF, kscale_udomain.Pointers[1].ValueF));
            PointF vdomain = new PointF((float)Math.Min(kscale_vdomain.Pointers[0].ValueF, kscale_vdomain.Pointers[1].ValueF), (float)Math.Max(kscale_vdomain.Pointers[0].ValueF, kscale_vdomain.Pointers[1].ValueF));
            PointF xdomain = new PointF((float)Math.Min(kscale_xdomain.Pointers[0].ValueF, kscale_xdomain.Pointers[1].ValueF), (float)Math.Max(kscale_xdomain.Pointers[0].ValueF, kscale_xdomain.Pointers[1].ValueF));
            PointF ydomain = new PointF((float)Math.Min(kscale_ydomain.Pointers[0].ValueF, kscale_ydomain.Pointers[1].ValueF), (float)Math.Max(kscale_ydomain.Pointers[0].ValueF, kscale_ydomain.Pointers[1].ValueF));

            equationfilevalue = new EquationFile();
            equationfilevalue.DomainU = udomain;
            equationfilevalue.DomainV = vdomain;
            equationfilevalue.DomainX = xdomain;
            equationfilevalue.DomainY = ydomain;
            equationfilevalue.Expliciteqaution = textBox_explicit.Text;
            equationfilevalue.ParamX = textbox_x.Text;
            equationfilevalue.ParamY = textbox_y.Text;
            equationfilevalue.ParamZ = textbox_z.Text;
            equationfilevalue.IsExplicit = tabControl_eqs.SelectedIndex == 1 ? true : false;
            equationfilevalue.Stepu = tabControl_eqs.SelectedIndex == 0 ? kscale_psu.ValueF : kscale_esx.ValueF;
            equationfilevalue.Stepv = tabControl_eqs.SelectedIndex == 0 ? kscale_psv.ValueF : kscale_esy.ValueF;
          

            List<RichTextBox> tbs = new List<RichTextBox>(flowLayoutPanel1.Controls.OfType<RichTextBox>());
            List<string> helpingvarsequations = new List<string>();

            for (int i = 0; i < tbs.Count; i++)
            {
                if (tbs[i].Text.Trim() != "")
                {
                    helpingvarsequations.Add(tbs[i].Text);
                }
                else { helpingvarsequations.Add("1"); }
            }
            equationfilevalue.ParamHelpingvariblesequations = helpingvarsequations.ToArray();


            List<RichTextBox> tbs2 = new List<RichTextBox>(flowLayoutPanel2.Controls.OfType<RichTextBox>());
            List<string> helpingvarsequations2 = new List<string>();

            for (int i = 0; i < tbs2.Count; i++)
            {
                if (tbs2[i].Text.Trim() != "")
                {
                    helpingvarsequations2.Add(tbs2[i].Text);
                }
                else { helpingvarsequations2.Add("1"); }
            }
            equationfilevalue.ExplicitHelpingvariblesequations = helpingvarsequations2.ToArray();

            
            return false;
         
                  
       
        }
        public EquationFile GetDesigenEquationFile()
        { 
            EquationFile value = new EquationFile();
            value.DomainV = VDomain_of_Desigen_ZEquation;
             value.ParamX = X_equation_of_Desigen;
             value.ParamY = Y_equation_of_Desigen;
            value.ParamZ = Z_equation_of_Desigen;
            value.IsExplicit = false;
            value.Stepv = kscale_dvstep.ValueF;
            return value;
        }
      
        private void kgriditem1_Click(object sender, EventArgs e)
        {

            if (updateequationfile())
            {

                this.DialogResult = DialogResult.OK;
            }
        }

        private void kgriditem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxx_TextChanged(object sender, EventArgs e)
        {


        }

        private void kscale1_ValueChanged(object sender, Kscaleeventargs e)
        {
        }

        private void kscalegradient_ValueChanged(object sender, Kscaleeventargs e)
        {
        }
       
      
        public string ModelAddress
        { get { return textBox1.Text; }set { textBox1.Text = value; } }
       
        public int Innermodelindex
        { get { return (int)numericUpDown1.Value; } set { numericUpDown1.Value = value ; } }

        public string Text_to_generate
        { get
            {
                string value = "";
                this.Invoke(new Action(() => { value = textBox2.Text; }));
                return value;
            } set { textBox2.Text = value; } }

        public string Z_equation_of_Text_to_generate
        { get
            {
                string value = "";
                this.Invoke(new Action(() => { value = richTextBox4.Text; }));
                return value;           
            }

            set {richTextBox4.Text = value; } }

        public string Z_equation_of_Desigen
        {
            get
            {
                string value = "";
                this.Invoke(new Action(() => { value = rtb_dz.Text; }));
                return value;
            }

            set { rtb_dz.Text = value; }
        }
        public string Y_equation_of_Desigen
        {
            get
            {
                string value = "";
                this.Invoke(new Action(() => { value = rtb_dy.Text; }));
                return value;
            }

            set { rtb_dy.Text = value; }
        }
        public string X_equation_of_Desigen
        {
            get
            {
                string value = "";
                this.Invoke(new Action(() => { value = rtb_dx.Text; }));
                return value;
            }

            set { rtb_dx.Text = value; }
        }
        public bool Desigen_generatefromline
        {
            get
            {
                bool value = false;
                this.Invoke(new Action(() => { value = radioButton4.Checked; }));
                return value;
            }

            set { radioButton4.Checked=value; }
        }
        public bool Desigen_back_closed_face
        {
            get
            {
                bool value = false;
                this.Invoke(new Action(() => { value = this.checkBox_back.Checked; }));
                return value;
            }

            set { checkBox_back.Checked = value; }
        }
        public bool Desigen_front_closed_face
        {
            get
            {
                bool value = false;
                this.Invoke(new Action(() => { value = this.checkBox_front.Checked; }));
                return value;
            }

            set { checkBox_front.Checked = value; }
        }
        public PointF VDomain_of_Desigen_ZEquation
        {
            get
            {
                PointF value = new PointF();
                this.Invoke(new Action(() => { value = new PointF(this.kscale_dvdomain.MinValue, kscale_dvdomain.MaxValue); }));
                return value;
            }

            set { kscale_dvdomain.Pointers[0].ValueF = value.X; kscale_dvdomain.Pointers[1].ValueF = value.Y; }
        }
        public float VStep_of_Desigen_ZEquation
        {
            get
            {
                float value = 0;
                this.Invoke(new Action(() => { value = kscale_dvstep.ValueF; }));
                return value;
            }
            set { kscale_dvstep.ValueF = value; }
        }
        public Font Font_of_text_to_generate
        { get
            {
                Font value = null ;
                this.Invoke(new Action(() => { value = textBox2.Font; }));
                return value; ;
            } set { textBox2.Font = value; } }
  
       public static string res = "resources#";
        public Models3Dlist GetLoadModel()
        {
            string s = textBox1.Text;
            Models3Dlist mdls = null;
            if (s.ToLower().StartsWith(res))
            {
                if (s.ToLower() ==  (res+"electorns_super_ball_levels").ToLower())
                {
                 
                    mdls = Getmodelfromresources("electorns_super_ball_levels");
                }else if (s.ToLower()==(res+"f16").ToLower())
                {

                    mdls = Getmodelfromresources("f16");

                }
                else
                {
                    int indx = int.Parse(s.Substring(res.Length, s.Length - res.Length));

                    if (indx < modelslist.Count)
                    {
                   //     try
                   //     {
                            mdls = Getmodelfromresources(modelslist[indx]);
                         
                    //    }
                     //   catch { return null; }

                    }
                    else
                    {
                        mdls = null;
                    }

                }

            }
            else
            {
                    mdls = Models3Dlist.FromFile(s);
              
            }
            for (int i=0;i<mdls.Models.Count;i++)
            {
                if (mdls.Models[i]!=null)
                {
                    mdls.Models[i].IsVisible = true;
                }
            }
            return mdls;
        }
        public void LoadModel(string s, int innerindx,Models3Dlist mdl=null)
        {
            ModelAddress = s; Innermodelindex = innerindx;
            Models3Dlist mdls = null;
            if (mdl == null)
            {
           mdls  = GetLoadModel();
            }
            else
            {
                mdls = mdl;
            }
            if (mdls != null)
            {
                label18.Text = mdls.Models.Count.ToString();
                numericUpDown1.Maximum = mdls.Models.Count - 1;
            }
            Genetarion_Type = GenerationType.Model;
     
          
        }
   
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        private void kgriditem1_Click_1(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = textbox_x.Text;
           
            Control c = sender as Control;
            Control contaner = this.isExplicit() ? flowLayoutPanel2 : flowLayoutPanel1;
            List<kgriditem> clcs = new List<kgriditem>(contaner.Controls.OfType<kgriditem>());
            List<RichTextBox> tbs = new List<RichTextBox>(contaner.Controls.OfType<RichTextBox>());
            int indx = clcs.Count;
         
            string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };
            List<string> helpmarks = new List<string>();
            helpmarks.AddRange(mainmarks);
            for (int i = 0; i < indx; i++)
            {
                helpmarks.Add("c" + (i + 1).ToString());
            }
            cf.Vars = helpmarks.ToArray();
          
          if (cf.ShowDialog() == DialogResult.OK)
            {
                textbox_x.Text = cf.equationvalue;
            }
        }

        private void kgriditem2_Click_1(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = textbox_y.Text;

            Control c = sender as Control;
            Control contaner = this.isExplicit() ? flowLayoutPanel2 : flowLayoutPanel1;
            List<kgriditem> clcs = new List<kgriditem>(contaner.Controls.OfType<kgriditem>());
            List<RichTextBox> tbs = new List<RichTextBox>(contaner.Controls.OfType<RichTextBox>());
            int indx = clcs.Count;

            string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };
            List<string> helpmarks = new List<string>();
            helpmarks.AddRange(mainmarks);
            for (int i = 0; i < indx; i++)
            {
                helpmarks.Add("c" + (i + 1).ToString());
            }
            cf.Vars = helpmarks.ToArray();

            if (cf.ShowDialog() == DialogResult.OK)
            {
                textbox_y.Text = cf.equationvalue;
            }


        }

        private void kgriditem4_Click_1(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = textbox_z.Text;

            Control c = sender as Control;
            Control contaner = this.isExplicit() ? flowLayoutPanel2 : flowLayoutPanel1;
            List<kgriditem> clcs = new List<kgriditem>(contaner.Controls.OfType<kgriditem>());
            List<RichTextBox> tbs = new List<RichTextBox>(contaner.Controls.OfType<RichTextBox>());
            int indx = clcs.Count;

            string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };
            List<string> helpmarks = new List<string>();
            helpmarks.AddRange(mainmarks);
            for (int i = 0; i < indx; i++)
            {
                helpmarks.Add("c" + (i + 1).ToString());
            }
            cf.Vars = helpmarks.ToArray();

            if (cf.ShowDialog() == DialogResult.OK)
            {
                textbox_z.Text = cf.equationvalue;
            }
        }

        private void kgriditem5_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = textBox_explicit.Text;

            Control c = sender as Control;
            Control contaner = this.isExplicit() ? flowLayoutPanel2 : flowLayoutPanel1;
            List<kgriditem> clcs = new List<kgriditem>(contaner.Controls.OfType<kgriditem>());
            List<RichTextBox> tbs = new List<RichTextBox>(contaner.Controls.OfType<RichTextBox>());
            int indx = clcs.Count;

            string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };
            List<string> helpmarks = new List<string>();
            helpmarks.AddRange(mainmarks);
            for (int i = 0; i < indx; i++)
            {
                helpmarks.Add("c" + (i + 1).ToString());
            }
            cf.Vars = helpmarks.ToArray();

            if (cf.ShowDialog() == DialogResult.OK)
            {
                textBox_explicit.Text = cf.equationvalue;
            }
        }

        private void tabControl2_Enter(object sender, EventArgs e)
        {
            Control t = sender as Control;
            if (t.BackColor != Color.Pink)
            {
                t.BackColor = Color.Yellow;
            }
        }

        private void textBox_X_Leave(object sender, EventArgs e)
        {
            Control t = sender as Control;
            if (t.BackColor != Color.Pink)
            {
                t.BackColor = Color.White;
            }
        }

        private void numeric_ps_ValueChanged(object sender, EventArgs e)
        {
            float ps = kscale_psu.ValueF;
            float v = (float)((NumericUpDown)sender).Value;
            if (ps != v && kscale_psu.IsOnValueChanging == false)
            {

                if (v <= kscale_psu.Maxmum && v >= kscale_psu.Minimum)
                {
                    kscale_psu.ValueF = v;
                }
                kscale_psu.makepointerlocation();
            }
        }

        private void kscale_ps_ValueChanging(object sender, Kscaleeventargs e)
        {


        }

        public bool ismodelvisible { get { return kscale_draw_model.Value == 0 ? false : true; } }

        private void kscale_es_ValueChanging(object sender, Kscaleeventargs e)
        {
        }
        private void kDynamkPanel1_openingclosing(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => this.Size = new Size(kDynamkPanel1.Width, kDynamkPanel1.Height)));
        }

        private void kDynamkPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void neweq_TextChanged(object sender, EventArgs e)
        {
            kDynamkPanel1.Title = this.Text;
        }

        private void neweq_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void kcombobox_parm_empls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcombobox_parm_empls.SelectedIndex >= 0)
            {
                EquationFile value = (EquationFile)((comboitem)((kcombobox)sender).SelectedItem).Tag;

                LoadEquation(value);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {









        }
        private void kcombobox_explict_examples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcombobox_explict_examples.SelectedIndex >= 0)
            {
                EquationFile value = (EquationFile)((comboitem)((kcombobox)sender).SelectedItem).Tag;
                LoadEquation(value);
            }
        }

        private void textBox_X_TextChanged_1(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            testrichtextbox_text(t, -1);
            toolTip1.SetToolTip(t, t.Text);
        }

        private void textBox_explicit_TextChanged(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            testrichtextbox_text(t, -1);
            toolTip1.SetToolTip(t, t.Text);
        }
        public event EventHandler draw;
        private void kg_redraw_Click(object sender, EventArgs e)
        {
            if (draw != null)
            { draw(sender, e); }
        }
        public event EventHandler ismodelvisible_changed;
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void kgriditem3_Click_1(object sender, EventArgs e)
        {

        }

        private void kDynamkPanel2_openingclosing(object sender, EventArgs e)
        {
            int itmsz = 0; Point gf ;
           this.Invoke(new Action(()=> {
               itmsz = tabControl_main.ItemSize.Height + tabControl_main.Location.Y + tabControl_eqs.Location.Y + tabControl_eqs.ItemSize.Height;    
           }));
           
            int height = kDynamkPanel_parm.Height + kDynamkPanel_parm.Location.Y +12+ itmsz;
            this.Invoke(new Action(() => this.Size = new Size(kDynamkPanel1.Width, height)));
            this.kDynamkPanel1.Invoke(new Action(() => this.kDynamkPanel1.Size = new Size(kDynamkPanel1.Width, height)));
        }

        private void kgriditem6_Click(object sender, EventArgs e)
        {
            this.updateequationfile();
            Control c = sender as Control;
            Control contaner = this.isExplicit() ? flowLayoutPanel2 : flowLayoutPanel1;
            List<kgriditem > clcs = new List<kgriditem >(contaner.Controls.OfType<kgriditem >());    
            List<RichTextBox> tbs = new List<RichTextBox>(contaner.Controls.OfType<RichTextBox>());
            int indx = clcs.IndexOf(c as kgriditem);
            CalculatorForm cf = new CalculatorForm();
            string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };
            List<string> helpmarks = new List<string>();
            helpmarks.AddRange(mainmarks);
            for (int i = 0; i < indx; i++)
            {
                helpmarks.Add("c" + (i + 1).ToString());
            }
            cf.Vars = helpmarks.ToArray();
            cf.equationvalue = tbs[indx].Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                tbs[indx].Text = cf.equationvalue;
            }
        }
        void testrichtextbox_text(RichTextBox rtb, int helpmarkindex, bool reclolor = true)
        { 
            updateequationfile();
            List<string> allmarks = new List<string>();
            List<float> allvalues = new List<float>(new float[2] {1 , 1}) ;

            if (Genetarion_Type == GenerationType.Text)
            { allmarks.Add("v");allvalues.Add(1); allmarks.Add("u"); allvalues.Add(1); }
            else if (Genetarion_Type==GenerationType.Desigen)
            {
                allmarks.AddRange(new string[] { "x", "y", "v" }); allvalues.AddRange(new float[] { 1, 1, 1 });
            }
            else
            {
                if (helpmarkindex == -1)
                {
                    allmarks.AddRange(equationfilevalue.GetAllSymbols());
                    for (int s = 0; s < equationfilevalue.ParamHelpingvariblesequations.Length; s++)
                    {
                        allvalues.Add(1);
                    }

                }
                else
                {
                    string[] mainmarks = this.isExplicit() == true ? new string[2] { "x", "y" } : new string[2] { "u", "v" };


                    allmarks.AddRange(mainmarks);
                    for (int i = 0; i < helpmarkindex; i++)
                    {
                        allmarks.Add("c" + (i + 1).ToString()); allvalues.Add(0);
                    }
                }
            }
            if (reclolor)
            {
                fnc.ColorMathEquation(rtb, allmarks.ToArray());
            }
          
            bool isright = true;
            try
            {
                EquationSolver.SolveEQ(rtb.Text, allmarks.ToArray(), allvalues.ToArray(), false);
                isright = true;
            }
            catch (MathException me)
            {
                isright = true;
            }
            catch (Exception ex)
            {
                isright = false;
                if (helpmarkindex != -1 && rtb.Text.Trim()=="")
                { isright = true; }
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
            }else
            {
            rtb.BackColor = Color.Pink;
            }
        }
        public kDynamkPanel DynamkPanel() { return kDynamkPanel1; }
        public kDynamkPanel HelpingVarsPanel() { return kDynamkPanel_parm; }
        private void rtb_c1_TextChanged(object sender, EventArgs e)
        {
            updateequationfile();
            Control c = this.isExplicit() == true ? flowLayoutPanel2 : flowLayoutPanel1;

            List<RichTextBox> tbs = new List<RichTextBox>(c.Controls.OfType<RichTextBox>());
            List<kgriditem> clcs = new List<kgriditem >(c.Controls.OfType<kgriditem >());

            RichTextBox rtb = sender as RichTextBox;
            int indx = tbs.IndexOf(rtb);
            testrichtextbox_text(rtb,  indx);
            for (int i = 0; i < tbs.Count; i++)
            {
                if (tbs[i].Text != "")
                {
                    if (i+1< tbs.Count)
                    {
                        tbs[i + 1].Enabled = true;
                        clcs[i + 1].Enabled = true;
                    }
                   
                }
                else
                    {
                        for (int s = i+1; s < tbs.Count; s++)
                        { tbs[s].Enabled = false;
                         clcs[s].Enabled = false;
}
                        break;
                    }
            }
            toolTip1.SetToolTip(rtb, rtb.Text);
        }
        private void clearconstants(bool isxplt)
        {
            if (isxplt) { flowLayoutPanel2.Controls.Clear(); } else { flowLayoutPanel1.Controls.Clear(); }   
        }
        private void removeconstant(bool isxplt)
        {
            Control c = isxplt == true ? flowLayoutPanel2 : flowLayoutPanel1;
            if (c.Controls.Count > 3)
            { c.Controls.RemoveAt(c.Controls.Count - 3);
                 c.Controls.RemoveAt(c.Controls.Count - 2); 
                c.Controls.RemoveAt(c.Controls.Count - 1);
              
               
            }
        }
        private void addconstants(string[] texts,bool isxplct)
        {
        foreach(string s in texts)
        { addconstant(s,isxplct); }
        }
        private void addconstant(string text,bool isxplct)
        {
            Control c = isxplct ==true ? flowLayoutPanel2 : flowLayoutPanel1;

            List<RichTextBox> tbs = new List<RichTextBox>(c.Controls.OfType<RichTextBox>());
            int indx = tbs.Count + 1;
            RichTextBox rtb = new RichTextBox();
            Label lbl = new Label();
            kgriditem kgi = new kgriditem();
         
            lbl.AutoSize = false;  
            lbl.Size = new System.Drawing.Size(26, 28);              
            lbl.Text = "C"+indx;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            rtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
         | System.Windows.Forms.AnchorStyles.Right)));
            rtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            rtb.Enabled = false;
            rtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            rtb.DetectUrls = false;
            rtb.WordWrap = false;
            rtb.TextChanged += new System.EventHandler(this.rtb_c1_TextChanged);
            rtb.Enter += new System.EventHandler(this.tabControl2_Enter);
            rtb.Leave += new System.EventHandler(this.textBox_X_Leave);
          

            kgi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            kgi.BackColor = System.Drawing.Color.White;
            kgi.Borderwidth = 2;
            kgi.Centerimage =EquationGrapher3D.Properties.Resources.calculator_math_icon ;
            kgi.CenterImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            kgi.ColorAnimationEffect = false;
            kgi.FillPercent = 0F;
            kgi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            kgi.ForeColor = System.Drawing.Color.Black;
            kgi.ImgEffect = SolveText.kgriditem.ImgEffectss.none;
            kgi.Overcolor1 = System.Drawing.Color.Lime;
            kgi.Overcolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            kgi.Paintstyle = SolveText.KitemPaintStyle.Flat;
            kgi.SecondColor = System.Drawing.Color.Teal;
            kgi.Selectable = false;
            kgi.SelectedColor = System.Drawing.Color.Teal;
            kgi.SelectedColor2 = System.Drawing.Color.PowderBlue;
            kgi.ShowSubPanel = false;
            kgi.ShowText = false;
            kgi.Size = new System.Drawing.Size(22, 25);                              
            kgi.Theme = SolveText.KitemThemes.none;
            kgi.Click += new System.EventHandler(this.kgriditem6_Click);
            kgi.Enabled = false;
            if (indx==1)
            { kgi.Enabled = true;
            rtb.Enabled = true;
            }
            rtb.ContextMenuStrip = textcontext.make();

  rtb.Size = new System.Drawing.Size(c.Width-label1.Width-kgi.Width-5, 25);
       
            c.Controls.Add(lbl);
           c.Controls.Add(rtb);
            c.Controls.Add(kgi);
 
            rtb.Text = text;
        }
 

        private void kgriditem35_Click(object sender, EventArgs e)
        {
            addconstant("1",false);
        }

        private void kgriditem15_Click(object sender, EventArgs e)
        {
            removeconstant(false );
        }

       
        public enum GenerationType{equation,Model,Text,Desigen};
  
        public GenerationType Genetarion_Type
        { get
            {
                GenerationType value=GenerationType.equation;
                this.Invoke(new Action(() => {value = (GenerationType)tabControl_main.SelectedIndex; }));
                return value;
            }
            set
            {
                tabControl_main.SelectedIndex = (int)value;
            }

        }
 
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl_main.SelectedIndex == 1)
            {
                kDynamkPanel1.Height = panel1.Height + tabControl_main.Location.Y + tabControl_main.ItemSize.Height + 10 + panel1.Location.Y;
                //dont replcae the loaded model if it was loaded
                if (textBox1.Text == "")
                {
                    kcombobox1.SelectedIndex = 1;
                }
            }
            else if (tabControl_main.SelectedIndex == 0)
            {
                kDynamkPanel1.Height = tabControl_main.Location.Y + tabControl_main.ItemSize.Height + tabControl_eqs.Height + 10;

            }
            else if (tabControl_main.SelectedIndex == 2)
            {
                kDynamkPanel1.Height = panel2.Height + tabControl_main.Location.Y + tabControl_main.ItemSize.Height + 10 + panel2.Location.Y;
             
            }
            this.Height = kDynamkPanel1.Height;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dv = new OpenFileDialog();
            dv.Filter = "Models file(.mdls)|*.mdls";
            if (dv.ShowDialog() == DialogResult.OK)
            {
                LoadModel(dv.FileName, 0);     
            }
        }

        private void kgriditem9_Click(object sender, EventArgs e)
        {
            addconstant("1", true);
        }

        private void kgriditem3_Click(object sender, EventArgs e)
        {
            removeconstant(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FontDialog fg = new FontDialog();
            if (fg.ShowDialog()==DialogResult.OK)
            {
                textBox2.Font = fg.Font;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
             RichTextBox t = sender as RichTextBox;
            testrichtextbox_text(t, -1);
        }

        private void kgriditem13_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.Vars = new string[] {  "v" };
            cf.equationvalue = richTextBox4.Text;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                richTextBox4.Text = cf.equationvalue;
            }
        }

        private void kcombobox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Models3Dlist value = (Models3Dlist)((comboitem)((kcombobox)sender).SelectedItem).Tag;
            LoadModel(res+kcombobox1.SelectedIndex.ToString(),0);
        }

        private void kgriditem14_Click(object sender, EventArgs e)
        {
            removeconstant(true);
        }

        private void kDynamkPanel3_openingclosing(object sender, EventArgs e)
        {
            int itmsz = 0; 
            this.Invoke(new Action(() => {
                itmsz = tabControl_main.ItemSize.Height + tabControl_main.Location.Y + tabControl_eqs.Location.Y + tabControl_eqs.ItemSize.Height;
            }));

            int height = kDynamkPanel_explicit.Height + kDynamkPanel_explicit.Location.Y + 12 + itmsz;
            this.Invoke(new Action(() => this.Size = new Size(kDynamkPanel1.Width, height)));
            this.kDynamkPanel1.Invoke(new Action(() => this.kDynamkPanel1.Size = new Size(kDynamkPanel1.Width, height)));

        }

        private void kscale_draw_model_ValueChanged(object sender, Kscaleeventargs e)
        {
            if (ismodelvisible_changed!=null)
            {
                ismodelvisible_changed(sender, e);
            }
        }
        bool ismousedown = false;
        bool ismousemove = false;
        bool isondrawing = false;
        PointF downpoint = new PointF();
        int crnt_colsed_face_vector_indx = -1;
        private void pic_desigen_MouseDown(object sender, MouseEventArgs e)
        {
            isondrawing = true;
            ismousedown = true;
            downpoint = e.Location;
            if (isonfacedefining == false)
            {
                desigenpoints.Add(e.Location);
            }
            else
            {
                for (int i = 0; i < desigenpoints.Count; i++)
                {
                    PointF p = desigenpoints[i];

                    if (p.X + 3 > downpoint.X && p.X - 3 < downpoint.X)
                    {
                        if (p.Y + 3 > downpoint.Y && p.Y - 3 < downpoint.Y)
                        {                         
                            crnt_colsed_face_vector_indx += 1; 
                            label23.Text =crnt_colsed_face_vector_indx+1+ " Points picked of 4";
                                    
                            int[] dd = desigenclosedfaces[desigenclosedfaces.Count - 1];
                            dd[crnt_colsed_face_vector_indx] = i;
                            desigenclosedfaces[desigenclosedfaces.Count - 1] =dd;
                           if (crnt_colsed_face_vector_indx==3)
                          {
                              button5.PerformClick();
                          }
                           break;
                          
                        }
                    }
                }
            }
        }
        
        public List<PointF> DesigenPoints
        {
            get { return desigenpoints; }
        }
        public List<int[]> DesigenClosedFaces
        {
            get { return desigenclosedfaces; }
        }
        List<PointF> desigenpoints = new List<PointF>();
        List<int[]> desigenclosedfaces = new List<int[]>();
      
        private void pic_desigen_MouseMove(object sender, MouseEventArgs e)
        {
            updatdrawing(e);
        }
        private void updatdrawing(MouseEventArgs e)
        {
            bool freedraw = radioButton1.Checked;
            bool drawlines = checkBox1.Checked ;
            bool drawcurves = checkBox2.Checked;
          
            ismousemove = true;
            PointF crntpoint = e.Location;
            Bitmap b = new Bitmap(pic_desigen.Width, pic_desigen.Height);
            Graphics g = Graphics.FromImage(b);

            if (freedraw  && isondrawing && ismousedown)
             {  desigenpoints.Add(crntpoint);}
          
             
            List<PointF> pnts = new List<PointF>(desigenpoints);
            if (isondrawing&&freedraw==false&&isonfacedefining==false)
            {pnts.Add(crntpoint);}

            for (int i = 0; i < desigenclosedfaces.Count;i++)
          {
              int[] indxs = desigenclosedfaces[i];
              if (indxs[0]==-1||indxs[1]==-1||indxs[2]==-1||indxs[3]==-1)
              {
                  continue;
              }
              PointF[] facepnts = new PointF[4] { desigenpoints[indxs[0]], desigenpoints[indxs[1]], desigenpoints[indxs[2]], desigenpoints[indxs[3]] };
              g.FillPolygon(new SolidBrush(Color.GreenYellow), facepnts);
              PointF[] closedfacepnts = new PointF[5]{facepnts[0],facepnts[1],facepnts[2],facepnts[3],facepnts[0]};
              g.DrawLines(new Pen(Color.Brown, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, closedfacepnts);
          }

          if (pnts.Count >= 2)
          {
          if (drawlines){ g.DrawLines(new Pen(Color.Black, 2), pnts.ToArray());}
          if (drawcurves){g.DrawCurve(new Pen(Color.Blue, 2), pnts.ToArray());}     
          }
          Color Pointercolor = Color.Green;
         
              for (int i = 0; i < pnts.Count; i++)
              {
                  PointF p = pnts[i];
                  float pntsz = freedraw == true ? 2f : 2f;
                      Color c = Color.Red;             
        if (isonfacedefining)
                  {
                      if (p.X+3>crntpoint.X&&p.X-3<crntpoint.X)
                      {
                          if (p.Y + 3 > crntpoint.Y && p.Y - 3 < crntpoint.Y)
                          {
                              pntsz = 4;
                              c = Color.Yellow;
                              Pointercolor = Color.Yellow;
                              g.DrawEllipse(new Pen(Color.Red), p.X - 9, p.Y - 9,9 * 2,9 * 2);
        
                          }
                      }
                      
                          if (desigenclosedfaces.Count>0)
                          {
                              int[] idxs = desigenclosedfaces[desigenclosedfaces.Count - 1];
                              for (int n=0;n<crnt_colsed_face_vector_indx+1;n++)
                              {
                                 if (idxs[n]==i)
                                 { c = Color.Green; pntsz = 4.5f;
                                 g.DrawEllipse(new Pen(Color.Red), p.X - 7, p.Y - 7, 7 * 2, 7 * 2);
        
                                 }
                              }
                          }
                      }
                 
               
                  g.FillEllipse(new SolidBrush(c), p.X - pntsz, p.Y - pntsz, pntsz * 2, pntsz * 2);
              }
          
          
          
            g.DrawLine(new Pen(Pointercolor, 2), crntpoint.X - 10, crntpoint.Y, crntpoint.X + 10, crntpoint.Y);
          g.DrawLine(new Pen(Pointercolor, 2), crntpoint.X, crntpoint.Y - 10, crntpoint.X, crntpoint.Y + 10);
       
            pic_desigen.BackgroundImage = b;
        
          pic_desigen.Refresh();
        }
        private void pic_desigen_MouseUp(object sender, MouseEventArgs e)
        {
            ismousedown = false;
         }

        private void button3_Click(object sender, EventArgs e)
        {
            isondrawing = false;
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left,0,0,0,0));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            desigenpoints.Clear(); crnt_colsed_face_vector_indx =-1;
            desigenclosedfaces.Clear();
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));
        
        }

        private void kscale_curve_ValueChanging(object sender, Kscaleeventargs e)
        {
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));
        
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void kcombobo_dex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcombobox_dex.SelectedIndex >= 0)
            {
                EquationFile value = (EquationFile)((comboitem)((kcombobox)sender).SelectedItem).Tag;

                LoadDesigenEquation(value);
            }
        }

        private void rtb_dx_TextChanged(object sender, EventArgs e)
        {
            RichTextBox t = sender as RichTextBox;
            testrichtextbox_text(t, -1);
            toolTip1.SetToolTip(t, t.Text);
        }

        private void kgriditem18_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = rtb_dx.Text;
            Control c = sender as Control;          
            cf.Vars =  new string[] { "x", "y","v"};
            if (cf.ShowDialog() == DialogResult.OK)
            {
                rtb_dx.Text = cf.equationvalue;
            }
        }

        private void kgriditem17_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = rtb_dy.Text;
            Control c = sender as Control;
            cf.Vars = new string[] { "x", "y", "v" };
            if (cf.ShowDialog() == DialogResult.OK)
            {
                rtb_dy.Text = cf.equationvalue;
            }
        }

        private void kgriditem16_Click(object sender, EventArgs e)
        {
            CalculatorForm cf = new CalculatorForm();
            cf.equationvalue = rtb_dz.Text;
            Control c = sender as Control;
            cf.Vars = new string[] { "x", "y", "v" };
            if (cf.ShowDialog() == DialogResult.OK)
            {
                rtb_dz.Text = cf.equationvalue;
            }
        }
        bool isonfacedefining = false;

        private void button5_Click(object sender, EventArgs e)
        {
            isonfacedefining = !isonfacedefining;
            
            if (isonfacedefining)
            {
                label23.Text = "0 Points picked of 4";             
               desigenclosedfaces.Add(new int[4]{-1,-1,-1,-1});
            }
            else
            {
              
                isondrawing = false;
                if (desigenclosedfaces.Count>0)
                {             
                    if (crnt_colsed_face_vector_indx!=3)
                    {                                  
                        desigenclosedfaces.RemoveAt(desigenclosedfaces.Count - 1);
                    }
                } 
                label23.Text =desigenclosedfaces.Count+" Face(s) detected";             
           
            }
            crnt_colsed_face_vector_indx = -1;
            button5.Text = isonfacedefining == true ? "Cancel Defining" : "Define Closed Face";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            crnt_colsed_face_vector_indx = -1;
            desigenclosedfaces.Clear();
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));
        
        }

        private void button6_Click(object sender, EventArgs e)
        {
            crnt_colsed_face_vector_indx = -1;
            if (desigenclosedfaces.Count > 0)
            {desigenclosedfaces.RemoveAt(desigenclosedfaces.Count - 1);}                         
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));
        
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            updatdrawing(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0));     
        }

        private void ifif(object sender, EventArgs e)
        {
            
        }

        private void pic_desigen_Click(object sender, EventArgs e)
        {

        }
    }
}