using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	[Header("UI Panels")]
	public GameObject mainMenu;
	public GameObject panelOptions;

	// Ấn Play → vào level 1
	public void PlayGame()
	{
		SceneManager.LoadScene("Man_1");  // Đảm bảo đúng tên scene
	}

	// Ấn Options → bật Panel Options
	public void OpenOptions()
	{
		mainMenu.SetActive(false);
		panelOptions.SetActive(true);
	}

	// Ấn Back → quay về MainMenu
	public void BackToMainMenu()
	{
		panelOptions.SetActive(false);
		mainMenu.SetActive(true);
	}

	// Chọn Level tương ứng
	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}

	// Thoát game
	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit game!"); // Chỉ hoạt động khi build, không chạy trong editor
	}
}
