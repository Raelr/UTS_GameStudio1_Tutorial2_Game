using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBlock : Platform
{
    [Header("Default Sprite")]
    [SerializeField]
    Sprite defaultSprite;


    [Header("Sprite When Hit")]
    [SerializeField]
    Sprite hitSprite;

    private void Awake() {

        renderer = GetComponentInChildren<SpriteRenderer>();

        renderer.sprite = defaultSprite;
    }

    public override bool AllowedToJumpThrough(float direction, bool isCheckingDIrection) {

        return platformType == PlatformType.Solid ? false : true;
    }

    public override bool CanFallThrough() {

        return platformType == PlatformType.Solid ? false : true;
    }

    public override void OnPlayerHit(bool destroy) {

        if (animator != null) {

            if (renderer.sprite == defaultSprite) {
                renderer.sprite = hitSprite;
            }

            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation() {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Hit")) {

            animator.SetBool("isHit", true);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime / 5);

            animator.SetBool("isHit", false);

        } else {

            yield return null;
        }
    }
}
