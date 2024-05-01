using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class SceneTransitioner : MonoBehaviour
{
    CanvasGroup m_canvasGroup;
    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(0, 0.5f));
    }
    public void TransitionScene(string sceneToTransitionTo)
    {
        StartCoroutine(FadeCanvasGroup(1, 0.5f));
        SceneManager.LoadScene(sceneToTransitionTo);
    }
    IEnumerator FadeCanvasGroup(float desiredOpacity, float desiredTime)
    {
        float startingOpacity = m_canvasGroup.alpha;
        float timePassed = 0;
        while (timePassed < desiredTime)
        {
            timePassed += Time.deltaTime;
            m_canvasGroup.alpha = timePassed.Remap(0, desiredTime, startingOpacity, desiredOpacity);
            yield return null;
        }
    }
}
