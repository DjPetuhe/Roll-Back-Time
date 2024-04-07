using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomChooser : MonoBehaviour
{
    [Header("Default UDRL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUDRL;
    [SerializeField] List<Tilemap> defaultRoomsUDRL;

    [Header("Default UDL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUDL;
    [SerializeField] List<Tilemap> defaultRoomsUDL;

    [Header("Default UDR Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUDR;
    [SerializeField] List<Tilemap> defaultRoomsUDR;

    [Header("Default URL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsURL;
    [SerializeField] List<Tilemap> defaultRoomsURL;
    
    [Header("Default DRL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsDRL;
    [SerializeField] List<Tilemap> defaultRoomsDRL;

    [Header("Default UD Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUD;
    [SerializeField] List<Tilemap> defaultRoomsUD;

    [Header("Default UR Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUR;
    [SerializeField] List<Tilemap> defaultRoomsUR;

    [Header("Default UL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsUL;
    [SerializeField] List<Tilemap> defaultRoomsUL;

    [Header("Default DR Rooms")]
    [SerializeField] List<Tilemap> startingRoomsDR;
    [SerializeField] List<Tilemap> defaultRoomsDR;

    [Header("Default DL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsDL;
    [SerializeField] List<Tilemap> defaultRoomsDL;

    [Header("Default RL Rooms")]
    [SerializeField] List<Tilemap> startingRoomsRL;
    [SerializeField] List<Tilemap> defaultRoomsRL;

    [Header("Default U Rooms")]
    [SerializeField] List<Tilemap> startingRoomsU;
    [SerializeField] List<Tilemap> defaultRoomsU;
    [SerializeField] List<Tilemap> bossRoomsU;

    [Header("Default D Rooms")]
    [SerializeField] List<Tilemap> startingRoomsD;
    [SerializeField] List<Tilemap> defaultRoomsD;
    [SerializeField] List<Tilemap> bossRoomsD;

    [Header("Default R Rooms")]
    [SerializeField] List<Tilemap> startingRoomsR;
    [SerializeField] List<Tilemap> defaultRoomsR;
    [SerializeField] List<Tilemap> bossRoomsR;

    [Header("Default L Rooms")]
    [SerializeField] List<Tilemap> startingRoomsL;
    [SerializeField] List<Tilemap> defaultRoomsL;
    [SerializeField] List<Tilemap> bossRoomsL;

    private static readonly List<(int i, int j, Directions d)> AdjacentDirections = new()
    {
        (-1, 0, Directions.U),
        (1, 0, Directions.D),
        (0, 1, Directions.R),
        (0, -1, Directions.L)
    };

    private static readonly System.Random Rand = new();

    public Tilemap ChooseRoom(int i, int j, Rooms[,] level)
    {
        Directions direction = GetDirection(i, j, level);
        List<Tilemap> list = ChooseRoomList(level[i, j], direction);

        if (list.Count <= 0)
            throw new NotImplementedException("These types of rooms for some reason not yet implemented");

        return list[Rand.Next(list.Count)];
    }

    private Directions GetDirection(int i, int j, Rooms[,] level)
    {
        string directions = string.Empty;
        foreach ((int adjI, int adjJ, Directions d) in AdjacentDirections)
        {
            if (i + adjI < 0 || i + adjI >= level.GetLength(0))
                continue;
            if (j + adjJ < 0 || j + adjJ >= level.GetLength(1))
                continue;
            if (level[i + adjI, j + adjJ] != Rooms.Empty)
                directions += d.ToString();
        }
        Enum.TryParse(directions, out Directions dir);
        return dir;
    }

    private List<Tilemap> ChooseRoomList(Rooms room, Directions direction)
    {
        return (room, direction) switch
        {
            (Rooms.Boss, Directions.U) => bossRoomsU,
            (Rooms.Boss, Directions.D) => bossRoomsD,
            (Rooms.Boss, Directions.R) => bossRoomsR,
            (Rooms.Boss, Directions.L) => bossRoomsL,
            (Rooms.Starting, Directions.U) => startingRoomsU,
            (Rooms.Starting, Directions.D) => startingRoomsD,
            (Rooms.Starting, Directions.R) => startingRoomsR,
            (Rooms.Starting, Directions.L) => startingRoomsL,
            (Rooms.Starting, Directions.UD) => startingRoomsUD,
            (Rooms.Starting, Directions.UR) => startingRoomsUR,
            (Rooms.Starting, Directions.UL) => startingRoomsUL,
            (Rooms.Starting, Directions.DR) => startingRoomsDR,
            (Rooms.Starting, Directions.DL) => startingRoomsDL,
            (Rooms.Starting, Directions.RL) => startingRoomsRL,
            (Rooms.Starting, Directions.UDR) => startingRoomsUDR,
            (Rooms.Starting, Directions.UDL) => startingRoomsUDL,
            (Rooms.Starting, Directions.URL) => startingRoomsURL,
            (Rooms.Starting, Directions.DRL) => startingRoomsDRL,
            (Rooms.Starting, Directions.UDRL) => startingRoomsUDRL,
            (Rooms.Default, Directions.U) => defaultRoomsU,
            (Rooms.Default, Directions.D) => defaultRoomsD,
            (Rooms.Default, Directions.R) => defaultRoomsR,
            (Rooms.Default, Directions.L) => defaultRoomsL,
            (Rooms.Default, Directions.UD) => defaultRoomsUD,
            (Rooms.Default, Directions.UR) => defaultRoomsUR,
            (Rooms.Default, Directions.UL) => defaultRoomsUL,
            (Rooms.Default, Directions.DR) => defaultRoomsDR,
            (Rooms.Default, Directions.DL) => defaultRoomsDL,
            (Rooms.Default, Directions.RL) => defaultRoomsRL,
            (Rooms.Default, Directions.UDR) => defaultRoomsUDR,
            (Rooms.Default, Directions.UDL) => defaultRoomsUDL,
            (Rooms.Default, Directions.URL) => defaultRoomsURL,
            (Rooms.Default, Directions.DRL) => defaultRoomsDRL,
            (Rooms.Default, Directions.UDRL) => defaultRoomsUDRL,
            _ => defaultRoomsUDRL
        };
    }
}
