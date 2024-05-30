using UnityEngine;
using System.Collections.Generic;

public class RoomDeactivator : MonoBehaviour
{
    [SerializeField] List<Door> Doors;
    [SerializeField] Chest chest;
    
    public int RoomCellIndex { get; set; }

    public void Cleared()
    {
        GameObject.FindGameObjectWithTag("LevelCreator").GetComponent<LevelCreator>().MarkRoomCleared(RoomCellIndex);
    }

    public void Deactivate()
    {
        foreach (Door door in Doors)
            door.Triggered = true;
        chest.Deactivate();
    }
}
