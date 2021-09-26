using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace SolveText
{
    public static class Rotate3D
    {
        public static void RotateVector(Axes axs, float angel, Vector3 vector)
        {

            float ang =  fnc.Topi(angel);
            float sin_t = (float)Math.Sin(ang);
            float cos_t = (float)Math.Cos(ang);
            float v1 = 0;
            float v2 = 0;
            switch (axs)
            {
                case Axes.X:
                    v1 = vector.Y;
                    v2 = vector.Z;
                    vector.Y = v1 * cos_t - v2 * sin_t;
                    vector.Z = v2 * cos_t + v1 * sin_t;
                    break;
                case Axes.Y:
                    v1 = vector.X;
                    v2 = vector.Z;
                    vector.X = v1 * cos_t - v2 * sin_t;
                    vector.Z = v2 * cos_t + v1 * sin_t;
                    break;
                case Axes.Z:
                    v1 = vector.X;
                    v2 = vector.Y;
                    vector.X = v1 * cos_t - v2 * sin_t;
                    vector.Y = v2 * cos_t + v1 * sin_t;
                    break;
            }

        }
        public static Vector3 RotateVector_Clone(Axes axs, float angel, Vector3 vector)
        {
            float ang = fnc.Topi(angel);
            float sin_t = (float)Math.Sin(ang);
            float cos_t = (float)Math.Cos(ang);
            float v1 = 0;
            float v2 = 0;
            Vector3 value = vector.Clone();
            switch (axs)
            {
                case Axes.X:
                    v1 = vector.Y;
                    v2 = vector.Z;
                    value.Y = v1 * cos_t - v2 * sin_t;
                    value.Z = v2 * cos_t + v1 * sin_t;
                    break;
                case Axes.Y:
                    v1 = vector.X;
                    v2 = vector.Z;
                    value.X = v1 * cos_t - v2 * sin_t;
                    value.Z = v2 * cos_t + v1 * sin_t;
                    break;
                case Axes.Z:
                    v1 = vector.X;
                    v2 = vector.Y;
                    value.X = v1 * cos_t - v2 * sin_t;
                    value.Y = v2 * cos_t + v1 * sin_t;
                    break;
            }
            return value;
        }
        public static void RotateVectors(Axes axs, float angel, Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                Rotate3D.RotateVector(axs, angel, vectors[i]);
            }
        }
        public static Vector3[] RotateVectors_clone(Axes axs, float angel, Vector3[] vectors)
        {
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                value[i] = Rotate3D.RotateVector_Clone(axs, angel, vectors[i]);
            }
            return value;
        }
        public static RectangleF3D RotateVectors_inform(Axes axs, float angel, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = vectors[i];
                Rotate3D.RotateVector(axs, angel, vec);

                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }

            }
            return new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
        }
        public static object[] RotateVectors_inform_clone(Axes axs, float angel, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = Rotate3D.RotateVector_Clone(axs, angel, vectors[i]);
                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }

            }
            RectangleF3D rec = new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
            return new object[2] { value, rec };
        }

        public static void RotateVector(PointF3D angels, Vector3 vector)
        {
            RotateVector(Axes.X, angels.X, vector);
            RotateVector(Axes.Y, angels.Y, vector);
            RotateVector(Axes.Z, angels.Z, vector);
        }
        public static Vector3 RotateVector_Clone(PointF3D angels, Vector3 vector)
        {
            Vector3 vec;
            vec = RotateVector_Clone(Axes.X, angels.X, vector);
            vec = RotateVector_Clone(Axes.Y, angels.Y, vec);
            vec = RotateVector_Clone(Axes.Z, angels.Z, vec);
            return vec;
        }
        public static void RotateVectors(PointF3D angels, Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                Rotate3D.RotateVector(angels, vectors[i]);
            }
        }
        public static Vector3[] RotateVectors_clone(PointF3D angles, Vector3[] vectors)
        {
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                value[i] = Rotate3D.RotateVector_Clone(angles, vectors[i]);
            }
            return value;
        }
        public static RectangleF3D RotateVectors_inform(PointF3D angels, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = vectors[i];
                Rotate3D.RotateVector(angels, vec);

                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }

            }
            return new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
        }
        public static object[] RotateVectors_inform_clone(PointF3D angles, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = Rotate3D.RotateVector_Clone(angles, vectors[i]);
                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }
            }
            RectangleF3D rec = new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
            return new object[2] { value, rec };
        }

        public static void RotateVector(PointF angel, Vector3 vector)
        {
            RotateVector(Axes.X, angel.Y, vector);
            RotateVector(Axes.Y, angel.X, vector);

        }
        public static Vector3 RotateVector_Clone(PointF angel, Vector3 vector)
        {
            Vector3 vec;
            vec = RotateVector_Clone(Axes.X, angel.Y, vector);
            vec = RotateVector_Clone(Axes.Y, angel.X, vec);

            return vec;
        }
        public static void RotateVectors(PointF angel, Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                Rotate3D.RotateVector(angel, vectors[i]);
            }
        }
        public static Vector3[] RotateVectors_clone(PointF angle, Vector3[] vectors)
        {
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                value[i] = Rotate3D.RotateVector_Clone(angle, vectors[i]);
            }
            return value;
        }
        public static RectangleF3D RotateVectors_inform(PointF angel, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = vectors[i];
                Rotate3D.RotateVector(angel, vec);

                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }

            }
            return new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
        }
        public static object[] RotateVectors_inform_clone(PointF angle, Vector3[] vectors)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();
            Vector3[] value = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vec = Rotate3D.RotateVector_Clone(angle, vectors[i]);
                if (vec.X > max_pnt.X)
                { max_pnt.X = vec.X; }
                else if (vec.X < min_pnt.X)
                { min_pnt.X = vec.X; }

                if (vec.Y > max_pnt.Y)
                { max_pnt.Y = vec.Y; }
                else if (vec.Y < min_pnt.Y)
                { min_pnt.Y = vec.Y; }

                if (vec.Z > max_pnt.Z)
                { max_pnt.Z = vec.Z; }
                else if (vec.Z < min_pnt.Z)
                { min_pnt.Z = vec.Z; }
            }
            RectangleF3D rec = new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
            return new object[2] { value, rec };
        }

        public static RectangleF3D RotateFaces_inform(PointF angel, Face[][] Faces)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();
            Vector3 vec = new Vector3();
            List<Vector3> ved = new List<Vector3>();
            for (int i = 0; i < Faces.Length; i++)
            {
                for (int n = 0; n < Faces[i].Length; n++)
                {

                    Face f = Faces[i][n];

                    for (int v = 0; v < 4;v++)
                    {
                        if (v == 0) { vec = f.TopLeft; }else if (v == 1) { vec = f.TopRight; }
                         else if (v == 2) { vec = f.BottomRight; }else { vec = f.BottomLeft; }                 
                   
                        Rotate3D.RotateVector(angel, vec);
                  
                        if (vec.X > max_pnt.X){ max_pnt.X = vec.X; }else if (vec.X < min_pnt.X){ min_pnt.X = vec.X; }
                    if (vec.Y > max_pnt.Y){ max_pnt.Y = vec.Y; }else if (vec.Y < min_pnt.Y){ min_pnt.Y = vec.Y; }
                    if (vec.Z > max_pnt.Z){ max_pnt.Z = vec.Z; }else if (vec.Z < min_pnt.Z){ min_pnt.Z = vec.Z; }
                   
                    }               
                }
            }
            return new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
        }
        public static RectangleF3D RotateFaces_inform(PointF3D angel, Face[][] Faces)
        {
            PointF3D max_pnt = new PointF3D();
            PointF3D min_pnt = new PointF3D();
            Vector3 vec = new Vector3();
            List<Vector3> ved = new List<Vector3>();
            for (int i = 0; i < Faces.Length; i++)
            {
                for (int n = 0; n < Faces[i].Length; n++)
                {

                    Face f = Faces[i][n];

                    for (int v = 0; v < 4; v++)
                    {
                        if (v == 0) { vec = f.TopLeft; }
                        else if (v == 1) { vec = f.TopRight; }
                        else if (v == 2) { vec = f.BottomRight; } else { vec = f.BottomLeft; }

                        Rotate3D.RotateVector(angel, vec);

                        if (vec.X > max_pnt.X) { max_pnt.X = vec.X; } else if (vec.X < min_pnt.X) { min_pnt.X = vec.X; }
                        if (vec.Y > max_pnt.Y) { max_pnt.Y = vec.Y; } else if (vec.Y < min_pnt.Y) { min_pnt.Y = vec.Y; }
                        if (vec.Z > max_pnt.Z) { max_pnt.Z = vec.Z; } else if (vec.Z < min_pnt.Z) { min_pnt.Z = vec.Z; }

                    }
                }
            }
            return new RectangleF3D(min_pnt.X, min_pnt.Y, min_pnt.Z, max_pnt.X - min_pnt.X, max_pnt.Y - min_pnt.Y, max_pnt.Z - min_pnt.Z);
        }
    }
}
