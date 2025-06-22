using UnityEngine;
using UnityEngine.UI;

public class AudioToggleUI : MonoBehaviour
{
	public AudioSource bgMusic;          // Kéo AudioSource của BackgroundMusic vào đây
	public GameObject buttonSoundOn;     // Kéo Button_Sound_On vào đây
	public GameObject buttonSoundOff;    // Kéo Button_Sound_Off vào đây

	private bool isMuted = false;

	void Start()
	{
		UpdateUI();
	}

	public void ToggleAudio()
	{
		isMuted = !isMuted;
		bgMusic.mute = isMuted;
		UpdateUI();
	}

	void UpdateUI()
	{
		buttonSoundOn.SetActive(!isMuted);
		buttonSoundOff.SetActive(isMuted);
	}
}
