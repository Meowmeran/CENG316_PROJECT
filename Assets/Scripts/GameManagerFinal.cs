using UnityEngine;

public class GameManagerFinal : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private EntitySpawner entitySpawner;
    [SerializeField] private VisualEffectHandler effectHandler;

    [SerializeField] private int requiredKills = 80;
    [SerializeField] private int currentKills = 0;
    [SerializeField] private bool gameStartedOnce = false;


    void Start()
    {
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


    public void GameEnd()
    {
        musicManager.EndMusic();
        entitySpawner.StopSpawning();
        entitySpawner.DespawnAllEnemies();
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
            GameEnd();
        }
    }



}
