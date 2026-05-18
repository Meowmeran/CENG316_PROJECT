using UnityEngine;

public class DisableObjetsWhenInteract : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDisable;
    
    public void EnableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }
    }
    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
