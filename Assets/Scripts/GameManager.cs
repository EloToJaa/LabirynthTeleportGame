using UnityEngine;
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
        useInfo.text = "";

        LessTimeOff();
        audioSource = GetComponent<AudioSource>();

        InvokeRepeating(nameof(Stopper), 1, 1);
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
    }

    public void EndGame()
    {
        CancelInvoke(nameof(Stopper));
        if(gameWon)
        {
            PlayClip(winClip);
            pauseEnd.text = "You won!";
            reloadInfo.text = "Press R to reload the game";
        }
        else
        {
            PlayClip(loseClip);
            pauseEnd.text = "You lost!";
            reloadInfo.text = "Press R to reload the game";
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
