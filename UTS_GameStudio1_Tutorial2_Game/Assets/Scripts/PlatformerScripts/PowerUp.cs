using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PowerUp : PlatformUser
{
    [SerializeField]
    protected Animator animator;

    protected bool isInitialising;

    public enum Abilities {Mashroom, Fire, Invincible}
    protected Abilities ability;

    protected Controller2D controller;

    protected void InitialiseAnimation() {

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation() {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MushroomRisingAnimation")) {

            isInitialising = true;

            animator.SetBool("Instantiated", true);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime);

            isInitialising = false;

            animator.SetBool("Instantiated", false);

        } else {

            yield return null;
        }
    }

    public abstract void OnPickup();

    public Abilities GetAbility(){
        return ability;
    }
}
