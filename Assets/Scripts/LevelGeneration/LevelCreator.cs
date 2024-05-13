using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] Tilemap BackgroundTilemap;
    [SerializeField] Tile BackgroundTile;

    private const int RoomWidth = 30;
    private const int RoomHeight = 30;

    private void Awake()
    { 
        LevelGenerator generator = new(0.5f);
        RoomChooser chooser = gameObject.GetComponent<RoomChooser>();
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

                Tilemap room = chooser.ChooseRoom(i, j, level);
                Tilemap createdRoom = Instantiate(room);
                createdRoom.transform.parent = gameObject.transform;
                createdRoom.transform.position = new((j - LevelGenerator.StartingRoomJ) * RoomWidth, (LevelGenerator.StartingRoomI - i) * RoomHeight);
            }
        }
        FillBackgroundTilemap(minI, maxI, minJ, maxJ);
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

    private void FillBackgroundTilemap(int minI, int maxI, int minJ, int maxJ)
    {
        for (int i = minI - 1; i <= maxI + 1; i++)
        {
            for (int j = minJ - 1; j <= maxJ + 1; j++)
            {
                int posYCenter = (LevelGenerator.StartingRoomI - i) * RoomHeight;
                int posXCenter = (j - LevelGenerator.StartingRoomJ) * RoomWidth;
                for (int k = -15; k < RoomHeight - 15; k++)
                {
                    for (int l = -15; l < RoomWidth - 15; l++)
                    {
                        BackgroundTilemap.SetTile(new(posXCenter + l, posYCenter + k), BackgroundTile);
                    }
                }
            }
        }
    }
}
