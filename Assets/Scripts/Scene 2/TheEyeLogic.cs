using System.Collections;
using UnityEngine;

public class TheEyeLogic : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform reboundPoint;
    [SerializeField] private Transform watchPoint;
    [SerializeField] private GameObject TheEyeVisual;


    [Header("Reference")]
    [SerializeField] private PlayerMovementDetection playerMovementDetection;
    [SerializeField] private GameManagerStop gameManager;

    [Header("Movement")]
    public float speed = 1f;

    [Header("Timers")]
    [SerializeField] float allowence = 0.2f;
    [SerializeField] float minReboundTimer = 5f;
    [SerializeField] float maxReboundTimer = 10f;
    [SerializeField] float minWatchTimer = 8f;
    [SerializeField] float maxWatchTimer = 25f;

    [Header("Shake")]
    [SerializeField] private Transform CameraOffset;
    [SerializeField] private float minDistanceToShake = 5f;
    [SerializeField] private float shakeMultiplier = 0.1f;

    private GameObject player;

    Coroutine eyeLoop;

    enum EyeState
    {
        Watching,
        Rebounding
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null && Camera.main != null)
            player = Camera.main.gameObject;

        if (player == null)
            Debug.LogError("Player not found.");

        TheEyeVisual.transform.position = reboundPoint.position;

        eyeLoop = StartCoroutine(EyeBehaviourLoop());
    }

    void Update()
    {
        ShakeCamera();
    }

    // =====================================================
    // MASTER STATE LOOP
    // =====================================================
    IEnumerator EyeBehaviourLoop()
    {
        while (true)
        {

            float reboundTimer =
                Random.Range(minReboundTimer, maxReboundTimer);

            yield return new WaitForSeconds(reboundTimer);
            
            yield return MoveEye(watchPoint.position);

            float watchTimer =
                Random.Range(minWatchTimer, maxWatchTimer);

            yield return ObservePlayer(watchTimer);

            yield return MoveEye(reboundPoint.position);
        }
    }

    // =====================================================
    // MOVEMENT
    // =====================================================
    IEnumerator MoveEye(Vector3 targetPos)
    {
        while (Vector3.Distance(TheEyeVisual.transform.position, targetPos) > 0.01f)
        {
            TheEyeVisual.transform.position = Vector3.MoveTowards(
                TheEyeVisual.transform.position,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }
        TheEyeVisual.transform.position = targetPos;
    }

    // =====================================================
    // DETECTION
    // =====================================================
    IEnumerator ObservePlayer(float duration)
    {
        yield return new WaitForSeconds(allowence);

        playerMovementDetection.CaptureLastPositions();

        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            if (playerMovementDetection.HasMoved())
            {
                gameManager.OnGameOver();
                yield break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    // =====================================================
    // CAMERA SHAKE
    // =====================================================
    void ShakeCamera()
    {
        if (player == null) return;

        float distanceToPlayer =
            Vector3.Distance(
                TheEyeVisual.transform.position,
                player.transform.position);

        if (distanceToPlayer > minDistanceToShake)
        {
            CameraOffset.localPosition = Vector3.zero;
            return;
        }

        float shakeAmount =
            Mathf.Clamp01(1f / (distanceToPlayer * distanceToPlayer + 0.001f));

        shakeAmount *= shakeMultiplier;

        CameraOffset.localPosition = new Vector3(
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount),
            0f);
    }

    public void OnWin()
    {
        StopCoroutine(eyeLoop);
        TheEyeVisual.SetActive(false);
    }

    public void OnGameOver()
    {
        StopCoroutine(eyeLoop);
    }
}