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

namespace NEA
{
    public partial class RPG : Form
    {
        #region Static variables and delegate setup
        public static int currentRoom;
        public static List<Room> Rooms = new List<Room>();
        public static List<string> Inventory = new List<string>();

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
            #region temporary Rooms data for testing
            Rooms.Add(new Room("Porch", -1, 1, 6, -1, -1, -1));
            Rooms.Add(new Room("Hall", -1, 3, 2, 0, -1, -1, "Up", 7, "Stair")); 
            Rooms.Add(new Room("Kitchen", 1, 5, 4, 6, -1, -1, "Down", 14, "Key"));
            Rooms.Add(new Room("Dining Room", -1, -1, 5, 1, -1, -1));
            Rooms.Add(new Room("Bathroom", 2, 5, -1, -1, -1, -1, "Stair"));
            Rooms.Add(new Room("Living Room", 3, -1, -1, 4, -1, -1));
            Rooms.Add(new Room("Garage", 0, 2, -1, -1, -1, -1));
            Rooms.Add(new Room("Upstairs Hall", 8, 10, 11, 13, -1, 1));
            Rooms.Add(new Room("Study", -1, 9, 7, -1, -1, -1));
            Rooms.Add(new Room("Master Bedroom", -1, -1, 10, 8, -1, -1));
            Rooms.Add(new Room("Upstairs Bathroom", 9, -1, 12, 7, -1, -1));
            Rooms.Add(new Room("Spare Bedroom", 7, 12, -1, -1, -1, -1));
            Rooms.Add(new Room("Roof", 10, -1, -1, 11, -1, -1, "Key"));
            Rooms.Add(new Room("Bedroom", -1, 7, -1, -1, -1, -1));
            Rooms.Add(new Room("Basement", -1, 15, -1, 16, 2, -1));
            Rooms.Add(new Room("Washing Room", -1, -1, -1, 14, -1, -1));
            Rooms.Add(new Room("Cinema", -1, 14, -1, -1, -1, -1));
            #endregion
             
            // this enables the program to loop through delegates until the program is fully enabled
            DisplayOption oldShowMenu = ShowMenu;
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
            Label lblTitle = CreateLabel("RPG Designer and Player", "lblTitle");
            // sets the Label to span over 5 columns so it is wider
            tblLayout.SetColumnSpan(lblTitle, 5);
            // adds the title to the table
            tblLayout.Controls.Add(lblTitle, 0, 0);
            TitleButtons(tblLayout);
        }
        private Label CreateLabel(string text, string name)
        {
            // creates a temporary Label
            Label lblTemp = new Label();
            // sets text formatting to centre alignment
            lblTemp.TextAlign = ContentAlignment.MiddleCenter;
            // allows the label to get larger and smaller as the window does so
            lblTemp.AutoSize = true;
            // makes the label stretch as the window changes
            lblTemp.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            // sets the label font
            lblTemp.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            // sets location for the button - the centre of the container (TableLayoutPanel cell)
            lblTemp.Location = new Point(this.Width / 2 - 2 * lblTemp.Width, 50);
            // sets the text on the label to contain the string from the first parameter
            lblTemp.Text = text;
            // sets the name of the label to the string from the second parameter
            lblTemp.Name = name;

            // returns the created label for more permanent storage where the function was called from
            return lblTemp;
        }

        private void TitleButtons(TableLayoutPanel tblLayout)
        {
            /* creates a button for the Designer option and adds it to the table */
            Button btnDesign = CreateButton(tblLayout, "Design", DesignRPG);
            tblLayout.Controls.Add(btnDesign, 1, 2);

            /* creates a button for the Play option and adds it to the table */
            Button btnPlay = CreateButton(tblLayout, "Play", PlayRPG);
            tblLayout.Controls.Add(btnPlay, 3, 2);

            /* creates a button to quit, adds it to the table, adds EventHandler to quit */
            Button btnQuit = CreateButton(tblLayout, "Quit", Quit);
            tblLayout.Controls.Add(btnQuit, 2, 4);
        }
        private Button CreateButton(TableLayoutPanel tblLayout, string text, DisplayOption option)
        {
            // creates a temporary button
            Button tempButton = new Button();
            // sets the text on the button to the string from parameters
            tempButton.Text = text;
            // makes the button stretch as the window changes
            tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            // EventHandler, when button is clicked the parameter method is run through the use of Delegates
            tempButton.Click += new EventHandler((sender, e) =>
            {
                ShowMenu = new DisplayOption(option);
                ShowMenu(tblLayout);
            });

            return tempButton;
        }

        
        private void Quit(TableLayoutPanel tblLayout)
        {
            // quits the application.
            Application.Exit();
        }
    }
}
