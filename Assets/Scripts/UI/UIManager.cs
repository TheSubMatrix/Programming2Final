using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image m_healthBarMask;
    [SerializeField] CanvasGroup gameOverCanvasGroup;
    public void UpdateHealthBar(uint currentHealth, uint maxHealth)
    {
        if (m_healthBarMask != null)
        {
            m_healthBarMask.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }
    public void OnDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 1.0f;
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        }
    }
    public void OnMenuButtonPressed()
    {
        FindObjectOfType<SceneTransitioner>().TransitionScene("Main Menu");
    }
    public void OnRetryButtonPressed()
    {
        FindObjectOfType<SceneTransitioner>().TransitionScene("Level One");
    }
}
