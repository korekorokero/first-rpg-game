using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AttackAnimation();
    }

    private void OnFire(InputValue fireValue)
    {
        Debug.Log("Fire!");
        anim.SetBool("attack", true);
    }

    private void AttackAnimation()
    {  
        if (anim.GetBool("attack"))
        {
            string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            clipName = clipName.Remove(13, clipName.Length - 13);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && clipName == "Player_Attack")
            {
                anim.SetBool("attack", false);
            }
        }

    }
}
