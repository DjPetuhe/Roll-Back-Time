using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public static class LoadManager
{
    private static readonly string PerksFilePath = Application.dataPath + "/Data/perks.json";

    public static List<Perk> ReadPerksFromJson()
    {
        string json = File.ReadAllText(PerksFilePath);
        return JsonUtility.FromJson<PerkWrapper>(json).Perks.ToList();
    }
}