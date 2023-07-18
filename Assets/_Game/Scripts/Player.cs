using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwpoint;
    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDead = false;
    private float horizontal;
    private int coin = 0;
    private Vector3 savePoint;
   

    // Update is called once per frame
    void FixedUpdate()
    {
       isGrounded = CheckGrounded();
       horizontal = Input.GetAxisRaw("Horizontal");

        if(isDead)
        {
            return;
        }

        if(isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(isGrounded )
        {
            if (isJumping)
            {
                return;
            }
            // jump
            if (Input.GetKeyDown(KeyCode.B) && isGrounded)
            {
                Jump();

            }
            
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack
            if(Input.GetKeyDown(KeyCode.F) && isGrounded)
            {
                Attack();
            }


            // throw
            if (Input.GetKeyDown(KeyCode.G) && isGrounded)
            {
                Throw();
            }
        }
        // check fall
        if (!isGrounded && rb.velocity.y < 0f)
        {
            ChangeAnim("jumpFail");
            isJumping = false;
        }


        // Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 :180,0 )) ;
        }
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
      
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.6f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,0.6f,groundLayer);
        return hit.collider != null;
    }
    private void Attack()
    {

        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack),0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
 
    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab, throwpoint.position, throwpoint.rotation);
   }
    private void ResetAttack()
    {
        ChangeAnim("ilde");
        isAttack = false;  
    }
    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }
        if(collision.tag == "DeadZone")
        {
            isDead = true;
            ChangeAnim("dead");
            Invoke(nameof(OnInit), 1f);
        }
    }
}
