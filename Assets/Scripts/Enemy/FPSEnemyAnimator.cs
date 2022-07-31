using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPSEnemyAnimator : MonoBehaviour
{
    Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public void StartRunning()
    {
        Animator.SetBool("IsRunning", true);
    }
    public void StopRunning()
    {
        Animator.SetBool("IsRunning", false);
    }
    public void StartShooting()
    {
        Animator.SetBool("IsShooting", true);
    }
    public void StopShooting()
    {
        Animator.SetBool("IsShooting", false);
    }
    public void Dying()
    {
        Animator.SetBool("IsDying", true);
    }
   
    public void StartWalking()
    {
        Animator.SetBool("IsWalking", true);
    }
    public void StopWalking()
    {
        Animator.SetBool("IsWalking", false);
    }
    public void StartGetHit()
    {
        Animator.SetBool("GetHit", true);
    }
    public void StopGetHit()
    {
        Animator.SetBool("GetHit", false);
    }
}
