using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerFPS : MonoBehaviour
{
    public float Life = 50f; 
    public void TakeDamage(float amount)
    {
        Life -= amount;
        if(Life <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
