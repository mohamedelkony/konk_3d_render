using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Reflection;
using System.Linq;
using System.Drawing.Imaging;
using Microsoft.CSharp;
using System.Reflection.Emit;
namespace SolveText
{

    public class LockBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[PixelCount * step];
                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }
    }
    #region kgrphs
    [Serializable()]
    public class KStringFormat : Object, ICloneable
    {
        public StringAlignment Alignment = StringAlignment.Near;
        public StringDigitSubstitute DigitSubstitutionMethod { get { return this.ToSF().DigitSubstitutionMethod; } }
        public int DigitSubstitutionLanguage { get { return this.ToSF().DigitSubstitutionLanguage; } }
        public StringFormatFlags FormatFlags = (StringFormatFlags)0;
        public HotkeyPrefix HotkeyPrefix = HotkeyPrefix.None;
        public StringAlignment LineAlignment = StringAlignment.Near;
        public StringTrimming Trimming = StringTrimming.Character;
        public KStringFormat()
        { }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public StringFormat ToSF()
        {
            StringFormat v = new StringFormat();
            v.Alignment = this.Alignment;
            v.FormatFlags = this.FormatFlags;
            v.HotkeyPrefix = this.HotkeyPrefix;
            v.LineAlignment = this.LineAlignment;
            v.Trimming = this.Trimming;
            return v;
        }
    }
    [Serializable()]
    public class KColorBlend : ICloneable
    {
        public KColorBlend(ColorBlend cb)
        {


            this.postions = cb.Positions; this.colors = cb.Colors; this.AllowEquater = new bool[0];
        }
        public System.Drawing.Drawing2D.ColorBlend ToCB()
        {
            System.Drawing.Drawing2D.ColorBlend value = new ColorBlend();
            value.Colors = this.Colors;
            value.Positions = this.postions;

            return value;
        }
        private bool[] allowequater;

        public bool[] AllowEquater
        {
            get { return allowequater; }
            set { allowequater = value; }
        }

        private float[] postions;

        public float[] Positions
        {
            get { return postions; }
            set { postions = value; }
        }
        private Color[] colors;

        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        public KColorBlend()
        {

        }
        public int Count()
        {
            if (postions == null || colors == null)
            { return -1; }
            else if (postions.Length != colors.Length)
            {
                return -1;
            }
            else
            { return postions.Length; }
        }
        public KColorBlend(int count)
        {
            this.postions = new float[count];
            this.colors = new Color[count];
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    [Serializable()]
    public class KBlend : ICloneable
    {
        public KBlend(Blend cb)
        { this.postions = cb.Positions; this.factors = cb.Factors; }
        public System.Drawing.Drawing2D.Blend ToB()
        {
            System.Drawing.Drawing2D.Blend value = new Blend();
            value.Factors = this.Factors;
            value.Positions = this.postions;

            return value;
        }


        private float[] postions;

        public float[] Positions
        {
            get { return postions; }
            set { postions = value; }
        }
        private float[] factors;

        public float[] Factors
        {
            get { return factors; }
            set { factors = value; }
        }
        public KBlend()
        {

        }
        public int Count()
        {
            if (postions == null || factors == null)
            { return -1; }
            else if (postions.Length != factors.Length)
            {
                return -1;
            }
            else
            { return postions.Length; }
        }
        public KBlend(int count)
        {
            this.postions = new float[count];
            this.factors = new float[count];
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class kpen
    {
        public kpen()
        {

        }
        public kpen(Color solidcolour)
        { this.SolidColor = solidcolour; }
        public kpen(Color solidcolour, int width)
        { this.SolidColor = solidcolour; this.Width = width; }
        public kpen(KColorBlend cb)
        { this.ColorBlend = cb; }
        public kpen(KColorBlend cb, int width)
        { this.ColorBlend = cb; this.Width = width; }
        private int thick = 0;

        public int Thick
        {
            get { return thick; }
            set { thick = value; }
        }
        private Color solidcolor = Color.Black;
        public Color SolidColor
        {
            get { return solidcolor; }
            set { solidcolor = value; }
        }

        KColorBlend cb = null;
        public KColorBlend ColorBlend
        {
            get { return cb; }
            set { cb = value; }
        }

        int width = 1;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        bool enablecutedit = true;
        public bool EnableCutEdit
        {
            get { return enablecutedit; }
            set { enablecutedit = value; }
        }
/*
        dublcorners dc = new dublcorners();
        public dublcorners DoubleCorners
        {
            get { return dc; }
            set { dc = value; }
        }
        */
        int[][] spacingtype = new int[1][] { new int[2] { 0, 0 } };
        public int[][] SpacingType
        {
            get { return spacingtype; }
            set { spacingtype = value; }
        }

        float startcut = 0;
        public float StartCut
        {
            get { return startcut; }
            set { startcut = value; }
        }

        bool centercut = false;
        public bool CenterCut
        {
            get { return centercut; }
            set { centercut = value; }
        }

        int cutrotateangle = 0;
        public int CutRotateAngle
        {
            get { return cutrotateangle; }
            set { cutrotateangle = value; }
        }

        Bitmap cutiamge = null;
        public Bitmap CutIamge
        {
            get { return cutiamge; }
            set { cutiamge = value; }
        }

        bool enablecut = true;
        public bool EnableCut
        {
            get { return enablecut; }
            set { enablecut = value; }
        }

        GraphicsPath cutpath = null;
        public GraphicsPath CutPath
        {
            get { return cutpath; }
            set { cutpath = value; }
        }
    }
    public class cornsbrush
    {
        KColorBlend cornersblend = new KColorBlend();

        public KColorBlend Cornersblend
        {
            get { return cornersblend; }
            set { cornersblend = value; }
        }
        private bool iscenterfill = true;

        public bool Iscenterfill
        {
            get { return iscenterfill; }
            set { iscenterfill = value; }
        }
        KBlend blend = null;

        public KBlend Blend
        {
            get { return blend; }
            set { blend = value; }
        }

        KColorBlend equtorblend = null;

        public KColorBlend Equtorblend
        {
            get { return equtorblend; }
            set { equtorblend = value; }
        }

        int[] cornersloctaioninequator = new int[] { };

        public int[] Cornersloctaioninequator
        {
            get { return cornersloctaioninequator; }
            set { cornersloctaioninequator = value; }
        }

        Color color1;

        public Color Color1
        {
            get { return color1; }
            set { color1 = value; }
        }

        Color color2;

        public Color Color2
        {
            get { return color2; }
            set { color2 = value; }
        }

        PointF movedcenterpoint = new PointF();

        public PointF Movedcenterpoint
        {
            get { return movedcenterpoint; }
            set { movedcenterpoint = value; }
        }

        int startangle = 0;

        public int Startangle
        {
            get { return startangle; }
            set { startangle = value; }
        }

        PointF[] cotners = new PointF[] { };
        bool solidfill = false;

        public bool SolidFill
        {
            get { return solidfill; }
            set { solidfill = value; }
        }
        private bool solidequater = false;

        public bool Solidequater
        {
            get { return solidequater; }
            set { solidequater = value; }
        }
        public PointF[] Cotners
        {
            get { return cotners; }
            set { cotners = value; }
        }

        RectangleF rectangle = new RectangleF();

        public RectangleF Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        public cornsbrush()
        { }
        public cornsbrush(RectangleF rec)
        {
            this.Rectangle = rec;
        }
    }
    public class KGrphs
    {
        #region puplic static methods
        public static PointF[] Getlinepoints(PointF p1, PointF p2)
        {
            int count = (int)Math.Max(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
            PointF[] pps = new PointF[count];

            PointF Rate = new PointF();
            Rate.X = fnc.divdec(p2.X - p1.X, count);
            Rate.Y = fnc.divdec(p2.Y - p1.Y, count);
            for (int ix = 0; ix < count; ix += 1)
            {

                pps[ix].X = Rate.X * ix + p1.X;

            }

            for (int iy = 0; iy < count; iy += 1)
            {

                pps[iy].Y = Rate.Y * iy + p1.Y;

            }


            return pps;
        }
        public static PointF[] Getlinepoints(float f1, float f2, float f3, float f4)
        {
            return Getlinepoints(new PointF(f1, f2), new PointF(f3, f4));
        }
        public static Vector3[] Getlinepoints(Vector3 p1, Vector3 p2)
        {
            int count = (int)Math.Max(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
            Vector3[] pps = new Vector3[count];

            Vector3 Rate = new Vector3();
            Rate.X = (p2.X - p1.X) / (float)count;
            Rate.Y = (p2.Y - p1.Y) / (float)count;
            Rate.Z = (p2.Z - p1.Z) / (float)count;
            for (int ix = 0; ix < count; ix += 1)
            {
                pps[ix] = new Vector3();
                pps[ix].X = Rate.X * ix + p1.X;

            }

            for (int iy = 0; iy < count; iy += 1)
            {

                pps[iy].Y = Rate.Y * iy + p1.Y;

            }

            for (int iz = 0; iz < count; iz += 1)
            {

                pps[iz].Z = Rate.Z * iz + p1.Z;

            }


            return pps;
        }

        public static Color[] GetGradientColors(Color c1, Color c2)
        {
            int absR = c2.R - c1.R;
            int absg = c2.G - c1.G;
            int absb = c2.B - c1.B;

            int count = fnc.MaxOfThree(Math.Abs(absR), Math.Abs(absg), Math.Abs(absb));
            if (count == 0) { count = 1; }
            Color[] cols = new Color[count];

            float rateR = fnc.divdec(absR, count);
            float rateG = fnc.divdec(absg, count);
            float rateB = fnc.divdec(absb, count);

            for (int i = 0; i < count; i++)
            {
                int vR = fnc.tint(c1.R + (rateR * i));
                int vG = fnc.tint(c1.G + (rateG * i));
                int vB = fnc.tint(c1.B + (rateB * i));

                cols[i] = Color.FromArgb(vR, vG, vB);

            }
            return cols;
        }
        public static Color[] GetMultiGradientColors(Color[] cb)
        {
            List<Color> value = new List<Color>();
            for (int i = 0; i < cb.Length - 1; i++)
            {
                Color[] cc = GetGradientColors(cb[i], cb[i + 1]);
                value.AddRange(cc);
            }
            return value.ToArray();
        }
        public static RectangleF GetRotatedRectangle(int ang, SizeF sz)
        {
            float angle = fnc.Topi(ang);

            float radians = fnc.Topi(angle);

            PointF p1 = new PointF();
            p1.X = (float)((0 * Math.Cos(angle)) + (0 * -Math.Sin(angle)));
            p1.Y = (float)((0 * Math.Sin(angle)) + (0 * Math.Cos(angle)));

            PointF p2 = new PointF();
            p2.X = (float)((0 * Math.Cos(angle)) + (sz.Height * -Math.Sin(angle)));
            p2.Y = (float)((0 * Math.Sin(angle)) + (sz.Height * Math.Cos(angle)));

            PointF p3 = new PointF();
            p3.X = (float)((sz.Width * Math.Cos(angle)) + (0 * -Math.Sin(angle)));
            p3.Y = (float)((sz.Width * Math.Sin(angle)) + (0 * Math.Cos(angle)));

            PointF p4 = new PointF();
            p4.X = (float)((sz.Width * Math.Cos(angle)) + (sz.Height * -Math.Sin(angle)));
            p4.Y = (float)((sz.Width * Math.Sin(angle)) + (sz.Height * Math.Cos(angle)));

            SizeF sf = new SizeF();

            float minx = Math.Min(p1.X, Math.Min(p2.X, Math.Min(p3.X, p4.X)));
            float miny = Math.Min(p1.Y, Math.Min(p2.Y, Math.Min(p3.Y, p4.Y)));

            sf.Width = Math.Max(p1.X, Math.Max(p2.X, Math.Max(p3.X, p4.X))) - minx;
            sf.Height = Math.Max(p1.Y, Math.Max(p2.Y, Math.Max(p3.Y, p4.Y))) - miny;

            return new RectangleF(minx, miny, sf.Width, sf.Height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ang"></param>
        /// <param name="rec"></param>
        /// <param name="center">if (false) rotate around firstpoint else locate rec at center</param>
        /// <returns></returns>
        public static RectangleF GetRotatedRectangle(int ang, RectangleF rec, bool center)
        {
            RectangleF crntrec = GetRotatedRectangle(ang, rec.Size);
            RectangleF bigrec = GetRotatedRectangle(45, rec.Size);

            float trnx = (rec.Width - crntrec.Width) / 2 - crntrec.X;
            float trny = (rec.Height - crntrec.Height) / 2 - crntrec.Y;

            RectangleF value = new RectangleF();
            value.Size = crntrec.Size;
            value.X = crntrec.X + rec.X; value.Y = crntrec.Y + rec.Y;
            if (center)
            {
                value.X += trnx;
                value.Y += trny;
            }

            return value;

        }


        public static Rectangle GetRotatedRectangle(int ang, Size sz)
        { return Rectangle.Round(GetRotatedRectangle(ang, new SizeF(sz.Width, sz.Height))); }
        public static Bitmap GetRotatedImage(int angl, Bitmap SrcBitmap, bool autocenter)
        {

            Rectangle bigrec = GetRotatedRectangle(45, SrcBitmap.Size);

            Bitmap bv = new Bitmap(bigrec.Width, bigrec.Height);
            Rectangle crntrec = GetRotatedRectangle(angl, SrcBitmap.Size);
            int trnx = (bigrec.Width - crntrec.Width) / 2 + (-1 * crntrec.X);
            int trny = (bigrec.Height - crntrec.Height) / 2 + (-1 * crntrec.Y);
            float angle = fnc.Topi(angl);
            for (int x = 0; x < SrcBitmap.Width; x++)
            {
                for (int y = 0; y < SrcBitmap.Height; y++)
                {
                    Point v = new Point();
                    v.X = (int)((x * Math.Cos(angle)) + (y * -Math.Sin(angle)));
                    v.Y = (int)((x * Math.Sin(angle)) + (y * Math.Cos(angle)));
                    if (autocenter)
                    {
                        v.X += trnx; v.Y += trny;
                    }
                    Color cv = SrcBitmap.GetPixel(x, y);

                    //    SetPixel(v.X, v.Y, cv, bv, true);



                }
            }
            return bv;
        }
        public static Point rotatepoint(Point p, Rectangle round, float angle, Point centerloc, bool autocenter)
        {


            Rectangle bigrec = GetRotatedRectangle(45, round.Size);

            Rectangle crntrec = GetRotatedRectangle((int)angle, round.Size);
            angle = (float)fnc.Topi(angle);
            int trnx = (bigrec.Width - crntrec.Width) / 2 + (-1 * crntrec.X);
            int trny = (bigrec.Height - crntrec.Height) / 2 + (-1 * crntrec.Y);

            Point v = new Point();
            p.X = p.X - round.X; p.Y = p.Y - round.Y;
            v.X = (int)((p.X * Math.Cos(angle)) + (p.Y * -Math.Sin(angle)));
            v.Y = (int)((p.X * Math.Sin(angle)) + (p.Y * Math.Cos(angle)));
            v.X += centerloc.X;
            v.Y += centerloc.Y;

            if (autocenter)
            {
                v.X += trnx; v.Y += trny;

                v.X -= bigrec.Width / 2;
                v.Y -= bigrec.Height / 2;
            }

            return v;
        }
        public static PointF rotatepoint(PointF p, RectangleF round, float angle, PointF centerloc, bool autocenter)
        {

            RectangleF bigrec = GetRotatedRectangle(45, round.Size);

            RectangleF crntrec = GetRotatedRectangle((int)angle, round.Size);
            angle = (float)fnc.Topi(angle);
            float trnx = (bigrec.Width - crntrec.Width) / 2 + (-1 * crntrec.X);
            float trny = (bigrec.Height - crntrec.Height) / 2 + (-1 * crntrec.Y);

            PointF v = new PointF();
            p.X = p.X - round.X; p.Y = p.Y - round.Y;
            v.X = (int)((p.X * Math.Cos(angle)) + (p.Y * -Math.Sin(angle)));
            v.Y = (int)((p.X * Math.Sin(angle)) + (p.Y * Math.Cos(angle)));
            v.X += centerloc.X;
            v.Y += centerloc.Y;

            if (autocenter)
            {
                v.X += trnx; v.Y += trny;

                v.X -= bigrec.Width / 2;
                v.Y -= bigrec.Height / 2;
            }

            return v;
        }

        // rotate POINT with know of trns,bigrectange(bigsize) to avoid claculate them everytime if it were in loop     
        public static PointF rotatepoint(PointF p, Point round, Point trns, Size bigsize, float angle, Point centerloc, bool autocenter)
        {


            angle = (float)fnc.Topi(angle);

            Point v = new Point();
            p.X = p.X - round.X; p.Y = p.Y - round.Y;
            v.X = (int)((p.X * Math.Cos(angle)) + (p.Y * -Math.Sin(angle)));
            v.Y = (int)((p.X * Math.Sin(angle)) + (p.Y * Math.Cos(angle)));
            v.X += centerloc.X;
            v.Y += centerloc.Y;

            if (autocenter)
            {
                v.X += trns.X; v.Y += trns.Y;
                v.X -= bigsize.Width / 2;
                v.Y -= bigsize.Height / 2;
            }

            return v;
        }
        public static Point rotatepoint(Point p, Point round, Point trns, Size bigsize, float angle, Point centerloc, bool autocenter)
        {
            return rotatepoint(p, round, trns, bigsize, angle, centerloc, autocenter);
        }
        public static GraphicsPath rotatepath(GraphicsPath gp, int angle, Rectangle gprec)
        {
            if (gp.PointCount == 0)
            { return new GraphicsPath(); }
            GraphicsPath v = new GraphicsPath();
            PointF[] ps = new PointF[gp.PointCount];
            Byte[] bb = new Byte[ps.Length];
            for (int i = 0; i < gp.PointCount; i++)
            {
                ps[i] = KGrphs.rotatepoint(gp.PathPoints[i], gprec, angle, new Point(gprec.X + gprec.Width / 2, gprec.Y + gprec.Height / 2), true);
                bb[i] = gp.PathTypes[i];
            }
            return new GraphicsPath(ps, bb);
        }
     
        public static GraphicsPath multiplaypath(GraphicsPath gp, PointF pnt)
        {
            if (gp.PointCount == 0)
            { return new GraphicsPath(); }
            GraphicsPath v = new GraphicsPath();
            PointF[] ps = new PointF[gp.PointCount];
            Byte[] bb = new Byte[ps.Length];
            for (int i = 0; i < gp.PointCount; i++)
            {
                PointF c = gp.PathPoints[i];
                ps[i] = new PointF(pnt.X + c.X, pnt.Y + c.Y);
                bb[i] = gp.PathTypes[i];
            }
            return new GraphicsPath(ps, bb);
        }
        public static PointF[] multiplaypoints(PointF[] ps, PointF pnt)
        {
            PointF[] value = new PointF[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
               
               value[i] = new PointF(pnt.X + ps[i].X, pnt.Y + ps[i].Y);
              
            }
            return value;
        }
        public static Vector3[] multiplaypoints(Vector3[] ps, PointF3D pnt)
        {
            Vector3[] v = new Vector3[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                v[i] = (Vector3)ps[i].Clone();
               v[i].X= pnt.X + ps[i].X;
                v[i].Y= pnt.Y + ps[i].Y;
               v[i].Z= pnt.Z + ps[i].Z;
            }
            return v;
        }
        public static PointF[] resizepoints(PointF[] pnts, SizeF sz,bool recenter=true)
        {


            PointF[] ps = new PointF[pnts.Length];

            RectangleF rec = Recofpnts(pnts);
            for (int i = 0; i < pnts.Length; i++)
            {
                PointF c = pnts[i];
                SizeF saferecsize =new SizeF( rec.Width == 0 ? 1 : rec.Width,rec.Height==0?1:rec.Height);
                PointF translate = recenter == true ? rec.Location : new PointF();
                ps[i] = new PointF(translate.X + (c.X - rec.X) / (saferecsize.Width) * sz.Width, translate.Y + (c.Y - rec.Y) / (saferecsize.Height) * sz.Height);
               
            }
            return ps;
        }
        public static Vector3[] resize3dpoints(Vector3[] pnts, SizeF3D sz,RectangleF3D recofpnts)
        {


            Vector3[] ps = new Vector3[pnts.Length];

            RectangleF3D rec = recofpnts;
            for (int i = 0; i < pnts.Length; i++)
            {
           
                
                Vector3 c = (Vector3)pnts[i].Clone();
                if (rec.Width==0)
                {
                    c.X = 0;
                }
                else
                {
              c.X = rec.X + (c.X - rec.X) / (rec.Width) * sz.Width;              
                }
                if (rec.Height == 0)
                {
                    c.Y = 0;
                }
                else
                {
                    c.Y = rec.Y + (c.Y - rec.Y) / (rec.Height) * sz.Height;
                }
                if (rec.Thick == 0)
                {
                    c.Z = 0;
                }
                else
                {
                    c.Z = rec.Z + (c.Z - rec.Z) / (rec.Thick) * sz.Thick;
                }
              
               ps[i] = c;
            }
            return ps;
        }
      
        public static Vector3[] resize3dpoints(Vector3[] pnts, SizeF3D sz)
        {


            Vector3[] ps = new Vector3[pnts.Length];
            RectangleF3D rec = Recof3dpnts(pnts);
            for (int i = 0; i < pnts.Length; i++)
            {
                Vector3 c =(Vector3) pnts[i].Clone();
            
                 c.X = rec.X + (c.X - rec.X) / (rec.Width) * sz.Width;
                 c.Y=rec.Y + (c.Y - rec.Y) / (rec.Height ) * sz.Height;
                c.Z=rec.Z + (c.Z - rec.Z) / (rec.Thick ) * sz.Thick;
              
                if (float.IsNaN(c.X))
                {
                    c.X = pnts[i].X;
                }
                if (float.IsNaN(c.Y))
                {
                    c.Y = pnts[i].Y;
                }
                if (float.IsNaN(c.Z))
                {
                    c.Z = pnts[i].Z;
                } 
                ps[i] = c;
            
            }
            return ps;
        }

        public static GraphicsPath resizepath(GraphicsPath gp, SizeF sz)
        {
            if (gp.PointCount == 0)
            { return new GraphicsPath(); }
            GraphicsPath v = new GraphicsPath();
            PointF[] ps = new PointF[gp.PathPoints.Length];
            Byte[] bb = new Byte[ps.Length];
            RectangleF rec = Recofpnts(gp.PathPoints);
            for (int i = 0; i < gp.PathPoints.Length; i++)
            {
                PointF c = gp.PathPoints[i];
                ps[i] = new PointF(rec.X + (c.X - rec.X) / (rec.Width ) * sz.Width, rec.Y + (c.Y - rec.Y) / (rec.Height ) * sz.Height);
                bb[i] = gp.PathTypes[i];
            }
            return new GraphicsPath(ps, bb);
        }
        public static RectangleF Recofpnts(PointF[] pnts)
        {

            SizeF g = new SizeF();
            PointF smin = pnts[0];
            PointF smax = pnts[0];
            for (int i = 0; i < pnts.Length; i++)
            {
                PointF c = pnts[i];
                if (c.X < smin.X)
                { smin.X = c.X; }
                if (c.Y < smin.Y)
                {smin.Y = c.Y; }
            
                if (c.X > smax.X)
                { smax.X = c.X; }
                if (c.Y > smax.Y)
                { smax.Y = c.Y; }
            }
           
            g.Width = smax.X - smin.X;
            g.Height = smax.Y - smin.Y;
            return new RectangleF(smin.X, smin.Y, g.Width, g.Height);
        }
        public static RectangleF3D Recof3dpnts(Vector3[] pnts)
        {

            SizeF g = new SizeF();
            float thick = 0;
         
            Vector3 smin =(Vector3)pnts[0].Clone();
            Vector3 smax = (Vector3)pnts[0].Clone();
            for (int i = 0; i < pnts.Length; i++)
            {
                Vector3 c =(Vector3) pnts[i].Clone();
                if (c.X < smin.X)
                { smin.X = c.X; }
                if (c.Y < smin.Y)
                { smin.Y = c.Y; }
                if (c.Z < smin.Z)
                { smin.Z = c.Z; }

                //=======================
               
                if (c.X > smax.X)
                { smax.X = c.X; }
                if (c.Y > smax.Y)
                { smax.Y = c.Y; }
                if (c.Z > smax.Z)
                { smax.Z = c.Z; }

            }
            g.Width = smax.X - smin.X;
            g.Height = smax.Y - smin.Y;
            thick = smax.Z - smin.Z;

            return new RectangleF3D(smin.X,smin.Y,smin.Z,g.Width,g.Height,thick);
        }
       
        #endregion

        #region properts & installation
        public RectangleF rotaterec;
        int rotatrangle = 0;
        public int Rotatrangle
        {
            get { return rotatrangle; }
            set { rotatrangle = value; }
        }
        bool centerrotate = false;
        public bool Centerrotate
        {
            get { return centerrotate; }
            set { centerrotate = value; }
        }

        private GraphicsPath clippath = null;
        public GraphicsPath Clippath
        {
            get { return clippath; }
            set { clippath = value; }
        }
        Bitmap editedimage;
        public Bitmap Editedimage
        {
            get { return editedimage; }

        }
        Bitmap orignimage;
        public Bitmap Orignimage
        {
            get { return orignimage; }

        }

        public KGrphs(Bitmap image)
        {
            this.editedimage = image;
            this.orignimage = image;
        }
        #endregion

        #region inner private helping methods
        #region setpixels methods
        private void SetPixel(Point pf, Color c)
        {
            SetPixel((int)pf.X, (int)pf.Y, c, editedimage, true);
        }
        private void SetPixel(int fx, int fy, Color c)
        {
            SetPixel(fx, fy, c, editedimage, true);
        }
        private void SetPixel(int fx, int fy, Color c, bool fix)
        {
            SetPixel(fx, fy, c, editedimage, fix);
        }
        private void SetPixel(float fx, float fy, Color c)
        {
            SetPixel((int)fx, (int)fy, c, editedimage, true);
        }
        private void SetPixel(float fx, float fy, Color c, bool fix)
        {
            SetPixel((int)fx, (int)fy, c, editedimage, fix);
        }
        private void SetPixel(PointF p1, Color c)
        {
            SetPixel((int)p1.X, (int)p1.Y, c, editedimage, true);
        }
        private void SetPixel(int fx, int fy, Color c, Bitmap b, bool fix, GraphicsPath clippth)
        {
            if (rot())
            {
                PointF v = rotatepoint(new Point(fx, fy), rotaterec, rotatrangle, rotaterec.Location, centerrotate);
                fx = (int)v.X; fy = (int)v.Y;
            }
            setbabpix(fx, fy, c, b, clippath);

            if (fix)
            {
                setbabpix(fx + 1, fy, c, b, clippath);
                setbabpix(fx - 1, fy, c, b, clippath);
                setbabpix(fx, fy + 1, c, b, clippath);
                setbabpix(fx, fy - 1, c, b, clippath);
            }
        }
        private void SetPixel(int fx, int fy, Color c, Bitmap b, bool fix)
        {
            SetPixel(fx, fy, c, b, fix, null);
        }
        private void setbabpix(int fx, int fy, Color c, Bitmap b, GraphicsPath clippth)
        {
            int x = fx; int y = fy;

            if (x >= 0 && x < b.Width)
            {
                if (y >= 0 && y < b.Height)
                {
                    if (clippath != null)
                    {

                        if (clippth.IsVisible(x, y) == true)
                        {
                            b.SetPixel(x, y, c);
                        }
                    }
                    else
                    {
                        b.SetPixel(x, y, c);
                    }
                }
            }
        }
        private void setbabpix(int fx, int fy, Color c, Bitmap b)
        {
            setbabpix(fx, fy, c, b, null);
        }
        #endregion
        private PointF getpointoutofarray(PointF[][] arr, int num)
        {
            int dx = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (num >= dx && num < dx + arr[i].Length)
                {
                    return arr[i][num - dx];
                }
                dx += arr[i].Length;
            }

            throw new Exception("Error");
        }
        bool rot()
        {
            if (centerrotate || rotatrangle != 0)
            {
                return true;
            }
            return false;
        }
        private Bitmap resizeimage(Bitmap b, Size sz)
        {
            Bitmap v = new Bitmap(sz.Width, sz.Height);
            Graphics g = Graphics.FromImage(v);
            g.DrawImage(b, 0, 0, sz.Width, sz.Height);
            return v;
        }
        #endregion

        #region Main puplic draw methods

        public void FillPoints(cornsbrush brsh, RectangleF rec)
        {
            this.rotaterec = rec;

            this.fillpointsGradientsx(brsh.Cornersblend, brsh.Equtorblend, brsh.Cornersloctaioninequator, brsh.Movedcenterpoint, brsh.Startangle, brsh.Cotners, brsh.Rectangle, brsh.SolidFill, brsh.Solidequater, true);
        }
        public void FillCircle(cornsbrush brsh, RectangleF rec)
        {
            FillCircle(brsh, rec, 360);
        }
        public void FillCircle(cornsbrush brsh, RectangleF rec, int sweepangle)
        {
            this.rotaterec = rec;

            if (brsh.Iscenterfill)
            {
                this.fillcirclegradients(brsh.Cornersblend, brsh.Equtorblend, brsh.Cornersloctaioninequator, brsh.Movedcenterpoint, brsh.Startangle, brsh.Rectangle, sweepangle, brsh.SolidFill, brsh.Solidequater, true);
            }
            else
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddArc(rec, brsh.Startangle, sweepangle);
                this.fillRecGradients(brsh.Cornersblend, rec, brsh.Startangle, gp);
            }
        }
        public void FillRectangle(cornsbrush brsh, RectangleF rec)
        {
            this.rotaterec = Rectangle.Round(rec);
            if (brsh.Blend != null)
            {
                Color[] cls = GetGradientColors(brsh.Color1, brsh.Color2);
                KColorBlend cb = new KColorBlend(brsh.Blend.Positions.Length);

                for (int i = 0; i < brsh.Blend.Factors.Length; i++)
                {
                    Color cv = cls[(int)((cls.Length - 1) * brsh.Blend.Factors[i])];
                    cb.Colors[i] = cv;
                    cb.Positions[i] = brsh.Blend.Positions[i];
                }
                this.fillRecGradients(cb, rec, brsh.Startangle);
            }
            else
            {
                this.fillRecGradients(brsh.Cornersblend, rec, brsh.Startangle);
            }
        }

       /* public void DrawLine(kpen kp, PointF p1, PointF p2)
        {
            this.rotaterec = new Rectangle((int)p1.X, (int)p1.Y, (int)Math.Abs(p1.X - p2.X), (int)Math.Abs(p1.Y - p2.Y));
            int num = kp.Thick;
            KColorBlend cbv = kp.ColorBlend;
            if (kp.ColorBlend == null)
            {
                cbv = new KColorBlend(2); cbv.Colors = new Color[2] { kp.SolidColor, kp.SolidColor }; cbv.Positions = new float[2] { 0, 1 };
            }
            GraphicsPath gp = new GraphicsPath();
            dublcorners dc = new dublcorners();
            gp.AddRectangle(new Rectangle(0, 0, 100, 100));

            GraphicsPath mainclip = this.drawline(p1, p2, cbv, kp.Width, kp.DoubleCorners, kp.SpacingType, kp.StartCut, kp.CenterCut, kp.CutRotateAngle, kp.CutIamge, kp.EnableCut, kp.CutPath, kp.EnableCutEdit, null);
            for (int i = 1; i <= num; i++)
            {
                PointF pv1 = p1; PointF pv2 = p2;
                int incrs = i + 1 / 2;
                if (i % 2 == 0)
                {
                    if (p1.X == p2.X)
                    { pv1.X += incrs; pv2.X += incrs; }
                    else if (p1.Y == p2.Y)
                    { pv1.Y += incrs; pv2.Y += incrs; }
                    else
                    {
                        pv1.X += incrs; pv2.X += incrs;
                    }
                }
                else
                {
                    if (p1.X == p2.X)
                    { pv1.X -= incrs; pv2.X -= incrs; }
                    else if (p1.Y == p2.Y)
                    { pv1.Y -= incrs; pv2.Y -= incrs; }
                    else
                    {
                        pv1.X -= incrs; pv2.X -= incrs;
                    }
                }

                this.drawline(pv1, pv2, cbv, kp.Width, dc, kp.SpacingType, kp.StartCut, kp.CenterCut, 0, null, kp.EnableCut, gp, true, mainclip);

            }

        }
        public void DrawLines(kpen kp, PointF[] lns)
        {
            this.rotaterec = Recofpnts(lns);
            int num = kp.Thick;
            KColorBlend cbv = kp.ColorBlend;
            if (kp.ColorBlend == null)
            {
                cbv = new KColorBlend(2); cbv.Colors = new Color[2] { kp.SolidColor, kp.SolidColor }; cbv.Positions = new float[2] { 0, 1 };
            }

            dublcorners dc = new dublcorners();

            this.drawlines(lns, kp.ColorBlend, kp.Width, kp.DoubleCorners, kp.SpacingType, kp.StartCut);
            for (int i = 1; i <= num; i++)
            {
                PointF[] pnts = new PointF[lns.Length];
                for (int n = 0; n < lns.Length - 1; n++)
                {
                    PointF p1 = lns[n]; PointF p2 = lns[n + 1];
                    PointF pv1 = lns[n]; PointF pv2 = lns[n + 1];
                    int incrs = i + 1 / 2;
                    if (i % 2 == 0)
                    {
                        if (p1.X == p2.X)
                        { pv1.X += incrs; pv2.X += incrs; }
                        else if (p1.Y == p2.Y)
                        { pv1.Y += incrs; pv2.Y += incrs; }
                        else
                        {
                            pv1.X += incrs; pv2.X += incrs;
                        }
                    }
                    else
                    {
                        if (p1.X == p2.X)
                        { pv1.X -= incrs; pv2.X -= incrs; }
                        else if (p1.Y == p2.Y)
                        { pv1.Y -= incrs; pv2.Y -= incrs; }
                        else
                        {
                            pv1.X -= incrs; pv2.X -= incrs;
                        }
                    }
                    pnts[n] = pv1; pnts[n + 1] = pv2;
                }
                this.drawlines(pnts, kp.ColorBlend, kp.Width, kp.DoubleCorners, kp.SpacingType, kp.StartCut);

            }
        }
        public void DrawLines(kpen kp, Point[] lns)
        {
            PointF[] pf = new PointF[lns.Length];
            for (int i = 0; i < lns.Length; i++)
            {
                pf[i] = lns[i];
            }
            DrawLines(kp, lns);
        }
    */
        public void drawimage(Bitmap small, Point loc)
        {
            rotaterec = new Rectangle(loc, small.Size);
            for (int x = 0; x < small.Width; x++)
            {
                for (int y = 0; y < small.Height; y++)
                {
                    SetPixel(loc.X + x, loc.Y + y, small.GetPixel(x, y), true);
                }
            }
        }
        public void drawimage(Bitmap small, PointF loc)
        {
            drawimage(small, Point.Round(loc));
        }
        public void DrawRotatedImage(int angl, Bitmap SrcBitmap, Point centerloc, bool autocenter)
        {

            Rectangle bigrec = GetRotatedRectangle(45, SrcBitmap.Size);

            Rectangle crntrec = GetRotatedRectangle(angl, SrcBitmap.Size);
            int trnx = (bigrec.Width - crntrec.Width) / 2 + (-1 * crntrec.X);
            int trny = (bigrec.Height - crntrec.Height) / 2 + (-1 * crntrec.Y);
            float angle = fnc.Topi(angl);
            for (int x = 0; x < SrcBitmap.Width; x++)
            {
                for (int y = 0; y < SrcBitmap.Height; y++)
                {

                    Color cv = SrcBitmap.GetPixel(x, y);

                    Point vn = rotatepoint(new Point(x, y), Point.Empty, new Point(trnx, trny), bigrec.Size, angl, centerloc, autocenter);
                    SetPixel(vn.X, vn.Y, cv);
                    SetPixel(vn.X + 1, vn.Y, cv);
                    SetPixel(vn.X - 1, vn.Y, cv);
                    SetPixel(vn.X, vn.Y + 1, cv);
                    SetPixel(vn.X, vn.Y - 1, cv);

                }
            }

        }
        #endregion

        #region main privte draw methods
        public void DrawImage(Bitmap img, PointF[] linespoints,bool autosize=true  )
        {
          
            RectangleF rec = Recofpnts(linespoints);
            Size imgsz = img.Size;
           PointF hertp = new PointF(rec.X + rec.Width /2f, rec.Y + rec.Height /2f);
           Size recsz = Size.Round(rec.Size);
            if (recsz.Width<=0)
            {
                return;
            }
            if (recsz.Height <= 0)
            {
                return;
            }
            if (autosize)
            {
                if (imgsz.Width != recsz.Width )
             {
                img = resizeimage(img, recsz);
             }
                else if (imgsz.Height != recsz.Height)
                {
                    img = resizeimage(img, recsz);
             
                }
            }
            imgsz =Size.Round( rec.Size);
            LockBitmap lockBitmap2 = new LockBitmap(img);
            lockBitmap2.LockBits();
            Size editimgsz = editedimage.Size;
          LockBitmap lockBitmap = new LockBitmap(editedimage);
            lockBitmap.LockBits();

          
          
            int totaltpnts = 0;
            PointF[][] Alllinepoints = new PointF[linespoints.Length][];

            for (int i = 0; i < linespoints.Length; i++)
            {
              
                if (i == linespoints.Length - 1)
                {
                    Alllinepoints[linespoints.Length - 1] = Getlinepoints(linespoints[linespoints.Length - 1], linespoints[0]);
                    totaltpnts += Alllinepoints[linespoints.Length - 1].Length;
                }
                else
                {
                    Alllinepoints[i] = Getlinepoints(linespoints[i], linespoints[i + 1]);
                   totaltpnts += Alllinepoints[i].Length;
                }
            }

            float Qhalfnq = (float)Math.Sqrt(Math.Pow(imgsz.Width / 2, 2) + Math.Pow(imgsz.Height / 2, 2));
          
             for (int h = 0; h < totaltpnts; h += 1)
            {
                PointF pp = new PointF();
                pp = getpointoutofarray(Alllinepoints, h);

                float halfnq = (float)Math.Sqrt(Math.Pow(pp.X - hertp.X, 2) + Math.Pow(pp.Y - hertp.Y, 2));
                float ang = (float)(Math.Atan((Math.Abs((pp.Y - hertp.Y)) / Math.Abs(pp.X - hertp.X))) * (180 / Math.PI));             
                ang = fnc.correctquart(ang, pp.X - hertp.X, hertp.Y - pp.Y);
                 List<Color> cls = new List<Color>();

                for (float f = 0; f < Qhalfnq; f += 1f)
                {
                    Point pvalue = new Point();
                    pvalue.X = (int)(imgsz.Width / 2f + f * (float)Math.Cos(fnc.Topi(ang)));
                    pvalue.Y = (int)(imgsz.Height / 2f + f * (float)Math.Sin(-1 * fnc.Topi(ang)));


                    if (pvalue.X >= 0 && pvalue.X < imgsz.Width)
                    {
                        if (pvalue.Y >= 0 && pvalue.Y < imgsz.Height)
                        {
                               
                                  cls.Add(lockBitmap2.GetPixel(pvalue.X, pvalue.Y));

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                
                }
               
                for (float f = 0; f < halfnq; f += 1f)
                {
                    PointF orginp = new PointF();
                    orginp.X = (hertp.X + f * (float)Math.Cos(fnc.Topi(ang)));
                    orginp.Y = (hertp.Y + f * (float)Math.Sin(-1 * fnc.Topi(ang)));
                    Color cvalue = Color.Black;
                    Point pvalue = new Point((int)orginp.X, (int)orginp.Y);
                    int inxr =(int) ((f / halfnq) * cls.Count);
                    if (cls.Count == 0)
                    {
                        cvalue = lockBitmap2.GetPixel(0, 0);
                    }
                    else
                    {
                        cvalue = cls[inxr];
                    }
                        if (pvalue.X>=rec.X&&pvalue.X<rec.Right&&pvalue.X>=0&&pvalue.X<editimgsz.Width)
                  {
                      if (pvalue.Y >= rec.Y && pvalue.Y < rec.Bottom && pvalue.Y >= 0 && pvalue.Y < editimgsz.Height)
                       {
lockBitmap.SetPixel(pvalue.X, pvalue.Y, cvalue);
                      }
                  }
                    
               }
            }
            lockBitmap.UnlockBits();
            lockBitmap2.UnlockBits();
        }
        private void fillpointsGradientsx(KColorBlend cb, KColorBlend cbnq, int[] tocb, PointF TheCenterpoint, int StartAngle, PointF[] linespoints, RectangleF rec, bool solidbelnd, bool solidequter, bool setpixel)
        {
            PointF nq = new PointF();
            nq.X = (float)(rec.Width / 2F);
            nq.Y = (float)(rec.Height / 2F);

            PointF hertp = new PointF(rec.X + rec.Width * TheCenterpoint.X, rec.Y + rec.Height * TheCenterpoint.Y);
            int totaltpnts = 0;
            PointF[][] Alllinepoints = new PointF[linespoints.Length][];

            for (int i = 0; i < linespoints.Length; i++)
            {
                if (i == linespoints.Length - 1)
                {
                    Alllinepoints[linespoints.Length - 1] = Getlinepoints(linespoints[linespoints.Length - 1], linespoints[0]);
                    totaltpnts += Alllinepoints[linespoints.Length - 1].Length;
                }
                else
                {
                    Alllinepoints[i] = Getlinepoints(linespoints[i], linespoints[i + 1]);
                    totaltpnts += Alllinepoints[i].Length;
                }
            }

            Color[] nqcolors = cbnq.Colors;
            float[] nqpostions = cbnq.Positions;
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc = new Color[] { };
                if (!solidbelnd)
                {
                    cc = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);
                }


                int start = (int)(((float)StartAngle / (float)totaltpnts * 360f) + (cb.Positions[i] * totaltpnts));
                int lenth = (int)((cb.Positions[i + 1] * totaltpnts) - (cb.Positions[i] * totaltpnts));
                for (int h = start; h < start + lenth; h += 1)
                {
                    Color ccx = Color.Black;
                    if (solidbelnd)
                    {
                        ccx = cb.Colors[i];
                    }
                    else
                    {
                        int indxof2 = (int)(fnc.divdec(cc.Length - 1, lenth) * ((h) - start));
                        ccx = cc[indxof2];
                    }
                    for (int u = 0; u < tocb.Length; u++)
                    {
                        nqcolors[tocb[u]] = ccx;
                    }
                    // Color[] ccenters = gradycols(cb.Colors[i], cb.Colors[i + 1]);
                    //  nqcolors[nqcolors.Length - 1] = ccx;                  
                    PointF pp = new PointF();
                    if (h >= totaltpnts)
                    {
                        pp = getpointoutofarray(Alllinepoints, h - totaltpnts);
                    }
                    else
                    {
                        pp = getpointoutofarray(Alllinepoints, h);
                    }
                    float halfnq = (float)Math.Sqrt(Math.Pow(pp.X - hertp.X, 2) + Math.Pow(pp.Y - hertp.Y, 2));
                    float ang = (float)(Math.Atan((Math.Abs((pp.Y - hertp.Y)) / Math.Abs(pp.X - hertp.X))) * (180 / Math.PI));
                    ang = fnc.correctquart(ang, pp.X - hertp.X, hertp.Y - pp.Y);

                    //======
                    for (int e = 0; e < nqcolors.Length; e++)
                    {
                        Color[] cols2 = new Color[] { };
                        float estart = 0;
                        float elenth = 0;
                        if (nqcolors.Length > 1)
                        {
                            if (e == nqcolors.Length - 1)
                            { break; }
                            cols2 = GetGradientColors(nqcolors[e], nqcolors[e + 1]);
                            estart = 0 + (nqpostions[e] * halfnq);
                            elenth = ((nqpostions[e + 1] * halfnq) - (nqpostions[e] * halfnq));


                        }
                        else
                        {
                            estart = 0;
                            elenth = halfnq;

                        }

                        //    Color ccenter = cc[(cc.Length - 1) - indxof2];

                        for (float f = estart; f < estart + elenth; f += 1f)
                        {

                            PointF orginp = new PointF();
                            orginp.X = (hertp.X + f * (float)Math.Cos(fnc.Topi(ang)));
                            orginp.Y = (hertp.Y + f * (float)Math.Sin(-1 * fnc.Topi(ang)));

                            Color cvalue;

                            if (nqcolors.Length > 1)
                            {
                                if (solidequter)
                                {
                                    cvalue = nqcolors[e];
                                }
                                else
                                {
                                    int indexof3 = (int)(fnc.divdec(cols2.Length - 1, elenth) * (f - estart));
                                    cvalue = cols2[indexof3];

                                }
                            }
                            else
                            {
                                cvalue = nqcolors[0];
                            }



                            Point pvalue = new Point((int)orginp.X, (int)orginp.Y);

                            if (setpixel)
                            {
                                
                                this.SetPixel(pvalue.X, pvalue.Y, cvalue, true);
                            }
                        }
                    }
                }

            }



        }
        private void fillcirclegradients(KColorBlend cb, KColorBlend cbnq, int[] tocb, PointF TheCenterpoint, int StartAngle, RectangleF rec, int arcangle, bool solidbelnd, bool solidequter, bool setpixel)
        {


            PointF nq = new PointF();
            nq.X = (float)(rec.Width / 2F);
            nq.Y = (float)(rec.Height / 2F);
            PointF hertp = new PointF(rec.X + rec.Width * TheCenterpoint.X, rec.Y + rec.Height * TheCenterpoint.Y);


            Color[] nqcolors = cbnq.Colors;
            float[] nqpostions = cbnq.Positions;
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc = new Color[] { };
                if (!solidbelnd)
                {

                    cc = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);

                }

                float start = StartAngle + (cb.Positions[i] * arcangle);
                float lenth = (((cb.Positions[i + 1] * arcangle)) - ((cb.Positions[i] * arcangle)));
                for (float h = start; h < start + lenth; h += 0.5f)
                {
                    Color ccx = Color.Black;
                    if (solidbelnd)
                    {
                        ccx = cb.Colors[i];
                    }
                    else
                    {
                        int indxof2 = (int)(fnc.divdec(cc.Length - 1, lenth) * ((h) - start));
                        ccx = cc[indxof2];
                    }
                    for (int u = 0; u < tocb.Length; u++)
                    {

                        // nqcolors[tocb[u]] = ccx;
                    }
                    for (int u = 0; u < cb.AllowEquater.Length; u++)
                    {
                        if (cb.AllowEquater[u] == false)
                        {
                            nqcolors[u] = ccx;
                        }
                    }
                    // Color[] ccenters = gradycols(cb.Colors[i], cb.Colors[i + 1]);
                    //    nqcolors[nqcolors.Length - 1] = ccx;
                    PointF pp = new PointF();
                    pp.X = (nq.X + nq.X * (float)Math.Cos(fnc.Topi(h)));
                    pp.Y = (nq.Y + nq.Y * (float)Math.Sin(fnc.Topi(h)));

                    float halfnq = (float)Math.Sqrt(Math.Pow(pp.X - hertp.X, 2) + Math.Pow(pp.Y - hertp.Y, 2));
                    float ang = (float)(Math.Atan((Math.Abs((pp.Y - hertp.Y)) / Math.Abs(pp.X - hertp.X))) * (180 / Math.PI));
                    ang = fnc.correctquart(ang, pp.X - hertp.X, hertp.Y - pp.Y);

                    //======
                    for (int e = 0; e < nqcolors.Length; e++)
                    {
                        Color[] cols2 = new Color[] { };
                        float estart = 0;
                        float elenth = 0;

                        if (nqcolors.Length > 1)
                        {
                            if (e == nqcolors.Length - 1)
                            { break; }
                            if (solidequter == false)
                            {
                                cols2 = GetGradientColors(nqcolors[e], nqcolors[e + 1]);
                            }
                            estart = 0 + (nqpostions[e] * halfnq);
                            elenth = ((nqpostions[e + 1] * halfnq) - (nqpostions[e] * halfnq));


                        }
                        else
                        {
                            estart = 0;
                            elenth = halfnq;

                        }

                        for (float f = estart; f < estart + elenth; f += 1f)
                        {

                            PointF orginp = new PointF();

                            orginp.X = (hertp.X + f * (float)Math.Cos(fnc.Topi(ang)));
                            orginp.Y = (hertp.Y + f * (float)Math.Sin(-1 * fnc.Topi(ang)));

                            Color cvalue;
                            if (nqcolors.Length > 1)
                            {
                                if (solidequter)
                                { cvalue = nqcolors[e]; }
                                else
                                {
                                    int indexof3 = (int)(fnc.divdec(cols2.Length - 1, elenth) * (f - estart));
                                    cvalue = cols2[indexof3];
                                }
                            }
                            else
                            {
                                cvalue = nqcolors[0];
                            }

                            Point pvalue = new Point((int)orginp.X, (int)orginp.Y);

                            if (setpixel)
                            { SetPixel((int)rec.X + pvalue.X, (int)rec.Y + pvalue.Y, cvalue, true); }


                        }
                    }
                }

            }

        }
        private void fillRecGradients(KColorBlend cb, RectangleF rec, int angle)
        { fillRecGradients(cb, rec, angle, null); }
        private void fillRecGradients(KColorBlend cb, RectangleF rec, int angle, GraphicsPath reg)
        {

            RectangleF mrec = GetRotatedRectangle(angle, rec, true);
            RectangleF rotrec = new RectangleF(new PointF(rec.X + rec.Width / 2, rec.Y + rec.Height / 2), mrec.Size);

            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);
                float start = (cb.Positions[i] * mrec.Height);
                float lenth = ((cb.Positions[i + 1] * mrec.Height) - (cb.Positions[i] * mrec.Height));
                for (float h = start; h < start + lenth; h++)
                {
                    Color cv = cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))];

                    for (int w = 0; w < mrec.Width; w++)
                    {
                        PointF pv = new PointF(w, h);

                        pv.X += rec.X + rec.Width / 2;
                        pv.Y += rec.Y + rec.Height / 2;

                        pv = rotatepoint(pv, rotrec, angle, rotrec.Location, true);
                        if (rec.Contains(pv))
                        {
                            if (reg == null)
                            {
                                SetPixel(pv.X, pv.Y, cv, true);
                            }
                            else
                            {
                                if (reg.IsVisible(pv))
                                {
                                    SetPixel(pv.X, pv.Y, cv, true);
                                }
                            }
                        }

                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="cb"></param>
        /// <param name="width"></param>
        /// <param name="dc"></param>
        /// <param name="spacingtype"></param>
        /// <param name="startcut"></param>
        /// <param name="centercut">if (true) cut appers its half like arc if (false) all appers</param>
        enum rr { norm, path, img, none };
       /*
        private GraphicsPath drawline(PointF p1, PointF p2, KColorBlend cb, int width, dublcorners dc, int[][] spacingtype, float startcut, bool centercut, int cutrotateangle, Bitmap cutiamge, bool enablecut, GraphicsPath cutpath, bool enablecutedit, GraphicsPath specialallclippath)
        {
            rr a = rr.norm;
            GraphicsPath allinnerpath = new GraphicsPath();
            GraphicsPath innerclippath = null; int innerrotateangle = 0; RectangleF innnerrotaterec = new RectangleF();
            CornersFilter cf = dc.Cornersfilter;
            RectangeEdgeFilter ef = dc.Edgefilter;

            PointF[] pnts = Getlinepoints(p1, p2);
            int numofempty = 0;
            int numoffill = 0;
            int lineangle = 360 - fnc.GetAngle(Point.Round(p1), Point.Round(p2)); if (lineangle == 360) { lineangle = 0; }

            int oo = (int)(pnts.Length * startcut);
            int crntspacetype = 0;

            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);

                int start = (int)(cb.Positions[i] * pnts.Length);
                int realstart = 0;
                int reallenth = 0;
                bool fr = false;
                if (start < oo)
                {
                    fr = true;
                    realstart = start;
                    reallenth = (int)(cb.Positions[i + 1] * pnts.Length) - realstart;
                    start += oo - start;
                }
                int lenth = (int)(cb.Positions[i + 1] * pnts.Length) - start;
                for (int h = start; h < start + lenth; h++)
                {
                    Color c = cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))];
                    PointF v = pnts[h];
                    if (fr)
                    {
                        c = cc1[(int)(fnc.divdec(cc1.Length - 1, reallenth) * (h - realstart))];
                    }

                    if (numoffill == 0)
                    {
                        numofempty = spacingtype[crntspacetype][1];
                        numoffill = spacingtype[crntspacetype][0];
                        crntspacetype += 1;
                        if (crntspacetype > spacingtype.Length - 1)
                        { crntspacetype = 0; }
                    }




                    if (numofempty != 0)
                    {
                        numofempty -= 1;
                    }
                    else
                    {

                        numoffill -= 1;

                        if (numoffill == width)
                        {
                            if (enablecut)
                            {
                                if (enablecutedit)
                                {
                                    Graphics g = Graphics.FromImage(editedimage);
                                    //     g.DrawLine(new Pen(Color.Black), 0, v.Y, editedimage.Width, v.Y);

                                    int id = h + width;
                                    if (id < pnts.Length)
                                    {

                                        PointF lastcutpoint = pnts[id];
                                        SizeF sizeofrotatecutrec = new SizeF(Math.Abs(lastcutpoint.X - v.X), Math.Abs(lastcutpoint.Y - v.Y));

                                        //       g.DrawLine(new Pen(Color.Black), 0, lastcutpoint.Y, editedimage.Width, lastcutpoint.Y);
                                        sizeofrotatecutrec.Width = fnc.Getequaterofnext(sizeofrotatecutrec.Width, lineangle);
                                        sizeofrotatecutrec.Height = sizeofrotatecutrec.Width;// fnc.Getequaterofnext(szf.Height, rotatrangle);
                                        SizeF sizeofcutrec = new SizeF((lastcutpoint.X - v.X) / 2, (lastcutpoint.Y - v.Y) / 2);

                                        RectangleF trec = new RectangleF(new PointF(v.X, v.Y), sizeofrotatecutrec);
                                        if (centercut == false)
                                        {
                                            trec.X += sizeofcutrec.Width;
                                            trec.Y += sizeofcutrec.Height;
                                        }
                                        if (cutpath != null)
                                        {

                                            GraphicsPath vb = resizepath(cutpath, trec.Size);
                                            PointF z = new PointF(v.X, v.Y);
                                            if (centercut == true)
                                            {
                                                z.X -= sizeofcutrec.Width;
                                                z.Y -= sizeofcutrec.Height;
                                            }
                                            vb = multiplaypath(vb, z);
                                            vb = rotatepath(vb, lineangle, new Rectangle(Point.Round(z), Size.Round(trec.Size)));

                                            //      g.FillPath(Brushes.Black, vb);

                                            if (specialallclippath != null)
                                            {
                                                innerclippath = specialallclippath;
                                            }
                                            else
                                            {
                                                innerclippath = vb; allinnerpath.AddPath(vb, true);
                                            }

                                            a = rr.path;
                                        }
                                        if (cutiamge != null)
                                        {
                                            innerrotateangle = lineangle + cutrotateangle;
                                            innnerrotaterec = trec;
                                            this.drawimage(resizeimage(cutiamge, Size.Round(trec.Size)), trec.Location);
                                            a = rr.img;
                                        }
                                    }
                                }
                                else if (enablecutedit == false)
                                {
                                    a = rr.none;
                                }
                            }
                        }

                        else if (numoffill > width || width > spacingtype[crntspacetype][0])
                        {
                            a = rr.norm;
                        }
                        if (a == rr.img)
                        {
                            v = rotatepoint(v, innnerrotaterec, innerrotateangle, innnerrotaterec.Location, true);
                            if (innerclippath != null)
                            {
                                if (innerclippath.IsVisible(v))
                                {
                                    SetPixel(v, c);
                                }
                            }
                            else
                            {
                                SetPixel(v, c);
                            }
                        }
                        else if (a == rr.path)
                        {
                            if (innerclippath != null)
                            {
                                if (innerclippath.IsVisible(v))
                                {
                                    SetPixel(v, c);
                                }
                            }
                            else
                            {
                                SetPixel(v, c);
                            }
                        }
                        else if (a == rr.norm)
                        {

                            SetPixel(v, c);
                            for (int w = 0; w <= width; w++)
                            {
                                if (cf.Bottom)
                                { SetPixel(v.X, v.Y + w, c); }
                                if (cf.Left)
                                { SetPixel(v.X - w, v.Y, c); }
                                if (cf.Right)
                                { SetPixel(v.X + w, v.Y, c); }
                                if (cf.Top)
                                { SetPixel(v.X, v.Y - w, c); }
                                if (ef.BottomLeft)
                                { SetPixel(v.X - w, v.Y + w, c); }
                                if (ef.BottomRight)
                                { SetPixel(v.X + w, v.Y + w, c); }
                                if (ef.TopLeft)
                                { SetPixel(v.X - w, v.Y - w, c); }
                                if (ef.TopRight)
                                { SetPixel(v.X + w, v.Y - w, c); }
                            }
                        }


                    }
                }
            }

            //====================
            //++++++++++++++++++++++++
            //__________________

            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);



                int start = (int)(cb.Positions[i] * pnts.Length);


                int lenth = (int)(cb.Positions[i + 1] * pnts.Length) - start;

                for (int h = start; h < start + lenth; h++)
                {
                    PointF v = pnts[h];
                    Color c = cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))];

                    if (h > oo)
                    { break; }
                    if (numoffill == 0)
                    {
                        numofempty = spacingtype[crntspacetype][1];
                        numoffill = spacingtype[crntspacetype][0];
                        crntspacetype += 1;
                        if (crntspacetype > spacingtype.Length - 1)
                        { crntspacetype = 0; }


                    }
                    int temprotateangl = rotatrangle;
                    bool tempcenterrotate = centerrotate;

                    if (numofempty != 0)
                    {
                        numofempty -= 1;
                    }
                    else
                    {

                        numoffill -= 1;
                        if (numoffill == width)
                        {

                            if (enablecut)
                            {
                                if (enablecutedit)
                                {
                                    Graphics g = Graphics.FromImage(editedimage);
                                    //g.DrawLine(new Pen(Color.Black), 0, v.Y, editedimage.Width, v.Y);


                                    int id = h + width;
                                    if (id < pnts.Length)
                                    {

                                        if (lineangle == 360) { lineangle = 0; }
                                        PointF lastcutpoint = pnts[id];
                                        SizeF sizeofrotatecutrec = new SizeF(Math.Abs(lastcutpoint.X - v.X), Math.Abs(lastcutpoint.Y - v.Y));

                                        //g.DrawLine(new Pen(Color.Black), 0, lastcutpoint.Y, editedimage.Width, lastcutpoint.Y);
                                        sizeofrotatecutrec.Width = fnc.Getequaterofnext(sizeofrotatecutrec.Width, lineangle);
                                        sizeofrotatecutrec.Height = sizeofrotatecutrec.Width;// fnc.Getequaterofnext(szf.Height, rotatrangle);
                                        SizeF sizeofcutrec = new SizeF((lastcutpoint.X - v.X) / 2, (lastcutpoint.Y - v.Y) / 2);

                                        Rectangle trec = new Rectangle(new Point((int)(v.X), (int)(v.Y)), Size.Round(sizeofrotatecutrec));
                                        if (centercut == false)
                                        {
                                            trec.X += (int)sizeofcutrec.Width;
                                            trec.Y += (int)sizeofcutrec.Height;
                                        }
                                        if (cutpath != null)
                                        {
                                            GraphicsPath vb = resizepath(cutpath, trec.Size);
                                            PointF z = new PointF(v.X, v.Y);
                                            if (centercut == true)
                                            {
                                                z.X -= sizeofcutrec.Width;
                                                z.Y -= sizeofcutrec.Height;
                                            }
                                            vb = multiplaypath(vb, z);
                                            vb = rotatepath(vb, lineangle + cutrotateangle, new Rectangle(Point.Round(z), trec.Size));

                                            //  g.FillPath(Brushes.Black,vb);
                                            if (specialallclippath != null)
                                            { innerclippath = specialallclippath; }
                                            else
                                            {
                                                innerclippath = vb;
                                                allinnerpath.AddPath(vb, true);
                                            }
                                            a = rr.path;
                                        }
                                        if (cutiamge != null)
                                        {
                                            innerrotateangle = lineangle + cutrotateangle;
                                            innnerrotaterec = trec;
                                            a = rr.img;
                                            this.drawimage(resizeimage(cutiamge, trec.Size), trec.Location);
                                        }
                                    }
                                }
                                else if (enablecutedit == false)
                                {
                                    a = rr.none;
                                }
                            }

                        }

                        else if (numoffill > width || width > spacingtype[crntspacetype][0])
                        {

                            a = rr.norm;

                        }
                        if (a == rr.img)
                        {
                            v = rotatepoint(v, innnerrotaterec, innerrotateangle, innnerrotaterec.Location, true);
                            if (innerclippath != null)
                            {
                                if (innerclippath.IsVisible(v))
                                {
                                    SetPixel(v, c);
                                }
                            }
                            else
                            {
                                SetPixel(v, c);
                            }
                        }
                        else if (a == rr.path)
                        {
                            if (innerclippath != null)
                            {
                                if (innerclippath.IsVisible(v))
                                {
                                    SetPixel(v, c);
                                }
                            }
                            else
                            {
                                SetPixel(v, c);
                            }
                        }
                        else if (a == rr.norm)
                        {

                            SetPixel(v, c);
                            for (int w = 0; w <= width; w++)
                            {
                                if (cf.Bottom)
                                { SetPixel(v.X, v.Y + w, c); }
                                if (cf.Left)
                                { SetPixel(v.X - w, v.Y, c); }
                                if (cf.Right)
                                { SetPixel(v.X + w, v.Y, c); }
                                if (cf.Top)
                                { SetPixel(v.X, v.Y - w, c); }
                                if (ef.BottomLeft)
                                { SetPixel(v.X - w, v.Y + w, c); }
                                if (ef.BottomRight)
                                { SetPixel(v.X + w, v.Y + w, c); }
                                if (ef.TopLeft)
                                { SetPixel(v.X - w, v.Y - w, c); }
                                if (ef.TopRight)
                                { SetPixel(v.X + w, v.Y - w, c); }
                            }
                        }
                    }
                }
            }
            return allinnerpath;
        }
        private void drawlines(PointF[] lns, KColorBlend cb, int width, dublcorners dc, int[][] spacingtype, float startspacingtype)
        {
            int totaltpnts = 0;
            PointF[][] Alllinepoints = new PointF[lns.Length][];

            for (int i = 0; i < lns.Length; i++)
            {
                if (i == lns.Length - 1)
                {
                    Alllinepoints[lns.Length - 1] = Getlinepoints(lns[lns.Length - 1], lns[0]);
                    totaltpnts += Alllinepoints[lns.Length - 1].Length;
                }
                else
                {
                    Alllinepoints[i] = Getlinepoints(lns[i], lns[i + 1]);
                    totaltpnts += Alllinepoints[i].Length;
                }
            }
            int oo = (int)(startspacingtype * totaltpnts);
            CornersFilter cf = dc.Cornersfilter;
            RectangeEdgeFilter ef = dc.Edgefilter;


            int numofempty = 0;
            int numoffill = 0;
            if (oo >= spacingtype[0][1])
            {
                //      numofempty = spacingtype[0][1];
                //      if (oo - numofempty > 0)
                //        { numoffill = oo - numofempty; }
            }
            else
            {
                //       numofempty = oo;
            }


            int crntspacetype = 0;
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);



                int start = (int)(cb.Positions[i] * totaltpnts);
                int realstart = 0;
                int reallenth = 0;
                bool fr = false;
                if (start < oo)
                {
                    fr = true;
                    realstart = start;
                    reallenth = (int)(cb.Positions[i + 1] * totaltpnts) - realstart;
                    start += oo - start;
                }
                int lenth = (int)(cb.Positions[i + 1] * totaltpnts) - start;
                for (int h = start; h < start + lenth; h++)
                {
                    PointF v = getpointoutofarray(Alllinepoints, h);
                    Color c = cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))];
                    if (fr)
                    {
                        c = cc1[(int)(fnc.divdec(cc1.Length - 1, reallenth) * (h - realstart))];
                    }

                    if (numofempty != 0)
                    {
                        numofempty -= 1;
                    }
                    else
                    {
                        if (numoffill == 0)
                        {
                            numofempty = spacingtype[crntspacetype][1];
                            numoffill = spacingtype[crntspacetype][0];
                            crntspacetype += 1;
                            if (crntspacetype > spacingtype.Length - 1)
                            { crntspacetype = 0; }


                        }
                        numoffill -= 1;
                        SetPixel(v, c);
                        for (int w = 0; w <= width; w++)
                        {
                            if (cf.Bottom)
                            {
                                SetPixel(v.X, v.Y + w, c);
                            }
                            if (cf.Left)
                            { SetPixel(v.X - w, v.Y, c); }
                            if (cf.Right)
                            { SetPixel(v.X + w, v.Y, c); }
                            if (cf.Top)
                            { SetPixel(v.X, v.Y - w, c); }
                            if (ef.BottomLeft)
                            { SetPixel(v.X - w, v.Y + w, c); }
                            if (ef.BottomRight)
                            { SetPixel(v.X + w, v.Y + w, c); }
                            if (ef.TopLeft)
                            { SetPixel(v.X - w, v.Y - w, c); }
                            if (ef.TopRight)
                            { SetPixel(v.X + w, v.Y - w, c); }


                        }
                    }
                }
            }
            //****************
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);



                int start = (int)(cb.Positions[i] * totaltpnts);

                int lenth = (int)(cb.Positions[i + 1] * totaltpnts) - start;
                for (int h = start; h < start + lenth; h++)
                {
                    PointF v = getpointoutofarray(Alllinepoints, h);
                    Color c = cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))];


                    if (h > oo)
                    { break; }
                    if (numofempty != 0)
                    {
                        numofempty -= 1;
                    }
                    else
                    {
                        if (numoffill == 0)
                        {
                            numofempty = spacingtype[crntspacetype][1];
                            numoffill = spacingtype[crntspacetype][0];
                            crntspacetype += 1;
                            if (crntspacetype > spacingtype.Length - 1)
                            { crntspacetype = 0; }


                        }
                        numoffill -= 1;
                        SetPixel(v, c);
                        for (int w = 0; w <= width; w++)
                        {
                            if (cf.Bottom)
                            {
                                SetPixel(v.X, v.Y + w, c);
                            }
                            if (cf.Left)
                            { SetPixel(v.X - w, v.Y, c); }
                            if (cf.Right)
                            { SetPixel(v.X + w, v.Y, c); }
                            if (cf.Top)
                            { SetPixel(v.X, v.Y - w, c); }
                            if (ef.BottomLeft)
                            { SetPixel(v.X - w, v.Y + w, c); }
                            if (ef.BottomRight)
                            { SetPixel(v.X + w, v.Y + w, c); }
                            if (ef.TopLeft)
                            { SetPixel(v.X - w, v.Y - w, c); }
                            if (ef.TopRight)
                            { SetPixel(v.X + w, v.Y - w, c); }


                        }
                    }
                }
            }
        }
      */
        #endregion

    }
    #endregion
 [Serializable()]
    public struct RectangleF3D
    {
        public RectangleF To2D()
        { return new RectangleF(this.X, this.Y, Width, Height); }
        public float X
        {
            get { return Location.X; }
            set
            {
                Location.X = value;
            }
        }
        public float Y
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }
   

        public float Z
        {
             get { return Location.Z; }
            set { Location.Z = value; }
        }
        public float Width
        {  get { return this.Size.Width; }
            set { this.Size.Width = value; }
        } 
        public float Height  
        {  get { return this.Size.Height; }
            set { this.Size.Height = value; }
        }
        public float Thick
        {
            get { return this.Size.Thick; }
            set { this.Size.Thick = value; }
        } 
        public RectangleF3D(float x,float y,float z,float width,float height,float thick)
        {
            this.Size = new SizeF3D(width, height, thick);
            this.Location = new PointF3D(x, y, z);
        }
        public SizeF3D Size;
        public PointF3D Location;
        public RectangleF3D(PointF3D loc,SizeF3D size)
        {
            this.Size = size;
   this.Location=loc;
        }
        public RectangleF3D( Vector3 loc,SizeF3D size)
        {
            this.Size = size;
            this.Location = new PointF3D(loc.X, loc.Y, loc.Z);
        }
        public void Multiplay(PointF3D loc,SizeF3D sz)
        {
            this.Location.Multiplay(loc);
            this.Size.Multiplay(sz);
        }
        public override string ToString()
        {
            return "location " + Location.ToString() + ", Size " + Size.ToString();
        }
       /* public static RectangleF3D operator +(RectangleF3D v1, RectangleF3D v2)
        {
            return new RectangleF3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static RectangleF3D operator +(Vector3D v1, float v2)
        {
            return new RectangleF3D(v1.X + v2, v1.Y + v2, v1.Z + v2);
        }
        public static RectangleF3D operator -(RectangleF3D v1, RectangleF3D v2)
        {
            return new RectangleF3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static RectangleF3D operator *(RectangleF3D v1, RectangleF3D v2)
        {
            return new RectangleF3D(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static RectangleF3D operator *(RectangleF3D v1, float v)
        {
            return new RectangleF3D(v1.X * v, v1.Y * v, v1.Z * v);
        }
        public static RectangleF3D operator *(float v, RectangleF3D v1)
        {
            return new RectangleF3D(v1.X * v, v1.Y * v, v1.Z * v);
        }
        public static RectangleF3D operator /(RectangleF3D v1, RectangleF3D v2)
        {
            return new RectangleF3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }
        public static Vector3D operator /(RectangleF3D v1, float v)
        {
            return new RectangleF3D(v1.X / v, v1.Y / v, v1.Z / v);
        }
        */
    }
    [Serializable()]
    public struct  SizeF3D
    {
       
        public float Width ;
        public float Height ;
        public float Thick ;
        public SizeF3D( float width, float height, float thick)
        {         
            this.Width = width; this.Height = height; this.Thick = thick;
        }
        public override string ToString()
        {
            return string.Format("Width={0},Height={1},Thick={2}", new object[3] {Width, Height, Thick });
        }
        public void Multiplay(SizeF3D s)
        {
            this.Width += s.Width; this.Height += s.Height; this.Thick += s.Thick;
        }
        public static SizeF3D operator *(SizeF3D v1, float v2)
        {
            return new SizeF3D(v1.Width * v2, v1.Height * v2, v1.Thick * v2);
        }

    }
    [Serializable()]
    public struct PointF3D
    {
        public Vector3 ToV()
        { return new Vector3(X, Y, Z); }
        public float X;
        public float Y;
        public float Z;
        public PointF3D(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public void Multiplay(PointF3D p)
        {
            this.X += p.X; this.Y += p.Y; this.Z += p.Z;
        }
        public void Multiplay(float x,float  y,float z)
        {
            this.X += x; this.Y +=y; this.Z +=z;
        }
        public override string ToString()
        {
            return string.Format("X={0},Y={1},Z={2}", new object[3] { X, Y, Z });
        }

        public static PointF3D operator +(PointF3D v1, PointF3D v2)
        {
            return new PointF3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static PointF3D operator +(PointF3D v1, float v2)
        {
            return new PointF3D(v1.X + v2, v1.Y + v2, v1.Z + v2);
        }
        public static PointF3D operator -(PointF3D v1, PointF3D v2)
        {
            return new PointF3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static PointF3D operator -(PointF3D v1, float v2)
        {
            return new PointF3D(v1.X - v2, v1.Y - v2, v1.Z - v2);
        }
        public static PointF3D operator +(PointF3D v1, SizeF3D v2)
        {
            return new PointF3D(v1.X + v2.Width, v1.Y + v2.Height, v1.Z + v2.Thick);
        }
        public static PointF3D operator -(PointF3D v1, SizeF3D v2)
        {
            return new PointF3D(v1.X- v2.Width, v1.Y - v2.Height, v1.Z - v2.Thick);
        }
        public static explicit operator PointF3D(PointF x)
        {
            return new PointF3D(x.X, x.Y,0);
        }
    }
    #region Kgrid 

    [DefaultEvent("Selecteditemchanged")]
    public class kgrid : System.Windows.Forms.ContainerControl
    {

        #region arrging items

        private bool allowscrolling = true;

        public bool AllowScrolling
        {
            get { return allowscrolling; }
            set { allowscrolling = value; }
        }

        [Browsable(false)]


        Point innerspace = new Point(0, 0);

        public Point InnerSpace
        {
            get { return innerspace; }
            set { innerspace = value; SortItemsLocation(); }
        }
        public bool canscroll;
        public void SortItemsLocation(int xx, Rectangle Recofxx)
        {
            largestitemsloc = new Point(0, 0);
            for (int i = 0; i < this.realcontrols.Count; i++)
            {
                if (this.Controls.Contains(this.realcontrols[i]))
                {

                    if (i != xx)
                    {
                        SortItemLocation(i, xx, Recofxx);
                    }
                }
            }
        }
        public void SortItemsLocation()
        {
            largestitemsloc = new Point(0, 0);
            for (int i = 0; i < this.realcontrols.Count; i++)
            {
                if (this.Controls.Contains(this.realcontrols[i]))
                {

                    SortItemLocation(i);
                }
            }
        }
        Size scrollarea = new Size();
        private Orientation scrolldirction = Orientation.Vertical;
        [DefaultValue(Orientation.Vertical)]
        public Orientation ScrollDirection
        {
            get { return scrolldirction; }
            set
            {
                scrolledloc = false;
                canscroll = false;
                scrolldirction = value;
                rightscroll.OrientationDirection = value;

                if (scrolldirction == Orientation.Vertical)
                {
                    rightscroll.Size = new Size(20, this.Height);
                    rightscroll.Location = new Point(this.Width - rightscroll.Width, 0);
                }
                else
                {
                    rightscroll.Size = new Size(this.Width, 20);
                    rightscroll.Location = new Point(0, this.Height - rightscroll.Height);

                }

                SortItemsLocation();

            }
        }

        Point largestitemsloc = new Point(0, 0);

        /*      void measurescrollsize()
              {
                  Point largestloc = new Point(0,0);
                  for (int i = 0; i < this.items.Count; i++)
                  {
                      Control c=items[i];
                      if (c.Location.Y + c.Height + innerspace.Y > largestitemsloc.Y)
                      {
                          largestitemsloc.Y = c.Location.Y + c.Height + innerspace.Y;

                      }
                            if (c.Location.X > largestitemsloc.X)
                      { largestitemsloc.X = c.Location.X; }
                  }
                  largestitemsloc = largestloc;
              }
         */
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            SortItemsLocation();
        }
        bool duringsortcorrect = false;
        public void SortItemLocation(int i)
        { SortItemLocation(i, -1, Rectangle.Empty); }
        public void SortItemLocation(int i, int epect, Rectangle RecOfExpect)
        {
            if (i == epect)
            { return; }
            Control c = realcontrols[i];
            Point scorllloc = new Point();
            Point itemloc = new Point();

            if (scrolldirction == Orientation.Vertical)
            {
                if (canscroll)
                { scorllloc.X = rightscroll.Width; }

                if (i > 0)
                {
                    Rectangle prerec = new Rectangle(this.realcontrols[i - 1].Location, this.realcontrols[i - 1].Size);

                    if (i - 1 == epect)
                    { prerec = RecOfExpect; }
                    if (prerec.X + prerec.Width + c.Width + innerspace.X < this.Width - scorllloc.X)
                    {
                        itemloc = new Point(prerec.X + prerec.Width + innerspace.X, prerec.Y);
                    }
                    else
                    {
                        int heighst = 0;
                        for (int x = 0; x < realcontrols.Count; x++)
                        {
                            if (x != epect)
                            {
                                if (realcontrols[x].Location.Y == prerec.Y)
                                {
                                    if (realcontrols[x].Height > heighst)
                                    {
                                        heighst = realcontrols[x].Height;
                                    }

                                }
                            }
                        }
                        if (i - 1 == epect)
                        { heighst = prerec.Height; }
                        itemloc = new Point(innerspace.X, prerec.Y + heighst + innerspace.Y);

                    }
                }
                else if (i == 0)
                {
                    itemloc = innerspace;
                }

                if (itemloc.Y + c.Height + innerspace.Y > largestitemsloc.Y)
                {
                    largestitemsloc.Y = itemloc.Y + c.Height + innerspace.Y;





                    if (largestitemsloc.Y > this.Height)
                    {
                        scrollarea.Height = largestitemsloc.Y - this.Height;
                        int moversz = largestitemsloc.Y;
                        moversz = (int)((fnc.divdec(this.Height, moversz)) * this.Height);



                        if (moversz < 10)
                        {
                            moversz = 10;
                        }
                        if (moversz > rightscroll.Height)
                        {

                            moversz = rightscroll.Height - 1;
                        }

                        rightscroll.MoverHeight = moversz;
                        if (this.Controls.Contains(rightscroll) == false)
                        {

                            this.Controls.Add(rightscroll);
                            refreshscaleproperties();
                        }
                        rightscroll.Visible = true;
                        if (canscroll == false)
                        {
                            canscroll = true;

                            if (scrolledloc == false && allowscrolling)
                            {
                                rightscroll.Visible = true;
                                scrolledloc = true;
                                duringsortcorrect = true;
                                SortItemsLocation();
                                duringsortcorrect = false;

                            }
                        }
                    }
                    else if (largestitemsloc.Y <= this.Height && duringsortcorrect == false)
                    {
                        rightscroll.Visible = false;
                        canscroll = false;
                    }
                }
                else if (largestitemsloc.Y <= this.Height && duringsortcorrect == false)
                {

                    rightscroll.Visible = false;
                    canscroll = false;

                }

            }
            //Horizontal ==================================================
            else if (scrolldirction == Orientation.Horizontal)
            {

                if (canscroll)
                { scorllloc.Y = rightscroll.Height; }
                if (i > 0)
                {
                    Rectangle prerec = new Rectangle(this.realcontrols[i - 1].Location, this.realcontrols[i - 1].Size);

                    if (i - 1 == epect)
                    { prerec = RecOfExpect; }

                    if (prerec.Location.Y + prerec.Height + c.Height + innerspace.Y < this.Height - scorllloc.Y)
                    {
                        itemloc = new Point(prerec.Location.X, prerec.Location.Y + prerec.Height + innerspace.Y);
                    }
                    else
                    {

                        int widest = 0;
                        for (int x = 0; x < realcontrols.Count; x++)
                        {
                            if (realcontrols[x].Location.X == prerec.Location.X)
                            {
                                if (realcontrols[x].Width > widest)
                                {
                                    widest = realcontrols[x].Width;
                                }

                            }
                        }
                        if (i - 1 == epect)
                        { widest = prerec.Width; }

                        itemloc = new Point(prerec.Location.X + widest + innerspace.X, innerspace.Y);


                    }
                }
                else if (i == 0) { itemloc = innerspace; }

                if (itemloc.X + c.Width + innerspace.X > largestitemsloc.X)
                {
                    largestitemsloc.X = itemloc.X + c.Width + innerspace.X;





                    if (largestitemsloc.X > this.Width)
                    {
                        scrollarea.Width = largestitemsloc.X - this.Width;
                        int moversz = largestitemsloc.X;
                        moversz = (int)((fnc.divdec(this.Width, moversz)) * this.Width);



                        if (moversz < 10)
                        {
                            moversz = 10;
                        }
                        if (moversz > rightscroll.Width)
                        {

                            moversz = rightscroll.Width - 1;
                        }

                        rightscroll.Moverwidth = moversz;
                        if (this.Controls.Contains(rightscroll) == false)
                        {

                            this.Controls.Add(rightscroll);
                            refreshscaleproperties();
                        }
                        rightscroll.Visible = true;
                        if (canscroll == false)
                        {
                            canscroll = true;

                            if (scrolledloc == false && allowscrolling)
                            {
                                rightscroll.Visible = true;
                                scrolledloc = true;
                                duringsortcorrect = true;
                                SortItemsLocation();
                                duringsortcorrect = false;

                            }
                        }
                    }
                    else if (largestitemsloc.X <= this.Width && duringsortcorrect == false)
                    {
                        if (duringsortcorrect == false)
                        {
                            rightscroll.Visible = false;
                            canscroll = false;
                        }
                    }
                }
                else if (largestitemsloc.X <= this.Width && duringsortcorrect == false)
                {
                    if (duringsortcorrect == false)
                    {
                        rightscroll.Visible = false;
                        canscroll = false;
                    }
                }
                //***********
            }



            locatinchangedbykgrid = true;
            reallocs[i] = itemloc;
            c.Location = itemloc;
            locatinchangedbykgrid = false;


        }
        bool locatinchangedbykgrid = false;
        bool scrolledloc = false;
        public kscale rightscroll;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public kscale Rightscroll
        {
            get { return rightscroll; }
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (canscroll == false) return;
            items[0].Text = (e.Delta + "  " + e.Clicks);
            int sgn = Math.Sign(e.Delta);
            int d = -1 * sgn * 5;
            float v = rightscroll.Value + d;

            if (v >= rightscroll.Minimum && v <= rightscroll.Maxmum)
            { }
            else
            {
                if (sgn == 1)
                {
                    v = rightscroll.Minimum;
                }
                else { v = rightscroll.Maxmum; }
            }
            rightscroll.ValueF = v;
            rightscroll.OnValueChanging(new Kscaleeventargs((int)v));
        }

        private void rightscroll_valuechanging(object sender, Kscaleeventargs e)
        {
            foreach (Control c in this.realcontrols)
            {

                locatinchangedbykgrid = true;
                if (scrolldirction == Orientation.Vertical)
                {

                    c.Location = new Point(c.Location.X, reallocs[realcontrols.IndexOf(c)].Y - (int)(fnc.divdec(e.Value, rightscroll.Maxmum) * scrollarea.Height));

                }
                else
                {
                    c.Location = new Point(reallocs[realcontrols.IndexOf(c)].X - (int)(fnc.divdec(e.Value, rightscroll.Maxmum) * scrollarea.Width), c.Location.Y);


                }
                locatinchangedbykgrid = false;

            }
            this.Invalidate();
        }

        void refreshscaleproperties()
        {
            rightscroll.OrientationDirection = scrolldirction;
            rightscroll.ValueChanging -= new Kscaleeventhandler(rightscroll_valuechanging);
            rightscroll.ValueChanging += new Kscaleeventhandler(rightscroll_valuechanging);
            rightscroll.Rectangleedegfilter = new RectangeEdgeFilter(false, false, false, false);
            rightscroll.Style = KscalePaintMode.Flat;
            rightscroll.Visible = false;
            rightscroll.AreaMargin = new Point(0, 100);
            rightscroll.Theme = KscaleThemes.Dark;
            rightscroll.Fillselectedarea = false;
            rightscroll.BringToFront();
        }

        #endregion

        bool autosortafterremoveconreol = false;
        [DefaultValue(false)]
        public bool AutoSortAfterRemoveControl
        {
            get { return autosortafterremoveconreol; }
            set { autosortafterremoveconreol = value; }
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);


            if (e.Control is kgriditem)
            {

                kgriditem kgi = (kgriditem)e.Control;
                if (kgi.Index == this.selectedindex)
                { this.selectedindex = -1; }


            }
            if (realcontrols.Contains(e.Control))
            {
                e.Control.LocationChanged -= new EventHandler(Control_LocationChanged);
                realcontrols.Remove(e.Control);
                reallocs.Remove(e.Control.Location);
            }
            if (autosortafterremoveconreol)
            { SortItemsLocation(); }

        }
        public List<Control> notmovingcontrols = new List<Control>();
        public List<Control> realcontrols = new List<Control>();

        List<Point> reallocs = new List<Point>();
        private void Control_LocationChanged(object sender, EventArgs e)
        {

            if (locatinchangedbykgrid == false)
            {
                if (this.DesignMode)
                {
                    Control cntrl = ((Control)sender);
                    Control overcntrl = null;
                    int indexofsecond = indexofchild(cntrl.Location, realcontrols.IndexOf(cntrl));
                    if (indexofsecond != -1)
                    {

                        overcntrl = realcontrols[indexofsecond];

                        recof2 = new Rectangle(overcntrl.Location, overcntrl.Size);
                        int d1 = this.Controls.IndexOf(cntrl);
                        int d2 = this.Controls.IndexOf(overcntrl);
                        if (d1 != -1 && d2 != -1)
                        {

                            this.Controls.SetChildIndex(this.Controls[d1], d2);

                            UpdateRealControls();



                        }
                    }


                    this.SortItemsLocation();
                }
                else
                {
                    int dx = realcontrols.IndexOf(((Control)sender));
                    this.SortItemLocation(dx);
                }
            }

        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (notmovingcontrols.Contains(e.Control) == false)
            {

                realcontrols.Add(e.Control);
                e.Control.LocationChanged += new EventHandler(Control_LocationChanged);
                e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
                e.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
                e.Control.MouseUp += new MouseEventHandler(Control_MouseUp);

                reallocs.Add(e.Control.Location);
                SortItemLocation(realcontrols.IndexOf(e.Control));
            }
            if (e.Control is kgriditem)
            {
                kgriditem kgi = (kgriditem)e.Control;
                if (kgi.Index == -1)
                {

                    this.Items.Add(kgi);
                }
            }
        }




        Boolean ndragr;
        int nmousexr;
        int nmouseyr;
        Form dsd = new Form();
        private void button11_Click(object sender, EventArgs e)
        {
            if (dsd.IsDisposed)
            { dsd = new Form(); }
            Control cntrl = ((Control)sender);
            dsd.BackColor = Color.Green;
            dsd.FormBorderStyle = FormBorderStyle.None;
            dsd.AllowTransparency = true;
            dsd.Opacity = 0.4;
            Point loc = new Point();
            Control cont = ((Control)sender);
            loc = cont.PointToScreen(Point.Empty);
            dsd.Location = loc;
            dsd.StartPosition = FormStartPosition.Manual;
            Bitmap bbb = new Bitmap(cont.Width, cont.Height);
            cont.DrawToBitmap(bbb, new Rectangle(0, 0, cont.Width, cont.Height));
            dsd.BackgroundImageLayout = ImageLayout.Stretch;
            dsd.BackgroundImage = bbb;
            //     dsd.MouseMove += new MouseEventHandler(frm_MouseMove);
            //      dsd.MouseUp += new MouseEventHandler(frm_MouseUp);            
            dsd.Size = cntrl.Size;
            dsd.ShowDialog();
        }



        public int indexoffirst = -1;
        public int indexofsecond = -1;
        public int indexofpre = -1;
        Point dwnx;
        Control downc;
        private bool customzing = false;
        [DefaultValue(false), Category("Behavior")]
        public bool Customzing
        {
            get { return customzing; }
            set { customzing = value; }
        }
        //  bool doarangeeffect = false;
        public void UpdateRealControls()
        {
            List<Control> v = new List<Control>();
            for (int i = 0; i < Controls.Count; i++)
            {
                if (notmovingcontrols.Contains(Controls[i]) == false)
                {
                    v.Add(Controls[i]);
                }
            }
            realcontrols.Clear();
            realcontrols = v;
        }
        public int indexofchild(Point p)
        {
            return indexofchild(p, -1);

        }
        public int indexofchild(Point p, int ecpected)
        {
            int v = -1;
            for (int i = 0; i < this.realcontrols.Count; i++)
            {
                if (i != ecpected)
                {
                    Control c = realcontrols[i];
                    Rectangle rec = new Rectangle(c.Location, c.Size);
                    if (rec.Contains(p))
                    { v = i; return v; }
                }
            }
            return -1;

        }
        public bool OnControlMoving = false;
        Rectangle recof2 = new Rectangle();
        bool cntrlchanged = false;
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (customzing)
            {
                ndragr = true;
                OnControlMoving = false;
                Control cntrl = ((Control)sender);
                downc = cntrl;
                indexoffirst = this.realcontrols.IndexOf(cntrl);
                dwnx = e.Location;
                nmousexr = MousePosition.X - cntrl.Left;
                nmouseyr = MousePosition.Y - cntrl.Top;
                foreach (Control c in this.realcontrols)
                { c.SendToBack(); }
                cntrl.BringToFront();
            }   //    rightscroll.BringToFront();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            cntrlchanged = false;

            if (ndragr == true && ((Control)sender) == downc)
            {
                Point cloc = new Point(MousePosition.X - nmousexr, MousePosition.Y - nmouseyr);
                if (OnControlMoving == false)
                {
                    if (Math.Abs(e.Location.X - dwnx.X) <= 5 & Math.Abs(e.Location.Y - dwnx.Y) <= 5)
                    { OnControlMoving = false; }
                    else
                    { OnControlMoving = true; }
                }
                if (OnControlMoving == false)
                { return; }
                this.locatinchangedbykgrid = true;

                Control cntrl = ((Control)sender);
                Control overcntrl = null;


                if (cloc.X < 0)
                { cloc.X = 0; }
                else if (cloc.X + cntrl.Width > this.Width)
                { cloc.X = this.Width - cntrl.Width; }

                if (cloc.Y < 0)
                { cloc.Y = 0; }
                else if (cloc.Y + cntrl.Height > this.Height)
                { cloc.Y = this.Height - cntrl.Height; }
                cntrl.Location = cloc;

                //  rightscroll.BringToFront();
                if (cntrl is kgriditem && ((kgriditem)cntrl).domainarrngeeffect == false)
                {

                    cntrl.Cursor = Cursors.SizeAll;
                    //    ((kgriditem)cntrl).domainarrngeeffect = true; cntrl.Invalidate();
                }


                int temp2 = indexofsecond;
                int indxnow = indexofchild(new Point(cloc.X + dwnx.X, cloc.Y + dwnx.Y), realcontrols.IndexOf(((Control)sender)));
                if (indxnow != -1)
                { } indexofsecond = indxnow;
                if (temp2 != indexofsecond)
                { cntrlchanged = true; indexofpre = temp2; }
                if (indexofpre >= 0 && indexofpre < realcontrols.Count)
                {
                    // realcontrols[indexofpre].Invalidate();
                }

                if (indexofsecond != -1)
                {

                    overcntrl = realcontrols[indexofsecond];
                    //   overcntrl.Invalidate();
                    if (cntrlchanged)
                    {
                        recof2 = new Rectangle(overcntrl.Location, overcntrl.Size);
                    }
                }



                if (indexoffirst != -1 && indexofsecond != -1 && cntrlchanged)
                {
                    int d1 = this.Controls.IndexOf(cntrl);
                    int d2 = this.Controls.IndexOf(overcntrl);
                    if (d1 != -1 && d2 != -1)
                    {

                        this.Controls.SetChildIndex(this.Controls[d1], d2);

                        UpdateRealControls();

                        this.SortItemsLocation(realcontrols.IndexOf(cntrl), recof2);

                    }
                }

            }
            this.locatinchangedbykgrid = false;

        }
        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (customzing == false)
            { return; }
            Control cntrl = ((Control)sender);

            if (OnControlMoving && ((Control)sender) == downc)
            {


                this.SortItemsLocation();
                if (canscroll)
                {
                    int tempv = rightscroll.Value;


                    rightscroll.Value = tempv;
                    rightscroll.OnValueChanging(new Kscaleeventargs(rightscroll.Value));
                }
                if (cntrl is kgriditem)
                {
                    ((kgriditem)cntrl).domainarrngeeffect = false;

                }
                cntrl.Cursor = Cursors.Default;
                cntrl.Invalidate();
            }
            OnCustomzed(new EventArgs());
            ndragr = false; downc = null;
        }
        private void Control_Paint(object sender, PaintEventArgs e)
        {
            Control cntrl = ((Control)sender);
            if (realcontrols.IndexOf(cntrl) == indexofsecond)
            {
                Bitmap b = new Bitmap(cntrl.Width, cntrl.Height);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.FromArgb(125, 0, 255, 255));
                e.Graphics.DrawImage(b, 0, 0);
            }
            else
            { cntrl.Text = "NO"; }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (scrolldirction == Orientation.Vertical)
            {
                rightscroll.Size = new Size(20, this.Height);
                rightscroll.Location = new Point(this.Width - rightscroll.Width, 0);
            }
            else
            {
                rightscroll.Size = new Size(this.Width, 20);
                rightscroll.Location = new Point(0, this.Height - rightscroll.Height);

            }

            SortItemsLocation();
        }

        public event EventHandler Customzed;
        public void OnCustomzed(EventArgs e)
        {
            if (Customzed != null)
            {
                Customzed(this, e);
            }
        }
        public kgrid()
        {
       
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
            rightscroll = new kscale();
            rightscroll.Pointers.Add(new pointer());
            rightscroll.SelectedIndex = 0;
            rightscroll.OrientationDirection = scrolldirction;
            notmovingcontrols.Add(rightscroll);
            if (this.Controls.Contains(rightscroll) == false)
            {
                this.Controls.Add(rightscroll);
            }
            rightscroll.ValueChanging += new Kscaleeventhandler(rightscroll_valuechanging);
            rightscroll.Rectangleedegfilter = new RectangeEdgeFilter(false, false, false, false);
            rightscroll.Style = KscalePaintMode.Flat;
            rightscroll.Visible = false;
            rightscroll.AreaMargin = new Point(0, 100);
            rightscroll.Theme = KscaleThemes.Dark;
            rightscroll.Fillselectedarea = false;
            rightscroll.BringToFront();
            rightscroll.EndInit();
            this.groups.KGrid = this;
            this.items.KGrid = this;

            this.Multiselect = false;
            this.BackgroundImage = null;
        }

        #region selection
        public Boolean OnSelection
        {
            get
            {
                if (drag)
                {
                    return drag;
                }
                else
                {
                    foreach (kDynamkPanel kgb in groups)
                    {
                        if (kgb.OnSelection)
                            return true;

                    }
                }
                return false;
            }
        }

        private bool mouseselection = false;
        [DefaultValue(false)]
        public bool Mouseselection
        {
            get { return mouseselection; }
            set { mouseselection = value; }
        }
        private bool controlkeyselection = false;

        [DefaultValue(false)]
        public bool ControlKeySelection
        {
            get { return controlkeyselection; }
            set { controlkeyselection = value; }
        }


        bool multiselect = false;
        [DefaultValue(false)]
        public bool Multiselect
        {
            get { return multiselect; }
            set { multiselect = value; }
        }
        private int selectedindex = -1;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap b = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(b);

            g.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
            if (this.Parent != null)
            {
                if (this.Parent is KTabControl)
                {
                    if (this.selecteditem != null)
                    {
                        float lwd = ((KTabControl)this.Parent).LineWidth;
                        g.DrawLine(new Pen(this.selecteditem.SelectedColor, lwd), 0, this.Height - lwd / 2, this.Width, this.Height - lwd / 2);
                    }
                }
            }
            if (BackgroundImage != null)
            {
                int xds = (int)(1 * rightscroll.ValueF / rightscroll.Maxmum * scrollarea.Height);
                Rectangle rec2 = new Rectangle(new Point(0, xds), this.Size);
                g.DrawImage(BackgroundImage, fnc.OImg(BackgroundImage, this.DisplayRectangle, BackgroundImageLayout), rec2, GraphicsUnit.Pixel);
            }
            e.Graphics.DrawImage(b, 0, 0);
        }
        #region Draw selection
        Boolean drag = false;
        Rectangle selctionrec = new Rectangle(0, 0, 0, 0);
        Point startPoint;


        protected override void OnMouseDown(MouseEventArgs e)
        {

            base.OnMouseDown(e);
            if (this.Mouseselection)
            {
                foreach (kgriditem kgi in this.Items)
                {
                    if (kgi.Iselected == true)
                    {
                        kgi.Iselected = false;
                    }
                }
                drag = true;
                startPoint = MousePosition;
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (drag == true)
            {
                ControlPaint.DrawReversibleFrame(selctionrec, Color.Black, FrameStyle.Dashed);
                selctionrec.Location = startPoint;

                Size ssz = (Size)MousePosition;
                if (ssz.Width > this.PointToScreen(Point.Empty).X + this.Width)
                {
                    ssz.Width = this.PointToScreen(Point.Empty).X + this.Width - 1;
                }
                else if (ssz.Width < this.PointToScreen(Point.Empty).X)
                {
                    ssz.Width = this.PointToScreen(Point.Empty).X + 1;
                }
                if (ssz.Height > this.PointToScreen(Point.Empty).Y + this.Height)
                {
                    ssz.Height = this.PointToScreen(Point.Empty).Y + this.Height - 1;
                }
                else if (ssz.Height < this.PointToScreen(Point.Empty).Y)
                {
                    ssz.Height = this.PointToScreen(Point.Empty).Y + 1;
                }
                selctionrec.Size = ssz - (Size)selctionrec.Location;


                ControlPaint.DrawReversibleFrame(selctionrec, Color.Black, FrameStyle.Dashed);
                //     fff.DrawRectangle(new Pen(Color.FromArgb(50,150,75,135),5), selctionrec);


                //
                //   fff.Dispose();
                Rectangle controlRectangle;

                if (selctionrec.Width < 0)
                {
                    selctionrec.X = selctionrec.X + selctionrec.Width;
                    selctionrec.Width = -1 * selctionrec.Width;
                }
                if (selctionrec.Height < 0)
                {
                    selctionrec.Y = selctionrec.Y + selctionrec.Height;
                    selctionrec.Height = -1 * selctionrec.Height;
                }
                for (int i = 0; i < items.Count; i++)
                {
                    controlRectangle = items[i].RectangleToScreen(items[i].ClientRectangle);
                    if (controlRectangle.IntersectsWith(selctionrec))
                    {
                        if (items[i].Iselected == false)
                        { items[i].Iselected = true; }
                    }
                    else
                    { if (items[i].Iselected)items[i].Iselected = false; }
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e); if (drag)
            {
                drag = false;
                selctionrec = new Rectangle(0, 0, 0, 0);
                //  tms = mss.up; 

            }
        }

        #endregion
        [Browsable(false)]
        public int Selectedindex
        {
            get { return selectedindex; }
            set
            {
                selectedindex = value;

                if (value != -1)
                {
                    if (items[value].Iselected == false)
                    { items[value].Iselected = true; }
                    Runselectedindexchangedevent(this, new kgrideventargs(this.items[value]));
                }

            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public kgriditem selecteditem
        {
            get
            {

                if (Selectedindex > -1)
                {

                    return Items[Selectedindex];
                }
                else
                {
                    return null;
                }

            }
            set
            {
                if (value != null)
                {
                    Selectedindex = value.Index;

                }
            }
        }
        public List<kgriditem> selecteditems
        {
            get
            {
                List<kgriditem> selects = new List<kgriditem>();

                foreach (kgriditem c in this.items)
                {
                    if (c.Iselected == true)
                    {
                        selects.Add(c);
                    }
                }

                return selects;
            }

        }
        #endregion
        #region unreng
        bool grouping = true;
        public bool Grouping
        {
            get { return grouping; }
            set
            {
                if (grouping == value)
                { return; }
                else { grouping = value; }
                if (value)
                {
                    List<Control> temp = new List<Control>(realcontrols);

                    this.Controls.Clear();

                    foreach (kgriditem kgi in items)
                    {

                        if (kgi.Group != null)
                        {
                            kgi.Group = kgi.Group;

                        }

                    }
                    foreach (kDynamkPanel kgb in groups)
                    {
                        Controls.Add(kgb);

                    }
                    foreach (Control c in temp)
                    {
                        if (c is kgriditem == false)
                        { this.Controls.Add(c); }

                    }
                    if (autosizegroups)
                    { makegruopssize(); }
                }
                else
                {

                    List<kgriditem> tempitems = new List<kgriditem>(items);

                    List<Control> temp = new List<Control>(realcontrols);

                    this.Controls.Clear();
                    this.Items.Clear();
                    bool addd = true;
                    foreach (kgriditem kgi in tempitems)
                    {
                        kgi.Index = -1;
                        this.Controls.Add(kgi);
                    }
                    foreach (Control c in temp)
                    {
                        addd = true;
                        if (c is kgriditem)
                        { addd = false; }

                        if (c is kDynamkPanel)
                        {
                            if (groups.Contains((kDynamkPanel)c))
                            { addd = false; }
                        }
                        if (addd)
                        {
                            this.Controls.Add(c);
                        }
                    }


                }

            }

        }

        kgridGroupsCollection groups = new kgridGroupsCollection();

        public kgridGroupsCollection Groups { get { return groups; } }


        KgridItemsCollection items = new KgridItemsCollection();
        public KgridItemsCollection Items { get { return items; } }

        bool Autosizegruops = true;
        public bool autosizegroups
        {

            get { return Autosizegruops; }
            set
            {

                Autosizegruops = value;
                if (value)
                { makegruopssize(); }

            }
        }
        public void makegruopssize()
        {
            foreach (kDynamkPanel thekgb in Groups)
            {
                if (thekgb.Controls.Count > 0)
                {
                    thekgb.Width = this.Width - 35;
                    thekgb.Height = thekgb.Controls[thekgb.Controls.Count - 1].Top + thekgb.Controls[thekgb.Controls.Count - 1].Height + 10;
                }
            }
        }



        [Description("Occurs when the selecteditem cahnge"), Browsable(true), Category("Items")]
        public event kgrideventhandler Selecteditemchanged;
        public event kgrideventhandler Itemdoubleclick;


        public enum mss { don, mov, up, non };
        /// <summary>
        /// start the event for the item which dbl clicked
        /// </summary>
        public void OnItemDoubleClick(object sender, kgrideventargs e)
        {
            if (Itemdoubleclick != null)
            {
                Itemdoubleclick(this, e);
            }
        }

        public void Runselectedindexchangedevent(object sender, kgrideventargs e)
        {
            if (Selecteditemchanged != null)
            {
                Selecteditemchanged(this, e);
            }
        }
        public void ClearAll()
        {
            this.items.Clear();
            this.groups.Clear();
            foreach (Control c in this.Controls)
            {
                if (c != rightscroll)
                {
                    this.Controls.Remove(c);
                }
            }

        }

        bool sort = false;
        public bool Sort
        {
            get { return sort; }
            set
            {
                sort = value;
                if (value)
                {
                    DoSort();
                }
            }
        }
        public void DoSort()
        {
            items.Sort((m1, m2) => m1.Text.CompareTo(m2.Text));
            Invalidate();
            /*  kgriditem[] kgic =this.items.ToArray();
                      Array.Sort(kgic);
                  
                      this.items.Clear();
                      foreach (kgriditem kgi in kgic)
                      {
                          this.items.Add(kgi);
                      }
              */
        }
        bool isaccesdingsort = true;
        public bool IsAccesdingSort
        {
            get { return isaccesdingsort; }
            set
            {
                if (value == isaccesdingsort)
                { return; }

                isaccesdingsort = value;
                if (Sort)
                { DoSort(); }


            }
        }

        #endregion


    }
    public delegate void kgrideventhandler(object sender, kgrideventargs e);
    public class kgrideventargs : System.EventArgs
    {


        public kgrideventargs() { }
        public kgrideventargs(kgriditem kgriditem)
        {
            this.item = kgriditem;
        }
        public kgriditem item { get; set; }
    }

    public class kgridGroupsCollection : System.Collections.Generic.List<kDynamkPanel>
    {
        public new void Add(kDynamkPanel value)
        {


            foreach (kDynamkPanel kgb in this)
            {
                if (kgb.Title.ToLower() == value.Title.ToLower())
                {
                    return;
                }
            }

            if (value.Controls.Count > 0 && KGrid.autosizegroups)
            {
                value.Width = value.Width - 35;
                value.Height = value.Controls[value.Controls.Count - 1].Top + value.Controls[value.Controls.Count - 1].Height + 10;

            }
            if (_kgrid.Grouping)
            {
                _kgrid.Controls.Add(value);
            }
            base.Add(value);
        }
        private kgrid _kgrid;
        public kgrid KGrid { get { return _kgrid; } set { _kgrid = value; } }

        public kgridGroupsCollection(kgrid Kgrid) { _kgrid = Kgrid; }
        public kgridGroupsCollection() : this(null) { }

    }
    public class KgridItemsCollection : System.Collections.Generic.List<kgriditem>
    {
        public void UpdateIndex()
        {
            for (int i = 0; i < this.Count; i++)
            { this[i].Index = i; }
        }
        public new void Remove(kgriditem value)
        {
            _kgrid.Controls.Remove(value);
            base.Remove(value);
            UpdateIndex();
        }
        public new void RemoveAt(int f)
        { Remove(this[f]); }
        public new void Clear()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.RemoveAt(i);
            }
        }
        public new void Add(kgriditem value)
        {
            base.Add(value);

            value.Index = this.Count - 1;

            if (_kgrid.Grouping)
            {
                if (value.Group == null) { _kgrid.Controls.Add(value); }

            }
            else
            {
                _kgrid.Controls.Add(value);

            }

        }

        private kgrid _kgrid;
        public kgrid KGrid { get { return _kgrid; } set { _kgrid = value; } }

        public KgridItemsCollection(kgrid Kgrid) { _kgrid = Kgrid; }
        public KgridItemsCollection() : this(null) { }

    }

   
    #endregion

    
    #region fnc && pnt
    public static class pnt
    {


        public static Color[] GetGradientColors(Color c1, Color c2)
        {
            int absR = c2.R - c1.R;
            int absg = c2.G - c1.G;
            int absb = c2.B - c1.B;

            int count = fnc.MaxOfThree(Math.Abs(absR), Math.Abs(absg), Math.Abs(absb));
            if (count == 0) { count = 1; }
            Color[] cols = new Color[count];

            float rateR = fnc.divdec(absR, count);
            float rateG = fnc.divdec(absg, count);
            float rateB = fnc.divdec(absb, count);

            for (int i = 0; i < count; i++)
            {
                int vR = fnc.tint(c1.R + (rateR * i));
                int vG = fnc.tint(c1.G + (rateG * i));
                int vB = fnc.tint(c1.B + (rateB * i));

                cols[i] = Color.FromArgb(vR, vG, vB);

            }
            return cols;
        }
        public static Color[] GetMultiGradientColors(ColorBlend cb)
        {
            List<Color> value = new List<Color>();
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);
                value.AddRange(cc);
            }
            return value.ToArray();
        }
        public static Bitmap DrawRecGradients(ColorBlend cb, Rectangle rec, Bitmap Bmp)
        {


            Color[] cols = GetMultiGradientColors(cb);

            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {

                Color[] cc1 = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);



                float start = rec.Y + (cb.Positions[i] * rec.Height);
                float lenth = ((cb.Positions[i + 1] * rec.Height) - (cb.Positions[i] * rec.Height));
                for (float h = start; h < start + lenth; h++)
                {
                    for (int w = rec.X; w < rec.Right; w++)
                    {
                        Bmp.SetPixel((int)(w), (int)h, cc1[(int)(fnc.divdec(cc1.Length - 1, lenth) * (h - start))]);

                        // this.srcbitmab.SetPixel((int)(w), (int)h, cols[(int)(fnc.divdec(cols.Length - 1, rec.Height) * (h - rec.Y))]);
                    }

                }
            }
            return Bmp;
        }
        public static Bitmap DrawCircleGradients(ColorBlend cb, ColorBlend cbcolmn, PointF TheCenterpoint, int StartAngle, Rectangle rec, Bitmap Bmp)
        {



            PointF nq = new PointF();
            nq.X = (float)(rec.Width / 2F);
            nq.Y = (float)(rec.Height / 2F);
            PointF hertp = new PointF(rec.Width * TheCenterpoint.X, rec.Height * TheCenterpoint.Y);


            Color[] nqcolors = cbcolmn.Colors;
            float[] nqpostions = cbcolmn.Positions;
            for (int i = 0; i < cb.Colors.Length - 1; i++)
            {
                Color[] cc = GetGradientColors(cb.Colors[i], cb.Colors[i + 1]);



                float start = StartAngle + (cb.Positions[i] * 360);
                float lenth = (StartAngle + ((cb.Positions[i + 1] * 360)) - (StartAngle + (cb.Positions[i] * 360)));
                for (float h = start; h < start + lenth; h += 0.5f)
                {

                    int indxof2 = (int)(fnc.divdec(cc.Length - 1, lenth) * ((h) - start));
                    Color ccx = cc[indxof2];

                    // Color[] ccenters = gradycols(cb.Colors[i], cb.Colors[i + 1]);
                    nqcolors[cbcolmn.Positions.Length - 1] = ccx;
                    PointF pp = new PointF();
                    pp.X = (nq.X + nq.X * (float)Math.Cos(fnc.Topi(h)));
                    pp.Y = (nq.Y + nq.Y * (float)Math.Sin(fnc.Topi(h)));

                    float halfnq = (float)Math.Sqrt(Math.Pow(pp.X - hertp.X, 2) + Math.Pow(pp.Y - hertp.Y, 2));
                    // float ang = (float)(Math.Atan((Math.Abs((pp.Y - hertp.Y)) / Math.Abs(pp.X - hertp.X))) * (180 / Math.PI));
                    float ang = (float)(Math.Asin(0.5) * (180 / Math.PI));
                    ang = fnc.correctquart(ang, pp.X - hertp.X, hertp.Y - pp.Y);

                    //======
                    for (int e = 0; e < nqcolors.Length - 1; e++)
                    {


                        //    Color ccenter = cc[(cc.Length - 1) - indxof2];
                        Color[] cols2 = GetGradientColors(nqcolors[e], nqcolors[e + 1]);
                        float estart = 0 + (nqpostions[e] * halfnq);
                        float elenth = ((nqpostions[e + 1] * halfnq) - (nqpostions[e] * halfnq));
                        for (float f = estart; f < estart + elenth; f += 1f)
                        {

                            PointF orginp = new PointF();



                            orginp.X = rec.X + (hertp.X + f * (float)Math.Cos(fnc.Topi(ang)));
                            orginp.Y = rec.Y + (hertp.Y + f * (float)Math.Sin(-1 * fnc.Topi(ang)));





                            int indexof3 = (int)(fnc.divdec(cols2.Length - 1, elenth) * (f - estart));



                            //  ((Bitmap)picdraw.BackgroundImage).SetPixel((int)orginp.X, (int)orginp.Y, ccx);
                            Bmp.SetPixel((int)orginp.X, (int)orginp.Y, cols2[indexof3]);

                        }
                    }
                }

            }

            return Bmp;

        }

    }
    public class MathException : Exception
    {
        public MathException()
        {

        }
        public MathException(string message)
            : base(message)
        {

        }
    }
    public static class fnc
    {
        private static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
{
  float u = 1 - t;
  float tt = t*t;
  float uu = u*u;
  float uuu = uu * u;
  float ttt = tt * t;
 
  Vector3 p = uuu * p0; //first term
  p += 3* uu * t * p1; //second term
  p += 3 * u * tt * p2; //third term
  p += ttt * p3; //fourth term
 
  return p;
}
        public static  Vector3[] GetBezirPoints(Vector3[] vecs)
        {
            List<Vector3> drawingPoints = new List<Vector3>();
            for (int i = 0; i < vecs.Length - 3; i += 3)
            {
                Vector3 p0 = vecs[i];
                Vector3 p1 = vecs[i + 1];
                Vector3 p2 = vecs[i + 2];
                Vector3 p3 = vecs[i + 3];

                if (i == 0) //Only do this for the first endpoint.
                //When i != 0, this coincides with the end
                //point of the previous segment
                {
                    drawingPoints.Add(CalculateBezierPoint(0, p0, p1, p2, p3));
                }
                float SEGMENTS_PER_CURVE =10;
                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    drawingPoints.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
                }
            }

            return drawingPoints.ToArray();
        }
        public static class InterpolateColor
        {
            public static Color AddColor(Color left, int right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R + right;
                g = left.G + right;
                b = left.B + right;
                a = left.A + right;
                return SafeColor(a, r, g, b);
            }
            public static Color SubtractColor(Color left, int right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R - right;
                g = left.G - right;
                b = left.B - right;
                a = left.A - right;
                return SafeColor(a, r, g, b);
            }
            public static Color multiplyColor(Color left, int right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R * right;
                g = left.G * right;
                b = left.B * right;
                a = left.A * right;
                return SafeColor(a, r, g, b);
            }
            public static Color divisionColor(Color left, int right,bool dividealpha=false)
            {
                float r = 0; float a = 0; float g = 0; float b = 0;
                r =(float) left.R /(float) right;
                g =(float) left.G /(float) right;
                b =(float) left.B /(float) right;
                if (dividealpha)
                {
                    a = (float)left.A / (float)right;
                }
                else
                {
                    a = left.A;
                }
                return SafeColor(a, r, g, b);
            }

            public static Color AddColor(Color left, Color right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R + right.R;
                g = left.G + right.G;
                b = left.B + right.B;
                a = left.A + right.A;
                return SafeColor(a, r, g, b);
            }
            public static Color SubtractColor(Color left, Color right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R - right.R;
                g = left.G - right.G;
                b = left.B - right.B;
                a = left.A - right.A;
                return SafeColor(a, r, g, b);
            }
            public static Color multiplyColor(Color left, Color right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R * right.R;
                g = left.G * right.G;
                b = left.B * right.B;
                a = left.A * right.A;
                return SafeColor(a, r, g, b);
            }
            public static Color divisionColor(Color left, Color right)
            {
                int r = 0; int a = 0; int g = 0; int b = 0;
                r = left.R / right.R;
                g = left.G / right.G;
                b = left.B / right.B;
                a = left.A / right.A;
                return SafeColor(a, r, g, b);
            }
            public static Color SafeColor(float a, float r, float g, float b)
            {
                float fcr = r; float fca = a; float fcg = g; float fcb = b;
                if (fcr < 0) { fcr = 0; } else if (fcr > 255) { fcr = 255; }
                if (fcg < 0) { fcg = 0; } else if (fcg > 255) { fcg = 255; }
                if (fcb < 0) { fcb = 0; } else if (fcb > 255) { fcb = 255; }
                if (fca < 0) { fca = 0; } else if (fca > 255) { fca = 255; }
                return Color.FromArgb((int)fca,(int) fcr, (int)fcg,(int) fcb);
            }
        }
            /// <summary>
        /// returns true for (1.02,1.03,1)
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="matchrange"></param>
        /// <returns></returns>
        public static  bool FloatEqual(float f1,float f2,int matchrange=4)
        {
            float s = 1 /(float) (Math.Pow(10,matchrange));
            if (f1+s>f2&&f1-s<f2)
            { return true; }
            return false;
        }
        public static void ColorMathEquation(RichTextBox cnt, string[] vars = null)
        {
            bool wasfocsed = cnt.Focused;
            int selctstart = cnt.SelectionStart;
            int lenth = cnt.SelectionLength;
            cnt.FindForm().Focus();
            string all = cnt.Text;
            cnt.Select(0, all.Length);
            cnt.SelectionColor = System.Drawing.Color.Black;
            cnt.Select(selctstart, lenth);
            cnt.Enabled = false;
            string[] fncs = EquationSolver.fncs;
            for (int i = 0; i < fncs.Length; i++)
            {
                string txt = fncs[i];
                int index = 0;
                while ((index = all.IndexOf(txt, index)) != -1)
                {
                    cnt.Select(index, txt.Length);
                    
                    cnt.SelectionColor = System.Drawing.Color.Red;
                    index += txt.Length;
                  
                }
            }
           
            string[] dfns = EquationSolver.defends;
            for (int i = 0; i < dfns.Length; i++)
            {
                string txt = dfns[i];
                int index = 0;
                while ((index = all.IndexOf(txt, index)) != -1)
                {
                    cnt.Select(index, txt.Length);
                    cnt.SelectionColor = System.Drawing.Color.Blue;
                 
                    index += txt.Length;
                }
            }
            string[] brcks = new string[3] { "(", ")", "|" };
            for (int i = 0; i < brcks.Length; i++)
            {
                string txt = brcks[i];
                int index = 0;
                while ((index = all.IndexOf(txt, index)) != -1)
                {
                    cnt.Select(index, txt.Length);
                    cnt.SelectionColor = System.Drawing.Color.DarkGreen;
                   
                    index += txt.Length;
                }
            }
            if (vars != null)
            {
                for (int i = 0; i < vars.Length; i++)
                {
                    string txt = vars[i];
                    int index = 0;
                    while ((index = all.IndexOf(txt, index)) != -1)
                    {
                        cnt.Select(index, txt.Length);
                        cnt.SelectionColor = System.Drawing.Color.DeepPink;
                       
                        index += txt.Length;
                    }
                }
            } 
            cnt.Enabled = true ;
             cnt.Select(selctstart, lenth);
             if (wasfocsed)
             {
                 cnt.Focus();
             }
        }
        public static T CastK<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(x, value, null);
            }
            return (T)x;
        }
        public static int GetAngle(Point centerpoint, Point pont)
        {
            float yy = pont.Y;
            //yy sohuld be clac as mines if bown and postive if top 
            yy = centerpoint.Y - yy;
            float xx = pont.X;
            xx = xx - centerpoint.X;
            float tn = Math.Abs(yy) / Math.Abs(xx);
            float ang = (float)(Math.Atan(tn) * (180 / Math.PI));

            if (yy > 0 && xx > 0)
            { }
            else if (yy < 0 && xx > 0)
            {
                ang = 360 - ang;
            }
            else if (yy < 0 && xx < 0)
            { ang = 180 + ang; }
            else if (yy > 0 && xx < 0)
            {
                ang = 180 - ang;
            }
            else if (xx == 0 && yy < 0)
            {
                ang += 180;
                ang = 270;
            }
            else if (xx < 0 && yy == 0)
            {
                ang += 180;
                ang = 180;
            }
            else if (xx == 0 && yy == 0)
            { ang = 0; }

            return (int)ang;
        }
 
        public static float cutfloat(float num, uint floatplaces)
        {
       
            string vtext = num.ToString();
         if (num.ToString().Length>7)
         {
            vtext = EquationSolver.NumricE(num).ToString();
         }
            if (vtext.Contains("."))
            {
                int idxofdot = vtext.IndexOf(".");

                if (floatplaces == 0)
                {
                    vtext = vtext.Substring(0, idxofdot);
                }
                else
                {
                    if (vtext.ToString().Length - 1 > idxofdot + floatplaces)
                    {
                        vtext = vtext.ToString().Substring(0, idxofdot + (int)floatplaces + 1);
                    }
                }
            }
           
            return float.Parse(vtext);
        }
        public static Vector3[][] GetPlot3DObject()
        {
            Vector3[] nodes = new Vector3[]{new Vector3(40.6,28.3,-1.1),
new Vector3(40.1,30.4,-1.1),
new Vector3(40.7,31.1,-1.1),
new Vector3(42.0,30.4,-1.1),
new Vector3(43.5,28.3,-1.1),
new Vector3(37.5,28.3,14.5),
new Vector3(37.0,30.4,14.3),
new Vector3(37.6,31.1,14.5),
new Vector3(38.8,30.4,15.1),
new Vector3(40.2,28.3,15.6),
new Vector3(29.1,28.3,27.1),
new Vector3(28.7,30.4,26.8),
new Vector3(29.1,31.1,27.2),
new Vector3(30.1,30.4,28.1),
new Vector3(31.1,28.3,29.2),
new Vector3(16.5,28.3,35.6),
new Vector3(16.2,30.4,35.1),
new Vector3(16.5,31.1,35.7),
new Vector3(17.0,30.4,36.9),
new Vector3(17.6,28.3,38.2),
new Vector3(0.8,28.3,38.7),
new Vector3(0.8,30.4,38.1),
new Vector3(0.8,31.1,38.8),
new Vector3(0.8,30.4,40.1),
new Vector3(0.8,28.3,41.5),
new Vector3(-15.9,28.3,35.6),
new Vector3(-15.0,30.4,35.1),
new Vector3(-15.0,31.1,35.7),
new Vector3(-15.4,30.4,36.9),
new Vector3(-15.9,28.3,38.2),
new Vector3(-28.4,28.3,27.1),
new Vector3(-27.4,30.4,26.8),
new Vector3(-27.6,31.1,27.2),
new Vector3(-28.4,30.4,28.1),
new Vector3(-29.4,28.3,29.2),
new Vector3(-36.2,28.3,14.5),
new Vector3(-35.5,30.4,14.3),
new Vector3(-36.0,31.1,14.5),
new Vector3(-37.2,30.4,15.1),
new Vector3(-38.5,28.3,15.6),
new Vector3(-39.0,28.3,-1.1),
new Vector3(-38.4,30.4,-1.1),
new Vector3(-39.1,31.1,-1.1),
new Vector3(-40.4,30.4,-1.1),
new Vector3(-41.8,28.3,-1.1),
new Vector3(-35.9,28.3,-16.7),
new Vector3(-35.4,30.4,-16.5),
new Vector3(-36.0,31.1,-16.8),
new Vector3(-37.2,30.4,-17.3),
new Vector3(-38.5,28.3,-17.8),
new Vector3(-27.4,28.3,-29.4),
new Vector3(-27.0,30.4,-29.0),
new Vector3(-27.5,31.1,-29.4),
new Vector3(-28.4,30.4,-30.4),
new Vector3(-29.4,28.3,-31.4),
new Vector3(-14.8,28.3,-37.8),
new Vector3(-14.6,30.4,-37.3),
new Vector3(-14.8,31.1,-37.9),
new Vector3(-15.3,30.4,-39.1),
new Vector3(-15.9,28.3,-40.4),
new Vector3(0.8,28.3,-40.9),
new Vector3(0.8,30.4,-40.3),
new Vector3(0.8,31.1,-41.0),
new Vector3(0.8,30.4,-42.3),
new Vector3(0.8,28.3,-43.7),
new Vector3(16.5,28.3,-37.8),
new Vector3(16.2,30.4,-37.3),
new Vector3(16.5,31.1,-37.9),
new Vector3(17.0,30.4,-39.1),
new Vector3(17.6,28.3,-40.4),
new Vector3(29.1,28.3,-29.4),
new Vector3(28.7,30.4,-29.0),
new Vector3(29.1,31.1,-29.4),
new Vector3(30.1,30.4,-30.4),
new Vector3(31.1,28.3,-31.4),
new Vector3(37.5,28.3,-16.7),
new Vector3(37.0,30.4,-16.5),
new Vector3(37.6,31.1,-16.8),
new Vector3(38.8,30.4,-17.3),
new Vector3(40.2,28.3,-17.8),
new Vector3(48.7,17.2,-1.1),
new Vector3(53.2,6.2,-1.1),
new Vector3(56.5,-4.3,-1.1),
new Vector3(57.7,-14.3,-1.1),
new Vector3(45.0,17.2,17.7),
new Vector3(49.2,6.2,19.5),
new Vector3(52.1,-4.3,20.7),
new Vector3(53.3,-14.3,21.2),
new Vector3(34.8,17.2,32.9),
new Vector3(38.0,6.2,36.1),
new Vector3(40.3,-4.3,38.4),
new Vector3(41.2,-14.3,39.3),
new Vector3(19.6,17.2,43.0),
new Vector3(21.4,6.2,47.2),
new Vector3(22.7,-4.3,50.2),
new Vector3(23.1,-14.3,51.3),
new Vector3(0.8,17.2,46.7),
new Vector3(0.8,6.2,51.3),
new Vector3(0.8,-4.3,54.5),
new Vector3(0.8,-14.3,55.7),
new Vector3(-18.0,17.2,43.0),
new Vector3(-19.7,6.2,47.2),
new Vector3(-21.0,-4.3,50.2),
new Vector3(-21.5,-14.3,51.3),
new Vector3(-33.1,17.2,32.9),
new Vector3(-36.4,6.2,36.1),
new Vector3(-38.7,-4.3,38.4),
new Vector3(-39.5,-14.3,39.3),
new Vector3(-43.3,17.2,17.7),
new Vector3(-47.5,6.2,19.5),
new Vector3(-50.5,-4.3,20.7),
new Vector3(-51.6,-14.3,21.2),
new Vector3(-47.0,17.2,-1.1),
new Vector3(-51.6,6.2,-1.1),
new Vector3(-54.8,-4.3,-1.1),
new Vector3(-56.0,-14.3,-1.1),
new Vector3(-43.3,17.2,-19.9),
new Vector3(-47.5,6.2,-21.7),
new Vector3(-50.5,-4.3,-22.9),
new Vector3(-51.6,-14.3,-23.4),
new Vector3(-33.1,17.2,-35.1),
new Vector3(-36.4,6.2,-38.3),
new Vector3(-38.7,-4.3,-40.6),
new Vector3(-39.5,-14.3,-41.5),
new Vector3(-18.0,17.2,-45.3),
new Vector3(-19.7,6.2,-49.5),
new Vector3(-21.0,-4.3,-52.4),
new Vector3(-21.5,-14.3,-53.6),
new Vector3(0.8,17.2,-49.0),
new Vector3(0.8,6.2,-53.5),
new Vector3(0.8,-4.3,-56.7),
new Vector3(0.8,-14.3,-58.0),
new Vector3(19.6,17.2,-45.3),
new Vector3(21.4,6.2,-49.5),
new Vector3(22.7,-4.3,-52.4),
new Vector3(23.1,-14.3,-53.6),
new Vector3(34.8,17.2,-35.1),
new Vector3(38.0,6.2,-38.3),
new Vector3(40.3,-4.3,-40.6),
new Vector3(41.2,-14.3,-41.5),
new Vector3(45.0,17.2,-19.9),
new Vector3(49.2,6.2,-21.7),
new Vector3(52.1,-4.3,-22.9),
new Vector3(53.3,-14.3,-23.4),
new Vector3(55.5,-22.7,-1.1),
new Vector3(50.6,-28.9,-1.1),
new Vector3(45.7,-33.2,-1.1),
new Vector3(43.5,-35.6,-1.1),
new Vector3(51.2,-22.7,20.3),
new Vector3(46.7,-28.9,18.4),
new Vector3(42.2,-33.2,16.5),
new Vector3(40.2,-35.6,15.6),
new Vector3(39.6,-22.7,37.7),
new Vector3(36.1,-28.9,34.2),
new Vector3(32.7,-33.2,30.7),
new Vector3(31.1,-35.6,29.2),
new Vector3(22.3,-22.7,49.3),
new Vector3(20.4,-28.9,44.8),
new Vector3(18.4,-33.2,40.3),
new Vector3(17.6,-35.6,38.2),
new Vector3(0.8,-22.7,53.5),
new Vector3(0.8,-28.9,48.6),
new Vector3(0.8,-33.2,43.8),
new Vector3(0.8,-35.6,41.5),
new Vector3(-20.6,-22.7,49.3),
new Vector3(-18.7,-28.9,44.8),
new Vector3(-16.8,-33.2,40.3),
new Vector3(-15.9,-35.6,38.2),
new Vector3(-38.0,-22.7,37.7),
new Vector3(-34.5,-28.9,34.2),
new Vector3(-31.0,-33.2,30.7),
new Vector3(-29.4,-35.6,29.2),
new Vector3(-49.6,-22.7,20.3),
new Vector3(-45.1,-28.9,18.4),
new Vector3(-40.6,-33.2,16.5),
new Vector3(-38.5,-35.6,15.6),
new Vector3(-53.8,-22.7,-1.1),
new Vector3(-48.9,-28.9,-1.1),
new Vector3(-44.0,-33.2,-1.1),
new Vector3(-41.8,-35.6,-1.1),
new Vector3(-49.6,-22.7,-22.6),
new Vector3(-45.1,-28.9,-20.6),
new Vector3(-40.6,-33.2,-18.7),
new Vector3(-38.5,-35.6,-17.8),
new Vector3(-38.0,-22.7,-39.9),
new Vector3(-34.5,-28.9,-36.4),
new Vector3(-31.0,-33.2,-33.0),
new Vector3(-29.4,-35.6,-31.4),
new Vector3(-20.6,-22.7,-51.5),
new Vector3(-18.7,-28.9,-47.0),
new Vector3(-16.8,-33.2,-42.5),
new Vector3(-15.9,-35.6,-40.4),
new Vector3(0.8,-22.7,-55.7),
new Vector3(0.8,-28.9,-50.9),
new Vector3(0.8,-33.2,-46.0),
new Vector3(0.8,-35.6,-43.7),
new Vector3(22.3,-22.7,-51.5),
new Vector3(20.4,-28.9,-47.0),
new Vector3(18.4,-33.2,-42.5),
new Vector3(17.6,-35.6,-40.4),
new Vector3(39.6,-22.7,-39.9),
new Vector3(36.1,-28.9,-36.4),
new Vector3(32.7,-33.2,-33.0),
new Vector3(31.1,-35.6,-31.4),
new Vector3(51.2,-22.7,-22.6),
new Vector3(46.7,-28.9,-20.6),
new Vector3(42.2,-33.2,-18.7),
new Vector3(40.2,-35.6,-17.8),
new Vector3(42.5,-37.2,-1.1),
new Vector3(37.3,-38.5,-1.1),
new Vector3(24.6,-39.5,-1.1),
new Vector3(0.8,-39.9,-1.1),
new Vector3(39.3,-37.2,15.2),
new Vector3(34.5,-38.5,13.2),
new Vector3(22.7,-39.5,8.2),
new Vector3(30.4,-37.2,28.5),
new Vector3(26.8,-38.5,24.8),
new Vector3(17.7,-39.5,15.8),
new Vector3(17.2,-37.2,37.3),
new Vector3(15.2,-38.5,32.6),
new Vector3(10.2,-39.5,20.8),
new Vector3(0.8,-37.2,40.6),
new Vector3(0.8,-38.5,35.4),
new Vector3(0.8,-39.5,22.6),
new Vector3(-15.5,-37.2,37.3),
new Vector3(-13.5,-38.5,32.6),
new Vector3(-8.5,-39.5,20.8),
new Vector3(-28.8,-37.2,28.5),
new Vector3(-25.1,-38.5,24.8),
new Vector3(-16.0,-39.5,15.8),
new Vector3(-37.6,-37.2,15.2),
new Vector3(-32.8,-38.5,13.2),
new Vector3(-21.1,-39.5,8.2),
new Vector3(-40.8,-37.2,-1.1),
new Vector3(-35.7,-38.5,-1.1),
new Vector3(-22.9,-39.5,-1.1),
new Vector3(-37.6,-37.2,-17.5),
new Vector3(-32.8,-38.5,-15.4),
new Vector3(-21.1,-39.5,-10.4),
new Vector3(-28.8,-37.2,-30.7),
new Vector3(-25.1,-38.5,-27.0),
new Vector3(-16.0,-39.5,-18.0),
new Vector3(-15.5,-37.2,-39.6),
new Vector3(-13.5,-38.5,-34.8),
new Vector3(-8.5,-39.5,-23.0),
new Vector3(0.8,-37.2,-42.8),
new Vector3(0.8,-38.5,-37.6),
new Vector3(0.8,-39.5,-24.9),
new Vector3(17.2,-37.2,-39.6),
new Vector3(15.2,-38.5,-34.8),
new Vector3(10.2,-39.5,-23.0),
new Vector3(30.4,-37.2,-30.7),
new Vector3(26.8,-38.5,-27.0),
new Vector3(17.7,-39.5,-18.0),
new Vector3(39.3,-37.2,-17.5),
new Vector3(34.5,-38.5,-15.4),
new Vector3(22.7,-39.5,-10.4),
new Vector3(-44.6,17.7,-1.1),
new Vector3(-57.9,17.6,-1.1),
new Vector3(-67.7,16.9,-1.1),
new Vector3(-73.8,15.0,-1.1),
new Vector3(-75.9,11.3,-1.1),
new Vector3(-44.2,18.7,3.7),
new Vector3(-58.3,18.6,3.7),
new Vector3(-68.7,17.8,3.7),
new Vector3(-75.1,15.6,3.7),
new Vector3(-77.3,11.3,3.7),
new Vector3(-43.2,20.9,5.3),
new Vector3(-59.2,20.7,5.3),
new Vector3(-70.8,19.7,5.3),
new Vector3(-77.8,16.8,5.3),
new Vector3(-80.2,11.3,5.3),
new Vector3(-42.3,23.1,3.7),
new Vector3(-60.1,22.9,3.7),
new Vector3(-72.8,21.6,3.7),
new Vector3(-80.5,18.1,3.7),
new Vector3(-83.1,11.3,3.7),
new Vector3(-41.8,24.1,-1.1),
new Vector3(-60.5,23.9,-1.1),
new Vector3(-73.8,22.5,-1.1),
new Vector3(-81.8,18.7,-1.1),
new Vector3(-84.4,11.3,-1.1),
new Vector3(-42.3,23.1,-5.9),
new Vector3(-60.1,22.9,-5.9),
new Vector3(-72.8,21.6,-5.9),
new Vector3(-80.5,18.1,-5.9),
new Vector3(-83.1,11.3,-5.9),
new Vector3(-43.2,20.9,-7.5),
new Vector3(-59.2,20.7,-7.5),
new Vector3(-70.8,19.7,-7.5),
new Vector3(-77.8,16.8,-7.5),
new Vector3(-80.2,11.3,-7.5),
new Vector3(-44.2,18.7,-5.9),
new Vector3(-58.3,18.6,-5.9),
new Vector3(-68.7,17.8,-5.9),
new Vector3(-75.1,15.6,-5.9),
new Vector3(-77.3,11.3,-5.9),
new Vector3(-74.8,5.5,-1.1),
new Vector3(-71.3,-1.5,-1.1),
new Vector3(-65.1,-8.5,-1.1),
new Vector3(-56.0,-14.3,-1.1),
new Vector3(-76.0,4.9,3.7),
new Vector3(-72.2,-2.4,3.7),
new Vector3(-65.4,-9.6,3.7),
new Vector3(-55.6,-15.6,3.7),
new Vector3(-78.7,3.7,5.3),
new Vector3(-74.1,-4.2,5.3),
new Vector3(-66.1,-11.9,5.3),
new Vector3(-54.6,-18.6,5.3),
new Vector3(-81.3,2.5,3.7),
new Vector3(-75.9,-6.1,3.7),
new Vector3(-66.8,-14.2,3.7),
new Vector3(-53.6,-21.5,3.7),
new Vector3(-82.6,1.9,-1.1),
new Vector3(-76.8,-7.0,-1.1),
new Vector3(-67.1,-15.2,-1.1),
new Vector3(-53.2,-22.8,-1.1),
new Vector3(-81.3,2.5,-5.9),
new Vector3(-75.9,-6.1,-5.9),
new Vector3(-66.8,-14.2,-5.9),
new Vector3(-53.6,-21.5,-5.9),
new Vector3(-78.7,3.7,-7.5),
new Vector3(-74.1,-4.2,-7.5),
new Vector3(-66.1,-11.9,-7.5),
new Vector3(-54.6,-18.6,-7.5),
new Vector3(-76.0,4.9,-5.9),
new Vector3(-72.2,-2.4,-5.9),
new Vector3(-65.4,-9.6,-5.9),
new Vector3(-55.6,-15.6,-5.9),
new Vector3(49.2,0.6,-1.1),
new Vector3(62.8,3.8,-1.1),
new Vector3(68.7,11.3,-1.1),
new Vector3(71.9,20.4,-1.1),
new Vector3(77.6,28.3,-1.1),
new Vector3(49.2,-3.0,9.4),
new Vector3(63.8,1.0,8.4),
new Vector3(70.0,9.7,6.2),
new Vector3(73.6,19.8,3.9),
new Vector3(80.2,28.3,2.9),
new Vector3(49.2,-11.1,13.0),
new Vector3(66.1,-4.9,11.6),
new Vector3(73.0,6.2,8.6),
new Vector3(77.1,18.6,5.6),
new Vector3(86.1,28.3,4.2),
new Vector3(49.2,-19.2,9.4),
new Vector3(68.4,-10.9,8.4),
new Vector3(75.9,2.7,6.2),
new Vector3(80.7,17.3,3.9),
new Vector3(92.0,28.3,2.9),
new Vector3(49.2,-22.8,-1.1),
new Vector3(69.5,-13.6,-1.1),
new Vector3(77.2,1.2,-1.1),
new Vector3(82.3,16.8,-1.1),
new Vector3(94.6,28.3,-1.1),
new Vector3(49.2,-19.2,-11.7),
new Vector3(68.4,-10.9,-10.6),
new Vector3(75.9,2.7,-8.4),
new Vector3(80.7,17.3,-6.1),
new Vector3(92.0,28.3,-5.1),
new Vector3(49.2,-11.1,-15.2),
new Vector3(66.1,-4.9,-13.8),
new Vector3(73.0,6.2,-10.8),
new Vector3(77.1,18.6,-7.8),
new Vector3(86.1,28.3,-6.4),
new Vector3(49.2,-3.0,-11.7),
new Vector3(63.8,1.0,-10.6),
new Vector3(70.0,9.7,-8.4),
new Vector3(73.6,19.8,-6.1),
new Vector3(80.2,28.3,-5.1),
new Vector3(79.6,29.5,-1.1),
new Vector3(81.1,29.9,-1.1),
new Vector3(81.6,29.5,-1.1),
new Vector3(80.4,28.3,-1.1),
new Vector3(82.5,29.6,2.6),
new Vector3(83.8,30.0,2.1),
new Vector3(83.9,29.6,1.5),
new Vector3(82.2,28.3,1.3),
new Vector3(88.8,29.7,3.9),
new Vector3(89.7,30.2,3.2),
new Vector3(88.9,29.8,2.4),
new Vector3(86.1,28.3,2.1),
new Vector3(95.0,29.9,2.6),
new Vector3(95.6,30.5,2.1),
new Vector3(93.9,30.0,1.5),
new Vector3(90.0,28.3,1.3),
new Vector3(97.9,29.9,-1.1),
new Vector3(98.3,30.5,-1.1),
new Vector3(96.1,30.1,-1.1),
new Vector3(91.8,28.3,-1.1),
new Vector3(95.0,29.9,-4.9),
new Vector3(95.6,30.5,-4.3),
new Vector3(93.9,30.0,-3.8),
new Vector3(90.0,28.3,-3.5),
new Vector3(88.8,29.7,-6.1),
new Vector3(89.7,30.2,-5.4),
new Vector3(88.9,29.8,-4.6),
new Vector3(86.1,28.3,-4.3),
new Vector3(82.5,29.6,-4.9),
new Vector3(83.8,30.0,-4.3),
new Vector3(83.9,29.6,-3.8),
new Vector3(82.2,28.3,-3.5),
new Vector3(0.8,49.7,-1.1),
new Vector3(10.5,48.3,-1.1),
new Vector3(10.1,44.9,-1.1),
new Vector3(6.4,40.7,-1.1),
new Vector3(6.5,36.9,-1.1),
new Vector3(9.8,48.3,2.7),
new Vector3(9.4,44.9,2.5),
new Vector3(6.0,40.7,1.1),
new Vector3(6.1,36.9,1.1),
new Vector3(7.7,48.3,5.8),
new Vector3(7.4,44.9,5.5),
new Vector3(4.8,40.7,2.9),
new Vector3(4.9,36.9,2.9),
new Vector3(4.6,48.3,7.8),
new Vector3(4.5,44.9,7.4),
new Vector3(3.0,40.7,4.1),
new Vector3(3.1,36.9,4.1),
new Vector3(0.8,48.3,8.6),
new Vector3(0.8,44.9,8.1),
new Vector3(0.8,40.7,4.5),
new Vector3(0.8,36.9,4.6),
new Vector3(-3.0,48.3,7.8),
new Vector3(-2.8,44.9,7.4),
new Vector3(-1.4,40.7,4.1),
new Vector3(-1.4,36.9,4.1),
new Vector3(-6.1,48.3,5.8),
new Vector3(-5.7,44.9,5.5),
new Vector3(-3.1,40.7,2.9),
new Vector3(-3.2,36.9,2.9),
new Vector3(-8.1,48.3,2.7),
new Vector3(-7.7,44.9,2.5),
new Vector3(-4.3,40.7,1.1),
new Vector3(-4.4,36.9,1.1),
new Vector3(-8.9,48.3,-1.1),
new Vector3(-8.4,44.9,-1.1),
new Vector3(-4.8,40.7,-1.1),
new Vector3(-4.9,36.9,-1.1),
new Vector3(-8.1,48.3,-4.9),
new Vector3(-7.7,44.9,-4.7),
new Vector3(-4.3,40.7,-3.3),
new Vector3(-4.4,36.9,-3.3),
new Vector3(-6.1,48.3,-8.0),
new Vector3(-5.7,44.9,-7.7),
new Vector3(-3.1,40.7,-5.1),
new Vector3(-3.2,36.9,-5.1),
new Vector3(-3.0,48.3,-10.0),
new Vector3(-2.8,44.9,-9.6),
new Vector3(-1.4,40.7,-6.3),
new Vector3(-1.4,36.9,-6.4),
new Vector3(0.8,48.3,-10.8),
new Vector3(0.8,44.9,-10.3),
new Vector3(0.8,40.7,-6.7),
new Vector3(0.8,36.9,-6.8),
new Vector3(4.6,48.3,-10.0),
new Vector3(4.5,44.9,-9.6),
new Vector3(3.0,40.7,-6.3),
new Vector3(3.1,36.9,-6.4),
new Vector3(7.7,48.3,-8.0),
new Vector3(7.4,44.9,-7.7),
new Vector3(4.8,40.7,-5.1),
new Vector3(4.9,36.9,-5.1),
new Vector3(9.8,48.3,-4.9),
new Vector3(9.4,44.9,-4.7),
new Vector3(6.0,40.7,-3.3),
new Vector3(6.1,36.9,-3.3),
new Vector3(13.8,34.3,-1.1),
new Vector3(24.3,32.6,-1.1),
new Vector3(33.7,30.9,-1.1),
new Vector3(37.8,28.3,-1.1),
new Vector3(12.8,34.3,4.0),
new Vector3(22.5,32.6,8.1),
new Vector3(31.2,30.9,11.8),
new Vector3(34.9,28.3,13.4),
new Vector3(10.0,34.3,8.1),
new Vector3(17.5,32.6,15.5),
new Vector3(24.2,30.9,22.2),
new Vector3(27.1,28.3,25.1),
new Vector3(5.9,34.3,10.9),
new Vector3(10.0,32.6,20.5),
new Vector3(13.7,30.9,29.2),
new Vector3(15.3,28.3,33.0),
new Vector3(0.8,34.3,11.9),
new Vector3(0.8,32.6,22.3),
new Vector3(0.8,30.9,31.8),
new Vector3(0.8,28.3,35.8),
new Vector3(-4.3,34.3,10.9),
new Vector3(-8.4,32.6,20.5),
new Vector3(-12.1,30.9,29.2),
new Vector3(-13.7,28.3,33.0),
new Vector3(-8.4,34.3,8.1),
new Vector3(-15.8,32.6,15.5),
new Vector3(-22.5,30.9,22.2),
new Vector3(-25.4,28.3,25.1),
new Vector3(-11.1,34.3,4.0),
new Vector3(-20.8,32.6,8.1),
new Vector3(-29.5,30.9,11.8),
new Vector3(-33.3,28.3,13.4),
new Vector3(-12.1,34.3,-1.1),
new Vector3(-22.6,32.6,-1.1),
new Vector3(-32.0,30.9,-1.1),
new Vector3(-36.1,28.3,-1.1),
new Vector3(-11.1,34.3,-6.2),
new Vector3(-20.8,32.6,-10.3),
new Vector3(-29.5,30.9,-14.0),
new Vector3(-33.3,28.3,-15.6),
new Vector3(-8.4,34.3,-10.3),
new Vector3(-15.8,32.6,-17.8),
new Vector3(-22.5,30.9,-24.4),
new Vector3(-25.4,28.3,-27.3),
new Vector3(-4.3,34.3,-13.1),
new Vector3(-8.4,32.6,-22.7),
new Vector3(-12.1,30.9,-31.4),
new Vector3(-13.7,28.3,-35.2),
new Vector3(0.8,34.3,-14.1),
new Vector3(0.8,32.6,-24.6),
new Vector3(0.8,30.9,-34.0),
new Vector3(0.8,28.3,-38.1),
new Vector3(5.9,34.3,-13.1),
new Vector3(10.0,32.6,-22.7),
new Vector3(13.7,30.9,-31.4),
new Vector3(15.3,28.3,-35.2),
new Vector3(10.0,34.3,-10.3),
new Vector3(17.5,32.6,-17.8),
new Vector3(24.2,30.9,-24.4),
new Vector3(27.1,28.3,-27.3),
new Vector3(12.8,34.3,-6.2),
new Vector3(22.5,32.6,-10.3),
new Vector3(31.2,30.9,-14.0),
new Vector3(34.9,28.3,-15.6)};
            int[][] edges = new int[][]{new int[]{505,509},
new int[] {439,438},
new int[] {479,474},
new int[] {215,218},
new int[] {382,377},
new int[] {468,469},
new int[] {512,507},
new int[] {497,501},
new int[] {145,204},
new int[] {374,369},
new int[] {381,385},
new int[] {442,401},
new int[] {99,103},
new int[] {411,406},
new int[] {489,493},
new int[] {36,41},
new int[] {148,152},
new int[] {500,504},
new int[] {473,468},
new int[] {203,207},
new int[] {526,466},
new int[] {288,287},
new int[] {324,328},
new int[] {494,429},
new int[] {85,80},
new int[] {135,139},
new int[] {507,511},
new int[] {417,416},
new int[] {455,450},
new int[] {291,296},
new int[] {129,133},
new int[] {506,441},
new int[] {336,335},
new int[] {473,477},
new int[] {485,480},
new int[] {524,519},
new int[] {318,322},
new int[] {526,461},
new int[] {141,81},
new int[] {85,89},
new int[] {308,312},
new int[] {211,244},
new int[] {166,170},
new int[] {387,382},
new int[] {502,437},
new int[] {413,408},
new int[] {491,495},
new int[] {337,331},
new int[] {264,269},
new int[] {66,60},
new int[] {289,283},
new int[] {45,40},
new int[] {300,304},
new int[] {175,174},
new int[] {391,390},
new int[] {461,460},
new int[] {78,3},
new int[] {388,383},
new int[] {46,40},
new int[] {125,124},
new int[] {73,78},
new int[] {488,483},
new int[] {475,479},
new int[] {113,112},
new int[] {423,418},
new int[] {501,505},
new int[] {246,249},
new int[] {343,348},
new int[] {156,91},
new int[] {395,399},
new int[] {418,401},
new int[] {46,51},
new int[] {105,104},
new int[] {142,141},
new int[] {493,497},
new int[] {35,30},
new int[] {243,246},
new int[] {34,39},
new int[] {109,108},
new int[] {37,42},
new int[] {404,405},
new int[] {428,423},
new int[] {18,23},
new int[] {230,233},
new int[] {322,326},
new int[] {362,356},
new int[] {71,65},
new int[] {365,364},
new int[] {484,483},
new int[] {498,433},
new int[] {340,339},
new int[] {51,45},
new int[] {217,213},
new int[] {58,57},
new int[] {331,365},
new int[] {405,409},
new int[] {244,247},
new int[] {227,230},
new int[] {325,291},
new int[] {312,307},
new int[] {276,281},
new int[] {352,346},
new int[] {322,321},
new int[] {362,367},
new int[] {418,422},
new int[] {1,6},
new int[] {207,147},
new int[] {341,340},
new int[] {345,350},
new int[] {346,340},
new int[] {519,518},
new int[] {518,522},
new int[] {355,360},
new int[] {268,273},
new int[] {197,192},
new int[] {399,371},
new int[] {42,41},
new int[] {409,413},
new int[] {228,231},
new int[] {265,270},
new int[] {420,419},
new int[] {205,145},
new int[] {64,69},
new int[] {527,526},
new int[] {372,376},
new int[] {346,351},
new int[] {79,4},
new int[] {317,281},
new int[] {197,201},
new int[] {23,22},
new int[] {406,410},
new int[] {69,68},
new int[] {519,523},
new int[] {367,366},
new int[] {31,30},
new int[] {338,343},
new int[] {11,10},
new int[] {357,351},
new int[] {363,357},
new int[] {484,488},
new int[] {115,110},
new int[] {177,172},
new int[] {215,155},
new int[] {263,257},
new int[] {47,52},
new int[] {86,81},
new int[] {481,476},
new int[] {155,159},
new int[] {263,262},
new int[] {134,129},
new int[] {165,160},
new int[] {115,119},
new int[] {185,184},
new int[] {485,484},
new int[] {347,346},
new int[] {245,195},
new int[] {446,401},
new int[] {122,126},
new int[] {416,415},
new int[] {199,203},
new int[] {368,333},
new int[] {319,318},
new int[] {208,209},
new int[] {31,36},
new int[] {165,169},
new int[] {114,118},
new int[] {260,294},
new int[] {2,3},
new int[] {335,334},
new int[] {122,121},
new int[] {88,9},
new int[] {56,50},
new int[] {242,245},
new int[] {206,146},
new int[] {136,74},
new int[] {483,478},
new int[] {106,110},
new int[] {269,274},
new int[] {452,451},
new int[] {147,151},
new int[] {259,260},
new int[] {376,371},
new int[] {515,514},
new int[] {212,147},
new int[] {298,325},
new int[] {114,113},
new int[] {525,520},
new int[] {112,39},
new int[] {257,258},
new int[] {386,381},
new int[] {307,302},
new int[] {208,207},
new int[] {484,479},
new int[] {393,358},
new int[] {106,105},
new int[] {453,452},
new int[] {261,295},
new int[] {190,194},
new int[] {413,417},
new int[] {90,94},
new int[] {436,435},
new int[] {503,507},
new int[] {69,74},
new int[] {200,204},
new int[] {221,159},
new int[] {416,420},
new int[] {307,311},
new int[] {459,454},
new int[] {82,86},
new int[] {211,211},
new int[] {504,508},
new int[] {432,427},
new int[] {172,107},
new int[] {90,89},
new int[] {12,6},
new int[] {496,500},
new int[] {225,221},
new int[] {183,182},
new int[] {344,339},
new int[] {514,518},
new int[] {408,407},
new int[] {6,0},
new int[] {417,412},
new int[] {488,492},
new int[] {326,321},
new int[] {62,67},
new int[] {522,526},
new int[] {48,47},
new int[] {421,420},
new int[] {27,21},
new int[] {205,200},
new int[] {318,313},
new int[] {53,52},
new int[] {166,165},
new int[] {36,35},
new int[] {283,282},
new int[] {450,454},
new int[] {361,366},
new int[] {472,476},
new int[] {310,305},
new int[] {510,514},
new int[] {307,306},
new int[] {317,321},
new int[] {176,180},
new int[] {513,512},
new int[] {258,263},
new int[] {84,88},
new int[] {163,158},
new int[] {255,209},
new int[] {388,387},
new int[] {107,106},
new int[] {291,290},
new int[] {238,234},
new int[] {330,331},
new int[] {9,8},
new int[] {353,352},
new int[] {13,12},
new int[] {441,440},
new int[] {312,316},
new int[] {220,216},
new int[] {464,459},
new int[] {393,397},
new int[] {73,67},
new int[] {238,237},
new int[] {61,55},
new int[] {264,263},
new int[] {156,160},
new int[] {103,107},
new int[] {433,432},
new int[] {24,29},
new int[] {256,255},
new int[] {136,140},
new int[] {49,43},
new int[] {434,401},
new int[] {349,344},
new int[] {301,266},
new int[] {375,370},
new int[] {254,207},
new int[] {440,444},
new int[] {425,424},
new int[] {130,134},
new int[] {288,293},
new int[] {200,135},
new int[] {492,496},
new int[] {76,70},
new int[] {432,436},
new int[] {367,332},
new int[] {187,186},
new int[] {265,259},
new int[] {529,524},
new int[] {211,253},
new int[] {480,484},
new int[] {143,138},
new int[] {158,157},
new int[] {460,459},
new int[] {316,320},
new int[] {191,190},
new int[] {340,334},
new int[] {58,52},
new int[] {424,428},
new int[] {316,311},
new int[] {191,186},
new int[] {62,56},
new int[] {523,518},
new int[] {378,373},
new int[] {12,11},
new int[] {271,276},
new int[] {311,310},
new int[] {96,24},
new int[] {206,205},
new int[] {515,519},
new int[] {294,293},
new int[] {80,81},
new int[] {171,170},
new int[] {345,339},
new int[] {54,48},
new int[] {58,63},
new int[] {61,66},
new int[] {447,451},
new int[] {470,474},
new int[] {42,36},
new int[] {47,41},
new int[] {286,285},
new int[] {510,445},
new int[] {276,275},
new int[] {207,206},
new int[] {229,232},
new int[] {394,398},
new int[] {244,240},
new int[] {355,354},
new int[] {501,496},
new int[] {222,225},
new int[] {196,200},
new int[] {471,470},
new int[] {130,129},
new int[] {42,47},
new int[] {247,250},
new int[] {67,61},
new int[] {379,383},
new int[] {161,160},
new int[] {303,302},
new int[] {370,397},
new int[] {280,274},
new int[] {255,251},
new int[] {443,438},
new int[] {80,79},
new int[] {394,393},
new int[] {289,294},
new int[] {456,455},
new int[] {116,49},
new int[] {228,224},
new int[] {247,243},
new int[] {180,184},
new int[] {209,213},
new int[] {284,289},
new int[] {231,234},
new int[] {121,120},
new int[] {381,348},
new int[] {172,176},
new int[] {249,245},
new int[] {47,46},
new int[] {63,62},
new int[] {384,379},
new int[] {293,287},
new int[] {23,17},
new int[] {135,134},
new int[] {367,361},
new int[] {377,343},
new int[] {231,227},
new int[] {323,322},
new int[] {297,296},
new int[] {396,395},
new int[] {385,353},
new int[] {167,162},
new int[] {356,361},
new int[] {198,193},
new int[] {223,219},
new int[] {101,96},
new int[] {186,181},
new int[] {502,506},
new int[] {387,386},
new int[] {159,154},
new int[] {496,495},
new int[] {277,272},
new int[] {443,447},
new int[] {190,185},
new int[] {450,401},
new int[] {180,119},
new int[] {529,469},
new int[] {101,105},
new int[] {364,359},
new int[] {189,193},
new int[] {290,289},
new int[] {140,80},
new int[] {149,153},
new int[] {170,165},
new int[] {486,490},
new int[] {407,402},
new int[] {153,148},
new int[] {335,329},
new int[] {427,431},
new int[] {87,82},
new int[] {132,136},
new int[] {122,117},
new int[] {92,14},
new int[] {437,432},
new int[] {140,79},
new int[] {242,187},
new int[] {12,17},
new int[] {196,131},
new int[] {366,360},
new int[] {153,157},
new int[] {528,468},
new int[] {10,5},
new int[] {192,127},
new int[] {211,256},
new int[] {437,441},
new int[] {40,35},
new int[] {21,15},
new int[] {332,366},
new int[] {279,284},
new int[] {323,318},
new int[] {145,149},
new int[] {151,155},
new int[] {373,338},
new int[] {306,301},
new int[] {150,154},
new int[] {227,171},
new int[] {429,433},
new int[] {111,110},
new int[] {211,210},
new int[] {273,267},
new int[] {333,369},
new int[] {284,283},
new int[] {326,298},
new int[] {361,355},
new int[] {15,10},
new int[] {329,330},
new int[] {405,464},
new int[] {359,354},
new int[] {368,362},
new int[] {350,355},
new int[] {467,526},
new int[] {414,401},
new int[] {254,208},
new int[] {439,443},
new int[] {377,381},
new int[] {357,356},
new int[] {288,282},
new int[] {438,442},
new int[] {78,77},
new int[] {248,195},
new int[] {398,393},
new int[] {354,349},
new int[] {369,373},
new int[] {233,179},
new int[] {21,26},
new int[] {2,76},
new int[] {319,323},
new int[] {79,78},
new int[] {299,300},
new int[] {13,18},
new int[] {324,323},
new int[] {3,4},
new int[] {389,393},
new int[] {483,482},
new int[] {174,178},
new int[] {448,447},
new int[] {33,32},
new int[] {327,322},
new int[] {274,279},
new int[] {260,265},
new int[] {336,330},
new int[] {179,174},
new int[] {218,159},
new int[] {24,23},
new int[] {389,353},
new int[] {421,416},
new int[] {209,254},
new int[] {181,180},
new int[] {211,238},
new int[] {308,307},
new int[] {155,154},
new int[] {313,281},
new int[] {160,164},
new int[] {179,183},
new int[] {203,198},
new int[] {117,112},
new int[] {210,255},
new int[] {391,395},
new int[] {507,502},
new int[] {397,363},
new int[] {7,6},
new int[] {390,394},
new int[] {451,450},
new int[] {226,225},
new int[] {161,156},
new int[] {91,95},
new int[] {271,270},
new int[] {276,270},
new int[] {522,461},
new int[] {132,64},
new int[] {117,121},
new int[] {233,175},
new int[] {305,266},
new int[] {70,65},
new int[] {147,206},
new int[] {465,460},
new int[] {270,264},
new int[] {491,486},
new int[] {109,113},
new int[] {204,139},
new int[] {400,399},
new int[] {383,387},
new int[] {420,415},
new int[] {100,29},
new int[] {194,198},
new int[] {270,275},
new int[] {492,487},
new int[] {330,364},
new int[] {154,158},
new int[] {449,444},
new int[] {237,233},
new int[] {475,470},
new int[] {521,525},
new int[] {520,515},
new int[] {116,44},
new int[] {261,266},
new int[] {250,253},
new int[] {253,256},
new int[] {25,20},
new int[] {395,390},
new int[] {363,368},
new int[] {163,162},
new int[] {146,150},
new int[] {124,128},
new int[] {194,193},
new int[] {493,488},
new int[] {131,135},
new int[] {97,96},
new int[] {154,153},
new int[] {430,401},
new int[] {476,471},
new int[] {251,254},
new int[] {259,293},
new int[] {189,188},
new int[] {234,237},
new int[] {499,498},
new int[] {110,109},
new int[] {28,22},
new int[] {177,176},
new int[] {217,220},
new int[] {401,402},
new int[] {510,449},
new int[] {317,286},
new int[] {248,251},
new int[] {451,446},
new int[] {0,75},
new int[] {202,201},
new int[] {232,228},
new int[] {252,255},
new int[] {290,284},
new int[] {184,188},
new int[] {38,43},
new int[] {169,168},
new int[] {482,417},
new int[] {447,446},
new int[] {498,502},
new int[] {235,238},
new int[] {240,243},
new int[] {173,172},
new int[] {321,291},
new int[] {218,221},
new int[] {257,292},
new int[] {356,355},
new int[] {304,308},
new int[] {302,301},
new int[] {256,252},
new int[] {59,58},
new int[] {409,404},
new int[] {232,235},
new int[] {514,449},
new int[] {188,192},
new int[] {192,131},
new int[] {467,468},
new int[] {216,212},
new int[] {235,231},
new int[] {236,239},
new int[] {168,172},
new int[] {466,465},
new int[] {219,222},
new int[] {245,248},
new int[] {304,299},
new int[] {457,456},
new int[] {495,490},
new int[] {32,31},
new int[] {253,249},
new int[] {126,121},
new int[] {38,37},
new int[] {43,42},
new int[] {338,332},
new int[] {11,5},
new int[] {216,219},
new int[] {237,240},
new int[] {464,404},
new int[] {191,195},
new int[] {219,215},
new int[] {220,223},
new int[] {76,1},
new int[] {118,113},
new int[] {125,129},
new int[] {495,499},
new int[] {136,69},
new int[] {155,150},
new int[] {106,101},
new int[] {99,94},
new int[] {89,84},
new int[] {146,147},
new int[] {436,431},
new int[] {380,375},
new int[] {108,39},
new int[] {174,169},
new int[] {137,132},
new int[] {123,127},
new int[] {199,194},
new int[] {459,458},
new int[] {497,492},
new int[] {102,97},
new int[] {509,504},
new int[] {365,359},
new int[] {479,483},
new int[] {266,265},
new int[] {89,93},
new int[] {90,85},
new int[] {420,424},
new int[] {39,44},
new int[] {51,50},
new int[] {463,458},
new int[] {328,327},
new int[] {419,423},
new int[] {94,89},
new int[] {137,141},
new int[] {474,413},
new int[] {131,126},
new int[] {81,85},
new int[] {28,33},
new int[] {157,152},
new int[] {107,111},
new int[] {505,504},
new int[] {75,70},
new int[] {477,476},
new int[] {298,299},
new int[] {376,380},
new int[] {503,502},
new int[] {33,27},
new int[] {511,510},
new int[] {37,31},
new int[] {278,283},
new int[] {339,334},
new int[] {517,512},
new int[] {283,288},
new int[] {124,54},
new int[] {287,282},
new int[] {488,487},
new int[] {399,398},
new int[] {1,75},
new int[] {48,42},
new int[] {496,491},
new int[] {487,486},
new int[] {444,443},
new int[] {139,143},
new int[] {517,521},
new int[] {293,292},
new int[] {77,2},
new int[] {149,144},
new int[] {374,378},
new int[] {431,426},
new int[] {315,319},
new int[] {479,478},
new int[] {481,485},
new int[] {44,38},
new int[] {449,453},
new int[] {472,471},
new int[] {511,515},
new int[] {278,277},
new int[] {282,277},
new int[] {504,503},
new int[] {382,386},
new int[] {470,409},
new int[] {447,442},
new int[] {428,427},
new int[] {431,435},
new int[] {92,19},
new int[] {477,481},
new int[] {79,73},
new int[] {299,303},
new int[] {128,64},
new int[] {476,480},
new int[] {66,65},
new int[] {251,199},
new int[] {8,13},
new int[] {211,232},
new int[] {85,84},
new int[] {511,506},
new int[] {455,454},
new int[] {29,34},
new int[] {362,361},
new int[] {415,419},
new int[] {61,60},
new int[] {9,14},
new int[] {264,258},
new int[] {320,324},
new int[] {275,280},
new int[] {353,358},
new int[] {500,499},
new int[] {49,48},
new int[] {0,1},
new int[] {116,120},
new int[] {211,241},
new int[] {397,369},
new int[] {440,439},
new int[] {413,412},
new int[] {72,77},
new int[] {41,40},
new int[] {285,279},
new int[] {108,112},
new int[] {501,500},
new int[] {342,347},
new int[] {121,116},
new int[] {182,177},
new int[] {347,352},
new int[] {7,1},
new int[] {97,92},
new int[] {407,411},
new int[] {372,399},
new int[] {103,98},
new int[] {469,473},
new int[] {152,91},
new int[] {83,142},
new int[] {337,342},
new int[] {271,265},
new int[] {311,315},
new int[] {121,125},
new int[] {156,95},
new int[] {71,76},
new int[] {294,288},
new int[] {95,90},
new int[] {461,465},
new int[] {466,467},
new int[] {105,100},
new int[] {415,414},
new int[] {98,93},
new int[] {113,117},
new int[] {77,71},
new int[] {133,128},
new int[] {286,280},
new int[] {342,341},
new int[] {129,124},
new int[] {390,389},
new int[] {105,109},
new int[] {394,389},
new int[] {341,335},
new int[] {438,401},
new int[] {471,475},
new int[] {133,137},
new int[] {82,141},
new int[] {343,342},
new int[] {518,457},
new int[] {320,319},
new int[] {489,484},
new int[] {1,2},
new int[] {130,125},
new int[] {4,9},
new int[] {197,196},
new int[] {275,274},
new int[] {157,161},
new int[] {119,123},
new int[] {209,210},
new int[] {110,114},
new int[] {401,401},
new int[] {366,331},
new int[] {313,317},
new int[] {98,102},
new int[] {520,519},
new int[] {263,268},
new int[] {86,90},
new int[] {480,479},
new int[] {120,54},
new int[] {379,374},
new int[] {252,248},
new int[] {144,145},
new int[] {325,297},
new int[] {435,434},
new int[] {332,333},
new int[] {134,138},
new int[] {305,309},
new int[] {378,377},
new int[] {422,401},
new int[] {200,139},
new int[] {195,194},
new int[] {341,346},
new int[] {328,300},
new int[] {94,98},
new int[] {98,97},
new int[] {268,267},
new int[] {297,301},
new int[] {142,82},
new int[] {333,338},
new int[] {138,137},
new int[] {63,57},
new int[] {467,471},
new int[] {14,8},
new int[] {490,425},
new int[] {165,164},
new int[] {148,83},
new int[] {334,329},
new int[] {249,252},
new int[] {416,411},
new int[] {144,143},
new int[] {266,260},
new int[] {14,19},
new int[] {204,143},
new int[] {241,244},
new int[] {193,188},
new int[] {112,44},
new int[] {2,7},
new int[] {430,434},
new int[] {458,462},
new int[] {208,212},
new int[] {233,236},
new int[] {3,77},
new int[] {266,271},
new int[] {291,285},
new int[] {241,237},
new int[] {129,128},
new int[] {26,25},
new int[] {525,524},
new int[] {68,73},
new int[] {331,332},
new int[] {184,123},
new int[] {285,290},
new int[] {452,447},
new int[] {59,64},
new int[] {188,127},
new int[] {414,418},
new int[] {18,17},
new int[] {427,422},
new int[] {8,7},
new int[] {176,115},
new int[] {145,146},
new int[] {309,313},
new int[] {281,280},
new int[] {195,190},
new int[] {93,88},
new int[] {262,257},
new int[] {119,114},
new int[] {178,173},
new int[] {52,57},
new int[] {168,107},
new int[] {509,508},
new int[] {301,305},
new int[] {393,363},
new int[] {32,37},
new int[] {141,136},
new int[] {273,272},
new int[] {17,16},
new int[] {172,111},
new int[] {44,49},
new int[] {445,440},
new int[] {448,452},
new int[] {240,239},
new int[] {319,314},
new int[] {16,21},
new int[] {53,47},
new int[] {60,55},
new int[] {333,367},
new int[] {78,72},
new int[] {151,146},
new int[] {360,365},
new int[] {392,391},
new int[] {201,200},
new int[] {310,314},
new int[] {224,167},
new int[] {429,424},
new int[] {369,370},
new int[] {205,204},
new int[] {412,407},
new int[] {236,179},
new int[] {230,175},
new int[] {214,213},
new int[] {309,276},
new int[] {189,184},
new int[] {260,261},
new int[] {9,3},
new int[] {174,173},
new int[] {275,269},
new int[] {497,496},
new int[] {299,326},
new int[] {66,71},
new int[] {211,226},
new int[] {138,142},
new int[] {150,149},
new int[] {308,303},
new int[] {365,330},
new int[] {439,434},
new int[] {99,98},
new int[] {358,363},
new int[] {489,488},
new int[] {382,381},
new int[] {330,335},
new int[] {336,341},
new int[] {355,349},
new int[] {203,202},
new int[] {24,18},
new int[] {389,358},
new int[] {293,258},
new int[] {300,327},
new int[] {513,517},
new int[] {441,436},
new int[] {528,527},
new int[] {211,235},
new int[] {374,373},
new int[] {193,192},
new int[] {507,506},
new int[] {53,58},
new int[] {33,38},
new int[] {72,71},
new int[] {383,382},
new int[] {403,404},
new int[] {468,472},
new int[] {473,472},
new int[] {221,224},
new int[] {245,191},
new int[] {311,306},
new int[] {441,445},
new int[] {494,433},
new int[] {120,124},
new int[] {375,374},
new int[] {397,368},
new int[] {254,203},
new int[] {425,420},
new int[] {460,464},
new int[] {465,464},
new int[] {502,441},
new int[] {358,357},
new int[] {491,490},
new int[] {274,268},
new int[] {22,27},
new int[] {433,437},
new int[] {384,388},
new int[] {67,66},
new int[] {353,347},
new int[] {112,116},
new int[] {350,349},
new int[] {143,142},
new int[] {425,429},
new int[] {478,417},
new int[] {104,108},
new int[] {316,315},
new int[] {475,474},
new int[] {526,465},
new int[] {175,170},
new int[] {187,191},
new int[] {206,201},
new int[] {391,386},
new int[] {395,394},
new int[] {147,208},
new int[] {166,161},
new int[] {193,197},
new int[] {16,15},
new int[] {327,299},
new int[] {292,287},
new int[] {22,21},
new int[] {154,149},
new int[] {88,92},
new int[] {493,492},
new int[] {28,27},
new int[] {303,307},
new int[] {158,153},
new int[] {91,86},
new int[] {348,353},
new int[] {80,84},
new int[] {171,175},
new int[] {267,262},
new int[] {454,458},
new int[] {109,104},
new int[] {343,337},
new int[] {512,516},
new int[] {159,163},
new int[] {113,108},
new int[] {19,24},
new int[] {93,97},
new int[] {274,273},
new int[] {244,243},
new int[] {463,403},
new int[] {486,421},
new int[] {181,185},
new int[] {128,59},
new int[] {404,408},
new int[] {23,28},
new int[] {409,408},
new int[] {161,165},
new int[] {312,311},
new int[] {11,16},
new int[] {50,45},
new int[] {478,413},
new int[] {173,177},
new int[] {29,23},
new int[] {396,400},
new int[] {518,453},
new int[] {198,202},
new int[] {385,389},
new int[] {228,227},
new int[] {325,296},
new int[] {212,151},
new int[] {527,522},
new int[] {296,295},
new int[] {167,166},
new int[] {380,379},
new int[] {20,15},
new int[] {57,56},
new int[] {101,100},
new int[] {32,26},
new int[] {182,186},
new int[] {37,36},
new int[] {4,80},
new int[] {323,327},
new int[] {159,158},
new int[] {36,30},
new int[] {240,236},
new int[] {149,148},
new int[] {26,20},
new int[] {215,151},
new int[] {5,0},
new int[] {86,85},
new int[] {272,267},
new int[] {59,53},
new int[] {406,401},
new int[] {348,342},
new int[] {303,298},
new int[] {134,133},
new int[] {212,215},
new int[] {26,31},
new int[] {68,67},
new int[] {345,344},
new int[] {87,86},
new int[] {408,403},
new int[] {314,318},
new int[] {64,63},
new int[] {290,295},
new int[] {437,436},
new int[] {43,37},
new int[] {348,347},
new int[] {242,191},
new int[] {373,333},
new int[] {485,489},
new int[] {196,135},
new int[] {306,310},
new int[] {52,51},
new int[] {380,384},
new int[] {258,292},
new int[] {314,313},
new int[] {81,82},
new int[] {392,387},
new int[] {259,264},
new int[] {298,302},
new int[] {376,375},
new int[] {140,74},
new int[] {239,187},
new int[] {114,109},
new int[] {524,523},
new int[] {411,410},
new int[] {306,305},
new int[] {402,403},
new int[] {227,167},
new int[] {123,118},
new int[] {476,475},
new int[] {162,157},
new int[] {56,61},
new int[] {321,325},
new int[] {269,263},
new int[] {453,448},
new int[] {127,122},
new int[] {265,264},
new int[] {188,123},
new int[] {48,53},
new int[] {371,398},
new int[] {360,359},
new int[] {487,491},
new int[] {146,205},
new int[] {107,102},
new int[] {453,457},
new int[] {169,164},
new int[] {279,278},
new int[] {284,278},
new int[] {351,350},
new int[] {482,421},
new int[] {69,63},
new int[] {417,421},
new int[] {278,272},
new int[] {499,494},
new int[] {251,203},
new int[] {127,131},
new int[] {211,220},
new int[] {398,370},
new int[] {363,362},
new int[] {294,259},
new int[] {65,60},
new int[] {411,415},
new int[] {326,325},
new int[] {431,430},
new int[] {302,306},
new int[] {202,206},
new int[] {385,348},
new int[] {499,503},
new int[] {500,495},
new int[] {139,134},
new int[] {162,166},
new int[] {516,520},
new int[] {318,317},
new int[] {211,229},
new int[] {506,510},
new int[] {17,22},
new int[] {175,179},
new int[] {455,459},
new int[] {315,310},
new int[] {448,443},
new int[] {357,362},
new int[] {327,326},
new int[] {324,319},
new int[] {310,309},
new int[] {421,425},
new int[] {213,212},
new int[] {158,162},
new int[] {162,161},
new int[] {258,259},
new int[] {483,487},
new int[] {57,62},
new int[] {462,402},
new int[] {490,494},
new int[] {164,99},
new int[] {403,407},
new int[] {285,284},
new int[] {34,28},
new int[] {225,228},
new int[] {49,54},
new int[] {512,511},
new int[] {117,116},
new int[] {68,62},
new int[] {464,463},
new int[] {192,196},
new int[] {468,527},
new int[] {505,500},
new int[] {238,241},
new int[] {41,46},
new int[] {408,412},
new int[] {446,450},
new int[] {523,522},
new int[] {474,478},
new int[] {76,75},
new int[] {204,144},
new int[] {423,427},
new int[] {226,229},
new int[] {508,507},
new int[] {522,457},
new int[] {466,470},
new int[] {73,72},
new int[] {295,260},
new int[] {243,239},
new int[] {527,467},
new int[] {482,486},
new int[] {194,189},
new int[] {451,455},
new int[] {329,364},
new int[] {77,76},
new int[] {83,144},
new int[] {250,246},
new int[] {342,336},
new int[] {305,271},
new int[] {400,395},
new int[] {220,219},
new int[] {460,455},
new int[] {46,45},
new int[] {492,491},
new int[] {19,13},
new int[] {224,227},
new int[] {371,372},
new int[] {81,140},
new int[] {246,242},
new int[] {84,9},
new int[] {432,431},
new int[] {183,178},
new int[] {181,176},
new int[] {283,277},
new int[] {234,230},
new int[] {442,446},
new int[] {461,456},
new int[] {100,24},
new int[] {19,18},
new int[] {100,104},
new int[] {381,343},
new int[] {185,180},
new int[] {246,245},
new int[] {379,378},
new int[] {434,438},
new int[] {164,168},
new int[] {96,19},
new int[] {207,202},
new int[] {163,167},
new int[] {110,105},
new int[] {217,216},
new int[] {419,414},
new int[] {97,101},
new int[] {426,430},
new int[] {471,466},
new int[] {185,189},
new int[] {222,218},
new int[] {424,423},
new int[] {120,49},
new int[] {247,246},
new int[] {352,351},
new int[] {337,336},
new int[] {235,234},
new int[] {177,181},
new int[] {128,132},
new int[] {27,32},
new int[] {528,523},
new int[] {222,221},
new int[] {297,298},
new int[] {232,231},
new int[] {412,416},
new int[] {514,453},
new int[] {169,173},
new int[] {280,285},
new int[] {377,338},
new int[] {231,230},
new int[] {83,87},
new int[] {423,422},
new int[] {219,218},
new int[] {457,452},
new int[] {295,289},
new int[] {407,406},
new int[] {118,122},
new int[] {384,383},
new int[] {223,222},
new int[] {495,494},
new int[] {135,130},
new int[] {67,72},
new int[] {322,317},
new int[] {465,405},
new int[] {216,215},
new int[] {443,442},
new int[] {396,391},
new int[] {126,130},
new int[] {183,187},
new int[] {30,25},
new int[] {52,46},
new int[] {198,197},
new int[] {457,461},
new int[] {102,106},
new int[] {370,371},
new int[] {469,528},
new int[] {347,341},
new int[] {229,228},
new int[] {190,189},
new int[] {525,529},
new int[] {180,115},
new int[] {400,372},
new int[] {427,426},
new int[] {199,198},
new int[] {410,401},
new int[] {119,118},
new int[] {182,181},
new int[] {386,390},
new int[] {124,59},
new int[] {456,451},
new int[] {509,513},
new int[] {463,462},
new int[] {529,528},
new int[] {108,34},
new int[] {328,323},
new int[] {153,152},
new int[] {520,524},
new int[] {88,14},
new int[] {56,55},
new int[] {459,463},
new int[] {157,156},
new int[] {452,456},
new int[] {4,78},
new int[] {386,385},
new int[] {104,29},
new int[] {213,208},
new int[] {142,137},
new int[] {281,286},
new int[] {517,516},
new int[] {6,11},
new int[] {243,242},
new int[] {273,278},
new int[] {214,217},
new int[] {436,440},
new int[] {39,38},
new int[] {422,426},
new int[] {27,26},
new int[] {256,210},
new int[] {111,106},
new int[] {82,83},
new int[] {211,214},
new int[] {428,432},
new int[] {160,99},
new int[] {21,20},
new int[] {279,273},
new int[] {63,68},
new int[] {351,345},
new int[] {321,286},
new int[] {472,467},
new int[] {458,401},
new int[] {150,145},
new int[] {226,222},
new int[] {92,96},
new int[] {111,115},
new int[] {369,368},
new int[] {6,5},
new int[] {211,223},
new int[] {138,133},
new int[] {57,51},
new int[] {331,336},
new int[] {248,199},
new int[] {361,360},
new int[] {398,397},
new int[] {524,528},
new int[] {519,514},
new int[] {95,99},
new int[] {72,66},
new int[] {41,35},
new int[] {445,449},
new int[] {94,93},
new int[] {508,512},
new int[] {143,83},
new int[] {261,297},
new int[] {415,410},
new int[] {286,291},
new int[] {3,8},
new int[] {375,379},
new int[] {440,435},
new int[] {17,11},
new int[] {388,392},
new int[] {7,12},
new int[] {358,352},
new int[] {313,276},
new int[] {356,350},
new int[] {179,178},
new int[] {218,155},
new int[] {13,7},
new int[] {93,92},
new int[] {350,344},
new int[] {424,419},
new int[] {103,102},
new int[] {152,87},
new int[] {523,527},
new int[] {84,4},
new int[] {91,90},
new int[] {132,69},
new int[] {301,261},
new int[] {211,247},
new int[] {95,94},
new int[] {176,111},
new int[] {16,10},
new int[] {54,59},
new int[] {22,16},
new int[] {433,428},
new int[] {133,132},
new int[] {229,225},
new int[] {390,385},
new int[] {503,498},
new int[] {239,242},
new int[] {125,120},
new int[] {371,375},
new int[] {211,250},
new int[] {486,425},
new int[] {521,520},
new int[] {62,61},
new int[] {378,382},
new int[] {270,269},
new int[] {187,182},
new int[] {320,315},
new int[] {71,70},
new int[] {54,53},
new int[] {131,130},
new int[] {370,374},
new int[] {34,33},
new int[] {223,226},
new int[] {250,249},
new int[] {74,73},
new int[] {295,294},
new int[] {171,166},
new int[] {29,28},
new int[] {340,345},
new int[] {289,288},
new int[] {8,2},
new int[] {480,475},
new int[] {506,445},
new int[] {435,430},
new int[] {332,337},
new int[] {280,279},
new int[] {234,233},
new int[] {296,290},
new int[] {152,156},
new int[] {429,428},
new int[] {51,56},
new int[] {221,163},
new int[] {167,171},
new int[] {498,437},
new int[] {456,460},
new int[] {494,498},
new int[] {252,251},
new int[] {144,148},
new int[] {435,439},
new int[] {269,268},
new int[] {43,48},
new int[] {210,214},
new int[] {104,34},
new int[] {490,429},
new int[] {164,103},
new int[] {173,168},
new int[] {195,199},
new int[] {454,401},
new int[] {346,345},
new int[] {148,87},
new int[] {426,401},
new int[] {249,248},
new int[] {268,262},
new int[] {253,252},
new int[] {74,68},
new int[] {478,482},
new int[] {338,337},
new int[] {38,32},
new int[] {255,254},
new int[] {241,240},
new int[] {373,377},
new int[] {403,462},
new int[] {304,303},
new int[] {87,91},
new int[] {508,503},
new int[] {55,50},
new int[] {314,309},
new int[] {64,58},
new int[] {126,125},
new int[] {74,79},
new int[] {186,190},
new int[] {352,357},
new int[] {225,224},
new int[] {237,236},
new int[] {239,183},
new int[] {351,356},
new int[] {281,275},
new int[] {184,119},
new int[] {404,463},
new int[] {118,117},
new int[] {123,122},
new int[] {178,182},
new int[] {115,114},
new int[] {410,414},
new int[] {160,95},
new int[] {127,126},
new int[] {186,185},
new int[] {89,88},
new int[] {515,510},
new int[] {170,174},
new int[] {470,405},
new int[] {419,418},
new int[] {402,406},
new int[] {335,340},
new int[] {137,136},
new int[] {504,499},
new int[] {178,177},
new int[] {168,103},
new int[] {387,391},
new int[] {481,480},
new int[] {449,448},
new int[] {102,101},
new int[] {141,140},
new int[] {18,12},
new int[] {516,511},
new int[] {296,261},
new int[] {170,169},
new int[] {445,444},
new int[] {360,354},
new int[] {366,365},
new int[] {474,409},
new int[] {392,396},
new int[] {513,508},
new int[] {477,472},
new int[] {213,216},
new int[] {309,271},
new int[] {521,516},
new int[] {39,33},
new int[] {151,150},
new int[] {462,401},
new int[] {201,196},
new int[] {224,163},
new int[] {405,466},
new int[] {139,138},
new int[] {211,217},
new int[] {412,411},
new int[] {31,25},
new int[] {236,183},
new int[] {315,314},
new int[] {516,515},
new int[] {230,171},
new int[] {214,209},
new int[] {399,394},
new int[] {383,378},
new int[] {44,43},
new int[] {444,448},
new int[] {302,297},
new int[] {487,482},
new int[] {201,205},
new int[] {202,197},
new int[] {14,13},
new int[] {444,439},
new int[] {368,367},
new int[] {96,100}};

          //  SolveText.Form1.object3d p = new SolveText.Form1.object3d();
          //  p.forepoints = nodes;
         //   p.speciallines = edges;
          List<Vector3[]> v=new List<Vector3[]>();
           for (int i=0;i<edges.Length;i++)   
           {int[] g=edges[i];
               Vector3[] k = new Vector3[4];
               k[0] = nodes[g[0]];
               k[1] = nodes[g[1]];
               if (i<edges.Length-1)
               {
                   int[] y = edges[i+1];

  k[2] = nodes[y[0]];
  k[3] = nodes[y[1]];
  v.Add(k);
               }
              
           }
           return v.ToArray();

        }
        public static PointF[] resizepath(PointF[] pnts, SizeF sz)
        {


            PointF[] ps = new PointF[pnts.Length];

            RectangleF rec = Recofpnts(pnts);
            for (int i = 0; i < pnts.Length; i++)
            {
                PointF c = pnts[i];
                ps[i] = new PointF(rec.X + (c.X - rec.X) / (rec.Width - rec.X) * sz.Width, rec.Y + (c.Y - rec.Y) / (rec.Height - rec.Y) * sz.Height);

            }
            return ps;
        }
        public static RectangleF Recofpnts(PointF[] pnts)
        {

            SizeF g = new SizeF();
            for (int i = 0; i < pnts.Length; i++)
            {
                PointF c = pnts[i];
                if (c.X > g.Width)
                { g.Width = c.X; }
                if (c.Y > g.Height)
                { g.Height = c.Y; }

            }
            PointF s = new PointF(g.Width, g.Height);
            for (int i = 0; i < pnts.Length; i++)
            {
                PointF c = pnts[i];
                if (c.X < s.X)
                { s.X = c.X; }
                if (c.Y < s.Y)
                { s.Y = c.Y; }

            }
            g.Width -= s.X; g.Height -= s.Y;
            return new RectangleF(s, g);
        }
        public static class Selraliz
        {
            public static string getCommandstartup()
            {
                return String.Join(" ", Environment.GetCommandLineArgs().Skip(1).ToArray());
            }
           public static  void ffff()
            {

            }
            public static void EstablishNewFileType(string ProgramPath, string Extension, string FileIcon, string Description)
            {
            /*
              string E= "." + Extension + "\\";
              dynamic W  = Microsoft.VisualBasic.Interaction.CreateObject("WScript.Shell", "");  
              W.regwrite("HKCR\\" + E, Extension + " File");
              W.regwrite("HKCR\\" + Extension + " File\\", Description);
              W.regwrite("HKCR\\" + Extension + " File\\DefaultIcon\\", FileIcon);
              W.regwrite("HKCR\\" + Extension + " File\\Shell\\Open\\Command\\", ProgramPath + " %1");
              W.regwrite("HKCR\\" + Extension + " File\\Shell\\", "Open");
       */
            }
            public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
            {
                using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, objectToWrite);
                }

            }
            public static T ReadFromBinaryFile<T>(string filePath)
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
            }
        }
        public static T ReadFromStream<T>(Stream s)
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(s);

        }
        public static float correctquart(float angle, float xx, float yy)
        {
            float value = angle;

            if (yy > 0 && xx > 0)
            { }
            else if (yy < 0 && xx > 0)
            { value = 360 - angle; }
            else if (yy < 0 && xx < 0)
            { value = 180 + angle; }
            else if (yy > 0 && xx < 0)
            { value = 180 - angle; }
            else if (xx == 0 && yy < 0)
            { value += 180; }
            else if (xx < 0 && yy == 0)
            { value += 180; }
            else if (xx == 0 && yy == 0)
            { value = 0; }
            return value;
        }
        public static int MaxOfThree(int w1, int w2, int w3)
        {
            if (w1 > w2)
            {
                if (w1 > w3)
                { return w1; }
                else { return w3; }

            }
            else
                if (w2 > w3)
                {
                    return w2;

                }
                else { return w3; }
        }
        public static class GifConvert
        {

            private static void WriteGifImg(byte[] B, BinaryWriter BW, int spd)
            {
                B[785] = (byte)spd;
                B[786] = 0;
                B[798] = (byte)(B[798] | 0x87);
                BW.Write(B, 781, 18);
                BW.Write(B, 13, 768);
                BW.Write(B, 799, B.Length - 800);
            }

            static byte[] GifAnimation =  
        {
        33,255,11,78,69,84,83,67,65,
		80,69,50,46,48,3,1,0,0,0 
        };
            public static void ConvertToGIF(Bitmap[] images, string saveaddres, Size imagesize, Color backcolor, int speed)
            {


                Bitmap egif1 = new Bitmap(imagesize.Width, imagesize.Height);
                Graphics g = Graphics.FromImage(egif1);


                g.Clear(backcolor);
                g.DrawImage(images[0], 0, 0, egif1.Width, egif1.Height);

                MemoryStream MemoryStream1 = new MemoryStream();
                BinaryReader BinaryReader1 = new BinaryReader(MemoryStream1);

                BinaryWriter BinaryWriter1 = new BinaryWriter(new FileStream(saveaddres, FileMode.Create));
                egif1.Save(MemoryStream1, System.Drawing.Imaging.ImageFormat.Gif);
                byte[] B = MemoryStream1.ToArray();
                B[10] = (byte)(B[10] & ((0x78)));
                BinaryWriter1.Write(B, 0, 13);
                BinaryWriter1.Write(GifAnimation);
                WriteGifImg(B, BinaryWriter1, speed);
                for (int I = 1; I <= images.Length - 1; I++)
                {
                    MemoryStream1.SetLength(0);

                    Bitmap egifx = new Bitmap(egif1.Width, egif1.Height);
                    Graphics gx = Graphics.FromImage(egifx);
                    gx.Clear(backcolor);
                    gx.DrawImage(images[I], 0, 0, egifx.Width, egifx.Height);

                    egifx.Save(MemoryStream1, System.Drawing.Imaging.ImageFormat.Gif);
                    B = MemoryStream1.ToArray();
                    WriteGifImg(B, BinaryWriter1, speed);
                }
                BinaryWriter1.Write(B[B.Length - 1]);

                BinaryWriter1.Close();
                MemoryStream1.Dispose();

            }
            public static void ConvertToGIF(Bitmap[] images, string saveaddres, int speed)
            {
                ConvertToGIF(images, saveaddres, images[0].Size, Color.Transparent, speed);
            }

        }

        public static void CopyProperties(object dest, object src)
        {
            fnc.CopyProperties(dest, src, false);
        }
        public static void CopyProperties(object dest, object src, bool excludeappearence)
        {
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                if (excludeappearence)
                {
                    if (item.Category == "Appearance")
                    {
                        item.SetValue(dest, item.GetValue(src));
                    }
                }
                else
                { item.SetValue(dest, item.GetValue(src)); }
            }
        }
        public static void CopyFields(object TFrom,object TTo)
        {

            foreach (FieldInfo fiFrom in TFrom.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                FieldInfo fiTo =TTo.GetType().GetField(fiFrom.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                fiTo.SetValue(TTo, fiFrom.GetValue(TFrom));
               
            }
          
        }

        public static void CopyProperties(object dest, object src,string catgory,bool onlycopy)
        {
            catgory = catgory.ToLower();
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                if (onlycopy)
                {
                    if (item.Category.ToLower() == catgory)
                    {
                        item.SetValue(dest, item.GetValue(src));
                    }
                }
                else
                {
                    if (item.Category.ToLower() != catgory)
                    {
                        item.SetValue(dest, item.GetValue(src));
                    }
                }
            }
        }
        public static float Topi(float n, bool topi = true)
        {
            if (topi)
            {
                return (float)((n) * (Math.PI / 180f));
            }
            else
            {
                return (float)((n) * ( 180f/Math.PI ));
        
            }
        }

        public static Bitmap GetSiteIcon(string link)
        {
            Uri url = new Uri(link, UriKind.RelativeOrAbsolute);
            if (url.HostNameType == UriHostNameType.Dns)
            {
                try
                {
                    string iconURL = "http://" + url.Host + "/favicon.ico";
                    System.Net.WebRequest f = System.Net.HttpWebRequest.Create(iconURL);

                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)f.GetResponse();
                    System.IO.Stream stream = response.GetResponseStream();
                    Bitmap favicon = (Bitmap)Image.FromStream(stream);
                    return favicon;
                }
                catch
                { return null; }
            }
            return null;
        }
        public static class Mouse
        {
            [DllImport("user32.dll")]

            static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
            private const int MOUSEEVENTF_MOVE = 0x0001;
            private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
            private const int MOUSEEVENTF_LEFTUP = 0x0004;
            private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
            private const int MOUSEEVENTF_RIGHTUP = 0x0010;
            private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
            private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
            private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
            public static void Move(int xDelta, int yDelta)
            {
                mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
            }
            public static void MoveTo(int x, int y)
            {
                mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x, y, 0, 0);
            }
            public static void LeftClick()
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }

            public static void LeftDown()
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }

            public static void LeftUp()
            {
                mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }

            public static void RightClick()
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }

            public static void RightDown()
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }

            public static void RightUp()
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            }
        }

        public static Color Oppositecolor(Color c)
        {
            Color value = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
            return value;
        }
        public static Color metrobackcolor(Image image)
        {
            int r = 0; int g = 0; int b = 0;

            int w, h;
            if (image.Width > 400)
            { w = 400; }
            else { w = image.Width; }
            if (image.Height > 400)
            { h = 400; }
            else { h = image.Height; }
            using (Bitmap m = new Bitmap(image, new Size(w, h)))
            {

                for (int x = 0; x < m.Width; x++)
                {
                    for (int y = 0; y < m.Height; y++)
                    {
                        r += m.GetPixel(x, y).R; g += m.GetPixel(x, y).G; b += m.GetPixel(x, y).B;
                    }
                }
                int bts = m.Width * m.Height;
                r = fnc.divdectint(r, bts);
                g = fnc.divdectint(g, bts);
                b = fnc.divdectint(b, bts);


            }
            return Color.FromArgb(r, g, b);
        }
        public static Color blacker(Color main, int rate)
        {
            if (rate > 255 || rate < 0)
            { throw new Exception("Can't as rate > 255"); }
            int r, g, b;
            Color ret;

            if (main.R < rate - 1)
            { r = rate - main.R; }
            else
            { r = main.R - rate; }
            if (main.G < rate - 1)
            { g = rate - main.G; }
            else
            { g = main.G - rate; }
            if (main.B < rate - 1)
            { b = rate - main.B; }
            else
            { b = main.B - rate; }

            ret = Color.FromArgb(r, g, b);

            return ret;
        }
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            int ArcRectangleWidth = Curve * 2;
            GraphicsPath P = new GraphicsPath();

            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);

            return P;

        }
        public static Rectangle OImg(Image img, Rectangle space, ImageLayout imagelayout)
        {
            if (img == null)
            { return new Rectangle(); }
            Size ImgSiz = new Size();

            Point ImgLoc = new Point(space.X, space.Y);

            Bitmap image = new Bitmap(img);
            switch (imagelayout)
            {
                case ImageLayout.Zoom:
                    ImgSiz.Width = fnc.tint(fnc.divdec(image.Width * space.Height, image.Height), true);
                    ImgSiz.Height = fnc.tint(fnc.divdec(image.Height * space.Width, image.Width), true);
                    if (ImgSiz.Width > space.Width)
                    { ImgSiz.Width = space.Width; }
                    if (ImgSiz.Height > space.Height)
                    { ImgSiz.Height = space.Height; }

                    if (ImgSiz.Width > image.Width) ImgSiz.Width = image.Width;
                    if (ImgSiz.Height > image.Height) ImgSiz.Height = image.Height;

                    ImgLoc.X = space.Width / 2 - ImgSiz.Width / 2 + space.X;
                    ImgLoc.Y = space.Height / 2 - ImgSiz.Height / 2 + space.Y;
                    break;
                case ImageLayout.Tile:
                    ImgLoc.X = space.X; ImgLoc.Y = space.Y;
                    ImgSiz = space.Size;
                    break;
                case ImageLayout.Stretch:
                    ImgSiz = space.Size;
                    ImgLoc.X = space.X; ImgLoc.Y = space.Y;
                    break;
                case ImageLayout.None:
                    ImgSiz = image.Size;
                    ImgLoc.X = space.X; ImgLoc.Y = space.Y;
                    break;
                case ImageLayout.Center:
                    ImgSiz = image.Size;
                    if (ImgSiz.Width < space.Width)
                        ImgLoc.X = space.Width / 2 - ImgSiz.Width / 2 + space.X; ImgLoc.Y = space.Height / 2 - ImgSiz.Height / 2 + space.Y;
                    if (ImgSiz.Height < space.Height)
                        ImgLoc.X = space.Width / 2 - ImgSiz.Width / 2 + space.X; ImgLoc.Y = space.Height / 2 - ImgSiz.Height / 2 + space.Y;
                    break;
            }

            return new Rectangle(ImgLoc, ImgSiz);
        }
        public static Bitmap Get3dImg(Bitmap theimg, Size siz, int theangle, int scale)
        {
            Bitmap b = new Bitmap(siz.Width, siz.Height);
            Graphics g = Graphics.FromImage(b);

            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(0, 0);

            //                                                       R,center
            p1.X = fnc.tint(Math.Cos((90 - theangle) * Math.PI / 180) * (siz.Width / 2) + (siz.Width / 2), true);
            p1.Y = fnc.tint(Math.Sin((90 - theangle) * Math.PI / 180) * scale, true);


            p2.X = fnc.tint(Math.Cos((270 - theangle) * Math.PI / 180) * (siz.Width / 2) + (siz.Width / 2), true);
            p2.Y = fnc.tint(Math.Sin((270 - theangle) * Math.PI / 180) * scale, true);

            p3.X = fnc.tint(Math.Cos((90 - theangle) * Math.PI / 180) * (siz.Width / 2) + (siz.Width / 2), true);
            p3.Y = fnc.tint(Math.Sin((90 - theangle) * Math.PI / 180) * scale + siz.Height, true);

            p4.X = fnc.tint(Math.Cos((270 - theangle) * Math.PI / 180) * (siz.Width / 2) + (siz.Width / 2), true);
            p4.Y = fnc.tint(Math.Sin((270 - theangle) * Math.PI / 180) * scale + siz.Height, true);

            Point[] pnts = { p1, p2, p3 };
            g.DrawImage(theimg, pnts);

            return b;
        }

        public static int tint(Object number)
        {
            return fnc.tint(number, true);
        }
        public static int tint(Object number, Boolean nearing)
        {
            int converted = -1;
            Boolean cannearing = false;
            if (Information.IsNumeric(number))
            {
                if (number.ToString().Length >= 8)
                {
                    number = number.ToString().Substring(0, 7);
                }
                if (number.ToString().Contains("."))
                {
                    if (number.ToString().Length > number.ToString().IndexOf("."))
                    {
                        int itsindx = number.ToString().IndexOf(".") + 1;


                        if (int.Parse(number.ToString().Substring(itsindx, 1)) >= 5)
                        {
                            cannearing = true;
                        }

                    }
                    number = number.ToString().Substring(0, number.ToString().IndexOf("."));
                }

                try
                {
                    converted = int.Parse(number.ToString());
                    if (cannearing == true && nearing)
                    {
                        converted += 1;
                    }
                }
                catch (FormatException s)
                {
                    throw new FormatException(s.Message);
                }
                catch (Exception s)
                { throw s; }


            }

            return converted;
        }
        public static float divdec(Object d1, Object d2)
        {
            float divd = -1;
            if (Information.IsNumeric(d1.ToString()) && Information.IsNumeric(d2.ToString()))
            {
                float dd1 = float.Parse(d1.ToString());
                float dd2 = float.Parse(d2.ToString());
                divd = dd1 / dd2;
            }
            else
            {
                throw new FormatException("No Correct numbers");
            }
            return divd;
        }
        public static int divdectint(Object d1, Object d2)
        {
            return tint(divdec(d1, d2), true);
        }
        public static PointF[] starline(float xc, float yc, float rw, float rh)
        {
            rw = rw / 2;
            rh = rh / 2;
            xc += rw;
            yc += rh;

            float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0);
            float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
            float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0);
            float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
            float r1w = rw * fnc.divdec(cos72, cos36);
            float r1h = rh * fnc.divdec(cos72, cos36);
            // Fill the star:
            PointF[] pts = new PointF[10];
            pts[0] = new PointF(xc, yc - rh);
            pts[1] = new PointF(xc + r1w * sin36, yc - r1h * cos36);
            pts[2] = new PointF(xc + rw * sin72, yc - rh * cos72);
            pts[3] = new PointF(xc + r1w * sin72, yc + r1h * cos72);
            pts[4] = new PointF(xc + rw * sin36, yc + rh * cos36);
            pts[5] = new PointF(xc, yc + r1h);
            pts[6] = new PointF(xc - rw * sin36, yc + rh * cos36);
            pts[7] = new PointF(xc - r1w * sin72, yc + r1h * cos72);
            pts[8] = new PointF(xc - rw * sin72, yc - rh * cos72);
            pts[9] = new PointF(xc - r1w * sin36, yc - r1h * cos36);
            return pts;
        }
        public static PointF[] polygon6lines(RectangleF therec)
        {
            PointF[] pps = new PointF[6];
            pps[0] = new PointF(therec.X, therec.Y + therec.Height / 2);
            pps[1] = new PointF(therec.X + therec.Width / 3, therec.Bottom);
            pps[2] = new PointF(therec.X + (therec.Width / 3) * 2, therec.Bottom);
            pps[3] = new PointF(therec.Right, pps[0].Y);
            pps[4] = new PointF(pps[2].X, therec.Y);
            pps[5] = new PointF(pps[1].X, therec.Y);
            return pps;
        }
        public static PointF[] polygon6lines(float x, float y, float width, float height)
        { return polygon6lines(new RectangleF(x, y, width, height)); }
    }
    #endregion
    #region graphic extension
    static class GraphicsExtension
    {
      
        public static void FillStar(this Graphics g, Brush b, float xc, float yc, float rw, float rh)
        {
            // r: determines the size of the star.
            // xc, yc: determine the location of the star.
            rw = rw / 2;
            rh = rh / 2;
            xc += rw;
            yc += rh;

            float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0);
            float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
            float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0);
            float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
            float r1w = rw * fnc.divdec(cos72, cos36);
            float r1h = rh * fnc.divdec(cos72, cos36);
            // Fill the star:
            PointF[] pts = new PointF[10];
            pts[0] = new PointF(xc, yc - rh);
            pts[1] = new PointF(xc + r1w * sin36, yc - r1h * cos36);
            pts[2] = new PointF(xc + rw * sin72, yc - rh * cos72);
            pts[3] = new PointF(xc + r1w * sin72, yc + r1h * cos72);
            pts[4] = new PointF(xc + rw * sin36, yc + rh * cos36);
            pts[5] = new PointF(xc, yc + r1h);
            pts[6] = new PointF(xc - rw * sin36, yc + rh * cos36);
            pts[7] = new PointF(xc - r1w * sin72, yc + r1h * cos72);
            pts[8] = new PointF(xc - rw * sin72, yc - rh * cos72);
            pts[9] = new PointF(xc - r1w * sin36, yc - r1h * cos36);

            g.FillPolygon(b, pts);

        }
        public static void FillStar(this Graphics g, Brush b, Rectangle rec)
        {
            FillStar(g, b, rec.X, rec.Y, rec.Width, rec.Height);
        }
        public static void DrawStar(this Graphics g, Pen b, float xc, float yc, float rw, float rh)
        {
            // r: determines the size of the star.
            // xc, yc: determine the location of the star.
            rw = rw / 2;
            rh = rh / 2;
            xc += rw;
            yc += rh;

            float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0);
            float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
            float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0);
            float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
            float r1w = rw * fnc.divdec(cos72, cos36);
            float r1h = rh * fnc.divdec(cos72, cos36);
            // Fill the star:
            PointF[] pts = new PointF[10];
            pts[0] = new PointF(xc, yc - rh);
            pts[1] = new PointF(xc + r1w * sin36, yc - r1h * cos36);
            pts[2] = new PointF(xc + rw * sin72, yc - rh * cos72);
            pts[3] = new PointF(xc + r1w * sin72, yc + r1h * cos72);
            pts[4] = new PointF(xc + rw * sin36, yc + rh * cos36);
            pts[5] = new PointF(xc, yc + r1h);
            pts[6] = new PointF(xc - rw * sin36, yc + rh * cos36);
            pts[7] = new PointF(xc - r1w * sin72, yc + r1h * cos72);
            pts[8] = new PointF(xc - rw * sin72, yc - rh * cos72);
            pts[9] = new PointF(xc - r1w * sin36, yc - r1h * cos36);

            g.DrawPolygon(b, pts);

        }
        public static void DrawStar(this Graphics g, Pen b, Rectangle rec)
        {
            DrawStar(g, b, rec.X, rec.Y, rec.Width, rec.Height);
        }


        public static float MakRectangleradius(this Graphics graphics, RectangleF rectangle)
        {
            return (float)((Math.Min(rectangle.Width, rectangle.Height)) / 2.0);
        }
public static Vector3 To3D(this PointF p)
        {
            return new Vector3(p.X, p.Y, 0);
        }
        public static GraphicsPath GenerateRoundedRectangle(this Graphics graphics, RectangleF rectangle, float radius, RectangeEdgeFilter filter)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F || filter.IsNoun)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= 1)
                {
                }
                radius = radius * graphics.MakRectangleradius(rectangle);
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                if (filter.TopLeft)

                    path.AddArc(arc, 180, 90);
                else
                {
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
                    path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
                }
                arc.X = rectangle.Right - diameter;
                if (filter.TopRight)
                    path.AddArc(arc, 270, 90);
                else
                {
                    path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
                    path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X + arc.Width, arc.Y);
                }
                arc.Y = rectangle.Bottom - diameter;
                if (filter.BottomRight)
                    path.AddArc(arc, 0, 90);
                else
                {
                    path.AddLine(arc.X + arc.Width, arc.Y, arc.X + arc.Width, arc.Y + arc.Height);
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X + arc.Width, arc.Y + arc.Height);
                }
                arc.X = rectangle.Left;
                if (filter.BottomLeft)
                    path.AddArc(arc, 90, 90);
                else
                {
                    path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X, arc.Y + arc.Height);
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
                }
                path.CloseFigure();
            }
            return path;
        }

        #region draw params
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius, RectangeEdgeFilter filter)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius, filter);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            graphics.DrawRoundedRectangle(pen, x, y, width, height, radius, RectangeEdgeFilter.All());
        }
        public static void DrawRoundedRectangle(
                this Graphics graphics,
                Pen pen,
                int x,
                int y,
                int width,
                int height,
                int radius)
        {
            graphics.DrawRoundedRectangle(
                    pen,
                    Convert.ToSingle(x),
                    Convert.ToSingle(y),
                    Convert.ToSingle(width),
                    Convert.ToSingle(height),
                    Convert.ToSingle(radius));
        }
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            Rectangle rectangle,
            int radius,
            RectangeEdgeFilter filter)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            Rectangle rectangle,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
             RectangeEdgeFilter.All());
        }
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, RectangleF rectangle, float radius, RectangeEdgeFilter filter)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            RectangleF rectangle,
            float radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
               RectangeEdgeFilter.All());
        }
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                float x,
                float y,
                float width,
                float height,
                float radius,
                RectangeEdgeFilter filter)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius, filter);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                float x,
                float y,
                float width,
                float height,
                float radius)
        {
            graphics.FillRoundedRectangle(
                    brush,
                    x,
                    y,
                    width,
                    height,
                    radius,
                   RectangeEdgeFilter.All());
        }
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                int x,
                int y,
                int width,
                int height,
                float radius)
        {
            graphics.FillRoundedRectangle(
                    brush,
                    Convert.ToSingle(x),
                    Convert.ToSingle(y),
                    Convert.ToSingle(width),
                    Convert.ToSingle(height),
                    Convert.ToSingle(radius));
        }
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            Rectangle rectangle,
            float radius,
          RectangeEdgeFilter filter)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            Rectangle rectangle,
            float radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
              RectangeEdgeFilter.All());
        }
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            RectangleF rectangle,
            float radius,
           RectangeEdgeFilter filter)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            RectangleF rectangle,
            float radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                RectangeEdgeFilter.All());
        }
    }
        #endregion

    [TypeConverter(typeof(controlConverter))]
    public class RectangeEdgeFilter
    {
        public RectangeEdgeFilter() { }
        public static RectangeEdgeFilter All()
        {
            RectangeEdgeFilter r = new RectangeEdgeFilter(); r.IsAll = true;
            return r;

        }
        public static RectangeEdgeFilter Noun()
        {
            RectangeEdgeFilter r = new RectangeEdgeFilter(); r.IsNoun = true;
            return r;

        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", new object[] { topleft.ToString(), TopRight.ToString(), bottomleft.ToString(), bottomright.ToString() });
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAll
        {
            get
            {
                if (topleft && topright && bottomleft && BottomRight)
                { return true; }
                else
                { return false; }
            }
            set
            {
                if (value)
                {
                    topleft = true; topright = true; bottomleft = true; BottomRight = true;
                }

            }

        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNoun
        {
            get
            {
                if (topleft == false && topright == false && bottomleft == false && BottomRight == false)
                { return true; }
                else
                { return false; }
            }
            set
            {
                if (value)
                {
                    topleft = false; topright = false; bottomleft = false; BottomRight = false;
                }

            }

        }
        private bool topleft = false;
        [DefaultValue(false)]
        public bool TopLeft
        {
            get { return topleft; }
            set { topleft = value; }
        }
        private bool topright = false;
        [DefaultValue(false)]
        public bool TopRight
        {
            get { return topright; }
            set { topright = value; }
        }
        private bool bottomleft = false;
        [DefaultValue(false)]
        public bool BottomLeft
        {
            get { return bottomleft; }
            set { bottomleft = value; }
        }
        private bool bottomright = false;
        [DefaultValue(false)]
        public bool BottomRight
        {
            get { return bottomright; }
            set { bottomright = value; }
        }

        public RectangeEdgeFilter(bool topleft, bool topright, bool bottomleft, bool bottomright)
        {
            this.topleft = topleft; this.topright = topright; this.bottomleft = bottomleft; this.bottomright = bottomright;
        }
    }
    public class controlConverter : TypeConverter
    {
        // Display the “+” symbol near the property name:
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            List<PropertyDescriptor> p = new List<PropertyDescriptor>();
            PropertyDescriptorCollection g = TypeDescriptor.GetProperties(typeof(RectangeEdgeFilter));
            foreach (PropertyDescriptor f in g)
            {
                if (f.IsBrowsable)
                { p.Add(f); }
            }
            return new PropertyDescriptorCollection(p.ToArray());
        }


    }

    #endregion

    #region KtabControl
    public class KTabsCollection : System.Collections.Generic.List<KtabItem>
    {
        public void UpdateIndex()
        {
            for (int i = 0; i < this.Count; i++)
            { this[i].Index = i; }
        }
        public new void Remove(KtabItem value)
        {
            _ktabcontrol.Controls.Remove(value);
            base.Remove(value);
            if (_ktabcontrol.Grid.Items.Contains(value.Header) == true)
            {
                _ktabcontrol.Grid.Items.Remove(value.Header);
            }

            UpdateIndex();
        }
        public new void RemoveAt(int f)
        { Remove(this[f]); }
        public new void Clear()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.RemoveAt(i);
            }
        }
        public new void Add(KtabItem value)
        {


            if (_ktabcontrol.Controls.Contains(value) == false)
            {
                _ktabcontrol.Controls.Add(value);
                base.Add(value);
                value.Index = this.Count - 1;
                value.Location = new Point(0, _ktabcontrol.Grid.Location.Y + 5);
                value.Size = new Size(_ktabcontrol.Width, _ktabcontrol.Height - _ktabcontrol.Grid.Location.Y - 5);

            }
            if (_ktabcontrol.Grid.Items.Contains(value.Header) == false)
            {
                _ktabcontrol.Grid.Items.Add(value.Header);
            }

        }

        private KTabControl _ktabcontrol;
        public KTabControl KtabControl { get { return _ktabcontrol; } set { _ktabcontrol = value; } }

        public KTabsCollection(KTabControl Parnt) { _ktabcontrol = Parnt; }
        public KTabsCollection() : this(null) { }

    }

    public class ktabcontroldesginer : System.Windows.Forms.Design.ControlDesigner
    {

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Record instance of control we're designing
            // Hook up events

            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            s.SelectionChanged += new EventHandler(OnSelectionChanged);

            c.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
        }
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            tabControl.Size = new Size(150, 300);
            if (tabControl.Controls.Count == 0)
            {
                this.Addgrid(null, null);

                this.tabControl.Grid.Selectedindex = 0;
            }


        }
        private void OnSelectionChanged(object sender, System.EventArgs e)
        {

            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            if (s.PrimarySelection is kgriditem)
            {
                kgriditem kgi = (kgriditem)s.PrimarySelection;

                for (int i = 0; i < tabControl.Tabs.Count; i++)
                {
                    if (tabControl.Tabs[i].Header == kgi)
                    {
                        tabControl.SelectedIndex = i;
                        //   tabControl.Tabs[i].BringToFront();
                        break;
                    }
                }
            }
            //      scale.OnSelectionChanged();

        }

        public void OnComponentRemoving(object sender, ComponentEventArgs e)
        {

            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));

            //  c.OnComponentChanging(tabControl, null);
            //       MessageBox.Show(e.Component.ToString());
            // If the user is removing a pointer
            if (e.Component is kgriditem)
            {
                for (int i = 0; i < tabControl.Tabs.Count; i++)
                {
                    if (tabControl.Tabs[i].Header == (kgriditem)e.Component)
                    {
                        c.OnComponentChanging(tabControl, null);

                        tabControl.Controls.Remove(tabControl.Tabs[i]);
                        tabControl.Grid.Items.Remove((kgriditem)e.Component);
                        KtabItem kti = tabControl.Tabs[i];
                        tabControl.Tabs.RemoveAt(i);
                        h.DestroyComponent(kti);
                        c.OnComponentChanged(tabControl, null, null, null);
                        break;
                    }
                }

            }
            if (e.Component is KtabItem)
            {
                KtabItem v = (KtabItem)e.Component;

                if (tabControl.Tabs.Contains((KtabItem)e.Component))
                {
                    c.OnComponentChanging(tabControl, null);

                    //   tabControl.Grid.Controls.Remove(v.Header);
                    tabControl.Grid.Items.Remove(v.Header);

                    tabControl.Controls.Remove(v);
                    tabControl.Tabs.Remove(v);

                    h.DestroyComponent(v.Header);


                    c.OnComponentChanged(tabControl, null, null, null);
                }

            }
            // If the user is removing the control itself
            if (e.Component == tabControl)
            {


                tabControl.Grid.Selectedindex = -1;

                for (int i = tabControl.Tabs.Count - 1; i >= 0; i--)
                {
                    c.OnComponentChanging(tabControl, null);

                    h.DestroyComponent(tabControl.Tabs[i]);
                    //    h.DestroyComponent(tabControl.Tabs[i].Header);

                    //   tabControl.Grid.Controls.Remove(tabControl.Tabs[i].Header);
                    //    tabControl.Controls.Remove(tabControl.Tabs[i]);
                    c.OnComponentChanged(tabControl, null, null, null);
                }
                c.OnComponentChanging(tabControl, null);

                h.DestroyComponent(tabControl.Grid);
                c.OnComponentChanged(tabControl, null, null, null);
            }
            //       c.OnComponentChanged(tabControl, null, null, null);

        }


        public void RemoveTab(object sender, System.EventArgs e)
        {

            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));

            int dx = tabControl.Tabs.Count - 1;

            h.DestroyComponent(tabControl.Tabs[dx].Header);







        }
        public void AddTab(object sender, System.EventArgs e)
        {
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction dt;
            //       IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));


            dt = h.CreateTransaction("Add KgridItem");
            kgriditem kgi = (kgriditem)h.CreateComponent(typeof(kgriditem));
            kgi.Theme = KitemThemes.sky;
            kgi.ShowSelectedTrangle = false;
            kgi.Text = "Tab " + (tabControl.Tabs.Count + 1).ToString();
            kgi.Height = tabControl.Grid.Height - tabControl.LineWidth;


            kgi.BorderFilter_Selected.TopLeft = kgi.BorderFilter_Over.TopLeft = false;
            dt.Commit();
            dt = h.CreateTransaction("Add KtabItem");
            KtabItem kgt = (KtabItem)h.CreateComponent(typeof(KtabItem));
            kgt.Header = kgi;
            kgt.Index = tabControl.Tabs.Count - 1;
            kgt.BackColor = Color.SlateGray;

            kgt.Location = new Point(0, tabControl.Grid.Location.Y + tabControl.Grid.Height);
            kgt.Size = new Size(tabControl.Width, tabControl.Height - tabControl.Grid.Location.Y - tabControl.Grid.Height);

            tabControl.Tabs.Add(kgt);

            tabControl.Grid.Controls.Add(kgi);
            tabControl.Controls.Add(kgt);
            //    this.tabControl.Grid.Dock = DockStyle.Top;
            //    kgt.Dock = DockStyle.Fill;

            dt.Commit();
        }

        public void Addgrid(object sender, System.EventArgs e)
        {
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction dt;
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Add a new button to the collection
            dt = h.CreateTransaction("Add Kgrid");

            kgrid gd;

            gd = (kgrid)h.CreateComponent(typeof(kgrid));
            c.OnComponentChanging(tabControl, null);
            if (tabControl.Controls.Contains(gd) == false)
            {
            }
            //  tabControl.Grid = gd;
            gd.Grouping = false;
            gd.Location = new Point(0, 0);
            //   gd.Dock = DockStyle.Top;
            gd.Size = new Size(tabControl.Width, tabControl.GridSize);

            gd.InnerSpace = new Point(2, 0);
            gd.Customzing = true;
            tabControl.Controls.Add(gd);

            c.OnComponentChanged(tabControl, null, null, null);
            dt.Commit();
            //  AddTab(null,null);
            //   return;
            dt = h.CreateTransaction("Add KgridItem");
            kgriditem kgi = (kgriditem)h.CreateComponent(typeof(kgriditem));
            kgi.Theme = KitemThemes.sky;
            kgi.ShowSelectedTrangle = false;
            kgi.Text = "Tab " + (tabControl.Tabs.Count + 1).ToString();
            kgi.Height = gd.Height - tabControl.LineWidth;


            kgi.BorderFilter_Selected.TopLeft = kgi.BorderFilter_Over.TopLeft = false;
            c.OnComponentChanged(tabControl, null, null, null);

            dt.Commit();
            dt = h.CreateTransaction("Add KtabItem");
            KtabItem kgt = (KtabItem)h.CreateComponent(typeof(KtabItem));
            kgt.Header = kgi;
            kgt.Index = tabControl.Tabs.Count - 1;
            kgt.BackColor = Color.SlateGray;

            kgt.Location = new Point(0, gd.Location.Y + gd.Height);
            kgt.Size = new Size(tabControl.Width, tabControl.Height - gd.Location.Y - gd.Height);
            gd.Items.Add(kgi);

            tabControl.Tabs.Add(kgt);
            tabControl.Controls.Add(kgt);
            //  this.tabControl.Grid.Dock = DockStyle.Top;
            //    kgt.Dock = DockStyle.Fill;
            c.OnComponentChanged(tabControl, null, null, null);

            dt.Commit();
        }
        private DesignerVerbCollection _verbs;
        private DesignerVerb _AddTabItem;
        private DesignerVerb _RemoveTabItem;
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection();

                    _AddTabItem = new DesignerVerb("Add TAb", this.AddTab);
                    _RemoveTabItem = new DesignerVerb("Remove Tab", this.RemoveTab);
                    _verbs.Add(_AddTabItem);
                    _verbs.Add(_RemoveTabItem);
                }
                return _verbs;
            }
        }
        protected override void Dispose(bool disposing)
        {
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Unhook events
            s.SelectionChanged -= new EventHandler(OnSelectionChanged);
            c.ComponentRemoving -= new ComponentEventHandler(OnComponentRemoving);

            base.Dispose(disposing);
        }


        private KTabControl tabControl
        {
            get
            {
                return (KTabControl)this.Control;
            }
        }
        public ktabcontroldesginer() { }


    }

    //   [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [DesignerAttribute(typeof(ktabcontroldesginer))]
    public class KTabControl : ContainerControl
    {
        List<KtabItem> tabs = new List<KtabItem>();
        //  KTabsCollection tabs = new KTabsCollection();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<KtabItem> Tabs
        {
            get { return tabs; }

        }
        private kgrid grid;

        public kgrid Grid
        {
            get
            {
                if (grid == null)
                {
                    foreach (Control c in this.Controls)
                    {
                        if (c is kgrid)
                        { grid = (kgrid)c; break; }

                    }

                }

                return grid;
            }
            set { grid = value; }
        }

        public int SelectedIndex
        {
            get
            {
                if (Grid == null) return -1;
                return Grid.Selectedindex;
            }
            set
            {
                if (Grid != null)
                {
                    if (grid.Items.Count > value)
                    {
                        Grid.Selectedindex = value;
                    }
                }
            }
        }

        private int linewidth = 4;

        public int LineWidth
        {
            get { return linewidth; }
            set { linewidth = value; if (this.Grid != null)this.Grid.Invalidate(); }
        }
        private int gridsize = 28;

        public int GridSize
        {
            get { return gridsize; }
            set { gridsize = value; this.OnSizeChanged(new EventArgs()); if (this.Grid != null)this.Grid.Invalidate(); }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Grid != null)
            {
                Grid.Size = new Size(this.Width, GridSize);
                Grid.SortItemsLocation();

                for (int i = 0; i < this.tabs.Count; i++)
                { tabs[i].Size = new Size(Width, Height - Grid.Height); }
            }
        }
        /*
        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(new Point(0, gridsize), new Size(Width, Height - gridsize)); ;
            }
        }
       */
        private void onslectedindexshanged(object sender, kgrideventargs e)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].Header == e.item)
                {
                    tabs[i].BringToFront();
                    Grid.Invalidate();
                    break;
                }
            }
        }
        private void ongridsizechanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.tabs.Count; i++)
            {
                tabs[i].Size = new Size(Width, Height - Grid.Height);
                tabs[i].Location = new Point(0, Grid.Height);
            }
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control == Grid)
            {

                Grid.Selecteditemchanged += new kgrideventhandler(onslectedindexshanged);
                Grid.SizeChanged += new EventHandler(ongridsizechanged);
            }
        }

        public KTabControl()
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer , true);

            //   tabs.KtabControl = this;
            //  grid = new kgrid();
            //    grid.Grouping = false;
            //    grid.Location = new Point(0, 0);
            //     grid.Size = new Size(this.Width, 30);
            //     tabs.Add(new KtabItem(new kgriditem()));
            // this.Controls.Add(grid);
        }

    }
    public class KtabItem : Panel
    {
        public int Index = -1;
        private kgriditem header;
        public kgriditem Header
        {
            get { return header; }
            set { header = value; }
        }
        public KtabItem()
        { header = new kgriditem(); }
        public KtabItem(kgriditem theheader)
        {

            this.header = theheader;
        }
    }
    #endregion

    #region Kgriditem
    public class kgriditemControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        
        protected override bool GetHitTest(Point point)
      {
           return base.GetHitTest(point);
      }
 
        private bool mouseover = false;
        // This color is a private fiISelectionService)this.
        // This boolean state reflectseld for the OutlineColor property. 
        private Color lineColor = Color.Red;

        // This color is used to outline the control when the mouse is  
        // over the control. 
        public Color OutlineColor
        {
            get
            {
                return lineColor;
            }
            set
            {
                lineColor = value;
            }
        }


        private kgriditem kgriditem { get { return this.Control as kgriditem; } }

       
       
        // Sets a value and refreshes the control's display when the  
        // mouse position enters the area of the control.
 public kgriditemControlDesigner()
        {
        }
        protected override void OnMouseEnter()
        {


            //      this.Control.Refresh();
            if (this.kgriditem.Parent != null)
            {
                if (this.kgriditem.Parent is kgrid)
                {
                    if (this.kgriditem.Parent.Parent != null)
                    {
                        if (this.kgriditem.Parent.Parent is KTabControl)
                        {
                            this.mouseover = true;

                        }
                    }
                }
            }
        }

        // Sets a value and refreshes the control's display when the  
        // mouse position enters the area of the control.         
        protected override void OnMouseLeave()
        {
            if (this.mouseover == true)
            {

                this.mouseover = false;


            }
        }


        // Draws an outline around the control when the mouse is  
        // over the control.     
        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
        {
            //     Bitmap b = new Bitmap(kgriditem.Width, kgriditem.Height);
            //     Graphics g = Graphics.FromImage(b);
            //    g.Clear(Color.Blue);
            // 
            if (this.mouseover)
            {    //pe.Graphics.DrawImage(b, 0, 0, this.kgriditem.Width, this.kgriditem.Height);



                pe.Graphics.DrawImage(this.kgriditem.overb(kgriditem.Mosstat.selected), 0, 0, this.kgriditem.Width, this.kgriditem.Height);

                //   pe.Graphics.DrawImage(this.kgriditem.overb(kgriditem.Mosstat.over), 0, 0,this.kgriditem.Width,this.kgriditem.Height);


            }
        }

        // Adds a property to this designer's control at design time  
        // that indicates the outline color to use.  
        // The DesignOnlyAttribute ensures that the OutlineColor 
        // property is not serialized by the designer. 
        /*  protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
          PropertyDescriptor pd = TypeDescriptor.CreateProperty(
                typeof(kgriditemControlDesigner),
                "OutlineColor",
                typeof(System.Drawing.Color),
                new Attribute[] { new DesignOnlyAttribute(true) });
            
        //    properties.Add("OutlineColor", pd);

        }
       */
    }

    public enum KitemPaintStyle { Gradient, DoubleFlat, Flat }
    public enum KitemClickingEffects { Button, Metro, None }
    public enum KitemShapes { tile, grid, metro }
    public enum KitemThemes { sky, DeepSky, orng, red, dark, light, green, metro, none, defult }
    [DesignerAttribute(typeof(kgriditemControlDesigner))]
    public class kgriditem : System.Windows.Forms.Control, IComparable
    {

      
      
        public int CompareTo(Object other)
        {
            kgriditem oth = other as kgriditem;
            int res = 0;
            if (this.Parent != null)
            {
                if (this.Parent is kgrid)
                {
                    if (((kgrid)this.Parent).IsAccesdingSort)
                    {
                        res = this.Text.CompareTo(oth.Text);
                    }
                    else { res = oth.Text.CompareTo(this.Text); }
                }

            }
            return res;
        }
        public override string ToString()
        {
            return String.Format("Name : {0} Text : {1}", Name, Text);
        }
        kDynamkPanel group;
        [DefaultValue(null)]
        public kDynamkPanel Group
        {
            get { return group; }
            set
            {
                if (value != null)
                {

                    if (!value.Controls.Contains(this))
                    {

                        value.Controls.Add(this);

                        group = value;
                    }
                }
            }
        }

        #region properties
#region bordes
        private RectangeEdgeFilter borderfilter_normal = RectangeEdgeFilter.Noun();
        [Category("RectangleFilter"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RectangeEdgeFilter BorderFilter_Normal
        {
            get { return borderfilter_normal; }
            set { borderfilter_normal = value; Invalidate(); }
        }
        private RectangeEdgeFilter borderfilter_Over = RectangeEdgeFilter.All();
        [Category("RectangleFilter"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RectangeEdgeFilter BorderFilter_Over
        {
            get { return borderfilter_Over; }
            set { borderfilter_Over = value; Invalidate(); }
        }
        private RectangeEdgeFilter borderfilter_Selected = RectangeEdgeFilter.All();
        [Category("RectangleFilter"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RectangeEdgeFilter BorderFilter_Selected
        {
            get { return borderfilter_Selected; }
            set { borderfilter_Selected = value; Invalidate(); }
        }
#endregion
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }
     
        int subpanelarrowwidth = 25;
        [Category("Appearance"), DefaultValue(25)]
        public int SubPanelArrowWidth
        {
            get { return subpanelarrowwidth; }
            set { subpanelarrowwidth = value; Invalidate(); }
        }
        int subpanelformwidth = 150;
        [Category("Appearance"), DefaultValue(150)]
        public int SubPanelFormWidth
        {
            get { return subpanelformwidth; }
            set { subpanelformwidth = value; }
        }
        bool showsubpanel = false;
        [Category("Behavior"), DefaultValue(false)]
        public bool ShowSubPanel
        {
            get { return showsubpanel; }
            set { showsubpanel = value; Invalidate(); }
        }
        List<comboitem> tasks = new List<comboitem>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<comboitem> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }
        
      
        int selectedtaskindx = -1;
        [DefaultValue(-1),Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedTaskIndx
        {
            get { return selectedtaskindx; }       
        }

        #region FilledArea

        private FilledAreaPainting filledareapainting = FilledAreaPainting.FilledAreaColorproperty;
        [Category("FilledArea"), DefaultValue(FilledAreaPainting.FilledAreaColorproperty)]
        public FilledAreaPainting FilledAreaPainting
        {
            get { return filledareapainting; }
            set { filledareapainting = value; Invalidate(); }
        }

        private HatchStyle pattrentype = HatchStyle.ForwardDiagonal;
        [Category("FilledArea"), DefaultValue(HatchStyle.ForwardDiagonal),
         Description("The filled area painted with this Pattren type with transperncy value of 'FilledAreaTransparency'  property"
          + "\n  Only appear when 'FilledAreaPainting'prperty is set to 'pattrenlayer'")
        ]
        public HatchStyle FilledAreaPattrenType
        {
            get { return pattrentype; }
            set { pattrentype = value; Invalidate(); }
        }
        private Color filledareacolor = Color.Black;
        [Category("FilledArea"), DefaultValue(typeof(Color), "Black")]
        public Color FilledAreaColor
        {
            get { return filledareacolor; }
            set { filledareacolor = value; Invalidate(); }
        }
        private float filledareatransparency = 0.5F;
        [Category("FilledArea"), DefaultValue(0.5F)]
        public float FilledAreaTransparency
        {
            get { return filledareatransparency; }
            set
            {
                filledareatransparency = value;
                if (value < 0 || value > 1) { throw new Exception("Value must bp from 0.0 to 1.0"); }
                Invalidate();
            }
        }
       
        bool fillselectedarea = false ;
        [Category("FilledArea"), DefaultValue(false)]
        public bool Fillselectedarea
        {
            get { return fillselectedarea; }
            set { fillselectedarea = value; Invalidate(); }
        }   private float fillpercent = 0;
        [Category("FilledArea"), DefaultValue(0)]
        public float FillPercent
        {
            get { return fillpercent; }
            set { if (value < 0 || value > 100)throw new Exception("Value out of range 0:100");
                fillpercent = value; Invalidate(); }
        }
        #endregion
        private bool selectable = true;
        [Category("Behavior"), DefaultValue(true)]
        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }

        private bool showtext = true;
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowText
        {
            get { return showtext; }
            set { showtext = value; Invalidate(); }
        }

        private bool showimage = true;
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowImage
        {
            get { return showimage; }
            set { showimage = value; Invalidate(); }
        }

        Color bordercolor;
        [Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return bordercolor; }
            set
            {
                bordercolor = value;
                Invalidate();
            }
        }
        bool showselectedtrangle = true;
        [Category("Showing"), DefaultValue(true)]
        public bool ShowSelectedTrangle
        {
            get { return showselectedtrangle; }
            set { showselectedtrangle = value; }
        }

        Color secondcolor;
        [Category("Appearance")]
        public Color SecondColor
        {
            get { return secondcolor; }
            set
            {
                secondcolor = value;
                if (Automakeovercolor == true)
                {
                    Makecolors();
                }
                Invalidate();
            }
        }

        Color selectedcolor;
        [Category("Appearance"), Description("The item color when its is selected")]
        public Color SelectedColor
        {
            get { return selectedcolor; }
            set { selectedcolor = value; Invalidate(); }
        }


        Color selectedcolor2;
        [Category("Appearance"), Description("The item color when its is selected")]
        public Color SelectedColor2
        {
            get { return selectedcolor2; }
            set { selectedcolor2 = value; Invalidate(); }
        }

        Color overcolor1;
        [Category("Appearance")]
        public Color Overcolor1
        {
            get { return overcolor1; }
            set
            {
                overcolor1 = value;
                Invalidate();
            }
        }

        Color overcolor2;
        [Category("Appearance")]
        public Color Overcolor2
        {
            get { return overcolor2; }
            set
            {
                overcolor2 = value;
                Invalidate();
            }
        }

        bool automakeovercolors = false;
        [Category("Appearance"), DefaultValue(false)]
        public bool Automakeovercolor
        {
            get { return automakeovercolors; }
            set
            {
                automakeovercolors = value;
                if (automakeovercolors == true)
                {
                    Makecolors();
                }
                Invalidate();
            }
        }
        private void Makecolors()
        {
            Overcolor1 = ControlPaint.Light(this.BackColor, 0.8f);
            Overcolor2 = ControlPaint.Light(this.secondcolor, 0.8f);

        }
        bool isselected;
        [DefaultValue(false)]
        public bool Iselected
        {
            get { return isselected; }
            set
            {

                if (value && selectable)
                {
                 //   this.Focus();

                    if (this.Parent is kgrid)
                    {
                        kgrid kg = (kgrid)this.Parent;
                        if (kg.Multiselect == false)
                        {

                            if (!(kg.OnSelection || (ModifierKeys == Keys.Control && kg.ControlKeySelection)))
                            {
                                foreach (kgriditem kgi in kg.Items)
                                {
                                    if (kgi.Iselected == true) { kgi.Iselected = false; }
                                }
                            }


                        }
                        mosstat = Mosstat.selected;
                        isselected = true;
                        kg.Selectedindex = this.Index;
                    }
                    else if (this.Parent != null)
                    {
                        if (this.Parent.Parent != null)
                        {
                            if (this.Parent.Parent is kgrid && this.Parent is kDynamkPanel)
                            {
                                kgrid kg = (kgrid)this.Parent.Parent;
                                if (kg.Multiselect == false)
                                {
                                    if (!(kg.OnSelection || (ModifierKeys == Keys.Control && kg.ControlKeySelection)))
                                    {
                                        foreach (kDynamkPanel kgb1 in kg.Groups)
                                        {

                                            foreach (Control c2 in kgb1.Controls)
                                            {
                                                if (c2 is kgriditem)
                                                {
                                                    kgriditem k = (kgriditem)c2;
                                                    if (k.Iselected == true) { k.Iselected = false; }
                                                }
                                            }

                                        }
                                    }
                                }

                                mosstat = Mosstat.selected;
                                isselected = true;
                                kg.Selectedindex = this.Index;

                            }
                        }
                        else
                        {

                            mosstat = Mosstat.selected;
                            isselected = true;

                        }
                    }

                    else
                    {

                        mosstat = Mosstat.selected;
                        isselected = true;


                    }
                }



                else
                {
                    mosstat = Mosstat.none;
                    isselected = false;
                }

                Invalidate();

            }
        }

        Image centerimage;
        [Category("Appearance"), DefaultValue(null)]
        public Image Centerimage
        {
            get { return centerimage; }
            set
            {
                centerimage = value;
                ImgSiz = fnc.OImg(value, this.DisplayRectangle, centerimagelayout).Size;
                tempreal = (Bitmap)value;
                Invalidate();
            }
        }

        ImageLayout centerimagelayout = ImageLayout.Zoom;
        [Category("Appearance"), DefaultValue(ImageLayout.Zoom)]
        public ImageLayout CenterImageLayout
        {
            get { return centerimagelayout; }
            set { centerimagelayout = value; Invalidate(); }
        }

        int borderwidth = 4;
        [Category("Appearance"), DefaultValue(4)]
        public int Borderwidth
        {
            get { return borderwidth; }
            set
            {
                borderwidth = value;
                Invalidate();
            }
        }


        bool showimageborder = false;
        [Category("Appearance"), DefaultValue(false)]
        public bool Showimageborder
        {
            get
            {
                return showimageborder;
            }
            set
            {
                showimageborder = value;
                Invalidate();
            }
        }
        private List<object> infos = new List<object>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<object> Infos
        {
            get { return infos; }
            set { infos = value; Invalidate(); }
        }

        private List<object> tags = new List<object>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<object> Tags
        {
            get { return tags; }
            set { tags = value; Invalidate(); }
        }
        public int Index = -1;
       
        #endregion

        #region Enums

        #region themes
        KitemThemes theme = KitemThemes.DeepSky;
        [Category("Appearance"), Description("auto give apreantice colors to control"), DefaultValue(KitemThemes.DeepSky)]
        public KitemThemes Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                switch (theme)
                {
                    case KitemThemes.defult:
                        try
                        {
                            this.BackColor = Color.Transparent;// this.Parent.BackColor;
                            this.SecondColor = BackColor;// this.BackColor;
                            this.Automakeovercolor = true;
                            this.SelectedColor = ControlPaint.Dark(BackColor, 0.6f);
                            this.BorderColor = ControlPaint.Dark(BackColor, 1.0f);
                        }
                        catch { }

                        break;
                    case KitemThemes.sky:
                        this.Automakeovercolor = false;
                        this.BackColor = Color.White;
                        this.SecondColor = Color.White;
                        this.Overcolor1 = Color.Cyan;
                        this.Overcolor2 = Color.PaleTurquoise;
                        this.SelectedColor = Color.DeepSkyBlue;
                        this.BorderColor = Color.DodgerBlue;
                        break;
                    case KitemThemes.DeepSky:
                        this.Automakeovercolor = false;
                        this.BackColor = Color.Aqua;
                        this.SecondColor = Color.Teal;
                        this.Overcolor1 = Color.FromArgb(255, 64, 255, 255);
                        this.Overcolor2 = Color.FromArgb(255, 0, 223, 223);
                        this.SelectedColor = Color.Teal;
                        this.SelectedColor2 = Color.PowderBlue;
                        this.BorderColor = Color.Black;
                        break;
                    case KitemThemes.orng:
                        this.BackColor = Color.FromArgb(255, 192, 128);
                        this.SecondColor = Color.LightSalmon;
                        this.Automakeovercolor = true;
                        this.SelectedColor = Color.Orange;
                        this.BorderColor = Color.Chocolate;
                        break;
                    case KitemThemes.green:
                        this.BackColor = Color.Lime;
                        this.SecondColor = Color.LightGreen;
                        this.Automakeovercolor = true;
                        this.SelectedColor = Color.YellowGreen;
                        this.BorderColor = Color.OliveDrab;
                        break;
                    case KitemThemes.dark:
                        this.BackColor = Color.DarkGray;
                        this.SecondColor = Color.LightGray;
                        this.Automakeovercolor = true;
                        this.SelectedColor = Color.Gray;
                        this.BorderColor = Color.FromArgb(64, 64, 64);
                        break;
                    case KitemThemes.light:
                        this.BackColor = Color.FromArgb(224, 224, 224);
                        this.SecondColor = Color.White;
                        this.Automakeovercolor = true;
                        this.SelectedColor = Color.White;
                        this.BorderColor = Color.Gray;
                        break;
                    case KitemThemes.red:
                        this.BackColor = Color.Red;
                        this.SecondColor = Color.LightCoral;
                        this.Automakeovercolor = true;
                        this.SelectedColor = Color.FromArgb(255, 128, 128);
                        this.BorderColor = Color.Maroon;
                        break;
                    case KitemThemes.metro:
                        if (centerimage != null)
                        {
                            metrocolor = fnc.metrobackcolor(centerimage);
                            BackColor = secondcolor = overcolor1 = overcolor2 = selectedcolor = metrocolor;
                            this.bordercolor = fnc.Oppositecolor(metrocolor);
                        }
                        break;
                }
                Invalidate();
            }
        }
        #endregion

        KitemShapes shape = KitemShapes.grid;
        [Category("Appearance"), Description("The layout type of control"), DefaultValue(KitemShapes.grid)]
        public KitemShapes Shape
        {
            get { return shape; }
            set
            {
                shape = value;
                Invalidate();
            }
        }
        private KitemClickingEffects clickingeffect = KitemClickingEffects.Metro;
        [Category("Image Effect"), Description("Painting when mouse button is clicking"), DefaultValue(KitemClickingEffects.Metro)]
        public KitemClickingEffects ClickingEffect { get { return clickingeffect; } set { clickingeffect = value; } }

        private KitemPaintStyle paintstyle = KitemPaintStyle.DoubleFlat;
        [Category("Appearance"), Description("The painting mode"), DefaultValue(KitemPaintStyle.DoubleFlat)]
        public KitemPaintStyle Paintstyle { get { return paintstyle; } set { paintstyle = value; Invalidate(); } }

        public enum Mosstat { none = 1, over = 2, selected = 3 }
        private Mosstat mosstat = Mosstat.none;

        #endregion
       
        bool isoverpanel = false;
        bool ondown = false;
        bool isrightbtn = false;
        RectangleF subpanerec = new RectangleF();
        SizeF strngsiz;
        Color metrocolor;
       

        #region "Instalization"
        public kgriditem() : this("", null) { }
        public kgriditem(string thetext) : this(thetext, null) { }
        public kgriditem(string thetext, Size thesize)
            : this(thetext, null)
        {
            this.Size = thesize;
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
         

        }
        public kgriditem(string thetext, Image theimage)
        {

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserMouse, true);
            this.ForeColor = Color.Black;
            this.Centerimage = theimage;
            this.Width = 80;
            this.Height = 80;
            if (thetext != "")
            {
                this.Text = thetext;
            }
            threadstartcoloranimation = new ParameterizedThreadStart(DoColorAnimation);
            threadcoloranimation = new Thread(threadstartcoloranimation);
        
            this.Theme = KitemThemes.DeepSky;
            this.Theme = KitemThemes.none;
        }
        #endregion
       
         ~kgriditem()
        {
            skew = skews.none;
            if (threadcoloranimation != null)
            {
                threadcoloranimation.Abort();
            }
             }
        
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e); 
            skew = skews.none;
            if (threadcoloranimation != null)
            {
                threadcoloranimation.Abort();
            }
        }
        
        #region effects

        Bitmap tempreal;

        private bool imgready = false;

        private bool zombol = true;



        private int Cangle = 270;

        private enum skews { inner, outer, none }

        skews skew = skews.none;

        int imgeffectwait = 10;
        [Category("Effects"), Description("The time to wait until next move"), DefaultValue(10)]
        public int ImageEffectwait
        {
            get { return imgeffectwait; }
            set { imgeffectwait = value; }
        }

        int zoomstep = 20;
        [Category("Effects"), Description("The space between frame and and other between 1 and 100"), DefaultValue(20)]
        public int Zoomstep
        {
            get { return zoomstep; }
            set { zoomstep = value; }
        }

        int skewscale = 40;
        [Category("Effects"), DefaultValue(40)]
        public int SkewScale { get { return skewscale; } set { skewscale = value; } }

        int zoomscale = 50;
        [Category("Effects"), Description("Indectis the zoom long in the image"), DefaultValue(50)]
        public int ZoomScale
        {
            get { return zoomscale; }
            set { zoomscale = value; }
        }
        private Size ImgSiz;

        ImgEffectss imgeffect = ImgEffectss.skew3d;
        public enum ImgEffectss { skew3d, zooming, rotate, slide, none, blackwhite, blue }
        [Category("Effects"), DefaultValue(ImgEffectss.skew3d)]
        public ImgEffectss ImgEffect
        {
            get { return imgeffect; }
            set
            {
                if (tempreal != null) { centerimage = tempreal; Invalidate(); }
                imgeffect = value;
            }
        }

        delegate void dlg();
        void asCenterimage(Image m)
        {
            centerimage = m;

            Invalidate();
        }
        private Color currentanimationcolor;
        int currentcolorprogress = 0;
        private bool coloranimationeffect = true;
        [Category("Effects"), DefaultValue(true)]
        public bool ColorAnimationEffect
        {
            get { return coloranimationeffect; }
            set { coloranimationeffect = value; }
        }
        private int coloranimationspeed = 10;
        [Category("Effects"), DefaultValue(10)]
        public int ColorAnimationSpeed
        {
            get { return coloranimationspeed; }
            set { coloranimationspeed = value; }
        }

        private void DoColorAnimation(object isforward)
        {
            bool forward = (bool)isforward;
            Color[] grads = pnt.GetGradientColors(BackColor, overcolor1);
            if (forward)
            {
                if (currentcolorprogress <= 0)
                {
                    currentcolorprogress = 0;

                }
                for (int i = currentcolorprogress; i < grads.Length; i += coloranimationspeed)
                {
                    currentcolorprogress += coloranimationspeed;
                    currentanimationcolor = grads[i];
                    this.Invoke(new Action(() =>
                    {
                        this.Invalidate();
                    }));

                    if (currentcolorprogress >= grads.Length - 1)
                    {
                        currentcolorprogress = grads.Length - 1;

                    }
                    System.Threading.Thread.Sleep(10);
                }
                if (currentcolorprogress >= grads.Length - 1)
                {
                    currentcolorprogress = grads.Length - 1;

                }
            }
            else
            {
                if (currentcolorprogress >= grads.Length - 1)
                {
                    currentcolorprogress = grads.Length - 1;
                }
                for (int i = currentcolorprogress; i >= 0; i -= coloranimationspeed)
                {
                   currentcolorprogress -= coloranimationspeed;
                    currentanimationcolor = grads[i];
                
                    if (this.IsDisposed)
                    { return; }
                    if (this.IsHandleCreated==false)
                    { return; }
                        
                     this.Invoke(new Action(() =>
                    {  this.Invalidate();
                    }));

                    if (currentcolorprogress <= 0)
                    {
                        currentcolorprogress = 0;

                    }
                    System.Threading.Thread.Sleep(10);
                }
                currentcolorprogress = 0;
                currentanimationcolor = grads[0];
                this.Invoke(new Action(() =>
                {
                    mosstat = Mosstat.none;
                    this.Invalidate();
                }));


            }

        }
        private void doskew3din()
        {
            try
            {

          
            if (skew != skews.inner)
            { return; }
            Bitmap LTempimg;
            try
            {

                LTempimg = new Bitmap(tempreal);
            }
            catch
            {
                this.Invoke(new Action(() => { this.Text =this.Text; }));
                while (true)
                {
                    try
                    {
                        LTempimg = new Bitmap(tempreal);
                        break;
                    }
                    catch { }
                }

            }
            do
            {
                if (skew != skews.inner)
                { return; }

                this.Invoke(new Action(() =>
                {
                    Bitmap nf = new Bitmap(ftg);
                    asCenterimage(fnc.Get3dImg(nf, ImgSiz, Cangle, SkewScale));

                }));
                Cangle += 10;
                System.Threading.Thread.Sleep(imgeffectwait);

            }
            while (Cangle < 635 && skew == skews.inner);

            if (skew != skews.inner) { return; }


            centerimage = ftg;
            Invalidate();
            skew = skews.none;
            Cangle = 270;
  }
            catch
            { }

        }
        private void doskew3dout()
        {

            if (skew != skews.outer)
            { return; }
            try {
               

                do
                {
                    if (skew != skews.outer || this.IsHandleCreated == false)
                    { return; }
                    this.Invoke(new Action(() =>
                    {
                        Bitmap nf = new Bitmap(ftg);
                        asCenterimage(fnc.Get3dImg(nf, ImgSiz, Cangle, SkewScale));
                    }));
                    Cangle -= 10;
                    System.Threading.Thread.Sleep(imgeffectwait);
                }
                while (Cangle >= 270 && skew == skews.outer);
                if (skew != skews.outer)
                { return; }
                centerimage = ftg;
                Invalidate();
                Cangle = 270;
                skew = skews.none;
            }catch
            {

            }

            
        }
        PointF centzomx = new PointF(0, 0);
        PointF zoomrate = new PointF(0, 0);
        Bitmap LTempimg2 = new Bitmap(1, 1);
        private void dlgzoomin()
        {
            if (zombol == false)
            { return; }
             try {
                Bitmap LTempimg2; LTempimg2 = new Bitmap((Bitmap)centerimage.Clone()); Bitmap b;


            zoomrate = new PointF((fnc.divdec(fnc.divdec(ImgSiz.Width, 100) * fnc.divdec(zoomscale, 2), 100) * zoomstep), (fnc.divdec(fnc.divdec(ImgSiz.Height, 100) * fnc.divdec(zoomscale, 2), 100) * zoomstep));
          
                do
                {
                    b = new Bitmap(ImgSiz.Width, ImgSiz.Height);

                    Graphics g = Graphics.FromImage(b);

                    g.DrawImage(LTempimg2, centzomx.X, centzomx.Y, b.Width - (2 * centzomx.X), b.Height - (2 * centzomx.Y));


                    centzomx.X -= zoomrate.X;

                    centzomx.Y -= zoomrate.Y;


                    this.Invoke(new Action(() =>
                    {
                        asCenterimage(b);
                    }));
                    System.Threading.Thread.Sleep(imgeffectwait);
                }
                while (centzomx.X > -1 * fnc.tint(fnc.divdec(ImgSiz.Width, 100) * (ZoomScale / 2)) && centzomx.Y > -1 * fnc.tint(fnc.divdec(ImgSiz.Height, 100) * (ZoomScale / 2)) && zombol == true);
            }
            catch
            {

            }
        }
        private void dlgzoomout()
        {
            if (zombol == true)
            { return; }
            try {
                Bitmap LTempimg = new Bitmap(LTempimg2);
                do
                {
                    Bitmap b = new Bitmap(ImgSiz.Width, ImgSiz.Height);
                    Graphics g = Graphics.FromImage(b);
                    g.DrawImage(LTempimg, centzomx.X, centzomx.Y, b.Width - (2 * centzomx.X), b.Height - (2 * centzomx.Y));

                    g.Dispose();
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new Action(() =>
                        {
                            asCenterimage(b);
                        }));
                    }
                    else { return; }
                    centzomx.X += zoomrate.X;

                    centzomx.Y += zoomrate.Y;


                    System.Threading.Thread.Sleep(imgeffectwait);
                }
                while (centzomx.X < 0 && centzomx.Y < 0 && zombol == false);

                centerimage = tempreal;
                Invalidate();
            }
            catch { }
        }

        private void dorotatein()
        {
            // if (zombol == false)
            // { return; }
            Bitmap LTempimg2; LTempimg2 = (Bitmap)centerimage.Clone(); Bitmap b;



            do
            {
                b = new Bitmap(ImgSiz.Width, ImgSiz.Height);

                Graphics g = Graphics.FromImage(b);

                g.TranslateTransform(b.Width / 2, b.Height / 2);
                g.RotateTransform(Cangle);

                g.DrawImage(LTempimg2, -1 * b.Width / 2, -1 * b.Height / 2, b.Width, b.Height);

                Cangle += 10;



                this.Invoke(new Action(() =>
                {
                    asCenterimage(b);
                }));
                System.Threading.Thread.Sleep(imgeffectwait);
            }
            while (Cangle < 720 && zombol == true);

        }
        ParameterizedThreadStart threadstartcoloranimation;
        Thread threadcoloranimation;
        Bitmap ftg;
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Cursor = Cursors.Hand;
            if (mosstat != Mosstat.selected)
            {
                mosstat = Mosstat.over;
                Invalidate();
                if (coloranimationeffect)
                {
                    if (threadcoloranimation.IsAlive)
                    {
                        threadcoloranimation.Abort();
                    }
                    threadcoloranimation = new Thread(threadstartcoloranimation);

                    threadcoloranimation.Start(true);

                }
            }
            if (imgready && centerimage != null)
            {  ftg=(Bitmap)tempreal.Clone();
                switch (ImgEffect)
                {
                      
                    case ImgEffectss.skew3d:

                        skew = skews.inner;
                        dlg dlgvd1 = doskew3din;
                        dlgvd1.BeginInvoke(null, null);

                        break;
                    case ImgEffectss.zooming:

                        zombol = true;
                        dlg dlgvd2 = dlgzoomin;
                        dlgvd2.BeginInvoke(null, null);

                        break;
                    case ImgEffectss.rotate:
                        zombol = true;
                        dlg ff = dorotatein;
                        ff.BeginInvoke(null
                            , null);
                        break;
                }
            }

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isoverpanel = false;
            this.Cursor = Cursors.Default;
            if (!(mosstat == Mosstat.selected))
            {
                if (coloranimationeffect)
                {
                    if (threadcoloranimation != null)
                    {
                        if (threadcoloranimation.IsAlive)
                        {
                            threadcoloranimation.Abort();
                        }
                        threadcoloranimation = new Thread(threadstartcoloranimation);

                        threadcoloranimation.Start(false);
                    }
                }
                else
                {
                    mosstat = Mosstat.none;
                    Invalidate();
                }
            }

            if (ImgEffect == ImgEffectss.zooming)
            {

                zombol = false;
                dlg dlgvdout = dlgzoomout;
                dlgvdout.BeginInvoke(null, null);
            }
            else if (ImgEffect == ImgEffectss.skew3d)
            {
                if (skew != skews.none)
                {
                    skew = skews.outer;



                    dlg dlgvd3d = doskew3dout;
                    dlgvd3d.BeginInvoke(null, null);

                }

            }




        }

        #endregion

        #region Events

        public event kgrideventhandler SelectedTaskChanged;
        public void OnSelectedTaskChanged(object sender,kgrideventargs e)
        {
            selectedtaskindx = e.item.Index;
            if (SelectedTaskChanged!=null)
            { SelectedTaskChanged(sender,e); }
        }   
      
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);

            if (this.Theme == KitemThemes.defult)
            {
                Invalidate();
            }

        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (automakeovercolors == true)
            { Makecolors(); }
        }
        ///not working
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);


            kgrid kg = new kgrid();
            if (this.Parent is kgrid)
            {

                kg = (kgrid)this.Parent;
            }
            else if (this.Parent.Parent is kgrid)
            { kg = (kgrid)this.Parent.Parent; }

            switch (e.KeyCode)
            {
                case Keys.Right:

                    if (this.Index < kg.Items.Count - 1)
                    {
                        kg.Items[this.Index + 1].Iselected = true;
                    }

                    break;
                case Keys.Left:
                    if (this.Index - 1 > -1)
                    {
                        kg.Items[this.Index - 1].Iselected = true;
                    }
                    break;
                case Keys.Up:
                    int trns = kg.Width / this.Width;
                    if (this.Index - trns > -1)
                    {
                        kg.Items[this.Index - trns].Iselected = true;
                    }
                    break;
                case Keys.Down:
                    int trns2 = kg.Width / this.Width;
                    if (this.Index + trns2 < kg.Items.Count - 1)
                    {
                        kg.Items[this.Index + trns2].Iselected = true;
                    }
                    break;
            }


        }
    
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ondown = true;
            if (e.Button == MouseButtons.Right)
            { isrightbtn = true; }
            else { isrightbtn = false; }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (showsubpanel)
            {
                RectangleF subrec = new RectangleF(this.Width - subpanelarrowwidth, 0, subpanelarrowwidth, Height);
                if (subrec.Contains(e.Location))
                {
                    if (isoverpanel == false)
                    {
                        isoverpanel = true;
                        Invalidate();
                    }
                }
                else if (isoverpanel == true) { isoverpanel = false; Invalidate(); }
            }
        }
   
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ondown = false;
            isoverpanel = false;
           Invalidate();
        }
        
        protected override void OnDoubleClick(EventArgs e)
        {

            base.OnDoubleClick(e);
            if (!isrightbtn)
            {
                if (this.Parent is kgrid)
                {
                    kgrid kg = (kgrid)this.Parent;
                    kgrideventargs kea = new kgrideventargs();
                    kea.item = this;
                    kg.OnItemDoubleClick(this.Parent, kea);
                }
                else if (this.Parent.Parent is kgrid)
                {
                    kgrid kg = (kgrid)this.Parent.Parent;
                    kgrideventargs kea = new kgrideventargs();
                    kea.item = this;
                    kg.OnItemDoubleClick(this.Parent, kea);

                }

            }
        }



        protected override void OnClick(EventArgs e)
        {



            if (!(isrightbtn && isselected))
            {
                this.Iselected = !this.Iselected;


            }
            if (isoverpanel)
            {
                ShowTasks();
            }
            else
            {
                base.OnClick(e);
            }
        }

     
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (this.Parent != null && this.Parent.Parent != null)
            {
                if (this.Parent.Parent is kgrid && this.Parent is kDynamkPanel)
                {
                    if (((kgrid)this.Parent.Parent).autosizegroups)
                    { ((kgrid)this.Parent.Parent).makegruopssize(); }
                }
            }
            if (centerimage != null)
            {
                ImgSiz = fnc.OImg(centerimage, this.DisplayRectangle, centerimagelayout).Size;
            }
            textsize();

            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            textsize();
            Invalidate();
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
           
        }
        #endregion

        private void textsize()
        {
            strngsiz = this.CreateGraphics().MeasureString(Text, Font, this.Width - 10);
            SizeF linestng = this.CreateGraphics().MeasureString(Text, Font, new SizeF(this.Width - 10, 20));

            if (strngsiz.Height > 20)
            {

                if (strngsiz.Height > 2 * linestng.Height)
                {
                    strngsiz.Height = 2 * linestng.Height;

                }
            }
        }
        public void ShowTasks()
        {
            TasksForm tf = new TasksForm();
            tf.orgn = this;
            tf.selectedindexchanged += new kgrideventhandler(OnSelectedTaskChanged);
           
            tf.AddTasks(this.tasks.ToArray());
            tf.Show();
           
        }
       
        #region paint

        
       
        
      
     
        public int MesureInfosheghit()
        {
            float ly = 0;
            if (this.infos.Count > 0)
            {
                Graphics g = this.CreateGraphics();           
                SizeF strngsz = g.MeasureString(this.Text, this.Font, this.Width-10);
                if (strngsz.Height > 2 * g.MeasureString(this.Text, this.Font).Height)
                { strngsz.Height = 2 * g.MeasureString(this.Text, this.Font).Height; }
                ly = strngsz.Height + 2;
                foreach (string stng in this.infos)
                {
                    Font ft = new Font(this.Font.Name, this.Font.Size - 1);
                    strngsz = g.MeasureString(stng, ft);
                    ly += strngsz.Height + 2;
                  
                }
            }
            return (int)(ly+1);
        }
     
        public Bitmap overb(Mosstat mossy)
        {
            Mosstat tempmosstat = mosstat;
            mosstat = mossy;
            Bitmap b = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (ondown && ClickingEffect == KitemClickingEffects.Button)
            {
                g.TranslateTransform(1, 1);
            }
            float lg = 270;
            Pen Mpn = new Pen(this.BorderColor, this.Borderwidth);
            LinearGradientBrush Mhlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, (float)this.Height / 5f * 2)), BackColor, SecondColor, lg);
            LinearGradientBrush Mlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, this.Height)), BackColor, SecondColor, lg);

            SolidBrush Msd = new SolidBrush(BackColor);
            SolidBrush M2sd = new SolidBrush(secondcolor);
            switch (mosstat)
            {
                case Mosstat.over:

                    Mhlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, (float)this.Height / 5f * 2)), currentanimationcolor, Overcolor2, lg);
                    Mlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, this.Height)), currentanimationcolor, Overcolor2, lg);
                    Msd.Color = currentanimationcolor;//overcolor1;
                    M2sd.Color = Overcolor2;
                    break;
                //============================================================================
                case Mosstat.none:

                    break;
                //============================================================================
                case Mosstat.selected:

                    Mhlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, (float)this.Height / 5f * 2)), SelectedColor, SelectedColor2, lg);
                    Mlgb = new LinearGradientBrush(new RectangleF(new PointF(0, 0), new SizeF(this.Width, this.Height)), SelectedColor, SelectedColor2, lg);
                    Msd.Color = SelectedColor;
                    M2sd.Color = selectedcolor2;
                    break;
            }
            #region mode

            if (Theme == KitemThemes.metro && (skew != skews.none || centzomx.X < 0 && centzomx.Y < 0))
            {
                metrocolor = fnc.metrobackcolor(centerimage);
                g.Clear(metrocolor);
            }
            else
            {
                if (theme == KitemThemes.defult && mosstat != Mosstat.selected)
                {
                    g.Clear(Color.Transparent);

                }
                else
                {
                    switch (Paintstyle)
                    {
                        case KitemPaintStyle.Gradient:

                            g.FillRectangle(Mlgb, this.DisplayRectangle);
                            break;

                        case KitemPaintStyle.DoubleFlat:

                            g.FillRectangle(Mhlgb, Mhlgb.Rectangle);
                            g.FillRectangle(Msd, new Rectangle(0, this.Height / 5 * 2, this.Width, this.Height - this.Height / 5 * 2));

                            break;
                        case KitemPaintStyle.Flat:
                            g.FillRectangle(Msd, this.DisplayRectangle);
                            break;
                    }
                }
            }
            #endregion

            StringFormat stf1 = new StringFormat();
            stf1.Alignment = StringAlignment.Center;
            stf1.FormatFlags = StringFormatFlags.LineLimit;
            stf1.LineAlignment = StringAlignment.Center;
            stf1.Trimming = StringTrimming.EllipsisCharacter;

            RectangleF strngrec = new RectangleF(0, 0, 1, 1);
            Rectangle imgrec = new Rectangle(0, 0, 1, 1);
            int imgreceidth, imgrechit;


            #region  layout
            switch (Shape)
            {
                case KitemShapes.grid:
                    strngrec.X = 2; strngrec.Width = this.Width - 4;

                    int texth = (int)this.strngsiz.Height;
                    if (!showtext)
                    { texth = 0; }
                    if (showimage && centerimage != null)
                    {
                        imgreceidth = this.Width - 4;
                        imgrechit = this.Height - texth - 4;
                        if (imgreceidth < 0) imgreceidth = 0;
                        if (imgrechit < 0) imgrechit = 0;


                        imgrec = new Rectangle(2, 2, imgreceidth, imgrechit);

                        if (Showimageborder == true)
                        { g.DrawRectangle(new Pen(this.BorderColor, 1), imgrec); }

                        if (!(imgreceidth == 0 || imgrechit == 0))
                        {
                            try
                            {
                                if (centerimagelayout != ImageLayout.Tile)
                                { g.DrawImage(centerimage, fnc.OImg(centerimage, imgrec, CenterImageLayout)); }
                                else
                                {
                                    g.FillRectangle(new TextureBrush(centerimage), fnc.OImg(centerimage, imgrec, centerimagelayout));
                                }
                            }
                            catch { }
                        }
                        strngrec.Height = strngsiz.Height;
                    }
                    else
                    {
                        imgrec = new Rectangle(0, 0, 0, 0);
                        strngrec.Height = this.Height - 4;
                    }
                    strngrec.Y = imgrec.Height + imgrec.X + 2;
                    if (showtext)
                    {
                       g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), strngrec, stf1);
                    }
                    break;
                case KitemShapes.metro:
                    if (centerimage != null)
                    {
                        g.Clear(fnc.metrobackcolor(centerimage));
                        strngrec = new RectangleF(10, this.Size.Height - 25, this.Size.Width - 20, 20);

                       g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), strngrec, stf1);

                        imgreceidth = this.Size.Width - 20;
                        imgrechit = this.Size.Height - 35;
                        if (imgreceidth < 0) { imgreceidth = 0; }
                        if (imgrechit < 0) { imgrechit = 0; }
                        imgrec = new Rectangle(10, 10, imgreceidth, imgrechit);

                        if (Showimageborder == true)
                        { g.DrawRectangle(new Pen(this.BorderColor, this.Borderwidth / 3), imgrec); }


                        if (centerimagelayout != ImageLayout.Tile)
                        { g.DrawImage(centerimage, fnc.OImg(centerimage, imgrec, CenterImageLayout)); }
                        else
                        {
                            g.FillRectangle(new TextureBrush(centerimage), fnc.OImg(centerimage, imgrec, centerimagelayout));
                        }
                    }
                    break;
                case KitemShapes.tile:


                    if (showimage && this.centerimage != null)
                    {              
                        //img width is 30% of control width                        
                        imgreceidth = (int)((this.Width / 100f) * 30f);
                        imgrechit = this.Height - 4;
                        if (imgreceidth < 0) imgreceidth = 0;
                        if (imgrechit < 0) imgrechit = 0;
                        if (!showtext)
                        { imgreceidth = Width - 4; }
                        imgrec = new Rectangle(2, 2, imgreceidth, imgrechit);
                    }
                    else
                    {
                        imgrec = new Rectangle(0, 0, 0, 0);
                    }

                    if (centerimage != null && showimage)
                    {
                        imgrec = fnc.OImg(centerimage, imgrec, centerimagelayout);
                        imgrec.X = 2;
                        if (centerimagelayout != ImageLayout.Tile)
                        { g.DrawImage(centerimage, imgrec); }
                        else
                        {
                            g.FillRectangle(new TextureBrush(centerimage), imgrec);
                        }
                        if (Showimageborder == true)
                        { g.DrawRectangle(new Pen(this.BorderColor, 1), imgrec); }

                    }

                    if (showtext)
                    {
                        int subrecwd = 0;
                        if (showsubpanel) subrecwd = subpanelarrowwidth;
                        if (this.infos.Count > 0)
                        {
                            strngrec = new RectangleF(1+ imgrec.Width, 1, this.Width - 1 - imgrec.Width-subrecwd , 20);
                            stf1.Alignment = StringAlignment.Near;
                            SizeF strngsz = g.MeasureString(this.Text, this.Font, (int)strngrec.Width);
                            if (strngsz.Height > 2 * g.MeasureString(this.Text, this.Font).Height)
                            { strngsz.Height = 2 * g.MeasureString(this.Text, this.Font).Height; }
                            strngrec.Height = strngsz.Height;
                           g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), strngrec, stf1);
                            float ly = strngsz.Height + 2;
                            foreach (string stng in this.infos)
                            {
                                Font ft = new Font(this.Font.Name, this.Font.Size - 1);
                                strngsz = g.MeasureString(stng, ft);
                                strngrec = new RectangleF(5 + imgrec.Width, ly, strngrec.Width, strngsz.Height);

                                if (ly + strngsz.Height + 2 < this.Height)
                                {
                                   g.DrawString(stng, ft, new SolidBrush(this.ForeColor), strngrec, stf1);
                                  
                                
                                }
                                else
                                {
                                    break;
                                }
                                ly += strngsz.Height + 2;
                                
                            }                         
                            subpanerec = new RectangleF(strngrec.Right, 0, subrecwd, Height);
                            float tnglecntr = 3; 
                            Color subrecbackcol = isoverpanel == true ? selectedcolor2 : selectedcolor;
                            g.FillRectangle(new SolidBrush(subrecbackcol), subpanerec);
                            g.FillPolygon(new SolidBrush(bordercolor), kcombobox.Triangle(new PointF
                                (subpanerec.X + subpanerec.Width / 2 - subpanerec.Width / (tnglecntr * 2),
                                subpanerec.Y + subpanerec.Height / 2 - subpanerec.Height / (tnglecntr * 2))
                                , new SizeF(subpanerec.Width / tnglecntr, subpanerec.Height / tnglecntr)));
                            if (borderfilter_normal.IsAll)
                            {
                                g.DrawRectangle(new Pen(bordercolor, 2f), subpanerec.X, subpanerec.Y + 1, subpanerec.Width - 1, subpanerec.Height - 2);
                            }
                        }
                        else
                        {
                            strngrec = new RectangleF(1 +imgrec.Right, 1, this.Width - 1 - imgrec.Right-subrecwd , this.Height - 2);

                            stf1.Alignment = StringAlignment.Center;
                            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), strngrec,stf1);
                            subpanerec = new RectangleF(strngrec.Right,0,subrecwd, Height);
                            float tnglecntr = 3;
                            Color subrecbackcol = isoverpanel == true ? selectedcolor2 : selectedcolor;
                            g.FillRectangle(new SolidBrush(subrecbackcol), subpanerec);
                            g.FillPolygon(new SolidBrush(bordercolor),kcombobox.Triangle(new PointF
                                (subpanerec.X+subpanerec.Width/2-subpanerec.Width/(tnglecntr*2),
                                subpanerec.Y + subpanerec.Height / 2 - subpanerec.Height / (tnglecntr*2))
                                , new SizeF(subpanerec.Width / tnglecntr, subpanerec.Height / tnglecntr)));
                            if (borderfilter_normal.IsAll)
                            {
                                g.DrawRectangle(new Pen(bordercolor, 2f), subpanerec.X, subpanerec.Y + 1, subpanerec.Width - 1, subpanerec.Height - 2);
                            } 
                        }      
                    }


                    break;
            }
            #endregion

            float oldwd = Mpn.Width;
            Mpn.Width = borderwidth;

            switch (mosstat)
            {
                case Mosstat.none:

                    if (borderfilter_normal.TopLeft)
                    { g.DrawLine(Mpn, 0, this.Height, this.Width, this.Height); }
                    if (borderfilter_normal.BottomLeft)
                    { g.DrawLine(Mpn, 0, 0, 0, this.Height); }
                    if (borderfilter_normal.BottomRight)
                    { g.DrawLine(Mpn, this.Width, 0, this.Width, this.Height); }
                    if (borderfilter_normal.TopRight)
                    { g.DrawLine(Mpn, 0, 0, this.Width, 0); }

                    break;
                case Mosstat.over:
                   
                    if (this.BorderFilter_Over.TopLeft)
                    {
                      g.DrawLine(Mpn, 0, this.Height, this.Width, this.Height);
                    }
                    if (borderfilter_Over.BottomLeft)
                    {
                      g.DrawLine(Mpn, 0, 0, 0, this.Height);

                    }
                    if (borderfilter_Over.BottomRight)
                    { g.DrawLine(Mpn, this.Width, 0, this.Width, this.Height); 
                    }
                    if (borderfilter_Over.TopRight)
                    {
                        g.DrawLine(Mpn, 0, 0, this.Width, 0);
                    }



                    break;
                case Mosstat.selected:
                    if (this.borderfilter_Selected.TopLeft)
                    { g.DrawLine(Mpn, 0, this.Height, this.Width, this.Height); }
                    if (borderfilter_Selected.BottomLeft)
                    { g.DrawLine(Mpn, 0, 0, 0, this.Height); }
                    if (borderfilter_Selected.BottomRight)
                    { g.DrawLine(Mpn, this.Width, 0, this.Width, this.Height); }
                    if (borderfilter_Selected.TopRight)
                    { g.DrawLine(Mpn, 0, 0, this.Width, 0); }

                    Mpn.Width = 8;
                    if (showselectedtrangle)
                    {
                        Point[] pnts = new Point[3];
                        pnts[0] = new Point(this.Width - 35, borderwidth / 2); pnts[1] = new Point(this.Width - borderwidth / 2, 35);
                        pnts[2] = new Point(this.Width - borderwidth / 2, borderwidth / 2);
                        g.FillPolygon(new SolidBrush(Color.FromArgb(190, BackColor)), pnts);
                        g.DrawPolygon(new Pen(this.BorderColor, borderwidth / 2), pnts);
                        Pen pn = new Pen(this.bordercolor, 3);
                        pnts[0] = new Point(this.Width - 21, 12); pnts[1] = new Point(this.Width - 16, 16); pnts[2] = new Point(this.Width - 8, 8);
                        g.DrawLine(pn, pnts[0], pnts[1]); g.DrawLine(pn, pnts[1], pnts[2]);
                    }
                    break;
            }
            Mpn.Width = oldwd;
            #region      Fill section area  =====================================================================================

            if (fillselectedarea)
            {
                RectangleF fillrec = this.DisplayRectangle;
            fillrec.Width = FillPercent / 100f * this.DisplayRectangle.Width;
           
                int ca = (int)(filledareatransparency * 255F);
                if (fillrec.Width > 0 && fillrec.Height > 0)
                {
                    if (filledareapainting == FilledAreaPainting.FilledAreaColorproperty)
                    {
                        g.FillRoundedRectangle(new SolidBrush(Color.FromArgb(ca, filledareacolor)), fillrec, 0, RectangeEdgeFilter.Noun());
                    }
                    else
                    {
                        HatchBrush hb = new HatchBrush(pattrentype, Color.FromArgb(ca, FilledAreaColor), Color.FromArgb(ca, selectedcolor));
                        g.FillRoundedRectangle(hb, fillrec, 1f, RectangeEdgeFilter.Noun());

                    }

                }

               
            }





            #endregion 
            if (!Enabled)
            {

                for (int r = 0; r < b.Width; r++)
                {
                    for (int c = 0; c < b.Height; c++)
                    {
                        int m = (b.GetPixel(r, c).R + b.GetPixel(r, c).G + b.GetPixel(r, c).B) / 3;
                        b.SetPixel(r, c, Color.FromArgb(m, m, m));
                    }
                }
            }
           
            if (domainarrngeeffect)
            { b = TransParentbBitmab(b, 100); }


            g.Dispose();


            if (imgready == false)
            { imgready = true; }
            mosstat = tempmosstat;
            return b;
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (ondown && ClickingEffect == KitemClickingEffects.Metro)
            {

               e.Graphics.DrawImage(overb(mosstat), 2, 2, this.Width - 4, this.Height - 4);
            }
            else
            {
                e.Graphics.DrawImage(overb(mosstat), 0, 0);
            }

        }
        #endregion
        Bitmap TransParentbBitmab(Bitmap b, int opicty)
        {
            Bitmap v = new Bitmap(b.Width, b.Height);
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    v.SetPixel(x, y, Color.FromArgb(opicty, b.GetPixel(x, y)));
                }
            }
            return v;
        }
        public bool domainarrngeeffect = false;

    }
    #endregion


    #region KDynmkPanel
    public enum MainAlignments { left, right }
    public enum BorderFilter
    {
        none = 0,
        Left = 1,
        Top = 2,
        Bottom = 4,
        Right = 8,
        All = Left | Top | Bottom | Right
    }
    [DefaultEvent("openingclosing")]
    public class kDynamkPanel : System.Windows.Forms.Panel
    {
        [Browsable(false)]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }

        Size mnht;
        [Category("Layout")]
        public Size RealHeight
        {
            get { return mnht; }
            set { mnht = value; Invalidate(); }
        }
       
        Image timage;// = pntfctmix.Properties.Resources.color__graphics__paint_icon;
        [Category("Appearance")]
        public Image Titleimage { get { return timage; } set { timage = value; Invalidate(); } }

        private bool enableopenandclose = true;
        [Category("Behavior")]
        public bool Enableopenandclose
        {
            get { return enableopenandclose; }
            set { enableopenandclose = value; }
        }

        private bool showopenkey = true;
        [Category("Behavior")]
        public bool ShowOpenKey
        {
            get { return showopenkey; }
            set { showopenkey = value; Invalidate(); }
        }
        bool opentopdown = true;
         [Category("Behavior"),DefaultValue(true)]
        public bool OpenTopDown
        {
            get { return opentopdown; }
            set { opentopdown = value; }
        }

        bool isopened = true;
        [Category("Behavior")]
        public bool IsOpened
        {

            get { return isopened; }
            set
            {
                if (isopened == value) { return; }
                if (value)
                {
                    doopen();
                }
                else
                {
                    doclose();
                }
            }
        }



        enum moss { openover, colsedover, over, none }
        moss mos = moss.none;

        StringAlignment ttitlealignment = StringAlignment.Near;
        [Category("Appearance")]
        public StringAlignment TitleAlignment { get { return ttitlealignment; } set { ttitlealignment = value; Invalidate(); } }


        MainAlignments keyalgment = MainAlignments.right;
        [Category("Appearance")]
        public MainAlignments KeyAlignment { get { return keyalgment; } set { keyalgment = value; Invalidate(); } }

        MainAlignments imgalgment = MainAlignments.right;
        [Category("Appearance")]
        public MainAlignments ImageAlignment { get { return imgalgment; } set { imgalgment = value; Invalidate(); } }



        string title = "New KGroupBox";
        [Category("Appearance")]
        public string Title { get { return title; } set { title = value; Invalidate(); } }

        Color tbrdrclr = Color.Black;
        [Category("Appearance")]
        public Color BorderColor { get { return tbrdrclr; } set { tbrdrclr = value; Invalidate(); } }

        Color ttitlebckclr = Color.FromArgb(205, 100, 23);
        [Category("Appearance")]
        public Color TitleBackColor { get { return ttitlebckclr; } set { ttitlebckclr = value; Invalidate(); } }

        Color ttitleforeclr = Color.Black;
        [Category("Appearance")]
        public Color TitleForeColor { get { return ttitleforeclr; } set { ttitleforeclr = value; Invalidate(); } }


        Font ttitlefont;
        [Category("Appearance")]
        public Font TitleFont
        {
            get { return ttitlefont; }
            set
            {
                ttitlefont = value;

                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mos = moss.over;
            Invalidate();
        }
        bool selectable = true;

        public bool SelectAble
        {
            get { return selectable; }
            set { selectable = value; }
        }
        bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selectable) selected = value;
                Invalidate();
            }
        }
        public event EventHandler openingclosing;
        public void OnOpeningClosing()
        {
            if (openingclosing !=null)
            {
            this.Invoke(new Action(()=>{ this.openingclosing(this, new EventArgs()); }));
            }
        }
        #region Draw selection
        Boolean drag = false;
        Rectangle selctionrec = new Rectangle(0, 0, 0, 0);
        Point startPoint;
       
        protected override void OnMouseDown(MouseEventArgs e)
        {

            base.OnMouseDown(e);
            if (this.Parent != null)
            {
                if (this.Parent is kgrid)
                {
                    kgrid kg = (kgrid)this.Parent;
                    if (kg.Mouseselection)
                    {
                        foreach (kgriditem kgi in kg.Items)
                        {
                            if (kgi.Iselected == true)
                            {
                                kgi.Iselected = false;
                            }
                        }
                        drag = true;
                        startPoint = MousePosition;
                    }
                }
            }


        }
        [Browsable(false)]
        public bool OnSelection { get { return drag; } }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);


            if (drag == true)
            {
                kgrid kg = (kgrid)this.Parent;

                ControlPaint.DrawReversibleFrame(selctionrec, Color.Black, FrameStyle.Dashed);

                selctionrec.Location = startPoint;

                Size ssz = (Size)MousePosition;
                if (ssz.Width > kg.PointToScreen(Point.Empty).X + kg.Width)
                {
                    ssz.Width = kg.PointToScreen(Point.Empty).X + kg.Width - 1;
                }
                else if (ssz.Width < kg.PointToScreen(Point.Empty).X)
                {
                    ssz.Width = kg.PointToScreen(Point.Empty).X + 1;
                }
                if (ssz.Height > kg.PointToScreen(Point.Empty).Y + kg.Height)
                {
                    ssz.Height = kg.PointToScreen(Point.Empty).Y + kg.Height - 1;
                }
                else if (ssz.Height < kg.PointToScreen(Point.Empty).Y)
                {
                    ssz.Height = kg.PointToScreen(Point.Empty).Y + 1;
                }
                selctionrec.Size = ssz - (Size)selctionrec.Location;


                ControlPaint.DrawReversibleFrame(selctionrec, Color.Black, FrameStyle.Dashed);

                Rectangle controlRectangle;

                if (selctionrec.Width < 0)
                {
                    selctionrec.X = selctionrec.X + selctionrec.Width;
                    selctionrec.Width = -1 * selctionrec.Width;
                }
                if (selctionrec.Height < 0)
                {
                    selctionrec.Y = selctionrec.Y + selctionrec.Height;
                    selctionrec.Height = -1 * selctionrec.Height;
                }
                for (int i = 0; i < kg.Items.Count; i++)
                {
                    controlRectangle = kg.Items[i].RectangleToScreen(kg.Items[i].ClientRectangle);
                    if (controlRectangle.IntersectsWith(selctionrec))
                    {
                        if (kg.Items[i].Iselected == false)
                        { kg.Items[i].Iselected = true; }
                    }
                    else
                    { if (kg.Items[i].Iselected)kg.Items[i].Iselected = false; }
                }

            }
            else
            {
                RectangleF taskrec = new RectangleF(this.Width - 30, 0, 30, Height);
                if (taskrec.Contains(e.Location))
                {
                   
                }
               moss temp = mos;
                Point trngmosloc = this.PointToScreen(Point.Empty);
                Point trngloc = new Point(MousePosition.X - trngmosloc.X, MousePosition.Y - trngmosloc.Y);
                if (keyrec.IntersectsWith(new Rectangle(trngloc, keyrec.Size)))
                {
                    if (IsOpened)
                    { mos = moss.openover; }
                    else { mos = moss.colsedover; }

                }
                else
                {
                    mos = moss.over;
                }
                if (temp != mos)
                {
                    Invalidate();
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e); if (drag)
            {
                drag = false;
                selctionrec = new Rectangle(0, 0, 0, 0);


            }
        }

        #endregion
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mos = moss.none;
            Invalidate();
        }

        void open()
        {
            if (tclose != null) { tclose.Abort(); }

            busy = true;
            if (opentopdown)
            {
                do
                {
                    this.Invoke(new Action(() =>
                    { this.Height += fnc.tint(fnc.divdec(mnht.Height, 10) * 1); }));
                    OnOpeningClosing();
                    Thread.Sleep(7);
                } while (this.Height <= mnht.Height - 5);
                this.Invoke(new Action(() => { this.Height = mnht.Height; isopened = true; AutoScroll = tempscrl; }));
                OnOpeningClosing();
         
            }
            else
            {
                do
                {
                    this.Invoke(new Action(() =>
                    { this.Width += fnc.tint(fnc.divdec(mnht.Width, 10) * 1); }));

                    Thread.Sleep(7);
                    OnOpeningClosing();
                } while (this.Width <= mnht.Width - 5);
                this.Invoke(new Action(() => { this.Width = mnht.Width; isopened = true; AutoScroll = tempscrl; }));
                OnOpeningClosing();
         
            }
            busy = false;
        }
        void close()
        {
            if (topen != null) { topen.Abort(); }

            busy = true;
            int stngh = (int)this.CreateGraphics().MeasureString(this.Title, this.ttitlefont).Height + 5;
            if (opentopdown==false)
            {
                stngh = (int)this.CreateGraphics().MeasureString(this.Title, this.ttitlefont).Width + 30;
            }
            if (this.title == "")
            { stngh = 15; }
            if (opentopdown)
            {
                do
                {
                    this.Invoke(new Action(() =>
                    { this.Height -= fnc.tint(fnc.divdec(mnht.Height, 10) * 1); }));
                    OnOpeningClosing();
                    Thread.Sleep(7);
                } while (this.Height >= stngh + 5);
                this.Invoke(new Action(() => { this.Height = stngh + 3; isopened = false; }));
                OnOpeningClosing();
         
            }
            else
            {
                do
                {
                    this.Invoke(new Action(() =>
                    { this.Width -= fnc.tint(fnc.divdec(mnht.Width, 10) * 1); }));
                    OnOpeningClosing();
                    Thread.Sleep(7);
                } while (this.Width >= stngh + 5);
                this.Invoke(new Action(() => { this.Width= stngh + 3; isopened = false; }));
                OnOpeningClosing();
         
            }
            busy = false;
        }

        bool tempscrl;
        Thread topen, tclose;
        void doopen()
        {
            if (this.Enableopenandclose == false) return;

            if (isopened == false)
            {
                ThreadStart ts = new ThreadStart(open);
                topen = new Thread(ts);

                topen.Start();
            }
        }
        void doclose()
        {
            if (this.Enableopenandclose == false) return;

            if (isopened)
            {
                mnht = this.Size;
                tempscrl = AutoScroll;

                this.Focus();
                this.AutoScroll = false;
                ThreadStart ts = new ThreadStart(close);
                tclose = new Thread(ts);
                tclose.Start();
            }
        }
        bool busy = false;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!busy)
            {
                mnht = this.Size;
            }
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (mos == moss.openover || mos == moss.colsedover)
            {

                IsOpened = !IsOpened;

            }
            else
            { if (selectable)Selected = !Selected; }
        }

        Color ttrngclr1 = Color.Black;
        [Category("Appearance")]
        public Color KeyColor1 { get { return ttrngclr1; } set { ttrngclr1 = value; Invalidate(); } }

        Color ttrngclr2 = Color.Silver;
        [Category("Appearance")]
        public Color KeyColor2 { get { return ttrngclr2; } set { ttrngclr2 = value; Invalidate(); } }

        bool titleline = true;
        [Category("Appearance")]

        public bool Titleline
        {
            get { return titleline; }
            set { titleline = value; Invalidate(); }
        }
        bool undertitleline = true;
        [Category("Appearance")]
        public bool UnderTitleLine
        {
            get { return undertitleline; }
            set { undertitleline = value; Invalidate(); }
        }

        public kDynamkPanel() : this("New KGroupBox") { }

        public override Rectangle DisplayRectangle
        {
            get
            {
                if (title == "")
                {
                    return new Rectangle(0, 20, Width, Height);
                }
                else
                {
                    return new Rectangle(0, (int)this.CreateGraphics().MeasureString(Title, TitleFont).Height + 8, this.Width, this.Height - (int)this.CreateGraphics().MeasureString(Title, TitleFont).Height - 8);
                } 
            }
        }
     

        public kDynamkPanel(string thecaption)
        {

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            ttitlefont = this.Font;
            this.Padding = new Padding(10, (int)this.CreateGraphics().MeasureString(Title, Font).Height + 5, 0, 0);

            AutoScroll = false;
            tempscrl = AutoScroll;
            BackColor = Color.White;

            timage = null;
            TitleBackColor = Color.FromArgb(64, 64, 64);
            TitleForeColor = Color.White;
            this.KeyColor1 = Color.White;
            this.KeyColor2 = Color.Aqua;
            mnht = this.Size;

            this.Title = thecaption;

        }


        private BorderFilter borderfilter = BorderFilter.All;
        [Category("Appearance")]
        public BorderFilter Borderfilter
        { get { return borderfilter; } set { borderfilter = value; Invalidate(); } }

        private int borderwidth = 1;
        [Category("Appearance")]
        public int BorderWidth
        {
            get { return borderwidth; }
            set { borderwidth = value; Invalidate(); }
        }


        Rectangle keyrec = new Rectangle(0, 0, 10, 10);
        int spc = 5;

        protected override void OnPaint(PaintEventArgs e)
        {
            

            Bitmap b = new Bitmap(Width, Height); Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.TranslateTransform(this.AutoScrollPosition.X, AutoScrollPosition.Y);

            Color keycol = KeyColor1;
            Color titlebk = TitleBackColor;
            if (mos == moss.colsedover || mos == moss.openover)
            {
                keycol = KeyColor2;
                titlebk = ControlPaint.Light(titlebk, .5f);
                if (titlebk == TitleBackColor)
                {
                    titlebk = ControlPaint.Dark(titlebk, .3f);
                }
            }
            if (mos == moss.over)
            {
                titlebk = ControlPaint.Light(titlebk, .5f);
                if (titlebk == TitleBackColor)
                {
                    titlebk = ControlPaint.Dark(titlebk, .3f);
                }
            }
            if (Selected)
            {
                titlebk = ControlPaint.Dark(titlebk, .5f);
                if (titlebk == TitleBackColor)
                {
                    titlebk = ControlPaint.Light(titlebk, .5f);
                }
            }
            Size textsz = Size.Round(g.MeasureString(this.Title, this.TitleFont));
            if (Title == "")
            { textsz = new Size(1, 15); }
            Rectangle strngrec = new Rectangle();
            Rectangle imgrec = new Rectangle();
            Rectangle liner = new Rectangle();

            Pen pn = new Pen(BorderColor, borderwidth);

            #region layout
            strngrec.Height = textsz.Height;
            strngrec.Y = 5;
            keyrec.Y = (int)(strngrec.Height / 2) + 5 - keyrec.Height / 2;
            if (Titleimage != null)
            {
                imgrec.Height = imgrec.Width = strngrec.Height;
                imgrec.Y = 5;
            }


            liner.Y = keyrec.Y + keyrec.Height / 2;


            #region Key&image layout
            switch (KeyAlignment)
            {
                case MainAlignments.right:

                    keyrec.X = this.Width - (10) - keyrec.Width;


                    strngrec.Width = Width - keyrec.Width - imgrec.Width - spc * 2;
                    switch (imgalgment)
                    {
                        case MainAlignments.left:
                            imgrec.X = spc;
                            strngrec.X = imgrec.Right + spc;
                            break;
                        case MainAlignments.right:
                            imgrec.X = keyrec.X - imgrec.Width - spc;
                            strngrec.X = spc;
                            break;
                    }

                    liner.Width -= keyrec.Width + spc;
                    break;
                case MainAlignments.left:

                    keyrec.X = spc;
                    strngrec.Width = Width - (imgrec.Width + keyrec.Width + spc * 2);

                    switch (imgalgment)
                    {
                        case MainAlignments.left:

                            imgrec.X = keyrec.Right + spc * 2;

                            strngrec.X = keyrec.Right + imgrec.Width + spc * 2;

                            break;
                        case MainAlignments.right:
                            imgrec.X = Width - imgrec.Width - spc;
                            strngrec.X = keyrec.Right + spc;
                            break;
                    }

                    break;

            }
            #endregion

            Point[] openkeypnts = new Point[3] { new Point(keyrec.X + keyrec.Width, keyrec.Y), new Point(keyrec.X, keyrec.Y + keyrec.Height), new Point(keyrec.X + keyrec.Width, keyrec.Y + keyrec.Height) };
            Point[] colsedopenkeys = new Point[3] { new Point(keyrec.X, keyrec.Y), new Point(keyrec.X, keyrec.Y + keyrec.Height), new Point(keyrec.X + keyrec.Width, keyrec.Y + (keyrec.Height / 2)) };


            if (textsz.Width > strngrec.Width)
            { textsz.Width = strngrec.Width; }
            liner.X = strngrec.X + textsz.Width;
            liner.Width += strngrec.Width - textsz.Width;

            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap); sf.Trimming = StringTrimming.EllipsisCharacter; sf.Alignment = TitleAlignment;
            #endregion

            #region drawing


            if (BackColor == Color.Transparent)
            { g.Clear(Color.Transparent); }
            g.FillRectangle(new SolidBrush(titlebk), new Rectangle(0, 0, this.Width, strngrec.Height + spc + 1));
            if (Titleline)
            {

                g.DrawLine(pn, liner.X, liner.Y, liner.X + liner.Width, liner.Y);
            }
            if (UnderTitleLine)
                g.DrawLine(pn, 0, strngrec.Height + spc + 1, Width, strngrec.Height + spc + 1);

            if (ShowOpenKey)
            {
                if (IsOpened)
                {
                    if (mos == moss.openover)
                    { g.FillPolygon(new SolidBrush(keycol), openkeypnts); }
                    else
                    { g.DrawPolygon(new Pen(keycol, 1), openkeypnts); }

                }
                else
                {
                    g.FillPolygon(new SolidBrush(keycol), colsedopenkeys);
                }
            }


            g.DrawString(this.Title, this.TitleFont, new SolidBrush(this.TitleForeColor), strngrec, sf);

            if (Titleimage != null && imgrec.Width > 0 && imgrec.Height > 0)
            {
                g.FillRectangle(new SolidBrush(titlebk), imgrec);
                g.DrawImage(Titleimage, fnc.OImg(Titleimage, imgrec, ImageLayout.Zoom));
            }
            if ((BorderFilter.Left & Borderfilter) == BorderFilter.Left)
            {
                g.DrawLine(pn, 0 + pn.Width / 2, 0, 0 + pn.Width / 2, Height);
            }
            if ((BorderFilter.Right & Borderfilter) == BorderFilter.Right)
            {
                g.DrawLine(pn, Width - pn.Width / 2, 0, Width - pn.Width / 2, Height);

            }
            if ((BorderFilter.Bottom & Borderfilter) == BorderFilter.Bottom)
            {
                g.DrawLine(pn, 0, Height - pn.Width / 2, Width, Height - pn.Width / 2);

            }
            if ((BorderFilter.Top & Borderfilter) == BorderFilter.Top)
            {
                g.DrawLine(pn, 0, 0 + pn.Width / 2, Width, 0 + pn.Width / 2);

            }
            #endregion


            e.Graphics.DrawImage(b, 0, 0);

        }
    }


    #endregion

    public class kPictureBox : System.Windows.Forms.PictureBox
    {
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        [Browsable(true)]
     
        public override Font Font
{
	  get 
	{ 
		 return base.Font;
	}
	  set 
	{ 
		base.Font = value;
	}
}
        [Browsable(true)]
     
        public override Color ForeColor
{
	  get 
	{ 
		 return base.ForeColor;
	}
	  set 
	{ 
		base.ForeColor = value;
	}
}
 public kPictureBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer|ControlStyles.Selectable , true);
           
        }
 private bool showtext = true;

 public bool ShowText
 {
     get { return showtext; }
     set { showtext = value; Invalidate(); }
 }
 private bool showtextonimage = false;

 public bool Showtextonimage
 {
     get { return showtextonimage; }
     set { showtextonimage = value; }
 }private bool selectable = true;

 public bool Selectable
 {
     get { return selectable; }
     set { selectable = value; }
 }
 private RectangleF selectionrec = new RectangleF(0,0,100,100);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
 public RectangleF Selectionrec
 {
     get { return selectionrec; }
     set { selectionrec = value; Invalidate(); }
 }
        bool isdrag = false;
        Point donp = new Point();
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            donp = e.Location;
            isdrag = true;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        if (isdrag)
        {
            Point crntp = e.Location;
            if (crntp.X < 0) crntp.X = 0;
            if (crntp.Y < 0) crntp.Y = 0;
            if (crntp.X > this.Width) crntp.X = this.Width;
            if (crntp.Y > this.Height) crntp.Y = this.Height;
            RectangleF recdraw = new RectangleF();
            if (crntp.X > donp.X)
            {
                recdraw.X = donp.X; recdraw.Width = crntp.X - donp.X;
            }
            else
            {
                recdraw.X = crntp.X; recdraw.Width = donp.X - crntp.X;
            }

            if (crntp.Y > donp.Y)
            {
                recdraw.Y = donp.Y; recdraw.Height = crntp.Y - donp.Y;
            }
            else
            {
                recdraw.Y = crntp.Y; recdraw.Height = donp.Y - crntp.Y;
            }

            selectionrec.X = (float)recdraw.X /(float) this.Width * 100f;
            selectionrec.Y = (float)recdraw.Y / (float)this.Height * 100f;
            selectionrec.Width = ((float)recdraw.Width / (float)this.Width * 100f);
            selectionrec.Height = ((float)recdraw.Height / (float)this.Height * 100f) ;
         
          Invalidate();
        
        }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isdrag = false;
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            Bitmap b=new Bitmap(this.Width,this.Height);
            Graphics g = Graphics.FromImage(b); g.TextRenderingHint = TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat(); sf.Alignment = StringAlignment.Center; sf.LineAlignment = StringAlignment.Center;
            RectangleF drawselcec = new RectangleF();
        
         drawselcec.X = selectionrec.X / 100f * this.Width;
            drawselcec.Y = selectionrec.Y / 100f * this.Height;
            drawselcec.Width = selectionrec.Width / 100f * this.Width;
            drawselcec.Height = selectionrec.Height / 100f * this.Height;
           
            g.SetClip(this.DisplayRectangle);
            g.ExcludeClip(new System.Drawing.Region(drawselcec));
            g.FillRectangle(new SolidBrush(Color.FromArgb(110,Color.Black)),this.DisplayRectangle);
            g.ResetClip();
           
            if (ShowText)
            {
                if (BackgroundImage == null)
                {
                    g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.DisplayRectangle, sf);
                }
                else if (showtextonimage)
                {
                    g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.DisplayRectangle, sf);
                }
            }

            pe.Graphics.DrawImage(b, 0, 0);
        }
    }
    #region Kcombobox
    public class kcombobox : System.Windows.Forms.ComboBox
    {
        public enum Specials { Colors, Fonts, EngAlphapet, None }
        private Specials specialfor = Specials.None;

        public Specials Specialfor
        {
            get { return specialfor; }
            set
            {
                specialfor = value;
                switch (value)
                {
                    case Specials.Colors:
                        this.Items.Clear();
                        foreach (KnownColor kn in Enum.GetValues(typeof(KnownColor)))
                        {

                            this.Items.Add(new comboitem(" ", Color.FromKnownColor(kn)));
                        }
                        this.kind = kinds.flat;
                        this.IntegralHeight = false;
                        break;
                    case Specials.Fonts:
                        this.Items.Clear();
                        int h = 0;
                        foreach (FontFamily family in new System.Drawing.Text.InstalledFontCollection().Families)
                        {
                            System.Drawing.Font f = new Font(family.Name, 13);

                            this.Items.Add(new comboitem(family.Name, f));
                            if (f.Height > h)
                            { h = f.Height; }

                        }
                        this.IntegralHeight = false;
                        //-  this.ItemHeight = h;
                        break;
                    case Specials.EngAlphapet:
                        this.Items.Clear();
                        for (int i = 'a'; i <= 'z'; i++)
                        {
                            this.Items.Add(new comboitem(((char)i).ToString()));
                        }
                        break;
                    case Specials.None:
                        this.Items.Clear();
                        break;
                }
            }
        }

        public enum imageviews { strech, side }
        imageviews timageview = imageviews.side;
        public imageviews imageview
        {
            get { return timageview; }
            set { timageview = value; Invalidate(); }
        }

        public enum kinds { gradient, flat }
        kinds tkind = kinds.gradient;
        public kinds kind { get { return tkind; } set { tkind = value; Invalidate(); } }

        public kcombobox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 20;
            this.IntegralHeight = true;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }


        public static PointF[] Triangle(PointF Location, SizeF Size)
        {
            PointF[] ReturnPoints = new PointF[4];
            ReturnPoints[0] = Location;
            ReturnPoints[1] = new PointF(Location.X + Size.Width, Location.Y);
            ReturnPoints[2] = new PointF(Location.X + Size.Width / 2, Location.Y + Size.Height);
            ReturnPoints[3] = Location;

            return ReturnPoints;
        }
        bool isrmove = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int Xx = MousePosition.X - (this.PointToScreen(Point.Empty).X + this.Width - 25);
            if (Xx > 0)
            {
                if (isrmove != true)
                { Invalidate(); }
                isrmove = true;

            }
            else
            {
                if (isrmove != false)
                { Invalidate(); }
                isrmove = false;
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isrmove = false;
        }
        Color bordercolor = Color.Black;
        [Category("Appearance")]
        public Color BorderColor
        {
            get { return bordercolor; }
            set { bordercolor = value; Invalidate(); }
        }

        Color keycolor = Color.White;
        [Category("Appearance")]
        public Color KeyColor
        {
            get { return keycolor; }
            set { keycolor = value; Invalidate(); }
        }
        Color keybackcolor = Color.FromArgb(30, 30, 30);
        [Category("Appearance")]
        public Color KeyBackColor
        {
            get { return keybackcolor; }
            set { keybackcolor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!(DropDownStyle == ComboBoxStyle.DropDownList))
                DropDownStyle = ComboBoxStyle.DropDownList;
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);



            Color bc = (this.BackColor); Color fc = this.ForeColor; Font fnt = new Font(Font.FontFamily, Font.Size); Image itsimage = null;

            Color brdrc = BorderColor;
            Color keybc = KeyBackColor;
            if (isrmove)
            {
                brdrc = ControlPaint.Light(brdrc, 0.4f);
                keybc = ControlPaint.Light(keybc, .4f);
            }

            string stng = "";
            if (SelectedIndex != -1)
            {
                this.SelectedItem = this.Items[this.SelectedIndex];
                stng = this.Items[SelectedIndex].ToString();
                if (this.Items[SelectedIndex].GetType() == typeof(comboitem))
                {
                    comboitem ci = (comboitem)this.Items[SelectedIndex];
                    stng = ci.Text;
                    if (ci.Backcolor != Color.Empty) { bc = ci.Backcolor; }
                    if (ci.Forecolor != Color.Empty) { fc = ci.Forecolor; }
                    if (ci.Font != null) { fnt = ci.Font; }
                    if (ci.Image != null) { itsimage = ci.Image; }
                }
            }





            switch (kind)
            {
                case kinds.gradient:
                    LinearGradientBrush GradientBrush = new LinearGradientBrush(new Rectangle(0, 0, Width - 25, Height / 5 * 2), ControlPaint.Light(bc, .2f), bc, 90f);
                    G.FillRectangle(GradientBrush, new Rectangle(0, 0, Width - 25, Height / 5 * 2));
                    G.FillRectangle(new SolidBrush(bc), 0, 0, Width - 25, Height);
                    break;
                case kinds.flat:
                    SolidBrush sb = new SolidBrush(bc);
                    G.FillRectangle(sb, e.ClipRectangle);
                    break;
            }



            int S1 = (int)G.MeasureString(stng, fnt).Height;
            if (SelectedIndex != -1)
            {
                if (itsimage != null)
                {
                    switch (imageview)
                    {
                        case imageviews.side:
                            G.DrawString(stng, fnt, new SolidBrush(fc), new Rectangle(ItemHeight + 2, Height / 2 - S1 / 2, this.Width - 25 - ItemHeight - 2, this.ItemHeight - 2), new StringFormat(StringFormatFlags.NoWrap));

                            G.DrawImage(itsimage, 2, 2, ItemHeight, this.Height - 4);
                            break;
                        case imageviews.strech:
                            G.DrawImage(itsimage, 2, 2, this.Width - 26, this.Height - 5);
                            break;
                    }

                }
                else
                {
                    G.DrawString(stng, fnt, new SolidBrush(fc), new Rectangle(2, Height / 2 - S1 / 2, this.Width - 25 - 2, this.ItemHeight - 2), new StringFormat(StringFormatFlags.NoWrap));

                }

            }
            else
            {
                if ((Items != null) & Items.Count > 0)
                {

                    G.DrawString(stng, fnt, new SolidBrush(fc), 4, Height / 2 - S1 / 2);
                }
            }






            //Fill Key BackColor
            G.FillRectangle(new SolidBrush(keybc), Width - 25, 1, 25, Height - 3);
            //Fill Key
            G.FillPolygon(new SolidBrush(KeyColor), Triangle(new Point(Width - 15, Height / 2), new Size(5, 3)));
            //Draw Borders
            G.DrawRectangle(new Pen(brdrc, 2), new Rectangle(1, 1, Width - 2, Height - 2));
            G.DrawRectangle(new Pen(brdrc, 2), new Rectangle(Width - 25, 1, 24, Height - 2));

            e.Graphics.DrawImage(B, 0, 0);
            G.Dispose();
            B.Dispose();
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //     e.DrawFocusRectangle(); //Draw Its rectangle
            e.DrawBackground();
            if (e.Index < 0) return;

            Color bc = (this.BackColor); Color fc = this.ForeColor;
            Font fnt = new Font(this.Font.FontFamily, Font.Size); string stng = this.Items[e.Index].ToString();
            Image itsimage = null;
            if (this.Items[e.Index].GetType() == typeof(comboitem))
            {
                comboitem ci = (comboitem)this.Items[e.Index];
                stng = ci.Text;
                if (ci.Backcolor != Color.Empty) { bc = ci.Backcolor; }
                if (ci.Forecolor != Color.Empty) { fc = ci.Forecolor; }
                if (ci.Font != null) { fnt = ci.Font; }
                if (ci.Image != null) { itsimage = ci.Image; }
            }
            switch (kind)
            {
                case kinds.gradient:
                    //if ismouseover
                    if ((e.State == (DrawItemState)785) || (e.State == (DrawItemState)17))
                    {
                        e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
                        Rectangle x2 = new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, e.Bounds.Height));
                        Rectangle x3 = new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, (e.Bounds.Height / 2)));

                        SolidBrush sb = new SolidBrush(Color.FromArgb(255, bc));

                        e.Graphics.FillRectangle(sb, e.Bounds);
                        Rectangle rec = e.Bounds;
                        rec.X = e.Bounds.Width - 8;
                        rec.Width = 8;
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, bc)), rec);


                    }
                    //no mouse over
                    else
                    {
                        e.Graphics.FillRectangle(new LinearGradientBrush(new Rectangle(e.Bounds.Location, new Size(e.Bounds.Size.Width, e.Bounds.Size.Height)), Color.FromArgb(Math.Abs(bc.R - 20), Math.Abs(bc.G - 20), Math.Abs(bc.B - 50)), BackColor, 90), e.Bounds);
                    }
                    break;
                case kinds.flat:
                    //if ismouseover
                    if ((e.State == (DrawItemState)785) || (e.State == (DrawItemState)17))
                    {
                        //    e.Graphics.FillRectangle(Brushes.White, e.Bounds);

                        //  LinearGradientBrush G1 = new LinearGradientBrush(new Point(x2.X, x2.Y), new Point(x2.X, x2.Y + x2.Height), Color.FromArgb(200, bc), Color.FromArgb(100, bc));
                        SolidBrush sb = new SolidBrush(Color.FromArgb(255, bc));

                        e.Graphics.FillRectangle(sb, e.Bounds);
                        Rectangle rec = e.Bounds;
                        rec.X = e.Bounds.Width - 8;
                        rec.Width = 8;
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, bc)), rec);

                    }
                    //no mouse over
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(Math.Abs(bc.R - 20), Math.Abs(bc.G - 20), Math.Abs(bc.B - 50))), e.Bounds);
                    }

                    break;
            }


            //draw srin & image in any case
            if (itsimage != null)
            {
                switch (imageview)
                {
                    case imageviews.side:
                        int S1 = (int)e.Graphics.MeasureString(Items[e.Index].ToString(), Font).Height;

                        e.Graphics.DrawString(stng, Font, new SolidBrush(fc), new Rectangle(ItemHeight + 1, e.Bounds.Y + Height / 2 - S1 / 2, this.Width - 25 - ItemHeight - 2, this.ItemHeight - 2), new StringFormat(StringFormatFlags.NoWrap));
                        //   e.Graphics.DrawString(stng, Font, new SolidBrush(fc), new Rectangle(ItemHeight + 2, e.Bounds.Y + Height / 2 - S1 / 2, this.Width - 25 - ItemHeight - 2, this.ItemHeight - 2));

                        e.Graphics.DrawImage(itsimage, e.Bounds.X + 1, e.Bounds.Y + 1, ItemHeight - 2, ItemHeight - 2);
                        break;
                    case imageviews.strech:
                        e.Graphics.DrawImage(itsimage, e.Bounds.X + 1, e.Bounds.Y + 1, this.Width - 30, ItemHeight - 2);
                        break;
                }
            }
            else
            {

                int S1 = (int)(e.Graphics.MeasureString(stng, fnt).Height);
                e.Graphics.DrawString(stng, fnt, new SolidBrush(fc), new Rectangle(2, e.Bounds.Y + Height / 2 - S1 / 2, this.Width - 25 - 2, this.ItemHeight - 2), new StringFormat(StringFormatFlags.NoWrap));

            }

            base.OnDrawItem(e);

        }
        protected override void OnDropDownClosed(EventArgs e)
        {
            Invalidate();
            base.OnDropDownClosed(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e); Invalidate();
        }
        public override string ToString()
        {
            if (this.Items[SelectedIndex].GetType() == typeof(comboitem))
            { return ((comboitem)SelectedItem).Text; }
            return base.ToString();
        }

    }
    public class comboitem : Object
    {
        Color tforecolor = Color.Empty;
        public Color Forecolor { get { return tforecolor; } set { tforecolor = value; } }
        public object Tag;
        Font tfont = null;
        public Font Font { get { return tfont; } set { tfont = value; } }

        System.Drawing.Image timage = null;
        public System.Drawing.Image Image { get { return timage; } set { timage = value; } }

        Color tbackcolor = Color.Empty;
        public Color Backcolor { get { return tbackcolor; } set { tbackcolor = value; } }

        string ttext = string.Empty;
        public string Text { get { return ttext; } set { ttext = value; } }

        public comboitem(string thetext, Font thefont, Color thebackcolor, Color theforecolor)
        {
            this.Text = thetext; this.Font = thefont; this.Backcolor = thebackcolor; this.Forecolor = theforecolor;
        }
        public comboitem(string thetext, Font thefont, Color thebackcolor, Color theforecolor, System.Drawing.Image theimage)
        {
            this.Text = thetext; this.Font = thefont; this.Backcolor = thebackcolor; this.Forecolor = theforecolor; this.Image = theimage;
        }
        public comboitem(string thetext, System.Drawing.Image theimage)
        {
            this.Text = thetext; this.Image = theimage;
        }

        public comboitem(string thetext, Font thefont)
        {
            this.Text = thetext; this.Font = thefont;
        }
        public comboitem(string thetext, Color thebackcolor, Color theforecolor)
        {
            this.Text = thetext; this.Backcolor = thebackcolor; this.Forecolor = theforecolor;
        }
        public comboitem(string thetext, Color thebackcolor)
        {
            this.Text = thetext; this.Backcolor = thebackcolor;
        }
        public comboitem(string thetext)
        {
            this.Text = thetext;
        }
        public comboitem(string thetext,object tag)
        {
            this.Tag = tag;
            this.Text = thetext;
        }
        public comboitem(string thetext, object tag,Image img)
        {
            this.Image = img;
            this.Tag = tag;
            this.Text = thetext;
        }
        public comboitem(string thetext, Font thefont, Color theforecolor)
        {
            this.Text = thetext; this.Font = thefont; this.Forecolor = theforecolor;
        }
        public comboitem()
        {

        }
        public override string ToString()
        {
            if (this.Text == string.Empty)
            { return base.ToString(); }
            else { return this.Text; }
        }
    }
    #endregion
    #region eventes
    public delegate void inteventhandler(object sender, Inteventaregs e);
    public class Inteventaregs : System.EventArgs
    {
        public Inteventaregs(int value) { this.Value = value; }
        public Inteventaregs() { }
        public int Value { get; set; }
    }
    public delegate void booleventhandler(object sender, booleventargs e);
    public class booleventargs : System.EventArgs
    {
        public booleventargs() { }
        public booleventargs(bool Value)
        { this.Value = Value; }
        public bool Value { get; set; }
    }
    #endregion
    #region Kswitch
    [DefaultEvent("ValueChanged")]

    public class kswitch : System.Windows.Forms.Control, ISupportInitialize
    {
        /// <summary>
        /// Occurs when value set to true or false ,note that it Occurs only when user finally choose excatly when he relase mouse right button
        /// </summary>
        public event booleventhandler ValueChanged;

        void ValueChanged_1(object sender, booleventargs e) { }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return null;
            }

        }
        public kswitch() : this(false, Color.Cyan) { }
        public kswitch(bool value) : this(value, Color.Cyan) { }
        public kswitch(bool value, Color selectedcolor)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            this.value = value;
            this.selectedcolor = selectedcolor;
            this.Width = 120;

        }

        private bool showvalue = true;
        public bool Showvalue
        {
            get { return showvalue; }
            set { showvalue = value; Invalidate(); }
        }

        private bool showtext = true;

        public bool Showtext
        {
            get { return showtext; }
            set { showtext = value; Invalidate(); }
        }

        private Color movercolor = Color.Black;


        public Color Movercolor
        {
            get { return movercolor; }
            set { movercolor = value; Invalidate(); }
        }

        private bool value = false;
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (value)
                {
                    xvalue = this.Width - pointerwidth - 5;
                    svalue = on;

                }
                else
                {
                    xvalue = (int)xytext.Width + 5;
                    svalue = off;
                }
                if (ValueChanged != null)
                {
                    ValueChanged(this, new booleventargs(this.Value));
                }
                Invalidate();
            }
        }
        private int pointerwidth = 13;

        public int Pointerwidth
        {
            get { return pointerwidth; }
            set { pointerwidth = value; Invalidate(); }
        }
        private Color selectedcolor = Color.Cyan;
        public Color Selectedcolor
        {
            get { return selectedcolor; }
            set { selectedcolor = value; Invalidate(); }
        }
        public enum ValueText { OnOff, YesNo, TrueFalse, NiceBad, HardEasy };
      //  private ValueText valuetext = ValueText.OnOff;
     
        public string OnText
        {
            get { return on; }
            set { on = value; Invalidate(); }
        }
    
        public string OffText
        {
            get { return off; }
            set { off = value; Invalidate(); }
        }
       


        bool isdown = false; bool ismove = false;
        private int xvalue = -2;
        private int downx = 0;
        bool mos = false;
        string svalue = "Off";
        string on = "On";
        string off = "Off";
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e); mos = true; Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e); mos = false; Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            downx = MousePosition.X - (this.PointToScreen(Point.Empty).X) - xvalue;
            isdown = true;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isdown)
            {
                ismove = true;

                xvalue = MousePosition.X - this.PointToScreen(Point.Empty).X - downx;
                if (xvalue > this.Width - pntrec.Width - 5)
                {
                    xvalue = this.Width - (int)pntrec.Width - 5;
                    svalue = on;
                }
                else if (xvalue - xytext.Width - 5 < 0)
                {
                    xvalue = (int)xytext.Width + 5;
                    svalue = off;
                }
                else
                {
                    if (xvalue >= fnc.divdectint(this.Width + xytext.Width, 2))
                    {
                        svalue = on;
                    }
                    else
                    {
                        svalue = off;
                    }
                }

            }

            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);

            if (isdown)
            {
                if (ismove)
                {

                    if (xvalue >= fnc.divdectint(this.Width + xytext.Width, 2))
                    {
                        xvalue = this.Width - (int)pntrec.Width - 5;
                        Value = true;

                    }
                    else
                    {
                        xvalue = (int)xytext.Width + 5;
                        Value = false;
                    }
                }

                else
                {

                    if (MousePosition.X - this.PointToScreen(Point.Empty).X >= fnc.divdectint(this.Width + xytext.Width, 2))
                    {
                        xvalue = this.Width - (int)pntrec.Width - 5;
                        Value = true;
                    }
                    else
                    {
                        xvalue = (int)xytext.Width + 5;
                        Value = false;
                    }
                }
            }
            isdown = false;
            ismove = false;
        }

        SizeF xytext = new SizeF(0, 0);
        Rectangle pntrec = new Rectangle();
        public void EndInit()
        {

        }
        public void BeginInit()
        { }


        public enum SwitchStyle { Win8, Round }
        private SwitchStyle switchstyle = SwitchStyle.Round;

        public SwitchStyle Switchstyle
        {
            get { return switchstyle; }
            set { switchstyle = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap b = new Bitmap(this.Width, this.Height); Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = TextRenderingHint.AntiAlias; g.CompositingQuality = CompositingQuality.HighQuality;
            //draw text
            int xtext = 0;
            if (showtext)
            {
                xytext = g.MeasureString(this.Text, this.Font);
                xtext = (int)xytext.Width;

                g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), 0, fnc.divdec(this.Height, 2) - fnc.divdec(xytext.Height, 2));

            }
            else
            {
                xytext = new SizeF(0, 0);
                xtext = 0;
            }

            // making colors
            Pen p = new Pen(this.BackColor, 5); Color mselectedcolor = selectedcolor;
            if (mos)
            {
                p.Color = ControlPaint.Light(p.Color, .04f);
                mselectedcolor = ControlPaint.Light(selectedcolor, 0.4f);
            }
            SolidBrush moverb = new SolidBrush(movercolor);
            // rectangels
            Rectangle area = new Rectangle(xtext, 0, Width - xtext - 0, Height - 0);
            pntrec = new Rectangle(xvalue, 5, pointerwidth, this.Height - 10);
            if (!isdown && !value)
            {
                pntrec.X = (int)xytext.Width + 5;
            }
            Rectangle selrec = new Rectangle(area.X + 5, 5, xvalue - (int)xytext.Width - 5, Height - 10);

            if (switchstyle == SwitchStyle.Round)
            {



                g.FillRoundedRectangle(new SolidBrush(ControlPaint.Dark(p.Color, .5f)), area, .5f);
                g.FillRoundedRectangle(new SolidBrush(p.Color), new Rectangle(area.X + 2, 2, area.Width - 4, area.Height - 4), 0.5f);

                //selection area              

                selrec.Width += (int)(fnc.divdec(pointerwidth, Width) * xvalue);

                // selrec.Y += 2; selrec.Height -= 4; 
                //  selrec.X += 2; selrec.Width -= 4;
                if (selrec.Width > pointerwidth / 2)
                {
                    g.FillRoundedRectangle(new SolidBrush(mselectedcolor), selrec, .2f);
                }
                //mover
                g.FillEllipse(new SolidBrush(ControlPaint.Dark(moverb.Color, 0.5f)), pntrec);
                g.FillEllipse(moverb, new Rectangle(pntrec.X + 2, pntrec.Y + 2, pntrec.Width - 4, pntrec.Height - 4));






            }
            else
            {
                //border
                g.FillRectangle(new SolidBrush(p.Color), area);
                //bordr
                g.FillRectangle(Brushes.White, area.X + 2, 2, area.Width - 4, area.Height - 4);
                //small space
                g.FillRectangle(new SolidBrush(p.Color), 5 + area.X, 5, Width - 10 - xtext, Height - 10);
                //mover
                g.FillRectangle(moverb, pntrec);
                //selection area              
                g.FillRectangle(new SolidBrush(mselectedcolor), selrec);

            }

            // darw value string
            using (Font f = new Font("Brandish", 10))
            {
                PointF spnt = new PointF();

                spnt.Y = fnc.divdec(this.Height, 2) - fnc.divdec(g.MeasureString((svalue), f).Height, 2);
                spnt.X = fnc.divdec(this.Width + xtext, 2) - fnc.divdec((g.MeasureString(svalue, f)).Width, 2);

                if (value && ismove == false)
                {
                    spnt.X = xytext.Width + fnc.divdec(selrec.Width, 2) - fnc.divdec((g.MeasureString(svalue, f)).Width, 2);
                }
                else if (!value && ismove == false)
                {
                    spnt.X = xytext.Width + pointerwidth + 5 + fnc.divdec(this.Width - pointerwidth - 10 - xytext.Width, 2) - fnc.divdec((g.MeasureString(svalue, f)).Width, 2);


                }
                if (showvalue) g.DrawString(svalue, f, new SolidBrush(this.ForeColor), spnt);
            }
            if (!Enabled)
            {

                for (int r = 0; r < b.Width; r++)
                {
                    for (int c = 0; c < b.Height; c++)
                    {
                        int m = (b.GetPixel(r, c).R + b.GetPixel(r, c).G + b.GetPixel(r, c).B) / 3;
                        b.SetPixel(r, c, Color.FromArgb(m, m, m));
                    }
                }
            }
            e.Graphics.DrawImage(b, 0, 0, this.Width, this.Height);
            e.Dispose();
            g.Dispose();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }
    }
    #endregion

    #region Kscale

    public class kscaledesgineractionlist : System.ComponentModel.Design.DesignerActionList
    {
        private kscale scale;
        private kscaledesginer d;
        public bool Allowaddpointers { get { return scale.AllowAddPointers; } set { scale.AllowAddPointers = value; } }
        public void AddPointer()
        {
            d.OnAddButton(null, null);
            scale.makepointerlocation(); scale.Invalidate();
        }
        public void RemovePointer()
        {
            if (scale.Pointers.Count > 1)
            {
                d.OnComponentRemoving(null, new ComponentEventArgs(this.scale.Pointers[this.scale.Pointers.Count - 1]));
                scale.makepointerlocation();
                scale.Invalidate();
            }
            else
            { MessageBox.Show("Can't remove the only pointer"); }

        }
        public void ChangeOrintation()
        {
            if (scale.OrientationDirection == Orientation.Horizontal)
            { scale.OrientationDirection = Orientation.Vertical; }
            else { scale.OrientationDirection = Orientation.Horizontal; }
        }
        public KscaleThemes theme { get { return scale.Theme; } set { ((kscale)this.Component).Theme = value; } }
        public KscalePaintMode style { get { return scale.Style; } set { scale.Style = value; } }
        public kscaledesgineractionlist(kscale v, kscaledesginer dd)
            : base(v)
        {
            scale = v; this.d = dd;
        }
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            try
            {
                items.Add(new DesignerActionHeaderItem("Commands"));
                items.Add(new DesignerActionHeaderItem("Appearance"));
                items.Add(new DesignerActionMethodItem(this, "AddPointer", "Add Pointer", "Commands"));
                items.Add(new DesignerActionMethodItem(this, "RemovePointer", "RemovePointer", "Commands"));
                items.Add(new DesignerActionMethodItem(this, "ChangeOrintation", "Change Orintation", "Commands"));
                items.Add(new DesignerActionPropertyItem("theme", "theme", "Appearance"));
                items.Add(new DesignerActionPropertyItem("style", "style", "Appearance"));
                items.Add(new DesignerActionPropertyItem("Allowaddpointers", "Allowaddpointers", "Commands"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while generating the action list panel for this KRBTabControl, " + ex.Message);
            }
            return items;
        }

    }
    public class kscaledesginer : System.Windows.Forms.Design.ControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Record instance of control we're designing
            // Hook up events
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            s.SelectionChanged += new EventHandler(OnSelectionChanged);

            c.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
        }
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            if (scale.Pointers.Count == 0)
            {
                this.OnAddButton(null, null);
                scale.SelectedIndex = 0;

            }

            scale.Width = 150;
            scale.Rectangleedegfilter.TopRight = scale.Rectangleedegfilter.BottomLeft = true;
            this.scale.Theme = KscaleThemes.Sky;
            this.scale.Theme = KscaleThemes.None;
        }
        private void OnSelectionChanged(object sender, System.EventArgs e)
        {
            scale.OnSelectionChanged();
        }
        public void OnComponentRemoving(object sender, ComponentEventArgs e)
        {

            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));

            // If the user is removing a pointer
            if (e.Component is pointer)
            {

                pointer p = (pointer)e.Component;
                if (this.scale.Pointers.Contains(p))
                {
                    c.OnComponentChanging(scale, null);
                    if (scale.SelectedIndex == p.Index)
                    {
                        scale.SelectedIndex = 0;

                    }
                    scale.Pointers.Remove(p);
                    c.OnComponentChanged(scale, null, null, null);
                    return;
                }
                else
                {// MessageBox.Show("this.scale ="+this.scale.ToString()+"\n pointer.scale.poinerts not contaian this pointer index is"+p.Index+" sender +"+sender.ToString()+" its owner  \n pointercollction "+((pointer)e.Component).Pointercollection.ToString()); }
                }
            }


            // If the user is removing the control itself
            if (e.Component == scale)
            {
                scale.SelectedIndex = 0;
                for (int i = scale.Pointers.Count - 1; i >= 0; i--)
                {
                    pointer p = scale.Pointers[i];
                    c.OnComponentChanging(scale, null);
                    scale.Pointers.Remove(p);
                    h.DestroyComponent(p);
                    c.OnComponentChanged(scale, null, null, null);
                }
            }
        }
        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                return scale.Pointers;
            }
        }
        protected override bool GetHitTest(System.Drawing.Point point)
        {
            Rectangle wrct;
            bool v = false;
            point = scale.PointToClient(point);
            foreach (pointer p in scale.Pointers)
            {
                wrct = Rectangle.Round(p.Rec);
                if (wrct.Contains(point))
                {

                    v = true;

                }
            }

            return v;
        }
        public void OnAddButton(object sender, System.EventArgs e)
        {
            pointer p;
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction dt;
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Add a new button to the collection
            dt = h.CreateTransaction("Add Button");
            p = (pointer)h.CreateComponent(typeof(pointer));
            c.OnComponentChanging(scale, null);
            if (scale.Pointers.Count > 0)
            {
                fnc.CopyProperties(p, scale.Pointers[0], true);
                p.ValueF = scale.Minimum;
            }
            scale.Pointers.Add(p);

            c.OnComponentChanged(scale, null, null, null);
            dt.Commit();
        }

        protected override void Dispose(bool disposing)
        {
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Unhook events
            s.SelectionChanged -= new EventHandler(OnSelectionChanged);
            c.ComponentRemoving -= new ComponentEventHandler(OnComponentRemoving);

            base.Dispose(disposing);
        }

        private DesignerVerb _changeoritaion;
        private DesignerVerbCollection _verbs;
        private DesignerActionListCollection _action;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_action == null)
                {
                    _action = new DesignerActionListCollection();
                    _action.Add(new kscaledesgineractionlist((kscale)Control, this));
                }
                return _action;
            }
        }
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection();

                    _changeoritaion = new DesignerVerb("Change Oritation", this.Changeoritation);

                    _verbs.Add(_changeoritaion);

                }
                return _verbs;
            }
        }
        private kscale scale
        {
            get
            {
                return (kscale)this.Control;
            }
        }
        public kscaledesginer() { }

        public void Changeoritation(object sender, EventArgs e)
        {
            if (scale.OrientationDirection == Orientation.Vertical)
            {
                scale.OrientationDirection = Orientation.Horizontal;
            }
            else
            { scale.OrientationDirection = Orientation.Vertical; }
        }
    }

    public delegate void Kscaleeventhandler(object sender, Kscaleeventargs e);
    public delegate void Kscalepointereventhandler(object sender, kscalepointereventargs e);

    public class kscalepointereventargs : EventArgs
    {
        public pointer Pointer { get; set; }
        public kscalepointereventargs(pointer item)
        { this.Pointer = item; }
        public kscalepointereventargs() : this(null) { }
    }
    public class Kscaleeventargs : Inteventaregs
    {
        public Kscaleeventargs() { }
        public Kscaleeventargs(int Value)
        { this.Value = Value; }
        public Kscaleeventargs(int value, Color selectedcolor)
        {
            this.Value = value;
            this.SelectedColor = selectedcolor;
        }

        public Color SelectedColor { get; set; }
    }
    #region enums
    public enum KscaleThemes { Sky, DarkSky, Dark, Pink, green, None }
    public enum KscalePaintMode { gradient, flaty, Flat, Maxed }
    public enum KscaleMoverShape { Circle, Rect, Trangle, Polygon }
    public enum KscaleAddPointerMode { when_no_pointer_near_minmum, Anyway }
    /// <summary>
    /// Determains the selcties area between pointers to be filled 
    /// </summary>  
    public enum KscaleFilledArea
    {
        /// <summary>
        /// normale filling
        /// </summary>
        Normal,
        /// <summary>
        /// Fill space between two pointers
        /// </summary>
        inner,
        /// <summary>
        /// Fill all spcae expect space between active pointer and next pointer
        /// </summary>
        outer
    }
    /// <summary>
    /// Determains the filled area appearance
    /// </summary>
    public enum FilledAreaPainting
    {
        /// <summary>
        /// Pattren layer with type of 'FilledAreaPattrenType' property 
        /// </summary>
        Pattrenlayer,
        /// <summary>
        /// Solid color with transparent value of 'FilledAreaTransparency' property 
        /// </summary>
        FilledAreaColorproperty,
    }
    #endregion
    public class PointerCollection : CollectionBase
    {
        public kscale scale;

        internal PointerCollection(kscale Control)
        {
            this.scale = Control;
        }

        public pointer this[int Index]
        {
            get
            {
                return (pointer)List[Index];
            }
        }

        public bool Contains(pointer Button)
        {
            return List.Contains(Button);
        }

        public int Add(pointer item)
        {

            int i;

            i = List.Add(item);
            item.Pointercollection = this;
            if (scale != null)
            {

                scale.makepointerlocation(item);
            }
            if (scale != null)
            {
                if (this.scale.setuped)
                {

                    this.scale.OnPointerAdded(new kscalepointereventargs(item));
                }
            }
            return i;
        }

        public void Remove(pointer p)
        {
            if (this.Count > 1)
            {
                if (this.scale.SelectedIndex == p.Index)
                { this.scale.SelectedIndex = 0; }
                List.Remove(p);
                p.Pointercollection = null;

            }
            else { }
        }
        public int IndexOf(pointer p)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] == p) { return i; }
            }
            return -1;
        }
    }
    [ToolboxItem(false), DesignTimeVisible(false)]
    public class pointer : System.ComponentModel.Component
    {
        public pointer() : this(null) { }

        public pointer(PointerCollection parent)
        {
            this.Pointercollection = parent;
        }

        PointerCollection itscollection;
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointerCollection Pointercollection
        {
            get { return itscollection; }
            set
            {
                itscollection = value;
                if (value != null)
                {
                    if (value.scale != null)
                    { scale = value.scale; }
                }
            }
        }

        //private int index = -1;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                if (this.Pointercollection == null) return -1;

                if (this.Pointercollection.Contains(this))
                {
                    return this.Pointercollection.IndexOf(this);
                }
                else
                { return -2; }
            }
        }

        private RectangleF rec = new RectangleF();
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RectangleF Rec
        {
            get
            {

                return rec;
            }
            set { rec = value; loc = value.Location; }
        }

        private PointF loc = PointF.Empty;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF Loc
        {
            get { return loc; }
            set { loc = value; rec.Location = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float X
        { get { return loc.X; } set { loc.X = value; rec.Location = loc; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Y
        { get { return loc.Y; } set { loc.Y = value; rec.Location = loc; } }

        private float downx = 0;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Downx
        {
            get { return downx; }
            set { downx = value; }
        }

        private float valuef = 0;
        public float ValueF
        {
            get { return valuef; }
            set
            {
                if (Pointercollection != null)
                {
                    if (Scale != null)
                    {
                        if (value < Scale.Minimum || value > Scale.Maxmum)
                        {
                            throw new IndexOutOfRangeException(string.Format("Value of {0} isn't valid ,Value must be between minimum({1}) and maxmum ({2})", value, Scale.Minimum, Scale.Maxmum));
                        }
                        if (Scale.IsOnValueChanging == false)
                        {
                            scale.PreValue = valuef;
                        }
                        valuef = value;

                        if (Scale.IsOnValueChanging == false)
                        {
                            if (scale.Parent is ColorPicker)
                            { if (((ColorPicker)scale.Parent).onmoving) { } }
                            Scale.makepointerlocation(this);
                            Scale.OnValueChanged(new Kscaleeventargs((int)value, Scale.SelectedColor));
                            Scale.Invalidate();
                        }



                    }
                }
            }
        }

        [Browsable(false), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get { return fnc.tint(valuef); }
            set
            {
                ValueF = value;

            }
        }
        private kscale scale = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public kscale Scale
        {
            get
            {
                if (scale == null)
                {
                    if (Pointercollection != null)
                    { return Pointercollection.scale; }
                }
                return scale;
            }
        }

        #region appreance properties
        private Bitmap image = null;
        [DefaultValue(null)]
        public Bitmap Image
        {
            get { return image; }
            set { image = value; Invalidate(); }
        }
        private Color color = Color.Cyan;
        [Category("Appearance")]
        public Color Movercolor
        {
            get { return color; }
            set { color = value; Invalidate(); }
        }

        private void Invalidate()
        { if (this.Scale != null) { this.Scale.Invalidate(); } }

        private bool showborder = true;
        [Category("Appearance")]
        public bool ShowBorder
        {
            get { return showborder; }
            set { showborder = value; Invalidate(); }
        }
        StringAlignment moveralgiment = StringAlignment.Center;
        [Category("Appearance")]
        public StringAlignment MoverAlgiment
        {
            get { return moveralgiment; }
            set { moveralgiment = value; Invalidate(); }
        }
        bool fillmover = true;
        [Category("Appearance")]
        public bool FillMover
        {
            get { return fillmover; }
            set { fillmover = value; Invalidate(); }
        }

        private KscaleMoverShape movershape = KscaleMoverShape.Rect;
        [Category("Appearance"), DefaultValue(KscaleMoverShape.Rect)]
        public KscaleMoverShape MoverShape
        {
            get { return movershape; }
            set { movershape = value; Invalidate(); }
        }
        #endregion
    }


    /// <summary>
    /// Powerful Slider tool with many using fildes
    /// </summary>
    [DefaultEvent("ValueChanged"), ToolboxBitmap(typeof(kscale), "toolboximage"), Description("Powerful Slider tool with many using fildes"), DesignerAttribute(typeof(kscaledesginer))]
    public class kscale : System.Windows.Forms.Control, ISupportInitialize
    {
      
        public static Bitmap toolboximage { get { return new Bitmap(16, 16); } }

        public kscale()
        {
           SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer
            | ControlStyles.Selectable | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
          
            pointers = new PointerCollection(this);
        }
        bool hangvaluechangedevent = false;

        public bool Hangvaluechangedevent
        {
            get { return hangvaluechangedevent; }
            set { hangvaluechangedevent = value; }
        }
        bool hangvaluechangingevent = false;

        public bool Hangvaluechangingevent
        {
            get { return hangvaluechangingevent; }
            set { hangvaluechangingevent = value; }
        }
        #region properties


        bool drawbacktranspesryrec = false;
        public bool DrawBackTranspersyRec
        {
            get { return drawbacktranspesryrec; }
            set { drawbacktranspesryrec = value; Invalidate(); }
        }

        #region FilledArea

        private FilledAreaPainting filledareapainting = FilledAreaPainting.FilledAreaColorproperty;
        [Category("FilledArea"), DefaultValue(FilledAreaPainting.FilledAreaColorproperty)]
        public FilledAreaPainting FilledAreaPainting
        {
            get { return filledareapainting; }
            set { filledareapainting = value; Invalidate(); }
        }

        private HatchStyle pattrentype = HatchStyle.ForwardDiagonal;
        [Category("FilledArea"), DefaultValue(HatchStyle.ForwardDiagonal),
         Description("The filled area painted with this Pattren type with transperncy value of 'FilledAreaTransparency'  property"
          + "\n  Only appear when 'FilledAreaPainting'prperty is set to 'pattrenlayer'")
        ]
        public HatchStyle FilledAreaPattrenType
        {
            get { return pattrentype; }
            set { pattrentype = value; Invalidate(); }
        }
        private Color filledareacolor = Color.Black;
        [Category("FilledArea"), DefaultValue(typeof(Color), "Black")]
        public Color FilledAreaColor
        {
            get { return filledareacolor; }
            set { filledareacolor = value; Invalidate(); }
        }
        private float filledareatransparency = 0.5F;
        [Category("FilledArea"), DefaultValue(0.5F)]
        public float FilledAreaTransparency
        {
            get { return filledareatransparency; }
            set
            {
                filledareatransparency = value;
                if (value < 0 || value > 1) { throw new Exception("Value must bp from 0.0 to 1.0"); }
                Invalidate();
            }
        }
        private KscaleFilledArea fillingmode = KscaleFilledArea.Normal;
        [Category("FilledArea"), DefaultValue(KscaleFilledArea.Normal), Description("Filling area size between pointers")]
        public KscaleFilledArea FillingMode
        {
            get { return fillingmode; }
            set { fillingmode = value; Invalidate(); }
        }
        bool fillselectedarea = true;
        [Category("FilledArea"), DefaultValue(true)]
        public bool Fillselectedarea
        {
            get { return fillselectedarea; }
            set { fillselectedarea = value; Invalidate(); }
        }
        #endregion
        #region Appearance

        float roundamount = 0.6f;
        [Category("Appearance"), DefaultValue(0.6f)]
        public float RoundAmount
        {
            get { return roundamount; }
            set { roundamount = value; imgdone = false; Invalidate(); }
        }

        private RectangeEdgeFilter rectedgefilter = new RectangeEdgeFilter();
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RectangeEdgeFilter Rectangleedegfilter
        {
            get { return rectedgefilter; }
            set { rectedgefilter = value; imgdone = false; Invalidate(); }
        }

        private Color smoothcolor = Color.SteelBlue;
        [Category("Appearance"), Description("the color of the top edge"), DefaultValue(typeof(Color), "SteelBlue")]
        public Color Smoothcolor
        {
            get { return smoothcolor; }
            set { smoothcolor = value; imgdone = false; Invalidate(); }
        }
        private Color maincolor = Color.Cyan;
        [Category("Appearance"), DefaultValue(typeof(Color), "Cyan"), Description("In flaty mode indactes the color of the unselected area ,In gradient indecates the right color")]
        public Color Maincolor
        {
            get { return maincolor; }
            set { maincolor = value; imgdone = false; Invalidate(); }
        }

        private Color bordercolor = Color.Teal;
        [Category("Appearance"), DefaultValue(typeof(Color), "Teal"), Description("indactes the color of the borders")]
        public Color Bordercolor
        {
            get { return bordercolor; }
            set { bordercolor = value; imgdone = false; Invalidate(); }
        }
        private bool showmediumline = false;
        [Category("Appearance"), DefaultValue(false), Description("indactes showing meduim line on the scale")]
        public bool Showmediumline
        {
            get { return showmediumline; }
            set { showmediumline = value; imgdone = false; Invalidate(); }
        }
        private bool showborder = true;
        [Category("Appearance"), DefaultValue(true), Description("indactes showing the borders")]
        public bool Showborder
        {
            get { return showborder; }
            set { showborder = value; imgdone = false; Invalidate(); }
        }


        private int moverwidth = 10;
        [Category("Appearance"), DefaultValue(10), Description("indactes the width of the selector")]
        public int Moverwidth
        {
            get { return moverwidth; }
            set
            {

                moverwidth = value;
                makepointerlocation();
                Invalidate();
            }

        }

        private int moverheight = 16;
        [Category("Appearance"), DefaultValue(16)]
        public int MoverHeight
        {
            get { return moverheight; }
            set { moverheight = value; makepointerlocation(); Invalidate(); }
        }

        private KscalePaintMode style = KscalePaintMode.flaty;
        [Category("Appearance"), DefaultValue(KscalePaintMode.flaty), Description("indactes the style of the kscale")]
        public KscalePaintMode Style
        {
            get { return style; }
            set { style = value; imgdone = false; Invalidate(); }
        }
        private KscaleThemes theme = KscaleThemes.None;
        [Category("Appearance"), DefaultValue(KscaleThemes.None)]
        public KscaleThemes Theme
        {
            get { return theme; }
            set
            {
                imgdone = false;
                theme = value;
                switch (value)
                {
                    case KscaleThemes.Sky:
                        Bordercolor = System.Drawing.Color.Teal;
                        ForeColor = System.Drawing.Color.Black;
                        Maincolor = System.Drawing.Color.Cyan;
                        DefultMovercolor = System.Drawing.Color.Turquoise;
                        Smoothcolor = System.Drawing.Color.SteelBlue;
                        controlbuttoncolor = maincolor;
                        ValueTextBackColor = System.Drawing.Color.White;
                        maxgradientcolors = new Color[] { Color.Red, Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255), Color.Red };
                        MakeMaxGradientPostions();
                        break;
                    case KscaleThemes.DarkSky:

                        this.Bordercolor = System.Drawing.Color.SteelBlue;
                        this.ControlButtonColor = System.Drawing.Color.DodgerBlue;
                        this.Maincolor = System.Drawing.Color.DeepSkyBlue;
                        this.MaxgradientColors = new System.Drawing.Color[] {
                System.Drawing.Color.FromArgb(192, 255, 255),
                System.Drawing.Color.Aqua,
                System.Drawing.Color.Teal,
                System.Drawing.Color.Blue,
                System.Drawing.Color.FromArgb(0,0, 192),
                System.Drawing.Color.Navy,
                System.Drawing.Color.FromArgb(0, 0, 64)};
                        this.MaxGradientPositions = new float[] { 0F, 0.1666667F, 0.3333333F, 0.5F, 0.6666667F, 0.8333334F, 1F };
                        this.Smoothcolor = System.Drawing.Color.DarkSlateBlue;
                        this.ValueTextBackColor = System.Drawing.Color.MediumTurquoise;
                        this.DefultMovercolor = System.Drawing.Color.DeepSkyBlue;

                        break;
                    case KscaleThemes.Dark:

                        ValueTextBackColor = System.Drawing.Color.White;
                        Bordercolor = System.Drawing.Color.FromArgb(64, 64, 64);
                        this.DefultMovercolor = System.Drawing.Color.Gray;
                        this.Smoothcolor = System.Drawing.Color.Gray;
                        this.Maincolor = System.Drawing.Color.FromArgb(64, 64, 64);
                        controlbuttoncolor = maincolor;

                        break;
                    case KscaleThemes.Pink:
                        this.ValueTextBackColor = System.Drawing.Color.LightGray;
                        this.Bordercolor = System.Drawing.SystemColors.ActiveBorder;
                        this.DefultMovercolor = System.Drawing.Color.LightGreen;
                        this.Smoothcolor = System.Drawing.Color.Firebrick;
                        this.Maincolor = System.Drawing.Color.DeepPink;
                        controlbuttoncolor = maincolor;

                        break;
                    case KscaleThemes.green:
                        this.Bordercolor = System.Drawing.Color.CadetBlue;
                        this.ControlButtonColor = System.Drawing.Color.Turquoise;
                        this.Maincolor = System.Drawing.Color.MediumTurquoise;
                        this.DefultMovercolor = System.Drawing.Color.Teal;
                        this.Smoothcolor = System.Drawing.Color.SpringGreen;
                        this.ValueTextBackColor = System.Drawing.Color.White;
                        this.MaxgradientColors = new System.Drawing.Color[] {
        System.Drawing.Color.Yellow,
        System.Drawing.Color.Gold,
        System.Drawing.Color.GreenYellow,
        System.Drawing.Color.Lime,
        System.Drawing.Color.Aquamarine,
        System.Drawing.Color.Cyan,
        System.Drawing.Color.MediumTurquoise};
                        break;
                    case KscaleThemes.None:

                        break;
                }
                foreach (pointer p in pointers)
                { p.Movercolor = DefultMovercolor; }

                Invalidate();
            }
        }

        private Orientation orientation = Orientation.Horizontal;
        [Category("Appearance"), DefaultValue(Orientation.Horizontal)]
        public Orientation OrientationDirection
        {
            get { return orientation; }
            set
            {
                if (value != orientation)
                {
                    imgdone = false;
                    if (value == Orientation.Vertical)
                    {
                        int temp = Height;
                        Height = Width;
                        Width = temp;
                        int TEMP2 = moverwidth;
                        moverwidth = moverheight;
                        moverheight = TEMP2;

                    }
                    else
                    {
                        int temp = Height;
                        Height = Width;
                        Width = temp;
                        int TEMP2 = moverwidth;
                        moverwidth = moverheight;
                        moverheight = TEMP2;


                    }
                    orientation = value;
                    maketextrec(ref textrec);
                    updatarea();
                }
                Invalidate();
            }
        }

        int btnsize = 20;
        [Category("Appearance"), DefaultValue(20)]
        public int ControlButtonSize
        {
            get { return btnsize; }
            set { imgdone = false; btnsize = value; updatarea(); Invalidate(); }
        }

        bool showcontrolbuttons = false;
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowControlButtons
        {
            get { return showcontrolbuttons; }
            set
            {
                imgdone = false;
                showcontrolbuttons = value;
                updatarea();
                Invalidate();
            }
        }


        Color controlbuttoncolor = Color.Cyan;
        [Category("Appearance"), DefaultValue(typeof(Color), "Cyan")]
        public Color ControlButtonColor
        {
            get { return controlbuttoncolor; }
            set { imgdone = false; controlbuttoncolor = value; Invalidate(); }
        }
        Color[] maxgradientcolors = new Color[] { Color.Orange, Color.Yellow, Color.Green, Color.Cyan, Color.Blue };
        [Category("MaxedColors")]
        public Color[] MaxgradientColors
        {
            get { return maxgradientcolors; }
            set
            {
                imgdone = false; maxgradientcolors = value; Invalidate();
            }
        }
        float[] maxgradientpositions = new float[] { 0.0f, 0.25f, 0.50f, .75f, 1.0f };
        [Category("MaxedColors")]
        public float[] MaxGradientPositions
        {
            get { return maxgradientpositions; }
            set
            {
                imgdone = false;
                maxgradientpositions = value;

                Invalidate();
            }
        }





        #endregion
        #region Text
        float intvaluesposition = .5f;
        [DefaultValue(.5f), Category("Text"), Description("Indecates the postion of the possiple values if showvalues=true")]
        public float IntValuesPosition
        {
            get { return intvaluesposition; }
            set { intvaluesposition = value; Invalidate(); }
        }
        private Font valuestextfont = new Font("Tohama", 8);
        [DefaultValue(typeof(Font), "Tohama, 8"), Category("Text"), Description("Indecates the font of the int values if showvalues=true")]
        public Font ValuesTextFont
        {
            get { return valuestextfont; }
            set { valuestextfont = value; Invalidate(); }
        }
        uint floatplaces = 1;
        [DefaultValue(1), Category("Text")]
        public uint FloatPlaces
        {
            get { return floatplaces; }
            set { floatplaces = value; Invalidate(); }
        }
        private bool drawvalues = false;
        [DefaultValue(false), Category("Text"), Description("draw the possiple integar value along the scale")]
        public bool DrawValues
        {
            get { return drawvalues; }
            set { drawvalues = value; Invalidate(); }
        }
        private Color valuebackcolor = Color.White;
        [Category("Text"), DefaultValue(typeof(Color), "White"), Description("Indecates the back color of value if its shown")]
        public Color ValueTextBackColor
        {
            get { return valuebackcolor; }
            set { imgdone = false; valuebackcolor = value; Invalidate(); }
        }
        bool rotatetextinvertical = false;
        [Category("Text"), DefaultValue(false), Description("Rotate text in vertical orintaion")]
        public bool RotateTextInVertical
        {
            get { return rotatetextinvertical; }
            set { imgdone = false; rotatetextinvertical = value; maketextrec(ref textrec); updatarea(); Invalidate(); }
        }
        private bool showvalue = false;
        [Category("Text"), DefaultValue(false), Description("Indecates showing the value in the medium of scale")]
        public bool Showvalue
        {
            get { return showvalue; }
            set { imgdone = false; showvalue = value; Invalidate(); }
        }

       float valuestextstep = 1;
           [Category("Text"), DefaultValue(1)]    
        public float ValuesTextStep
        {
            get { return valuestextstep; }
            set { valuestextstep = value; Invalidate(); }
        }
        bool autosizetext = true;
        [Category("Text"), DefaultValue(true)]
        public bool AutoSizeText
        {
            get { return autosizetext; }
            set { autosizetext = value; imgdone = false; updatarea(); Invalidate(); }
        }

        Size textsize = Size.Empty;
        [Category("Text"), Description("the size of text to draw at only affect if autotextsize=false")]
        public Size TextSize
        {
            get { return textsize; }
            set { textsize = value; imgdone = false; updatarea(); Invalidate(); }
        }

        private StringAlignment textalgiment = StringAlignment.Center;
        [Category("Text"), DefaultValue(StringAlignment.Center), Description("alignment of the text only affect if autotextsize=false")]
        public StringAlignment TextAlgiment
        {
            get { return textalgiment; }
            set { imgdone = false; textalgiment = value; Invalidate(); }
        } private bool showtext = true;
        [Category("Text"), DefaultValue(true), Description("Indecates showing the text")]
        public bool Showtext
        {
            get { return showtext; }
            set
            {
                imgdone = false; showtext = value; updatarea(); Invalidate();
            }
        }
        private bool alwayslargevaluetext = false;
        [Category("Text"), DefaultValue(false), Description("Always Maximze the value text")]
        public bool AlwaysLargeValueText
        {
            get { return alwayslargevaluetext; }
            set { alwayslargevaluetext = value; Invalidate(); }
        }

        #endregion
        #region Behavior
        private bool detectvalueswhilemoving = false;
        [DefaultValue(false), Category("Behavior"), Description("Soon converts value to nearist inteager value while moving only if(integarvlue=true)")]
        public bool DetectValuesWhileMoving
        {
            get { return detectvalueswhilemoving; }
            set { detectvalueswhilemoving = value; }
        }
        bool enabletextmode = false ;
        [Category("Behavior"), DefaultValue(false )]
        public bool Enabletextmode
        {
            get { return enabletextmode; }
            set { enabletextmode = value; }
        }
        bool overcolorseffect = true;
        [Category("Behavior"), DefaultValue(true)]
        public bool OverColorsEffect
        {
            get { return overcolorseffect; }
            set { overcolorseffect = value; }
        }
        bool integarvalue = true;
        [Category("Behavior"), DefaultValue(true), Description("onverts value to nearist inteager value")]
        public bool OnlyIntValue
        {
            get { return integarvalue; }
            set { integarvalue = value; }
        }
        private float minimum = 0;
        [Category("Behavior"), DefaultValue(0)]
        public float Minimum
        {
            get { return minimum; }
            set { minimum = value; Invalidate(); }
        }
        private float maxmum = 100;
        [Category("Behavior"), DefaultValue(100)]
        public float Maxmum
        {
            get { return maxmum; }
            set { maxmum = value; Invalidate(); }
        }
        bool allowaddpointers = false;
        [Category("Behavior"), DefaultValue(false), Description("Make user able to add pointers at run time")]
        public bool AllowAddPointers
        {
            get { return allowaddpointers; }
            set { allowaddpointers = value; }
        }
        public class s : TypeConverter
        {  // Display the “+” symbol near the property name:
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                List<PropertyDescriptor> p = new List<PropertyDescriptor>();
                PropertyDescriptorCollection g = TypeDescriptor.GetProperties(typeof(PointF));
                foreach (PropertyDescriptor f in g)
                {
                    if (f.IsBrowsable)
                    { p.Add(f); }
                }
                return new PropertyDescriptorCollection(p.ToArray());
            }
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string)) return true;
                return base.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            /// Converts the specified string into a PointF
            /// </summary>
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    try
                    {
                        string s = (string)value;
                        string[] converterParts = s.Split(',');
                        float x = 0;
                        float y = 0;
                        if (converterParts.Length > 1)
                        {
                            x = float.Parse(converterParts[0].Trim());
                            y = float.Parse(converterParts[1].Trim());
                        }
                        else if (converterParts.Length == 1)
                        {
                            x = float.Parse(converterParts[0].Trim());
                            y = 0;
                        }
                        else
                        {
                            x = 0F;
                            y = 0F;
                        }
                        return new PointF(x, y);
                    }
                    catch
                    {
                        throw new ArgumentException("Cannot convert [" + value.ToString() + "] to pointF");
                    }

                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }
            /// <summary>
            /// Converts the PointF into a string
            /// </summary>
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value.GetType() == typeof(PointF))
                    {
                        PointF pt = (PointF)value;
                        return string.Format("{0}, {1}", pt.X, pt.Y);
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        Point areamargin = new Point(10, 80);
        [Category("Layout"), Description("value from 0 to 100 indecaties space to draw background"), DefaultValue(typeof(Point), "{10,80}")]
        public Point AreaMargin
        {
            get { return areamargin; }
            set { areamargin = value; maketextrec(ref textrec); updatarea(); Invalidate(); }
        }

        private KscaleAddPointerMode addmode = KscaleAddPointerMode.Anyway;
        [Category("Behavior"), DefaultValue(KscaleAddPointerMode.Anyway)]
        public KscaleAddPointerMode AddPointerMode
        {
            get { return addmode; }
            set { addmode = value; }
        }
        private bool syncmaxedcolorstopointers = false;
        [Category("Behavior"), DefaultValue(false)]
        public bool SyncMaxedColorsToPointers
        {
            get { return syncmaxedcolorstopointers; }
            set { syncmaxedcolorstopointers = value; }
        }
        private bool editable = false;
        [Category("Behavior"), DefaultValue(false), Description("User cant change value ,like prograss par")]
        public bool EditAble
        {
            get { return editable; }
            set { editable = value; }
        }
        [Category("Behavior"), Browsable(false), Description("Sets or gets the value of the Scale in integer"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get
            {
                int v;
                if (this.pointers.Count > 0)
                { v = pointers[0].Value; }
                else
                { v = 0; }
                return v;






            }
            set
            {
                this.ValueF = value;
            }
        }
        [Category("Behavior"), Description("Sets or gets the float value of the Scale ")]
        public float ValueF
        {
            get
            {
                float v;
                if (this.pointers.Count > 0)
                { v = pointers[0].ValueF; }
                else
                { v = 0; }
                return v;






            }
            set
            {
                if (this.pointers.Count > 0)
                {

                    if (value < minimum || value > maxmum)
                    {
                        throw new IndexOutOfRangeException(string.Format("Value of {0} isn't valid ,Value must be between minimum({1}) and maxmum ({2})", value, minimum, maxmum));
                    }
                    pointers[0].ValueF = value;
                    makepointerlocation();
                    Invalidate();

                }
            }
        }
        #endregion
        #region defultpointer

        Bitmap defultmoverimage = null;
        [Category("DefultMover"), DefaultValue(null)]

        public Bitmap DefultMoverImage
        {
            get { return defultmoverimage; }
            set
            {
                defultmoverimage = value;
                foreach (pointer p in pointers)
                { p.Image = value; }
                Invalidate();
            }
        }


        StringAlignment moveralgiment = StringAlignment.Center;
        [Category("DefultMover"), DefaultValue(StringAlignment.Center)]
        public StringAlignment DefultMoverAlgiment
        {
            get { return moveralgiment; }
            set
            {
                moveralgiment = value;
                foreach (pointer p in pointers)
                { p.MoverAlgiment = value; }
                Invalidate();
            }
        }
        private KscaleMoverShape movershape = KscaleMoverShape.Rect;
        [Category("DefultMover"), DefaultValue(KscaleMoverShape.Rect)]
        public KscaleMoverShape DefultMoverShape
        {
            get { return movershape; }
            set
            {
                movershape = value; foreach (pointer p in pointers)
                { p.MoverShape = value; } Invalidate();
            }
        }

        private Color movercolor = Color.Turquoise;
        [Category("DefultMover"), Description("indactes the color of the selected area"), DefaultValue(typeof(Color), "Turquoise")]
        public Color DefultMovercolor
        {
            get { return movercolor; }
            set
            {
                movercolor = value;
                foreach (pointer p in pointers)
                { p.Movercolor = value; }
                Invalidate();
            }
        }


        private bool showmoverborder = true;
        [Category("DefultMover"), DefaultValue(true)]
        public bool DefultShowMoverbBorder
        {
            get { return showmoverborder; }
            set
            {
                showmoverborder = value;
                foreach (pointer p in pointers)
                { p.ShowBorder = value; }
                Invalidate();
            }
        }
        bool defultfillmover = true;
        [Category("DefultMover"), DefaultValue(true)]
        public bool DefultFillMover
        {
            get { return defultfillmover; }
            set
            {
                defultfillmover = value;
                foreach (pointer p in pointers)
                { p.FillMover = value; }
                Invalidate();
            }
        }

        #endregion
        #region BrowseAble(false)
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage { get { return base.BackgroundImage; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } }

        int selectedindex = -1;
        [Browsable(false)]
        public int SelectedIndex
        {
            get { return selectedindex; }
            set
            {
                selectedindex = value;

                this.OnSelectedpointerchanged(new Inteventaregs(value));

            }
        }
        private Color selectedcolor = Color.Empty;
        [Browsable(false)]
        public Color SelectedColor
        {
            get { return selectedcolor; }
        }
        PointerCollection pointers;
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PointerCollection Pointers
        {
            get { return pointers; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public pointer SelectedPointer
        {
            get { return this.pointers[selectedindex]; }
            set { SelectedIndex = value.Index; }
        }
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsOnValueChanging { get { return ismove; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float MinValue
        {
            get
            {
        {
            float minv = 0;
         for(int i=0;i<this.pointers.Count;i++)
         {
             if (i == 0)
             { minv = this.pointers[0].ValueF; }
             else
             {
                 if (  this.pointers[i].ValueF<minv)
                        minv = this.pointers[i].ValueF;
             }     
         } return minv; }}}
          [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]   
        public float MaxValue
        {
            get
            {
                {
                    float maxv = 0;
                    for (int i = 0; i < this.pointers.Count; i++)
                    {
                        if (i == 0)
                        { maxv = this.pointers[0].ValueF; }
                        else
                        {
                            if ( this.pointers[i].ValueF>maxv)
                                maxv = this.pointers[i].ValueF;
                        }
                    } return maxv;
                }
            }
        } 
        
        
        #endregion

        #endregion
        #region private

        public bool setuped = false;
        private bool isdown = false; private bool ismove = false;

        enum mus { none, mainover, leftover, rightover }
        mus mous = mus.none;
        private mus Mous
        {
            get { return mous; }
            set
            {
                mous = value;
            }
        }
        int overpointer = -1;
        RectangleF stringvaluerec = new RectangleF();
        RectangleF textrec = new RectangleF(0, 0, 1, 1);
        RectangleF AreaRec = new RectangleF();
        Rectangle leftbtnrec = new Rectangle();
        Rectangle rightbtnrec = new Rectangle();

        private class clnd : IComparable
        {

            public int CompareTo(Object other)
            {
                clnd oth = other as clnd;
                int res = 0;

                res = this.F.CompareTo(oth.F);

                return res;
            }

            public clnd(float fl, Color co, pointer pp)
            {
                this.F = fl;
                this.C = co;
                this.p = pp;
            }
            public clnd()
            { }
            float f = 0;

            public float F
            {
                get { return f; }
                set { f = value; }
            }
            private int firstindex;

            public int Firstindex
            {
                get { return firstindex; }
                set { firstindex = value; }
            }
            private pointer p;

            public pointer P
            {
                get { return p; }
                set { p = value; }
            }

            Color c = Color.Empty;

            public Color C
            {
                get { return c; }
                set { c = value; }
            }

        }
        #endregion
        #region voids
        public void BeginInit()
        {
        }
        public void EndInit()
        {
            setuped = true;

        }
        public void SafeSetValue(float value, int pointerindx = 0)
        {
            if (value > maxmum)
            { value = maxmum; }
            if (value < minimum)
            { value = minimum; }
            this.pointers[pointerindx].ValueF = value;
        }
        public void ForcedSetValue(float value, int pointerindx = 0)
        {
            if (value > maxmum)
            { maxmum = value; }
            if (value < minimum)
            { minimum = value; }
            this.pointers[pointerindx].ValueF = value;
        }
        internal void OnSelectionChanged()
        {
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));

            // See if the primary selection is one of our buttons
            foreach (pointer p in pointers)
            {
                if (s.PrimarySelection == p)
                {
                    overpointer = p.Index;
                    Invalidate();
                    break;
                }
            }



        }
        public void OnValueChanged(Kscaleeventargs e)
        {
            if (ValueChanged != null && setuped&&hangvaluechangedevent==false)
            {
                ValueChanged(this, e);
            }
        }
        public void OnValueChanging(Kscaleeventargs e)
        {
            if (ValueChanging != null && setuped&&hangvaluechangingevent==false)
            { ValueChanging(this, e); }
        }
        public void OnPointerAdded(kscalepointereventargs e)
        {
            if (PointerAdd != null && setuped)
            {
                PointerAdd(this, e);
            }
        }

        /// <summary>
        /// Makes the equals poistions for colors
        /// </summary>
        public void MakeMaxGradientPostions()
        {
            List<float> temp = new List<float>();
            temp.Add(0.0f);
            for (int i = 1; i < maxgradientcolors.Length - 1; i++)
            {
                float a = 1;
                float z = maxgradientcolors.Length - 1;
                float r = a / z;

                float v = r * (float)i;
                temp.Add(v);
            }
            temp.Add(1.0f);
            MaxGradientPositions = temp.ToArray();

        }
        void maketextrec(ref RectangleF rec)
        {
            StringFormat sf = new StringFormat();

            if (orientation == Orientation.Horizontal)
            {
                rec.Size = this.CreateGraphics().MeasureString(this.Text, Font, Width, sf);
                rec.X = 0;
                rec.Y = Height / 2 - textrec.Height / 2;
            }
            else
            {
                if (rotatetextinvertical)
                {
                    sf.FormatFlags = StringFormatFlags.DirectionVertical;
                }

                rec.Size = this.CreateGraphics().MeasureString(this.Text, Font, (int)AreaRec.Width, sf);
                rec.X = Width / 2 - textrec.Width / 2;
                rec.Y = 0;

            }


        }
        void updatarea()
        {
            int leftbtn = 0;
            if (showcontrolbuttons) { leftbtn = btnsize; }
            RectangleF dtextrec = textrec;


            if (showtext == false)
            { dtextrec = RectangleF.Empty; }
            else
            {
                if (autosizetext == false)
                { dtextrec.Size = TextSize; }

            }
            if (orientation == Orientation.Horizontal)
            {
                AreaRec.X = dtextrec.Width + leftbtn + 2;

                AreaRec.Y = 0;
                AreaRec.Height = this.Height;
                AreaRec.Width = this.Width - AreaRec.X - leftbtn;
                int btnh = (int)dtextrec.Height;
                if (dtextrec.Height >= 0)
                {
                    if (AreaRec.Height < 20)
                    { btnh = (int)AreaRec.Height - 1; }
                    else { btnh = 20; }
                }
                leftbtnrec = Rectangle.Round(new RectangleF(AreaRec.X - btnsize, AreaRec.Y + AreaRec.Height / 2 - leftbtnrec.Height / 2, btnsize - 4, btnh));

                rightbtnrec = Rectangle.Round(new RectangleF(AreaRec.Right + 2, leftbtnrec.Y, leftbtn - 4, leftbtnrec.Height));

            }
            else
            {
                AreaRec.X = 0;
                AreaRec.Y = dtextrec.Height + leftbtn + 2; ;

                AreaRec.Height = this.Height - AreaRec.Y - leftbtn;
                AreaRec.Width = this.Width - AreaRec.X;

                int btnw = (int)dtextrec.Height;
                if (dtextrec.Width >= 0)
                {
                    if (AreaRec.Width < 20)
                    { btnw = (int)AreaRec.Width - 1; }
                    else { btnw = 20; }
                }
                leftbtnrec = Rectangle.Round(new RectangleF(AreaRec.X + AreaRec.Width / 2 - leftbtnrec.Width / 2, AreaRec.Y - btnsize, btnw, btnsize - 4));

                rightbtnrec = Rectangle.Round(new RectangleF(leftbtnrec.X, AreaRec.Bottom + 2, leftbtnrec.Width, leftbtn - 4));

            }

            makepointerlocation();
        }
        public void makepointerlocation(int itemindex)
        { makepointerlocation(pointers[itemindex]); }
        public void makepointerlocation(pointer p)
        {

            if (orientation == Orientation.Horizontal)
            {
                p.X = ksclconvert(false, p.ValueF);
                p.Y = 0;
                p.Rec = new RectangleF(p.Rec.Location, new SizeF(moverwidth, AreaRec.Height));


            }
            else if (orientation == Orientation.Vertical)
            {
                p.Y = ksclconvert(false, p.ValueF);
                p.X = 0;
                p.Rec = new RectangleF(p.Rec.Location, new SizeF(AreaRec.Width, moverheight));

            }

        }
        public void makepointerlocation()
        {
            for (int i = 0; i < pointers.Count; i++)
            {
                makepointerlocation(i);
            }
        }
        clnd[] clnds;
        private void makemaexdsync(bool editcolors)
        {




            float fmax = maxmum;
            float f1 = 0;
            float ff = 0;



            clnds = new clnd[pointers.Count];



            for (int i = 0; i < pointers.Count; i++)
            {
                f1 = pointers[i].ValueF;

                ff = f1 / fmax;

                clnds[i] = new clnd(pointers[i].ValueF, pointers[i].Movercolor, pointers[i]);

            }


            Array.Sort(clnds);
            clnd[] tempclnd = (clnd[])clnds.Clone();

            int g = 0;
            int dx = 0;

            ff = pointers[pointers.Count - 1].ValueF / fmax;
            if (pointers[pointers.Count - 1].ValueF != maxmum)
            { g += 1; }


            ff = pointers[0].ValueF / fmax;
            if (pointers[0].ValueF != minimum)
            {
                g += 1;

                dx = 1;
            }

            if (g != 0)
            {
                clnds = new clnd[clnds.Length + g];
                if (editcolors)
                {
                    maxgradientcolors = new Color[clnds.Length];
                    maxgradientpositions = new float[clnds.Length];
                }
                for (int i = 0; i < tempclnd.Length; i++)
                {
                    clnds[i + dx] = tempclnd[i];
                }
                if (clnds[clnds.Length - 1] == null)
                {
                    clnds[clnds.Length - 1] = new clnd(maxmum, clnds[clnds.Length - 2].C, null);

                }
                if (clnds[0] == null)
                {
                    clnds[0] = new clnd(minimum, clnds[1].C, null);

                }

            }
            if (editcolors)
            {
                float minm = minimum;
                for (int i = 0; i < clnds.Length; i++)
                {
                    maxgradientcolors[i] = clnds[i].C;
                    f1 = clnds[i].F;
                    ff = (f1 - minm) / (fmax - minm);

                    maxgradientpositions[i] = ff;

                }
            }




        }
        #endregion
        #region eventes

        /// <summary>
        /// Occurs when value has been changed
        /// </summary>
        public event Kscaleeventhandler ValueChanged;
        /// <summary>
        /// Occurs while value is changing
        /// </summary>
        public event Kscaleeventhandler ValueChanging;

        /// <summary>
        /// Ocurrs when pointer added to the poinerscollection      
        /// </summary>
        public event Kscalepointereventhandler PointerAdd;
        /// <summary>
        /// Ocurrs when slected pointer changed
        /// </summary>
        public event inteventhandler Selectedpointerchanged;
        /// <summary>
        /// Rise the event with its args 
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedpointerchanged(Inteventaregs e)
        {
            if (this.Selectedpointerchanged != null && setuped)
            { this.Selectedpointerchanged(this, e); }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e); Mous = mus.none; overpointer = -1; Invalidate();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            maketextrec(ref textrec);

            updatarea();

        }


        protected override void OnMouseEnter(EventArgs e)
        {

            base.OnMouseEnter(e); mous = mus.mainover; Invalidate();
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (DesignMode) return;
            base.OnMouseClick(e);
            if (showcontrolbuttons)
            {
                if (leftbtnrec.Contains(e.Location))
                {
                    if (this.Value - 1 >= this.Minimum)
                    {

                        this.Value -= 1;

                    }
                }
                else if (rightbtnrec.Contains(e.Location))
                {
                    if (this.Value + 1 <= this.Maxmum)
                    {

                        this.Value += 1;

                    }
                }
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (showcontrolbuttons)
            {
                if (leftbtnrec.Contains(e.Location))
                {

                    if (this.Value - 10 >= this.Minimum)
                    {

                        this.Value -= 10;

                    }
                    else
                    { this.ValueF = minimum; }
                }
                else if (rightbtnrec.Contains(e.Location))
                {
                    if (this.Value + 10 <= this.Maxmum)
                    {

                        this.Value += 10;

                    }
                    else
                    { this.ValueF = maxmum; }
                }
            }
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            maketextrec(ref textrec);
            updatarea();
            Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Size szz = this.Size;

            if (this.Height < 10)
            { szz.Height = 10; }

            if (this.Width < 10)
            { szz.Width = 10; }
            this.Size = szz;
            maketextrec(ref textrec);
            updatarea();
            Invalidate();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();

        }

        #endregion

        #region down,move,up,value,convert


        bool addrec = false;
        private float prevalue = 0;

        public float PreValue
        {
            get { return prevalue; }
            set { prevalue = value; }
        }
        private float prescrollingvalue = 0;

        public float PreScrollingValue
        {
            get { return prescrollingvalue; }
            set { prescrollingvalue = value; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            prevalue = Value;
            if (DesignMode)
            {




                Rectangle wrct;
                ISelectionService s;
                ArrayList a;

                foreach (pointer p in pointers)
                {
                    wrct = Rectangle.Round(p.Rec);
                    if (wrct.Contains(e.X, e.Y))
                    {
                        s = (ISelectionService)GetService(typeof(ISelectionService));
                        a = new ArrayList();
                        a.Add(p);
                        //     s.SetSelectedComponents(a);

                        //  return;

                    }
                }
            }
            //  isdown = true;

            else
            { }

            base.OnMouseDown(e);
            bool addpointer = false;

            addrec = false;
            if (MouseButtons == MouseButtons.Left && AreaRec.Contains(e.Location) && EditAble == false)
            {
                if (allowaddpointers && !DesignMode)
                {

                    switch (orientation)
                    {
                        case Orientation.Horizontal:
                            if (e.X <= 40 + AreaRec.X)
                            {
                                addrec = true;
                            }
                            if (e.X <= 8 + AreaRec.X)
                            {

                                addpointer = true;
                                if (AddPointerMode == KscaleAddPointerMode.when_no_pointer_near_minmum)
                                {
                                    foreach (pointer p in pointers)
                                    {
                                        if (p.X <= 8 + AreaRec.X)
                                        {
                                            addpointer = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case Orientation.Vertical:
                            if (e.Y <= 40 + AreaRec.Y)
                            {
                                addrec = true;
                            }
                            if (e.Y <= 8 + AreaRec.Y)
                            {
                                addpointer = true;
                                foreach (pointer p in pointers)
                                {
                                    if (p.Y <= 8 + AreaRec.Y)
                                    {
                                        addpointer = false;
                                        break;
                                    }
                                }
                            }
                            break;
                    }

                }

                if (addpointer == true)
                {

                    pointer op = new pointer(pointers);
                    fnc.CopyProperties(op, pointers[0], true);
                    op.ValueF = 0;
                    this.pointers.Add(op);
                    this.SelectedIndex = this.pointers.Count - 1;




                }


                bool moveactvpointer = true;
                if (pointers.Count > 1 && addpointer == false)
                {
                    foreach (pointer p in pointers)
                    {

                        if (p.Rec.Contains(e.Location))
                        {
                            moveactvpointer = true;

                            this.SelectedIndex = p.Index;
                            break;


                        }
                        else { moveactvpointer = true; }
                    }

                }
                else
                {
                    moveactvpointer = true;
                }

                if (moveactvpointer)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        if (e.X > this.SelectedPointer.X && e.X < this.SelectedPointer.X + moverwidth)
                        {
                            this.SelectedPointer.Downx = e.X - this.SelectedPointer.X;
                        }
                    }
                    else
                    {
                        if (e.Y > this.SelectedPointer.Y && e.X < this.SelectedPointer.Y + moverheight)
                        {
                            this.SelectedPointer.Downx = e.Y - this.SelectedPointer.Y;
                        }
                    }

                    isdown = true;
                }
            }
            else isdown = false;
            base.OnMouseDown(e);

        }
        private float ksclconvert(bool topercent, float num)
        { return ksclconvert(topercent, num, true); }
        private float ksclconvert(bool topercent, float num, bool dodefultrightvalue)
        {
            float v = 0;
            if (orientation == Orientation.Horizontal)
            {
                if (topercent)
                {



                    v = (fnc.divdec(num - AreaRec.X, AreaRec.Width - moverwidth)) * (maxmum - minimum) + minimum;
                    if (v < minimum)
                    { v = minimum; }
                    else if (v > maxmum)
                    { v = maxmum; }

                    if (integarvalue && dodefultrightvalue)
                    {
                        if (v != fnc.tint(v, false))
                        {
                            float back = v - fnc.tint(v, false);
                            float front = (fnc.tint(v, false) + 1) - v;
                            if (back < front)
                            { v = fnc.tint(v, false); }
                            else
                            { v = fnc.tint(v, false) + 1; }

                        }
                    }
                }
                else
                {
                    v = (int)(fnc.tint((fnc.divdec(num - minimum, maxmum - minimum)) * (AreaRec.Width - moverwidth)) + AreaRec.X);
                }
            }
            else if (orientation == Orientation.Vertical)
            {
                if (topercent)
                {
                    v = fnc.tint((fnc.divdec(num - AreaRec.Y, AreaRec.Height - moverheight)) * (maxmum - minimum) + minimum);
                    if (v < minimum)
                    { v = minimum; }
                    else if (v > maxmum)
                    { v = maxmum; }

                    if (integarvalue && dodefultrightvalue)
                    {
                        if (v != fnc.tint(v, false))
                        {
                            float back = v - fnc.tint(v, false);
                            float front = (fnc.tint(v, false) + 1) - v;
                            if (back < front)
                            { v = fnc.tint(v, false); }
                            else
                            { v = fnc.tint(v, false) + 1; }

                        }
                    }

                }
                else
                {
                    v = (int)(fnc.tint((fnc.divdec(num - minimum, maxmum - minimum)) * (AreaRec.Height - moverheight)) + AreaRec.Y);
                }

            }
            return v;
        }



        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isdown == false && EditAble == false)
            {
                overpointer = -1;

                foreach (pointer p in this.Pointers)
                {
                    if (p.Rec.Contains(e.Location))
                    { overpointer = p.Index; break; }
                }
            }
            if (isdown && (this.pointers.Count > 0))
            {
                overpointer = this.SelectedIndex;
                ismove = true;
                prescrollingvalue = this.SelectedPointer.ValueF;
                if (orientation == Orientation.Horizontal)
                {

                    pointers[this.SelectedIndex].X = e.X - this.SelectedPointer.Downx;

                    if (this.SelectedPointer.X > AreaRec.Right - moverwidth)
                    {
                        this.SelectedPointer.X = AreaRec.Right - moverwidth;

                    }
                    else if (this.SelectedPointer.X < AreaRec.X)
                    {
                        this.SelectedPointer.X = AreaRec.X;
                    }

                    if (this.SelectedPointer.ValueF != ksclconvert(true, this.SelectedPointer.X, true))
                    {float v=ksclconvert(true, this.SelectedPointer.X, true);
                        if (v<this.minimum)v=minimum;
                        if (v>maxmum)v=maxmum;
                        this.SelectedPointer.ValueF = v;
                        if (this.ValueChanging != null)
                        {

                            ValueChanging(this, new Kscaleeventargs((int)ksclconvert(true, this.SelectedPointer.X, true), SelectedColor));

                        }
                    }
                    if (detectvalueswhilemoving)
                    {
                        this.SelectedPointer.X = ksclconvert(false, this.SelectedPointer.ValueF);
                    }

                }

                else if (orientation == Orientation.Vertical)
                {
                    pointers[this.SelectedIndex].Y = e.Y - this.SelectedPointer.Downx;

                    if (this.SelectedPointer.Y > AreaRec.Bottom - moverheight)
                    {
                        this.SelectedPointer.Y = AreaRec.Bottom - moverheight;

                    }
                    else if (this.SelectedPointer.Y < AreaRec.Y)
                    {
                        this.SelectedPointer.Y = AreaRec.Y;
                    }

                    if (this.SelectedPointer.ValueF != ksclconvert(true, this.SelectedPointer.Y, true))
                    {
                        float v = ksclconvert(true, this.SelectedPointer.Y, true);
                        if (v < this.minimum) v = minimum;
                        if (v > maxmum) v = maxmum;
                        this.SelectedPointer.ValueF = v;
                     
                        if (this.ValueChanging != null)
                        {

                            ValueChanging(this, new Kscaleeventargs((int)ksclconvert(true, this.SelectedPointer.Y, true), SelectedColor));

                        }
                    }
                    if (detectvalueswhilemoving)
                    {
                        this.SelectedPointer.Y = ksclconvert(false, this.SelectedPointer.ValueF);
                    }


                }



            }

            else
            {


                if (AreaRec.Contains(e.Location))
                {
                    Mous = mus.mainover;
                }
                else if (showcontrolbuttons)
                {
                    if (leftbtnrec.Contains(e.Location))
                    {
                        Mous = mus.leftover;
                    }
                    else if (rightbtnrec.Contains(e.Location))
                    {
                        Mous = mus.rightover;
                    }
                    else
                    { Mous = mus.none; }
                }
                else { Mous = mus.none; }
            }

            Invalidate();


        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            addrec = false;
            if (!ismove && isdown)
            {
                if (orientation == Orientation.Horizontal)
                {

                    this.SelectedPointer.X = e.X - this.SelectedPointer.Downx;

                    if (this.SelectedPointer.X > AreaRec.Right - moverwidth)
                    {
                        this.SelectedPointer.X = AreaRec.Right - moverwidth;
                    }
                    else if (this.SelectedPointer.X < AreaRec.X)
                    {
                        this.SelectedPointer.X = AreaRec.X;
                    }
                    this.SelectedPointer.ValueF = ksclconvert(true, this.SelectedPointer.X);
                    if (OnlyIntValue)
                    {
                        this.SelectedPointer.X = ksclconvert(false, this.SelectedPointer.ValueF);
                    }
                }
                else if (orientation == Orientation.Vertical)
                {
                    this.SelectedPointer.Y = e.Y - this.SelectedPointer.Downx;

                    if (this.SelectedPointer.Y > AreaRec.Bottom - moverheight)
                    {
                        this.SelectedPointer.Y = AreaRec.Bottom - moverheight;
                    }
                    else if (this.SelectedPointer.Y < AreaRec.Y)
                    {
                        this.SelectedPointer.Y = AreaRec.Y;
                    }
                    this.SelectedPointer.ValueF = ksclconvert(true, this.SelectedPointer.Y);
                    if (OnlyIntValue)
                    {
                        this.SelectedPointer.Y = ksclconvert(false, this.SelectedPointer.ValueF);
                    }
                }

            }
            ismove = false;
            if (isdown)
            {
                if (orientation == Orientation.Horizontal)
                {
                    if (OnlyIntValue && !detectvalueswhilemoving)
                    {
                        this.SelectedPointer.X = ksclconvert(false, this.SelectedPointer.ValueF);
                    }
                }
                else
                {
                    if (OnlyIntValue && !detectvalueswhilemoving)
                    {
                        this.SelectedPointer.Y = ksclconvert(false, this.SelectedPointer.ValueF);
                    }
                }
                isdown = false; ismove = false;

                if (ValueChanged != null)
                {
                    ValueChanged(this, new Kscaleeventargs(this.Value, SelectedColor));
                }
                Invalidate();
            }
            isdown = false; ismove = false;
        }

        #endregion
        Bitmap wallimg;

        public bool imgdone = false;

        private int sr = -1;
        [DefaultValue(-1), Category("SyucRGBColor")]
        public int Sr
        {
            get { return sr; }
            set { sr = value; Invalidate(); }
        }
        private int sg = -1;
        [DefaultValue(-1), Category("SyucRGBColor")]
        public int Sg
        {
            get { return sg; }
            set { sg = value; Invalidate(); }
        }
        private int sb = -1;
        [DefaultValue(-1), Category("SyucRGBColor")]
        public int Sb
        {
            get { return sb; }
            set { sb = value; Invalidate(); }
        }
        private bool showallpointersvalues = false;
          [DefaultValue(false) , Category("Text")]
      
        public bool Showallpointersvalues
        {
            get { return showallpointersvalues; }
            set { showallpointersvalues = value; }
        }

          private float[] valuestextposition = new float[] { };
          [Category("ValuesText")]     
         
          public float[] Valuestextposition
          {
              get { return valuestextposition; }
              set { valuestextposition = value; }
          }
          private string[] valuestexts = new string[] { };
           [ Category("ValuesText")]     
          public string[] Valuestexts
          {
              get { return valuestexts; }
              set { valuestexts = value; }
          }
           private bool showtextvalues = false;
           [Category("ValuesText")]     
        
           public bool Showtextvalues
           {
               get { return showtextvalues; }
               set { showtextvalues = value; }
           }
           private bool textmode = false;

           public bool Textmode
           {
               get { return textmode; }
              private  set { textmode = value; }
           }
           protected override void OnDoubleClick(EventArgs e)
           {
               base.OnDoubleClick(e);
               if (enabletextmode)
               {
                   textmode = true;
                   edittext = "Enter value";
                   Invalidate();
                   this.Focus();
               }
           }
           protected override void OnLostFocus(EventArgs e)
           {
             base.OnLostFocus(e);
             if (textmode)
             {
                 float v;
                 if (float.TryParse(edittext, out v))
                 {
                     if (v < minimum)
                     {
                         minimum = v;
                     }
                     else if (v > maxmum)
                     {
                         maxmum = v;
                     }
                     this.SelectedPointer.ValueF = v;
                     this.makepointerlocation();
                   
                 }
                 textmode = false;
             } Invalidate();
           }
         
        string edittext="";
      
           protected override void OnKeyDown(KeyEventArgs e)
           {  
               base.OnKeyDown(e);
               if (edittext=="Enter value")
               {edittext="";}
           if (e.KeyCode==Keys.Enter)
           { textmode = false; }
           else if (e.KeyCode==Keys.NumPad1||e.KeyCode==Keys.D1)
           {
                edittext+="1";
           }
           else if (e.KeyCode==Keys.NumPad2||e.KeyCode == Keys.D2)
           {
               edittext += "2";
           }
           else if (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3)
           {
               edittext += "3";
           }
           else if (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4)
           {
               edittext += "4";
           }
           else if (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5)
           {
               edittext += "5";
           }
           else if (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6)
           {
               edittext += "6";
           }
           else if (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7)
           {
               edittext += "7";
           }
           else if (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8)
           {
               edittext += "8";
           }
           else if (e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9)
           {
               edittext += "9";
           }
           else if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0)
           {
               edittext += "0";
           }
           else if (e.KeyCode == Keys.Decimal )
           {
               edittext += ".";
           }
           else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
           {
               if (edittext.Length > 0)
               {
                   edittext = edittext.Substring(0, edittext.Length - 1);
               }
           }
           else if (e.KeyCode == Keys.Subtract)
           {
               edittext += "-";
           }

           else
           {
             //  MessageBox.Show("Input Only numbers \n {-0123456789.}");
               textmode = true;
           }
               if (edittext.Length>6)
               {
                   edittext = edittext.Substring(0, 6);
               MessageBox.Show("Max Lenght 6 numbers !");
               textmode = true;
               }
               Invalidate();
           }

           protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
           {
               base.OnPreviewKeyDown(e); 
               if (e.KeyCode==Keys.Enter)
               {
                   float v;
                   if (float.TryParse(edittext, out v))
                   {
                       this.ForcedSetValue(v,this.selectedindex);
                       this.makepointerlocation();
                       Invalidate();
                   }
               }
           
           }
          
         
        #region drawing
        RectangleF mrgnrec = new RectangleF();
        RectangleF selctionrec = new RectangleF();
        RectangleF outerselctionrec = new RectangleF();
        StringFormat sf = new StringFormat();
        int brushangle = 0;
        bool syncRGBcolors = false;
        [DefaultValue(false)]
        public bool SyncRGBcolors
        {
            get { return syncRGBcolors; }
            set { syncRGBcolors = value; Invalidate(); }
        }

        private System.Drawing.Bitmap GEtSyncedimg(int r, int g, int b, int h, int w)
        {
            Bitmap bb = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                int rlx = (int)(fnc.divdec(x, w) * (float)255);
                Color v = Color.Black;
                if (r == -1)
                { v = Color.FromArgb(rlx, g, b); }
                else if (g == -1)
                { v = Color.FromArgb(r, rlx, b); }
                else if (b == -1)
                { v = Color.FromArgb(r, g, rlx); }
                for (int y = 0; y < h; y++)
                {
                    bb.SetPixel(x, y, v);
                }
            }
            return bb;

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap b = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (pointers.Count <= 0)
            {
                g.Clear(Color.Black); e.Graphics.DrawImage(b, 0, 0); return;
            }

            Pen borderpen = new Pen(bordercolor, 1);
            #region Makecolors
            Color c1 = smoothcolor; Color c2 = maincolor; Color brdr = bordercolor; Color mov = movercolor; Color lbtnclr = controlbuttoncolor; Color rbtnclr = controlbuttoncolor;
            if (overcolorseffect && !editable)
            {
                switch (mous)
                {
                    case mus.mainover:

                        movercolor = ControlPaint.Light(movercolor, .05f);
                        maincolor = ControlPaint.LightLight(maincolor);

                        break;
                    case mus.leftover:
                        lbtnclr = ControlPaint.Light(controlbuttoncolor, 1.0f);
                        break;
                    case mus.rightover:
                        rbtnclr = ControlPaint.Light(controlbuttoncolor, 1.0f);

                        break;
                }
            }
            #endregion

            if (!IsOnValueChanging || true)
            {
                #region layout
                if (orientation == Orientation.Vertical && RotateTextInVertical)
                { sf.FormatFlags = StringFormatFlags.DirectionVertical; }









                brushangle = 90;
                if (orientation == Orientation.Horizontal)
                { brushangle = 0; }

                if (orientation == Orientation.Horizontal)
                {

                    mrgnrec = new RectangleF(AreaRec.X, AreaRec.Y + AreaRec.Height * (((float)areamargin.X) / 100f), AreaRec.Width, AreaRec.Height * (((float)areamargin.Y) / 100f));
                }
                else
                {
                    mrgnrec = new RectangleF(AreaRec.X + AreaRec.Width * (((float)areamargin.X) / 100f), AreaRec.Y, AreaRec.Width * (((float)areamargin.Y) / 100f), AreaRec.Height);



                }
                if (mrgnrec.Width < 1)
                { mrgnrec.Width = 1; }
                if (mrgnrec.Height < 1)
                { mrgnrec.Height = 1; }





                #endregion
            }
      #region drawing
            if (!IsOnValueChanging || SyncMaxedColorsToPointers || true)
            {

                #region MakeBrush & draw area

                if (this.BackColor == Color.Transparent)
                {
                    g.Clear(Color.Transparent);
                }

                ///in flaty mode first rec get out margnrec due to rectangle filter                    
                g.SetClip(g.GenerateRoundedRectangle(mrgnrec, roundamount, rectedgefilter));
                #region draw back transparent rec
                if (drawbacktranspesryrec)
                {
                    bool blackrec = true;
                    SolidBrush recb = new SolidBrush(Color.Black);
                    for (int xxx = (int)mrgnrec.X; xxx < mrgnrec.Right; xxx += 5)
                    {
                           blackrec = !blackrec;
                       bool blackrec2 = blackrec;
                           
                        for (int yyy = (int)mrgnrec.Y; yyy < mrgnrec.Bottom; yyy += 5)
                        {
                            blackrec2 =! blackrec2;
                           if (blackrec2)
                            {
                                recb.Color = Color.Black;

                            }
                            else
                            {
                                recb.Color = Color.White;
                            }
                            Size sz = new Size(5, 5);
                            if (xxx + 5 > mrgnrec.Right)
                            {
                                sz.Width = (int)mrgnrec.Right - xxx;
                            }

                            else if (yyy + 5 > mrgnrec.Bottom)
                            {
                                sz.Height = (int)mrgnrec.Bottom - yyy;
                            }
                            g.FillRectangle(recb, new Rectangle(xxx, yyy, sz.Width, sz.Height));


                        }

                    }

                }
                #endregion
                LinearGradientBrush scllnr = new LinearGradientBrush(mrgnrec, smoothcolor, Maincolor, 360 + brushangle);
                if (syncRGBcolors)
                {
                    if ((sb != -1 && sr != -1) || (sb != -1 && sg != -1) || (sg != -1 && sr != -1))
                    {
                        g.DrawImage(GEtSyncedimg(sr, sg, sb, (int)mrgnrec.Height, (int)mrgnrec.Width), mrgnrec.X, mrgnrec.Y, mrgnrec.Width, mrgnrec.Height);
                    }
                }
                else
                {
                    switch (style)
                    {
                        case KscalePaintMode.Flat:
                            g.FillRoundedRectangle(new SolidBrush(maincolor), scllnr.Rectangle, roundamount, Rectangleedegfilter);

                            break;
                        case KscalePaintMode.flaty:
                            RectangleF fsclrec = mrgnrec;
                            int ang = 0;
                            if (orientation == Orientation.Horizontal)
                            {
                                fsclrec.Height = mrgnrec.Height / 5 * 2;
                            }
                            else
                            {
                                ang = 180;
                                fsclrec.Width = mrgnrec.Width / 5 * 2;
                            }

                            scllnr = new LinearGradientBrush(fsclrec, smoothcolor, Maincolor, 90 + brushangle + ang);
                            g.FillRoundedRectangle(new SolidBrush(maincolor), mrgnrec, roundamount, Rectangleedegfilter);
                            g.FillRoundedRectangle(scllnr, scllnr.Rectangle, roundamount, Rectangleedegfilter);

                            break;
                        case KscalePaintMode.gradient:
                            g.FillRoundedRectangle(scllnr, scllnr.Rectangle, roundamount, Rectangleedegfilter);

                            break;
                        case KscalePaintMode.Maxed:
                            if (SyncMaxedColorsToPointers)
                            {
                                makemaexdsync(true);
                            }
                            if (maxgradientpositions.Length == maxgradientcolors.Length)
                            {


                                ColorBlend cb = new ColorBlend();
                                cb.Colors = maxgradientcolors;
                                cb.Positions = maxgradientpositions;
                                try
                                {
                                    scllnr.InterpolationColors = cb;
                                }
                                catch
                                { }
                                g.FillRoundedRectangle(scllnr, scllnr.Rectangle, roundamount, Rectangleedegfilter);

                            }
                            break;
                    }
                }
                g.ResetClip();
                Brush mainb = scllnr;
                #endregion
                if (textmode == false)
                {
                    #region Detect SelectedColor

                    if (orientation == Orientation.Horizontal)
                    {
                        if (this.SelectedPointer.X > 0)
                        {
                            try
                            {
                                selectedcolor = b.GetPixel((int)(this.SelectedPointer.X), Height / 2);
                            }
                            catch
                            { selectedcolor = Color.Empty; }
                        }

                    }

                    else
                    {
                        if (this.SelectedPointer.Y > 0)
                        {
                            try
                            {
                                selectedcolor = b.GetPixel(Width / 2, (int)(this.SelectedPointer.Y));
                            }
                            catch
                            {
                                selectedcolor = Color.Empty;
                            }

                        }
                    }
                    #endregion
                }
                    #region draw control buttomns================================================

                    Brush rb = new SolidBrush(rbtnclr);
                    Brush lb = new SolidBrush(lbtnclr);
                    if (showcontrolbuttons)
                    {
                        borderpen.Width = 2;

                        //leftbutton
                        //------------
                        //background

                        g.FillRoundedRectangle(lb, leftbtnrec, roundamount, Rectangleedegfilter);

                        //-
                        g.DrawLine(borderpen, leftbtnrec.X + 4, leftbtnrec.Y + leftbtnrec.Height / 2, leftbtnrec.Right - 4, leftbtnrec.Y + leftbtnrec.Height / 2);

                        //border
                        if (showborder)
                        {
                            g.DrawRoundedRectangle(borderpen, leftbtnrec, roundamount, Rectangleedegfilter);
                        }
                        //right button
                        //-----------------
                        //fillground
                        g.FillRoundedRectangle(rb, rightbtnrec, roundamount, Rectangleedegfilter);
                        //draw -
                        g.DrawLine(borderpen, rightbtnrec.X + 4, rightbtnrec.Y + rightbtnrec.Height / 2, rightbtnrec.Right - 4, rightbtnrec.Y + rightbtnrec.Height / 2);
                        //draw |
                        g.DrawLine(borderpen, rightbtnrec.X + rightbtnrec.Width / 2, rightbtnrec.Y + 4, rightbtnrec.X + rightbtnrec.Width / 2, rightbtnrec.Bottom - 4);

                        //border
                        if (showborder)
                        {
                            g.DrawRoundedRectangle(borderpen, rightbtnrec, roundamount, Rectangleedegfilter);
                        }
                    }

                    #endregion
                    if (textmode == false)
                    {
                        #region draw mudium lines=====================================================================================
                        if (showmediumline)
                        {
                            borderpen.DashStyle = DashStyle.Dash;
                            borderpen.Width = 2;
                            borderpen.Color = Color.FromArgb(150, bordercolor);
                            if (orientation == Orientation.Horizontal)
                            {
                                g.DrawLine(borderpen, mrgnrec.X, mrgnrec.Height / 2 + mrgnrec.Y, mrgnrec.X + mrgnrec.Width, mrgnrec.Height / 2 + mrgnrec.Y);
                                borderpen.DashStyle = DashStyle.Solid;
                                //     g.DrawLine(borderpen, mrgnrec.X + mrgnrec.Width / 2 - 0.5F, mrgnrec.Y + mrgnrec.Height / 2 - 5, mrgnrec.X + mrgnrec.Width / 2 - 0.5F, mrgnrec.Y + mrgnrec.Height / 2 + 5);
                            }

                            else
                            {
                                g.DrawLine(borderpen, mrgnrec.Width / 2 + mrgnrec.X, mrgnrec.Y, mrgnrec.Width / 2 + mrgnrec.X, mrgnrec.Y + mrgnrec.Height);
                                borderpen.DashStyle = DashStyle.Solid;
                                g.DrawLine(borderpen, mrgnrec.X + mrgnrec.Width / 2 - 5, mrgnrec.Y + mrgnrec.Height / 2 - 0.5F, mrgnrec.X + mrgnrec.Width / 2 + 5, mrgnrec.Y + mrgnrec.Height / 2 - 0.5F);
                            }

                        }
                        #endregion
                    }


                    #region draw text=====================================================================================
                    if (showtext)
                    {
                        StringFormat tsf = new StringFormat();
                        RectangleF dtextrec = textrec;
                        if (autosizetext == false)
                        {
                            dtextrec.Size = TextSize;
                            if (this.orientation==Orientation.Horizontal)
                            { dtextrec.Height = textrec.Height; }
                            else{
                                dtextrec.Width=textrec.Width;
                            }
                            tsf.Alignment = textalgiment;
                            tsf.LineAlignment = StringAlignment.Center;                        
                            tsf.Trimming = StringTrimming.None;
                            tsf.FormatFlags = StringFormatFlags.NoClip;

                        }

                        g.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), dtextrec, tsf);
                        sf.Alignment = StringAlignment.Center;
                    }

                    #endregion

                    #region drawvalues ======================================================================
                    PointF stepedtextloc = new PointF();

                    for (float i = minimum; i <= maxmum; i += valuestextstep)
                    {
                        if (drawvalues == false && showmediumline == false)
                        { break; } if (i.ToString().Length > 7)
                        {
                            i = EquationSolver.NumricE(i);
                        }
                        if (orientation == Orientation.Horizontal)
                        {
                            borderpen.Width = 2;
                            stepedtextloc = new PointF(ksclconvert(false, i), intvaluesposition * AreaRec.Height);
                            SizeF tsize = g.MeasureString(i.ToString(), valuestextfont);
                            if (showmediumline)
                            {
                                g.DrawLine(borderpen, stepedtextloc.X, stepedtextloc.Y - 5, stepedtextloc.X, stepedtextloc.Y + 5);
                                stepedtextloc.Y += tsize.Height;
                            }

                            if (drawvalues)
                            {


                                stepedtextloc.Y -= tsize.Height / 2;
                                stepedtextloc.X -= tsize.Width / 2;

                                if (i == minimum)
                                { stepedtextloc.X = mrgnrec.X; }

                                g.DrawString(fnc.cutfloat(i, floatplaces).ToString(), valuestextfont, new SolidBrush(ForeColor), stepedtextloc);

                            }
                        }
                        else
                        {
                            stepedtextloc = new PointF(mrgnrec.Width / 2, ksclconvert(false, i));
                            stepedtextloc.X -= g.MeasureString(i.ToString(), valuestextfont).Width / 2;
                            if (i == Maxmum)
                            { stepedtextloc.Y -= g.MeasureString(i.ToString(), valuestextfont).Height / 2; }
                            g.DrawString(i.ToString(), valuestextfont, new SolidBrush(ForeColor), stepedtextloc);
                            g.DrawLine(borderpen, stepedtextloc.X - 2, stepedtextloc.Y, stepedtextloc.X + 2, stepedtextloc.Y);

                        }
                    }

                    #endregion

                    #region add pointer rectangle space ====================================================================
                    if (addrec && allowaddpointers)
                    {
                        if (orientation == Orientation.Horizontal)
                        {
                            { g.DrawRectangle(new Pen(Color.Black), mrgnrec.X, mrgnrec.Y, 8, mrgnrec.Height); }

                        }
                        else
                        { g.DrawRectangle(new Pen(Color.Black), mrgnrec.X, mrgnrec.Y, mrgnrec.Width, 8); }
                    }
                    #endregion

                    wallimg = (Bitmap)b.Clone();
                
            }
   

            #region selection rec layout

            else if (textmode==false)
            {
                g.DrawImage(wallimg, 0, 0, Width, Height);
            }


            GraphicsPath gp = new GraphicsPath();
            GraphicsPath areapath = g.GenerateRoundedRectangle(mrgnrec, roundamount, rectedgefilter);

            selctionrec = mrgnrec;
            outerselctionrec = new RectangleF();


            if (textmode==false)
            {
            switch (fillingmode)
            {
                case KscaleFilledArea.Normal:
                    if (orientation == Orientation.Horizontal)
                    {
                        selctionrec.Width = this.SelectedPointer.Rec.X - AreaRec.X + ((float)Moverwidth * (fnc.divdec((Value - minimum), (maxmum - minimum))));
                    }
                    else
                    {
                        selctionrec.Height = this.SelectedPointer.Rec.Y - AreaRec.Y + ((float)moverheight * (fnc.divdec((Value - minimum), (maxmum - minimum))));
                    }

                    break;
                case KscaleFilledArea.inner:
                case KscaleFilledArea.outer:

                    makemaexdsync(false);

                    float x1 = 0;
                    float x2 = 0;
                    float x3 = 0;
                    for (int i = 0; i < clnds.Length; i++)
                    {


                        if (clnds[i].P != null)
                        {

                            if (clnds[i].P.Index == SelectedIndex)
                            {
                                x2 = ksclconvert(false, clnds[i].F);

                                if (i != 0)
                                {
                                    x1 = ksclconvert(false, clnds[i - 1].F);
                                }
                                else
                                {
                                    x1 = ksclconvert(false, minimum);
                                }
                                if (i + 1 < clnds.Length)
                                {
                                    x3 = ksclconvert(false, clnds[i + 1].F);

                                }
                                else
                                {
                                    x2 = ksclconvert(false, clnds[i - 1].F);
                                    x3 = ksclconvert(false, clnds[i].F);

                                }
                                if (i + 1 < clnds.Length)
                                {
                                    if (clnds[i + 1].P == null)
                                    {
                                        if (i - 1 >= 0 && i - 1 < clnds.Length)
                                        {
                                            x2 = ksclconvert(false, clnds[i - 1].F);
                                        }
                                        else
                                        { x2 = ksclconvert(false, minimum); }
                                        x3 = ksclconvert(false, clnds[i].F);

                                    }

                                }
                                break;
                            }
                        }
                    }
                    if (fillingmode == KscaleFilledArea.inner)
                    {
                        if (orientation == Orientation.Horizontal)
                        {
                            selctionrec.X = x2;
                            selctionrec.Width = x3 - x2 + ((float)Moverwidth * (fnc.divdec((maxmum - minimum), (maxmum - minimum))));
                        }
                        else
                        {
                            selctionrec.Y = x2;
                            selctionrec.Height = x3 - x2 + ((float)Moverwidth * (fnc.divdec((maxmum - minimum), (maxmum - minimum))));

                        }
                    }
                    else if (fillingmode == KscaleFilledArea.outer)
                    {
                        {
                            if (orientation == Orientation.Horizontal)
                            {
                                selctionrec.X = ksclconvert(false, minimum);
                                selctionrec.Width = x2 - selctionrec.X;
                                outerselctionrec = mrgnrec;
                                outerselctionrec.X = x3;
                                outerselctionrec.Width = ksclconvert(false, maxmum) - outerselctionrec.X + ((float)Moverwidth * (fnc.divdec((maxmum - minimum), (maxmum - minimum))));
                            }
                            else
                            {
                                selctionrec.Y = ksclconvert(false, minimum);
                                selctionrec.Height = x2 - selctionrec.Y;
                                outerselctionrec = mrgnrec;
                                outerselctionrec.Y = x3;
                                outerselctionrec.Height = ksclconvert(false, maxmum) - outerselctionrec.Y + ((float)moverheight * (fnc.divdec((maxmum - minimum), (maxmum - minimum))));

                            }
                        }
                    }
                    break;
            }}
            #endregion
           
                #region      Fill section area  =====================================================================================

                if (fillselectedarea)
                {
                    g.SetClip(areapath);
                    int ca = (int)(filledareatransparency * 255F);
                    if (selctionrec.Width > 0 && selctionrec.Height > 0)
                    {
                        if (filledareapainting == FilledAreaPainting.FilledAreaColorproperty)
                        {
                            g.FillRoundedRectangle(new SolidBrush(Color.FromArgb(ca, filledareacolor)), selctionrec, 0, RectangeEdgeFilter.Noun());
                        }
                        else
                        {
                            HatchBrush hb = new HatchBrush(pattrentype, Color.FromArgb(ca, FilledAreaColor), Color.FromArgb(ca, selectedcolor));
                            g.FillRoundedRectangle(hb, selctionrec, roundamount, RectangeEdgeFilter.Noun());

                        }

                    }

                    if (fillingmode == KscaleFilledArea.outer)
                    {
                        if (outerselctionrec.Width > 0 && outerselctionrec.Height > 0)
                        {
                            if (filledareapainting == FilledAreaPainting.FilledAreaColorproperty)
                            {
                                g.FillRoundedRectangle(new SolidBrush(Color.FromArgb(ca, filledareacolor)), outerselctionrec, 0, RectangeEdgeFilter.Noun());
                            }
                            else
                            {
                                HatchBrush hb = new HatchBrush(pattrentype, Color.FromArgb(ca, FilledAreaColor), Color.FromArgb(ca, selectedcolor));
                                g.FillRoundedRectangle(hb, outerselctionrec, roundamount, RectangeEdgeFilter.Noun());

                            }
                        }
                    }
                    g.ResetClip();
                }





                #endregion
           
            if (textmode)
            {
                Font valuefont = this.Font;
                if (AlwaysLargeValueText == true || IsOnValueChanging)
                {
                    valuefont = new Font(valuefont.FontFamily, valuefont.Size + 4, FontStyle.Bold);
                }
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), mrgnrec);
                
                g.DrawString(edittext,valuefont,new SolidBrush(ForeColor),mrgnrec);
            }
            #region main borders    =====================================================================================
            if (showborder)
            {
                g.DrawRoundedRectangle(new Pen(bordercolor, 2), mrgnrec.X + 1.0F, mrgnrec.Y + 1, mrgnrec.Width - 2.0F, mrgnrec.Height - 2, roundamount, Rectangleedegfilter);
            }

            #endregion

    if (textmode==false)
    { 
         
            #region  Pointers ==========================================================================================
            RectangleF moverrec = new RectangleF();
            PointF[] pp = new PointF[3];

            foreach (pointer p in pointers)
            {

                PointF[] polgnps = new PointF[5];

                moverrec = p.Rec;
                if (p.MoverAlgiment == StringAlignment.Near)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        moverrec.Height = moverheight;
                        moverrec.Y = (int)(AreaRec.Bottom - moverrec.Height);
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.X, moverrec.Bottom);
                            pp[1] = new PointF(moverrec.X + moverwidth / 2, moverrec.Y);
                            pp[2] = new PointF(moverrec.X + moverwidth, moverrec.Bottom);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.X, moverrec.Bottom);
                            polgnps[1] = new PointF(moverrec.X, moverrec.Y + 8);
                            polgnps[2] = new PointF(moverrec.X + moverwidth / 2, moverrec.Y);
                            polgnps[3] = new PointF(moverrec.X + moverwidth, moverrec.Y + 8);
                            polgnps[4] = new PointF(moverrec.X + moverwidth, moverrec.Bottom);


                        }
                    }
                    else
                    {
                        moverrec.Width = moverwidth;
                        moverrec.X = (int)(AreaRec.Right - moverrec.Width);
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.Right, moverrec.Y);
                            pp[1] = new PointF(moverrec.X, moverrec.Y + moverrec.Height / 2);
                            pp[2] = new PointF(moverrec.Right, moverrec.Bottom);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.Right, moverrec.Y);
                            polgnps[1] = new PointF(moverrec.X + 8, moverrec.Y);
                            polgnps[2] = new PointF(moverrec.X, moverrec.Y + moverheight / 2);
                            polgnps[3] = new PointF(moverrec.X + 8, moverrec.Y + moverheight);
                            polgnps[4] = new PointF(moverrec.Right, moverrec.Y + moverheight);


                        }
                    }


                }
                else if (p.MoverAlgiment == StringAlignment.Far)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        moverrec.Height = moverheight;
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.X, moverrec.Y);
                            pp[1] = new PointF(moverrec.X + moverrec.Width / 2, moverrec.Height);
                            pp[2] = new PointF(moverrec.Right, moverrec.Y);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.X, moverrec.Y);
                            polgnps[1] = new PointF(moverrec.X, moverrec.Y + moverheight - 8);
                            polgnps[2] = new PointF(moverrec.X + moverwidth / 2, moverrec.Y + moverheight);
                            polgnps[3] = new PointF(moverrec.X + moverwidth, moverrec.Y + moverheight - 8);
                            polgnps[4] = new PointF(moverrec.X + moverwidth, moverrec.Y);


                        }
                    }
                    else
                    {
                        moverrec.Width = moverwidth;
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.X, moverrec.Y);
                            pp[1] = new PointF(moverrec.Width, moverrec.Y + moverrec.Height / 2);
                            pp[2] = new PointF(moverrec.X, moverrec.Bottom);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.X, moverrec.Y);
                            polgnps[1] = new PointF(moverrec.X + moverwidth - 8, moverrec.Y);
                            polgnps[2] = new PointF(moverrec.X + moverwidth, moverrec.Y + moverheight / 2);
                            polgnps[3] = new PointF(moverrec.X + moverwidth - 8, moverrec.Y + moverheight);
                            polgnps[4] = new PointF(moverrec.X, moverrec.Y + moverheight);


                        }
                    }

                }
                else if (p.MoverAlgiment == StringAlignment.Center)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.X, moverrec.Bottom);
                            pp[1] = new PointF(moverrec.X + moverwidth / 2, moverrec.Y);
                            pp[2] = new PointF(moverrec.X + moverwidth, moverrec.Bottom);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.X, moverrec.Bottom);
                            polgnps[1] = new PointF(moverrec.X, moverrec.Y + 8);
                            polgnps[2] = new PointF(moverrec.X + moverrec.Width / 2, moverrec.Y);
                            polgnps[3] = new PointF(moverrec.X + moverrec.Width, moverrec.Y + 8);
                            polgnps[4] = new PointF(moverrec.X + moverrec.Width, moverrec.Bottom);


                        }
                    }
                    else
                    {
                        if (movershape == KscaleMoverShape.Trangle)
                        {
                            pp[0] = new PointF(moverrec.Right, moverrec.Y);
                            pp[1] = new PointF(moverrec.X, moverrec.Y + moverrec.Height / 2);
                            pp[2] = new PointF(moverrec.Right, moverrec.Bottom);
                        }
                        else if (movershape == KscaleMoverShape.Polygon)
                        {
                            polgnps[0] = new PointF(moverrec.Right, moverrec.Y);
                            polgnps[1] = new PointF(moverrec.X + 8, moverrec.Y);
                            polgnps[2] = new PointF(moverrec.X, moverrec.Y + moverrec.Height / 2);
                            polgnps[3] = new PointF(moverrec.X + 8, moverrec.Y + moverrec.Height);
                            polgnps[4] = new PointF(moverrec.Right, moverrec.Y + moverrec.Height);


                        }
                    }

                }
                if (p.FillMover)
                {
                    Color bas = movercolor;
                    movercolor = p.Movercolor;
                    if (p.Index == overpointer)
                    {
                        movercolor = ControlPaint.Dark(p.Movercolor, 0.2f);
                    }
                    Brush mbb = new SolidBrush(movercolor);
                    if (p.Image != null)
                    {
                        g.DrawImage(p.Image, fnc.OImg(p.Image, Rectangle.Round(moverrec), ImageLayout.Stretch));
                    }
                    else if (p.MoverShape == KscaleMoverShape.Rect)
                    {
                        gp.AddRectangle(moverrec);
                        g.FillRoundedRectangle(mbb, moverrec, roundamount, rectedgefilter);



                    }
                    else if (p.MoverShape == KscaleMoverShape.Trangle)
                    {

                        gp.AddLines(pp);
                        g.FillPolygon(mbb, pp);
                    }
                    else if (p.MoverShape == KscaleMoverShape.Polygon)
                    {
                        gp.AddLines(polgnps);
                        g.FillPolygon(mbb, polgnps);
                    }
                    else
                    {
                        gp.AddEllipse(moverrec);
                        g.FillEllipse(mbb, moverrec);

                    }


                    if (p.Index == this.SelectedIndex && pointers.Count > 1)
                    {
                        g.SetClip(gp);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)), Rectangle.Round(new RectangleF(p.X, p.Rec.Height - 8, p.Rec.Width, 8)));
                        g.ResetClip();

                    }

                    movercolor = bas;
                }
                #region    mover border=====================================================================================
                if (p.ShowBorder)
                {
                    if (p.MoverShape == KscaleMoverShape.Rect)
                    {
                        g.DrawRoundedRectangle(new Pen(bordercolor, 2), moverrec.X + 1, moverrec.Y + 1, moverrec.Width - 2, moverrec.Height - 2, roundamount, Rectangleedegfilter);

                    }
                    else if (p.MoverShape == KscaleMoverShape.Trangle)
                    {

                        g.DrawPolygon(new Pen(bordercolor, 2), pp);
                    }
                    else if (p.MoverShape == KscaleMoverShape.Polygon)
                    {
                        g.DrawPolygon(new Pen(bordercolor, 2), polgnps);
                    }
                    else
                    {
                        g.DrawEllipse(new Pen(bordercolor, 2), moverrec.X + 1, moverrec.Y + 1, moverrec.Width - 2, moverrec.Height - 2);

                    }
                }
                #endregion

            }
            #endregion
         #region    draw value=========================================================================================
            if (showvalue)
            {
                string valuetodraw = "";
                if (showtextvalues == false)
                {
                    if (pointers.Count == 2)
                    {
                
                        valuetodraw = fnc.cutfloat(MinValue,floatplaces) + "," + fnc.cutfloat(MaxValue,floatplaces);
                    
                    }
                    else
                    {
                        for (int i = 0; i < pointers.Count; i++)
                        {
                          
                            if (i != 0)
                            {
                                valuetodraw += ",";
                            }
                            valuetodraw += fnc.cutfloat(this.pointers[i].ValueF, floatplaces);
                        }
                    }
                }
                else
                {

                    int indx = Valuestextposition.ToList().IndexOf(this.Value);
                    if (indx >= 0 && indx < valuestexts.Length)
                    {
                        valuetodraw = valuestexts[indx];
                    }
                }
                Font valuefont = this.Font;
                if (AlwaysLargeValueText == true || IsOnValueChanging)
                {
                    valuefont = new Font(valuefont.FontFamily, valuefont.Size + 4, FontStyle.Bold);
                }


                stringvaluerec.Size = g.MeasureString(valuetodraw, valuefont);

                if (orientation == Orientation.Horizontal)
                {

                    stringvaluerec.X = fnc.divdec(mrgnrec.Width, 2) - fnc.divdec(stringvaluerec.Width, 2) + mrgnrec.X;
                    stringvaluerec.Y = fnc.divdec(mrgnrec.Height, 2) - fnc.divdec(stringvaluerec.Height, 2) + mrgnrec.Y;

                }
                else
                {
                    stringvaluerec.Y = mrgnrec.Height / 2 - stringvaluerec.Height / 2 + mrgnrec.Y;
                    stringvaluerec.X = fnc.divdec(mrgnrec.Width, 2) - fnc.divdec(stringvaluerec.Width, 2) + mrgnrec.X;
                }
           //     if (AlwaysLargeValueText == true || IsOnValueChanging)
                {
                    Rectangle intstringvaluerec = Rectangle.Round(stringvaluerec);
                   g.DrawRectangle(new Pen(Color.Black), stringvaluerec.X,stringvaluerec.Y,stringvaluerec.Width,stringvaluerec.Height);
                  //  intstringvaluerec.Inflate(-1, -1);

//                    g.DrawRectangle(new Pen(Color.White), intstringvaluerec);
              //      intstringvaluerec.Inflate(-1, -1);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(125, ValueTextBackColor)), stringvaluerec);

                }
                   
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                g.DrawString(valuetodraw, valuefont, new SolidBrush(ForeColor), stringvaluerec, sf);


            }

            #endregion
        }


                #endregion

            #region not enabled effect
    {
        g.Dispose();
                if (!Enabled)
                {
                    GraphicsPath gpe = g.GenerateRoundedRectangle(mrgnrec, roundamount, rectedgefilter);
                    for (int x = (int)mrgnrec.X; x < mrgnrec.Right; x++)
                    {
                        for (int y = (int)mrgnrec.Y; y < mrgnrec.Bottom; y++)
                        {
                            Color c = b.GetPixel(x, y);
                        if (gpe.IsVisible(x,y))
                            {
                                int m = (int)((c.R + c.G +c.B) /4f);
                      b.SetPixel(x, y, Color.FromArgb(m, m, m));
                            }
                        }
                    }

                }
   
            }
            #endregion

            if (mous != mus.none)
            { movercolor = mov; smoothcolor = c1; maincolor = c2; bordercolor = brdr; }
            e.Graphics.DrawImage(b, 0, 0); ;


            b.Dispose(); 

        }
        #endregion
    
      
    }

    #endregion

    #region Picker2D

    public delegate void ColorPickereventhandler(ColorPicker sender, ColorPickereventargs e);
    public class ColorPickereventargs : System.EventArgs
    {
        public ColorPickereventargs()
        { }
        public ColorPickereventargs(Point Value)
        { this.Value = Value; }
        public ColorPickereventargs(Point Value, Color selctedcolor)
        {
            this.Value = Value;
            this.SelectedColor = selctedcolor;
        }
        public Color SelectedColor { get; set; }
        public Point Value { get; set; }
    }
    public enum GraphicsVetors { IdentityColorinfour, Identitycolorin3, HSV_Ciclr, Maxedcolors, Blending, HSV_Rectangle, None }

    public class duoblescaledesginer : System.Windows.Forms.Design.ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // Record instance of control we're designing
            this.ths.Size = new Size(200, 80);
            // Hook up events
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));
            //  IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            //   s.SelectionChanged += new EventHandler(OnSelectionChanged);
            //
            //      this.OnAddButton(null, null);
            //      this.OnAddButton(null, null);
            //   MessageBox.Show(ths.Controls.Count.ToString());
            //   c.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
        }
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            if (ths.Controls.Count < 1)
            {
                //         OnAddButton(null, null); OnAddButton(null, null);
                //    ths.scalex.Pointers.Add(new pointer());
                //     ths.scalex.SelectedIndex = 0;
                //     ths.scaley.Pointers.Add(new pointer());
                //      ths.scaley.SelectedIndex = 0;
            }
        }
        private void OnSelectionChanged(object sender, System.EventArgs e)
        {
            ths.OnSelectionChanged();
        }


        protected override bool GetHitTest(System.Drawing.Point point)
        {
            Rectangle wrct;
            point = ths.PointToClient(point);
            for (int i = 0; i < 2; i++)
            {
                Control c = ths.Controls[i];
                wrct = new Rectangle(c.Location, c.Size);
                // 
                if (wrct.Contains(point))
                { return false; }
            }

            return false;
        }
        public void OnAddButton(object sender, System.EventArgs e)
        {
            kscale s;
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction dt;
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Add a new button to the collection
            dt = h.CreateTransaction("Add scle");
            s = (kscale)h.CreateComponent(typeof(kscale));
            c.OnComponentChanging(ths, null);


            ths.Controls.Add(s);

            c.OnComponentChanged(ths, null, null, null);
            dt.Commit();
        }



        private DesignerVerbCollection _verbs;
        private DesignerActionListCollection _action;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_action == null)
                {
                    _action = new DesignerActionListCollection();
                    //      _action.Add(new kscaledesgineractionlist((kscale)Control, this));
                }
                return _action;
            }
        }
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection();

                    //   _changeoritaion = new DesignerVerb("Change Oritation", this.Changeoritation);

                    //    _verbs.Add(_changeoritaion);

                }
                return _verbs;
            }
        }
        private ColorPicker ths
        {
            get
            {
                return (ColorPicker)this.Control;
            }
        }
        public duoblescaledesginer()
        { }

    }
    public class ColorHandler
    {


        public struct HSV
        {
            // All values are between 0 and 255.
            public int Hue;
            public int Saturation;
            public int value;

            public HSV(int H, int S, int V)
            {
                Hue = H;
                Saturation = S;
                value = V;
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2})", Hue, Saturation, value);
            }
        }

        public static Color HSVtoRGB(int H, int S, int V)
        {
            // H, S, and V must all be between 0 and 255.
            return HSVtoRGB(new HSV(H, S, V));
        }
        public static Color HSVtoRGB(HSV HSV)
        {
            // HSV contains values scaled as in the color wheel:
            // that is, all from 0 to 255. 

            // for ( this code to work, HSV.Hue needs
            // to be scaled from 0 to 360 (it//s the angle of the selected
            // point within the circle). HSV.Saturation and HSV.value must be 
            // scaled to be between 0 and 1.

            double h;
            double s;
            double v;

            double r = 0;
            double g = 0;
            double b = 0;

            // Scale Hue to be between 0 and 360. Saturation
            // and value scale to be between 0 and 1.
            h = ((double)HSV.Hue / 255 * 360) % 360;
            s = (double)HSV.Saturation / 255;
            v = (double)HSV.value / 255;

            if (s == 0)
            {
                // If s is 0, all colors are the same.
                // This is some flavor of gray.
                r = v;
                g = v;
                b = v;
            }
            else
            {
                double p;
                double q;
                double t;

                double fractionalSector;
                int sectorNumber;
                double sectorPos;

                // The color wheel consists of 6 sectors.
                // Figure out which sector you//re in.
                sectorPos = h / 60;
                sectorNumber = (int)(Math.Floor(sectorPos));

                // get the fractional part of the sector.
                // That is, how many degrees into the sector
                // are you?
                fractionalSector = sectorPos - sectorNumber;

                // Calculate values for the three axes
                // of the color. 
                p = v * (1 - s);
                q = v * (1 - (s * fractionalSector));
                t = v * (1 - (s * (1 - fractionalSector)));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
            // return an RGB structure, with values scaled
            // to be between 0 and 255.
            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static HSV RGBtoHSV(Color RGB)
        {
            // In this function, R, G, and B values must be scaled 
            // to be between 0 and 1.
            // HSV.Hue will be a value between 0 and 360, and 
            // HSV.Saturation and value are between 0 and 1.
            // The code must scale these to be between 0 and 255 for
            // the purposes of this application.

            double min;
            double max;
            double delta;

            double r = (double)RGB.R / 255;
            double g = (double)RGB.G / 255;
            double b = (double)RGB.B / 255;

            double h;
            double s;
            double v;

            min = Math.Min(Math.Min(r, g), b);
            max = Math.Max(Math.Max(r, g), b);
            v = max;
            delta = max - min;
            if (max == 0 || delta == 0)
            {
                // R, G, and B must be 0, or all the same.
                // In this case, S is 0, and H is undefined.
                // Using H = 0 is as good as any...
                s = 0;
                h = 0;
            }
            else
            {
                s = delta / max;
                if (r == max)
                {
                    // Between Yellow and Magenta
                    h = (g - b) / delta;
                }
                else if (g == max)
                {
                    // Between Cyan and Yellow
                    h = 2 + (b - r) / delta;
                }
                else
                {
                    // Between Magenta and Cyan
                    h = 4 + (r - g) / delta;
                }

            }
            // Scale h to be between 0 and 360. 
            // This may require adding 360, if the value
            // is negative.
            h *= 60;
            if (h < 0)
            {
                h += 360;
            }

            // Scale to the requirements of this 
            // application. All values are between 0 and 255.
            return new HSV((int)(h / 360 * 255), (int)(s * 255), (int)(v * 255));
        }
    }


    [DefaultEvent("ValueChanging"), DesignerAttribute(typeof(duoblescaledesginer))]
    public class ColorPicker : ContainerControl, ISupportInitialize
    {
        #region new events
        /// <summary>
        /// Occurs when value has been changed
        /// </summary>
        public event ColorPickereventhandler ValueChanged;
        /// <summary>
        /// Occurs while value is changing
        /// </summary>
        public event ColorPickereventhandler ValueChanging;
        #endregion
        private const int COLOR_COUNT = 6 * 256;
 
        #region voids
        private int centerhslcirclelight =255;
        [DefaultValue(255)]
        public int Centerhslcirclelight
        {
            get { return centerhslcirclelight; }
            set { centerhslcirclelight = value; updatebackgrphic(); Invalidate(); }
        }
        private void CalcCoordsAndUpdate(ColorHandler.HSV HSV)
        {
            // Convert color to real-world coordinates and then calculate
            // the various points. HSV.Hue represents the degrees (0 to 360), 
            // HSV.Saturation represents the radius. 
            // This procedure doesn't draw anything--it simply 
            // updates class-level variables. The UpdateDisplay
            // procedure uses these values to update the screen.

            // Given the angle (HSV.Hue), and distance from 
            // the center (HSV.Saturation), and the center, 
            // calculate the point corresponding to 
            // the selected color, on the color wheel.
            Point colorPoint = GetPoint((double)HSV.Hue / 255 * 360,
                  (float)HSV.Saturation / 255 * arearec.Width / 2,
                  arearec.Size);
            this.Value =Point.Round( converttovalue(colorPoint));


            //  selectedColor = ColorHandler.HSVtoColor(HSV);
            //
        }
        private Bitmap CreateGradient(Size colorsize)
        {
            // Create a new PathGradientBrush, supplying
            // an array of points created by calling
            // the GetPoints method.
            using (PathGradientBrush pgb =
                new PathGradientBrush(GetPoints(colorsize, true)))
            {
                // Set the various properties. Note the SurroundColors
                // property, which contains an array of points, 
                // in a one-to-one relationship with the points
                // that created the gradient.
                pgb.CenterColor = Color.FromArgb(centerhslcirclelight, centerhslcirclelight, centerhslcirclelight);
                pgb.CenterPoint = new PointF(colorsize.Width / 2, colorsize.Height / 2);
                pgb.SurroundColors = GetColors();
                // Create a new bitmap containing
                // the color wheel gradient, so the 
                // code only needs to do all this 
                // work once. Later code uses the bitmap
                // rather than recreating the gradient.
                Bitmap colorImage = new Bitmap(colorsize.Width, colorsize.Height);



                using (Graphics ng =
                                 Graphics.FromImage(colorImage))
                {

                    ng.FillEllipse(pgb, 0, 0,
                        colorsize.Width, colorsize.Height);
                }
                return colorImage;
            }
        }

        private Color[] GetColors()
        {
            // Create an array of COLOR_COUNT
            // colors, looping through all the 
            // hues between 0 and 255, broken
            // into COLOR_COUNT intervals. HSV is
            // particularly well-suited for this, 
            // because the only value that changes
            // as you create colors is the Hue.
            Color[] Colors = new Color[COLOR_COUNT];
     
            for (int i = 0; i <= COLOR_COUNT - 1; i++)
                Colors[i] = ColorHandler.HSVtoRGB((int)((double)(i * 255) / COLOR_COUNT), 255, centerhslcirclelight);
            return Colors;
        }

        private Point[] GetPoints(Size sz, bool f)
        {
            // Generate the array of points that describe
            // the locations of the COLOR_COUNT colors to be 
            // displayed on the color wheel.

            Point[] Points = new Point[COLOR_COUNT];

            for (int i = 0; i <= COLOR_COUNT - 1; i++)
                Points[i] = GetPoint((double)(i * 360) / COLOR_COUNT, sz, f);
            return Points;
        }

        private Point GetPoint(double degrees, Size sz, bool f)
        {
            if (f)
            {
                double radians = fnc.Topi((float)degrees);

                Point centerpoint = new Point(sz.Width / 2, sz.Height / 2);

                return new Point((int)(centerpoint.X + (sz.Width / 2 * Math.Cos(radians))),
                    (int)(centerpoint.Y - (sz.Height / 2 * Math.Sin(radians))));
            }
            else
            {
                return new Point((int)((sz.Width * degrees)),
                        (int)((sz.Height / 2)));

            }
        }

        private Point GetPoint(double degrees, float rad, Size sz)
        {

            double radians = fnc.Topi((float)degrees);

            Point centerpoint = new Point(sz.Width / 2, sz.Height / 2);

            return new Point((int)(centerpoint.X + (rad * Math.Cos(radians))),
                (int)(centerpoint.Y - (rad * Math.Sin(radians))));

        }

        public void BeginInit()
        {


        }
        public void EndInit()
        {

        }

        public Point[] updatevalues()
        {
            int cn = Math.Max(scalex.Pointers.Count, scaley.Pointers.Count);

            Point[] v = new Point[cn];
            for (int i = 0; i < cn; i++)
            {
                if (scalex.Pointers.Count - 1 >= i)
                { v[i].X = scalex.Pointers[i].Value; }

                if (scaley.Pointers.Count - 1 >= i)
                { v[i].Y = scaley.Pointers[i].Value; }


            }
            values = v;
            return v;
        }

        void updatelayoutrectangle()
        {
            int rxh = 0; int ryw = 0;
            if (showscales)
            {
                rxh = scalex.Height;
                ryw = scaley.Width;
            }
            int rw = Width;
            if (Width <= 0)
                rw = 100;
            int rh = Height;
            if (Height <= 0)
                rh = 100;
            arearec = new Rectangle(0, 0, rw - ryw, rh - rxh);



        }

        void updatebackgrphic()
        {
            updatelayoutrectangle();
            if (arearec.Width == 0)
                arearec.Width = 1;
            if (arearec.Height == 0)
                arearec.Height = 1;
            Rectangle s = arearec;
            if (highgraohics == false)
            { s = new Rectangle(0, 0, 80, 80); }
            backb = new Bitmap(s.Width, s.Height);

            Graphics g = Graphics.FromImage(backb);
            Brush mb = new SolidBrush(BackColor);
            GraphicsPath gb = new GraphicsPath();

            gb.AddRectangle(s);
            PathGradientBrush pgb = new PathGradientBrush(gb);


            switch (Graphicvector)
            {
                case GraphicsVetors.IdentityColorinfour:
                    pgb.CenterColor = enterdcolor;
                    pgb.SurroundColors = new Color[] { Color.Black, Color.Red, Color.Blue, Color.White };
                    mb = pgb; g.FillRectangle(mb, arearec);

                    break;
                case GraphicsVetors.Maxedcolors:

                    ColorBlend cb = new ColorBlend(7);
                    cb.Colors = new Color[] { Color.Orange, Color.Yellow, Color.Green, Color.Cyan, Color.Blue, Color.Fuchsia, Color.Red };
                    cb.Positions = new float[] { 0f, 0.16f, 0.33f, 0.5f, 0.66f, 0.83f, 1.0f };
                    pgb.InterpolationColors = cb;

                    mb = pgb; g.FillRectangle(mb, arearec);
                    break;
                case GraphicsVetors.Identitycolorin3:
                    ColorBlend cb2 = new ColorBlend(3);
                    cb2.Colors = new Color[] { Color.White, enterdcolor, Color.Black };
                    cb2.Positions = new float[] { 0f, .5f, 1.0f };
                    LinearGradientBrush lgb = new LinearGradientBrush(s, Color.Red, Color.Red, 45);
                    lgb.InterpolationColors = cb2;
                    mb = lgb; g.FillRectangle(mb, arearec);
                    break;
                case GraphicsVetors.Blending:
                    try
                    {
                        Blend blnd = new Blend();
                        blndpositons = new float[Values.Length];
                        blndfactors = new float[Values.Length];
                        clnd[] clnd1 = new clnd[Values.Length];
                        for (int i = 0; i < Values.Length; i++)
                        {
                            float rf = 100;
                            float r1 = Values[i].X;

                            float rs1 = r1 / rf;
                            float r2 = Values[i].Y;
                            float rs2 = r2 / rf;


                            clnd1[i] = new clnd(rs1, rs2);
                        }
                        Array.Sort(clnd1);
                        for (int i = 0; i < clnd1.Length; i++)
                        {
                            blndpositons[i] = clnd1[i].F;
                            blndfactors[i] = clnd1[i].F2;
                        }
                        blndpositons[blndpositons.Length - 1] = 1;
                        blndpositons[0] = 0;
                        blnd.Positions = blndpositons;
                        blnd.Factors = blndfactors;



                        LinearGradientBrush lgb2 = new LinearGradientBrush(s, enterdcolor, entercolor2, 0f);
                        lgb2.Blend = blnd;
                        mb = lgb2; g.FillRectangle(mb, arearec);
                    }

                    catch
                    { }

                    break;
                case GraphicsVetors.HSV_Ciclr:
                    backb = (Bitmap)CreateGradient(new Size(arearec.Width, arearec.Height)).Clone();
                    break;
                case GraphicsVetors.HSV_Rectangle:

                    const int width = 360 + 1;
                    const int height = 100 + 1;

                    Bitmap bmp = new Bitmap(width, height);

                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            double h = x;
                            double ss = 100 - y;
                            double l = 100 - y;

                            Color color = HsbToRgb(new ColorHandler.HSV((int)h, (int)ss, (int)l));

                            bmp.SetPixel(x, y, color);
                        }
                    }

                    backb = (Bitmap)bmp.Clone();
                    bmp.Dispose();
                    break;
                case GraphicsVetors.None:
                    break;



            }
            g.Dispose();
            imgdone = true;
            if (Graphicvector == GraphicsVetors.Blending)
            {// imgdone = false;
            }
        }
        public Color HsbToRgb(
          ColorHandler.HSV hsb)
        {
            double red = 0, green = 0, blue = 0;

            double h = hsb.Hue;
            double s = ((double)hsb.Saturation) / 100;
            double b = ((double)hsb.value) / 100;

            if (s == 0)
            {
                red = b;
                green = b;
                blue = b;
            }
            else
            {
                // the color wheel has six sectors.

                double sectorPosition = h / 60;
                var sectorNumber = (int)Math.Floor(sectorPosition);
                double fractionalSector = sectorPosition - sectorNumber;

                double p = b * (1 - s);
                double q = b * (1 - (s * fractionalSector));
                double t = b * (1 - (s * (1 - fractionalSector)));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        red = b;
                        green = t;
                        blue = p;
                        break;

                    case 1:
                        red = q;
                        green = b;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = b;
                        blue = t;
                        break;

                    case 3:
                        red = p;
                        green = q;
                        blue = b;
                        break;

                    case 4:
                        red = t;
                        green = p;
                        blue = b;
                        break;

                    case 5:
                        red = b;
                        green = p;
                        blue = q;
                        break;
                }
            }

            var nRed = (int)Math.Round(red * 255);
            var nGreen = (int)Math.Round(green * 255);
            var nBlue = (int)Math.Round(blue * 255);

            return Color.FromArgb(nRed, nGreen, nBlue);
        }
        public void SetColor(Color value)
        {
            CalcCoordsAndUpdate(ColorHandler.RGBtoHSV(value));
        }
        public void SetColor(ColorHandler.HSV value)
        {
            CalcCoordsAndUpdate(value);
        }
        PointF converttopoint(int p)
        {

            PointF dvalue = new PointF();

            dvalue.X = (int)(fnc.tint((fnc.divdec(Values[p].X - scalex.Minimum, scalex.Maxmum - scalex.Minimum)) * (arearec.Width)) + arearec.X);
            dvalue.Y = (int)(fnc.tint((fnc.divdec(Values[p].Y - scaley.Minimum, scaley.Maxmum - scaley.Minimum)) * (arearec.Height)) + arearec.Y);
            return dvalue;
        }
        PointF converttopoint(PointF p)
        {

            PointF dvalue = new PointF();

            dvalue.X = (fnc.tint((fnc.divdec(p.X - scalex.Minimum, scalex.Maxmum - scalex.Minimum)) * (arearec.Width)) + arearec.X);
            dvalue.Y = (fnc.tint((fnc.divdec(p.Y - scaley.Minimum, scaley.Maxmum - scaley.Minimum)) * (arearec.Height)) + arearec.Y);
            return dvalue;
        }
        PointF converttovalue(PointF num)
        {
            PointF v = new PointF();
            v.X = fnc.tint((fnc.divdec(num.X - arearec.X, arearec.Width)) * (scalex.Maxmum - scalex.Minimum) + scalex.Minimum);
            if (v.X < scalex.Minimum)
            { v.X = scalex.Minimum; }
            else if (v.X > scalex.Maxmum)
            { v.X = scalex.Maxmum; }
            v.Y = fnc.tint((fnc.divdec(num.Y - arearec.Y, arearec.Height)) * (scaley.Maxmum - scaley.Minimum) + scaley.Minimum);
            if (v.Y < scaley.Minimum)
            { v.Y = scaley.Minimum; }
            else if (v.Y > scaley.Maxmum)
            { v.Y = scaley.Maxmum; }
            return v;
        }
        #region constriction
        public ColorPicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ContainerControl | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);

            scalex = new kscale();
            scaley = new kscale();
            scalex.Pointers.Add(new pointer());
            scalex.SelectedIndex = 0;
            scaley.Pointers.Add(new pointer());
            scaley.SelectedIndex = 0;


            scaley.OrientationDirection = Orientation.Vertical;
            scalex.DefultMoverShape = scaley.DefultMoverShape = KscaleMoverShape.Trangle;
            scalex.DefultMoverAlgiment = scaley.DefultMoverAlgiment = StringAlignment.Near;
            scalex.OnlyIntValue = scaley.OnlyIntValue = false;

            scalex.ValueChanging += new Kscaleeventhandler(kscalevaluechange);
            scaley.ValueChanging += new Kscaleeventhandler(kscalevaluechange);
            scaley.ValueChanged += new Kscaleeventhandler(kscalevaluechange);
            scalex.ValueChanged += new Kscaleeventhandler(kscalevaluechange);
            scalex.PointerAdd += new Kscalepointereventhandler(kscalex_pointeradd);
            scaley.PointerAdd += new Kscalepointereventhandler(kscaley_pointeradd);

            updatevalues();

            this.Controls.Add(scalex);
            this.Controls.Add(scaley);


            this.OnSizeChanged(null);
            this.ScaleSize = new Size(15, 15);
        }
        #endregion
        #endregion

        #region private

        bool isdown = false;
        int slct = -1;
        Rectangle arearec = new Rectangle();
        Bitmap backb = new Bitmap(10, 10);
        bool imgdone = false;

        private class clnd : IComparable
        {

            public int CompareTo(Object other)
            {
                clnd oth = other as clnd;
                int res = 0;

                res = this.F.CompareTo(oth.F);

                return res;
            }

            public clnd(float fl, float f2)
            {
                this.F = fl;
                this.F2 = f2;

            }
            public clnd()
            { }
            float f = 0;

            public float F
            {
                get { return f; }
                set { f = value; }
            }


            float f2 = 0;
            public float F2
            {
                get { return f2; }
                set { f2 = value; }
            }
        }

        #endregion


        public float[] blndpositons = new float[0];
        public float[] blndfactors = new float[0];

        #region on events methods

        private void kscalevaluechange(object s, Kscaleeventargs e)
        {
            Invalidate();
            updatevalues();
            kscale d = s as kscale;
            if (d.IsOnValueChanging)
            {
                onvaluechanging(this, new ColorPickereventargs(new Point(scalex.Value, scaley.Value), SelectedColor));
            }

        }

        public void kscalex_pointeradd(object senser, kscalepointereventargs e)
        {
            if (Scaley.Pointers.Count - 1 < e.Pointer.Index)
            {
                pointer p = new pointer(scaley.Pointers);
                fnc.CopyProperties(p, scaley.Pointers[0], true);
                p.ValueF = 10;
                scaley.Pointers.Add(p);
                scaley.makepointerlocation();
                updatevalues();
                scaley.Invalidate();
            }
        }
        public void kscaley_pointeradd(object senser, kscalepointereventargs e)
        {
            if (scalex.Pointers.Count - 1 < e.Pointer.Index)
            {
                pointer p = new pointer(scalex.Pointers);
                fnc.CopyProperties(p, scalex.Pointers[0], true);

                p.ValueF = 10;
                scalex.Pointers.Add(p);
                scalex.makepointerlocation();
                Update();
                scalex.Invalidate();
            }
        }

        internal void OnSelectionChanged()
        {
            ISelectionService s = (ISelectionService)GetService(typeof(ISelectionService));

            // See if the primary selection is one of our buttons
            foreach (Control p in this.Controls)
            {
                if (s.PrimarySelection == p)
                {

                    s.SetSelectedComponents(new Control[] { this.scalex });
                    break;
                }
            }

            Invalidate();

        }
     
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width < 50)
            {
                Width = 50;
            }
            if (Height < 50)
            {
                Height = 50;
            }

            scaley.Location = new Point(Width - scaley.Width, 0);
            scaley.Size = new Size(scaley.Width, Height - scalex.Height);
            scalex.Location = new Point(0, Height - scalex.Height);
            scalex.Size = new Size(Width - scaley.Width, scalex.Height);

            updatebackgrphic();
            Invalidate();
        }
        #endregion

        #region properties
        #region readonly




        private Point[] values = new Point[0];
        public Point[] Values
        {
            get
            {
                return values;
            }
        }

        public kscale scaley;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public kscale Scaley
        {
            get { return (kscale)this.Controls[1]; }
        }

        public kscale scalex;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public kscale Scalex
        {
            get { return (kscale)this.Controls[0]; }

        }

        Color selectedcolor = Color.Empty;
        public Color SelectedColor
        {
            get { return selectedcolor; }
            set { SetColor(value); Invalidate(); }
        }

        #endregion

        #region FilledArea
        private FilledAreaPainting filledareapainting = FilledAreaPainting.FilledAreaColorproperty;
        [Category("FilledArea"), DefaultValue(FilledAreaPainting.FilledAreaColorproperty)]
        public FilledAreaPainting FilledAreaPainting
        {
            get { return filledareapainting; }
            set { filledareapainting = value; Invalidate(); }
        }

        private HatchStyle pattrentype = HatchStyle.ForwardDiagonal;
        [Category("FilledArea"), DefaultValue(HatchStyle.ForwardDiagonal),
         Description("The filled area painted with this Pattren type with transperncy value of 'FilledAreaTransparency'  property"
          + "\n  Only appear when 'FilledAreaPainting'prperty is set to 'pattrenlayer'")
        ]
        public HatchStyle FilledAreaPattrenType
        {
            get { return pattrentype; }
            set { pattrentype = value; Invalidate(); }
        }
        private Color filledareacolor = Color.Black;
        [Category("FilledArea"), DefaultValue(typeof(Color), "Black")]
        public Color FilledAreaColor
        {
            get { return filledareacolor; }
            set { filledareacolor = value; Invalidate(); }
        }
        private Color forepattrencolor = Color.White;
        [Category("FilledArea"), DefaultValue(typeof(Color), "White")]
        public Color ForePattrenColor
        {
            get { return forepattrencolor; }
            set { forepattrencolor = value; Invalidate(); }
        }
        private float filledareatransparency = 0.5F;
        [Category("FilledArea"), DefaultValue(0.5F)]
        public float FilledAreaTransparency
        {
            get { return filledareatransparency; }
            set
            {
                filledareatransparency = value;
                if (value < 0 || value > 1) { throw new Exception("Value must bp from 0.0 to 1.0"); }
                Invalidate();
            }
        }

        bool fillarea = true;
        [Category("FilledArea"), DefaultValue(true)]
        public bool FillArea
        {
            get { return fillarea; }
            set { fillarea = value; Invalidate(); }

        }
        #endregion
        #region Apearance
        Color enterdcolor = Color.Yellow;
        [Category("Appearance")]
        public Color EnterdColor
        {
            get { return enterdcolor; }
            set
            {
                enterdcolor = value;
                updatebackgrphic();
                Invalidate();
            }
        }
        float selectorwidth = 1;
        [Category("Appearance")]
        public float SelectorWidth
        {
            get { return selectorwidth; }
            set { selectorwidth = value; Invalidate(); }
        }
        Color entercolor2 = Color.Cyan;
        [Category("Appearance")]
        public Color Entercolor2
        {
            get { return entercolor2; }
            set
            {
                entercolor2 = value; updatebackgrphic();
                Invalidate();
            }
        }


        private GraphicsVetors Graphicvector = GraphicsVetors.HSV_Ciclr;
        [Category("Appearance"), DefaultValue(GraphicsVetors.HSV_Ciclr)]

        public GraphicsVetors GraphicVector
        {
            get { return Graphicvector; }
            set { Graphicvector = value; updatebackgrphic(); Invalidate(); }
        }






        bool showscales = true;
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowScales
        {
            get { return showscales; }
            set
            {
                showscales = value;
                scaley.Visible = showscales; scalex.Visible = showscales;
                updatebackgrphic();
                Invalidate();
            }
        }

        bool highgraohics = true;
        [DefaultValue(true), Category("Appearance")]
        public bool HighGraohics
        {
            get { return highgraohics; }
            set { highgraohics = value; updatebackgrphic(); Invalidate(); }
        }


        bool showcolor = true;
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowColor
        {
            get { return showcolor; }
            set { showcolor = value; Invalidate(); }
        }
        #endregion
        #region other
        private Size scalessize = new Size(15, 15);

        [Category("Layout"), DefaultValue(typeof(Size), "15,15")]
        public Size ScaleSize
        {
            get { return new Size(scaley.Width, scalex.Height); }
            set { scalessize = value; this.scalex.Height = value.Height; this.scaley.Width = value.Width; this.OnSizeChanged(null); }
        }
        [Category("Behavior")]
        public Point Value
        {
            get { return new Point(scalex.Value, scaley.Value); }
            set { scalex.Value = value.X; scaley.Value = value.Y; }
        }
        #endregion
        #endregion





        #region mouse down,move,up events
        public bool onmoving { get { return isdown; } }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode)
            {
                Rectangle wrct;
                ISelectionService s;
                ArrayList a;

                for (int i = 0; i < 2; i++)
                {
                    Control button = Controls[i];
                    wrct = new Rectangle(button.Location, button.Size);
                    if (wrct.Contains(e.X, e.Y))
                    {
                        s = (ISelectionService)GetService(typeof(ISelectionService));
                        a = new ArrayList();
                        a.Add(button);
                        s.SetSelectedComponents(a);
                        return;
                    }
                }
            }
            base.OnMouseDown(e);
            if (arearec.Contains(e.Location))
            {
                if (Values.Length > 1)
                {
                    for (int i = 0; i < Values.Length; i++)
                    {
                        PointF p = new PointF(converttopoint(i).X - 5, converttopoint(i).Y - 5);
                        RectangleF rec = new RectangleF(p, new Size(10, 10));
                        if (rec.Contains(e.Location))
                        {
                            slct = i;
                        }
                    }
                }
                else
                {
                    slct = -1;
                }

                isdown = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            GraphicsPath gp = new GraphicsPath();
            if (Graphicvector==GraphicsVetors.HSV_Ciclr)
            {
        gp.AddEllipse(arearec);
    
            }
            else
            {
                gp.AddRectangle(arearec);
            }
            if (isdown)
            {
                if (arearec.Contains(e.Location)&&gp.IsVisible(e.Location))
                {
 

                    if (Values.Length > 1)
                    {
                        if (slct != -1)
                        {
                            Point v = new Point();
                            v.X = fnc.tint((fnc.divdec(e.X, arearec.Width)) * (scalex.Maxmum - scalex.Minimum) + scalex.Minimum);
                            v.Y = fnc.tint((fnc.divdec(e.Y, arearec.Height)) * (scaley.Maxmum - scaley.Minimum) + scaley.Minimum);

                            scalex.Pointers[slct].ValueF = v.X;
                            scaley.Pointers[slct].ValueF = v.Y;
                            scaley.makepointerlocation();
                            scalex.makepointerlocation();
                            scalex.Invalidate(); scaley.Invalidate();
                            this.Invalidate();

                         
                                onvaluechanging(this, new ColorPickereventargs(new Point(scalex.Value, scaley.Value), SelectedColor));
                            
                        }
                    }
                    else
                    {
                        Point v = new Point();
                        v.X = fnc.tint((fnc.divdec(e.X, arearec.Width)) * (scalex.Maxmum - scalex.Minimum) + scalex.Minimum);
                        v.Y = fnc.tint((fnc.divdec(e.Y, arearec.Height)) * (scaley.Maxmum - scaley.Minimum) + scaley.Minimum);

                     
                        scalex.Value = v.X;
                        scaley.Value = v.Y;
                    }
                   
                        onvaluechanging(this, new ColorPickereventargs(new Point(scalex.Value, scaley.Value), SelectedColor));
                    
                }
                else
                {
                    Point o =Point.Round(converttopoint(this.Value));
                    Cursor.Position = new Point(this.PointToScreen(Point.Empty).X+o.X,this.PointToScreen(Point.Empty).Y+o.Y);
                }
            }
        }
        bool hangvaluechangingevent = false;
        public bool HangValueChangingEvent { get { return hangvaluechangingevent; }set { hangvaluechangingevent = value; } }
        public void onvaluechanging(ColorPicker sender,ColorPickereventargs e)
        {
            if (ValueChanging != null&&hangvaluechangingevent==false)
            {
                ValueChanging(sender,e);
            }
        }
        bool hangvaluechangedevent = false;
        public bool HangValueChangedEvent { get { return hangvaluechangedevent; } set { hangvaluechangedevent = value; } }
        public void onvaluechanged(ColorPicker sender, ColorPickereventargs e)
        {
            if (ValueChanged != null && hangvaluechangedevent == false)
            {
                ValueChanged(sender, e);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isdown)
            {
                isdown = false;
                slct = -1;
               
                    onvaluechanged(this, new ColorPickereventargs(new Point(scalex.Value, scaley.Value), SelectedColor));
                
            }
        }
        #endregion

        #region paint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                Bitmap b = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(b);

                updatelayoutrectangle();

                PointF dvalue = converttopoint(0);

                List<PointF> dvls = new List<PointF>();

                for (int i = 0; i < this.Values.Length; i++)
                {
                    dvls.Add(converttopoint(i));
                }

                if (Graphicvector == GraphicsVetors.HSV_Ciclr)
                {
                    GraphicsPath gpp = new GraphicsPath();

                    gpp.AddEllipse(arearec);
                    g.SetClip(gpp);
                }

                Rectangle selectionrec = Rectangle.Round(new RectangleF( arearec.X, dvalue.Y + arearec.Y, dvalue.X, arearec.Height - dvalue.Y));
                if (imgdone)
                {
                    updatebackgrphic();

                    g.DrawImage(backb, 0, 0, arearec.Width, arearec.Height);
                    try
                    {
                        selectedcolor = b.GetPixel((int)dvalue.X,(int) dvalue.Y);
                    }
                    catch
                    { }
                    Pen pn = new Pen(Color.FromArgb(50, ForeColor), 2);


                    for (int i = 0; i < dvls.Count; i++)
                    {
                        if (i != -1)
                        {
                            pn.Width = selectorwidth;
                            PointF p = dvls[i];
                            g.DrawLine(pn, 0, p.Y, b.Width, p.Y);
                            g.DrawLine(pn, p.X, 0, p.X, b.Height);
                            g.DrawEllipse(pn, p.X - 5, p.Y - 5, 10, 10);
                            pn.Width = 2;
                            g.DrawEllipse(pn, p.X - 10, p.Y - 10, 20, 20);
                        }
                    }
                    if (dvls.Count > 1)
                    {
                        g.DrawCurve(pn, dvls.ToArray());
                    }
                    if (fillarea)
                    {
                        int ca = (int)(filledareatransparency * 255F);

                        if (dvls.Count > 1)
                        {
                            dvls.Insert(0, (converttopoint(new Point((int)scaley.Minimum,(int) scaley.Maxmum))));
                            dvls.Add(converttopoint(new Point((int)scalex.Maxmum,(int) scaley.Maxmum)));
                            if (filledareapainting == FilledAreaPainting.FilledAreaColorproperty)
                            {
                                g.FillClosedCurve(new SolidBrush(Color.FromArgb(ca, filledareacolor)), dvls.ToArray());
                            }
                            else
                            {
                                HatchBrush hb = new HatchBrush(pattrentype, Color.FromArgb(ca, forepattrencolor), Color.FromArgb(ca, FilledAreaColor));
                                g.FillClosedCurve(hb, dvls.ToArray());

                            }

                        }
                        else
                        {
                            if (filledareapainting == FilledAreaPainting.FilledAreaColorproperty)
                            {
                                g.FillRectangle(new SolidBrush(Color.FromArgb(ca, filledareacolor)), selectionrec);
                            }
                            else
                            {
                                HatchBrush hb = new HatchBrush(pattrentype, Color.FromArgb(ca, FilledAreaColor), Color.FromArgb(ca, selectedcolor));
                                g.FillRectangle(hb, selectionrec);

                            };
                        }
                    }

                    if (showcolor)
                    {
                        g.ResetClip();
                        g.FillEllipse(new SolidBrush(selectedcolor), scalex.Width, scaley.Height, Width - scalex.Width, Height - scaley.Height);
                    }
                    e.Graphics.DrawImage(b, 0, 0);
                    b.Dispose(); g.Dispose();

                }
            }
            catch
            { throw new Exception(); }
        }
        #endregion
    }
    #endregion

    #region drawshape

    public class DrawShape : Control
    {
        public DrawShape()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.Selectable, true);
            this.BackColor = Color.Transparent;
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);


        }
        bool isdown = false;
        Point pd = Point.Empty;
        int moveangle = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            moveangle = graphicangle;
            pd = e.Location;
            isdown = true;
            this.Focus();
            Invalidate();

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isdown = false;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }
        public enum mos { none, over, o0, o1, o2, o3, o4, o5, o6, o7, rot1, rot2, rot3, rot4 }
        private mos moss = mos.none;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            moss = mos.over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            moss = mos.none;
            Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isdown)
            {
                if (recs[0].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNWSE;
                    moss = mos.o0;
                }
                else if (recs[1].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNS;
                    moss = mos.o1;
                }
                else if (recs[2].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNESW;
                    moss = mos.o2;
                }
                else if (recs[3].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeWE;
                    moss = mos.o3;
                }
                else if (recs[4].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeWE;
                    moss = mos.o4;
                }
                else if (recs[5].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNESW;
                    moss = mos.o5;
                }

                else if (recs[6].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNS;
                    moss = mos.o6;
                }
                else if (recs[7].Contains(e.Location))
                {
                    this.Cursor = Cursors.SizeNWSE;
                    moss = mos.o7;
                }
                else if (recs[8].Contains(e.Location))
                {
                    //  this.Cursor = new Cursor(pntfctmix.Properties.Resources.arrow__left__redo__rotate_icon.GetHicon());
                    moss = mos.rot1;
                }
                else if (recs[9].Contains(e.Location))
                {
                    //       this.Cursor = new Cursor(pntfctmix.Properties.Resources.arrow__left__redo__rotate_icon.GetHicon());
                    moss = mos.rot2;
                }
                else if (recs[10].Contains(e.Location))
                {
                    //       this.Cursor = new Cursor(pntfctmix.Properties.Resources.arrow__left__redo__rotate_icon.GetHicon());
                    moss = mos.rot3;
                }
                else if (recs[11].Contains(e.Location))
                {
                    //       this.Cursor = new Cursor(pntfctmix.Properties.Resources.arrow__left__redo__rotate_icon.GetHicon());
                    moss = mos.rot4;
                }
                else if (arearec.Contains(e.Location))
                {
                    moss = mos.over;
                    this.Cursor = Cursors.SizeAll;
                }
                else
                {
                    moss = mos.none;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (isdown)
            {

                this.SuspendLayout();
                switch (moss)
                {
                    case mos.o0:


                        this.Width -= e.X; this.Height -= e.Y;
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y + e.Y);
                        if (graphicangle != 0)
                        {
                            rotatearea.Width -= e.X;

                            rotatearea.Height -= e.Y;


                            rotateloc = new Point(rotateloc.X + e.X, rotateloc.Y + e.Y);
                            //   Graphicangle = graphicangle;
                        }
                        break;
                    case mos.o1:

                        this.Height -= (e.Y);
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);

                        break;
                    case mos.o2:
                        this.Width = e.X; this.Height -= (e.Y);
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);

                        break;
                    case mos.o3:
                        this.Width -= e.X;
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);

                        break;
                    case mos.o4:
                        this.Width = e.X;

                        break;
                    case mos.o5:
                        this.Width -= e.X; this.Height = e.Y;
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);


                        break;
                    case mos.o6:
                        this.Height = e.Y; ;

                        break;
                    case mos.o7:
                        this.Width = e.X; this.Height = e.Y;

                        break;
                    case mos.rot1:

                    case mos.rot2:
                    case mos.rot3:
                    case mos.rot4:
                        double yy = e.Y;
                        yy = Height / 2 - yy;
                        double xx = e.X;
                        xx = xx - Width / 2;
                        double tn = Math.Abs(xx) / Math.Abs(yy);
                        double ang = Math.Atan(tn) * (180 / Math.PI);


                        if (yy > 0 && xx > 0)
                        { }
                        else if (yy < 0 && xx > 0)
                        { ang = 180 - ang; }
                        else if (yy < 0 && xx < 0)
                        { ang = 180 + ang; }
                        else if (yy > 0 && xx < 0)
                        { ang = 360 - ang; }
                        else if (xx == 0 && yy < 0)
                        { ang += 180; }
                        else if (xx < 0 && yy == 0)
                        { ang += 180; }
                        else if (xx == 0 && yy == 0)
                        { ang = 0; }
                        switch (moss)
                        {
                            case mos.rot1:

                                Graphicangle = (int)ang + moveangle + 45;
                                break;
                            case mos.rot2:

                                Graphicangle = (int)ang + moveangle - 45;
                                break;
                            case mos.rot3:
                                Graphicangle = (int)ang + moveangle + 135;

                                break;
                            case mos.rot4:
                                Graphicangle = (int)ang + moveangle - 135;

                                break;
                        }

                        break;
                    case mos.over:

                        this.Location = new Point(this.Location.X - (pd.X - e.X), this.Location.Y - (pd.Y - e.Y));

                        break;
                }
            }
            this.ResumeLayout();
            Invalidate();
        }
        Rectangle[] recs = new Rectangle[12];
        Size lockersize = new Size(5, 5);
        Size[] tmp = new Size[2];

        protected override void OnSizeChanged(EventArgs e)
        {
            int temp = base.Size.Width;
            if (this.Width < 1)
            { this.Width = 1; }
            if (this.Height < 1)
            { this.Height = 1; }
            arearec = new Rectangle(0, 0, Width, Height);
            areasize = arearec.Size;



            tmp[0] = tmp[1];
            tmp[1] = this.Size;
            if (Rotating == false)
            {
                rotatearea.Width += tmp[1].Width - tmp[0].Width;
                rotatearea.Height += tmp[1].Height - tmp[0].Height;

                if (rotatearea.Height <= 4)
                { rotatearea.Height = 4; }
                if (rotatearea.Width <= 4)
                { rotatearea.Width = 4; }
                //   Graphicangle = graphicangle;
            }
            recs[0] = new Rectangle(new Point(arearec.X, arearec.Y), lockersize);
            recs[1] = new Rectangle(new Point(arearec.Width / 2 + arearec.X, arearec.Y), lockersize);
            recs[2] = new Rectangle(new Point(arearec.Width - lockersize.Width, arearec.Y), lockersize);
            recs[3] = new Rectangle(new Point(arearec.X, arearec.Height / 2 + arearec.Y), lockersize);
            recs[4] = new Rectangle(new Point(arearec.Width - lockersize.Width, arearec.Height / 2 + arearec.Y), lockersize);
            recs[5] = new Rectangle(new Point(arearec.X, arearec.Bottom - lockersize.Height), lockersize);
            recs[6] = new Rectangle(new Point(arearec.Width / 2 + arearec.X, arearec.Bottom - lockersize.Height), lockersize);
            recs[7] = new Rectangle(new Point(arearec.Width - lockersize.Width, arearec.Bottom - lockersize.Height), lockersize);
            recs[8] = new Rectangle(recs[0].Location, new Size(lockersize.Width * 2, lockersize.Height * 2));
            recs[9] = new Rectangle(new Point(arearec.Width - lockersize.Width * 2, arearec.Y), new Size(lockersize.Width * 2, lockersize.Height * 2));
            recs[10] = new Rectangle(new Point(arearec.X, arearec.Bottom - lockersize.Height * 2), new Size(lockersize.Width * 2, lockersize.Height * 2));
            recs[11] = new Rectangle(new Point(arearec.Width - lockersize.Width * 2, arearec.Bottom - lockersize.Height * 2), new Size(lockersize.Width * 2, lockersize.Height * 2));


            if (!isdown)
            {
                Invalidate();
            }

            base.OnSizeChanged(e);


        }
        private Image centerimage;

        public Image CenterImage
        {
            get { return centerimage; }
            set { centerimage = value; Invalidate(); }
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (rotating == false)
            {
                if (graphicangle == 0)
                {
                    rotateloc = this.Location;
                }
                else
                {
                    rotateloc = new Point(this.Left - (int)rotrec(rotatearea).X, this.Top - (int)rotrec(rotatearea).Y);

                }
            }
        }
        Rectangle arearec;
        private Size areasize = new Size();

        public Size AreaSize
        {
            get { return areasize; }
        }
        private int graphicangle = 0;

        PointF[] psouter = new PointF[4];
        PointF[] psinner = new PointF[4];
        RectangleF rotrec(Size rec)
        {
            float nq = 0;
            nq = (float)Math.Sqrt(Math.Pow(fnc.divdec(rec.Width, 2), 2) + Math.Pow(fnc.divdec(rec.Height, 2), 2));

            RectangleF cc = new RectangleF();
            cc.X = (float)(nq * Math.Cos(fnc.Topi(180))) + rec.Width / 2;
            cc.Y = (float)(nq * Math.Sin(fnc.Topi(270))) + rec.Height / 2;
            cc.Width = (float)(nq * Math.Cos(fnc.Topi(0))) + rec.Width / 2 - cc.X;
            cc.Height = (float)(nq * Math.Sin(fnc.Topi(90))) + rec.Height / 2 - cc.Y;

            return cc;
        }
        public int Graphicangle
        {
            get { return graphicangle; }
            set
            {

                graphicangle = value;
                rotating = true;
                if (value == 0)
                {
                    this.Location = rotateloc;
                    this.Size = rotatearea;

                    rotating = false;

                    return;
                }


                float nq = 0;
                nq = (float)Math.Sqrt(Math.Pow(fnc.divdec(rotatearea.Width, 2), 2) + Math.Pow(fnc.divdec(rotatearea.Height, 2), 2));
                RectangleF cc = rotrec(rotatearea);
                this.Location = new Point((int)(rotateloc.X + (cc.X)), (int)(rotateloc.Y + (cc.Y)));
                this.Size = Size.Round(cc.Size);

                /*  for (int i = 0; i < psouter.Length; i++)
                               {
                 
                                   double rad = (value - (i * 90) - 45) * (Math.PI / 180);


                                   psouter[i].X = (float)(nq * Math.Cos(rad)) + rotatearea.Width / 2;
                                   psouter[i].Y = (float)(nq * Math.Sin(rad)) + rotatearea.Height / 2;

                               }
                               for (int i = 0; i < psinner.Length; i++)
                               {
                                   double rad = (value - (i * 90)) * (Math.PI / 180);



                                   psinner[i].X = (float)(rotatearea.Width / 2 * Math.Cos(rad)) + rotatearea.Width / 2;
                                   psinner[i].Y = (float)(rotatearea.Height / 2 * Math.Sin(rad)) + rotatearea.Height / 2;


                               }


                               Point lc = new Point();
                               Size sz = new Size();
                               lc = Point.Round(psouter[0]);
                               sz.Width = (int)psouter[0].X;
                               sz.Height = (int)psouter[0].Y;

                               for (int i = 0; i < psouter.Length; i++)
                               {
                                   if (psouter[i].X < lc.X)
                                   { lc.X = (int)psouter[i].X; }
                                   if (psouter[i].Y < lc.Y)
                                   { lc.Y = (int)psouter[i].Y; }

                                   if (psouter[i].X > sz.Width)
                                   { sz.Width = (int)psouter[i].X; }
                                   if (psouter[i].Y > sz.Height)

                                   { sz.Height = (int)psouter[i].Y; }

                               }

                               */






                rotating = false;







            }
        }

        Size rotatearea;
        public Size Rotatearea
        {
            get { return rotatearea; }
        }
        bool rotating = false;

        public bool Rotating
        {
            get { return rotating; }

        }
        string sss = "";
        private Point rotateloc;
        protected override void OnPaint(PaintEventArgs e)
        {

            Bitmap b = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(b); g.TextRenderingHint = TextRenderingHint.AntiAlias;
            Rectangle allrec = new Rectangle(0, 0, this.Width, this.Height);
            if (this.BackColor == Color.Transparent)
            {
                g.Clear(Color.Transparent);
            }

            Pen borderpen = new Pen(Color.FromArgb(100, Color.Black), 1.0f); borderpen.DashStyle = DashStyle.Dash;
            SolidBrush sb = new SolidBrush(borderpen.Color);



            g.TranslateTransform((b.Width / 2), ((b.Height) / 2));
            g.RotateTransform(graphicangle);
            borderpen.Color = Color.Red; borderpen.DashStyle = DashStyle.Solid;

            Rectangle r = fnc.OImg(centerimage, new Rectangle(new Point(0, 0), rotatearea), BackgroundImageLayout);
            if (centerimage != null)
            {
                g.DrawImage(centerimage, -1 * r.Width / 2, -1 * r.Height / 2, r.Width, r.Height);
            }
            madeimg = (Bitmap)b.Clone();
            g.DrawRectangle(borderpen, -1 * ((rotatearea.Width - 2) / 2) + 1, -1 * ((rotatearea.Height - 2) / 2) + 1, rotatearea.Width - 2, rotatearea.Height - 2);

            g.ResetTransform();
            borderpen.DashStyle = DashStyle.Dash;
            g.DrawRectangle(borderpen, arearec.X + 2, arearec.Y + 2, arearec.Width - 4, arearec.Height - 4);


            for (int i = recs.Length - 1 - 4; i > -1; i--)
            {

                sb.Color = Color.Blue;
                g.FillRectangle(sb, recs[i]);


            }
            // g.DrawLine(Pens.Black, 0, Height / 2, Width, Height / 2);
            for (int i = 0; i < this.Width; i += 40)
            {
                //     g.DrawString(i.ToString(), Font, Brushes.Black, new PointF(i, Height / 2 + 10));
            }
            for (int i = 0; i < psouter.Length; i++)
            {
                //    g.FillRectangle(new SolidBrush(Color.FromArgb(100,Color.Black)), psouter[i].X - 20, psouter[i].Y - 20, 40, 40);
            }
            if (sss != null)
            {
                g.DrawString(sss, this.Font, Brushes.Black, new RectangleF(new PointF(0, 5), this.Size));
            }
            e.Graphics.DrawImage(b, 0, 0);
        }
        Bitmap madeimg;

        public Bitmap Madeimg
        {
            get { return madeimg; }

        }
    }
    #endregion
    #region DrawingBox
    public class DrawingBox : ContainerControl
    {
        public DrawingBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);

        }
    }
    #endregion
    #region Angle
    public enum anglefilltype { None, Inner, Fill };
    [DefaultEvent("ValueChanging")]
    public class Angle : Control
    {
        public Angle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);

        }

        [DefaultValue(0), Category("Behavior")]
        public int Value
        {
            get
            {
                if (ang == 0) return 0;
                return 360 - ang;
            }
            set
            {
                if (value > max || value < min)
                { throw new Exception("Value out of range min:max"); }
                this.ang = 360 - value;

                if (ValueChanged != null)
                {
                    ValueChanged(this, new Inteventaregs(value));
                }
                Invalidate();
            }
        }

        public event inteventhandler ValueChanging;
        public event inteventhandler ValueChanged;

        Color groundcolor = Color.White;
        [Category("Appearance"), DefaultValue(typeof(Color), "White")]
        public Color GroundColor
        {
            get { return groundcolor; }
            set { groundcolor = value; Invalidate(); }
        }
        Color arrowcolor = Color.Black;
        [Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color ArrowColor
        {
            get { return arrowcolor; }
            set { arrowcolor = value; Invalidate(); }
        }
        Color fillareacolor = Color.Black;
        [Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color FillAreaColor
        {
            get { return fillareacolor; }
            set { fillareacolor = value; Invalidate(); }
        }
        private anglefilltype filltype = anglefilltype.Inner;
        [DefaultValue(anglefilltype.Inner), Category("Appearance")]
        public anglefilltype FillType
        {
            get { return filltype; }
            set { filltype = value; Invalidate(); }
        }
        bool showvalue = false;
        [DefaultValue(false), Category("Appearance")]
        public bool ShowValue
        {
            get { return showvalue; }
            set { showvalue = value; Invalidate(); }
        }
        bool showexplation = false;
        [DefaultValue(false), Category("Appearance")]
        public bool ShowExplation
        {
            get { return showexplation; }
            set { showexplation = value; Invalidate(); }
        }

        int innerfillwidth = 6;
        [DefaultValue(6), Category("Appearance")]
        public int InnerFillWidth
        {
            get { return innerfillwidth; }
            set { innerfillwidth = value; Invalidate(); }
        }
        bool detectequaterlenght = false;
        [DefaultValue(false), Category("Behavior")]
        public bool DetectEquaterLenght
        {
            get { return detectequaterlenght; }
            set { detectequaterlenght = value; }
        }
        int equaterlenght = 100;
        [DefaultValue(100), Category("Behavior")]
        public int EquaterLenght
        {
            get { return equaterlenght; }
            set
            {
                if (value < 0 || value > 100) { throw new Exception("value out of range 0:100"); }
                equaterlenght = value; Invalidate();
            }
        }
        bool drawarrow = false;
        [DefaultValue(false), Category("Appearance")]
        public bool DrawArrow
        {
            get { return drawarrow; }
            set { drawarrow = value; Invalidate(); }
        }

        bool drawline = true;
        [DefaultValue(true), Category("Appearance")]
        public bool DrawLine
        {
            get { return drawline; }
            set { drawline = value; Invalidate(); }
        }
        Bitmap centerimage;
        [DefaultValue(null), Category("Appearance")]
        public Bitmap CenterImage
        {
            get { return centerimage; }
            set { centerimage = value; Invalidate(); }
        }
        bool centerrotateimage = true;
        [DefaultValue(true), Category("Appearance")]
        public bool CenterRotateImage
        {
            get { return centerrotateimage; }
            set { centerrotateimage = value; Invalidate(); }
        }
        int min = 0;
        [DefaultValue(0), Category("Behavior")]
        public int Min
        {
            get { return min; }
            set
            {
                if (value < 0 || value > 360)
                { throw new Exception("value out of range 0:360"); }
                else if (value > max)
                { throw new Exception("Minmm can't be more than maxmum"); }

                if (Value < min) Value = min; min = value;
            }
        }
        int max = 360;
        [DefaultValue(360), Category("Behavior")]
        public int Max
        {
            get { return max; }
            set
            {
                if (value < 0 || value > 360)
                { throw new Exception("value out of range 0:360"); }
                else if (value < min)
                { throw new Exception("Maxmum can't be lower than minmum"); }
                if (Value > max) Value = max;
                max = value;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            Bitmap b = new Bitmap(Width, Height);
            int w = b.Width; int h = b.Height;
            Graphics g = Graphics.FromImage(b);
            if (BackColor == Color.Transparent)
            {
                g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, Width, Height);
            }

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            PointF centerpoint = new PointF(w / 2, h / 2);
            PointF valuepoint = new PointF();
            double rad = (ang) * (Math.PI / 180);



            valuepoint.X = (float)(Math.Cos(rad) * (w / 2f / 100f * equaterlenght) + (w / 2f));
            valuepoint.Y = (float)(Math.Sin(-rad) * (h / 2f / 100f * equaterlenght) + (h / 2f));


            Rectangle arearec = new Rectangle(2, 2, w - 4, h - 4);
            g.FillEllipse(Brushes.Black, DisplayRectangle);
            g.FillEllipse(new SolidBrush(groundcolor), arearec);

            if (showexplation)
            {
                Pen ppn = new Pen(Color.Black, 1);
                g.DrawEllipse(ppn, w / 8, h / 8, w - w / (4), h - h / (4));
                g.DrawEllipse(ppn, w / 4, h / 4, w - w / (2), h - h / (2));
                g.DrawLine(ppn, 0, h / 2, w, h / 2);
                g.DrawLine(ppn, w / 2, 0, w / 2, h);
            }
            int fakeang = 360 - ang;
            if (fakeang == 360) { fakeang = 0; }
            if (filltype == anglefilltype.Fill)
            {
                g.FillPie(new SolidBrush(Color.FromArgb(200, fillareacolor)), arearec, 0, fakeang);
            }
            else if (filltype == anglefilltype.Inner)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(arearec);
                g.SetClip(gp);
                g.DrawArc(new Pen(Color.FromArgb(200, fillareacolor), InnerFillWidth), arearec, 0, fakeang);
                g.ResetClip();
            }

            Pen pn = new Pen(arrowcolor, 3);
            if (drawline)
            {
                g.DrawLine(pn, centerpoint, valuepoint);
            }
            if (drawarrow)
            {
                pn.Width = 2;
                g.DrawLine(pn, new PointF(valuepoint.X - 6, valuepoint.Y), new PointF(valuepoint.X + 6, valuepoint.Y));
                g.DrawLine(pn, new PointF(valuepoint.X, valuepoint.Y - 6), new PointF(valuepoint.X, valuepoint.Y + 6));
            }
            g.FillEllipse(Brushes.Black, w / 2 - 3, h / 2 - 3, 6, 6);
            if (centerimage != null)
            {
                KGrphs kg = new KGrphs(b);
                kg.Rotatrangle = fakeang;
                kg.Centerrotate = centerrotateimage;
                Bitmap bci = new Bitmap(Width / 3, Height / 3);
                Graphics ggg = Graphics.FromImage(bci);
                ggg.DrawImage(centerimage, 0, 0, bci.Width, bci.Height);
                kg.drawimage(bci, centerpoint);
                ggg.Dispose();
            }
            if (showvalue)
            {
                SizeF valuesize = g.MeasureString((fakeang).ToString(), this.Font);
                PointF valueloc = new PointF(w / 2 - (valuesize.Width / 2), h / 2 + 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, BackColor)), new RectangleF(valueloc, valuesize));
                g.DrawString((fakeang).ToString(), this.Font, new SolidBrush(ForeColor), valueloc);
            }

            e.Graphics.DrawImage(b, 0, 0, Width, Height);
            g.Dispose();
        }

        bool isdown = false; bool ismove = false;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Height = this.Width; //Keep it a square

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isdown = true;

        }
        int ang = 0;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isdown)
            {
                ismove = true;
                Point centerp = new Point(Width / 2, Height / 2);
                if (detectequaterlenght)
                {
                    float y = Math.Abs(e.Y - centerp.Y);
                    float x = Math.Abs(e.X - centerp.X);
                    float ln = (float)Math.Sqrt(Math.Pow(y, 2) + Math.Pow(x, 2));
                    float bigln = (float)Math.Sqrt(Math.Pow(centerp.Y, 2) + Math.Pow(centerp.X, 2));
                    equaterlenght = (int)(ln / bigln * 100f);
                    if (equaterlenght > 100)
                    { equaterlenght = 100; }
                }
                ang = fnc.GetAngle(centerp, e.Location);
                if (Value < min)
                {
                    Value = Min;
                }
                else if (Value > max)
                { Value = Max; }
                Invalidate();
                if (ValueChanging != null)
                {
                    ValueChanging(this, new Inteventaregs(this.Value));
                }
            }

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!ismove)
            {
                ang = fnc.GetAngle(new Point(Width / 2, Height / 2), e.Location);
                if (Value < min)
                {
                    Value = Min;
                }
                else if (Value > max)
                { Value = Max; }
                Invalidate();

                if (ValueChanged != null)
                {
                    ValueChanged(this, new Inteventaregs(Value));
                }
            }
            isdown = false;
            ismove = false;
        }

    }
    #endregion
    #region Clock
    public enum Clockstyle { normal, gradiat }
    public enum Linestyle { line, dot }
    public enum TextMode { numbers, XII }
    public enum ArrowStyle { Line, Tarngle }
    public enum clockTypes { Analogue, Digital }
    public enum hourformat { hour12, hour24 }
    public enum clockThemes { gray, Sky, grady, custom }

    public class Clock : Control, ISupportInitialize
    {
        private hourformat digitalhoursystem = hourformat.hour12;
        public hourformat Digitalhoursystem
        {
            get { return digitalhoursystem; }
            set { digitalhoursystem = value; Invalidate(); }
        }

        private clockTypes type = clockTypes.Digital;
        public clockTypes Type
        {
            get { return type; }
            set { type = value; Invalidate(); }
        }

        private clockThemes theme = clockThemes.gray;
        private ArrowStyle arrowstyle = ArrowStyle.Tarngle;
        private TextMode textmode = TextMode.numbers;
        private Clockstyle clockstyle = Clockstyle.normal;
        private Linestyle linestyle = Linestyle.line;

        public clockThemes Theme
        {
            get { return theme; }
            set
            {
                switch (value)
                {
                    case clockThemes.gray:
                        fillcolor = Color.FromArgb(30, 30, 30);
                        bordercolor = Color.FromArgb(64, 64, 64);
                        color1 = Color.FromArgb(230, 230, 230);
                        color2 = Color.FromArgb(215, 215, 215);
                        sharpbordercolor = Color.FromArgb(30, 30, 30);
                        gradientcolors = new Color[] { Color.FromArgb(240, 240, 240), Color.FromArgb(210, 210, 210), Color.FromArgb(180, 180, 180), Color.FromArgb(140, 140, 140), Color.FromArgb(120, 120, 120), Color.FromArgb(100, 100, 100), Color.FromArgb(70, 70, 70) };
                        cntr = Color.FromArgb(50, 50, 50);
                        cntr2 = Color.FromArgb(200, 200, 200);
                        textforecolor = Color.FromArgb(100, 100, 100);
                        digitalpointcolor = textforecolor;
                        analoguecolors.Clear();
                        analoguecolors.AddRange(new Color[] { Color.Red, Color.Red, Color.Red, Color.Red, Color.FromArgb(64, 64, 64), Color.FromArgb(64, 64, 64) });
                        break;
                    case clockThemes.Sky:
                        fillcolor = Color.Blue;
                        bordercolor = Color.SteelBlue;
                        sharpbordercolor = Color.FromArgb(64, 64, 64);
                        color1 = Color.SkyBlue;
                        color2 = Color.PowderBlue;//{ Color.FromArgb(0, 0, 20), Color.FromArgb(10, 10, 40), Color.FromArgb(20, 20, 80), Color.FromArgb(30, 30, 120), Color.FromArgb(40, 40, 170), Color.FromArgb(50, 50, 210), Color.FromArgb(55, 55, 240) };
                        //   gradientcolors = new Color[] { Color.FromArgb(16,71, 11, 30), Color.FromArgb(16,71,11, 90), Color.FromArgb(16,70, 11, 130), Color.FromArgb(16,70, 10, 160), Color.FromArgb(16,70, 11, 200), Color.FromArgb(16,70, 11, 225), Color.FromArgb(16, 70,11, 255) };
                        gradientcolors = new Color[] { Color.LightCyan, Color.PowderBlue, Color.PaleTurquoise, Color.SkyBlue, Color.LightSteelBlue, Color.SteelBlue, Color.Cyan };

                        cntr = Color.SteelBlue;
                        cntr2 = Color.LightCyan;
                        textforecolor = Color.SteelBlue;
                        digitalpointcolor = textforecolor;
                        analoguecolors.Clear();
                        analoguecolors.AddRange(new Color[] { Color.Red, Color.Red, Color.Red, Color.Red, Color.Cyan, Color.Cyan });

                        break;
                    case clockThemes.grady:
                        fillcolor = Color.FromArgb(220, 180, 20, 50);
                        sharpbordercolor = Color.FromArgb(64, 64, 64);
                        bordercolor = Color.FromArgb(210, 10, 10);
                        color1 = Color.Yellow;
                        color2 = Color.YellowGreen;
                        gradientcolors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.LightGreen, Color.Cyan, Color.SteelBlue, Color.Gray };
                        cntr = Color.Red;
                        cntr2 = Color.Cyan;
                        textforecolor = Color.Red;
                        digitalpointcolor = textforecolor;
                        analoguecolors.Clear();
                        analoguecolors.AddRange(new Color[] { Color.Green, Color.Green, Color.Red, Color.Red, Color.Black, Color.Black });

                        break;
                }
                Invalidate();
            }
        }
        bool shownumbers = true;
        public bool Shownumbers
        {
            get { return shownumbers; }
            set { shownumbers = value; Invalidate(); }
        }

        bool showlines = true;
        public bool Showlines
        {
            get { return showlines; }
            set { showlines = value; Invalidate(); }
        }

        bool showmlisecond = false;
        public bool Showmlisecond
        {
            get { return showmlisecond; }
            set { showmlisecond = value; Invalidate(); }
        }

        bool showsecond = true;
        public bool Showsecond
        {
            get { return showsecond; }
            set { showsecond = value; Invalidate(); }
        }

        bool fillhours = true;
        public bool Fillhours
        {
            get { return fillhours; }
            set { fillhours = value; Invalidate(); }
        }

        private bool rotatetext = true;
        public bool Rotatetext
        {
            get { return rotatetext; }
            set { rotatetext = value; Invalidate(); }
        }

        private Color fillcolor = Color.Black;
        public Color Fillcolor
        {
            get { return fillcolor; }
            set { fillcolor = value; Invalidate(); }
        }

        private bool showbackarrow = true;
        public bool Showbackarrow
        {
            get { return showbackarrow; }
            set { showbackarrow = value; Invalidate(); }
        }

        Color cntr = Color.Empty;
        public Color Cntr
        {
            get { return cntr; }
            set { cntr = value; Invalidate(); }
        }

        Color cntr2 = Color.Empty;
        public Color Cntr2
        {
            get { return cntr2; }
            set { cntr2 = value; Invalidate(); }
        }

        private float centrsirclesize = 0.5f;

        public float Centrsirclesize
        {
            get { return centrsirclesize; }
            set { centrsirclesize = value; Invalidate(); }
        }

        private string[] IIVstng = new string[12] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
        //'''
        Font textfont = new Font("Valken", 10);

        Color bordercolor = Color.Black;
        public Color Bordercolor
        {
            get { return bordercolor; }
            set { bordercolor = value; Invalidate(); }
        }

        Color sharpbordercolor = Color.Black;
        public Color Sharpbordercolor
        {
            get { return sharpbordercolor; }
            set { sharpbordercolor = value; Invalidate(); }
        }

        Color color1 = Color.Black;
        public Color Color1
        {
            get { return color1; }
            set { color1 = value; Invalidate(); }
        }

        Color color2 = Color.Black;
        public Color Color2
        {
            get { return color2; }
            set { color2 = value; Invalidate(); }
        }

        Color[] gradientcolors = new Color[7];
        public Color[] Gradientcolors
        {
            get { return gradientcolors; }
            set { gradientcolors = value; Invalidate(); }
        }

        //'''''
        Color textforecolor = Color.Black;

        Color digitalpointcolor = Color.White;
        public Color Digitalpointcolor
        {
            get { return digitalpointcolor; }
            set { digitalpointcolor = value; Invalidate(); }
        }





        private List<Color> analoguecolors = new List<Color>();
        public List<Color> Analoguecolors
        {
            get { return analoguecolors; }
            set { analoguecolors = value; Invalidate(); }
        }

        private List<int> digitalspace = new List<int> { 2, 4 };
        public List<int> Digitalspace
        {
            get { return digitalspace; }
            set { digitalspace = value; Invalidate(); }
        }

        private int[] none = new int[] { 6, 5 };
        private int[] ntwo = new int[] { 6, 7, 2, 3, 4 };
        private int[] nthree = new int[] { 7, 6, 2, 5, 4 };
        private int[] nfour = new int[] { 1, 2, 6, 5 };
        private int[] nfive = new int[] { 1, 2, 5, 4, 7 };
        private int[] nsix = new int[] { 1, 2, 3, 4, 5, 7 };
        private int[] nseven = new int[] { 6, 7, 5 };
        private int[] neight = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        private int[] nnine = new int[] { 1, 2, 7, 6, 5, 4 };
        private int[] nzero = new int[] { 1, 3, 4, 5, 6, 7 };

        private Point[] first = new Point[4];
        private Point[] four = new Point[4];
        private Point[] two = new Point[4];
        private Point[] three = new Point[4];
        private Point[] five = new Point[4];
        private Point[] six = new Point[4];
        private Point[] svn = new Point[4];

        private void makearry(Point loc, Size ddsiz)
        {

            first[0] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 5 * ddsiz.Height / 100);
            first[1] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 40 * ddsiz.Height / 100);
            first[2] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 40 * ddsiz.Height / 100);
            first[3] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 20 * ddsiz.Height / 100);

            //2

            two[0] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 45 * ddsiz.Height / 100);
            two[1] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 55 * ddsiz.Height / 100);
            two[2] = new Point(loc.X + 90 * ddsiz.Width / 100, loc.Y + 55 * ddsiz.Height / 100);
            two[3] = new Point(loc.X + 80 * ddsiz.Width / 100, loc.Y + 45 * ddsiz.Height / 100);

            //3

            three[0] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 50 * ddsiz.Height / 100);
            three[1] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 60 * ddsiz.Height / 100);
            three[2] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 80 * ddsiz.Height / 100);
            three[3] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 80 * ddsiz.Height / 100);

            //4
            four[0] = new Point(loc.X + 5 * ddsiz.Width / 100, loc.Y + 85 * ddsiz.Height / 100);
            four[1] = new Point(loc.X + 25 * ddsiz.Width / 100, loc.Y + 95 * ddsiz.Height / 100);
            four[2] = new Point(loc.X + 87 * ddsiz.Width / 100, loc.Y + 95 * ddsiz.Height / 100);
            four[3] = new Point(loc.X + 72 * ddsiz.Width / 100, loc.Y + 85 * ddsiz.Height / 100);


            //5

            five[0] = new Point(loc.X + 95 * ddsiz.Width / 100, loc.Y + 95 * ddsiz.Height / 100);
            five[1] = new Point(loc.X + 80 * ddsiz.Width / 100, loc.Y + 85 * ddsiz.Height / 100);
            five[2] = new Point(loc.X + 80 * ddsiz.Width / 100, loc.Y + 60 * ddsiz.Height / 100);
            five[3] = new Point(loc.X + 95 * ddsiz.Width / 100, loc.Y + 60 * ddsiz.Height / 100);

            //6

            six[0] = new Point(loc.X + 95 * ddsiz.Width / 100, loc.Y + 55 * ddsiz.Height / 100);
            six[1] = new Point(loc.X + 80 * ddsiz.Width / 100, loc.Y + 40 * ddsiz.Height / 100);
            six[2] = new Point(loc.X + 80 * ddsiz.Width / 100, loc.Y + 5 * ddsiz.Height / 100);
            six[3] = new Point(loc.X + 95 * ddsiz.Width / 100, loc.Y + 5 * ddsiz.Height / 100);



            //7

            svn[0] = new Point(loc.X + 12 * ddsiz.Width / 100, loc.Y + 5 * ddsiz.Height / 100);
            svn[1] = new Point(loc.X + 32 * ddsiz.Width / 100, loc.Y + 20 * ddsiz.Height / 100);
            svn[2] = new Point(loc.X + 75 * ddsiz.Width / 100, loc.Y + 20 * ddsiz.Height / 100);
            svn[3] = new Point(loc.X + 75 * ddsiz.Width / 100, loc.Y + 5 * ddsiz.Height / 100);

        }
        private void drawback(ref Graphics gg, Color col)
        {
            Color cc = col;
            Pen pnn = new Pen(cc, 12);
            //1


            gg.FillPolygon(new SolidBrush(cc), first);
            gg.DrawPolygon(pnn, first);
            //2


            gg.FillPolygon(new SolidBrush(cc), two);
            gg.DrawPolygon(pnn, two);
            //3


            gg.FillPolygon(new SolidBrush(cc), three);
            gg.DrawPolygon(pnn, three);
            //4


            gg.FillPolygon(new SolidBrush(cc), four);
            gg.DrawPolygon(pnn, four);
            //5

            gg.FillPolygon(new SolidBrush(cc), five);
            gg.DrawPolygon(pnn, five);
            //6


            gg.FillPolygon(new SolidBrush(cc), six);
            gg.DrawPolygon(pnn, six);
            //7


            gg.FillPolygon(new SolidBrush(cc), svn);
            gg.DrawPolygon(pnn, svn);


        }
        private void drawcolm(int where, ref Graphics gg, Color colback)
        {

            Color cc = colback;
            Pen pnn = new Pen(cc, 8);
            switch (where)
            {

                case 1:
                    //1

                    gg.FillPolygon(new SolidBrush(cc), first);
                    gg.DrawPolygon(pnn, first);
                    break;
                case 2:
                    //2

                    gg.FillPolygon(new SolidBrush(cc), two);
                    gg.DrawPolygon(pnn, two);
                    break;
                case 3:
                    //3

                    gg.FillPolygon(new SolidBrush(cc), three);
                    gg.DrawPolygon(pnn, three);
                    break;
                case 4:
                    //4

                    gg.FillPolygon(new SolidBrush(cc), four);
                    gg.DrawPolygon(pnn, four);
                    break;
                case 5:
                    //5


                    gg.FillPolygon(new SolidBrush(cc), five);
                    gg.DrawPolygon(pnn, five);
                    break;
                case 6:
                    //6


                    gg.FillPolygon(new SolidBrush(cc), six);
                    gg.DrawPolygon(pnn, six);

                    break;
                case 7:
                    //7


                    gg.FillPolygon(new SolidBrush(cc), svn);
                    gg.DrawPolygon(pnn, svn);
                    break;
            }
        }
        private Bitmap bk(int n, Color col, Color colback, Point p, Size sz)
        {
            makearry(new Point(0, 0), sz);

            Bitmap b = new Bitmap(sz.Width, sz.Height); Graphics g = Graphics.FromImage(b);
            for (int i = 0; i <= 9; i++)
            {
                drawfornt(i, colback, p, sz, ref g);
            }
            drawfornt(n, col, p, sz, ref g);
            return b;
        }
        private void drawfornt(int n, Color col, Point p, Size sz, ref Graphics gg)
        {

            switch (n)
            {
                case 0:

                    foreach (int i in nzero)
                    {
                        drawcolm(i, ref gg, col);
                    }
                    break;

                case 1:
                    foreach (int i in none)
                    {
                        drawcolm(i, ref gg, col);
                    }
                    break;
                case 2:
                    foreach (int i in ntwo)
                    {
                        drawcolm(i, ref gg, col);
                    }
                    break;
                case 3:
                    foreach (int i in nthree)
                    {
                        drawcolm(i, ref gg, col);
                    }
                    break;
                case 4:
                    foreach (int i in nfour)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
                case 5:
                    foreach (int i in nfive)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
                case 6:
                    foreach (int i in nsix)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
                case 7:
                    foreach (int i in nseven)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
                case 8:
                    foreach (int i in neight)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
                case 9:
                    foreach (int i in nnine)
                    {
                        drawcolm(i, ref  gg, col);
                    }
                    break;
            }


        }
        private void drawints(int n, ref Graphics gg, Color col, Color colback, Point p, Size sz)
        {
            int clok = 0;


            for (int i = 0; i < n.ToString().Length; i++)
            {
                gg.DrawImage(bk(int.Parse(n.ToString()[i].ToString()), col, colback, new Point(0, 0), sz), p.X + (sz.Width * i + 10 + clok), p.Y);
            }
        }

        #region ck
        private Point ck(int value, int width, int height, int wtall, int htall, int div, bool spesialhour)
        {
            Double ang = 0;
            if (spesialhour)
            {
                ang = (fnc.divdec(fnc.divdec(360, 12), 60) * DateTime.Now.Minute) * (Math.PI / 180);
            }
            Double angle = (270 + (fnc.divdec(360, div) * value)) * (Math.PI / 180);
            angle += ang;


            Point pnt = new Point();
            wtall = width / 2 - wtall; htall = height / 2 - htall;
            pnt.X = fnc.tint(Math.Cos(angle) * (width / 2 - wtall) + width / 2);
            pnt.Y = fnc.tint(Math.Sin(angle) * (height / 2 - htall) + height / 2);

            return pnt;
        }
        private Point ck(int value, int width, int height, int wtall, int htall, int div)
        {
            return ck(value, width, height, wtall, htall, div, false);
        }
        private Point ck(int value, Size bigsiz, Size linesiz, int div, bool spesialhour)
        {
            return ck(value, bigsiz.Width, bigsiz.Height, linesiz.Width, linesiz.Height, div, spesialhour);
        }
        private Point ck(int value, Size bigsiz, Size linesiz, int div)
        {
            return ck(value, bigsiz.Width, bigsiz.Height, linesiz.Width, linesiz.Height, div, false);
        }
        #endregion

        public void watch()
        {
            t.Priority = ThreadPriority.Normal;

            if (!DesignMode && !IsDisposed)
            {

                while (true)
                {
                    try
                    {
                        if (!DesignMode)
                        {


                            if (!this.IsDisposed)
                            {
                                int h2 = 0;
                                if (digitalhoursystem == hourformat.hour12 && Analoguevalue.Hour > 12)
                                { h2 = Analoguevalue.Hour - 12; }
                                else
                                { h2 = Analoguevalue.Hour; }


                                int h1 = 0;
                                if (h2.ToString().Length > 1)
                                {
                                    h1 = int.Parse(h2.ToString().Substring(0, 1));
                                    h2 = int.Parse(h2.ToString().Substring(1, 1));
                                }

                                int m2 = Analoguevalue.Minute;
                                int m1 = 0;
                                if (m2.ToString().Length > 1)
                                {
                                    m1 = int.Parse(m2.ToString().Substring(0, 1));
                                    m2 = int.Parse(m2.ToString().Substring(1, 1));
                                }

                                int s2 = Analoguevalue.Second;
                                int s1 = 0;
                                if (s2.ToString().Length > 1)
                                {
                                    s1 = int.Parse(s2.ToString().Substring(0, 1));
                                    s2 = int.Parse(s2.ToString().Substring(1, 1));
                                }
                                digitalvalue.Clear();
                                digitalvalue.AddRange(new int[] { h1, h2, m1, m2, s1, s2 });
                                analoguevalue = DateTime.Now;

                                OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                                Thread.Sleep(20);
                            }
                            else { t.Abort(); }
                        }
                        else
                        { t.Abort(); }
                    }
                    catch
                    {
                        t.Abort();
                    }
                }
            }



        }

        Thread t;

        public Clock()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            int h2 = 0;
            if (digitalhoursystem == hourformat.hour12 && Analoguevalue.Hour > 12)
            { h2 = Analoguevalue.Hour - 12; }
            else
            { h2 = Analoguevalue.Hour; }


            int h1 = 0;
            if (h2.ToString().Length > 1)
            {
                h1 = int.Parse(h2.ToString().Substring(0, 1));
                h2 = int.Parse(h2.ToString().Substring(1, 1));
            }

            int m2 = Analoguevalue.Minute;
            int m1 = 0;
            if (m2.ToString().Length > 1)
            {
                m1 = int.Parse(m2.ToString().Substring(0, 1));
                m2 = int.Parse(m2.ToString().Substring(1, 1));
            }

            int s2 = Analoguevalue.Second;
            int s1 = 0;
            if (s2.ToString().Length > 1)
            {
                s1 = int.Parse(s2.ToString().Substring(0, 1));
                s2 = int.Parse(s2.ToString().Substring(1, 1));
            }
            digitalvalue.Clear();
            digitalvalue.AddRange(new int[] { h1, h2, m1, m2, s1, s2 });
            analoguevalue = DateTime.Now;
            this.Theme = clockThemes.gray;

        }
        public void EndInit()
        {
            if (Auto)
            {

                FindForm().Load += new EventHandler(Clock_Load);
            }
        }

        public void BeginInit()
        { }
        ~Clock()
        {
            t.Abort();
        }
        DateTime analoguevalue;
        public DateTime Analoguevalue
        {
            get { return analoguevalue; }
            set { analoguevalue = value; Invalidate(); }
        }

        private List<int> digitalvalue = new List<int>();
        public List<int> Digitalvalue
        {
            get { return digitalvalue; }
            set { digitalvalue = value; Invalidate(); }
        }

        private void Clock_Load(object sender, EventArgs e)
        {
            Isrun = true;
        }

        bool isrun = false;
        public bool Isrun
        {
            get { return isrun; }
            set
            {
                isrun = value;
                if (value)
                {
                    if (t == null)
                    {
                        ThreadStart ts = new ThreadStart(watch);
                        t = new Thread(ts);
                        t.IsBackground = true;
                        t.Start();

                    }
                    else if (!t.IsAlive)
                    {
                        ThreadStart ts = new ThreadStart(watch);
                        t = new Thread(ts);
                        t.IsBackground = true;

                        t.Start();
                    }
                }
                else { if (t != null)if (t.IsAlive) t.Abort(); }
            }
        }

        bool auto = true;
        public bool Auto
        {
            get { return auto; }
            set
            {
                auto = value;

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap b = new Bitmap(Width, Height);
            if (b.Width < 1000) b = new Bitmap(1000, b.Height);
            if (b.Height < 1000) b = new Bitmap(b.Width, 1000);

            Graphics g = Graphics.FromImage(b); g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle allrec = new Rectangle(0, 0, b.Width, b.Height);

            if (this.BackColor == Color.Transparent)
            {
                g.Clear(Color.Transparent);
            }

            #region graphic
            if (Type == clockTypes.Analogue)
            {
                #region Clock
                #region style
                switch (clockstyle)
                {
                    case Clockstyle.normal:

                        LinearGradientBrush lgb = new LinearGradientBrush(allrec, bordercolor, Color.FromArgb(180, bordercolor), 145);
                        g.FillEllipse(lgb, allrec);

                        GraphicsPath gb = new GraphicsPath();
                        gb.AddEllipse(fnc.divdec(b.Width, 20) * .5f, fnc.divdec(b.Width, 20) * .5f, b.Width - fnc.divdec(b.Width, 20) * 1.0f, b.Height - fnc.divdec(b.Width, 20) * 1.0f);

                        PathGradientBrush pgb = new PathGradientBrush(gb);

                        ColorBlend cb = new ColorBlend(3);
                        cb.Colors = new Color[] { color1, color2, color1 };
                        cb.Positions = new float[] { 0.0f, 0.4f, 1.0f };
                        pgb.InterpolationColors = cb;
                        g.FillPath(pgb, gb);

                        g.DrawEllipse(new Pen(sharpbordercolor, 3), 0, 0, b.Width - 5, b.Height - 5);

                        g.DrawEllipse(new Pen(sharpbordercolor, 3), fnc.divdec(b.Width, 20) * .5f, fnc.divdec(b.Width, 20) * .5f, b.Width - fnc.divdec(b.Width, 20) * 1.0f, b.Height - fnc.divdec(b.Width, 20) * 1.0f);



                        break;
                    case Clockstyle.gradiat:

                        lgb = new LinearGradientBrush(allrec, bordercolor, Color.FromArgb(210, bordercolor), 145);
                        g.FillEllipse(lgb, allrec);

                        gb = new GraphicsPath();
                        gb.AddEllipse(fnc.divdec(b.Width, 20) * .5f, fnc.divdec(b.Width, 20) * .5f, b.Width - fnc.divdec(b.Width, 20) * 1.0f, b.Height - fnc.divdec(b.Width, 20) * 1.0f);

                        pgb = new PathGradientBrush(gb);

                        cb = new ColorBlend(7);
                        cb.Colors = gradientcolors;
                        cb.Positions = new float[] { 0.0f, 0.02f, 0.30f, 0.45f, 0.60f, 0.75f, 1.0f };
                        pgb.InterpolationColors = cb;

                        g.FillPath(pgb, gb);


                        g.DrawEllipse(new Pen(Color.FromArgb(64, 64, 64), 3), 0, 0, b.Width - 5, b.Height - 5);

                        g.DrawEllipse(new Pen(Color.FromArgb(64, 64, 64), 3), fnc.divdec(b.Width, 20) * .5f, fnc.divdec(b.Width, 20) * .5f, b.Width - fnc.divdec(b.Width, 20) * 1.0f, b.Height - fnc.divdec(b.Width, 20) * 1.0f);

                        break;

                }
                #endregion
                #region numbers and lines
                for (int i = 1; i <= 60; i++)
                {

                    Double angle = ((270 + (360 / 60 * i)) * (Math.PI / 180));
                    Point pnt = new Point();

                    Point pnt2 = new Point();
                    Pen linespen = new Pen(textforecolor, 5 * fnc.divdec(b.Width, 1000));

                    pnt.X = fnc.tint(Math.Cos(angle) * (b.Width / 2 - fnc.divdec(b.Width, 20) * 1) + b.Width / 2);
                    pnt.Y = fnc.tint(Math.Sin(angle) * (b.Height / 2 - fnc.divdec(b.Width, 20) * 1) + b.Height / 2);

                    if (i % 5 == 0)
                    {
                        //lines
                        if (showlines)
                        {
                            switch (linestyle)
                            {
                                case Linestyle.line:
                                    linespen.Width = 9 * b.Width / 1000;
                                    pnt2.X = fnc.tint(Math.Cos(angle) * (b.Width / 2 - fnc.divdec(b.Width, 20) * 2) + b.Width / 2);
                                    pnt2.Y = fnc.tint(Math.Sin(angle) * (b.Height / 2 - fnc.divdec(b.Width, 20) * 2) + b.Height / 2);
                                    g.DrawLine(linespen, pnt, pnt2);
                                    break;
                                case Linestyle.dot:
                                    Size elpwd = new Size(fnc.divdectint(b.Width, 40), fnc.divdectint(b.Height, 40));
                                    g.FillEllipse(new SolidBrush(linespen.Color), pnt.X - elpwd.Width / 2, pnt.Y - elpwd.Height / 2, elpwd.Width, elpwd.Height);
                                    break;

                            }
                        }
                        //nnums;
                        if (shownumbers)
                        {
                            PointF stngp = new PointF();
                            if (showlines)
                            {
                                stngp.X = (float)(Math.Cos(angle) * (b.Width / 2 - fnc.divdec(b.Width, 20) * 3.2) + b.Width / 2);
                                stngp.Y = (float)(Math.Sin(angle) * (b.Height / 2 - fnc.divdec(b.Width, 20) * 3.2) + b.Height / 2);
                            }
                            else
                            {
                                stngp.X = (float)(Math.Cos(angle) * (b.Width / 2 - fnc.divdec(b.Width, 20) * 2) + b.Width / 2);
                                stngp.Y = (float)(Math.Sin(angle) * (b.Height / 2 - fnc.divdec(b.Width, 20) * 2) + b.Height / 2);
                            }
                            string draws = "";

                            switch (textmode)
                            {
                                case TextMode.XII:
                                    draws = IIVstng[(i / 5) - 1];
                                    break;
                                case TextMode.numbers:
                                    draws = (i / 5).ToString();
                                    break;
                            }
                            SizeF stngs = new SizeF();
                            float fontsize = 65 * fnc.divdec(b.Width, 1000);
                            Font fs = new Font(textfont.Name, fontsize);

                            stngs = g.MeasureString(draws, fs);
                            stngp.X -= (int)stngs.Width / 2;
                            stngp.Y -= (int)stngs.Height / 2;

                            if (rotatetext)
                            {
                                g.TranslateTransform(stngp.X + stngs.Width / 2, stngp.Y + stngs.Height / 2);
                                g.RotateTransform((360 / 60 * i));
                                stngp = new PointF(-1 * stngs.Width / 2, -1 * stngs.Height / 2);
                            }



                            g.DrawString(draws, fs, new SolidBrush(textforecolor), stngp);
                            g.ResetTransform();
                            //
                        }
                    }
                    else
                    {

                        //lines
                        if (showlines)
                        {
                            linespen.Width = 5 * fnc.divdec(b.Width, 1000);

                            pnt2.X = fnc.tint(Math.Cos(angle) * (b.Width / 2 - fnc.divdec(b.Width, 20) * 1.2) + b.Width / 2);
                            pnt2.Y = fnc.tint(Math.Sin(angle) * (b.Height / 2 - fnc.divdec(b.Width, 20) * 1.2) + b.Height / 2);
                            switch (linestyle)
                            {
                                case Linestyle.line:
                                    g.DrawLine(linespen, pnt, pnt2);
                                    break;
                                case Linestyle.dot:
                                    Size elpwd = new Size(fnc.divdectint(b.Width, 70), fnc.divdectint(b.Height, 70));
                                    g.FillEllipse(new SolidBrush(linespen.Color), pnt.X - elpwd.Width / 2, pnt.Y - elpwd.Height / 2, elpwd.Width, elpwd.Height);
                                    break;
                            }
                        }
                    }
                }
                #endregion
                #region arrows
                Point halfp = new Point(b.Width / 2, b.Height / 2);
                Size sectal = new Size(fnc.divdectint(b.Width, 100) * 38, fnc.divdectint(b.Width, 10) * 4);
                Size mnttal = new Size(fnc.divdectint(b.Width, 100) * 30, fnc.divdectint(b.Width, 10) * 3);
                Size hortal = new Size(fnc.divdectint(b.Width, 100) * 20, fnc.divdectint(b.Width, 10) * 2);

                Point bksecpnt = new Point(halfp.X, halfp.Y);
                Point bkmntpnt = new Point(halfp.X, halfp.Y);
                Point bkhorpnt = new Point(halfp.X, halfp.Y);
                if (showbackarrow)
                {
                    bksecpnt = ck(Analoguevalue.Second, b.Size, new Size(-1 * b.Width / 10, -1 * b.Height / 10), 60);
                    bkmntpnt = ck(Analoguevalue.Minute, b.Size, new Size(-1 * b.Width / 15, -1 * b.Height / 15), 60);
                    bkhorpnt = ck(Analoguevalue.Hour, b.Size, new Size(-1 * b.Width / 15, -1 * b.Height / 15), 12, true);

                }

                if (showmlisecond) g.DrawLine(new Pen(Color.Black, 2 * b.Width / 1000), halfp, ck(Analoguevalue.Millisecond, 1000, 1000, 300, 300, 1000));

                switch (arrowstyle)
                {
                    case ArrowStyle.Line:

                        g.DrawLine(new Pen(Color.Black, 16 * b.Width / 1000), bkhorpnt, ck(Analoguevalue.Hour, b.Size, hortal, 12, true));
                        g.DrawLine(new Pen(Color.Gray, 10 * b.Width / 1000), bkmntpnt, ck(Analoguevalue.Minute, b.Size, mnttal, 60));
                        if (showsecond) g.DrawLine(new Pen(Color.Red, 6 * b.Width / 1000), bksecpnt, ck(Analoguevalue.Second, b.Size, sectal, 60));

                        break;
                    case ArrowStyle.Tarngle:
                        Size secwd = new Size(-25 * b.Width / 1000, -25 * b.Width / 1000);
                        Size mntwd = new Size(-40 * b.Width / 1000, -40 * b.Width / 1000);
                        Size horwd = new Size(-30 * b.Width / 1000, -30 * b.Width / 1000);

                        if (showsecond)
                        {
                            Point pp1 = new Point(); pp1 = ck(Analoguevalue.Second - 10, b.Size, secwd, 60);
                            Point pp2 = new Point(); pp2 = ck(Analoguevalue.Second + 10, b.Size, secwd, 60);
                            g.FillPolygon(new SolidBrush(Color.Red), new Point[] { pp1, pp2, ck(Analoguevalue.Second, b.Size, sectal, 60) });
                        }
                        Point pps1 = new Point(); pps1 = ck(Analoguevalue.Minute - 10, b.Size, mntwd, 60);
                        Point pps2 = new Point(); pps2 = ck(Analoguevalue.Minute + 10, b.Size, mntwd, 60);
                        g.FillPolygon(new SolidBrush(Color.DimGray), new Point[] { pps1, pps2, ck(Analoguevalue.Minute, b.Size, mnttal, 60) });

                        Point pph1 = new Point(); pph1 = ck(Analoguevalue.Hour - 3, b.Size, horwd, 12, true);
                        Point pph2 = new Point(); pph2 = ck(Analoguevalue.Hour + 3, b.Size, horwd, 12, true);
                        g.FillPolygon(new SolidBrush(Color.Black), new Point[] { pph1, pph2, ck(Analoguevalue.Hour, b.Size, mnttal, 12, true) });

                        break;
                }
                if (fillhours) g.FillPie(new SolidBrush(Color.FromArgb(100, fillcolor)), new Rectangle(b.Width / 2 - hortal.Width, b.Height / 2 - hortal.Height, hortal.Width * 2, hortal.Height * 2), 270, fnc.divdec(fnc.divdec(360, 12), 60) * Analoguevalue.Minute + fnc.divdec(360, 12) * (Analoguevalue.Hour - (Analoguevalue.Hour > 12 ? 12 : 0)));

                #endregion
                #region center circle



                SizeF cntrsiz = new SizeF();
                cntrsiz.Height = fnc.divdectint(b.Height, 60) * (10 * centrsirclesize);
                cntrsiz.Width = fnc.divdectint(b.Width, 60) * (10 * centrsirclesize);

                SizeF cntrsizs = new SizeF();
                cntrsizs.Width = fnc.divdec(cntrsiz.Width, 100) * 70;
                cntrsizs.Height = fnc.divdec(cntrsiz.Height, 100) * 70;

                g.FillEllipse(new SolidBrush(cntr), b.Width / 2 - cntrsiz.Width / 2, b.Height / 2 - cntrsiz.Height / 2, cntrsiz.Width, cntrsiz.Height);
                g.FillEllipse(new SolidBrush(cntr2), b.Width / 2 - cntrsizs.Width / 2, b.Height / 2 - cntrsizs.Height / 2, cntrsizs.Width, cntrsizs.Height);

                #endregion
                #endregion
            }
            else
            {

                #region Digitail





                int apl = 255;

                g.FillRectangle(new SolidBrush(sharpbordercolor), allrec);
                g.FillRectangle(new SolidBrush(bordercolor), allrec.X + 20, allrec.Y + 20, allrec.Width - 40, allrec.Height - 40);
                g.FillRectangle(new SolidBrush(color1), allrec.X + 30, allrec.Y + 30, allrec.Width - 60, allrec.Height - 60);

                Size anlgsz = new Size((850 - (digitalspace.Count * 35)) / digitalvalue.Count, 800);
                int spc = 0;
                for (int i = 0; i <= digitalvalue.Count - 1; i++)
                {

                    for (int ii = 0; ii < digitalspace.Count; ii++)
                    {
                        if (digitalspace[ii] == i)
                        {
                            if (Analoguevalue.Second % 2 == 0 && ii == 1)
                            {
                                g.FillEllipse(new SolidBrush(Color.FromArgb(apl, digitalpointcolor)), 40 + i * (anlgsz.Width + 5) + spc + 4, 600, 30, 35);
                                g.FillEllipse(new SolidBrush(Color.FromArgb(apl, digitalpointcolor)), 40 + i * (anlgsz.Width + 5) + spc + 4, 300, 30, 35);


                            }
                            else if (Analoguevalue.Second % 59 != 0 && ii == 0)
                            {

                                g.FillEllipse(new SolidBrush(Color.FromArgb(apl, digitalpointcolor)), 40 + i * (anlgsz.Width + 5) + spc + 4, 600, 30, 35);
                                g.FillEllipse(new SolidBrush(Color.FromArgb(apl, digitalpointcolor)), 40 + i * (anlgsz.Width + 5) + spc + 4, 300, 30, 35);


                            }
                            spc += 35;
                            break;
                        }


                    }

                    drawints(digitalvalue[i], ref g, Color.FromArgb(255, analoguecolors[i]), Color.FromArgb(10, analoguecolors[i]), new Point(40 + i * (anlgsz.Width + 5) + spc, 100), anlgsz);
                }


                #endregion
            }
            #endregion

            e.Graphics.DrawImage(b, 0, 0, Width, Height);
        }
    }
    #endregion   
    public class wait:System.Windows.Forms.Control
        {
            Bitmap cb;
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
               
             if (start==false)
             {

                 incrs = false;
                 cb=GetBitmab(type);
                 incrs = true;
             }
             e.Graphics.Clear(BackColor);
                e.Graphics.DrawImage(cb,0,0,Width,Height);
            }
            bool incrs = true;
          public enum loaders { one, two, three }
          loaders type = loaders.one;
            [DefaultValue(loaders.one)]
          public loaders Type
          {
              get { return type; }
              set { type = value; if (start == true) { start = true; } Invalidate(); }
          }
          Thread tloader;
            private bool start = false;
            public bool Start
            {
                get { return start; }
                set {
                    start = value;
                    ParameterizedThreadStart f = new ParameterizedThreadStart(Loader);

                    if (value&&this.DesignMode==false)
                    {
                       tloader = new Thread(f);
                        tloader.Name = "loader";
                        tloader.Start(type);
                    }
                    else
                    {
                        if (tloader != null)
                        {
                            if (tloader.IsAlive)
                            {
                                tloader.Abort();

                            }
                        }
                    }
                }
            }
            Color color1 = Color.Red;

            public Color Color1
            {
                get { return color1; }
                set { color1 = value; Invalidate(); }
            }
            Bitmap GetBitmab(object mode)
            {
               
                Bitmap b = new Bitmap(1000,1000); Graphics g = Graphics.FromImage(b);
                if (incrs)
                {
                    for (int ff = 0; ff < pnts.Length; ff++)
                    {
                        if (pnts[ff] == num)
                        { pnts[ff] = 1; }
                        else
                        { pnts[ff] = pnts[ff] + 1; }
                    }
                }
                for (int i =0; i < num; i++)
                {
                    Double angle = 360 - ((360 / num * i) * (Math.PI / 180));
                    Pen p = new Pen(Color.Wheat, 30);// p.EndCap = LineCap.RoundAnchor; p.StartCap = LineCap.RoundAnchor;
                    PointF locf = new PointF(0F, 0F);
                    PointF locs = new PointF(0, 0);

                    locf.X = (float)(Math.Cos(angle) * (b.Width / 2 - 100) + b.Width / 2);
                    locf.Y = (float)(Math.Sin(angle) * (b.Height / 2 - 100) + b.Height / 2);
                    locs.X = (float)(Math.Cos(angle) * (b.Width / 2 - 300) + b.Width / 2);
                    locs.Y = (float)(Math.Sin(angle) * (b.Height / 2 - 300) + b.Height / 2);

                    float mt1 = 255;
                    float mt2 = num;
                    float mt = mt1 / num;
                    float tt = pnts[i];
                    float mm = mt * (tt);

                    p.Color = Color.FromArgb(255 - (int)mm, color1);

                    switch ((loaders)mode)
                    {
                        case loaders.one:



                            g.DrawLine(p, locs, locf);



                            break;
                        case loaders.two:
                            g.FillEllipse(new SolidBrush(p.Color), locf.X - 30, locf.Y - 30, (float)60, (float)60);

                            break;
                        case loaders.three:


                            g.FillPie(new SolidBrush(p.Color), 0, 0, 1000F, 1000F, (360 / num * i), 360 / num - 3);
                            g.FillEllipse(new SolidBrush(BackColor), 200, 200, 600, 600);
                            break;
                    }
                }
                return b;

            }
            public void Loader(object mode)
            {
               
                for (int n = 0; n <= 360; n += 8)
                {
                    if (n >= 360)
                        n = 0;
                    for (int ff = 0; ff < pnts.Length; ff++)
                    {
                        if (pnts[ff] == num)
                        { pnts[ff] = 1; }
                        else
                        { pnts[ff] = pnts[ff] + 1; }
                    }
                    if (this.IsDisposed==false&&this.IsHandleCreated==true)
                    {
                        try
                        {
                            this.Invoke(new Action(() => { cb = GetBitmab(mode); this.Invalidate(); }));
                        }
                        catch { tloader.Abort();}
                    }

                    Thread.Sleep(timeout);
                }



            }
            ~wait()
            {
                if (tloader != null)
                {
                    tloader.Abort();
                }
            } 
            public void Reset()
            {
                for (int ff = 0; ff < pnts.Length; ff++)
                {
                    pnts[ff] = ff + 1;
                }
            }
            int num = 30;
           [DefaultValue(30)]
            public int Num
            {
                get { return num; }
                set
                {
                    num = value; pnts = new int[num];

                    Reset();
                    Invalidate();
                }
            }
            int[] pnts;
            int timeout = 50;

            public int Timeout
            {
                get { return timeout; }
                set { timeout = value; }
            }
            public wait()
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |ControlStyles.SupportsTransparentBackColor| ControlStyles.ResizeRedraw, true);

                pnts = new int[num];
                for (int ff = 0; ff < pnts.Length; ff++)
                {
                    pnts[ff] = ff + 1;
                }
            }
        }
}