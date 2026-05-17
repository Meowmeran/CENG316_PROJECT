using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class GameManagerTouch : MonoBehaviour
{
    [SerializeField] private SoundInstancer victorySoundInstancer;
    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private PhaseHandler phaseHandler;
    [SerializeField] private TouchDetectionManager touchDetectionManager;
    [SerializeField] private JumpscareHandlerTouch jumpscareHandler;
    [SerializeField] private EffectHandlerTouch effectHandler;
    [SerializeField] private float jumpscareDelay = 2f;
    public bool isWin = false;
    public bool isGameOver = false;
    [SerializeField] private SoundInstancer gameOverSoundInstancer;
    
 


    

    void Update()
    {
        if (isWin || isGameOver)
        {
            return;
        }   
        if (touchDetectionManager.IsTouching() && phaseHandler.IsReady())
        {
            OnGameOver();
        }
    }

    public void OnWin()
    {
        isWin = true;
        effectHandler.OnWin();
        victorySoundInstancer.PlaySound();
        sceneSwitcher.LoadNextScene(7f);
    }

    public void OnGameOver()
    {
        isGameOver = true;
        StartCoroutine(GameOverSequence());
    }


    IEnumerator GameOverSequence()
    {
        gameOverSoundInstancer.PlaySound();
        yield return new WaitForSeconds(jumpscareDelay);
        effectHandler.OnGameOver();
        jumpscareHandler.Jumpscare();
        sceneSwitcher.ReloadCurrentScene(4f);
    }
}
