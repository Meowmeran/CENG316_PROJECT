using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderJumpscareHandler : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // References
    // ─────────────────────────────────────────────
    [Header("References")]
    public Camera jumpscareCamera;
    public GameObject spiderPrefab;
    public GameObject bigSpiderPrefab;  // Optional — falls back to spiderPrefab
    public GameManagerSpider gameManager;

    // ─────────────────────────────────────────────
    // Audio
    // ─────────────────────────────────────────────
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip crawlSound;        // Looped during crawl phase
    public AudioClip jumpscareSound;    // One-shot when big spider launches

    // ─────────────────────────────────────────────
    // Spawn Settings
    // ─────────────────────────────────────────────
    [Header("Spawn Settings")]
    public int spiderCount = 8;
    public float spawnRadius = 2f;
    public float bigSpiderSpawnDistance = 4f;

    // ─────────────────────────────────────────────
    // Timing
    // ─────────────────────────────────────────────
    [Header("Timing")]
    public float crawlDuration = 2f;
    public float attackDelay = 1f;
    public float lingerDuration = 0.4f;

    // ─────────────────────────────────────────────
    // Small Spider Movement
    // ─────────────────────────────────────────────
    [Header("Small Spider Movement")]
    public float crawlSpeed = 2f;
    public float wobbleAmount = 15f;
    public float wobbleFrequency = 8f;

    // ─────────────────────────────────────────────
    // Big Spider
    // ─────────────────────────────────────────────
    [Header("Big Spider Jump")]
    public float jumpSpeed = 12f;
    public float bigSpiderScale = 3f;
    public float jumpStopDistance = 0.15f;

    // ─────────────────────────────────────────────
    // Internal
    // ─────────────────────────────────────────────
    class SpiderData
    {
        public GameObject obj;
        public Vector3    crawlTarget;
        public float      rotationOffset;
    }

    private List<SpiderData> spiders   = new List<SpiderData>();
    private GameObject       bigSpider = null;

    // ─────────────────────────────────────────────
    // Trigger
    // ─────────────────────────────────────────────
    [ContextMenu("Trigger Jumpscare")]
    public void TriggerJumpscare()
    {
        StopAllCoroutines();
        CleanUp();
        StartCoroutine(JumpscareRoutine());
    }

    IEnumerator JumpscareRoutine()
    {
        SpawnSpiders();
        yield return StartCoroutine(CrawlPhase());
        yield return new WaitForSeconds(attackDelay);
        yield return StartCoroutine(BigSpiderJumpscare());
        CleanUp();
    }

    // ─────────────────────────────────────────────
    // Cleanup
    // ─────────────────────────────────────────────
    void CleanUp()
    {
        foreach (var s in spiders)
            if (s.obj != null) Destroy(s.obj);
        spiders.Clear();

        if (bigSpider != null) { Destroy(bigSpider); bigSpider = null; }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }

    // ─────────────────────────────────────────────
    // Spawn Small Spiders
    // ─────────────────────────────────────────────
    void SpawnSpiders()
    {
        Transform cam = jumpscareCamera.transform;

        for (int i = 0; i < spiderCount; i++)
        {
            Vector3 spawnPos = cam.position + Random.onUnitSphere * spawnRadius;
            GameObject spider = Instantiate(spiderPrefab, spawnPos, Quaternion.identity);

            SpiderData data = new SpiderData
            {
                obj            = spider,
                rotationOffset = Random.Range(0f, 360f)
            };

            Vector3 viewport  = new Vector3(Random.value, Random.value, Random.Range(0.3f, 0.7f));
            data.crawlTarget  = jumpscareCamera.ViewportToWorldPoint(viewport);

            spiders.Add(data);
        }
    }

    // ─────────────────────────────────────────────
    // Crawl Phase
    // ─────────────────────────────────────────────
    IEnumerator CrawlPhase()
    {
        if (audioSource != null && crawlSound != null)
        {
            audioSource.clip = crawlSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        float timer = 0f;

        while (timer < crawlDuration)
        {
            timer += Time.deltaTime;

            foreach (var spider in spiders)
            {
                if (!spider.obj) continue;

                Transform t = spider.obj.transform;

                // Move
                t.position = Vector3.MoveTowards(
                    t.position, spider.crawlTarget, crawlSpeed * Time.deltaTime);

                // Always face camera
                FaceCamera(t);

                // Wobble roll on top
                float wobble = Mathf.Sin(Time.time * wobbleFrequency + spider.rotationOffset) * wobbleAmount;
                t.Rotate(0f, 0f, wobble * Time.deltaTime, Space.Self);
            }

            yield return null;
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        // Stick spiders to camera
        foreach (var spider in spiders)
        {
            if (!spider.obj) continue;
            Transform t = spider.obj.transform;
            t.SetParent(jumpscareCamera.transform);
            Vector3 local = t.localPosition;
            local.z += Random.Range(-0.02f, 0.02f);
            t.localPosition = local;
        }
    }

    // ─────────────────────────────────────────────
    // Big Spider Jumpscare
    // ─────────────────────────────────────────────
    IEnumerator BigSpiderJumpscare()
    {
        Transform cam       = jumpscareCamera.transform;
        Vector3   spawnPos  = cam.position + cam.forward * bigSpiderSpawnDistance;
        GameObject prefab   = bigSpiderPrefab != null ? bigSpiderPrefab : spiderPrefab;

        bigSpider = Instantiate(prefab, spawnPos, Quaternion.identity);
        bigSpider.transform.localScale = Vector3.one * bigSpiderScale;

        FaceCamera(bigSpider.transform);

        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound);

        float speed = jumpSpeed * Random.Range(0.9f, 1.2f);

        while (bigSpider != null)
        {
            Transform t = bigSpider.transform;

            if (Vector3.Distance(t.position, cam.position) <= jumpStopDistance)
            {
                yield return new WaitForSeconds(lingerDuration);
                yield break;
            }

            t.position = Vector3.MoveTowards(t.position, cam.position, speed * Time.deltaTime);

            FaceCamera(t);

            // Dramatic spin during charge
            t.Rotate(0f, 0f, 360f * Time.deltaTime, Space.Self);

            yield return null;
        }
    }

    // ─────────────────────────────────────────────
    // Face Camera
    // All spiders are billboarded — they always look
    // directly at the camera, every frame.
    // ─────────────────────────────────────────────
    void FaceCamera(Transform t)
    {
        t.LookAt(jumpscareCamera.transform);

        // LookAt points +Z at the camera, which works for 3D meshes.
        // If your spiders are 2D sprites (art on the XY plane), Unity's
        // SpriteRenderer already handles billboarding automatically when
        // the Camera is set to Billboard mode, but if you need an explicit
        // fix just uncomment the line below to flip the forward axis:
        // t.rotation *= Quaternion.Euler(0f, 180f, 0f);
    }
}