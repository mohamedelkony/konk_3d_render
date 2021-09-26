using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SolveText;

namespace SolveText
{
    class EquationSolver
    {
     
        public   static string[] defends = new string[] { "^", "*", "/", "+", "-",">","<" };
        public   static string[][] Marks = new string[7][]
                {                     new string[] { "sqrt", "asin", "acos", "atan", "cosh", "sinh", "tanh" ,"cos", "sin", "tan",  "log", "abs", "exp", "sign","floor","ln","ceil","√" }
                   , new string[] { "|" }
                   , new string[] { "(", ")" }
                   ,new string[]{">","<"}
                   ,new string[] { "^" }
                   ,new string[] { "*", "/" }
                   ,new string[] { "+", "-" }
                };
        public static string[] fncs = new string[] { "sqrt", "asin", "acos", "atan", "cosh", "sinh", "tanh", "cos", "sin", "tan", "log", "abs", "exp", "sign", "floor", "ln", "ceil", "√" };
            string[] abs = new string[] { "|" };
            string[] bracks = new string[] { "(", ")" };
            string[] powers = new string[] { "^" };
            string[] mltply = new string[] { "*", "/" };
            string[] mins = new string[] { "+", "-" };
                 static string[] nums = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
       
        static float Topi(float n)
            {
                return (float)((n) * (Math.PI / 180));
            }
        static float Topi(float n,bool flt)
        {
            if (flt)
            {
                return (float)((n) * ( 3.14159f/ 180));
            }
            return (float)((n) * (Math.PI / 180));
        }
         static float SolveMark(string mark, float v1, float v2, bool isdeg)
            {
                float value = 0;               
                mark = mark.ToString();
                if (mark == "^")
                {
              value=(float) Math.Pow(v1, v2);
              if (float.IsNaN(value)||float.IsInfinity(value))
              {
                  errortext = v1 + "^" + v2;
                  throw new MathException("Can't Get Power of "+v1+" to"+ v2+" at " + errortext);
              }
                    return value;
                }
                else if (mark == "*")
                {
                    value = v1 * v2; return value;
                }
                else if (mark == ">")
                {
                    value = v1 > v2?v1:v2; return value;
                }
                else if (mark == "<")
                {
                    value = v1 <v2 ? v1 : v2; return value;
                }
                else if (mark == "/")
                {
                    if (v2==0)
                    {
                        errortext = v1 + "/" + 0;
                        throw new MathException("Can't divide by zero at " + errortext);
                    }
                    value = v1 / v2; return value;
                }
                else if (mark == "+")
                { value = v1 + v2; return value; }
                else if (mark == "-")
                { value = v1 - v2; return value; }
            else if (mark == "cos")
            {
                float vf = v1;
                if (isdeg)
                {
                    if (v1 == 90)
                    { return 0; }
                    vf = Topi(v1, true);

                }
                value = (float)Math.Cos(vf);
                return value;
            }
            else if (mark == "sin")
            {
                float vf = v1;
                if (isdeg)
                { vf = Topi(v1, true); }
                value = (float)Math.Sin(vf);
                return value;
            }
            else if (mark == "tan")
            {
                float vf = v1;
                if (isdeg)
                { vf = fnc.Topi(v1); }
                value = (float)Math.Tan(vf);
                return value;
            }
            else if (mark == "sqrt" || mark == "√")
                {
                    if (v1<0)
                    {
                        errortext ="sqrt("+v1+")";
                        throw new MathException("Can't Get Square root of mines number at " + errortext);                 
                    }
                    value = (float)Math.Sqrt(v1); return value;
                }
                else if (mark=="ln")
                {
                    if (v1<=0)
                    {
                        errortext = mark+"(" + v1 + ")";
                        throw new MathException("Can't Get ln of number less than or equalt to 0 at " + errortext);                               
                    }
                    double e = 2.718281828459;                   
                    value = (float)(Math.Log(v1, e)); return value;
                }
                else if (mark=="floor")
                {
                    value = (float)(Math.Floor(v1)); return value;
                }
                else if (mark=="ceil")
                {
                    value = (float)(Math.Ceiling(v1)); return value;
                }
                else if (mark=="exp")
                {
                    value = (float)(Math.Exp(v1));
                    if (float.IsNaN(value)||float.IsInfinity(value)) throw new MathException("Math Error panned operation"); return value;
                }
                else if (mark=="sign")
                {
                    value = (float)(Math.Sign(v1)); return value;
                }
                else if (mark=="abs")
                {
                    value = (float)(Math.Abs(v1)); return value;
                }

                else if (mark == "asin")
                {
                    if (v1 <= 1 && v1 >= -1)
                    {
                        value = (float)(Math.Asin(v1));
                        if (isdeg)
                        {
                            value = (float)(value * (180 / Math.PI));
                        }
                        return value;
                    }
                    else
                    {
                        errortext = "asin(" + v1 + ")";
                        throw new MathException("Can't Get asin(sin^-1) of  number less than -1 or bigger than 1 at " + errortext);  
                    }
                }
                else if (mark == "sinh")
                {
                    value = (float)(Math.Sinh(v1));
                    if (isdeg)
                    {
                        value = (float)(value * (180 / Math.PI));
                    }
                    return value;
                }
                else if (mark == "cosh")
                {
                    value = (float)(Math.Cosh(v1));

                    if (isdeg)
                    {
                        value = (float)(value * (180 / Math.PI));
                    }
                    return value;
                }
                else if (mark == "tanh")
                {
                    value = (float)(Math.Tanh(v1));
                    if (isdeg)
                    {
                        value = (float)(value * (180 / Math.PI));
                    }
                    return value;
                }
                else if (mark == "acos")
                { if (v1 <= 1 && v1 >= -1)
                    {
                    value = (float)(Math.Acos(v1));
                    if (isdeg)
                    {
                        value = (float)(value * (180 / Math.PI));
                    }
                    return value;
                    }
                else
                {
                    errortext = mark+"(" + v1 + ")";
                    throw new MathException("Can't Get acos(cos^-1) of  number less than -1 or bigger than 1 at " + errortext);
                }
                }
                else if (mark == "atan")
                {
                    value = (float)(Math.Atan(v1));
                    if (isdeg)
                    {
                        value = (float)(value * (180 / Math.PI));
                    }
                    return value;
                }
                
                else if (mark == "log")
                {
                    if (v1<=0)
                    {
                        errortext = mark + "(" + v1 + ")";
                        throw new MathException("Can't Get Log10 of number less than or equal to 0 at " + errortext);              
                    }
                    value = (float)Math.Log10(v1);
                    return value;
                }          
                throw new Exception("UnDefiend Polygen '" + mark + "'");             
            }

