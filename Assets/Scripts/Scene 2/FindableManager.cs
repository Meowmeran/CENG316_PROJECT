using UnityEngine;

public class FindableManager : MonoBehaviour
{
    [SerializeField] private FindableGenerator generator;
    [SerializeField] private GameManagerStop gameManager;

    [SerializeField] private int count = 7;
    [SerializeField] private int foundAmount = 0;

    void Start()
    {
        if (generator == null)
        {
            generator = FindFirstObjectByType<FindableGenerator>();
        }
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManagerStop>();
        }
        if (gameManager.minAmountToWin > count)
        {
            count = gameManager.minAmountToWin;
        }
        generator.Generate(count);


    }

    public void OnFound()
    {
        foundAmount++;
        gameManager.CheckFoundAmount(foundAmount);
    }

    public void RegenerateFindables()
    {
        generator.DestroyAllFindables(); 
        generator.Generate(count);
        foundAmount = 0;
    }

    public void DestroyAllFindables() => generator.DestroyAllFindables();

    void OnDestroy()
    {
        DestroyAllFindables();
    }

}
