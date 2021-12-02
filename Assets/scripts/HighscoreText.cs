using UnityEngine;
using UnityEngine.UI;

public class HighscoreText : MonoBehaviour
{
	private Text score;

	private void OnEnable()
	{
		score = GetComponent<Text>();
        score.text = $"High Score: { PlayerPrefs.GetInt("HighScore") }";
	}
}