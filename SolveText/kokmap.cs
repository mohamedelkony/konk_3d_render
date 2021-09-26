using System.Runtime;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SolveText
{
    public struct ScanLineData
    {
        public int currentY;
        public float ndotla;
        public float ndotlb;
        public float ndotlc;
        public float ndotld;
    }
    public class KokMap
    {
        private PointF translatetransform = new PointF();

        public PointF TranslateTransform
        {
            get { return translatetransform; }
            set { translatetransform = value; }
        }

        public static KokMap FormFile(string addres)
        {
            return fnc.Selraliz.ReadFromBinaryFile<KokMap>(addres);
        }
        public void Save(string addres)
        {
            fnc.Selraliz.WriteToBinaryFile<KokMap>(addres, this);
        }
        public byte[] Buffer;
        public float[] DepthBuffer;
        public object[] lockBuffer;
        private Size size;
        public Size Size
        {
            get { return size; }
        }
        public KokMap(Size imgsize) : this(imgsize.Width, imgsize.Height) { }
        public KokMap(int width, int height)
        {
            size = new Size(width, height);
            Buffer = new byte[width * height * 4];
            DepthBuffer = new float[width * height];
            for (int index = 0; index < this.DepthBuffer.Length; index++)
            {
                DepthBuffer[index] = float.MaxValue;
            }
            lockBuffer = new object[width * height];
            for (var i = 0; i < lockBuffer.Length; i++)
            {
                lockBuffer[i] = new object();
            }
        }
        public void Clear(Color clr)
        { this.Clear(clr.R, clr.G, clr.B, clr.A); }
        public void Clear(byte r, byte g, byte b, byte a)
        {
            for (int index = 0; index < Buffer.Length; index += 4)
            {
                Buffer[index] = b;
                Buffer[index + 1] = g;
                Buffer[index + 2] = r;
                Buffer[index + 3] = a;
            }
            for (int index = 0; index < this.DepthBuffer.Length; index++)
            {
                DepthBuffer[index] = float.MaxValue;
            }
        }
        public Bitmap ToBitmap()
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            System.Runtime.InteropServices.Marshal.Copy(Buffer, 0, bmpData.Scan0, Buffer.Length);
            //Unlock the pixels
            bmp.UnlockBits(bmpData);
            //Return the bitmap 
            return bmp;
        }

        public void SetPixel(Color clr, Point pnt)
        { SetPixel(clr, pnt.X, pnt.Y); }
        public void SetPixel(Color clr, int x, int y)
        {
            x += (int)translatetransform.X;
            y += (int)translatetransform.Y;

            int index = (x + y * this.size.Width) * 4;
            if (x >= 0 && y >= 0 && x < this.size.Width && y < this.size.Height)
            {
                Buffer[index] = clr.B;
                Buffer[index + 1] = clr.G;
                Buffer[index + 2] = clr.R;
                Buffer[index + 3] = clr.A;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void SafeSetPixel(Color clr, Point p) { this.SafeSetPixel(clr, p.X, p.Y); }
        public void SafeSetPixel(Color clr, int x, int y)
        {
            x += (int)translatetransform.X;
            y += (int)translatetransform.Y;

            int index = (x + y * this.size.Width) * 4;
            if (x >= 0 && y >= 0 && x < this.size.Width && y < this.size.Height)
            {
                Buffer[index] = clr.B;
                Buffer[index + 1] = clr.G;
                Buffer[index + 2] = clr.R;
                Buffer[index + 3] = clr.A;
            }

        }

        public void SafeSetPixel(Color clr, int x, int y, float z)
        {
            x += (int)translatetransform.X;
            y += (int)translatetransform.Y;
            int index = (x + y * this.size.Width);
            int index4 = index * 4;
            //       lock (lockBuffer[index])
            {


                if (x >= 0 && y >= 0 && x < this.size.Width && y < this.size.Height)
                {
                    if (DepthBuffer[index] < z)
                    {
                        return;
                    }
                    DepthBuffer[index] = z;

                    Buffer[index4] = clr.B;
                    Buffer[index4 + 1] = clr.G;
                    Buffer[index4 + 2] = clr.R;
                    Buffer[index4 + 3] = clr.A;
                }
            }
        }

        public PixelFormat PixelFormat { get { return System.Drawing.Imaging.PixelFormat.Format32bppArgb; } }
        public Color GetPixel(Point pnt) { return this.GetPixel(pnt.X, pnt.Y); }
        public Color GetPixel(int x, int y)
        {
            int index = (x + y * this.size.Width) * 4;
            if (Buffer.Length >= index + 3)
            {
                throw new IndexOutOfRangeException("Pixel is out of size");
            }
            return Color.FromArgb(Buffer[index + 3], Buffer[index], Buffer[index + 1], Buffer[index + 2]);
        }
        //   int r = (int)(color.R * ndotl); int g = (int)(color.G * ndotl); int b = (int)(color.B * ndotl);
        //         if (r > 255) { r = 255; } else if (r < 0) { r = 0; }
        //         if (g > 255) { g = 255; } else if (g < 0) { g = 0; }
        //          if (b > 255) { b = 255; } else if (b < 0) { b = 0; }
        //           Color v = Color.FromArgb(r, g, b);
        public void DrawLine(Color clr,float f1, float f2,float f3,float f4)
        { this.DrawLine(clr, new PointF(f1, f2), new PointF(f3, f4)); }
        public void DrawLine(Color clr, PointF p1, PointF p2)
        {
            PointF[] pnts = KGrphs.Getlinepoints(p1, p2);
            for (int h = 0; h < pnts.Length; h++)
            {
                Point p = Point.Round(pnts[h]);
                this.SafeSetPixel(clr, p.X, p.Y);
            }
        }
        public void DrawLine(Color clr, Vector3 p1, Vector3 p2)
        {
            Vector3[] pnts = KGrphs.Getlinepoints(p1, p2);
            for (int h = 0; h < pnts.Length; h++)
            {
                Vector3 p = pnts[h];
                this.SafeSetPixel(clr, (int)p.X, (int)p.Y, p.Z);
            }
        }
        private float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        // Interpolating the value between 2 vertices 
        // min is the starting point, max the ending point
        // and gradient the % between the 2 points
        private float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }
        float ComputeNDotL(Vector3 vertex, Vector3 normal, Vector3 lightPosition)
        {
            Vector3 lightDirection = lightPosition - vertex;

            normal = normal.Normalized();
            lightDirection = lightDirection.Normalized();

            /*   Vector3 viewdirection = new Vector3(-1 * lightPosition.X, lightPosition.Y, 5000000);

               Vector3 halfvector = lightPosition + viewdirection;
               float spec = Vector3.DotProduct(normal, halfvector.Normalized());
      
       

               float specular = (float)Math.Max(0, Math.Pow(spec, 50f));
             */
            return Math.Max(0, Vector3.DotProduct(normal, lightDirection));
        }

        void ProcessScanLine(ScanLineData data, Vertex va, Vertex vb, Vertex vc, Vertex vd, Color color)
        {
             Vector3 pa = va.Coordinates;
          Vector3 pb = vb.Coordinates;
          Vector3 pc = vc.Coordinates;
          Vector3 pd = vd.Coordinates;

          // Thanks to current Y, we can compute the gradient to compute others values like
          // the starting X (sx) and ending X (ex) to draw between
          // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
          var gradient1 = pa.Y != pb.Y ? (data.currentY - pa.Y) / (pb.Y - pa.Y) : 1;
          var gradient2 = pc.Y != pd.Y ? (data.currentY - pc.Y) / (pd.Y - pc.Y) : 1;

          int sx = (int)Interpolate(pa.X, pb.X, gradient1);
          int ex = (int)Interpolate(pc.X, pd.X, gradient2);

          // starting Z & ending Z
          float z1 = Interpolate(pa.Z, pb.Z, gradient1);
          float z2 = Interpolate(pc.Z, pd.Z, gradient2);
       
          var snl = Interpolate(data.ndotla, data.ndotlb, gradient1);
          var enl = Interpolate(data.ndotlc, data.ndotld, gradient2);

          // drawing a line from left (sx) to right (ex) 
          for (var x = sx; x < ex; x++)
          {
              float gradient = (x - sx) / (float)(ex - sx);

              var z = Interpolate(z1, z2, gradient);
              var ndotl = Interpolate(snl, enl, gradient);
      
              // changing the color value using the cosine of the angle
              // between the light vector and the normal vector
         int r=(int)(color.R * ndotl );int g= (int)(color.G * ndotl );int b=(int) (color.B * ndotl );
         if (r > 255) { r = 255; } else if (r < 0) { r = 0; }
              if (g > 255) { g = 255; } else if (g< 0) { g = 0; } 
              if (b > 255) { b = 255; } else if (b < 0) { b = 0; }
              Color v = Color.FromArgb(r, g, b);
              this.SafeSetPixel(v,x, data.currentY, z);
             
          }
      }
        

        public void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Color color)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (v1.Coordinates.Y > v2.Coordinates.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            if (v2.Coordinates.Y > v3.Coordinates.Y)
            {
                var temp = v2;
                v2 = v3;
                v3 = temp;
            }

            if (v1.Coordinates.Y > v2.Coordinates.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            Vector3 p1 = v1.Coordinates;
            Vector3 p2 = v2.Coordinates;
            Vector3 p3 = v3.Coordinates;

            // Light position 
            Vector3 lightPos = new Vector3(0, 0, 0);
            if ((v1.Normal + v2.Normal + v3.Normal).Z / 3f > 0f)
            {
                lightPos.Z = 5000;
            }
            else if ((v1.Normal + v2.Normal + v3.Normal).Z / 3f < 0f)
            {
                lightPos.Z = -5000;

            }
            // computing the cos of the angle between the light vector and the normal vector
            // it will return a value between 0 and 1 that will be used as the intensity of the color
            float nl1 = ComputeNDotL(v1.Coordinates, v1.Normal, lightPos);
            float nl2 = ComputeNDotL(v2.Coordinates, v2.Normal, lightPos);
            float nl3 = ComputeNDotL(v3.Coordinates, v3.Normal, lightPos);

            var data = new ScanLineData { };

            // computing lines' directions
            float dP1P2 = 0, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing slopes
            bool right = false;
            if (p2.Y - p1.Y > 0)
            {
                dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            }
            else if (p2.X > p1.X)

            { right = true; }



            if (p3.Y - p1.Y > 0)
                dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
            else
                dP1P3 = 0;

            if (right || dP1P2 > dP1P3)
            {
                for (int y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.currentY = y;

                    if (y < p2.Y)
                    {
                        data.ndotla = nl1;
                        data.ndotlb = nl3;
                        data.ndotlc = nl1;
                        data.ndotld = nl2;
                        ProcessScanLine(data, v1, v3, v1, v2, color);
                    }
                    else
                    {
                        data.ndotla = nl1;
                        data.ndotlb = nl3;
                        data.ndotlc = nl2;
                        data.ndotld = nl3;
                        ProcessScanLine(data, v1, v3, v2, v3, color);
                    }
                }
            }
            else
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.currentY = y;

                    if (y < p2.Y)
                    {
                        data.ndotla = nl1;
                        data.ndotlb = nl2;
                        data.ndotlc = nl1;
                        data.ndotld = nl3;
                        ProcessScanLine(data, v1, v2, v1, v3, color);
                    }
                    else
                    {
                        data.ndotla = nl2;
                        data.ndotlb = nl3;
                        data.ndotlc = nl1;
                        data.ndotld = nl3;
                        ProcessScanLine(data, v2, v3, v1, v3, color);
                    }
                }
            }
        }
    }
}
