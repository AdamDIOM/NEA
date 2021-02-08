Class Room
{
    private string Name;
    public string Item;
    private int North, East, South, West, Up, Down;
    Public Room(name, north, east, south, west, up, down, item = "none")
    {
        Name = name;
        North = north;
        East = east;
        South = south;
        West = west;
        Up = up;
        Down = down;
        Item = item;
    }

    public void SetItem(item){
        Item = item;
    }
    public string GetItem(){
        return item;
    }

    Public void SetNorth(north){
        North = north;
    }
    Public void SetEast(east){
        East = east;
    }
    Public void SetSouth(south){
        South = south;
    }
    Public void SetWest(west){
        West = west;
    }
    Public void SetUp(up){
        Up = up;
    }
    Public void SetDown(down){
        Down = down;
    }

    Public int GetNorth(){
        return North;
    }
    Public int GetEast(){
        return East;
    }
    Public int GetSouth(){
        return South;
    }
    Public int GetWest(){
        return West;
    }
    Public int GetUp(){
        return Up;
    }
    Public int GetDown(){
        return Down;
    }


}

