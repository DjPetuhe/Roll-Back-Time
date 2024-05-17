using System;

[Serializable]
public class RoomCell
{
    public Rooms RoomType;
    public Directions Direction;
    public int TilemapIndex;
    public int PositionI;
    public int PositionJ;
    public bool Cleared;
}