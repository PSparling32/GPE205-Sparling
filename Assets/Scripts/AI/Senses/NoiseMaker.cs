using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    private Coroutine soundCoroutine;

    public void SetParameters(float soundRadius)
    {
        GameManager.GetInstance().TriggerSound(new NoiseData(soundRadius, transform.position));
    }

    private void OnDestroy()
    {
        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
        }
    }
}
