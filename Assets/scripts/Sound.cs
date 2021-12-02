using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    private bool muted;

    public GameObject musicToggleButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleAudio();
        }
    }

    public void ToggleAudio()
    {
        if (muted)
        {
            DisableAudio();
            musicToggleButton.GetComponent<Image>().sprite = musicOnSprite;
        }
        else
        {
            EnableAudio();
            musicToggleButton.GetComponent<Image>().sprite = musicOffSprite;
        }
    }

    public void DisableAudio()
    {
        SetAudioMute(false);
    }

    public void EnableAudio()
    {
        SetAudioMute(true);
    }

    private void SetAudioMute(bool mute)
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int i = 0; i < sources.Length; ++i)
        {
            sources[i].mute = mute;
        }
        muted = mute;
    }
}