using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
	[Header("Panels")]
	public GameObject panelTutorial;
	public GameObject panelStory;
	public GameObject panelSetting;

	// Hàm mở Panel Tutorial
	public void OpenTutorial()
	{
		panelTutorial.SetActive(true);
	}

	public void CloseTutorial()
	{
		panelTutorial.SetActive(false);
	}

	// Hàm mở Panel Story
	public void OpenStory()
	{
		panelStory.SetActive(true);
	}

	public void CloseStory()
	{
		panelStory.SetActive(false);
	}

	// Hàm mở Panel Setting
	public void OpenSetting()
	{
		panelSetting.SetActive(true);
	}

	public void CloseSetting()
	{
		panelSetting.SetActive(false);
	}

	// Play Game (chuyển sang scene chơi game)
	public void PlayGame()
	{
		
		SceneManager.LoadScene("Man_1");
		// Debug.Log("Man_1");
	}

	// Thoát game
	public void QuitGame()
	{
		Debug.Log("Quit Game");
		Application.Quit();
	}
}
