using System.Collections;
using UnityEngine;

public class ClimbableStone : MonoBehaviour
{
    public bool isPossessed = false;
    [SerializeField] private GameObject spiderDream;
    [SerializeField] int touchingCount = 0;
    GameManagerSpider gm;

    [SerializeField] private float maxTouchingTime = 2f;
    private float touchingTimer = 0f;
    [SerializeField] private float possessChance = 0.25f;


    void Start()
    {
        if (UnityEngine.Random.value < possessChance)
        {
            possess();
        }
        else
        {
            unpossess();
        }

        if (gm == null)
            gm = FindAnyObjectByType<GameManagerSpider>();
        if (gm == null)
        {
            Debug.LogError("GameManagerSpider not found in scene.");
        }
    }
    void Update()
    {
        if (!gm.isWin &&!gm.isGameOver && touchingCount > 0)
            OnHoveredInside();
    }
    public void OnGrabbedByPlayer()
    {
        if (isPossessed == true && !gm.isGameOver && !gm.isWin)
        {
            gm.OnGameOver();
        }
    }
    public void OnReleasedByPlayer()
    {
        
    }

    public void OnHoverEnter()
    {
        touchingCount++;
    }
    public void OnHoverExit()
    {
        touchingCount--;
        if (touchingCount == 0)
        {
            touchingTimer = 0f;
        }
    }

    public void OnHoveredInside()
    {
        if (isPossessed == true)
        {
            touchingTimer += Time.deltaTime;
            if (touchingTimer >= maxTouchingTime)
            {
                gm.OnGameOver();
            }
        }
    }

    [ContextMenu("Possess")]
    public void possess()
    {
        isPossessed = true;
        if (spiderDream != null)
        {
            spiderDream.SetActive(true);
        }
    }
    [ContextMenu("Unpossess")]
    public void unpossess()
    {
        isPossessed = false;
        if (spiderDream != null)
        {
            spiderDream.SetActive(false);
        }
    }
}
