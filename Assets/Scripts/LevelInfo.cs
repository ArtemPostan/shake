using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
	public int targetCount;

	public List<TextMesh> countTexts;

	public int currentLevelIndex;

	public GameManager gameManagerPfb;

	public bool hasNextLevel = true;

	public bool needFlashlight;

	public LevelManager.gameModes testGameMode;

	public LevelManager.game3Ctypes testGame3CTypes;

	private void Start()
	{
		if (GameManager.Instance == null)
		{
			GameManager gameManager = UnityEngine.Object.Instantiate(gameManagerPfb);
			gameManager.Init();
			gameManager.LevelManager.gameMode = testGameMode;
			gameManager.LevelManager.game3CType = testGame3CTypes;
			gameManager.LevelManager.SetLevelIndex(currentLevelIndex);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		}
		GameManager.Instance.LevelManager.levelInfo = this;
		GameManager.Instance.LevelManager.StartLevel(base.transform.position, targetCount);
		for (int i = 0; i < countTexts.Count; i++)
		{
			countTexts[i].text = targetCount.ToString();
		}
		DataManager.SetEnteredLevel("Level" + currentLevelIndex.ToString());
	}

	private void Update()
	{
	}
}
