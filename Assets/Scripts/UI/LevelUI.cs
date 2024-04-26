using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelUI : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] GameObject timeField;
    [SerializeField] GameObject hpField;

    [Header("HP sprites")]
    [SerializeField] Sprite hpFull;
    [SerializeField] Sprite hpEmpty;

    [Header("Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    [Header("Panel")]
    [SerializeField] GameObject panel;

    [Header("End Game UI")]
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject endGameText;

    [Header("Perks")]
    [SerializeField] List<GameObject> perks;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Pause() => _gameManager.PauseGame();

    public void Resume() => _gameManager.ResumeGame();

    public void SetTime(float time)
    {
        int minutes = Mathf.CeilToInt(time) / 60;
        int seconds = Mathf.CeilToInt(time) % 60;
        string secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        timeField.GetComponent<TextMeshProUGUI>().text = $"{minutes}:" + secondsString;
    }

    public void SetHealth(float health, float maxHealth)
    {
        foreach (Transform child in hpField.transform)
        {
            if (health > 0)
            {
                child.GetComponent<Image>().sprite = hpFull;
                health--;
            }
            else
                child.GetComponent<Image>().sprite = hpEmpty;
        }
    }

    public void Restart() => SceneManager.LoadScene("LevelScene");

    public void QuitToMenu() => SceneManager.LoadScene("MainMenuScene");

    public void EndGamePopUp(bool gameOver)
    {
        panel.SetActive(true);
        gameOverUI.SetActive(true);
        gameOverText.GetComponent<TextMeshProUGUI>().text = gameOver ? "Game over!" : "You won!";
        endGameText.GetComponent<TextMeshProUGUI>().text = gameOver ? "You died! Try again!" : "Congratulations!";
    }

    public void SwitchPauseStatus(bool enable) => pauseButton.interactable = enable;

    public void ChoosingPerks()
    {

    }

    public void ChoosePerk(int id)
    {

    }
}