using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[Header("Main Panels")]
	public GameObject pausePanel;
	public GameObject settingPanel;
	public GameObject tutorialPanel;

	private bool isPaused = false;

	void Start()
	{
		pausePanel.SetActive(false);
		settingPanel.SetActive(false);
		tutorialPanel.SetActive(false);
	}

	public void TogglePause()
	{
		isPaused = !isPaused;

		if (isPaused)
		{
			Time.timeScale = 0f;
			pausePanel.SetActive(true);
		}
		else
		{
			ResumeGame();
		}
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1f;
		pausePanel.SetActive(false);
		settingPanel.SetActive(false);
		tutorialPanel.SetActive(false);
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void OpenSetting()
	{
		settingPanel.SetActive(true);
	}

	public void CloseSetting()
	{
		settingPanel.SetActive(false);
	}

	public void OpenTutorial()
	{
		tutorialPanel.SetActive(true);
	}

	public void CloseTutorial()
	{
		tutorialPanel.SetActive(false);
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");  
	}


}
