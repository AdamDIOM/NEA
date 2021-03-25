using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA
{
    public struct TempRoom
    {
        public string[] Directions;
        public string Item;
        public string RoomName;
    }
    public partial class RPG : Form
    {
        // creates a stack to store all the incomplete rooms
        // a stack is used so that a depth-first pathway is used for creation, rather than a breadth first (e.g. it will go the most down, then up, then west, then south, then east, then north)
        // use of a Stack
        Stack<TempRoom> TempRooms = new Stack<TempRoom>();
        // creates a list to store all the completed rooms
        List<TempRoom> CompleteRooms = new List<TempRoom>();

        /* this method clears anything that was previously on the display and shows the TextBoxes to input the data per room (and buttons to advance) */
        private void ResetDisplay(TableLayoutPanel tblLayout, bool finishButton = true)
        {
            List<Tuple<string, int, int>> Locations = CreateButtonTuples();

            /* this method clears away the current directional TextBoxes or the header data entry (first time) */
            ClearScreen(tblLayout);
            // removes the Finish Design button, if it exists
            tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(4, 5));
            /* this method shows new directional TextBoxes */
            ShowDirectionalTextBoxes(tblLayout, Locations);
            /* this method shows the next room button */
            AddButton(tblLayout, CreateButton(tblLayout, "Next Room", NextRoom), 4, 4);
            // if it is not the first time the method is run (default) this method shows the end design button
            if (finishButton) AddButton(tblLayout, CreateButton(tblLayout, "Finish Designing", FinishDesign), 4, 5);
        }

        /* this method shows the 6 directional textboxes and updates them accordingly depending on other adjacent rooms */
        private void ShowDirectionalTextBoxes(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalTextBoxes)
        {
            /* this method adds an empty TextBox to input the item into, if there is one */
            CreateTextBox(tblLayout, "Item", "txtItem", 3, 2);

            // only executes this code if there are incomplete rooms to do
            if (TempRooms.Count > 0)
            {
                // selects the first room that is incomplete, removes it from the stack as it will no longer be a temporary room but instead will become a completed room. this also means that later on the temporary room doesn't need to be removed from a collection as it gets removed when it is selected.
                TempRoom r = TempRooms.Pop();

                /* this method creates a title textbox that can't be changed (as the room is already half created) */
                CreateTextBox(tblLayout, r.RoomName, "txtCurrentRoom", 2, 3, editable: false);

                // loops through the six directions for the TextBoxes
                for (int i = 0; i < 6; i++)
                {
                    // if there is not an explicit room in for this direction
                    if (r.Directions[i] == null)
                    {
                        /* this method checks to see if there is a room in the desired direction by going around in a u shape from the original room in the 4 possible directions*/
                        FindImplicitRooms(tblLayout, i, r, DirectionalTextBoxes);
                    }
                    // if the direction is  explicitly defined
                    else
                    {
                        /* this method creates a non-editable textbox with the direction in it */
                        CreateTextBox(tblLayout, r.Directions[i], "txt" + DirectionalTextBoxes[i].Item1, DirectionalTextBoxes[i].Item2, DirectionalTextBoxes[i].Item3, editable: false);

                    }
                }
            }
            // if there are no incomplete rooms (e.g. at the start of the design process or if the user has finished all the previous rooms)
            else
            {
                /* this method adds the empty directional TextBoxes to add a fresh new room */
                BlankInputDisplay(tblLayout, DirectionalTextBoxes);
            }
        }

        /* this method uses logic to try find a room in a desired direction from an original room */
        private void FindImplicitRooms(TableLayoutPanel tblLayout, int desiredDirection, TempRoom originalRoom, List<Tuple<string, int, int>> DirectionalTextBoxes)
        {

            bool found = false;
            // checks loops through all six directions
            for (int a = 0; a < 6; a++)
            {
                // checks to ensure it isn't checking desired direction or the inverse of the desired direction
                if (a != desiredDirection && a != InverseDirection(desiredDirection).Item2)
                {
                    // checks to see if the original room (X) has a room to direction a
                    if (originalRoom.Directions[a] != null && originalRoom.Directions[a] != "*Empty*")
                    {
                        // loops through all of the completed rooms
                        // r.Directions[a] is the name of the room to find
                        /* this method finds a room in direction a from the original room (the first part of the u) */
                        TempRoom first = FindRoom(originalRoom, a, CompleteRooms, TempRooms, 1);

                        // if the first room has a room in the desired direction then check to see if X.a has direction i (desired direction)
                        if (first.Directions[desiredDirection] != null && first.Directions[desiredDirection] != "*Empty*")
                        {
                            /* this method finds a room in the original direction from the first found room (the second part of the u) */
                            TempRoom second = FindRoom(first, desiredDirection, CompleteRooms, TempRooms, 2);
                            //MessageBox.Show("a room has been found (2nd degree)");

                            // then check to see if X.a.i has direction inverse(a) as this is equivalent to X.i
                            if (second.Directions[InverseDirection(a).Item2] != null && second.Directions[InverseDirection(a).Item2] != "*Empty*")
                            {
                                //MessageBox.Show($"Found room in desired direction: {second.Directions[InverseDirection(a).Item2]}");
                                // as this is only needing the name of the room, the FindRoom method is not required and instead a TextBox can be directly created from the name
                                // the TextBox can still be edited just in case there is something different going on with the design, such as misaligned rooms
                                CreateTextBox(tblLayout, second.Directions[InverseDirection(a).Item2], "txt" + DirectionalTextBoxes[desiredDirection].Item1, DirectionalTextBoxes[desiredDirection].Item2, DirectionalTextBoxes[desiredDirection].Item3);
                                found = true;
                            }
                        }
                    }
                }
            }
            if (!found)
            {
                /* this method creates a default TextBox for if there is no explicit or implicit room for the desired direction */
                CreateTextBox(tblLayout, DirectionalTextBoxes[desiredDirection].Item1, "txt" + DirectionalTextBoxes[desiredDirection].Item1, DirectionalTextBoxes[desiredDirection].Item2, DirectionalTextBoxes[desiredDirection].Item3);
            }
        }

        /* this returns either a blank TempRoom or the desired room if it is possible */
        private TempRoom FindRoom(TempRoom originalRoom, int direction, List<TempRoom> CompletedRooms, Stack<TempRoom> IncompleteRooms, int degree)
        {
            TempRoom foundRoom = new TempRoom();

            foreach (TempRoom room in CompletedRooms)
            {
                // if a room with the desired name is found AND that room is connected to the original room
                if (room.RoomName == originalRoom.Directions[direction] && room.Directions[InverseDirection(direction).Item2] == originalRoom.RoomName)
                {
                    //MessageBox.Show($"a complete room has been found ({degree} degree) {room.RoomName}, looking for {direction}");
                    return room;
                }
            }

            foreach (TempRoom room in IncompleteRooms)
            {
                // if a room with the desired name is found AND that room is connected to the original room
                if (room.RoomName == originalRoom.Directions[direction] && room.Directions[InverseDirection(direction).Item2] == originalRoom.RoomName)
                {
                    //MessageBox.Show($"an incomplete room has been found ({degree}st degree) {room.RoomName}, looking for {direction}");
                    return room;
                }
            }

            return foundRoom;
        }

        /* this method shows six empty TextBoxes for data entry */
        private void BlankInputDisplay(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalTextBoxes)
        {
            // loops through each of the six directions in the list
            foreach (Tuple<string, int, int> tuple in DirectionalTextBoxes)
            {
                /* this method creates a temporary TextBox with text set to the direction and text centre-aligned, and adds it to tblLayout */
                CreateTextBox(tblLayout, tuple.Item1, "txt" + tuple.Item1, tuple.Item2, tuple.Item3);
            }
            /* this method creates a TextBox in order to get the current room's name */
            CreateTextBox(tblLayout, "Current Room", "txtCurrentRoom", 2, 3);
        }

        /* this method runs whenever the next room button is pressed -  */
        private void NextRoom(TableLayoutPanel tblLayout)
        {
            /* this method manages the output / storage of the data from the TextBoxes shown on screen and stores it efficiently */
            FinishRoom(tblLayout);
            /* this method prepares the screen for the next room to be input */
            ResetDisplay(tblLayout);
        }

        /* this method organises the data from the TextBoxes on screen to prepare it for permanent storage */
        private void FinishRoom(TableLayoutPanel tblLayout)
        {
            List<Tuple<string, int, int>> Locations = CreateButtonTuples();
            // north, east, south, west, up, down
            // creates a temporary room for storing the data in a structure
            TempRoom room = new TempRoom();

            /* this method gets the value of the six directions (or empty values) */
            room.Directions = StoreDirections(tblLayout, Locations, room);

            /* this method gets the value of the item (or default empty string) */
            room.Item = StoreItem(tblLayout);


            // checks to see if the name of the current room has been changed (must be changed for the room to be complete)
            if (tblLayout.GetControlFromPosition(2, 3).Text == "Current Room")
            {
                // outputs an error message
                MessageBox.Show("Current room needs a name!");
            }
            // if the room name has been changed, finalise the room
            else
            {
                // set RoomName to the input room name
                room.RoomName = tblLayout.GetControlFromPosition(2, 3).Text;
            }
        }

        private string[] StoreDirections(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> Locations, TempRoom room)
        {
            // initialises an array of length six
            string[] directions = new string[6];

            // loops through each of the six directional textboxes
            for (int i = 0; i < 6; i++)
            {
                // checks if the text for that direction hasn't changed (still shows the direction) so there is nothing in that direction
                if (tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text == Locations[i].Item1)
                {
                    // debug feature
                    //Debug.WriteLine("*Empty*");
                    // sets directional room to *Empty*
                    directions[i] = "*Empty*";
                }
                // if the text box has changed from the direction
                else
                {
                    // debug feature
                    // Debug.WriteLine(tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text);
                    // adds the name of the room to the directions array of the temporary room
                    directions[i] = tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text;

                    /* checks all the temprooms to see if there is a room with the name of the new location already existing (but not completed yet) and add the inverse direction to that incomplete room, and if the room does not yet exist it creates it */
                    TempRoom temproom = new TempRoom();
                    bool exists = false;
                    bool complete = false;
                    string[] tempDirections = new string[6];
                    // first check if it is in completerooms or not
                    foreach (TempRoom r in CompleteRooms)
                    {
                        if (r.RoomName == tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text)
                        {
                            complete = true;
                        }
                    }
                    // creates a temporary stack to use whilst finding a desired room
                    Stack<TempRoom> tempStack = new Stack<TempRoom>();

                    // loops through every incomplete room in the stack 
                    while (TempRooms.Count > 0)
                    {
                        // if the current top element is the same as the current direction (so is the desired incomplete room)
                        if (TempRooms.Peek().RoomName == tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text)
                        {
                            // remove the element from the stack and store it separately
                            temproom = TempRooms.Pop();
                            // set exists to true
                            exists = true;
                            // break out of the loop
                            break;
                        }
                        // if it isn't a desired room
                        else
                        {
                            // move the element from the top of the incomplete rooms stack onto the temporary stack
                            tempStack.Push(TempRooms.Pop());
                        }
                    }

                    // loop through every element in the temporary stack
                    while (tempStack.Count > 0)
                    {
                        // remove the element from the temporary stack and add it to the top 
                        TempRooms.Push(tempStack.Pop());
                    }

                    if (!complete)
                    {
                        if (!exists)
                        {
                            temproom = new TempRoom();
                            temproom.RoomName = tblLayout.GetControlFromPosition(Locations[i].Item2, Locations[i].Item3).Text;
                            temproom.Directions = new string[6];
                        }

                        int inverseDirection = InverseDirection(Locations[i].Item1).Item2;
                        temproom.Directions[inverseDirection] = tblLayout.GetControlFromPosition(2, 3).Text;

                        TempRooms.Push(temproom);
                    }


                }
            }

            return directions;
        }

        /* this method returns the item (or the default empty string) */
        private string StoreItem(TableLayoutPanel tblLayout)
        {
            // if the TextBox has not changed
            if (tblLayout.GetControlFromPosition(3, 2).Text == "Item")
            {
                // sets room's item to the default empty string
                return "*Empty*";
            }
            // if the TextBox has changed
            else
            {
                // adds the item name to the temporary room
                return tblLayout.GetControlFromPosition(3, 2).Text;
            }
        }

        
    }
}
