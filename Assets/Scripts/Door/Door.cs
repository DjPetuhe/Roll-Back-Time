using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject ClosedDoor;
    [SerializeField] GameObject OpenedDoor;

    private bool _triggered = false;

    public void TriggerClose()
    {
        if (_triggered)
            return;

        ClosedDoor.SetActive(true);
        OpenedDoor.SetActive(false);

        _triggered = true;
    }

    public void TriggerOpen()
    {
        ClosedDoor.SetActive(false);
        OpenedDoor.SetActive(true);
    }
}
