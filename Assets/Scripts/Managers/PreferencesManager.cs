using UnityEngine;

public class PreferencesManager : MonoBehaviour
{
    private static PreferencesManager s_instance;
    public bool ContinueRun { get; set; }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
