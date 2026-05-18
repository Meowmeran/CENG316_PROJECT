using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhaseHandler : MonoBehaviour
{
    [SerializeField] private Phase currentPhase = Phase.Start;
    [SerializeField] private GameManagerTouch gameManager;

    [Header("Phase Objects")]
    [SerializeField] private GameObject easy;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject hard;

    [Header("Handle")]
    [SerializeField] private XRGrabInteractable handleGrab;
    [SerializeField] private Rigidbody handleRB;

    private Vector3 handleSpawnPosition;
    private Quaternion handleSpawnRotation;

    private bool ready = true;

    void Start()
    {
        easy.SetActive(false);
        normal.SetActive(false);
        hard.SetActive(false);

        handleSpawnPosition = handleGrab.transform.position;
        handleSpawnRotation = handleGrab.transform.rotation;

        NextPhase();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Handle") && ready && currentPhase != Phase.End)
        {
            Debug.Log("Next phase!");
            NextPhase();
        }
    }

    [ContextMenu("Next Phase")]
    public void NextPhase()
    {
        if (!ready || currentPhase == Phase.End)
            return;

        Phase next = currentPhase + 1;

        StartCoroutine(ChangePhase(next));
        if (next == Phase.End)
            gameManager.OnWin();
    }

    IEnumerator ChangePhase(Phase newPhase)
    {
        ready = false;
        

        GameObject previous = GetPhaseObject(currentPhase);
        GameObject next = GetPhaseObject(newPhase);

        if (previous != null) previous.SetActive(false);
        if (next != null) next.SetActive(true);

        ResetHandle();
        currentPhase = newPhase;
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }

    GameObject GetPhaseObject(Phase phase)
    {
        switch (phase)
        {
            case Phase.Easy: return easy;
            case Phase.Normal: return normal;
            case Phase.Hard: return hard;
            default: return null;
        }
    }

    public bool IsReady() => ready;

    void ResetHandle()
    {
        if (handleGrab.isSelected)
        {
            var interactor = handleGrab.firstInteractorSelecting;
            handleGrab.interactionManager.SelectExit(interactor, handleGrab);
        }

        handleRB.isKinematic = true;
        handleGrab.transform.SetPositionAndRotation(handleSpawnPosition, handleSpawnRotation);
        handleRB.linearVelocity = Vector3.zero;
        handleRB.angularVelocity = Vector3.zero;

        StartCoroutine(ReenablePhysics());
    }

    IEnumerator ReenablePhysics()
    {
        yield return new WaitForFixedUpdate();
        handleRB.isKinematic = false;
    }
    public void freezePhysics()
    {
        
        if (handleGrab.isSelected)
        {
            var interactor = handleGrab.firstInteractorSelecting;
            handleGrab.interactionManager.SelectExit(interactor, handleGrab);
        }
        handleRB.isKinematic = true;
    }
}

public enum Phase
{
    Start,
    Easy,
    Normal,
    Hard,
    End
}