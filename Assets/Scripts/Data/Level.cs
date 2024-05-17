using System;

[Serializable]
public class Level
{
    public PlayerStats PlayerStats;
    public float PlayerPosX;
    public float PlayerPosY;
    public RoomCell[] Rooms;
    public int MinI;
    public int MaxI;
    public int MinJ;
    public int MaxJ;
}