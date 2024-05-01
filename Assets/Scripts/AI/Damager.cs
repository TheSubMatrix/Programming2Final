using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] uint m_damageToDeal;
    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.transform.root.GetComponent<IDamagable>();
        damagable?.Damage(m_damageToDeal);
    }
}
