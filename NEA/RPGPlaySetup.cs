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

        private void ChoosePlayFile(TableLayoutPanel tblLayout)
        {
            /* this method moves the original 3 buttons (Designer, Play, Quit) away and hides them */
            HideStandardButtons(tblLayout);

            // adds the three win options to the list
            SetWinOptions();

            // returns file paths of all default game saves and stores them in a string array
            string[] gameFilePaths = ReturnGameFilePaths();
            // loops through all of the default games and adds a play Button and a Label description
            ShowDefaultGames(tblLayout, gameFilePaths);

            // use OpenFileDialogue to allow user to select the file
            AddButton(tblLayout, CreateButton(tblLayout, "Upload", UploadGameFile), 4, 2);

            // adds an exit button to return to main menu
            AddButton(tblLayout, CreateButton(tblLayout, "Return to Menu", EndGame), 4, 4);

        }

        private void SetWinOptions()
        {
            WinOptions.Add(new WinCombo(1, "Explore all rooms to win", WinFindAllRooms));
            WinOptions.Add(new WinCombo(2, "Find item '{data[4]}' to win", WinFindItem));
            WinOptions.Add(new WinCombo(3, "Find room '{data[4]}' to win", WinFindOneRoom));
            Debug.WriteLine("win op len = " + WinOptions.Count());
        }

        private string[] ReturnGameFilePaths()
        {
            // gets the file paths for every game file stored locally
            string[] gameFilePaths = Directory.GetFiles(AppPath + "\\Games\\Default\\", "*.txt");

            // debug features
            Debug.WriteLine(" ");
            Debug.WriteLine(AppPath + "\\Games");
            foreach (string s in gameFilePaths)
            {
                Debug.WriteLine(s + "game");
            }

            return gameFilePaths;
        }

        private void ShowDefaultGames(TableLayoutPanel tblLayout, string[] gameFilePaths)
        {
            for (int a = 0; a < gameFilePaths.Length; a++)
            {
                // adds a button to select each of the individual game saves
                AddButton(tblLayout, CreateGameInfoButton(tblLayout, gameFilePaths[a]), 1, a + 1);
                // returns a string with the author, game description and win condition
                string description = GetFileDescription(gameFilePaths[a]);

                // creates a label using the description created above
                CreateLabel(tblLayout, description, "lblGame" + (a + 1).ToString(), 2, a + 1, colSpan: 2, fontSize: 10, alignment: ContentAlignment.MiddleLeft);
            }
        }

        private Button CreateGameInfoButton(TableLayoutPanel tblLayout, string FilePath)
        {
            // enables reading from files
            StreamReader FileReader = new StreamReader(FilePath);
            // returns the first line of the text file, containing the headers
            string data = FileReader.ReadLine();
            // splits the header into its separate components - Name, Author and Description
            string[] dataSplit = data.Split(';');
            // debug feature
            foreach (string s in dataSplit)
            {
                Debug.WriteLine(s);
            }
            // creates a button with the text being the name of the game
            Button tempbutton = CreateButton(dataSplit[0]);
            // on click it runs method PopulateRoom, sending parameters of TableLayoutPanel and the chosen file path/name
            tempbutton.Click += new EventHandler((sender, e) =>
            {
                Debug.WriteLine("this is a default game being put on screen");
                PopulateRooms(tblLayout, FilePath);
            });

            return tempbutton;
        }
        private void PopulateRooms(TableLayoutPanel tblLayout, string filePath)
        {

            // opens file at location filePath
            StreamReader FileReader = new StreamReader(filePath);
            // takes header data
            string data = FileReader.ReadLine();
            FindWinCondition(data);
            // wipes Rooms and Inventory lists
            Rooms = new List<Room>();
            Inventory = new List<Item>();
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
                // selects which constructor to use dependent on the string array length
                ConstructRoom(lineSplit);
            }

            // begins game
            PlayRPG(tblLayout, data);
        }

        private string GetFileDescription(string currentPath)
        {

            /* reads the first line of the file to get the basic file info (author, description, win conditions) */
            StreamReader reader = new StreamReader(currentPath);
            string[] data = reader.ReadLine().Split(';');
            // closes the file so that it does not cause issues later
            reader.Close();
            string description = "Author: " + data[1];
            description += "\n" + data[2] + "\n";

            // adds the win condition to the label dependant on which of the three win conditions it is
            description += ReturnWinOptions(data[3][0]).Text;
            return description;
        }

        private void ConstructRoom(string[] lineSplit)
        {
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
                                // use of child classes
                                Rooms.Add(new AdvancedRoom(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse), addDataSplit[0], Convert.ToInt32(addDataSplit[1]), addDataSplit[2]));
                                break;
                            // basic Room with an unlockable direction and an item
                            case 4:
                                // use of child classes
                                Rooms.Add(new AdvancedRoom(lineSplit[0], Array.ConvertAll(lineSplit[1].Split(','), int.Parse), addDataSplit[0], Convert.ToInt32(addDataSplit[1]), addDataSplit[2], addDataSplit[3]));
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
        }



        private void UploadGameFile(TableLayoutPanel tblLayout)
        {
            // creates a new OpenFileDialogue (dialogue window to select a file)
            OpenFileDialog OpenFile = new OpenFileDialog();
            // sets the type of file desired (text files)
            OpenFile.Filter = "txt files (*.txt) | *.txt";
            // sets the directory the window opens on to the one where the app is running from
            OpenFile.InitialDirectory = AppPath;
            // opens the dialogue window
            OpenFile.ShowDialog();
            // debug feature
            Debug.WriteLine(OpenFile.FileName);
            // this ensures that PopulateRooms does not run without a valid FileName
            if (OpenFile.FileName != "")
            {
                // runs method sending TableLayoutParameter and the chosen file's path and name
                PopulateRooms(tblLayout, OpenFile.FileName);
            }
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


    }
}
