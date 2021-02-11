using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace NEA
{
    public partial class RPG : Form
    {
        #region Static variables and delegate setup
        public static int currentRoom;
        public static List<Room> Rooms = new List<Room>();
        public static List<string> Inventory = new List<string>();
        public  string AppPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public delegate void DisplayOption(TableLayoutPanel tblLayout);
        public DisplayOption ShowMenu;
        #endregion
        public RPG()
        {
            InitializeComponent();
        }

        //what the program boots into
        private void RPG_Load(object sender, EventArgs e)
        {
            // creates the main TableLayoutPanel for the program to run from
            TableLayoutPanel tblLayout = new TableLayoutPanel();
            // creates an instance of the custom delegate 
            ShowMenu = new DisplayOption(InitialiseTable);
            // to be replaced with persistent data
            
             
            // this enables the program to loop through delegates until the program is fully enabled
            DisplayOption oldShowMenu = ShowMenu;
            // use of Delegates
            // runs delegate first time to allow while loop to run
            ShowMenu(tblLayout);
            /* loops until oldShowMenu and ShowMenu are the same */
            while (ShowMenu != oldShowMenu)
            {
                oldShowMenu = ShowMenu;
                ShowMenu(tblLayout);
            }
        }
        private void InitialiseTable(TableLayoutPanel tblLayout)
        {
            /* sets up the table width (window width), height (window heigh - 39), sets it to anchor to all sides so it will resize, adds 6 rows and 5 columns */
            tblLayout.Width = Width;
            tblLayout.Height = Height - 39;
            tblLayout.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            tblLayout.ColumnCount = 5;
            tblLayout.RowCount = 6;

            // temporary border for testing purposes
            //tblLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            /* sets each column to be 20% of the width of the window */
            for (int i = 0; i < tblLayout.ColumnCount; i++)
            {
                tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
            // sets the first row to be 20% of the window height
            tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            /* sets the remaining rows to be 16% of the window height (20% of remaining height) */
            for (int i = 1; i < tblLayout.RowCount; i++)
            {
                tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16));
            }

            // changes the delegate so when returning to the loop a different method is called
            ShowMenu = new DisplayOption(TitleScreen);

            // adds the TableLayoutPanel to the Form
            Controls.Add(tblLayout);
        }
        private void TitleScreen(TableLayoutPanel tblLayout)
        {
            // creates a Label to show the window title
            Label lblTitle = CreateLabel(tblLayout, "RPG Designer and Player", "lblTitle", 0, 0, 5);
            // adds the buttons for the title
            TitleButtons(tblLayout);
        }
        private void TitleButtons(TableLayoutPanel tblLayout)
        {
            /* creates a button for the Designer option and adds it to the table */
            AddButton(tblLayout, CreateButton(tblLayout, "Design", DesignRPG), 1, 2);

            /* creates a button for the Play option and adds it to the table */
            AddButton(tblLayout, CreateButton(tblLayout, "Play", ChoosePlayFile), 3, 2);

            /* creates a button to quit, adds it to the table, adds EventHandler to quit */
            AddButton(tblLayout, CreateButton(tblLayout, "Quit", Quit), 2, 4);
        }
        private void Quit(TableLayoutPanel tblLayout)
        {
            // quits the application.
            Application.Exit();
        }
    }
}
