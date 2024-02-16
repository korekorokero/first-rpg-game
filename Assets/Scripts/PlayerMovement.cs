using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { idle, walking }
    private enum Facing { front, back, side }
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float collisionOffset = 0.05f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Facing lastFacing = Facing.front, currentFacing;
    private MovementState currentState = MovementState.idle;
    public ContactFilter2D movementFilter;
    [SerializeField] private LayerMask barrier;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (IsBehind())
        {
            sr.sortingLayerID = SortingLayer.NameToID("Player");
        }
        else
        {
            sr.sortingLayerID = SortingLayer.NameToID("Default");
        }
        AnimationStateUpdate();
    }

    private void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            bool success = TryMove(movement);
            if (!success)
            {
                success = TryMove(new Vector2(movement.x, 0));
                if (!success)
                {
                    TryMove(new Vector2(0, movement.y));
                }
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        movement = movementValue.Get<Vector2>();
    }

    private bool TryMove(Vector2 direction)
    {   
        int count = rb.Cast(movement, movementFilter, castCollisions, movementSpeed * Time.fixedDeltaTime + collisionOffset);

        if(count == 0)
        {
            rb.MovePosition(rb.position + (movement * movementSpeed * Time.fixedDeltaTime));
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AnimationStateUpdate()
    {
        
        if (movement.x > 0f)
        {
            currentState = MovementState.walking;
            currentFacing = lastFacing = Facing.side;
            sr.flipX = false;
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

   private bool IsBehind()
    {
        return Physics2D.BoxCast(new Vector2(bc.bounds.center.x, bc.bounds.center.y - 0.375f), new Vector2(bc.bounds.size.x + 0.5f, bc.bounds.size.y + 0.5f), 0f, Vector2.down, .1f, barrier);
    }
}