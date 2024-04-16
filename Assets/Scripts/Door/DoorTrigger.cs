using UnityEngine;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] List<Door> doors;

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Door door in doors)
            door.TriggerClose();
    }
}
