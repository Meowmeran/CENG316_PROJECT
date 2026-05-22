using System.Collections;
using UnityEngine;

public class GameManagerFinal : MonoBehaviour
{
    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private EntitySpawner entitySpawner;
    [SerializeField] private VisualEffectHandler effectHandler;

    [SerializeField] private int requiredKills = 35;
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
    
    public void StartGame() => StartCoroutine(GameStart(3.4f));
    IEnumerator GameStart(float delay = 2f)
    {

        if (gameStartedOnce)
            yield break;
        gameStartedOnce = true;
        musicManager.GoIntense();
        new WaitForSeconds(delay);
        entitySpawner.StartSpawning();
    }


    public void OnWin()
    {
        isGameWin = true;
        effectHandler.OnWin();
        musicManager.EndMusic();
        entitySpawner.StopSpawning();
        entitySpawner.DespawnAllEnemies();
        sceneSwitcher.LoadNextScene(19f);
    }

    public void OnDeath()
    {
        isGameOver = true;
        musicManager.FadeAllOut();
        effectHandler.OnDeath();
        entitySpawner.StopSpawning();
        entitySpawner.DisappearAllEnemies();
        sceneSwitcher.ReloadCurrentScene(5f);
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
