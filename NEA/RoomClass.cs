using System.Collections.Generic;
using System.Diagnostics;

public struct Item
{
    public string Name;
    public bool Found;
}
public class AdvancedRoom : Room
{
    // use of Information Hiding as the needed item and the room it unlocks is not revealed
    private string Locked, NeededItem;
    // use of Information Hiding again
    private int ToUnlock;
    public AdvancedRoom(string name, int[] directions, string roomToUnlock, int lockedRoomNumber, string reqItem, string item = "none") : base(name, directions, item)
    {
        // constructor for the more advanced options - rooms that can be unlocked
        Locked = roomToUnlock;
        ToUnlock = lockedRoomNumber;
        NeededItem = reqItem;
    }

    public override void UnlockRoom(ref List<Item> Inventory)
    {
        Debug.WriteLine("child");
        // use of Defensive Programming
        try
        {
            // loops through every item in the inventory
            foreach (Item i in Inventory)
            {
                /* if the current item from the inventory is the Needed Item to unlock the room, then the room dictionary is adjusted so that the new room number is applied */
                if (i.Name == NeededItem)
                {
                    Inventory.Remove(i);
                    RoomDict[Locked] = ToUnlock;
                    // debug features
                    Debug.WriteLine(Locked);
                    Debug.WriteLine(ToUnlock);
                    break;
                }
            }
        }
        catch
        {
            Debug.WriteLine("no item required");
        }
    }
}

public class Room
{
    private string Name;
    // use of Composition here
    protected Dictionary<string, int> RoomDict = new Dictionary<string, int>();
    public Item Item;
    private bool Found;

    // constructor takes string for name, array of 6 directions and adds to dictionary and an optional item parameter
    public Room(string name, int[] directions, string item = "none")
    {
        Name = name;
        RoomDict.Add("North", directions[0]);
        RoomDict.Add("East", directions[1]);
        RoomDict.Add("South", directions[2]);
        RoomDict.Add("West", directions[3]);
        RoomDict.Add("Up", directions[4]);
        RoomDict.Add("Down", directions[5]);
        Item.Name = item;
        Item.Found = false;
        // always sets the room's default found to false as no rooms are discovered initially
        // use of this to express clarity between Item.Found and Room.Found
        this.Found = false;
    }
    // use of Polymorphism
    public virtual void UnlockRoom(ref List<Item> Inventory)
    {
        // debug feature
        Debug.WriteLine("no rooms to unlock");
    }

    // should never be needed but is available for future refactoring
    public void SetDictionary(Dictionary<string, int> dict)
    {
        RoomDict = dict;
    }

    // used for checking room directions and items
    public Dictionary<string, int> GetRoomsDictionary()
    {
        return RoomDict;
    }

    public void SetItem(Item item)
    {
        Item = item;
    }
    // changes the name of the item so that it is recognised as unavailable by the Get Item button
    public void DelItem()
    {
        Item.Found = true;
    }
    public Item GetItem()
    {
        return Item;
    }

    public string GetName()
    {
        return Name;
    }

    
    public void RoomFound()
    {
        Found = true;
    }
    public bool IsFound()
    {
        return Found;
    }
    // use of static method
    public static bool CheckFound(List<Room> Rooms)
    {
        bool allFound = true;
        foreach(Room room in Rooms)
        {
            //Debug.WriteLine(room.Name + ' ' + room.found);
            if (!room.Found)
            {
                allFound = false;
                break;
            }
        }
        return allFound;
    }

    #region set directions
    public void SetNorth(int north)
    {
        RoomDict["North"] = north;
    }
    public void SetEast(int east)
    {
        RoomDict["East"] = east;
    }
    public void SetSouth(int south)
    {
        RoomDict["South"] = south;
    }
    public void SetWest(int west)
    {
        RoomDict["West"] = west;
    }
    public void SetUp(int up)
    {
        RoomDict["Up"] = up;
    }
    public void SetDown(int down)
    {
        RoomDict["Down"] = down;
    }
    #endregion

    #region get directions
    public int GetNorth()
    {
        return RoomDict["North"];
    }
    public int GetEast()
    {
        return RoomDict["East"];
    }
    public int GetSouth()
    {
        return RoomDict["South"];
    }
    public int GetWest()
    {
        return RoomDict["West"];
    }
    public int GetUp()
    {
        return RoomDict["Up"];
    }
    public int GetDown()
    {
        return RoomDict["Down"];
    }
    #endregion
}

