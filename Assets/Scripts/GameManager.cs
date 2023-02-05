using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}


	public int enemiesThisLevel;
	public bool isPaused = false;

	public void ReturnToMainMenu() {
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
	public void NextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}
	
	public void ClearEnemiesThisLevel() {
		
	}
	public void EnemyKilled()
	{
		enemiesThisLevel--;
		Debug.Log(enemiesThisLevel);
		if (enemiesThisLevel <= 0)
		{
			enemiesThisLevel = 0;
			NextLevel();
		}
	}
	public void SetEnemiesThisLevel() {

	}
	public int GetEnemiesThisLevel() {
		return enemiesThisLevel;
	}
	public void RegisterEnemy() {
		enemiesThisLevel++;
	}

}
