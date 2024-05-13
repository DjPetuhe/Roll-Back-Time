using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Random = System.Random;

public class PerkUI : MonoBehaviour
{
    [Header("Perk Buttons")]
    [SerializeField] List<Image> PerkButtons;

    [Header("Perk Descriptions")]
    [SerializeField] List<TextMeshProUGUI> PerkDescriptions;

    [Header("Perk Button Images")]
    [SerializeField] Sprite usualImage;
    [SerializeField] Sprite rareImage;
    [SerializeField] Sprite balancedImage;

    [Header("Perk Manager")]
    [SerializeField] PerkManager PerkManager;

    private static readonly Random Rand = new();

    private GameManager _gameManager;

    private List<Perk> _perks;
    private List<int> _perksID = new();

    private const int PerksOptionsCount = 3;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _perks = PerkManager.ReadPerksFromJson();
    }

    public void ChooseOption(int index)
    {
        _gameManager.ApplyPerkChanges(_perks[_perksID[index]].PerkChanges);
    }

    public void RandomizePerks()
    {
        int perkId;
        _perksID.Clear();
        for (int i = 0; i < PerksOptionsCount; i++)
        {
            do
                perkId = Rand.Next(_perks.Count);
            while (_perksID.Contains(perkId));
            _perksID.Add(perkId);
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        int i = 0;
        foreach (int perkID in _perksID)
        {
            PerkButtons[i].sprite = PerkQualityToImage(_perks[perkID].PerkQuality);
            PerkDescriptions[i].text = _perks[perkID].Description;
            i++;
        }
    }

    private Sprite PerkQualityToImage(PerkQuality quality)
    {
        return quality switch
        {
            PerkQuality.usual => usualImage,
            PerkQuality.rare => rareImage,
            _ => balancedImage
        };
    }
}
