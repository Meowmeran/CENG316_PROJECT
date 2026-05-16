using UnityEngine;

public class Findable : MonoBehaviour
{
    FindableManager manager;
    [SerializeField] private SoundInstancer soundInstancer;
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
        soundInstancer.PlaySound();
        manager.OnFound();
        Destroy(gameObject);
    }
}
