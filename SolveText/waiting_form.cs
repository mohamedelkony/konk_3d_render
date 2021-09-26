using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveText
{
    public partial class waiting_form : Form
    {
        public waiting_form()
        {
            InitializeComponent();
        }
        public string Header {get { return label5.Text; }set { label5.Text = value; } }
    }
}
