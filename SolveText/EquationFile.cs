using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SolveText
{
    [Serializable()]
    public class EquationFile : ICloneable
    {
        
      
        public static EquationFile FromFile(string address)
        {
            return fnc.Selraliz.ReadFromBinaryFile<EquationFile>(address);
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
        public EquationFile()
        {

        }
        public EquationFile(string title, string explct, PointF xdomain, PointF ydomain)
        {
            this.expliciteqaution = explct; this.title = title; this.domainX = xdomain; this.domainY = ydomain;
        }
        public EquationFile(string title, string explct)
        {
            this.expliciteqaution = explct; this.title = title;
        }
        public EquationFile(string title)
        {
            this.title = title;
        }
        public EquationFile(string title, string x, string y, string z, PointF udomain, PointF vdomain)
        {
            this.paramX = x; this.paramY = y; this.paramZ = z; this.title = title; this.DomainU = udomain; this.domainV = vdomain;
        }
        public EquationFile(string title, string x, string y, string z, PointF udomain, PointF vdomain,Bitmap img)
        {
            this.paramX = x; this.paramY = y; this.paramZ = z; this.title = title; this.DomainU = udomain; this.domainV = vdomain; this.Image = img;
        }
        public EquationFile(string title, string x, string y, string z)
        {
            this.paramX = x; this.paramY = y; this.paramZ = z; this.title = title;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public override string ToString()
        {
            return string.Format("F(X)={0},X={1},system:{2},description:{3}", new object[4] { paramX, paramY, isdeg == true ? "Degrees" : "Raidains", description });
        }
        public string[] ParamHelpingvariblesequations=new string[0];
        public string[] ExplicitHelpingvariblesequations = new string[0];

        public string[] GetHelpingvaiblesSymbols()
        {
            List<string> helpingvarsmarks = new List<string>();
            if (this.isExplicit)
            {
                for (int i = 0; i < this.ExplicitHelpingvariblesequations.Length; i++)
                {
                    helpingvarsmarks.Add("c" + (i + 1));
                }
            }
            else
            {
                for (int i = 0; i < this.ParamHelpingvariblesequations.Length; i++)
                {
                    helpingvarsmarks.Add("c" + (i + 1));
                }
            }
            return helpingvarsmarks.ToArray();
        }           
        public string[] GetAllSymbols()
        {  string[] mainmarks=  new string[2] { "u", "v" };
        if (this.isExplicit) { mainmarks = new string[2] { "x", "y" }; }
            List<string> allstring = new List<string>();allstring.AddRange(mainmarks);
            allstring.AddRange(this.GetHelpingvaiblesSymbols());
            return allstring.ToArray();
        }
        private Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }
        private string paramX = "";
        public string ParamX
        {
            get { return paramX; }
            set { paramX = value; }
        }
        bool autodetectstep = true;

        public bool AutoDetectStep
        {
            get { return autodetectstep; }
            set { autodetectstep = value; }
        }
        private string paramY = "";

        public string ParamY
        {
            get { return paramY; }
            set { paramY = value; }
        }
        private string paramZ = "";

        public string ParamZ
        {
            get { return paramZ; }
            set { paramZ = value; }
        }
        private string expliciteqaution = "";

        public string Expliciteqaution
        {
            get { return expliciteqaution; }
            set { expliciteqaution = value; }
        }
        private float stepu = 1f;

        public float Stepu
        {
            get { return stepu; }
            set { stepu = value; }
        }
        private float stepv = 1f;

        public float Stepv
        {
            get { return stepv; }
            set { stepv = value; }
        }
        private bool isExplicit = false;

        public bool IsExplicit
        {
            get { return isExplicit; }
            set { isExplicit = value; }
        }

        private bool isdeg = true;

        public bool Isdeg
        {
            get { return isdeg; }
            set { isdeg = value; }
        }
        private PointF domainU = new PointF();
        public PointF DomainU
        {
            get { return domainU; }
            set { domainU = value; }
        }
        private PointF domainY = new PointF();
        public PointF DomainY
        {
            get { return domainY; }
            set { domainY = value; }
        }
        private PointF domainX = new PointF();
        public PointF DomainX
        {
            get { return domainX; }
            set { domainX = value; }
        }
        private PointF domainV = new PointF();
        public PointF DomainV
        {
            get { return domainV; }
            set { domainV = value; }
        }
        private Color color = Color.Black;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private string title = "";

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string description = "";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
