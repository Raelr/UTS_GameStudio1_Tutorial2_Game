﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidPlatform : Platform {

    //SFX
    [SerializeField]
    AudioClip bumpSound, breakBlockSound;

    public override bool AllowedToJumpThrough(float direction) {

        return platformType == PlatformType.Solid ? false : true;
    }

    public override bool CanFallThrough() {
        
        return platformType == PlatformType.Solid ? false : true;
    }

    public override void OnPlayerHit(bool destroy) {

        if (animator != null) {

            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation() {
       

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Hit")) {
            SoundManager.instance.PlaySingle(bumpSound);

            animator.SetBool("isHit", true);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime/5);

            animator.SetBool("isHit", false);

        } else {

            yield return null;
        }
    }
}
