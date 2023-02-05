using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunnyBox : MonoBehaviour
{
	void OnCollisionEnter(Collision c) {
		SceneManager.LoadScene(2);
	}
}

