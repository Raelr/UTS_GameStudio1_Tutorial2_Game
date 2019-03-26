using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

    private Image _livesBackground;
    private Text _worldLivesUIText;
    private Image _livesImgUI;
    private Text _livesUI;
    public double LivesScreenSecondsDelay;

    [Header("Player reference")]
    [SerializeField]
    Player player;

    [SerializeField]
    FlagPole flag;
    //SFX
    [SerializeField]
    AudioClip backgroundMusic, gameOverSound;

    bool stopTime;

    public int Lives { get; set; }

    private void Start()
    {
        //TO:DO - Add Black screen showing Level and Lives

        Lives = PlayerPrefs.GetInt("Lives");

        _livesBackground = GameObject.Find("Panel").GetComponent<Image>();
        _worldLivesUIText = GameObject.Find("WorldUI_LivesScreen").GetComponent<Text>();
        _livesImgUI = GameObject.Find("LivesImgUI").GetComponent<Image>();
        _livesUI = GameObject.Find("LivesUI").GetComponent<Text>();
        //THIS WILL NEED CHANGING TO PLAYERPREFS
        _livesUI.text = Lives.ToString("0");

        StartCoroutine(DisplayLivesScreen());
    }

    private IEnumerator DisplayLivesScreen() {

        yield return new WaitForSeconds(2f);

        Color c = _livesBackground.color;
        c.a = 0;
        _livesBackground.color = c;
        _worldLivesUIText.color = c;
        _livesImgUI.color = c;
        _livesUI.color = c;
        Time.timeScale = 1f;

        stopTime = false;

        SoundManager.instance.PlayLoop(backgroundMusic);
    }

    private void Awake() {

        if (instance == null) {
            instance = this;
        }
    }

<<<<<<< HEAD
    public void OnEnemyHit(){
        Debug.Log("Enemy Hit");
        // if player status is OnUnTouchable
        if ((player.CurrentStatus&Player.Status.OnUnTouchable) != 0){
            //Nothing happen maybe

        }else if ((player.CurrentStatus&Player.Status.OnInvincible) != 0){
            //Enemy get killed

        }else{
            OnPlayerHurt();
        }
    }

    public void OnPlayerHurt(){
        player.OnHurt();
        if (!player.IsAlive){
            Lives--;

            if (Lives < 0) //Mario dies when he has 0 lives LEFT, not when he reaches 0 lives.
            {
                GameOver();
=======
    void Update()
    {
        ScoreText.text = "MARIO\n" + _score.ToString("000000");
        CoinsText.text = "x " + _coins.ToString("00");
        TimeText.text = "TIME\n" + _time.ToString("000");

        if (!stopTime) {
            _time -= Time.deltaTime * (float)TimeReductionRate;
            if ((int)_time == 0) {
                TimeReductionRate = 0;
                OnPlayerKilled();
>>>>>>> 68b567973ef4f0b60a0d12b0669e03d167461661
            }
        }
    }

    public void OnPlayerKilled() {

        Lives--;

        PlayerPrefs.SetInt("Lives", Lives);

        player.OnDeath();
    }

    public void SetPlayerProjectile(FireProjectile projectile) {

        player.Projectile = projectile;
    }

    public void PlayEndAnimation() {

        flag.InitEndAnimation();
    }

    public void pickUpCoin()
    {
        _coins++;
        ImproveScore(200);
    }

    public void ImproveScore(int amount) {

        _score += amount;
    }

    public IEnumerator PlayAnimation(Animator animator, string animationName, string conditionName, bool value, float time, Action callBack = null) {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName.ToString())) {

            animator.SetBool(conditionName.ToString(), value);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime * time);

            animator.SetBool(conditionName.ToString(), !value);

            if (callBack != null) {

                callBack.Invoke();
            }

        } else {

            yield return null;
        }
    }

    public void RestartLevel() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu() {

        SceneManager.LoadScene("MainMenu");
    }

    public void GetPointsFromTime() {

        ImproveScore(Mathf.RoundToInt(_time * 50));
        stopTime = true;
    }
}
