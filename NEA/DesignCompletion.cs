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

        /* this method runs whenever the finish designing button is clicked, it either finalises the design or preps the user for another design */
        private void FinishDesign(TableLayoutPanel tblLayout)
        {
            /* this method runs the code to finalise the room */
            FinishRoom(tblLayout);
            // if there are no rooms in the TempRoom stack
            if (TempRooms.Count == 0)
            {
                // debug feature
                //MessageBox.Show("finished");
                /* this method progresses on to finalise the header for the output file */
                FindWinCondition(tblLayout);
            }
            // if there are still rooms in the TempRooms stack
            else
            {
                // output an error message to the user
                MessageBox.Show("Incomplete Rooms!");
                /* this method clears the window to get it ready for continued data entry */
                ResetDisplay(tblLayout);
            }


        }

        // use of overloading to have the same name method to do two different things
        private void FindWinCondition(TableLayoutPanel tblLayout)
        {
            // removes the 'Finish Design' button
            tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(4, 5));
            /* this method removes the design TextBoxes from the screen */
            ClearScreen(tblLayout);

            // creates a subtitle Label
            CreateLabel(tblLayout, "Win condition", "lblWinCondition", 1, 1, colSpan: 3, fontSize: 24.0F);

            // creates a dropdown menu to choose the win option
            ComboBox WinOptions = new ComboBox();

            // sets the style of the ComboBox
            WinOptions.DropDownStyle = ComboBoxStyle.DropDownList;
            // sets the font styling of the ComboBox
            WinOptions.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            // sets the ComboBox to span 3 columns
            tblLayout.SetColumnSpan(WinOptions, 3);

            /* adds the options for the ComboBox */
            WinOptions.Items.Add("Find all rooms");
            WinOptions.Items.Add("Find item");
            WinOptions.Items.Add("Find rooms");
            WinOptions.SelectedItem = "Find all rooms";

            // sets the anchor styling so the ComboBox fills the whole allocated space
            WinOptions.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);

            // sets an EventHandler to handle what happens when the ComboBox changes
            // use of Lambda expressions
            WinOptions.SelectedIndexChanged += new EventHandler((sender, e) => WinOptionsUpdate(tblLayout));

            // adds the ComboBox to the TableLayoutPanel
            tblLayout.Controls.Add(WinOptions, 1, 2);

            // adds a button to finalise design
            AddButton(tblLayout, CreateButton(tblLayout, "Complete Design!", WinConditionOutput), 2, 5);
        }

        /* this method runs every time the dropdown box to choose a win condition changes */
        //-change find item and find room to instead use a dropdown menu to only select rooms and items that exist in the designed game--------------------------------------------
        private void WinOptionsUpdate(TableLayoutPanel tblLayout)
        {
            // debug feature
            Debug.WriteLine("text changed");
            // finds the win option from the dropdown menu
            string winOp = tblLayout.GetControlFromPosition(2, 2).Text;
            // clears any previous input boxes that may have been on the screen below the dropdown menu
            ClearScreen(tblLayout, yMin: 3, yMax: 4, removeReturn: false);
            // finds which win option it is
            switch (winOp)
            {
                case "Find all rooms":
                    break;
                case "Find item":
                    // creates a TextBox to enter the item to find to win
                    CreateTextBox(tblLayout, "item", "txtItemEntry", 2, 3);
                    break;
                case "Find rooms":
                    // creates a TextBox to enter the room to find to win
                    CreateTextBox(tblLayout, "room", "txtRoomEntry", 2, 3);
                    break;
                // this should never be used but if it does it just follows the conventions of Find all rooms
                default:
                    break;
            }
        }

        /* this method outputs the win condition onto the end of the file header */
        private void WinConditionOutput(TableLayoutPanel tblLayout)
        {
            // gets the win option from the ComboBox
            string winOp = tblLayout.GetControlFromPosition(2, 2).Text;
            // initialises a string ready to output to permanent storage
            string outputText;
            // prepares for the output
            switch (winOp)
            {
                case "Find all rooms":
                    // output string becomes just 1 as this is how find all rooms is defined in the play conditions
                    outputText = "1";
                    break;
                case "Find item":
                    // output string becomes 2 as this is the identifier for find an item
                    outputText = "2;";
                    // adds the item from the input box to the output string
                    outputText += tblLayout.GetControlFromPosition(2, 3).Text;
                    break;
                case "Find rooms":
                    // output string becomes 2 as this is the identifier for find a room
                    outputText = "3;";
                    // adds the room from the input box to the output string
                    outputText = tblLayout.GetControlFromPosition(2, 3).Text;
                    break;
                // this should not be run however just takes default find all rooms output
                default:
                    outputText = "1";
                    break;
            }


            using (StreamWriter output = new StreamWriter(gameName + ".txt", true))
            {
                // writes the header to the file
                output.WriteLine(outputText);
                output.Close();
            }

            Debug.WriteLine(winOp);

            CompleteDesign(tblLayout);

        }

        /* this method runs when the user has finalised the win condition and the file is ready to output the rooms */
        private void CompleteDesign(TableLayoutPanel tblLayout)
        {
            // clears the screen to hide the win condition questions and buttons
            ClearScreen(tblLayout);

            // loops through every room in the CompleteRooms list
            for (int roomIndex = 0; roomIndex < CompleteRooms.Count; roomIndex++)
            {
                // sets string currentroom to the room we are looking for
                string currentRoom = CompleteRooms[roomIndex].RoomName;
                // debug feature
                Debug.WriteLine("current room " + currentRoom);
                // loops through the six directions
                for (int i = 0; i < 6; i++)
                {
                    /* if the direction in the current room is empty, set it to -1 */
                    if (CompleteRooms[roomIndex].Directions[i] == "*Empty*")
                    {
                        CompleteRooms[roomIndex].Directions[i] = "-1";
                    }
                    // loops through every room in the CompleteRooms list
                    for (int roomIndex2 = 0; roomIndex2 < CompleteRooms.Count; roomIndex2++)
                    {
                        /* if the text for the room in this inner loop is the name of the room in the outer loop, change the text to the number that the outer loop is */
                        if (CompleteRooms[roomIndex2].Directions[i] == currentRoom)
                        {
                            CompleteRooms[roomIndex2].Directions[i] = roomIndex.ToString();
                        }
                    }

                }

                /* debug features */
                Debug.Write(CompleteRooms[roomIndex].RoomName);
                foreach (string s in CompleteRooms[roomIndex].Directions) Debug.Write(s);
                Debug.WriteLine("");
            }

            OutputDesign();
            /* this method takes the user back to the original screen to design a new game, play a game or quit the application */
            ReturnToMenuPage(tblLayout, "Designing Complete!");
        }

        /* this method outputs all the completed rooms to the persistent data store (text file) */
        private void OutputDesign()
        {
            // creates an instance of an IO device to output the rooms to a file
            using (StreamWriter output = new StreamWriter(gameName + ".txt", true))
            {
                // loops through every room in the list of completed rooms
                foreach (TempRoom r in CompleteRooms)
                {
                    /* creates a string in the following format: <room name>;<d1>,<d2>,<d3>,<d4>,<d5>,<d6>, */
                    string toOutput = "";
                    toOutput += r.RoomName;
                    toOutput += ";";
                    foreach (string s in r.Directions)
                    {
                        toOutput += s;
                        toOutput += ",";
                    }
                    // this removes the extra comma on the end
                    toOutput = toOutput.TrimEnd(',');

                    /* if there is an item available, adds ;<item> to the string */
                    if (r.Item != "*Empty*")
                    {
                        toOutput += ";";
                        toOutput += r.Item;
                    }

                    // writes the string to the file
                    output.WriteLine(toOutput);
                }
            }

        }

    }
}
