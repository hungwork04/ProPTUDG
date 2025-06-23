using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
	public void RestartGame()
	{
		Time.timeScale = 1f; 
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");  
	}
}
