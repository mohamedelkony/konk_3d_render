using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SolveText
{
    partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        
        }

 
        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void HelpForm_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://3dkonk.wix.com/3d-equation-render#!how-to-use/c1kdb");
        }
    }
}
