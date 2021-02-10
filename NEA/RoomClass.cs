using System;
using System.Collections.Generic;
using System.Diagnostics;
public class Room
{
    private string Name;
    public string Item;
    // use of composition here
    private Dictionary<string, int> RoomDict = new Dictionary<string, int>();
    // use of information hiding as the needed item and the room it unlocks is not revealed
    private string Locked, NeededItem;
    // use of information hiding again
    private int ToUnlock;
    private bool found;
    public Room(string name, int north, int east, int south, int west, int up, int down, string item = "none")
    {
        Name = name;
        RoomDict.Add("North", north);
        RoomDict.Add("East", east);
        RoomDict.Add("South", south);
        RoomDict.Add("West", west);
        RoomDict.Add("Up", up);
        RoomDict.Add("Down", down);
        Item = item;
        found = false;
    }

    public Room(string name, int north, int east, int south, int west, int up, int down, string unlocked, int number, string reqItem, string item = "none")
    {
        Name = name;
        RoomDict.Add("North", north);
        RoomDict.Add("East", east);
        RoomDict.Add("South", south);
        RoomDict.Add("West", west);
        RoomDict.Add("Up", up);
        RoomDict.Add("Down", down);
        Item = item;
        NeededItem = reqItem;
        Locked = unlocked;
        ToUnlock = number;
        found = false;
    }

    public void SetDictionary(Dictionary<string, int> dict)
    {
        RoomDict = dict;
    }

    public Dictionary<string, int> GetRoomsDictionary()
    {
        return RoomDict;
    }

    public void SetItem(string item)
    {
        Item = item;
    }
    public void DelItem()
    {
        Item += " Got!";
    }
    public string GetItem()
    {
        return Item;
    }

    public string GetName()
    {
        return Name;
    }

    public void UnlockRoom(ref List<string> Inventory)
    {
        try
        {
            foreach(string s in Inventory)
            {
                if(s == NeededItem)
                {
                    Inventory.Remove(s);
                    RoomDict[Locked] = ToUnlock;
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
    public void Found()
    {
        found = true;
    }
    public static bool CheckFound(List<Room> Rooms)
    {
        bool allFound = true;
        foreach(Room room in Rooms)
        {
            //Debug.WriteLine(room.Name + ' ' + room.found);
            if (!room.found)
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

