using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	// Start is called before the first frame update

	public List<GameObject> enemies;

	void Start()
	{
		
		EnemySpawn[] t = GetComponentsInChildren<EnemySpawn>();
		if (SettingsManager.RandomEnemies == false)
		{
			foreach (EnemySpawn o in t)
			{
				foreach (GameObject i in enemies)
				{
					if (o.thingToSpawn.name == i.name)
					{
						Instantiate(i, o.gameObject.transform.position, o.gameObject.transform.rotation);
					}
				}
			}
			return;
		}
		foreach (EnemySpawn o in t) {
			Instantiate(enemies[(int)Random.Range(0, enemies.Count)], o.gameObject.transform.position, o.gameObject.transform.rotation);
		}

		//GameManager.SetEnemiesThisLevel(enemies);

	}

}
