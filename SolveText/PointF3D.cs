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
namespace SolveText
{
    [Serializable()]
    public class Vector3 : IComparable
    {
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public Color color = Color.White;
        public bool ismodelwall = false;
        public bool isdeleted = false;
      public   PointF uv = new PointF();
      
        public int CompareTo(Object other)
        {
            Vector3 oth = other as Vector3;
            int res = 0;

            res = this.Z.CompareTo(oth.Z);

            return res;
        }
        public Vector3(double x, double y, double z)
        {
            this.X = (float)x; this.Y = (float)y; this.Z = (float)z;
        }
        public Vector3(float x, float y, float z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }
        public Vector3()
        {

        }
        public Vector3 Clone()
        { 
            return (Vector3)this.MemberwiseClone(); }
      /*  public Vector3D Multiply(Vector3D pnt)
        {
            return this.Multiply(pnt.X, pnt.Y, pnt.Z);
        }
        public Vector3D Multiply(float x, float y, float z)
        {
            return new Vector3D(this.X + x, this.Y +y, this.Z +z);  
        }*/
        public Vector3 Subtract(float x, float y, float z)
        {
            return new Vector3(this.X - x, this.Y - y, this.Z - z);
        }
        public Vector3 Subtract(Vector3 v)
        {
            return new Vector3(this.X - v.X, this.Y - v.Y, this.Z - v.Z);
        }
        public float Getlenght()
        {
            return (float)(Math.Sqrt(X * X + Y * Y + Z * Z));
        }
        public Vector3 Normalized()
        {
            float length = this.Getlenght();
            if (length==0)
            { length = 1; }
            return new Vector3(this.X / length, this.Y / length, this.Z / length);
        }
        public Vector3 HitBy(Vector3 p)
        {
            return HitBy(p.X, p.Y, p.Z);
        }
        public Vector3 HitBy(float x,float y,float z)
        {
            return new Vector3(X * x, Y * y, Z * z);
        }
        public Vector3 HitBy(float scale)
        {
            return HitBy(scale, scale, scale);
        }
        public PointF TOPointF()
        {
            return new PointF(this.X, this.Y);
        }
        public PointF3D TOPointF3SS()
        {
            return new PointF3D(this.X, this.Y, this.Z);
        }
        public override string ToString()
        {
            return string.Format("(X={0},Y={1},Z={2})", X, Y, Z);
        }
        public static Vector3 CrossProduct(Vector3 p1,Vector3 p2)
        {
            Vector3 cross = new Vector3();

            cross.X = p1.Y * p2.Z - p1.Z * p2.Y;

            cross.Y = p1.Z * p2.X - p1.X * p2.Z;

            cross.Z = p1.X * p2.Y - p1.Y * p2.X;
        
            return cross;
        }
        public static float DotProduct(Vector3 p1, Vector3 p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;

        }
 
        public static Vector3 operator+(Vector3 v1,Vector3 v2)
        {
            return  new Vector3(v1.X+v2.X,v1.Y+v2.Y,v1.Z+v2.Z);       
          
        }
        public static Vector3 operator+(Vector3 v1, float  v2)
        {
            return new Vector3(v1.X + v2, v1.Y + v2, v1.Z + v2);

        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
             return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3 operator -(Vector3 v1, float v2)
        {
            return new Vector3(v1.X - v2, v1.Y - v2, v1.Z - v2);
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static Vector3 operator *(Vector3 v1,float v)
        {
            return new Vector3(v1.X * v, v1.Y * v, v1.Z * v);
        }
        public static Vector3 operator *( float v,Vector3 v1)
        {
            return new Vector3(v1.X * v, v1.Y * v, v1.Z * v);
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }
        public static Vector3 operator /(Vector3 v1, float v)
        {
            return new Vector3(v1.X / v, v1.Y / v, v1.Z / v);
        }
        public static Vector3 operator ^(Vector3 v1, Vector3 v2)
        {
            return new Vector3((float)Math.Pow(v1.X, v2.X),(float) Math.Pow(v1.Y , v2.Y),(float)Math.Pow( v1.Z , v2.Z));
        }
        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z;
        }
        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1.X>v2.X&&v1.Y>v2.Y&&v1.Z>v2.Z;
        }
        public bool IsSameLocation(Vector3 v)
        { return IsSameLocation(this, v); }
        public static  bool IsSameLocation(Vector3 v1,Vector3 v2)
        {
            bool xs = (v1.X + 4 > v2.X) && (v1.X - 4 < v2.X);
            bool ys = (v1.Y + 4 > v2.Y) && (v1.Y - 4 < v2.Y);
            bool zs = (v1.Z + 4 > v2.Z) && (v1.Z - 4 < v2.Z);
            return (xs && ys && zs); 
        }
        /*
       public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return ((int)v1.X == (int)v2.X &&(int) v1.Y ==(int) v2.Y &&(int) v1.Z ==(int) v2.Z);
        }
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1== v2);
        }
       */
        /// <summary>
        /// reurns 2*v1*(v1.v2)-v2
        /// </summary>
        /// <param name="v1">normal vector</param>
        /// <param name="v2">light vector</param>
        /// <returns></returns>
        public static Vector3 Reflect(Vector3 v1,Vector3 v2)
        {
     //r=2*normal*(normal.light)-light
       
            Vector3 r = new Vector3();
            float dot = Vector3.DotProduct(v1, v2);
            r = (2 * v1 * dot);
            r=r-v2;
            return r;
        }
        public  Vector3 Pitch( float degree)
        {
            double x = this.X;
            double y = (this.Y * Math.Cos(degree)) - (this.Z * Math.Sin(degree));
            double z = (this.Y * Math.Sin(degree)) + (this.Z * Math.Cos(degree));
            return new Vector3(x, y, z);
        }
        public  Vector3 Yaw( float degree)
        {
            double x = (this.Z * Math.Sin(degree)) + (this.X * Math.Cos(degree));
            double y = this.Y;
            double z = (this.Z * Math.Cos(degree)) - (this.X * Math.Sin(degree));
            return new Vector3(x, y, z);
        }
        public  Vector3 Roll( double degree)
        {
            double x = (this.X * Math.Cos(degree)) - (this.Y * Math.Sin(degree));
            double y = (this.X * Math.Sin(degree)) + (this.Y * Math.Cos(degree));
            double z = this.Z;
            return new Vector3(x, y, z);
        }
        public static Vector3 GetCenterVect(Vector3[] vecs)
        {
            RectangleF3D rec = KGrphs.Recof3dpnts(vecs);
            return new Vector3(rec.X + rec.Width / 2f, rec.Y + rec.Height / 2f, rec.Z + rec.Thick / 2f);
        }
        public static Vector3 operator +(Vector3 v1, SizeF3D v2)
        {
            return new Vector3(v1.X + v2.Width, v1.Y + v2.Height, v1.Z + v2.Thick);
        }
        public static Vector3 operator -(Vector3  v1, SizeF3D v2)
        {
            return new Vector3(v1.X - v2.Width, v1.Y - v2.Height, v1.Z - v2.Thick);
        }
    }
    public struct Vertex
    {
        public Vector3 Normal;
        public Vector3 Coordinates;
        public Vertex(Vector3 cord,Vector3 norm)
        {
            this.Coordinates = cord;
            this.Normal = norm;
        }
    }
   
