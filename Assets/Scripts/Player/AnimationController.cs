using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void StartAttackAnimation()
    {
        Debug.Log("Fire");
        animator.SetBool("IsFire", true);
    }

    public void StopAttackAnimation()
    {
        animator.SetBool("IsFire", false);
    }

    public IEnumerator StartLandingAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsLanding", true);
        yield return new WaitForSeconds(1);
    }
    public void StopLandingAnimation()
    {
        animator.SetBool("IsLanding", false);
    }
   


}
