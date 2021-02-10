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
        private void PlayRPG(TableLayoutPanel tblLayout)
        {
            // sets the current position to be the starting room
            currentRoom = 0;
            /* this method moves the original 3 buttons (Designer, Play, Quit) away and hides them */
            HideStandardButtons(tblLayout, "Play");

            /* this function returns a list of tuples containing room name and tblLayout x coord and y coord */
            List<Tuple<string, int, int>> DirectionalButtons = CreateButtonTuples();

            /* this function returns a list of four labels: 'Current Room: ', current room, 'Inventory: ', inventory items; and shows them on screen */
            List<Label> LabelsList = ShowPlayLabels(tblLayout);

            /* this method loops through the list of tuples containing the button names and tblLayout coordinates and shows and en/disables each one */
            ShowButtons(tblLayout, LabelsList, DirectionalButtons);
        }


        private void HideStandardButtons(TableLayoutPanel tblLayout, string option)
        {
            // modifies the title
            tblLayout.GetControlFromPosition(0, 0).Text = option + " RPG";

            /* moves the controls from original locations to new locations and hides the three main menu buttons */
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(1, 2), 0, 5);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(3, 2), 1, 5);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(2, 4), 2, 5);


            tblLayout.GetControlFromPosition(0, 5).Hide();
            tblLayout.GetControlFromPosition(1, 5).Hide();
            tblLayout.GetControlFromPosition(2, 5).Hide();

        }


        private List<Tuple<string, int, int>> CreateButtonTuples()
        {
            // creates a list to contain the information about each of the six buttons
            List<Tuple<string, int, int>> DirectionalButtons = new List<Tuple<string, int, int>>();

            /* creates six tuples containing button info and adds to the list */
            DirectionalButtons.Add(new Tuple<string, int, int>("North", 2, 2));
            DirectionalButtons.Add(new Tuple<string, int, int>("East", 3, 3));
            DirectionalButtons.Add(new Tuple<string, int, int>("South", 2, 4));
            DirectionalButtons.Add(new Tuple<string, int, int>("West", 1, 3));
            DirectionalButtons.Add(new Tuple<string, int, int>("Up", 1, 2));
            DirectionalButtons.Add(new Tuple<string, int, int>("Down", 1, 4));

            // sends the list of buttons to where this function was called from
            return DirectionalButtons;
        }


        private List<Label> ShowPlayLabels(TableLayoutPanel tblLayout)
        {
            // creates a list to contain all four labels
            List<Label> LabelsList = new List<Label>();

            /* creation of the title label 'Current Room:' */
            Label lblCurrentRoom = CreateLabel("Current Room:", "lblCurrentRoom");
            tblLayout.SetColumnSpan(lblCurrentRoom, 2);
            LabelsList.Add(lblCurrentRoom);
            tblLayout.Controls.Add(lblCurrentRoom, 1, 1);

            /* creation of the title label 'Inventory:' */
            Label lblInventory = CreateLabel("Inventory:", "lblInventory");
            LabelsList.Add(lblInventory);
            tblLayout.Controls.Add(lblInventory, 4, 1);

            /* creation of the label to show the current room */
            Label lblRoom = CreateLabel(Rooms[currentRoom].GetName(), "lblRoom");
            LabelsList.Add(lblRoom);
            tblLayout.Controls.Add(lblRoom, 3, 1);

            /* creation of the label to show items in the inventory */
            Label lblInvItems = CreateLabel("", "lblInvItems");
            LabelsList.Add(lblInvItems);
            tblLayout.Controls.Add(lblInvItems, 4, 2);

            // returns the list of four labels to where the function was called from
            return LabelsList;
        }


        private void ShowButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            tblLayout.Controls.Add(CreateButton(tblLayout, "Exit Game", EndGame), 3, 4);
            // use of decomposition here
            ShowDirectionalButtons(tblLayout, LabelsList, DirectionalButtons);
            ShowItemsButton(tblLayout, LabelsList, DirectionalButtons);
            UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
        }

        private Button CreateButton(TableLayoutPanel tblLayout, string text)
        {
            // creates a temporary button
            Button tempButton = new Button();
            // sets the text on the button to the string from parameters
            tempButton.Text = text;
            // makes the button stretch as the window changes
            tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            return tempButton;
        }

        private void ShowDirectionalButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // loops through each tuple in the list, which contains the name and tblLayout x/y coordinates
            foreach (Tuple<string, int, int> tuple in DirectionalButtons)
            {
                // creates a temporary button with text 'Go <direction>'
                Button tempButton = CreateButton(tblLayout, "Go " + tuple.Item1);
                // adds EventHandler to the button to change room
                tempButton.Click += new EventHandler((sender, e) => {
                    // sets currentRoom to the direction (tuple.Item1)'s key pair (room integer) in the dictionary stored in the current room
                    currentRoom = Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1];
                    // updates the buttons to en/disable directions and change labels
                    UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
                });
                // adds the button to tblLayout at the x/y coordinates stored in the tuple
                tblLayout.Controls.Add(tempButton, tuple.Item2, tuple.Item3);
            }
        }

        private void ShowItemsButton(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // creates a temporary button with blank text
            Button itemButton = CreateButton(tblLayout, "");

            // adds EventHandler to the button to get item and update button
            itemButton.Click += new EventHandler((sender, e) =>
            {
                // adds item to the inventory
                Inventory.Add(Rooms[currentRoom].GetItem());
                // removes the item from the room so it cannot be got again
                Rooms[currentRoom].DelItem();
                // updates the buttons to disable the Get button and updates the label for inventory
                UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
            });
            // adds the button to tblLayout at the specified x/y coordinates
            tblLayout.Controls.Add(itemButton, 3, 2);
        }

        private void UpdateButtonsAndLabels(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalButtons, List<Label> LabelsList)
        {
            Rooms[currentRoom].Found();
            bool Complete = Room.CheckFound(Rooms);
            if (Complete)
            {
                Debug.WriteLine("completed!");
                EndGame(tblLayout);
                return;
            }
            // updates the titles to show what the current room is and inventory contents
            UpdateLabels(LabelsList);
            // en/disables the items button dependant on if there are items in the room
            UpdateItemsButton(tblLayout);
            // en/disables the directions buttons dependant on if travel to each direction is possible
            UpdateDirectionsButtons(tblLayout, DirectionalButtons);
        }
        private void UpdateLabels(List<Label> LabelsList)
        {
            // sets the text of current room label to be the name of the current room
            LabelsList[2].Text = Rooms[currentRoom].GetName();
            // checks if any rooms can be unlocked by sending the inventory to the object and attempting to change based on contents
            Rooms[currentRoom].UnlockRoom(ref Inventory);
            
            // clears the inventory items label
            LabelsList[3].Text = "";
            /* adds the name of each item in inventory to the inventory items label and adds a newline between each one */
            foreach (string s in Inventory)
            {
                LabelsList[3].Text += s;
                LabelsList[3].Text += "\n";
            }
        }
        private void UpdateItemsButton(TableLayoutPanel tblLayout)
        {
            /* checks if the item in the current room has been got by checking if the 'item' ends in t! (from Got!)
               then sets the text to <item name> Got! and disables the button*/
            if (Rooms[currentRoom].GetItem().Substring(Rooms[currentRoom].GetItem().Length - 2, 2) == "t!")
            {
                tblLayout.GetControlFromPosition(3, 2).Text = Rooms[currentRoom].GetItem();
                tblLayout.GetControlFromPosition(3, 2).Enabled = false;
            }
            /* checks if there is an item in the current room (by checking it isn't none)
              then sets text to Get <item name>, enables the button and sets a specific cursor*/
            else if (Rooms[currentRoom].GetItem() != "none")
            {
                tblLayout.GetControlFromPosition(3, 2).Text = "Get " + Rooms[currentRoom].GetItem();
                tblLayout.GetControlFromPosition(3, 2).Enabled = true;
                tblLayout.GetControlFromPosition(3, 2).Cursor = Cursors.Hand;
            }
            /* otherwise it just sets the button text to show there is no item and disables the button */
            else
            {
                tblLayout.GetControlFromPosition(3, 2).Text = "No items available";
                tblLayout.GetControlFromPosition(3, 2).Enabled = false;
            }
        }
        private void UpdateDirectionsButtons(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // loops through every tuple in the list to search all six directions
            foreach (Tuple<string, int, int> tuple in DirectionalButtons)
            {
                /* checks if the room in the direction specified is not negative (meaning nonexistent)
                  then enables the button for the specific x/y coordinates stored and sets specific cursor*/
                if (Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1] > -1)
                {
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Enabled = true;
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Cursor = Cursors.Hand;
                }
                /* if not, it disables the button for the specified x/y coordinates */
                else
                {
                    tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Enabled = false; ;
                }
            }
        }


        private void EndGame(TableLayoutPanel tblLayout)
        {
            //1,2 to 3,4
            for(int x = 1; x < 5; x++)
            {
                for(int y = 1; y < 5; y++)
                {
                    tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(x, y));
                    
                }
            }

            tblLayout.GetControlFromPosition(0, 0).Text = "RPG Designer and Player";


            tblLayout.GetControlFromPosition(0, 5).Show();
            tblLayout.GetControlFromPosition(1, 5).Show();
            tblLayout.GetControlFromPosition(2, 5).Show();
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(0, 5), 1, 2);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(1, 5), 3, 2);
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(2, 5), 2, 4);

            //.Controls.Add(CreateButton(tblLayout, "Exit Game", EndGame), 3, 4);

        }
    }
}
