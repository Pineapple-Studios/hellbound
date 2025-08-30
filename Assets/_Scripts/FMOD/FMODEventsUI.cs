using UnityEngine;
using FMODUnity;

public class FMODEventsUI : MonoBehaviour
{
    public static FMODEventsUI Instance;

    [Header("UI")]
    public EventReference moveUI;
    public EventReference clickUI;
    public EventReference sldMove;

    // OPCIONAIS (para futuro uso)
    //[Header("SplashScreen")]
    //public EventReference splashScreen;
    //public EventReference forBetterExp;

    //[Header("Cutscenes")]
    //public EventReference voiceActing;
    //public EventReference introST;
    //public EventReference finalST;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Mais de um FMODEventsUI!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}