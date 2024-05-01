using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] GameObject TimeField;

    [Header("Health Bar")]
    [SerializeField] Image HealthBar;
    [SerializeField] TextMeshProUGUI HealthText;

    [Header("Buttons")]
    [SerializeField] Button PauseButton;
    [SerializeField] Button ResumeButton;

    [Header("Panel")]
    [SerializeField] GameObject Panel;

    [Header("End Game UI")]
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject EndGameText;

    [Header("Perks UI")]
    [SerializeField] GameObject PerksUI;

    private GameManager _gameManager;
    private PerkUI _perkUI;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _perkUI = PerksUI.GetComponent<PerkUI>();
    }

    public void Pause() => _gameManager.PauseGame();

    public void Resume() => _gameManager.ResumeGame();

    public void SetTime(float time)
    {
        int minutes = Mathf.CeilToInt(time) / 60;
        int seconds = Mathf.CeilToInt(time) % 60;
        string secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        TimeField.GetComponent<TextMeshProUGUI>().text = $"{minutes}:" + secondsString;
    }

    public void SetHealth(float health, float maxHealth)
    {
        HealthBar.fillAmount = health / maxHealth;
        HealthText.text = $"{health} / {maxHealth}";
    }

    public void Restart() => SceneManager.LoadScene("LevelScene");

    public void QuitToMenu() => SceneManager.LoadScene("MainMenuScene");

    public void EndGamePopUp(bool gameOver)
    {
        Panel.SetActive(true);
        GameOverUI.SetActive(true);
        GameOverText.GetComponent<TextMeshProUGUI>().text = gameOver ? "Game over!" : "You won!";
        EndGameText.GetComponent<TextMeshProUGUI>().text = gameOver ? "You died! Try again!" : "Congratulations!";
    }

    public void SwitchPauseStatus(bool enable) => PauseButton.interactable = enable;

    public void ChoosingPerks()
    {
        Panel.SetActive(true);
        PerksUI.SetActive(true);
        _perkUI.RandomizePerks();
        _gameManager.PauseGame();
    }
}