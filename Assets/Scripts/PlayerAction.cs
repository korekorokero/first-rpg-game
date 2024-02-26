using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private bool buttonHold = false;
    private int attackCount = 0;
    private bool arrowFired = false;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AttackAnimation();
        CheckArrow();
    }

    private void OnAttack(InputValue attackValue)
    {
        if (attackValue.isPressed)
        {
            anim.SetBool("attack", true);
            buttonHold = true;
        }
        else
        {
            attackCount = (int)Math.Ceiling(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            buttonHold = false;
        }
    }

    private void OnFire(InputValue fireValue)
    {
        if (!arrowFired)
        {
            if (fireValue.isPressed)
            {
                anim.SetBool("fire", true);
            }
            else
            {
                anim.SetBool("fire", false);
            }
        }
    }

    private void AttackAnimation()
    {  
        if (anim.GetBool("attack"))
        {
            string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            clipName = clipName.Remove(13, clipName.Length - 13);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= (float)attackCount && clipName == "Player_Attack" && !buttonHold)
            {
                anim.SetBool("attack", false);
            }
        }

    }

    private void CheckArrow()
    {
        if (transform.childCount == 0)
        {
            arrowFired = true;
        }
        else
        {
            arrowFired = false;
        }
    }
}
