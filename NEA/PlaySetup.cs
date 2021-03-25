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
        /* this method sets up infrastructure for the user to choose their desired play file or upload their own */
        private void ChoosePlayFile(TableLayoutPanel tblLayout)
        {
            /* this method moves the original 3 buttons (Designer, Play, Quit) away and hides them */
            HideStandardButtons(tblLayout);

            /* this method adds the three win options to a list */
            SetWinOptions();

            // returns file paths of all default game saves and stores them in a string array */
            string[] gameFilePaths = ReturnGameFilePaths();
            // loops through all of the default games and adds a play Button and a Label description */
            ShowDefaultGames(tblLayout, gameFilePaths);

            // use OpenFileDialogue to allow user to select the file */
            AddButton(tblLayout, CreateButton(tblLayout, "Upload", UploadGameFile), 4, 2);

            // adds an exit button to return to main menu */
            AddButton(tblLayout, CreateButton(tblLayout, "Return to Menu", EndGame), 4, 4);

        }

        /* this method adds three win combinations (number, text and method) to a list */
        private void SetWinOptions()
        {
            /* the three different options to win with */
            WinCombos.Add(new WinCombo(1, "Explore all rooms to win", WinFindAllRooms));
            WinCombos.Add(new WinCombo(2, "Find item '{replace}' to win", WinFindItem));
            WinCombos.Add(new WinCombo(3, "Find room '{replace}' to win", WinFindOneRoom));
            // debug feature
            Debug.WriteLine("win op len = " + WinCombos.Count());
        }

        /* this method returns a string array containing the four default games */
        private string[] ReturnGameFilePaths()
        {
            // gets the file paths for every game file stored locally in the fefault games folder
            string[] gameFilePaths = Directory.GetFiles(AppPath + "\\Games\\Default\\", "*.txt");

            /* debug features */
            Debug.WriteLine(" ");
            Debug.WriteLine(AppPath + "\\Games");
            foreach (string s in gameFilePaths)
            {
                Debug.WriteLine(s + "game");
            }

            return gameFilePaths;
        }

        /* this method shows the four default games on the screen */
        private void ShowDefaultGames(TableLayoutPanel tblLayout, string[] gameFilePaths)
        {
            for (int a = 0; a < gameFilePaths.Length; a++)
            {
                /* this method adds a button to select each of the individual game saves */
                AddButton(tblLayout, CreateGameInfoButton(tblLayout, gameFilePaths[a]), 1, a + 1);
                /* this method returns a string with the author, game description and win condition */
                string description = GetFileDescription(gameFilePaths[a]);

                /* this method creates a label using the description created above */
                CreateLabel(tblLayout, description, "lblGame" + (a + 1).ToString(), 2, a + 1, colSpan: 2, fontSize: 10, alignment: ContentAlignment.MiddleLeft);
            }
        }

        /* this method returns a Button with the game's name, and sets up an EventHandler for when it is clicked */
        private Button CreateGameInfoButton(TableLayoutPanel tblLayout, string FilePath)
        {
            // enables reading from files
            StreamReader FileReader = new StreamReader(FilePath);
            // returns the first line of the text file, containing the headers
            string data = FileReader.ReadLine();
            // splits the header into its separate components - Name, Author and Description
            string[] dataSplit = data.Split(';');
            /* debug feature */
            foreach (string s in dataSplit)
            {
                Debug.WriteLine(s);
            }
            // creates a button with the text being the name of the game
            Button tempbutton = CreateButton(dataSplit[0]);
            // on click it runs method PopulateRoom, sending parameters of TableLayoutPanel and the chosen file path/name
            // use of Lambda expression
            tempbutton.Click += new EventHandler((sender, e) =>
            {
                // debug feature
                Debug.WriteLine("this is a default game being put on screen");
                /* this method runs when the button is clicked */
                PopulateRooms(tblLayout, FilePath);
            });

            return tempbutton;
        }

        /* this method gets the description of the game file from the input file path */
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
            // replaces {replace} with the specific data - uses try/catch to ignore strings that don't have {replace} or a 4th element to data[]
            try
            {
                description = description.Replace("{replace}", data[4]);
            }
            catch { }

            Debug.WriteLine(description);
            return description;
        }

        /* this method reads the chosen file (either default or uploaded), finds the win condition and fills the lists with the room data */
        private void PopulateRooms(TableLayoutPanel tblLayout, string filePath)
        {

            // opens file at location filePath
            StreamReader FileReader = new StreamReader(filePath);
            // takes header data
            string data = FileReader.ReadLine();
            FindWinCondition(data);
            /* wipes Rooms and Inventory lists */
            Rooms = new List<Room>();
            Inventory = new List<Item>();
            // loops through entire text tile
            while (!FileReader.EndOfStream)
            {
                // reads each line
                string line = FileReader.ReadLine();
                // splits the line into program understandable chunks
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

        /* this method finds the win condition from a string and sets the delegate to the desired condition */
        private void FindWinCondition(string header)
        {
            // splits the input string up by semicolons into understandable pieces
            string[] headerSplit = header.Split(';');
            // debug feature
            foreach (string s in headerSplit)
            {
                Debug.WriteLine(s);
            }
            // checks the third item of header (win condition) and uses first character incase it is multiple characters  (thus error)
            Debug.WriteLine(headerSplit[3][0]);
            /* this method returns the WinOption (custom struct) dependant on the input number */
            // use of Delegates
            CheckWin = ReturnWinOptions(headerSplit[3][0]).Option;
            /* if win arguments exist (5th element to headersplit) then set them. If not it just goes to catch and carries on */
            try
            {
                WinArguments = headerSplit[4];
                // debug feature
                Debug.WriteLine("winargs = " + WinArguments);
            }
            catch
            {
                // if there is an error, the default win option is assumed (find all rooms)
                CheckWin = WinCombos[0].Option;
            }
        }

        /* this method determines which type of room to create and adds that to the rooms List */
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

        /* this method runs if the user decides to upload their own game file */
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
            // this ensures that PopulateRooms does not run without a valid FileName (e.g. if the user cancels file selection)
            if (OpenFile.FileName != "")
            {
                /* this method populates the rooms list using the chosen file's path and name */
                PopulateRooms(tblLayout, OpenFile.FileName);
            }
        }
    }
}
