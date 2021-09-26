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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        /*
         تم العمل على البرنامج خلال 6 ايام وهو جزء من البرنامج الأم الذى اتمنى النتهاء منه قبل 3 ثانونى وبدأت عليه من اولى ثانوى الترم التانى اتمنى ان يفيدكم البنامج فى تمثيل الدوال وانتاج الاشكال الهندسيه ...من يريد الشفره المصدريه او الادوات المستخدمة يمكنه ان يكلمنى 01118979196 وشكرا 
         */
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void kgriditem1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(((Control)sender).Text);
    
        }
    }
}
