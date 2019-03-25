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

    [SerializeField]
    PowerUp powerUp;

    [SerializeField]
    Vector3 spawnOffset;

    //SFX
    [SerializeField]
    AudioClip bumpSound, breakBlockSound;

    [SerializeField]
    CoinSpawn coin;

    [SerializeField]
    int coinCount;

    private void Awake() {

        renderer = GetComponentInChildren<SpriteRenderer>();

        renderer.sprite = defaultSprite;
    }

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

        SpawnCoin();
    }

    public void SpawnCoin() {

        if (coin != null) {
            if (coinCount > 0) {
                coinCount--;
                Instantiate(coin, transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator PlayAnimation() {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Hit")) {
            SoundManager.instance.PlaySingle(bumpSound);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            animator.SetBool("isHit", true);

            if (renderer.sprite == defaultSprite) {
                renderer.sprite = hitSprite;

                if (powerUp != null) {

                    yield return new WaitForSeconds(animationTime / 2);

                    Instantiate(powerUp, transform.position + spawnOffset, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(animationTime / 5 * 2);

            animator.SetBool("isHit", false);

        } else {

            yield return null;
        }
    }
}
