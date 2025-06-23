using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
	[Header("Tên màn chơi tiếp theo (giống trong Build Settings)")]
	public int ManChoiIndex;

	public void LoadManChoiMoi()
	{
		SceneManager.LoadScene(ManChoiIndex);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			LoadManChoiMoi();
		}
	}
}
