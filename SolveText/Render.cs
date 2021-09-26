using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace SolveText
{
    public class Render
    {
       public bool drawinfo = true;
        public int drawboxplans = 1;
        public bool drawbox = false;
        public int drawaxes = 1;
        public bool shadow = true;
        public bool backface = false;
        public Color backfacecolor = Color.Blue;
        public bool frontface = false;
        public Color frontfacecolor = Color.Red;
        public bool lightray = false;
        public bool eyeray = false;
        public bool normalvector = false;
        public bool reflectedray = false;
        public bool lines = true;
        public bool colorlines = false;
        public bool fillplans = true;
        public bool points = false;
        public Vector3 lightposition = new Vector3(-100, -100, 0);
        public float diffuse = 0.8f;
        public float specular = 1f;
        public PointF plandomain = new PointF(0, 1);
        public PointF griddomain = new PointF(0, 1);
        public bool highlight_selectedpoints = false;
    
        public bool Dark_Render_engine = true;
        public Render()
        {

        }
    }
  
}
