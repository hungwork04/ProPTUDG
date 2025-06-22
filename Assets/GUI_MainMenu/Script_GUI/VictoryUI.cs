using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
	public GameObject victoryPanel;

	void Start()
	{
		victoryPanel.SetActive(false);
	}

	public void ShowVictory()
	{
		Time.timeScale = 0f;
		victoryPanel.SetActive(true);
	}

	public void PlayAgain()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Man_1"); 
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");  
	}
}