         bool isdegreee = true;

         public bool Isdegreee
         {
             get { return isdegreee; }
             set { isdegreee = value; }
         }
         static List<string> steps = new List<string>();
        public List<string> Steps
          { get { return steps; } }
        public static float NumricE(float v)
        {
         
            string s = v.ToString().ToLower();
            if (s.Contains("e-"))
            {
                return 0;
            }
            else if(s.Contains("e+"))
            {
                return int.MaxValue-1;
            }
            if (float.IsInfinity(v))
            { return int.MaxValue-1; }
            return v;
        }
        public  static bool isalphapet(char c)
        {
            for (int i='a';i<'z';i++)
            {
                if (((Char)i)==c)
                {
                 return true;
                }
            }
            return false;
        }
      public  static string Replacemathtext(string maintext,string vartext,string  varvalue)
         {
            if (vartext.Length>1)
            {
            return    maintext = maintext.Replace(vartext, varvalue);
            }
             bool replc = false;
            vartext=vartext.ToLower();
            for (int i = 0; i < maintext.Length; i++)
            {
                replc = true;
                if (maintext[i].ToString() == vartext)
                {           
                  for (int w=0;w<fncs.Length;w++)
                    {
                        if (fncs[w].Contains(vartext))
                        {
                            string fnc = fncs[w];
                            int fnclenght = fnc.Length;
                            int varindxinfnc = fnc.IndexOf(vartext);
                            if (maintext.Length > i - varindxinfnc + fnclenght&&i-varindxinfnc>=0)
                            {
                                if (maintext.Substring(i - varindxinfnc, fnclenght) == fnc)
                                {
                                    replc = false;
                                }
                            }
                        }
                    }
                    if (replc)
                    {
                      
                        maintext = maintext.Remove(i, 1);
                    maintext=    maintext.Insert(i,varvalue);
                    }
                }
            }
           
            return maintext;
         }

