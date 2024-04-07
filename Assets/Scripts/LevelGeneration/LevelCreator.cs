using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    private const int RoomWidth = 30;
    private const int RoomHeight = 30;

    private void Awake()
    { 
        LevelGenerator generator = new(0.5f);
        RoomChooser chooser = gameObject.GetComponent<RoomChooser>();
        Rooms[,] level = generator.CreateLevel(20);
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (level[i, j] == Rooms.Empty)
                    continue;
                Tilemap room = chooser.ChooseRoom(i, j, level);
                Tilemap createdRoom = Instantiate(room);
                createdRoom.transform.parent = gameObject.transform;
                createdRoom.transform.position = new((i - LevelGenerator.StartingRoomI) * RoomHeight, (j - LevelGenerator.StartingRoomJ) * RoomWidth);
            }
        }
    }
}
