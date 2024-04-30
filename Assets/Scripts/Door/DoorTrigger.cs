using UnityEngine;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] List<Door> Doors;

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Door door in Doors)
            door.TriggerClose();
    }
}
