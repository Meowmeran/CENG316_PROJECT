using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("Delay in seconds before the scene loads")]
    [SerializeField] private float transitionDelay = 0f;

    [Tooltip("Optional CanvasGroup used for fade in/out effect")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Tooltip("Duration of the fade animation")]
    [SerializeField] private float fadeDuration = 0.5f;

    // Whether a scene switch is currently in progress
    private bool isSwitching = false;

    // ──────────────────────────────────────────────
    // Public API
    // ──────────────────────────────────────────────

    /// <summary>Load a scene by name, respecting delay and optional fade.</summary>
    public void LoadScene(string sceneName)
    {
        if (isSwitching) return;
        StartCoroutine(SwitchSceneRoutine(sceneName, transitionDelay));
    }

    /// <summary>Load a scene by build index.</summary>
    public void LoadScene(int buildIndex)
    {
        if (isSwitching) return;
        StartCoroutine(SwitchSceneRoutine(buildIndex, transitionDelay));
    }

    /// <summary>Load a scene by name with a custom delay override.</summary>
    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        if (isSwitching) return;
        StartCoroutine(SwitchSceneRoutine(sceneName, delay));
    }

    /// <summary>Reload the currently active scene.</summary>
    public void ReloadCurrentScene()
    {
        if (isSwitching) return;
        string current = SceneManager.GetActiveScene().name;
        StartCoroutine(SwitchSceneRoutine(current, transitionDelay));
    }
    public void ReloadCurrentScene(float delay)
    {
        if (isSwitching) return;
        string current = SceneManager.GetActiveScene().name;
        StartCoroutine(SwitchSceneRoutine(current, delay));
    }

    /// <summary>Load the next scene in the build order (wraps around).</summary>
    public void LoadNextScene()
    {
        if (isSwitching) return;
        int next = (SceneManager.GetActiveScene().buildIndex + 1)
                   % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(SwitchSceneRoutine(next, transitionDelay));
    }

    public void LoadNextScene(float delay)
    {
        if (isSwitching) return;
        int next = (SceneManager.GetActiveScene().buildIndex + 1)
                   % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(SwitchSceneRoutine(next, delay));
    }

    /// <summary>Load the previous scene in the build order (wraps around).</summary>
    public void LoadPreviousScene()
    {
        if (isSwitching) return;
        int count = SceneManager.sceneCountInBuildSettings;
        int prev = (SceneManager.GetActiveScene().buildIndex - 1 + count) % count;
        StartCoroutine(SwitchSceneRoutine(prev, transitionDelay));
    }

    /// <summary>Quit the application (works in builds; stops play mode in Editor).</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ──────────────────────────────────────────────
    // Core Coroutines
    // ──────────────────────────────────────────────

    private IEnumerator SwitchSceneRoutine(string sceneName, float delay)
    {
        isSwitching = true;

        yield return new WaitForSeconds(delay);

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeRoutine(0f, 1f));

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator SwitchSceneRoutine(int buildIndex, float delay)
    {
        isSwitching = true;

        yield return new WaitForSeconds(delay);

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeRoutine(0f, 1f));

        SceneManager.LoadScene(buildIndex);
    }

    // ──────────────────────────────────────────────
    // Fade Helper
    // ──────────────────────────────────────────────

    private IEnumerator FadeRoutine(float from, float to)
    {
        if (fadeCanvasGroup == null) yield break;

        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }

    // ──────────────────────────────────────────────
    // Optional: Fade-in on scene start
    // ──────────────────────────────────────────────

    private IEnumerator Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            yield return StartCoroutine(FadeRoutine(1f, 0f));
        }
    }
}