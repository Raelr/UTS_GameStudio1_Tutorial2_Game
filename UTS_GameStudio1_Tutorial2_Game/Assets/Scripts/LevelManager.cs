using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Player reference")]
    [SerializeField]
    Player player;

    private void Awake() {
        
        if (instance == null) {
            instance = this;
        }
    }

    public void OnPlayerKilled() {

        player.OnDeath();
    }
}
