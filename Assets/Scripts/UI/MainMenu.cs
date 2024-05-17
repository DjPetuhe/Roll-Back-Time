using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button ContinueButton;

    public void Start() => ContinueButton.interactable = SaveLoadManager.SaveFileExists();

    public void Play(bool continueRun)
    {
        GameObject.FindGameObjectWithTag("PreferencesManager").GetComponent<PreferencesManager>().ContinueRun = continueRun;
        if (!continueRun)
            SaveLoadManager.DeletePreviousSave();
        SceneManager.LoadScene("LevelScene");
    }

    public void Quit() => Application.Quit();
}
