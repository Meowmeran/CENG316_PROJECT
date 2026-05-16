using UnityEngine;

public class Findable : MonoBehaviour
{
    FindableManager manager;
    void Start()
    {
        manager = FindFirstObjectByType<FindableManager>();
        if (manager == null)
        {
            Debug.LogError("FindableManager not found in scene.");
        }
    }
    public void OnFound()
    {
        manager.OnFound();
        Destroy(gameObject);
    }
}
