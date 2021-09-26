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
    public class textcontext : ContextMenuStrip
    {
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReplaceAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public static textcontext make() { return new textcontext(); }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            string copiedtext = rtb.SelectedText;
            if (copiedtext != "")
            {
                rtb.Text = rtb.Text.Replace(copiedtext, "");
                Clipboard.SetText(copiedtext);
            }
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            string replcaedtext = rtb.SelectedText;
            if (replcaedtext != "")
            {
                rtb.Text = rtb.Text.Replace(rtb.SelectedText, Clipboard.GetText());
            }
            else
            {
                rtb.Text = rtb.Text.Insert(rtb.SelectionStart, Clipboard.GetText());
            }
           
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            string copiedtext = rtb.SelectedText;
            if (rtb.SelectedText != "")
            {
                Clipboard.SetText(copiedtext);
            }

        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            string copiedtext = rtb.Text;         
                Clipboard.SetText(copiedtext);          
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            rtb.Text = "";
        }
        private void pasteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = (RichTextBox)this.SourceControl;
            rtb.Text = Clipboard.GetText();
        }
        public textcontext()
        {
            this.ReplaceAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         
            // 
            // this
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.copyAllToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.ReplaceAllToolStripMenuItem});
            this.Name = "this";
           this.Size = new System.Drawing.Size(153, 164);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Cut";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyAllToolStripMenuItem.Text = "Copy All";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.copyAllToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            //    // pasteAllToolStripMenuItem
            // 
            this.ReplaceAllToolStripMenuItem.Name = "pasteAllToolStripMenuItem";
            this.ReplaceAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ReplaceAllToolStripMenuItem.Text = "Paste All";
            this.ReplaceAllToolStripMenuItem.Click += new System.EventHandler(this.pasteAllToolStripMenuItem_Click);
        
        }
    }
}
