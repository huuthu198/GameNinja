using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPreb;
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
        healthBar.OnInit(100);
    }
    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("dead");
        Invoke(nameof(OnDespawn), 1f);
    }
    public void OnHit(float damage)
    {
        if (!isDeath)
        {
            hp -= damage;
            if (isDeath)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPreb, transform.position + Vector3.up, Quaternion.identity, gameObject.transform).OnInit(damage);
           
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
