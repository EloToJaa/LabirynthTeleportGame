using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int points { get; private set; }

    public int[] keys { get; private set; } = new int[3];

    [SerializeField]
    private int timeToEnd;

    public bool gamePaused { get; private set; }
    public bool gameEnded { get; private set; }
    public bool gameWon { get; private set; }

    public Text timeText;
    public Text pointsText;
    public Text redKeyText;
    public Text greenKeyText;
    public Text goldKeyText;
    public Image snowflake;

    public GameObject infoPanel;
    public Text pauseEnd;
    public Text reloadInfo;
    public Text useInfo;

    [SerializeField]
    private AudioClip resumeClip;

    [SerializeField]
    private AudioClip pauseClip;

    [SerializeField]
    private AudioClip winClip;

    [SerializeField]
    private AudioClip loseClip;

    private AudioSource audioSource;

    [SerializeField]
    private MusicManager musicManager;

    private bool lessTime = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        gameEnded = false;
        gameWon = false;

        if (timeToEnd <= 0)
        {
            timeToEnd = 180;
        }

        snowflake.enabled = false;
        timeText.text = timeToEnd.ToString();
        infoPanel.SetActive(false);
        pauseEnd.text = "Pause";
        reloadInfo.text = "";
        SetUseInfo("");

        LessTimeOff();
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1;

        InvokeRepeating(nameof(Stopper), 1, 1);
    }

    public void WinGame()
    {
        gameWon = true;
        gameEnded = true;
    }

    private void LessTimeOn()
    {
        musicManager.PitchThis(1.5f);
    }

    private void LessTimeOff()
    {
        musicManager.PitchThis(1f);
    }

    public void PlayClip(AudioClip playClip)
    {
        if(playClip == null)
        {
            return;
        }
        audioSource.clip = playClip;
        audioSource.Play();
    }

    public void AddKey(KeyColor keyColor)
    {
        keys[(int)keyColor]++;
        if(keyColor == KeyColor.Red)
        {
            redKeyText.text = keys[(int)keyColor].ToString();
        }
        else if(keyColor == KeyColor.Green)
        {
            greenKeyText.text = keys[(int)keyColor].ToString();
        }
        else if(keyColor == KeyColor.Gold)
        {
            goldKeyText.text = keys[(int)keyColor].ToString();
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        pointsText.text = points.ToString();
    }

    public void AddTime(int timeToAdd)
    {
        timeToEnd += timeToAdd;
        timeText.text = timeToEnd.ToString();
    }

    public void FreezeTime(int freezeFor)
    {
        CancelInvoke(nameof(Stopper));
        snowflake.enabled = true;
        InvokeRepeating(nameof(Stopper), freezeFor, 1);
    }

    private void Update()
    {
        PauseCheck();

        if(gameEnded)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Game");
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                Application.Quit();
            }
        }
    }

    public void EndGame()
    {
        CancelInvoke(nameof(Stopper));

        infoPanel.SetActive(true);
        reloadInfo.text = "Press R to reload the game\nPress N to quit";
        Time.timeScale = 0;

        if (gameWon)
        {
            PlayClip(winClip);
            pauseEnd.text = "You won!";
        }
        else
        {
            PlayClip(loseClip);
            pauseEnd.text = "You lost!";
        }
    }

    private void Stopper()
    {
        timeToEnd--;
        timeText.text = timeToEnd.ToString();
        snowflake.enabled = false;

        if (timeToEnd <= 0)
        {
            gameEnded = true;
            gameWon = false;
        }

        if (gameEnded)
        {
            EndGame();
        }

        if(timeToEnd <= 30 && !lessTime)
        {
            LessTimeOn();
            lessTime = true;
        }
        else if (timeToEnd > 30 && lessTime)
        {
            LessTimeOff();
            lessTime = false;
        }
    }

    public void SetUseInfo(string info)
    {
        useInfo.text = info;
    }

    public void PauseGame()
    {
        PlayClip(pauseClip);
        infoPanel.SetActive(true);
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        PlayClip(resumeClip);
        infoPanel.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    private void PauseCheck()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
