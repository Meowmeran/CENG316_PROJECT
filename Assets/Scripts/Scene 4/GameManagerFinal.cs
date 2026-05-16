using UnityEngine;

public class GameManagerFinal : MonoBehaviour
{
    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private EntitySpawner entitySpawner;
    [SerializeField] private VisualEffectHandler effectHandler;

    [SerializeField] private int requiredKills = 80;
    [SerializeField] private int currentKills = 0;
    [SerializeField] private bool gameStartedOnce = false;
    public bool isGameOver = false;
    public bool isGameWin = false;

    void Start()
    {
        isGameOver = false;
        isGameWin = false;
        if (sceneSwitcher == null)
        {
            sceneSwitcher = FindAnyObjectByType<SceneSwitcher>();
            if (sceneSwitcher == null)
            {
                Debug.LogError("SceneSwitcher not found.");
            }
        }
        gameStartedOnce = false;
    }
    [ContextMenu("Start Game")]
    public void GameStart()
    {

        if (gameStartedOnce) return;
        gameStartedOnce = true;
        musicManager.GoIntense();
        entitySpawner.StartSpawning();
    }


    public void OnWin()
    {
        isGameWin = true;
        effectHandler.OnWin();
        musicManager.EndMusic();
        entitySpawner.StopSpawning();
        entitySpawner.DespawnAllEnemies();
    }

    public void OnDeath()
    {
        musicManager.FadeAllOut();
        entitySpawner.StopSpawning();
    }

    public void OnKill()
    {
        currentKills++;
        CheckKills();
    }

    private void CheckKills()
    {
        if (currentKills >= requiredKills)
        {
            OnWin();
        }
    }


}
