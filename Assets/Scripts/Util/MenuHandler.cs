using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
    public GameObject menuPrefab;
    public Transform playerHead;
    

    [Header("Input")]
    public InputActionProperty menuButton;

    public float spawnDistance = 2f;
    public float spawnRotation = 90f;

    GameObject currentMenu;

    void OnEnable()
    {
        menuButton.action.Enable();
        menuButton.action.performed += OnMenuPressed;
    }

    void OnDisable()
    {
        menuButton.action.performed -= OnMenuPressed;
        menuButton.action.Disable();
    }

    void OnMenuPressed(InputAction.CallbackContext ctx)
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (currentMenu == null)
            SpawnMenu();
        else
            Destroy(currentMenu);
    }

    void SpawnMenu()
    {
        Vector3 pos =
            playerHead.position +
            playerHead.forward * spawnDistance;

        Quaternion rot =
            Quaternion.LookRotation(pos - playerHead.position);
            rot *= Quaternion.Euler(0f, spawnRotation, 0f);

        currentMenu = Instantiate(menuPrefab, pos, rot);
    }
}