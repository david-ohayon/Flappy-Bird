using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public AudioSource powerUpSound;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public Text scoreText;

    private enum PageState
    {
		None,
		Start,
		Countdown,
		GameOver
	}

	int score = 0;
    bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

    private void Awake()
    {
		if (Instance != null)
        {
			Destroy(gameObject);
		}
		else
        {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

    private void OnEnable()
    {
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
		CountdownText.OnCountdownFinished += OnCountdownFinished;
	}

	private void OnDisable()
    {
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
	}

	private void OnCountdownFinished()
    {
		SetPageState(PageState.None);
		OnGameStarted();
		score = 0;
		gameOver = false;
	}

	private void OnPlayerScored()
    {
		score++;
        if (score % 2 == 0 && score != 0)
        {
			Time.timeScale += 0.025f;
        }
        // special sound
        if (score % 10 == 0 && score != 0)
        {
			powerUpSound.Play();
        }
        scoreText.text = score.ToString();
	}

	private void OnPlayerDied()
    {
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedScore)
        {
			PlayerPrefs.SetInt("HighScore", score);
		}
		SetPageState(PageState.GameOver);
	}

	private void SetPageState(PageState _state)
    {
		switch (_state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				break;
			case PageState.Start:
				startPage.SetActive(true);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				break;
			case PageState.Countdown:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(true);
				break;
			case PageState.GameOver:
				startPage.SetActive(false);
				gameOverPage.SetActive(true);
				countdownPage.SetActive(false);
				break;
		}
	}
	
	public void ConfirmGameOver()
    {
		SetPageState(PageState.Start);
		scoreText.text = "0";
		OnGameOverConfirmed();
	}

	public void StartGame()
    {
		SetPageState(PageState.Countdown);
    }
}