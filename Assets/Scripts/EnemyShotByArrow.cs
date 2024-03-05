using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotByArrow : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D bc;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Arrow")
        {
            anim.SetBool("dead", true);
            bc.enabled = false;
        }
    }
}
