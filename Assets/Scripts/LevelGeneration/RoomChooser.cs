using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomChooser : MonoBehaviour
{
    [Header("UDRL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUDRL;
    [SerializeField] List<Tilemap> DefaultRoomsUDRL;

    [Header("UDL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUDL;
    [SerializeField] List<Tilemap> DefaultRoomsUDL;

    [Header("UDR Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUDR;
    [SerializeField] List<Tilemap> DefaultRoomsUDR;

    [Header("URL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsURL;
    [SerializeField] List<Tilemap> DefaultRoomsURL;
    
    [Header("DRL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsDRL;
    [SerializeField] List<Tilemap> DefaultRoomsDRL;

    [Header("UD Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUD;
    [SerializeField] List<Tilemap> DefaultRoomsUD;

    [Header("UR Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUR;
    [SerializeField] List<Tilemap> DefaultRoomsUR;

    [Header("UL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsUL;
    [SerializeField] List<Tilemap> DefaultRoomsUL;

    [Header("DR Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsDR;
    [SerializeField] List<Tilemap> DefaultRoomsDR;

    [Header("DL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsDL;
    [SerializeField] List<Tilemap> DefaultRoomsDL;

    [Header("RL Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsRL;
    [SerializeField] List<Tilemap> DefaultRoomsRL;

    [Header("U Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsU;
    [SerializeField] List<Tilemap> DefaultRoomsU;
    [SerializeField] List<Tilemap> BossRoomsU;

    [Header("D Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsD;
    [SerializeField] List<Tilemap> DefaultRoomsD;
    [SerializeField] List<Tilemap> BossRoomsD;

    [Header("R Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsR;
    [SerializeField] List<Tilemap> DefaultRoomsR;
    [SerializeField] List<Tilemap> BossRoomsR;

    [Header("L Rooms")]
    [SerializeField] List<Tilemap> StartingRoomsL;
    [SerializeField] List<Tilemap> DefaultRoomsL;
    [SerializeField] List<Tilemap> BossRoomsL;

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
            (Rooms.Boss, Directions.U) => BossRoomsU,
            (Rooms.Boss, Directions.D) => BossRoomsD,
            (Rooms.Boss, Directions.R) => BossRoomsR,
            (Rooms.Boss, Directions.L) => BossRoomsL,
            (Rooms.Starting, Directions.U) => StartingRoomsU,
            (Rooms.Starting, Directions.D) => StartingRoomsD,
            (Rooms.Starting, Directions.R) => StartingRoomsR,
            (Rooms.Starting, Directions.L) => StartingRoomsL,
            (Rooms.Starting, Directions.UD) => StartingRoomsUD,
            (Rooms.Starting, Directions.UR) => StartingRoomsUR,
            (Rooms.Starting, Directions.UL) => StartingRoomsUL,
            (Rooms.Starting, Directions.DR) => StartingRoomsDR,
            (Rooms.Starting, Directions.DL) => StartingRoomsDL,
            (Rooms.Starting, Directions.RL) => StartingRoomsRL,
            (Rooms.Starting, Directions.UDR) => StartingRoomsUDR,
            (Rooms.Starting, Directions.UDL) => StartingRoomsUDL,
            (Rooms.Starting, Directions.URL) => StartingRoomsURL,
            (Rooms.Starting, Directions.DRL) => StartingRoomsDRL,
            (Rooms.Starting, Directions.UDRL) => StartingRoomsUDRL,
            (Rooms.Default, Directions.U) => DefaultRoomsU,
            (Rooms.Default, Directions.D) => DefaultRoomsD,
            (Rooms.Default, Directions.R) => DefaultRoomsR,
            (Rooms.Default, Directions.L) => DefaultRoomsL,
            (Rooms.Default, Directions.UD) => DefaultRoomsUD,
            (Rooms.Default, Directions.UR) => DefaultRoomsUR,
            (Rooms.Default, Directions.UL) => DefaultRoomsUL,
            (Rooms.Default, Directions.DR) => DefaultRoomsDR,
            (Rooms.Default, Directions.DL) => DefaultRoomsDL,
            (Rooms.Default, Directions.RL) => DefaultRoomsRL,
            (Rooms.Default, Directions.UDR) => DefaultRoomsUDR,
            (Rooms.Default, Directions.UDL) => DefaultRoomsUDL,
            (Rooms.Default, Directions.URL) => DefaultRoomsURL,
            (Rooms.Default, Directions.DRL) => DefaultRoomsDRL,
            (Rooms.Default, Directions.UDRL) => DefaultRoomsUDRL,
            _ => DefaultRoomsUDRL
        };
    }
}
