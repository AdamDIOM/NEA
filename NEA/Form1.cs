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
        public static int currentRoom = 0;
        public static List<Room> Rooms = new List<Room>();
        public static List<string> Inventory = new List<string>();

        public delegate void DisplayOption(/*ref DisplayOption ShowMenu,*/ TableLayoutPanel tblLayout);
        public DisplayOption ShowMenu;// = new DisplayOption(InitialiseTable);
        #endregion
        public RPG()
        {
            InitializeComponent();
        }

        //what the program boots into
        private void RPG_Load(object sender, EventArgs e)
        {
            #region initial form setup preparation
            //creates the main table for the program to run from
            TableLayoutPanel tblLayout = new TableLayoutPanel();
            //delegate attempt
            ShowMenu = new DisplayOption(InitialiseTable);

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
            #endregion
            #region delegate startup 
            //goes to the specific option for the time in the program
            DisplayOption oldShowMenu = ShowMenu;
            //Debug.WriteLine(oldShowMenu == ShowMenu);
            //Debug.WriteLine("1: " + Convert.ToString(oldShowMenu == ShowMenu));
            ShowMenu(tblLayout);
            //Debug.WriteLine("2: " + Convert.ToString(oldShowMenu == ShowMenu));
            while (ShowMenu != oldShowMenu)
            {
                //Debug.WriteLine("1: " + Convert.ToString(oldShowMenu == ShowMenu));
                oldShowMenu = ShowMenu;
                //Debug.WriteLine("2: " + Convert.ToString(oldShowMenu == ShowMenu));
                ShowMenu(tblLayout);
                //Debug.WriteLine("3: " + Convert.ToString(oldShowMenu == ShowMenu));
            }
            #endregion
        }

        private void InitialiseTable(TableLayoutPanel tblLayout)
        {
            /*sets up the table width (window width), height (window heigh - 39),
            sets it to anchor to all sides so it can be resized, adds 6 rows and 5 columns*/
            tblLayout.Width = Width;
            tblLayout.Height = Height - 39;
            tblLayout.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            tblLayout.ColumnCount = 5;
            tblLayout.RowCount = 6;
            //temporary border for testing purposes
            //tblLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            //sets each column to be 20% of the width of the window
            for (int i = 0; i < tblLayout.ColumnCount; i++)
            {
                tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
            //sets the first row to be 20% of the window height
            tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            //sets the remaining rows to be 16% of the window height (20% of remaining height)
            for (int i = 1; i < tblLayout.RowCount; i++)
            {
                tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16));
            }

            //TitleScreen(ref ShowMenu, tblLayout);// replaced with delegate
            ShowMenu = new DisplayOption(TitleScreen);
            //ShowMenu(ref ShowMenu, tblLayout);

            Controls.Add(tblLayout);
        }
        private void TitleScreen(TableLayoutPanel tblLayout)
        {
            /*creates a lable for the title, aligns it to the centre, adds the text,
            sets autosize and anchors for resizing with window, sets font
            and sets location to the the centre of the page spanning 5 columns
            */
            #region title
            Label lblTitle = new Label();
            lblTitle.Text = "RPG Designer and Player";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Name = "lblTitle";
            lblTitle.AutoSize = true;
            lblTitle.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblTitle.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            lblTitle.Location = new Point(this.Width / 2 - 2 * lblTitle.Width, 50);
            tblLayout.SetColumnSpan(lblTitle, 5);
            //adds the title to the table
            tblLayout.Controls.Add(lblTitle, 0, 0);
            #endregion
            #region buttons
            /*creates a button for the Designer option, adds it to the table
            and sets anchors for auto resizing*/
            Button btnDesigner = new Button();
            btnDesigner.Text = "Designer";
            // sort this out
            //btnDesigner.Click += ;
            tblLayout.Controls.Add(btnDesigner, 1, 2);
            btnDesigner.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);

            /*creates a button for the Play option, adds it to the table
            and sets anchors for auto resizing*/
            Button btnPlay = new Button();
            btnPlay.Text = "Play";
            btnPlay.Click += new EventHandler((sender, e) => {
                ShowMenu = new DisplayOption(PlayScreen);
                ShowMenu(tblLayout);
                });
            tblLayout.Controls.Add(btnPlay, 3, 2);
            btnPlay.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);

            /*creates a button to quit, adds it to the table, adds EventHandler to quit
            and sets anchors for auto resizing*/
            Button btnQuit = new Button();
            btnQuit.Text = "Quit";
            btnQuit.Click += new EventHandler(Quit);
            tblLayout.Controls.Add(btnQuit, 2, 4);
            btnQuit.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            #endregion
            /*ShowMenu = new DisplayOption(InitialiseTable);
              used for testing only
             */
        }
        private void DesignerScreen(TableLayoutPanel tblLayout)
        {
            //setup the button controls for Design
        }
        private void PlayScreen(TableLayoutPanel tblLayout)
        {
            //setup the button controls for Play
            #region Move standard buttons away from the play grid
            tblLayout.GetControlFromPosition(0, 0).Text = "Play RPG";
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(1, 2), 0, 5);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(3, 2), 1, 5);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(2, 4), 3, 4);
            tblLayout.GetControlFromPosition(0, 5).Hide();
            tblLayout.GetControlFromPosition(1, 5).Hide();
            #endregion

            //list of tuples containing room name, x coord and y coord for tblLayout
            #region Create tuples for buttons and their respective locations in tblLayout
            List<Tuple<string, int, int>> directionButtons = new List<Tuple<string, int, int>>();
            directionButtons.Add(new Tuple<string, int, int>("North", 2, 2));
            directionButtons.Add(new Tuple<string, int, int>("East", 3, 3));
            directionButtons.Add(new Tuple<string, int, int>("South", 2, 4));
            directionButtons.Add(new Tuple<string, int, int>("West", 1, 3));
            directionButtons.Add(new Tuple<string, int, int>("Up", 1, 2));
            directionButtons.Add(new Tuple<string, int, int>("Down", 1, 4));
            #endregion

            #region labels for Current Room: and the room, as well as Inventory: and items in the inventory
            #region Current Room:
            Label lblCurrentRoom = new Label();
            lblCurrentRoom.Text = "Current Room:";
            lblCurrentRoom.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentRoom.Name = "lblCurrentRoom";
            lblCurrentRoom.AutoSize = true;
            lblCurrentRoom.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblCurrentRoom.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            lblCurrentRoom.Location = new Point(this.Width / 2 - 2 * lblCurrentRoom.Width, 50);
            tblLayout.SetColumnSpan(lblCurrentRoom, 2);
            //adds the title to the table
            tblLayout.Controls.Add(lblCurrentRoom, 1, 1);
            #endregion
            #region Inventory: 
            Label lblInventory = new Label();
            lblInventory.Text = "Inventory:";
            lblInventory.TextAlign = ContentAlignment.MiddleCenter;
            lblInventory.Name = "lblInventory";
            lblInventory.AutoSize = true;
            lblInventory.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblInventory.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            lblInventory.Location = new Point(this.Width / 2 - 2 * lblCurrentRoom.Width, 50);
            //adds the title to the table
            tblLayout.Controls.Add(lblInventory, 4, 1);
            #endregion
            #region Current room
            Label lblRoom = new Label();
            lblRoom.Text = Rooms[currentRoom].GetName();
            lblRoom.TextAlign = ContentAlignment.MiddleCenter;
            lblRoom.Name = "lblRoom";
            lblRoom.AutoSize = true;
            lblRoom.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblRoom.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            lblRoom.Location = new Point(this.Width / 2 - 2 * lblCurrentRoom.Width, 50);
            //adds the title to the table
            tblLayout.Controls.Add(lblRoom, 3, 1);
            #endregion
            #region Inventory Items
            Label lblInvItems = new Label();
            lblInvItems.Text = "";
            lblInvItems.TextAlign = ContentAlignment.MiddleCenter;
            lblInvItems.Name = "lblInvItems";
            lblInvItems.AutoSize = true;
            lblInvItems.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            lblInvItems.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            lblInvItems.Location = new Point(this.Width / 2 - 2 * lblCurrentRoom.Width, 50);
            //adds the title to the table
            tblLayout.Controls.Add(lblInvItems, 4, 2);
            #endregion
            #endregion

            //loops through the list of tuples containing the button names and coordinates for tblLayout and adds each one
            #region displays buttons and adds Event Handlers
            #region Directional buttons
            foreach (Tuple<string, int, int> tuple in directionButtons)
            {
                Button tempButton = new Button();
                tempButton.Text = "Go " + tuple.Item1;
                tempButton.Click += new EventHandler((sender, e) => {
                    currentRoom = Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1];
                    CheckDirections(tblLayout, directionButtons, lblRoom, lblInvItems);
                });
                tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
                tblLayout.Controls.Add(tempButton, tuple.Item2, tuple.Item3);
            }
            #endregion
            #region Get items button
            Button itemButton = new Button();
            itemButton.Text = "Get ";
            itemButton.Click += new EventHandler((sender, e) =>
             {
                 Inventory.Add(Rooms[currentRoom].GetItem());
                 lblInvItems.Text += Inventory.Last();
                 Rooms[currentRoom].DelItem();
                 CheckDirections(tblLayout, directionButtons, lblRoom, lblInvItems);
             });
            itemButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            tblLayout.Controls.Add(itemButton, 3, 2);
            CheckDirections(tblLayout, directionButtons, lblRoom, lblInvItems);
            #endregion
            #endregion
        }
        private void CheckDirections(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> directionButtons, Label lblRoom, Label lblInvItems)
        {
            #region Updates rooms for unlocks and updates room label & inventory label
            lblRoom.Text = Rooms[currentRoom].GetName();
            Rooms[currentRoom].UnlockRoom(ref Inventory);

            lblInvItems.Text = "";
            foreach (string s in Inventory)
            {
                lblInvItems.Text += s;
                lblInvItems.Text += "\n";
            }
            #endregion
            #region checks whether item from room is (1) got, (2) existent or (3) nonexistent and en/disables button
            if (Rooms[currentRoom].GetItem().Substring(Rooms[currentRoom].GetItem().Length-2, 2) == "t!")
            {
                tblLayout.GetControlFromPosition(3, 2).Text = Rooms[currentRoom].GetItem();
                tblLayout.GetControlFromPosition(3, 2).Enabled = false;
            }
            else if(Rooms[currentRoom].GetItem() != "none")
            {
                tblLayout.GetControlFromPosition(3, 2).Text = "Get " + Rooms[currentRoom].GetItem();
                tblLayout.GetControlFromPosition(3, 2).Enabled = true;
                tblLayout.GetControlFromPosition(3, 2).Cursor = Cursors.Hand;
            }
            else
            {
                tblLayout.GetControlFromPosition(3, 2).Text = "No items available";
                tblLayout.GetControlFromPosition(3, 2).Enabled = false;
            }
            #endregion
            #region checks all directions to see if they can be travelled or not
            foreach (Tuple<string, int, int> tuple in directionButtons)
            {
                if(Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1] > -1)
                {
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Enabled = true;
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Cursor = Cursors.Hand;
                }
                else
                {
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Enabled = false;;
                }
            }
            #endregion
        }
        private void Quit(object sender, EventArgs e)
        {
            //quits the application.
            Application.Exit();
        }
    }
}
