using UnityEngine;

public class GameManagerStop : MonoBehaviour
{
    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private EffectHandlerSpider effectHandler;
    public TheEyeLogic eyeLogic;
    private GameObject player;
    public bool isGameOver = false;
    public bool isWin = false;
    public int minAmountToWin = 7;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null && Camera.main != null)
            player = Camera.main.gameObject;
        if (player == null)
            Debug.LogError("Player not found.");
    }

    public void OnWin()
    {
        isWin = true;
        eyeLogic.OnWin();
        effectHandler.OnWin();
        sceneSwitcher.LoadNextScene(5f);
    }

    public void OnGameOver()
    {
        isGameOver = true;
        eyeLogic.OnGameOver();
        effectHandler.OnGameOver();
        sceneSwitcher.ReloadCurrentScene(5f);
    }

    public void CheckFoundAmount(int amount)
    {
        if (amount >= minAmountToWin && !isWin && !isGameOver)
        {
            OnWin();
        }
    }


}
