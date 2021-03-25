using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace NEA
{
    public partial class RPG : Form
    {
        // creates a string to store the file name so it can be accessed anywhere
        string gameName;
        /* this method shows the controls to input the header data for the file */
        private void DesignData(TableLayoutPanel tblLayout)
        {
            /* this method hides the three standard buttons */
            HideStandardButtons(tblLayout);
            /* creates 3 Labels and 3 TextBoxes for data entry */
            CreateLabel(tblLayout, "Game Name", "lblGameName", 0, 1, alignment: ContentAlignment.MiddleRight, colSpan: 2);
            CreateTextBox(tblLayout, "Enter game name here", "txtGameName", 2, 1, colSpan: 2);

            CreateLabel(tblLayout, "Your name", "lblName", 0, 2, alignment: ContentAlignment.MiddleRight, colSpan: 2);
            CreateTextBox(tblLayout, "Enter your name here", "txtName", 2, 2, colSpan: 2);

            CreateLabel(tblLayout, "Game Description", "lblDescription", 0, 3, alignment: ContentAlignment.MiddleRight, colSpan: 2);
            CreateTextBox(tblLayout, "Describe the game here", "txtName", 2, 3, colSpan: 2, rowSpan: 2, multiLine: true);

            /* adds a Button to advance to the next stage of design */
            AddButton(tblLayout, CreateButton(tblLayout, "Design RPG!", DesignRPG), 2, 5);
        }

        /* this method prepares the screen for data entry */
        private void DesignRPG(TableLayoutPanel tblLayout)
        {
            /* this method outputs the file header info, without the win condition */
            CreateFileHeader(tblLayout);

            /* this method changes the title to "Design RPG" */
            ModifyTitle(tblLayout, "Design RPG");

            /* this method removes the file data controls from the screen and 
             * shows the 6 TextBoxes using a list that stores the six directions and their TableLayoutPanel coordinates and
             * adds the Button required to advance to the following room */
            ResetDisplay(tblLayout, false);
        }

        /* this method takes the input data and writes the base header to the file */
        private void CreateFileHeader(TableLayoutPanel tblLayout)
        {
            // gets the name of the designed game
            gameName = tblLayout.GetControlFromPosition(2, 1).Text;
            // debug feature
            Debug.WriteLine(gameName);
            // gets the name of the user who is designing the game
            string userName = tblLayout.GetControlFromPosition(2, 2).Text;
            // debug feature
            Debug.WriteLine(userName);
            // gets the description of the game
            string description = tblLayout.GetControlFromPosition(2, 3).Text;
            // debug feature
            Debug.WriteLine(description);
            // concatenates the names and description to create the file header - adds a 1 as temporary win clause on the end
            string header = gameName + ";" + userName + ";" + description + ";";

            // creates file to write the first part of the header to - the win condition is determined at the end but is written to the file on the same line
            using (StreamWriter output = new StreamWriter(gameName + ".txt"))
            {
                // writes the header to the file
                output.Write(header);
                // debug feature
                //output.WriteLine("Porch;-1,1,-1,-1,-1,-1");
                //output.WriteLine("Hall;-1,-1,-1,0,-1,-1");
                output.Close();
            }
            //debug feature
            Debug.WriteLine(header);
        }


    }
}
