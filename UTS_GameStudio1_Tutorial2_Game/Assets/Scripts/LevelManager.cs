using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Player reference")]
    [SerializeField]
    Player player;

    [SerializeField]
    FlagPole flag;

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
       
    public void OnPlayerKilled() {

        player.OnDeath();
        Lives--; //Lose a Life

        if (Lives < 0) //Mario dies when he has 0 lives LEFT, not when he reaches 0 lives.
        {
            GameOver();           
        }
    }

    public void SetPlayerProjectile(FireProjectile projectile) {

        player.Projectile = projectile;
    }

    public void PlayEndAnimation() {

        flag.InitEndAnimation();
    }

    private void GameOver()
    {
        //TO:DO - Game Over code here
    }
}
