using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{

	public delegate void CountdownFinished();
	public static event CountdownFinished OnCountdownFinished;

    private Text countdown;

	public AudioSource countdownSound;

	private void OnEnable()
    {
		countdown = GetComponent<Text>();
		countdown.text = "3";
		countdownSound.Play();
		StartCoroutine(Countdown());
	}

    private IEnumerator Countdown()
    {
		int count = 3;
		for (int i = 0; i <= count; i++)
        {
			if (i != 3)
			{
				countdown.text = (count - i).ToString();
				countdown.color = new Color32(255, 71, 71, 255);
			}
			else
			{
				countdown.text = "Go !";
				countdown.color = new Color32(0, 245, 0, 255);
			}
			yield return new WaitForSeconds(1);
		}

		OnCountdownFinished();
	}
}