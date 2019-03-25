using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int _score = 0;
    private int _coins = 0;
    private float _time = 400f;
    public double TimeReductionRate = 2.5;
    public Text ScoreText;
    public Text CoinsText;
    public Text TimeText;

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

    void Update()
    {
        ScoreText.text = "MARIO\n" + _score.ToString("000000");
        CoinsText.text = "x " + _coins.ToString("00");
        TimeText.text = "TIME\n" + _time.ToString("000");
        _time -= Time.deltaTime * (float)TimeReductionRate;
        if ((int)_time == 0)
        {
            TimeReductionRate = 0;
            OnPlayerKilled();
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

    public void pickUpCoin()
    {
        _coins++;
        _score += 200;
    }
}
