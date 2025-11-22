using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText,highScoreText,gameOverScoreText;
    private int score,highScore;

    [SerializeField]EventSO eventSO;

    [SerializeField] GameObject menuUI, inGameUI, gameOverUI;

    [SerializeField] Image soundButton;
    [SerializeField] Sprite[] soundconSprite;
    public int Score
    {
        get {  return score; }
        set
        {
            score = value;
            scoreText.text=value.ToString();
        }
    }
    public int HighScore
    {
        get { return highScore; }
        set
        {
            highScore = value;
            highScoreText.text = value.ToString();
            PlayerPrefs.SetInt("PlayerHighScore", value);
        }
    }

    private void Awake()
    {
        eventSO.OnScoreIncrement += EventSO_OnScoreIncrement;
        eventSO.OnGameEnded += EventSO_OnGameEnded;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerHighScore"))
        {
            Score = 0;
            HighScore = 0;
        }
        else
        {
            HighScore = PlayerPrefs.GetInt("PlayerHighScore");
        }
        SetSoundICon();
    }
    private void OnDestroy()
    {
        eventSO.OnGameEnded -= EventSO_OnGameEnded;
        eventSO.OnScoreIncrement -= EventSO_OnScoreIncrement;
    }

    private void EventSO_OnGameEnded()
    {
        SoundManager.Instance.PlayGameOver();
        if (Score > HighScore)
        {
            HighScore=Score;
        }
        gameOverScoreText.text = scoreText.text;
        inGameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void GoToHome()
    {
        SoundManager.Instance.PlayUIClick();
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        SoundManager.Instance.PlayUIClick();
        menuUI.SetActive(false);
        inGameUI.SetActive(true);
        Gamemanager.Instance.RaiseEventSO(0);
        SoundManager.Instance.DisableBGmusic();
    }

    public void SoundButton()
    {
        SoundManager.Instance.IsSoundMuted = !SoundManager.Instance.IsSoundMuted;
        SetSoundICon();
        SoundManager.Instance.PlayUIClick();
    }

    void SetSoundICon()
    {
        if (!SoundManager.Instance.IsSoundMuted)
        {
            SoundManager.Instance.EnableBGmusic();
            soundButton.sprite = soundconSprite[0];
        }
        else
        {
            SoundManager.Instance.DisableBGmusic();
            soundButton.sprite = soundconSprite[1];
        }
    }

    private void EventSO_OnScoreIncrement()
    {
        Score++;
    }
}
