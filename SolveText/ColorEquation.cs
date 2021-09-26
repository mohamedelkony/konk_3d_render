using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolveText
{
    [Serializable()]
    public class ColorEquation
    {
        public string Name = "";
        public string Red = "100";
        public string Green = "100";
        public string Blue = "100"; 
        public string Alpha = "100";
        public ColorEquation()
        {

        }
        public ColorEquation(string red, string green, string blue)
        {
            this.Red = red; this.Green = green; this.Blue = blue;
        }
        public ColorEquation(string red, string green, string blue,string alpha)
        {
            this.Red = red; this.Green = green; this.Blue = blue; this.Alpha=alpha;
        }
      
        public ColorEquation(string name, string red, string green, string blue,string alpha)
        {
            this.Red = red; this.Green = green; this.Blue = blue; this.Name = name; this.Alpha = alpha;
        }
        public override string ToString()
        {
            return string.Format("R={0},G={1},B={2}", Red, Green, Blue);
        }
    }
}
