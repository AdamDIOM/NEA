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
    // use of Struct
    public struct WinCombo
    {
        public int Number;
        public string Text;
        public WinOption Option;
        public WinCombo(int number, string text, WinOption option)
        {
            Number = number;
            Text = text;
            Option = option;
        }
    }
    public partial class RPG : Form
    {
        // use of a Constant
        const string GAME_WON_MESSAGE = "Game won! Congratulations!";
        public WinOption CheckWin;
        public string WinArguments = "";
        public List<WinCombo> WinOptions = new List<WinCombo>();


        private void FindWinCondition(string header)
        {
            string[] headerSplit = header.Split(';');
            // debug feature
            foreach(string s in headerSplit)
            {
                Debug.WriteLine(s);
            }
            // checks the third item of header (win condition) and uses first character incase it is multiple characters  (thus error)
            Debug.WriteLine(headerSplit[3][0]);
            CheckWin = ReturnWinOptions(headerSplit[3][0]).Option;
            /* if win arguments exist (5th element to headersplit) then set them. If not it just goes to catch and carries on */
            try
            {
                WinArguments = headerSplit[4];
                Debug.WriteLine("winargs = " + WinArguments);
            }
            catch { }
        }
        private WinCombo ReturnWinOptions(char number)
        {
            switch (number)
            {
                case '1':
                    return WinOptions[0];
                case '2':
                    return WinOptions[1];
                case '3':
                    return WinOptions[2];
                default:
                    return WinOptions[0];
            }
        }

        private bool WinFindAllRooms(TableLayoutPanel tblLayout)
        {
            Debug.WriteLine("checking win all rooms");
            // uses static method to check if all rooms have been discovered
            bool Complete = Room.CheckFound(Rooms);
            /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
            if (Complete)
            {
                Debug.WriteLine("completed!");
                ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                return Complete;
            }
            // if not won, returns false so code from base is executed
            return Complete;
        }
        private bool WinFindItem(TableLayoutPanel tblLayout)
        {
            Debug.WriteLine("checking win find item");
            foreach(Item i in Inventory)
            {
                /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
                if (i.Name == WinArguments)
                {
                    Debug.WriteLine("completed!");
                    ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                    return true;
                }
            }
            // if not won, returns false so code from base is executed
            return false;
        }
        private bool WinFindOneRoom(TableLayoutPanel tblLayout)
        {
            Debug.WriteLine("checking win find one room");
            /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
            if (Rooms[currentRoom].GetName() == WinArguments)
            {
                Debug.WriteLine("completed!");
                ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                return true;
            }
            // if not won, returns false so code from base is executed
            return false;
        }

        private void ReturnToMenuPage(TableLayoutPanel tblLayout, string outputMessage)
        {
            // removes Controls across the four rows that change
            ClearScreen(tblLayout);

            // shows the user that they have won the map
            CreateLabel(tblLayout, outputMessage, "lblReturn", 1, 2, colSpan:3);

            // creates a button to allow user to end game and return to the main menu
            AddButton(tblLayout, CreateButton(tblLayout, "Return to Menu", EndGame), 2, 3);

        }
    }
}
