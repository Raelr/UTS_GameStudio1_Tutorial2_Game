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
        //Set delay on Lives Screen
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + (float)LivesScreenSecondsDelay;
        while (Time.realtimeSinceStartup < pauseEndTime) {
            yield return 0;
        }

        Color c = _livesBackground.color;
        c.a = 0;
        _livesBackground.color = c;
        _worldLivesUIText.color = c;
        _livesImgUI.color = c;
        _livesUI.color = c;
        Time.timeScale = 1f;

        SoundManager.instance.PlayLoop(backgroundMusic);
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

    private void GameOver()
    {
        SoundManager.instance.PlaySingle(gameOverSound);
        //TO:DO - Game Over code here
    }

    public void pickUpCoin()
    {
        _coins++;
        _score += 200;
    }

    public IEnumerator PlayAnimation(Animator animator, string animationName, string conditionName, bool value, Action callBack = null) {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName.ToString())) {

            animator.SetBool(conditionName.ToString(), value);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime * 5);

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
}
