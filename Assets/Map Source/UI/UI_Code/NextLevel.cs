using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
	[Header("Tên màn chơi tiếp theo (giống trong Build Settings)")]
	public string tenManChoi;

	public void LoadManChoiMoi()
	{
		SceneManager.LoadScene(tenManChoi);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			LoadManChoiMoi();
		}
	}
}
