using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    Enemy[] enemies;

    bool triggered;

    private void Start() {

        DisableMove();
        triggered = false;
    }

    void DisableMove() {

        StartCoroutine(WaitToEndOfFrame());

        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].CanMove = false;
            enemies[i].gameObject.SetActive(false);
        }
    }

    public void OnPlayerEnter() {

        if (!triggered) {
            for (int i = 0; i < enemies.Length; i++) {
                enemies[i].gameObject.SetActive(true);
                enemies[i].CanMove = true;

            }

            triggered = true;
        }
    }

    IEnumerator WaitToEndOfFrame() {

        yield return new WaitForEndOfFrame();
    }
}
