using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    Enemy[] enemies;

    private void Start() {

        DisableMove();
    }

    void DisableMove() {

        StartCoroutine(WaitToEndOfFrame());

        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].CanMove = false;
        }
    }

    public void OnPlayerEnter() {

        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].CanMove = true; 
        }
    }

    IEnumerator WaitToEndOfFrame() {

        yield return new WaitForEndOfFrame();
    }
}
