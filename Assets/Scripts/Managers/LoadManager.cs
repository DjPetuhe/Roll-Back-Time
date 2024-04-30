using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public static class LoadManager
{
    private const string PerksFilePath = "/perks.json";

    public static List<Perk> ReadPerksFromJson()
    {
        string json = File.ReadAllText(PerksFilePath);
        return JsonUtility.FromJson<Perk[]>(json).ToList();
    }
}