using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolveText
{
    public partial class TasksForm : Form
    {
        public TasksForm()
        {
            InitializeComponent();
        }
      
        public kgrid GetKgrid { get { return kgrid1; } }
        public kgriditem orgn = new kgriditem();
 
 public void AddTask(comboitem c)
        {
            kgriditem kgi = new kgriditem();
      
       fnc.CopyProperties( kgi,orgn, "Appearance",true);
            fnc.CopyProperties(kgi, orgn, "Effects", true);

         
            kgi.Text = c.Text;
           kgi.Centerimage = c.Image;
            //auto cpoid from orgm griditem
       //    kgi.BackColor = c.Backcolor;
            kgi.ForeColor = c.Forecolor;
            kgi.Font = c.Font;
        
           kgi.Size = new System.Drawing.Size(orgn.SubPanelFormWidth-2,30);
           kgi.Location = new Point();
     
          kgrid1.Items.Add(kgi);
        }
public void AddTasks(comboitem[] tasks)
 {
    for (int i=0;i<tasks.Length;i++)
    {
        AddTask(tasks[i]);
    }

 }
public void AddTasks(string [] tasks)
{
    for (int i = 0; i < tasks.Length; i++)
    {
        comboitem c = new comboitem(); c.Text = tasks[i];
        AddTask(c);
    }

}
//public int SelcetedIndex = -1;
        private void TasksForm_Load(object sender, EventArgs e)
        {Point loc=orgn.PointToScreen(new Point(0, orgn.Height));
            this.Width = orgn.SubPanelFormWidth;
            this.Height = kgrid1.Items.Count * (30 + 2) + 4;
            if (this.Height > 200) { this.Height = 200; }
            Size sz = this.Size;
            if (loc.Y + sz.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                this.Location=new Point(loc.X,orgn.PointToScreen(new Point(0, 0)).Y-this.Height);
                

            }
            else
            {
                this.Location = loc;
                
            }
        }
        public event kgrideventhandler selectedindexchanged;

        private void kgrid1_Selecteditemchanged(object sender, kgrideventargs e)
        {
      //      SelcetedIndex =e.item.Index;
           
            this.Close();
             if (selectedindexchanged != null)
            {
                selectedindexchanged(sender, e);                
            }
        }

        private void TasksForm_Deactivate(object sender, EventArgs e)
        {//SelcetedIndex=-1;
            this.Close();
        }

        private void TasksForm_Leave(object sender, EventArgs e)
        {
         
        }
    }
}
