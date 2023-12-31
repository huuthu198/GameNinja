using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    private IState currentState;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;

    private bool isRight = true;
    private Character target;
    public Character Target => target;
    
    private void Update()
    {
        if (currentState != null && !isDeath)
        {
            currentState.onExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.onExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.onEnter(this);
        }
    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetRange())
        {
            ChangeState(new AttackState());
        }
        else 
        if(target != null)
        {
            ChangeState(new PatrolState());

        }
        else
        {
            ChangeState( new IdleState());
        }
        
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public bool IsTargetRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
            target = null;
        }
    }


    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

}