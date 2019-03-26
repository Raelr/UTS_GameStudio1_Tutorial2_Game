using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    AudioClip spawnSound;

    private void Start() {

        animator = GetComponent<Animator>();

        StartCoroutine(LevelManager.instance.PlayAnimation(animator, "CoinSpawn", "Initialised", true, 0.4f, DestroySelf));

        SoundManager.instance.PlaySingle(spawnSound);

        LevelManager.instance.pickUpCoin();
    }

    public void DestroySelf() {

        gameObject.SetActive(false);
    }
}
