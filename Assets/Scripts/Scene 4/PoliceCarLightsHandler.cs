using System.Collections;
using UnityEngine;

public class PoliceCarLightsHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] reds;
    [SerializeField] private GameObject[] blues;

    [SerializeField] private int flashCount = 3;
    [SerializeField] private float flashInterval = 0.25f;
    [SerializeField] private float flashDuration = 0.25f;


    private void Start()
    {
        StartCoroutine(HandleLights());
    }

    IEnumerator HandleLights()
    {
        while (true)
        {
            for (int i = 0; i < flashCount; i++)
            {
                foreach (GameObject red in reds)
                {
                    red.SetActive(true);
                }
                yield return new WaitForSeconds(flashDuration);
                foreach (GameObject red in reds)
                {
                    red.SetActive(false);
                }
                yield return new WaitForSeconds(flashInterval);
            }
            for (int i = 0; i < flashCount; i++)
            {
                foreach (GameObject blue in blues)
                {
                    blue.SetActive(true);
                }
                yield return new WaitForSeconds(flashDuration);
                foreach (GameObject blue in blues)
                {
                    blue.SetActive(false);
                }
                yield return new WaitForSeconds(flashInterval);
            }
        }
    }
}
