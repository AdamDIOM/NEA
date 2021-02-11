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
        private void PlayRPG(TableLayoutPanel tblLayout)
        {
            // add intermediary window to allow user to select a specific game or upload their own...
            // use OpenFileDialogue to allow user to select the file
            // temporary rooms data reset

            Rooms = PopulateRooms(Rooms);
            
            // sets the current position to be the starting room
            currentRoom = 0;

            /* this method changes the title to "Play RPG" */
            ModifyTitle(tblLayout, "Play RPG");

            /* this method moves the original 3 buttons (Designer, Play, Quit) away and hides them */
            HideStandardButtons(tblLayout);

            /* this function returns a list of tuples containing room name and tblLayout x coord and y coord */
            List<Tuple<string, int, int>> DirectionalButtons = CreateButtonTuples();

            /* this function returns a list of four labels: 'Current Room: ', current room, 'Inventory: ', inventory items; and shows them on screen */
            List<Label> LabelsList = ShowPlayLabels(tblLayout);

            /* this method loops through the list of tuples containing the button names and tblLayout coordinates and shows and en/disables each one */
            ShowButtons(tblLayout, LabelsList, DirectionalButtons);
        }

        private void ChooseFile()
        {
            string[] gameFilePaths = ReturnGameFilePaths();

            Button button = CreateGameInfoButton(gameFilePaths[0]);

        }
        private Button CreateGameInfoButton(string FilePath) 
        {
            StreamReader FileReader = new StreamReader(FilePath);
            string data = FileReader.ReadLine();
            string[] dataSplit = data.Split(';');
            foreach(string s in dataSplit)
            {
                Debug.WriteLine(s);
            }

            return new Button();
        }
        private string[] ReturnGameFilePaths()
        {
            // gets the file paths for every game file stored locally
            string[] gameFilePaths = Directory.GetFiles(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Games\\", "*.txt");

            // debug features
            Debug.WriteLine(" ");
            Debug.WriteLine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Games");
            foreach (string s in gameFilePaths)
            {
                Debug.WriteLine(s + "game");
            }

            return gameFilePaths;
        }

        private List<Room> PopulateRooms(List<Room> Rooms)
        {
            ChooseFile();

            // creates temp filename
            string filename = "Games/Game1.txt";
            // opens file called filename
            StreamReader FileReader = new StreamReader(filename);
            // takes header data
            string data = FileReader.ReadLine();
            // wipes Rooms list
            Rooms = new List<Room>();
            // loops through entire text tile
            while (!FileReader.EndOfStream)
            {
                // reads each line
                string line = FileReader.ReadLine();
                // splits the line into understandable chunks
                string[] lineSplit = line.Split(';');
                // debug feature
                foreach (string newl in lineSplit)
                {
                    //Debug.WriteLine(newl);
                }
                // use polymorphism here with basic room vs advanced rooms-----------------------------------------------------------------------------------------------------------------------------------
                // selects which constructor to use dependent on the string array length
                switch (lineSplit.Length)
                {
                    // basic Room - name and 6 directions
                    case 2:
                        // adds basic Room to Rooms list
                        Rooms.Add(new Room(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse)));
                        break;
                    // advanced Room - name, 6 directions and additional data
                    case 3:
                        string[] addDataSplit = lineSplit[2].Split(',');
                        // selects which constructor to use dependent on additional data length
                        switch (addDataSplit.Length)
                        {
                            // basic Room with an item
                            case 1:
                                Rooms.Add(new Room(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse), addDataSplit[0]));
                                break;
                            // basic Room with an unlockable direction
                            case 3:
                                Rooms.Add(new Room(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse), addDataSplit[0], Convert.ToInt32(addDataSplit[1]), addDataSplit[2]));
                                break;
                            // basic Room with an unlockable direction and an item
                            case 4:
                                Rooms.Add(new Room(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse), addDataSplit[0], Convert.ToInt32(addDataSplit[1]), addDataSplit[2], addDataSplit[3]));
                                break;
                            // unrecognisable additional data format
                            default:
                                Debug.WriteLine(lineSplit[0] + "has problems");
                                break;
                        }
                        break;
                    // unrecognisable room format
                    default:
                        Debug.WriteLine(lineSplit[0] + "has problems");
                        break;
                }
                //Rooms.Add(new Room(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse)));
            }
            return Rooms;
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
            LabelsList.Add(CreateLabel(tblLayout, "Current Room:", "lblCurrentRoom", 1, 1, 2));

            /* creation of the title label 'Inventory:' */
            LabelsList.Add(CreateLabel(tblLayout, "Inventory", "lblInventory", 4, 1));

            /* creation of the label to show the current room */
            LabelsList.Add(CreateLabel(tblLayout, Rooms[currentRoom].GetName(), "lblRoom", 3, 1));

            /* creation of the label to show items in the inventory */
            LabelsList.Add(CreateLabel(tblLayout, "", "lblInvItems", 4, 2));

            // returns the list of four labels to where the function was called from
            return LabelsList;
        }

        private void ShowButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // adds button to TableLayoutPanel
            AddButton(tblLayout, CreateButton(tblLayout, "Exit Game", EndGame), 3, 4);
            // use of decomposition here
            ShowDirectionalButtons(tblLayout, LabelsList, DirectionalButtons);
            ShowItemsButton(tblLayout, LabelsList, DirectionalButtons);
            UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
        }

        private void ShowDirectionalButtons(TableLayoutPanel tblLayout, List<Label> LabelsList, List<Tuple<string, int, int>> DirectionalButtons)
        {
            // loops through each tuple in the list, which contains the name and tblLayout x/y coordinates
            foreach (Tuple<string, int, int> tuple in DirectionalButtons)
            {
                // creates a temporary button with text 'Go <direction>'
                string go = UpdateDirectionalText(tuple);
                
                Button tempButton = CreateButton(tblLayout, go);
                // adds EventHandler to the button to change room
                tempButton.Click += new EventHandler((sender, e) => {
                    // sets currentRoom to the direction (tuple.Item1)'s key pair (room integer) in the dictionary stored in the current room
                    currentRoom = Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1];
                    // updates the buttons to en/disable directions and change labels
                    UpdateButtonsAndLabels(tblLayout, DirectionalButtons, LabelsList);
                });
                // adds the button to tblLayout at the x/y coordinates stored in the tuple
                AddButton(tblLayout, tempButton, tuple.Item2, tuple.Item3);
            }
        }

        private string UpdateDirectionalText(Tuple<string, int, int> tuple)
        {
            
            // creates a temporary string 'Go < direction >'
            string go = "Go " + tuple.Item1;
            if(Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1] > -1)
            {
                if (Rooms[Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1]].IsFound())
                {
                    go += "\n (" + Rooms[Rooms[currentRoom].GetRoomsDictionary()[tuple.Item1]].GetName() + ")";
                }
                else
                {
                    go += "\n (Unknown)";
                }
            }
            return go;
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
            AddButton(tblLayout, itemButton, 3, 2);
        }

        private void UpdateButtonsAndLabels(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalButtons, List<Label> LabelsList)
        {
            Rooms[currentRoom].Found();
            bool Complete = Room.CheckFound(Rooms);
            if (Complete)
            {
                Debug.WriteLine("completed!");
                GameWon(tblLayout);
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
                LabelsList[3].Text += s + "\n";
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
                tblLayout.GetControlFromPosition(tuple.Item2, tuple.Item3).Text = UpdateDirectionalText(tuple);
            }
        }

        private void GameWon(TableLayoutPanel tblLayout)
        {
            // removes Controls across the four rows that change
            ClearScreen(tblLayout);

            // shows the user that they have won the map
            CreateLabel(tblLayout, "Game won! Congratulations!", "lblWon", 1, 2, 3);

            // creates a button to allow user to end game and return to the main menu
            AddButton(tblLayout, CreateButton(tblLayout, "Return to Menu", EndGame), 2, 3);

        }

        private void EndGame(TableLayoutPanel tblLayout)
        {
            // removes Controls across the four rows that change
            ClearScreen(tblLayout);

            // changes title to the original title
            ModifyTitle(tblLayout, "RPG Designer and Player");

            /* moves three main controls back to original positions */
            ShowStandardButtons(tblLayout);
        }
    }
}