        public string ErrorText { get { return errortext; } }
        public int ErrorIndex { get { return errorindex; } }
 
        public static float SolveEQ(string s,  bool isdeg)
        {
            s = s.ToLower();
           while (s.Contains(" "))
             {
             s = s.Replace(" ", "");
            }  while (s.Contains("pi"))
            {
                s = s.Replace("pi", isdeg ? "180" : "3.1415");
            }
           
            return SolveEQCode(s, isdeg);
        }
        public static float SolveEQ(string s, string varslog, float varvalue, bool isdeg)
        {
            s = s.ToLower();
           
            if (varslog != null)
            {


                s =  Replacemathtext(s, varslog, varvalue.ToString());
            }
            return SolveEQ(s, isdeg);
        }
        public static float SolveEQ(string s, string[] varslog, float[] varvalue, bool isdeg,bool resrvereplace=false)
        {
            s = s.ToLower();
            if (resrvereplace)
            {
                for (int i = varslog.Length - 1; i >= 0; i--)
                {
                    s = Replacemathtext(s, varslog[i], varvalue[i].ToString());
                }
            }
            else
            {
                for (int i = 0; i < varslog.Length; i++)
                {
                    s = Replacemathtext(s, varslog[i], varvalue[i].ToString());
                }
            }
            return SolveEQ(s, isdeg);
        }
 
        public float SolveEquation(string s)
        {
           
            return this.SolveEquation(s, this.isdegreee);
        }
        public float SolveEquation(string s, bool isdeg)
        {
            s = s.ToLower();
            while (s.Contains(" "))
            {
                s = s.Replace(" ", "");
            } while (s.Contains("pi"))
            {
                s = s.Replace("pi", isdeg ? "180" : "3.1415");
            }
            steps.Clear(); errortext = "";
            errorindex = -1;
            return EquationSolver.SolveEQCode(s, isdeg);
        }
        public float SolveEquation(string s, string[] varslog, float[] varvalue, bool resrvereplace = false)
        {
            s = s.ToLower();
            if (resrvereplace)
            {
                for (int i = varslog.Length - 1; i >= 0; i--)
                {
                    s = Replacemathtext(s, varslog[i], varvalue[i].ToString());
                }
            }
            else
            {
                for (int i = 0; i < varslog.Length; i++)
                {
                    s = Replacemathtext(s, varslog[i], varvalue[i].ToString());
                }
            }
            return this.SolveEquation(s, this.isdegreee);
        }

        public float SolveEquation(string s, string varslog, float varvalue, bool isdeg)
        {
           
            s = s.ToLower();
            if (varslog != null)
            {

                s = Replacemathtext(s, varslog, varvalue.ToString());
            }
            return this.SolveEquation(s, isdeg);
        }
        public float SolveEquation(string s, string varslog, float varvalue)
        {
          
            s = s.ToLower();
            if (varslog != null)
            {

                s = Replacemathtext(s, varslog, varvalue.ToString());
            }

            return this.SolveEquation(s, isdegreee);
        }

