using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public List<Vector3> positions;
	private Vector3 currentCheckpoint;
	private Vector3 nextCheckpoint;
	public float timeInterval;
	float timePassed;
	int i = 0;
	public Vector3 velocity;
	private Vector3 previous;

	void Start()
	{
		currentCheckpoint = positions[0];
		nextCheckpoint = positions[1];
	}
	void Update()
	{
		timePassed += Time.deltaTime;
		gameObject.transform.position = Vector3.Lerp(currentCheckpoint, nextCheckpoint, timeInterval * timePassed);
		if (gameObject.transform.position == nextCheckpoint)
		{
			timePassed = 0;
			currentCheckpoint = nextCheckpoint;
			nextCheckpoint = NextPlatform();
		}
		velocity = (transform.position - previous) / Time.deltaTime;
      	previous = transform.position;
	}

	Vector3 NextPlatform() {
		if (i >= positions.Count - 1) {
			i = 0;
			return positions[0];
		}
		i++;
		return positions[i];
	}
}
