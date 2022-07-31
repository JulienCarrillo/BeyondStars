using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool IsPlayerBullet;
    public bool IsEnemyFPSBullet;
    public bool IsEnemyBullet;
    private string TargetTag;
    public int Damage;
    
    

    void Start()
    {
        if (IsEnemyBullet)
        {
            TargetTag = "Player";
            
        }
        if(IsPlayerBullet)
        {
            TargetTag = "Enemy";
        }
        if (IsEnemyFPSBullet)
        {
            TargetTag = "PlayerFPS";
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == TargetTag)
        {
            Debug.Log("Enter in if");
            Debug.Log(IsEnemyBullet);
            if (IsEnemyBullet)
            {
                other.GetComponentInParent<PlayerController>().TakeDamage(Damage);
            }
            else if(IsPlayerBullet)
            {
                other.GetComponentInParent<EnemyController>().TakeDamage(Damage);
            }
            else if (IsEnemyFPSBullet)
            {
                Debug.Log(other.gameObject.name);
                other.GetComponentInParent<FPSMouvement>().TakeDamage(Damage);
            }
        }
    }
}
