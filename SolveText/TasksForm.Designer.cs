namespace SolveText
{
    partial class TasksForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kgrid1 = new SolveText.kgrid();
            ((System.ComponentModel.ISupportInitialize)(this.kgrid1.Rightscroll)).BeginInit();
            this.SuspendLayout();
            // 
            // kgrid1
            // 
            this.kgrid1.AllowScrolling = true;
            this.kgrid1.autosizegroups = false;
            this.kgrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kgrid1.Grouping = false;
            this.kgrid1.InnerSpace = new System.Drawing.Point(1, 2);
            this.kgrid1.IsAccesdingSort = true;
            this.kgrid1.Location = new System.Drawing.Point(0, 0);
            this.kgrid1.Name = "kgrid1";
            // 
            // 
            // 
            this.kgrid1.Rightscroll.AreaMargin = new System.Drawing.Point(0, 100);
            this.kgrid1.Rightscroll.Bordercolor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kgrid1.Rightscroll.ControlButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kgrid1.Rightscroll.DefultMovercolor = System.Drawing.Color.Gray;
            this.kgrid1.Rightscroll.DrawBackTranspersyRec = false;
            this.kgrid1.Rightscroll.Fillselectedarea = false;
            this.kgrid1.Rightscroll.FloatPlaces = ((uint)(1u));
            this.kgrid1.Rightscroll.Hangvaluechangedevent = false;
            this.kgrid1.Rightscroll.Hangvaluechangingevent = false;
            this.kgrid1.Rightscroll.Location = new System.Drawing.Point(136, 0);
            this.kgrid1.Rightscroll.Maincolor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.kgrid1.Rightscroll.MaxgradientColors = new System.Drawing.Color[] {
        System.Drawing.Color.Orange,
        System.Drawing.Color.Yellow,
        System.Drawing.Color.Green,
        System.Drawing.Color.Cyan,
        System.Drawing.Color.Blue};
            this.kgrid1.Rightscroll.MaxGradientPositions = new float[] {
        0F,
        0.25F,
        0.5F,
        0.75F,
        1F};
            this.kgrid1.Rightscroll.Maxmum = 100F;
            this.kgrid1.Rightscroll.Minimum = 0F;
            this.kgrid1.Rightscroll.Name = "";
            this.kgrid1.Rightscroll.OrientationDirection = System.Windows.Forms.Orientation.Vertical;
            this.kgrid1.Rightscroll.PreScrollingValue = 0F;
            this.kgrid1.Rightscroll.PreValue = 0F;
            this.kgrid1.Rightscroll.SelectedIndex = 0;
            this.kgrid1.Rightscroll.Showtextvalues = false;
            this.kgrid1.Rightscroll.Size = new System.Drawing.Size(20, 169);
            this.kgrid1.Rightscroll.Smoothcolor = System.Drawing.Color.Gray;
            this.kgrid1.Rightscroll.Style = SolveText.KscalePaintMode.Flat;
            this.kgrid1.Rightscroll.TabIndex = 0;
            this.kgrid1.Rightscroll.TextSize = new System.Drawing.Size(0, 0);
            this.kgrid1.Rightscroll.Theme = SolveText.KscaleThemes.Dark;
            this.kgrid1.Rightscroll.ValueF = 0F;
            this.kgrid1.Rightscroll.Valuestextposition = new float[0];
            this.kgrid1.Rightscroll.Valuestexts = new string[0];
            this.kgrid1.Rightscroll.ValuesTextStep = 1F;
            this.kgrid1.Rightscroll.Visible = false;
            this.kgrid1.Selectedindex = -1;
            this.kgrid1.Size = new System.Drawing.Size(156, 169);
            this.kgrid1.Sort = false;
            this.kgrid1.TabIndex = 0;
            this.kgrid1.Text = "kgrid1";
            this.kgrid1.Selecteditemchanged += new SolveText.kgrideventhandler(this.kgrid1_Selecteditemchanged);
            // 
            // TasksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(156, 169);
            this.ControlBox = false;
            this.Controls.Add(this.kgrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TasksForm";
            this.Opacity = 0.98D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TasksForm";
            this.Deactivate += new System.EventHandler(this.TasksForm_Deactivate);
            this.Load += new System.EventHandler(this.TasksForm_Load);
            this.Leave += new System.EventHandler(this.TasksForm_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.kgrid1.Rightscroll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SolveText.kgrid kgrid1;
    }
}