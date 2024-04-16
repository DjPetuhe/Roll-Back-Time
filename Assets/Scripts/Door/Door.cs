using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject closedDoor;
    [SerializeField] GameObject openedDoor;

    private bool triggered = false;

    public void TriggerClose()
    {
        if (triggered)
            return;

        closedDoor.SetActive(true);
        openedDoor.SetActive(false);

        triggered = true;
    }

    public void TriggerOpen()
    {
        closedDoor.SetActive(false);
        openedDoor.SetActive(true);
    }
}
