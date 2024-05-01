using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image m_healthBarMask;
    public void UpdateHealthBar(uint currentHealth, uint maxHealth)
    {
        if (m_healthBarMask != null)
        {
            m_healthBarMask.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }
}
