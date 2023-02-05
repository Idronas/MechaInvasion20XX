using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
	static int enemiesThisLevel;
	public static bool isPaused = false;

	public static void ReturnToMainMenu() {
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
	public static void NextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}
	
	public static void ClearEnemiesThisLevel() {
		
	}
	public static void EnemyKilled() {
		enemiesThisLevel--;
		Debug.Log(enemiesThisLevel);
		if (enemiesThisLevel <= 0 ) NextLevel();
	}
	public static void SetEnemiesThisLevel() {

	}
	public static int GetEnemiesThisLevel() {
		return enemiesThisLevel;
	}
	public static void RegisterEnemy() {
		enemiesThisLevel++;
		Debug.Log(enemiesThisLevel);
	}

}
