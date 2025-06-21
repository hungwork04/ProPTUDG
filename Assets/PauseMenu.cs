using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;

	// Hàm bật Pause Menu
	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}

	// Hàm trở về màn hình chính
	public void Home()
	{
		Time.timeScale = 1f;  // Trả lại time scale trước khi load scene
		SceneManager.LoadScene("Main Menu");
	}

	// Hàm tiếp tục chơi
	public void Resume()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}

	// Hàm chơi lại màn hiện tại
	public void Restart()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
