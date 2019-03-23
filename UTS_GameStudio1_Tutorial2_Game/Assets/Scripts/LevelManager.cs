using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;


    [Header("Player reference")]
    [SerializeField]
    Player player;

    public int Lives { get; set; }

    private void Start()
    {
        //TO:DO - Add Black screen showing Level and Lives
        Lives = 3; //Start with 3 Lives
    }

    private void Awake() {
        
        if (instance == null) {
            instance = this;
        }
    }

    public void OnEnemyHit() {
        Debug.Log("Enemy Hit");
        // if player status is OnUnTouchable
        if ((player.CurrentStatus & Player.Status.OnUnTouchable) != 0) {
            //Nothing happen maybe

        } else if ((player.CurrentStatus & Player.Status.OnInvincible) != 0) {
            //Enemy get killed

        } else {
            OnPlayerHurt();
        }
    }

    public void OnPlayerHurt() {
        player.OnHurt();
        if (!player.IsAlive) {
            Lives--;

            if (Lives < 0) //Mario dies when he has 0 lives LEFT, not when he reaches 0 lives.
            {
                GameOver();
            }
        }
    }

    public void OnPlayerKilled() {

        player.OnDeath();
        Lives--; //Lose a Life

        if (Lives < 0) //Mario dies when he has 0 lives LEFT, not when he reaches 0 lives.
        {
            GameOver();           
        }
    }

    private void GameOver()
    {
        //TO:DO - Game Over code here
    }
}
