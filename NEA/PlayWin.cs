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
        /* default constructor for struct */
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
        // use of a Delegate
        public WinOption CheckWin;
        public string WinArguments = "";
        public List<WinCombo> WinCombos = new List<WinCombo>();


        /* this method returns the WinCombo for the entered number */
        private WinCombo ReturnWinOptions(char number)
        {
            switch (number)
            {
                case '1':
                    return WinCombos[0];
                case '2':
                    return WinCombos[1];
                case '3':
                    return WinCombos[2];
                default:
                    return WinCombos[0];
            }
        }

        /* this method is used to check if the player has won with option 1 (find all rooms) */
        private bool WinFindAllRooms(TableLayoutPanel tblLayout)
        {
            // debug feature
            Debug.WriteLine("checking win all rooms");
            // uses static method to check if all rooms have been discovered
            bool Complete = Room.CheckFound(Rooms);
            /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
            if (Complete)
            {
                // debug feature
                Debug.WriteLine("completed!");
                /* this method returns the user to an end of game screen */
                ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                return Complete;
            }
            // if not won, returns false so code from base is executed
            return Complete;
        }
        /* this method is used to check if the player has won with option 2 (find an item) */
        private bool WinFindItem(TableLayoutPanel tblLayout)
        {
            Debug.WriteLine("checking win find item");
            foreach(Item i in Inventory)
            {
                /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
                if (i.Name == WinArguments)
                {
                    // debug feature
                    Debug.WriteLine("completed!");
                    /* this method returns the user to an end of game screen */
                    ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                    return true;
                }
            }
            // if not won, returns false so code from base is executed
            return false;
        }
        /* this method is used to check if the player has won with option 3 (find one room) */
        private bool WinFindOneRoom(TableLayoutPanel tblLayout)
        {
            Debug.WriteLine("checking win find one room");
            /* if won, outputs to debug console, runs win code and returns true so that code from base isn't executed */
            if (Rooms[currentRoom].GetName() == WinArguments)
            {
                // debug feature
                Debug.WriteLine("completed!");
                /* this method returns the user to an end of game screen */
                ReturnToMenuPage(tblLayout, GAME_WON_MESSAGE);
                return true;
            }
            // if not won, returns false so code from base is executed
            return false;
        }
    }
}
