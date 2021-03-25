using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA
{
    
    // use of delegate at namespace level to use across class and struct
    public delegate bool WinOption(TableLayoutPanel tblLayout);
    public partial class RPG : Form
    {
        
        /* this method prepares the screen for playing */
        private void PlayRPG(TableLayoutPanel tblLayout, string gameData)
        {
            // sets the current position to be the starting room
            currentRoom = 0;

            /* this method changes the title to "Play RPG" */
            ModifyTitle(tblLayout, "Play RPG");

            /* this method clears the screen so the directional buttons can appear */
            ClearScreen(tblLayout);

            /* this function returns a list of tuples containing room name and tblLayout x coord and y coord */
            List<Tuple<string, int, int>> DirectionalButtons = CreateButtonTuples();

            /* this function returns a list of four labels: 'Current Room: ', current room, 'Inventory: ', inventory items; and shows them on screen */
            List<Label> LabelsList = ShowPlayLabels(tblLayout, gameData);

            /* this method loops through the list of tuples containing the button names and tblLayout coordinates and shows and en/disables each one */
            ShowButtons(tblLayout, LabelsList, DirectionalButtons);
        }
        //-----------------------------------------------add events to the different rooms?--------------------------------------------------------------------------------------------------------

        /* this method returns a List of Labels containing info about the game */
        private List<Label> ShowPlayLabels(TableLayoutPanel tblLayout, string gameData)
        {
            // creates a list to contain all of the labels
            List<Label> LabelsList = new List<Label>();

            /* this method creates the title label 'Current Room:' */
            LabelsList.Add(CreateLabel(tblLayout, "Current Room:", "lblCurrentRoom", 1, 1, colSpan:2));

            /* this method creates the title label 'Inventory:' */
            LabelsList.Add(CreateLabel(tblLayout, "Inventory:", "lblInventory", 4, 1));

            /* this method creates the label to show the current room */
            LabelsList.Add(CreateLabel(tblLayout, Rooms[currentRoom].GetName(), "lblRoom", 3, 1));

            /* this method creates the label to show items in the inventory */
            LabelsList.Add(CreateLabel(tblLayout, "", "lblInvItems", 4, 2));

            /* this method creates the title label 'Aim:' */
            LabelsList.Add(CreateLabel(tblLayout, "Aim:", "lblAim", 0, 1));

            // splits the file data into its separate chunks (name, author, description and win condition)
            string[] splitData = gameData.Split(';');
            /* this method selects the aim of the game from three options */
            string Aim = ReturnWinOptions(splitData[3][0]).Text;

            // replaces {replace} with the specific data - uses try/catch to ignore strings that don't have {replace} or a 4th element to data[]
            try
            {
                Aim = Aim.Replace("{replace}", splitData[4]);
            }
            catch { }

            /* this method creates the label to show the aim of the game */
            LabelsList.Add(CreateLabel(tblLayout, Aim, "lblGameAim", 0, 2, rowSpan:2));

            // returns the List of Labels to where the function was called from
            return LabelsList;
        }

        /* this method shows the Buttons on screen */
        private void ShowButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            /* this method adds exit Button to the TableLayoutPanel */
            AddButton(tblLayout, CreateButton(tblLayout, "Exit Game", EndGame), 3, 4);
            // use of decomposition here
            /* this method shows the six direction Buttons */
            ShowDirectionalButtons(tblLayout, LabelsList, DirectionalButtons);
            /* this method prepares a button to be used for items */
            ShowItemsButton(tblLayout, LabelsList, DirectionalButtons);
            /* this method updates all the Buttons/Labels on screen as the user moves room or gets items. */
            UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
        }

        /* this method outputs 6 buttons to the screen for the user to be able to click */
        private void ShowDirectionalButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // loops through each tuple in the list, which contains the name and tblLayout x/y coordinates
            foreach (Tuple<string, int, int> tuple in DirectionalButtons)
            {
                /* this method creates a temporary Button with text 'Go <direction>' */
                string go = UpdateDirectionalText(tuple);
                
                Button tempButton = CreateButton(go);
                // adds EventHandler to the button to change room
                tempButton.Click += new EventHandler((sender, e) => {
                    // sets currentRoom to the direction (tuple.Item1)'s key pair (room integer) in the dictionary stored in the current room
                    currentRoom = Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1];
                    /* this method updates the buttons to en/disable directions and change labels */
                    UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
                });
                /* this method adds the button to tblLayout at the x/y coordinates stored in the tuple */
                AddButton(tblLayout, tempButton, tuple.Item2, tuple.Item3);
            }
        }
        
        /* this method returns the text to show on a button based upon a tuple sent in as a parameter */
        private string UpdateDirectionalText(Tuple<string, int, int> tuple)
        {
            
            // creates a temporary string 'Go < direction >'
            string go = "Go " + tuple.Item1;
            // this checks if there is a room in the specified direction
            if(Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1] > -1)
            {
                // this checks if the room has been discovered (visited) before
                if (Rooms[Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1]].IsFound())
                {
                    // sets the text to Go < direction > (< room name >)
                    go += "\n (" + Rooms[Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1]].GetName() + ")";
                }
                else
                {
                    // sets the text to Go < direction > (Unknown)
                    go += "\n (Unknown)";
                }
            }
            return go;
        }

        /* this method a blank item Button */
        private void ShowItemsButton(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // creates a temporary button with blank text
            Button itemButton = CreateButton("");

            // adds EventHandler to the button to get item and update button
            itemButton.Click += new EventHandler((sender, e) =>
            {
                // adds item to the inventory
                Inventory.Add(Rooms[currentRoom].GetItem());
                // removes the item from the room so it cannot be got again
                Rooms[currentRoom].DelItem();
                /* this method updates the buttons to disable the Get button and updates the label for inventory */
                UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
            });
            /* this method adds the button to tblLayout at the specified x/y coordinates */
            AddButton(tblLayout, itemButton, 3, 2);
        }

        /* this method is called when the game starts and whenever a gameplay button is pressed */
        private void UpdateButtonsAndLabels(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalButtons, List<Label> LabelsList)
        {
            // changes status of the visited room to Found to allow for win detection and change of directional buttons
            Rooms[currentRoom].RoomFound();
            /* this method checks the win condition (using a delegate) based upon method specified in game source file */
            // returns true if won so that following code is not executed
            if (CheckWin(tblLayout)) return;
            
            /* this method updates the titles to show what the current room is and inventory contents */
            UpdateLabels(LabelsList);
            /* this method en/disables the items button dependant on if there are items in the room */
            UpdateItemsButton(tblLayout);
            /* this method en/disables the directions buttons dependant on if travel to each direction is possible */
            UpdateDirectionsButtons(tblLayout, DirectionalButtons);
        }
        /* this method changes the text labels on the screen */
        private void UpdateLabels(List<Label> LabelsList)
        {
            // sets the text of current room label to be the name of the current room
            LabelsList[2].Text = Rooms[currentRoom].GetName();
            // checks if any rooms can be unlocked by sending the inventory to the object and attempting to change based on contents
            // use of Reflection
            if (Rooms[currentRoom].GetType().ToString() == "AdvancedRoom") Rooms[currentRoom].UnlockRoom(ref Inventory);
            
            // clears the inventory items label
            LabelsList[3].Text = "";
            /* adds the name of each item in inventory to the inventory items label and adds a newline between each one */
            foreach (Item i in Inventory)
            {
                LabelsList[3].Text += i.Name + "\n";
            }
        }
        /* this method enables or disables the item button if there is/isn't an item in the current room */
        private void UpdateItemsButton(TableLayoutPanel tblLayout)
        {
            /* checks if the item in the current room has been found
               then sets the text to <item name> Got! and disables the button*/
            if (Rooms[currentRoom].GetItem().Found == true)
            {
                tblLayout.GetControlFromPosition(3, 2).Text = Rooms[currentRoom].GetItem().Name + " Got!";
                tblLayout.GetControlFromPosition(3, 2).Enabled = false;
            }
            /* checks if there is an item in the current room (by checking it isn't none)
              then sets text to Get <item name>, enables the button and sets a specific cursor*/
            else if (Rooms[currentRoom].GetItem().Name != "none")
            {
                tblLayout.GetControlFromPosition(3, 2).Text = "Get " + Rooms[currentRoom].GetItem().Name;
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
        /* this method updates each of the six directional buttons */
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
                /* this method changes the text value displayed on the button */
                tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Text = UpdateDirectionalText(tuple);
            }
        }
    }
}
