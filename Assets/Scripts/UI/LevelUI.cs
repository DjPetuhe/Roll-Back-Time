﻿using TMPro;
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
    [SerializeField] GameObject BossHealthBar;
    [SerializeField] Image BossHealth;

    [Header("Buttons")]
    [SerializeField] Button PauseButton;
    [SerializeField] Button ResumeButton;
    [SerializeField] Button SkillButton;

    [Header("CoolDowns")]
    [SerializeField] Image CooldownImage;
    [SerializeField] Image SkillTimeImage;

    [Header("Panels")]
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject RewindPanel;

    [Header("End Game UI")]
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject EndGameText;

    [Header("Perks UI")]
    [SerializeField] GameObject PerksUI;

    [Header("Time")]
    [SerializeField] float TimeBeforeGameOver;

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

    public void SetBossHealth(float health, float maxHealth) => BossHealth.fillAmount = health / maxHealth;

    public void ActivateBossHealth() => BossHealthBar.SetActive(true);
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

    public void FillCooldownImage(float percent) => CooldownImage.fillAmount = percent;

    public void DeactivateCooldownImage() => CooldownImage.enabled = false;

    public void FillSkillTimeImage(float percent) => SkillTimeImage.fillAmount = percent;

    public void DeactivateSkillTimeImage()
    {
        SkillTimeImage.enabled = false;
        CooldownImage.enabled = true;
    }

    public void ActivateCooldown() => CooldownImage.enabled = true;

    public void DisableSkill()
    {
        SkillButton.interactable = false;
        CooldownImage.color = Color.clear;
        SkillTimeImage.color = Color.clear;
    }

    public void RewindPanelActivation(bool active) => RewindPanel.SetActive(active);
}