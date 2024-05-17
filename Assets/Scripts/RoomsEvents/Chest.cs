using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject OpenedChest;
    [SerializeField] GameObject ClosedChest;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OpenedChest.activeSelf)
            return;

        OpenedChest.SetActive(true);
        ClosedChest.SetActive(false);
        GameObject.FindGameObjectWithTag("LevelUI").GetComponent<LevelUI>().ChoosingPerks();
        GetComponent<Waves>().StartWave();
    }

    public void Deactivate()
    {
        OpenedChest.SetActive(true);
        ClosedChest.SetActive(false);
    }
}
