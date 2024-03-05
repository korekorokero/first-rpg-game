using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireArrow : MonoBehaviour
{
    private int arrowDir = 0;
    private bool firing = false;
    private bool hitEnemy = false;
    [SerializeField] private float arrowSpeed = 15f;
    [SerializeField] private float arrowRange = 5f;

    private Animator animPlayer;
    private SpriteRenderer srPlayer, sr;
    private Rigidbody2D rb;
    private Vector2 arrowMovement;
    private Vector2 arrowStart = Vector2.zero;
    private BoxCollider2D bc;
    [SerializeField] private GameObject player;

    private void Start()
    {
        animPlayer = player.GetComponent<Animator>();
        srPlayer = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        ArrowDirection();
        if (animPlayer.GetBool("fire"))
        {
            firing = true;
        }
    }

    private void FixedUpdate()
    {
        ArrowFiring();
    }

    private void ArrowDirection()
    {
        int direction = animPlayer.GetInteger("facing");
        arrowDir = direction;
        if (!firing)
        {
            switch (direction)
            {
                case 0:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    arrowMovement = new Vector2(0, -1);
                    break;
                case 1:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    arrowMovement = new Vector2(0, 1);
                    break;
                case 2:
                    if (srPlayer.flipX)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        arrowMovement = new Vector2(-1, 0);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        arrowMovement = new Vector2(1, 0);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ArrowFiring()
    {
        if (animPlayer.GetBool("fire"))
        {
            arrowStart = rb.position;
        }

        if (!animPlayer.GetBool("fire") && firing)
        {
            sr.enabled = true;
            bc.enabled = true;
            gameObject.transform.SetParent(null);
            rb.MovePosition(rb.position + (arrowMovement * arrowSpeed * Time.fixedDeltaTime));
            if (rb.position.y <= arrowStart.y - arrowRange || rb.position.y >= arrowStart.y + arrowRange || rb.position.x <= arrowStart.x - arrowRange || rb.position.x >= arrowStart.x + arrowRange || hitEnemy)
            {
                sr.enabled = bc.enabled = firing = hitEnemy = false;
                transform.SetParent(player.transform);
                transform.position = transform.parent.position - new Vector3(0, 0.35f, 0);
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hitEnemy = true;
        }
    }
}
