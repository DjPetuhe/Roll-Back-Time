using System;
using System.Linq;
using System.Collections.Generic;

public class LevelGenerator
{
    private static readonly Random Rand = new();

    public const int LevelMatrixSize = 100;

    public const int MaxRoomsAmount = 100;
    public const int MinRoomsAmount = 5;
    
    public const int StartingRoomI = 50;
    public const int StartingRoomJ = 50;
    
    private readonly float _chanceOfNextRoom;

    private static readonly List<(int i, int j)> AdjacentDirections = new() { (-1, 0), (1, 0), (0, -1), (0, 1) };

    public LevelGenerator(float chanceOfNextRoom) => _chanceOfNextRoom = chanceOfNextRoom;

    public Rooms[,] CreateLevel(int roomsAmount)
    {
        if (roomsAmount < MinRoomsAmount || roomsAmount > MaxRoomsAmount)
            throw new ArgumentException("Rooms amount eather too large or too small!");

        Rooms[,] level = new Rooms[LevelMatrixSize, LevelMatrixSize];
        List<(int i, int j)>  deadends = new();
        Queue<(int i, int j)> uncheckedRooms = new();

        level[StartingRoomI, StartingRoomJ] = Rooms.Starting;
        uncheckedRooms.Enqueue((StartingRoomI, StartingRoomJ));
        deadends.Add((StartingRoomI, StartingRoomJ));
        int roomsLeft = roomsAmount - 1;

        while(roomsLeft > 0)
        {
            if (uncheckedRooms.Count == 0)
                return CreateLevel(roomsAmount);

            (int i, int j) = uncheckedRooms.Dequeue();
            foreach ((int adjI, int adjJ) in AdjacentDirections)
            {
                if (!CheckCell(i + adjI, j + adjJ, level) || Rand.NextDouble() < _chanceOfNextRoom)
                    continue;

                List<(int i, int j)> neighbours = GetAmountOfNeighbors(i + adjI, j + adjJ, level);
                if (neighbours.Count > Rand.Next(4) / 3 + 1)
                    continue;

                level[i + adjI, j + adjJ] = Rooms.Default;
                uncheckedRooms.Enqueue((i + adjI, j + adjJ));
                roomsLeft--;

                if (neighbours.Count == 1)
                    deadends.Add((i + adjI, j + adjJ));

                foreach ((int neighbourI, int neighbourJ) in neighbours)
                    deadends.Remove((neighbourI, neighbourJ));

                if (roomsLeft == 0)
                    break;
            }
        }

        if (!BossRoomPos(deadends, out (int i, int j) bossRoomPos))
            return CreateLevel(roomsAmount);

        level[bossRoomPos.i, bossRoomPos.j] = Rooms.Boss;

        return level;
    }

    private bool BossRoomPos(List<(int i, int j)> deadends, out (int i, int j) bossRoomPos)
    {
        bossRoomPos = (-1, -1);
        foreach ((int i, int j) pos in deadends.OrderBy(x => Rand.Next()))
        {
            if (Math.Abs(pos.i - StartingRoomI) > 2 || Math.Abs(pos.j - StartingRoomJ) > 2)
            {
                bossRoomPos = pos;
                return true;
            }
        }
        return false;
    }

    private bool CheckCell(int i, int j, Rooms[,] level)
    {
        if (i <= 0  || i >= LevelMatrixSize - 1)
            return false;
        if (j <= 0 || j >= LevelMatrixSize - 1)
            return false;
        if (level[i, j] != Rooms.Empty)
            return false;
        return true;
    }

    private List<(int i, int j)> GetAmountOfNeighbors(int i, int j, Rooms[,] level)
    {
        List<(int, int)> neigbours = new();
        foreach ((int adjI, int adjJ) in AdjacentDirections)
        {
            if (i + adjI < 0 || i + adjI >= LevelMatrixSize)
                continue;
            if (j + adjJ < 0 || j + adjJ >= LevelMatrixSize)
                continue;
            if (level[i + adjI, j + adjJ] != Rooms.Empty)
                neigbours.Add((i + adjI, j + adjJ));
        }
        return neigbours;
    }
}
