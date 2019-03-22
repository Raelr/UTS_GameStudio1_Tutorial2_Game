using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEditor.Experimental.UIElements;
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

    private void Awake() {

        if (instance == null)
        {
            instance = this;
        }
    }

    void Update() {
        ScoreText.text = "MARIO\n" + _score.ToString("000000");
        CoinsText.text = _coins.ToString("00");
        TimeText.text = "TIME\n" + _time.ToString("000");
        _time -= Time.deltaTime * (float)TimeReductionRate;
        if ((int)_time == 0){
            TimeReductionRate = 0;
            OnPlayerKilled();
        }
    }

    public void OnPlayerKilled() {
        player.OnDeath();
    }

    public void pickUpCoin() {
        _coins++;
        _score += 200;
    }
}
