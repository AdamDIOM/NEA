using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    public partial class RPG : Form
    {
        public delegate void DisplayOption(ref DisplayOption Menu, TableLayoutPanel tblLayout);
        public RPG()
        {
            InitializeComponent();
        }

        private void RPG_Load(object sender, EventArgs e)
        {
            //sort out delegate into initialisetable to choose between the three options?
            TableLayoutPanel tblLayout = new TableLayoutPanel();
            DisplayOption Menu = new DisplayOption(InitialiseTable);
            while (true)
            {
                InitialiseTable(ref Menu, tblLayout);
                //Menu(ref Menu, tblLayout);
            }
        }
        private void InitialiseTable(ref DisplayOption Menu, TableLayoutPanel tblLayout)
        {
            
            tblLayout.Width = Width;
            tblLayout.Height = Height-39;
            tblLayout.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            tblLayout.ColumnCount = 5;
            tblLayout.RowCount = 6;
            tblLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            
            for(int i = 0; i < tblLayout.ColumnCount; i++)
            {
                tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
            
            tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            for(int i = 1; i < tblLayout.RowCount; i++)
            {
                tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16));
            }

            //TitleScreen(ref Menu, tblLayout);// replaced with delegate
            //Menu = new DisplayOption(TitleScreen);

            Controls.Add(tblLayout);
        }
        private void TitleScreen(ref DisplayOption Menu, TableLayoutPanel tblLayout)
        {
            Label lblTitle = new Label();
            lblTitle.Text = "RPG Designer and Player";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Name = "lblTitle";
            lblTitle.AutoSize = true;
            lblTitle.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblTitle.Location = new Point(this.Width / 2 - 2 * lblTitle.Width, 50);
            lblTitle.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            tblLayout.SetColumnSpan(lblTitle, 5);

            tblLayout.Controls.Add(lblTitle, 0, 0);

            Button btnDesigner = new Button();
            btnDesigner.Text = "Designer";
            // sort this out
            //btnDesigner.Click += new EventHandler(ButtonClick(ref Menu));
            tblLayout.Controls.Add(btnDesigner, 1, 2);
            btnDesigner.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);

            Button btnPlay = new Button();
            btnPlay.Text = "Play";
            tblLayout.Controls.Add(btnPlay, 3, 2);
            btnPlay.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);

            Button btnQuit = new Button();
            btnQuit.Text = "Quit";
            btnQuit.Click += new EventHandler(Quit);
            tblLayout.Controls.Add(btnQuit, 2, 4);
            btnQuit.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        }
        private void ButtonClick(object sender, EventArgs e, ref DisplayOption Menu)
        {

        }
        private void DesignerScreen(ref DisplayOption Menu, TableLayoutPanel tblLayout)
        {

        }
        private void PlayScreen(ref DisplayOption Menu, TableLayoutPanel tblLayout)
        {

        }
        private void Quit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
