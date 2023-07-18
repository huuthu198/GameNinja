using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float hp;
    public bool isDeath => hp <= 0;
    private string currentAnim;

    public void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
    }
    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("dead");
        Invoke(nameof(OnDespawn), 1f);
    }
    public void OnHit(float damge)
    {
        
        if (isDeath)
        {
            hp -= damge;
            if (isDeath)
            {
                OnDeath();
            }
        }
        
    }
   
    protected void ChangeAnim(string animname)
    {
        if (currentAnim != animname)
        {
            animator.ResetTrigger(animname);
            currentAnim = animname;
            animator.SetTrigger(currentAnim);
        }
    }
  
}
