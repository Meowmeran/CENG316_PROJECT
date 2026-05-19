using System.Collections;
using UnityEngine;

public class GameManagerSpider : MonoBehaviour
{
    [SerializeField] private SceneSwitcher sceneSwitcher;
    private GameObject player;
    public bool isGameOver = false;
    public bool isWin = false;
    public EffectHandlerSpider effectHandler;
    public SpiderJumpscareHandler jumpscareHandler;
    [SerializeField] private SoundInstancer soundInstancer;
    [SerializeField] private BoxCollider finishCollider;
    [SerializeField] private BoxCollider deathCollider;
    void Start()
    {
        if (sceneSwitcher == null)
        {
            sceneSwitcher = FindAnyObjectByType<SceneSwitcher>();
            if (sceneSwitcher == null)
            {
                Debug.LogError("SceneSwitcher not found.");
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = Camera.main.gameObject;
            if (player == null)
            {
                Debug.LogError("Player not found.");
            }
        }
        else
        {
            StartCoroutine(CheckFinishLine());
        }
        isWin = false;
        isGameOver = false;

    }



    [ContextMenu("Game Over")]
    public void OnGameOver()
    {
        isGameOver = true;
        effectHandler.OnGameOver();
        jumpscareHandler.TriggerJumpscare();
        sceneSwitcher.ReloadCurrentScene(6f);
    }
    [ContextMenu("Win")]
    public void OnWin()
    {
        isWin = true;
        effectHandler.OnWin();
        soundInstancer.PlaySound();
        sceneSwitcher.LoadNextScene(5f);
    }

    private IEnumerator CheckFinishLine()
    {
        while (true)
        {
            if (finishCollider.bounds.Contains(player.transform.position))
            {
                OnWin();
                break;
            }
            if (deathCollider.bounds.Contains(player.transform.position))
            {
                OnGameOver();
                break;
            }
            WaitForSeconds wait = new(0.1f);
            yield return wait;
        }
    }
}
