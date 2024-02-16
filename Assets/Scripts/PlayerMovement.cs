using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { idle, walking }
    private enum Facing { front, back, side }
    private float dirX, dirY;
    [SerializeField] private float movementSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private Facing lastFacing = Facing.front, currentFacing;
    private MovementState currentState = MovementState.idle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        MovementUpdate();
        AnimationStateUpdate();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (movement * movementSpeed * Time.fixedDeltaTime));
    }

    private void MovementUpdate()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        if (dirX != 0f && dirY == 0f)
        {
            movement = new Vector2(dirX, 0);
        }
        else if (dirY != 0f && dirX == 0f)
        {
            movement = new Vector2(0, dirY);
        }
        else if (dirX == 0f && dirY == 0f)
        {
            movement = Vector2.zero;
        }
    }

    private void AnimationStateUpdate()
    {
        
        if (movement.x > 0f || movement.x > 0f && Input.GetButtonUp("Vertical"))
        {
            currentState = MovementState.walking;
            currentFacing = lastFacing = Facing.side;
        }
        else if (movement.x < 0f)
        {
            currentState = MovementState.walking;
            currentFacing = lastFacing = Facing.side;
            sr.flipX = true;
        }
        else if (movement.y > 0f)
        {
            currentState = MovementState.walking;
            currentFacing = lastFacing = Facing.back;
        }
        else if (movement.y < 0f)
        {
            currentState = MovementState.walking;
            currentFacing = lastFacing = Facing.front;
        }
        else
        {
            currentState = MovementState.idle;
            currentFacing = lastFacing;
        }

        anim.SetInteger("state", (int)currentState);
        anim.SetInteger("facing", (int)currentFacing);
    }
}
