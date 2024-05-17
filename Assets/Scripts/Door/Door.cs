using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject ClosedDoor;
    [SerializeField] GameObject OpenedDoor;

    public bool Triggered { get; set; }

    public void TriggerClose()
    {
        if (Triggered)
            return;

        ClosedDoor.SetActive(true);
        OpenedDoor.SetActive(false);

        Triggered = true;
    }

    public void TriggerOpen()
    {
        ClosedDoor.SetActive(false);
        OpenedDoor.SetActive(true);
    }
}
