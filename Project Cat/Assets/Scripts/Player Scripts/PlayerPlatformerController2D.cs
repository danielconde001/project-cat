using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController2D : PhysicsObject2D 
{
    [SerializeField] protected float maxSpeed = 7f;
    [SerializeField] protected float jumpTakeOffSpeed = 7f;
    [SerializeField] protected float hangTime = .2f;
    [SerializeField] protected float jumpBufferLength = .1f;

    protected bool canMove = false;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    protected float hangCounter;
    protected float jumpBufferCounter;

    [SerializeField] bool canDoubleJump = true;
    bool hasDoubledJumped = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        animator = GetComponent<Animator> ();
    }

    protected override void ComputeVelocity()
    {
        if (!canMove) return;

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");
        move.y = Input.GetAxis ("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            velocity += Vector2.ClampMagnitude(move, 1) * 15f;
        }

        // Coyote Time
        if (grounded)
        {
            hangCounter = hangTime;

            // Reset double jump
            canDoubleJump = true;
            hasDoubledJumped = false;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        //Jump Buffer
        if (Input.GetButtonDown ("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;

            // Double Jump
            if (canDoubleJump && hasDoubledJumped == false && !grounded) 
            {
                canDoubleJump = false;
                hasDoubledJumped = true;
                
                velocity.y = 0;
                velocity.y = jumpTakeOffSpeed;
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // The actual Jumping part
        if (jumpBufferCounter >= 0 && hangCounter > 0) 
        {
            velocity.y = jumpTakeOffSpeed;
            grounded = false;
            hangCounter = 0;
            jumpBufferCounter = 0;
        } 
        else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) 
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        
        

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        animator.SetFloat ("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
    }
}