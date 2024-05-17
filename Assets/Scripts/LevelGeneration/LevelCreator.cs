using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] Tilemap BackgroundTilemap;
    [SerializeField] Tile BackgroundTile;

    private const int RoomWidth = 30;
    private const int RoomHeight = 30;
    private GameManager _gameManager;
    private PreferencesManager _preferences;
    private RoomChooser _chooser;
    private List<RoomCell> _rooms = new();
    private int _minI;
    private int _maxI;
    private int _minJ;
    private int _maxJ;

    private void Awake()
    {
        _preferences = GameObject.FindGameObjectWithTag("PreferencesManager").GetComponent<PreferencesManager>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _chooser = gameObject.GetComponent<RoomChooser>();
        if (_preferences != null && _preferences.ContinueRun)
        {
            Level level = SaveLoadManager.LoadLevelFromJson();
            LoadFromSave(level);
        }
        else
            GenerateLevel();
        FillBackgroundTilemap();
    }

    private void GenerateLevel()
    {
        LevelGenerator generator = new(0.5f);
        Rooms[,] level = generator.CreateLevel(20);

        int minI = LevelGenerator.LevelMatrixSize;
        int minJ = LevelGenerator.LevelMatrixSize;
        int maxI = -1;
        int maxJ = -1;
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (level[i, j] == Rooms.Empty)
                    continue;

                CheckBounds(ref minI, ref maxI, ref minJ, ref maxJ, i, j);

                Tilemap room = _chooser.ChooseRoom(i, j, level, out Directions direction, out int tilemapIndex);
                _rooms.Add(new()
                {
                    RoomType = level[i, j],
                    Direction = direction,
                    TilemapIndex = tilemapIndex,
                    PositionI = i,
                    PositionJ = j
                });

                Tilemap createdRoom = Instantiate(room);
                if (createdRoom.TryGetComponent<RoomDeactivator>(out var deactivator))
                    deactivator.RoomCellIndex = _rooms.Count - 1;
                createdRoom.transform.parent = gameObject.transform;
                createdRoom.transform.position = new((j - LevelGenerator.StartingRoomJ) * RoomWidth, (LevelGenerator.StartingRoomI - i) * RoomHeight);
            }
        }
        _minI = minI;
        _maxI = maxI;
        _minJ = minJ;
        _maxJ = maxJ;
    }

    private void LoadFromSave(Level level)
    {
        _rooms = level.Rooms.ToList();
        _minI = level.MinI;
        _maxI = level.MaxI;
        _minJ = level.MinJ;
        _maxJ = level.MaxJ;
        for (int i = 0; i < _rooms.Count; i++)
        {
            Tilemap room = _chooser.ChooseRoom(_rooms[i].RoomType, _rooms[i].Direction, _rooms[i].TilemapIndex);
            Tilemap createdRoom = Instantiate(room);
            if (createdRoom.TryGetComponent<RoomDeactivator>(out var deactivator))
                deactivator.RoomCellIndex = i;
            if (_rooms[i].Cleared)
               deactivator.Deactivate();
            createdRoom.transform.parent = gameObject.transform;
            createdRoom.transform.position = new((_rooms[i].PositionJ - LevelGenerator.StartingRoomJ) * RoomWidth, (LevelGenerator.StartingRoomI - _rooms[i].PositionI) * RoomHeight);
        }
    }

    private void CheckBounds(ref int minI, ref int maxI, ref int minJ, ref int maxJ, int i, int j)
    {
        if (i < minI)
            minI = i;
        else if (i > maxI)
            maxI = i;

        if (j < minJ)
            minJ = j;
        else if (j > maxJ)
            maxJ = j;
    }

    private void FillBackgroundTilemap()
    {
        for (int i = _minI - 1; i <= _maxI + 1; i++)
        {
            for (int j = _minJ - 1; j <= _maxJ + 1; j++)
            {
                int posYCenter = (LevelGenerator.StartingRoomI - i) * RoomHeight;
                int posXCenter = (j - LevelGenerator.StartingRoomJ) * RoomWidth;
                for (int k = -15; k < RoomHeight - 15; k++)
                {
                    for (int l = -15; l < RoomWidth - 15; l++)
                        BackgroundTilemap.SetTile(new(posXCenter + l, posYCenter + k), BackgroundTile);
                }
            }
        }
    }

    public void MarkRoomCleared(int roomCellIndex)
    {
        if (roomCellIndex < 0 || roomCellIndex >= _rooms.Count)
            throw new ArgumentOutOfRangeException("Room cell index was out of range.");
        _rooms[roomCellIndex].Cleared = true;
        Vector3 playerPos = _gameManager.GetPlayerPos();
        Level curLevel = new()
        {
            PlayerStats = _gameManager.GetPlayerState(),
            PlayerPosX = playerPos.x,
            PlayerPosY = playerPos.y,
            Rooms = _rooms.ToArray(),
            MinI = _minI,
            MaxI = _maxI,
            MinJ = _minJ,
            MaxJ = _maxJ
        };
        SaveLoadManager.SaveLevelToJson(curLevel);
    }
}