    public struct Color4
    {
        public float R ;
        public float G ;
        public float B ;
        public float A ;
        public static explicit operator Color(Color4 c)
        {
            Color4 s = MakeColorSafe(c);
            return Color.FromArgb((int)s.A,(int)s.R,(int)s.G,(int)s.B);
        }
        public Color ToColor(Color4 c)
        {
            Color4 s = MakeColorSafe(c);
            return Color.FromArgb((int)s.A, (int)s.R, (int)s.G, (int)s.B);
        }
        public static Color4 FormColor(Color c)
        {
            return new Color4(c.A, c.R, c.G, c.B);
        }
        public Color4(float a, float r,float g,float b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }
        public static Color4 operator+(Color4 left, int right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R + right;
            g = left.G + right;
            b = left.B + right;
            a = left.A + right;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator -(Color4 left, int right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R - right;
            g = left.G - right;
            b = left.B - right;
            a = left.A - right;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator*(Color4 left, int right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R * right;
            g = left.G * right;
            b = left.B * right;
            a = left.A * right;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator/(Color4 left, float right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R / right;
            g = left.G / right;
            b = left.B / right;           
                a = left.A;
            return new Color4(a, r, g, b);
        }
        public static  explicit operator Color4 (Color c)
        {
           
            return Color4.FormColor(c);
        }
        public static Color4 operator+(Color4 left, Color4 right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R + right.R;
            g = left.G + right.G;
            b = left.B + right.B;
            a = left.A + right.A;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator-(Color4 left, Color4 right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R - right.R;
            g = left.G - right.G;
            b = left.B - right.B;
            a = left.A - right.A;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator*(Color4 left, Color4 right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R * right.R;
            g = left.G * right.G;
            b = left.B * right.B;
            a = left.A * right.A;
            return new Color4(a, r, g, b);
        }
        public static Color4 operator/(Color4 left, Color4 right)
        {
            float r = 0; float a = 0; float g = 0; float b = 0;
            r = left.R / right.R;
            g = left.G / right.G;
            b = left.B / right.B;
            a = left.A / right.A;
            return new Color4(a, r, g, b);
        }
        public static Color4 MakeColorSafe(float a, float r, float g, float b)
        {
            float fcr = r; float fca = a; float fcg = g; float fcb = b;
            if (fcr < 0) { fcr = 0; } else if (fcr > 255) { fcr = 255; }
            if (fcg < 0) { fcg = 0; } else if (fcg > 255) { fcg = 255; }
            if (fcb < 0) { fcb = 0; } else if (fcb > 255) { fcb = 255; }
            if (fca < 0) { fca = 0; } else if (fca > 255) { fca = 255; }
            return new Color4((int)fca, (int)fcr, (int)fcg, (int)fcb);
        }
        public static Color4 MakeColorSafe(Color4 c)
        {
            return MakeColorSafe(c.A, c.R, c.G, c.B);
        }
        public void MakeSafe()
        {
            this = MakeColorSafe(this);
        }
    }
}