        private static float SolveEQCode(string s, bool isdeg)
        {
        
         s = s.ToLower();
       
            float vv = 0;
            if (float.TryParse(s, out vv))
            {
                
                return vv;
            }        
            for (int i = 0; i < Marks[0].Length; i++)
            {
                if (s.Contains(Marks[0][i]))
                {
                    return solveslogan(s, Marks[0][i], isdeg);
                }
            }
            for (int pm = 1; pm < Marks.Length; pm++)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    for (int ps = 0; ps < Marks[pm].Length; ps++)
                    {
                        if (s[i].ToString() == Marks[pm][ps])
                        {

                            if (s[i] == '|')
                            {
                                return solveABS(s, isdeg);
                            }
                            else if (s[i] == '(')
                            {

                                return (solvebrack(s, isdeg));


                            }
                            else
                            {
                                if (i == 0)
                                {

                                }
                                else
                                {
                                    if (s[i - 1] !='e')
                                    {


                                        string ls = s.Substring(0, i);

                                        string ns = s.Substring(i + 1, s.Length - i - 1);

                                        float prenum = (getnextdirect(ls, false));
                                        float nexnum = (getnextdirect(ns, true));

                                       float v = SolveMark(Marks[pm][ps], prenum, nexnum, isdeg);
                                        string news1 = getothert(ls, false);
                                        string news3 = getothert(ns, true);

                                        string news = news1 + v.ToString() + news3;
                                        if (issolvingbrack == false)
                                        {
                                       steps.Add(news);
                                        }
                                        return SolveEQCode(news, isdeg);

                                    }
                                }
                            }
                        }
                    }
                }
            }

     
            throw new FormatException();
        }
       
        static bool issolvingbrack = false;
          static  int   numofbracks(string s, int start)
            {
                int numofbracks = 0;
                int innernum = 0;
                for (int i = start; i < s.Length; i++)
                {
                    if (s[i] == '(')
                    {
                        innernum += 1;


                        numofbracks += 1;

                    }
                    else if (s[i] == ')')
                    {
                        innernum -= 1;


                    }
                    if (innernum == 0 && numofbracks > 0)
                    { return numofbracks; }
                }
                return numofbracks;
            }
          static bool BrackSolvingForSolg = false;
        /// <summary>
        /// Solve cos,sin,tan...etc
        /// </summary>
        /// <param name="s"></param>
        /// <param name="slog"></param>
        /// <param name="isdeg"></param>
        /// <returns></returns>
         static float solveslogan(string s, string slog,bool isdeg,int paramscount=1)
            {
            s = s.ToLower();
                int indx = s.IndexOf(slog);
                float vv = 0;
                int indexoflastinnerbrack = indexof(s, indx, numofbracks(s, indx), ")");
             if (indexoflastinnerbrack==-1)
             {
                 errortext = s;
                 throw new Exception("Syntax Error : Missing Brack at " + s);
             }
                string barcktext = s.Substring(indx + slog.Length, indexoflastinnerbrack - indx - slog.Length + 1);
                BrackSolvingForSolg = true;
                float nexnum = float.NaN;
                for (int i = 0; i < Marks[0].Length; i++)
                {
                    if (barcktext.Contains(Marks[0][i]))
                    {
                        nexnum = solveslogan(barcktext, Marks[0][i], isdeg, paramscount);
                    }
                }
                if (float.IsNaN(nexnum))
                {
                    nexnum = solvebrack(barcktext,isdeg);
                }
                BrackSolvingForSolg = false;
                vv = SolveMark(slog, nexnum, 0, isdeg);
                string news1 = s.Substring(0, indx);
            
                if (indx > 0)
                {
                    if ((nums.Contains(s[indx - 1].ToString())) || (s[indx - 1] == ')'))
                    {
                        news1 = news1 + "*";
                    }
                }

                string news3 = s.Substring(indexoflastinnerbrack + 1, s.Length - indexoflastinnerbrack - 1);
                if (indexoflastinnerbrack + 1 < s.Length)
                {
                    if (nums.Contains(s[indexoflastinnerbrack + 1].ToString()) == true && s[indexoflastinnerbrack + 1] == '(' && s[indexoflastinnerbrack + 1] != '|')
                    {
                        news3 = "*" + news3;
                    }
                }

                string news = news1 + vv.ToString() + news3;
                steps.Add(news);
                if (news.Contains(slog))
                { return solveslogan(news, slog,isdeg); }
                else
                {
                    return SolveEQCode(news,isdeg);
                }
            }
           static float solvebrack(string s,bool isdeg)
            {
             
                int indexoinneropenbrack = 0;
                int numofbracks = 0;
                int currentbarck = 0;
                
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '(')
                    {
                        indexoinneropenbrack = i;
                        if (currentbarck > 0)
                        {
                            numofbracks += 1;
                        }
                    }
                    else if (s[i] == ')')
                    {

                        currentbarck += 1;
                        string news1 = s.Substring(0, indexoinneropenbrack);

                        if (indexoinneropenbrack > 0)
                        {
                            if ((nums.Contains(s[indexoinneropenbrack - 1].ToString())) || (s[indexoinneropenbrack - 1] == ')'))
                            {
                                news1 = news1 + "*";
                            }
                        }

                        if (s[0].ToString() == "-" && indexoinneropenbrack == 1)
                        { news1 = "-1*"; }

                        string news3 = s.Substring(i + 1, s.Length - i - 1);
                        if (i + 1 < s.Length)
                        {
                            if (nums.Contains(s[i + 1].ToString()) == true && s[i + 1]== '(' && s[i + 1] != '|')
                            {
                                news3 = "*" + news3;
                            }
                        }

                        string betwn = s.Substring(indexoinneropenbrack + 1, i - indexoinneropenbrack - 1);
                        issolvingbrack = true;
                        float valueofbrack = SolveEQCode(betwn,isdeg);
                        string news = news1 + valueofbrack + news3;
                        issolvingbrack = false;
                        if (BrackSolvingForSolg == false)
                        {
                            steps.Add(news);
                        }
                        if (news.Contains("("))
                        {
                            return solvebrack(news,isdeg);
                        }
                        else
                        { return SolveEQCode(news,isdeg); }
                    }

                }
                errortext = s.Substring(indexoinneropenbrack,s.Length-indexoinneropenbrack);
              
                throw new Exception("Syntax Error in Brackes at " + errortext);

            }

           static  float solveABS(string s,bool isdeg)
            {
                
                int indexoinneropenbrack = 0;
                int indexoinnerColsedbrack = 0;
                int numofinnerbracks = 0;
                int currentbarck = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '|')
                    {

                        if (currentbarck != 0)
                        {

                            if (defends.Contains(s[i - 1].ToString()))
                            {
                                numofinnerbracks += 1;
                                indexoinneropenbrack = i;
                            }
                            else if (s[i - 1].ToString() == "|")
                            {
                                numofinnerbracks += 1;
                                indexoinneropenbrack = i;
                            }
                            else
                            {
                                indexoinnerColsedbrack = i;
                                break;

                            }
                        }
                        else
                        {
                            indexoinneropenbrack = i;
                        }
                        currentbarck += 1;
                    }
                }

                string news1 = s.Substring(0, indexoinneropenbrack);
                string news3 = s.Substring(indexoinnerColsedbrack + 1, s.Length - indexoinnerColsedbrack - 1);

                if (indexoinneropenbrack > 0)
                {
                    if (defends.Contains(s[indexoinneropenbrack - 1].ToString()) == false && s[indexoinneropenbrack - 1].ToString() != "("
                        && s[indexoinneropenbrack - 1].ToString() != "|")
                    {
                        news1 = news1 + "*";
                    }
                }


                if (indexoinnerColsedbrack + 1 < s.Length)
                {
                    if (defends.Contains(s[indexoinnerColsedbrack + 1].ToString()) == false && s[indexoinnerColsedbrack + 1] != '|'
                                         && s[indexoinnerColsedbrack + 1] != ')')
                    {
                        news3 = "*" + news3;
                    }
                }

                issolvingbrack = true;
                float nv = SolveEQCode(s.Substring(indexoinneropenbrack + 1, indexoinnerColsedbrack - indexoinneropenbrack - 1),isdeg);
                if (nv < 0) { nv = nv * -1; }
                issolvingbrack = false;
                string news = news1 + nv + news3;
                steps.Add(news);

                if (news.Contains("|"))
                {
                    return solveABS(news,isdeg);
                }
                else
                { return SolveEQCode(news,isdeg); }





            }
           static    string  getothert(string s, bool forward)
            {
                string v = "";
                if (forward)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int pm = 0; pm < Marks.Length; pm++)
                        {
                            for (int ps = 0; ps < Marks[pm].Length; ps++)
                            {
                                if (s[i].ToString() == Marks[pm][ps])
                                {
                                    if (i == 0 && s[i]== '-')
                                    {

                                    }
                                    else
                                    {
                                        v = s.Substring(i, s.Length - i);

                                        if (i > 0)
                                        {
                                            if (s[i - 1] == 'e')
                                            {
                                                string vv = s.Substring(i + 1);
                                                string ff = getothert(vv, forward);
                                                v = ff;
                                            }
                                        }

                                        return v;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = s.Length - 1; i >= 0; i--)
                    {
                        for (int pm = 0; pm < Marks.Length; pm++)
                        {
                            for (int ps = 0; ps < Marks[pm].Length; ps++)
                            {
                                if (s[i].ToString() == Marks[pm][ps])
                                {
                                    if (i > 0)
                                    {
                                        for (int c = 1; c < Marks.Length; c++)
                                        {
                                            if (Marks[c].Contains(s[i - 1].ToString()))
                                            {
                                                return v =s.Substring(0, i); 

                                            }
                                        }
                                       
                                         if (s[i - 1] == 'e')
                                        {
                                            string n1 = s.Substring(0, i - 1);
                                            return getothert(n1, false);
                                        }
                                        else
                                        {
                                          return   v = s.Substring(0, i + 1);
                                        }
                                    }
                                    else if (s == "-")
                                    {
                                    }

                                    return v;
                                }
                            }
                        }
                    }
                }
                return "";
            }
          static  float getnextdirect(string s, bool forward)
            {
                return getnextdirect(s, forward, 0);
            }
     static  int errorindex = -1;
          static string errortext = "";
           static  float getnextdirect(string s, bool forward, int repeat)
            {
               
                float v = float.NaN;
                int repeated = 0; 
                if (forward)
                {

                    for (int i = 0; i < s.Length; i++)
                    {
                        for (int pm = 0; pm < Marks.Length; pm++)
                        {
                            for (int ps = 0; ps < Marks[pm].Length; ps++)
                            {
                               
                                if (s[i].ToString() == Marks[pm][ps])
                                {
                                    if (i == 0 && s[i] =='-')
                                    {
                                        string fg = getnextdirect(s.Substring(1), true).ToString();
                                        return float.Parse(("-" + fg).ToString());
                                    }
                                    else
                                    {
                                      
                                        if (s[i - 1] == 'e')
                                        {
                                            string n3 = s.Substring(i + 1, s.Length - i - 1);
                                            n3 = getnextdirect(n3, true).ToString();
                                            string n1 = s.Substring(0, i + 1);
                                            return float.Parse(n1 + n3);

                                        }
                                        else
                                        {
                                            v = float.Parse(s.Substring(0, i));
                                            return v;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = s.Length - 1; i >= 0; i--)
                    {
                        for (int pm = 0; pm < Marks.Length; pm++)
                        {
                            for (int ps = 0; ps < Marks[pm].Length; ps++)
                            {

                                if (s[i].ToString() == Marks[pm][ps])
                                {
                                    if (repeat == repeated)
                                    {



                                        if (i == 0 && s[i] == '-')
                                        {
                                            if (s == "-")
                                            { v = -1; }
                                            else
                                            {
                                                v = float.Parse(s.Substring(0, s.Length));
                                            }
                                        }
                                        else
                                        {
                                            if (s[i]=='-'&&i>0)
                                            {
                                                for (int c=1;c<Marks.Length;c++)
                                                {
                                                    if (Marks[c].Contains(s[i-1].ToString()))
                                                    {
                                                    return     v = float.Parse(s.Substring(i, s.Length - i));
                                                       
                                                    }
                                                }
                                            }
                                          
                                                v = float.Parse(s.Substring(i + 1, s.Length - 1 - i));

                                            

                                        }
                                        if (i > 0)
                                        {
                                            if (s[i - 1] == 'e')
                                            {
                                                string n3 = s.Substring(i - 1, s.Length - i + 1);
                                                string n1 = s.Substring(0, i - 1);
                                                n1 = getnextdirect(n1, false).ToString();
                                                return float.Parse(n1 + n3);
                                            }
                                        }
                                        return v;
                                    }
                                    repeated += 1;


                                }
                            }
                        }
                    }
                }
                float value = 0;
                if (float.TryParse(s, out value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Unknown expresion ('"+s+"')after some operator");
                }
            }

            static int indexof(string mains, int startindex, int repeat, int cnt, string tofind)
            {
                if (startindex + cnt > mains.Length)
                { throw new Exception("startindex+count =(" + (startindex + cnt).ToString() + ") > Lenght =(" + mains.Length + ")"); }
                int rep = 0;
                for (int i = startindex; i < startindex + cnt; i++)
                {
                    if (mains[i].ToString() == tofind)
                    {
                        rep += 1;
                        if (rep == repeat)
                        { return i; }

                    }
                }
                return -1;
            }
            static int indexof(string mains, int startindex, int repeat, string tofind)
            {
                return indexof(mains, startindex, repeat, mains.Length - startindex, tofind);
            }
            static int indexof(string mains, int startindex, string tofind)
            {
                return indexof(mains, startindex, 0, mains.Length - startindex, tofind);
            }
            static int indexof(string mains, string tofind)
            {
                return indexof(mains, 0, 0, mains.Length, tofind);
            }
            public EquationSolver(bool isdeg)
            {
               this.Isdegreee = isdeg;
            }
        public EquationSolver()
        {
          
        }
    }


}
