using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

public class SpiderVisualCube : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector Settings
    // ─────────────────────────────────────────────
    [Header("Spider Prefab / Sprites")]
    [Tooltip("Assign 9 different spider sprite prefabs (SpriteRenderer-based).")]
    public List<GameObject> spiderPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    [Range(1, 30)]
    public int spiderCount = 9;

    [Header("Speed")]
    public float minSpeed = 0.3f;
    public float maxSpeed = 1.2f;

    [Header("Size")]
    public float minScale = 0.05f;
    public float maxScale = 0.25f;

    [Header("Direction Change")]
    public float minDirectionInterval = 1.0f;
    public float maxDirectionInterval = 4.0f;

    [Header("Surface Offset")]
    [Tooltip("How far off the cube face the sprite floats (avoids z-fighting).")]
    public float surfaceOffset = 0.01f;

    // ─────────────────────────────────────────────
    //  Internal State
    // ─────────────────────────────────────────────
    private struct SpiderState
    {
        public GameObject go;
        public int faceIndex;           // 0-5: +X -X +Y -Y +Z -Z
        public Vector2 uvPos;           // 0..1 position on current face
        public Vector2 moveDir;         // normalised direction on face UV plane
        public float speed;
        public float dirTimer;
        public float dirInterval;
    }

    // Maps face index → (right axis, up axis, normal, center)
    private static readonly Vector3[] FaceNormals = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };
    private static readonly Vector3[] FaceRights = { Vector3.forward, Vector3.back, Vector3.right, Vector3.right, Vector3.right, Vector3.left };
    private static readonly Vector3[] FaceUps = { Vector3.up, Vector3.up, Vector3.forward, Vector3.forward, Vector3.up, Vector3.up };

    private List<SpiderState> spiders = new List<SpiderState>();
    private Vector3 cubeHalfSize;

    // ─────────────────────────────────────────────
    //  Unity Lifecycle
    // ─────────────────────────────────────────────
    void Start()
    {
        if (spiderPrefabs == null || spiderPrefabs.Count == 0)
        {
            Debug.LogError("[SpiderVisualCube] No spider prefabs assigned!");
            return;
        }

        Renderer rend = GetComponent<Renderer>();

        if (rend != null && rend is MeshRenderer)
        {
            cubeHalfSize = rend.GetComponent<MeshFilter>().sharedMesh.bounds.extents;
        }
        else
        {
            cubeHalfSize = transform.localScale * 0.5f;
        }

        for (int i = 0; i < spiderCount; i++)
            SpawnSpider();
    }

    void Update()
    {
        for (int i = 0; i < spiders.Count; i++)
        {
            SpiderState s = spiders[i];

            // ── Direction timer ──────────────────
            s.dirTimer -= Time.deltaTime;
            if (s.dirTimer <= 0f)
            {
                s.moveDir = RandomDir();
                s.dirTimer = Random.Range(s.dirInterval * 0.5f, s.dirInterval);
            }

            // ── Move on face UV ──────────────────
            s.uvPos += s.speed * Time.deltaTime * s.moveDir;

            // ── Edge crossing ────────────────────
            const float EDGE_EPS = 0.001f;

            if (s.uvPos.x < -EDGE_EPS || s.uvPos.x > 1f + EDGE_EPS ||
                s.uvPos.y < -EDGE_EPS || s.uvPos.y > 1f + EDGE_EPS)
            {
                s.faceIndex = RandomAdjacentFace(s.faceIndex, ref s.uvPos, ref s.moveDir);
            }

            // ── World position & rotation ────────
            ApplyTransform(ref s);

            spiders[i] = s;
        }
    }

    // ─────────────────────────────────────────────
    //  Spawn
    // ─────────────────────────────────────────────
    void SpawnSpider()
    {
        int prefabIdx = Random.Range(0, spiderPrefabs.Count);
        GameObject go = Instantiate(spiderPrefabs[prefabIdx], transform);
        float scale = Random.Range(minScale, maxScale);
        go.transform.localScale = Vector3.one * scale;

        SpiderState s = new SpiderState
        {
            go = go,
            faceIndex = Random.Range(0, 6),
            uvPos = new Vector2(Random.value, Random.value),
            moveDir = RandomDir(),
            speed = Random.Range(minSpeed, maxSpeed),
            dirTimer = Random.Range(minDirectionInterval, maxDirectionInterval),
            dirInterval = Random.Range(minDirectionInterval, maxDirectionInterval)
        };
        

        ApplyTransform(ref s);
        spiders.Add(s);
    }

    // ─────────────────────────────────────────────
    //  Transform Helpers
    // ─────────────────────────────────────────────

    /// <summary>Converts UV position on a cube face to world space and orients the sprite.</summary>
    void ApplyTransform(ref SpiderState s)
    {
        Vector3 normal = FaceNormals[s.faceIndex];
        Vector3 right = FaceRights[s.faceIndex];
        Vector3 up = FaceUps[s.faceIndex];

        // Half-extents along the face tangent axes
        float halfR = HalfExtent(right);
        float halfU = HalfExtent(up);

        // UV 0..1 → local position on face
        float localR = Mathf.Lerp(-halfR, halfR, s.uvPos.x);
        float localU = Mathf.Lerp(-halfU, halfU, s.uvPos.y);

        // Face center in local space
        Vector3 faceCenter = normal * HalfExtent(normal);

        Vector3 localPos = faceCenter + right * localR + up * localU + normal * surfaceOffset;
        s.go.transform.localPosition = localPos;

        // Orient: sprite faces outward (normal), sprite "up" aligns with face-up
        s.go.transform.localRotation = Quaternion.LookRotation(-normal, up);

        // Rotate sprite in its plane to match movement direction (visual flavour)
        float angle = Mathf.Atan2(s.moveDir.y, s.moveDir.x) * Mathf.Rad2Deg;
        s.go.transform.localRotation *= Quaternion.Euler(0f, 0f, angle);
    }

    float HalfExtent(Vector3 axis)
    {
        return Mathf.Abs(Vector3.Dot(cubeHalfSize, axis));
    }

    // ─────────────────────────────────────────────
    //  Edge-Crossing: wrap UV and switch face
    // ─────────────────────────────────────────────

    /// <summary>
    /// When a spider walks off an edge, pick the adjacent face and remap its UV
    /// so the crossing is seamless. Movement direction is also rotated to match.
    /// </summary>
    int RandomAdjacentFace(int currentFace, ref Vector2 uvPos, ref Vector2 moveDir)
    {
        Vector3 normal = FaceNormals[currentFace];
        Vector3 right = FaceRights[currentFace];
        Vector3 up = FaceUps[currentFace];

        float halfR = HalfExtent(right);
        float halfU = HalfExtent(up);

        // 1. Convert the out-of-bounds UV to temporary local 3D space
        float localR = Mathf.Lerp(-halfR, halfR, uvPos.x);
        float localU = Mathf.Lerp(-halfU, halfU, uvPos.y);
        Vector3 faceCenter = normal * HalfExtent(normal);
        Vector3 local3DPos = faceCenter + right * localR + up * localU;

        // Convert 2D move direction to 3D local direction
        Vector3 local3DDir = (right * moveDir.x + up * moveDir.y).normalized;

        // 2. Find which face the spider actually stepped onto in 3D space
        int newFace = currentFace;
        float maxOvershoot = 0f;

        for (int i = 0; i < 6; i++)
        {
            float dot = Vector3.Dot(local3DPos, FaceNormals[i]);
            float limit = HalfExtent(FaceNormals[i]);
            if (dot > limit && (dot - limit) > maxOvershoot)
            {
                maxOvershoot = dot - limit;
                newFace = i;
            }
        }

        // 3. Setup new face coordinate system
        Vector3 newNormal = FaceNormals[newFace];
        Vector3 newRight = FaceRights[newFace];
        Vector3 newUp = FaceUps[newFace];

        float newHalfR = HalfExtent(newRight);
        float newHalfU = HalfExtent(newUp);

        // 4. Snap 3D position safely inside the new face boundaries
        float rDot = Mathf.Clamp(Vector3.Dot(local3DPos, newRight), -newHalfR, newHalfR);
        float uDot = Mathf.Clamp(Vector3.Dot(local3DPos, newUp), -newHalfU, newHalfU);

        // Convert back to clean 0..1 UV coordinates for the new face
        uvPos.x = Mathf.InverseLerp(-newHalfR, newHalfR, rDot);
        uvPos.y = Mathf.InverseLerp(-newHalfU, newHalfU, uDot);

        // 5. Wrap the 3D direction vector around the 90-degree corner smoothly
        Vector3 tangentDir = Vector3.ProjectOnPlane(local3DDir, newNormal);
        if (tangentDir.sqrMagnitude < 0.001f)
        {
            // Fallback if walking perfectly perpendicular over a sharp edge
            tangentDir = -normal; 
        }
        tangentDir.Normalize();

        // Convert 3D direction back to the new face's 2D UV space
        moveDir.x = Vector3.Dot(tangentDir, newRight);
        moveDir.y = Vector3.Dot(tangentDir, newUp);
        moveDir.Normalize();

        return newFace;
    }

    // Adjacency table: [face][edge] → (neighbourFace, uvRemapMode)
    // Edges: 0=left(-X), 1=right(+X), 2=bottom(-Y), 3=top(+Y)
    // uvRemapMode: 0=direct, 1=flip-x, 2=flip-y, 3=transpose, 4=transpose-flip
    private static readonly int[,] AdjFace = new int[6, 4]
    {
        //       left  right  bottom  top
        /* +X */ { 4,    5,    3,      2  },
        /* -X */ { 5,    4,    3,      2  },
        /* +Y */ { 1,    0,    5,      4  },
        /* -Y */ { 1,    0,    4,      5  },
        /* +Z */ { 0,    1,    3,      2  },
        /* -Z */ { 1,    0,    3,      2  },
    };

    (int face, Vector2 uv, Vector2 dir) CrossEdge(int face, int edge, Vector2 uv, Vector2 dir)
    {
        int next = AdjFace[face, edge];

        // Simple seamless remap: mirror the overshot coordinate onto the new face
        Vector2 newUV = uv;
        Vector2 newDir = dir;

        switch (edge)
        {
            case 0: newUV = new Vector2(1f + uv.x, uv.y); newDir = new Vector2(-dir.x, dir.y); break; // crossed left
            case 1: newUV = new Vector2(uv.x - 1f, uv.y); newDir = new Vector2(-dir.x, dir.y); break; // crossed right
            case 2: newUV = new Vector2(uv.x, 1f + uv.y); newDir = new Vector2(dir.x, -dir.y); break; // crossed bottom
            case 3: newUV = new Vector2(uv.x, uv.y - 1f); newDir = new Vector2(dir.x, -dir.y); break; // crossed top
        }

        return (next, newUV, newDir);
    }

    // ─────────────────────────────────────────────
    //  Utilities
    // ─────────────────────────────────────────────
    static Vector2 RandomDir()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    // ─────────────────────────────────────────────
    //  Public API
    // ─────────────────────────────────────────────

    /// <summary>Despawn all spiders and re-spawn a fresh set.</summary>
    public void Respawn()
    {
        foreach (var s in spiders)
            if (s.go) Destroy(s.go);
        spiders.Clear();
        for (int i = 0; i < spiderCount; i++)
            SpawnSpider();
    }

    /// <summary>Multiply every spider's speed by a factor (e.g. 2 = double speed).</summary>
    public void SetSpeedMultiplier(float factor)
    {
        for (int i = 0; i < spiders.Count; i++)
        {
            var s = spiders[i];
            s.speed = Mathf.Clamp(s.speed * factor, minSpeed * 0.1f, maxSpeed * 5f);
            spiders[i] = s;
        }
    }
}