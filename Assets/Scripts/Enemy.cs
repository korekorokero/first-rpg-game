using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] private float maxHealth = 10f;

    private Animator anim;
    private BoxCollider2D bc;
    private Canvas healthCanvas;
    [SerializeField] private FloatingHealthBar healthBar;

    private void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        healthCanvas = GetComponentInChildren<Canvas>();
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Enemy_Death")
        {
            Destroy(gameObject);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Arrow")
        {
            TakeDamage(2f);
        }
    }

    private void Die()
    {
        anim.SetBool("dead", true);
        bc.enabled = false;
        healthCanvas.enabled = false;
    }
}
