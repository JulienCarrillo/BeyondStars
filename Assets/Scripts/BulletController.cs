using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool IsPlayerTarget;
    private string TargetTag;
    public int Damage;
    

    void Start()
    {
        if (IsPlayerTarget)
        {
            TargetTag = "Player";
            
        }
        else
        {
            TargetTag = "Enemy";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == TargetTag)
        {
            if (IsPlayerTarget)
            {
                other.GetComponentInParent<PlayerController>().TakeDamage(Damage);
            }
            else
            {
                other.GetComponentInParent<EnemyController>().TakeDamage(Damage);
            }
        }
    }
}
