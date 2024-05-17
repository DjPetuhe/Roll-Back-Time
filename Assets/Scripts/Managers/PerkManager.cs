using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PerkManager : MonoBehaviour
{
    [SerializeField] TextAsset perks;

    public List<Perk> ReadPerksFromJson()
    {
        string json = perks.text;
        return JsonUtility.FromJson<PerkWrapper>(json).Perks.ToList();
    }
}