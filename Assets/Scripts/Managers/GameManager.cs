using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
	static List<GameObject> enemiesThisLevel;
	public static bool isPaused = false;

	public static void ReturnToMainMenu() {
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}
	public static void NextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}
	
	public static void ClearEnemiesThisLevel() {
		enemiesThisLevel.Clear();
	}
	public static void EnemyKiled(GameObject e) {
		
	}
	public static void SetEnemiesThisLevel(List<GameObject> e) {
		enemiesThisLevel = e;
	}

}
