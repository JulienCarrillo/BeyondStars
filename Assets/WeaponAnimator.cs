using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartShooting()
    {
        Debug.Log("ShootAnim");
        animator.SetBool("IsShooting", true);
    }

    public void StopShooting()
    {
        animator.SetBool("IsShooting", false);
    }

    public void FXShoot(GameObject FX)
    {
        Transform canon = GameObject.FindGameObjectWithTag("FX").GetComponent<Transform>();
        Instantiate(FX, canon);
    }
}
