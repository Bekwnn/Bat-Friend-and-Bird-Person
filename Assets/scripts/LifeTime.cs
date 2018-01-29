using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
	public float lifetime = 2f;

	public float CurrentTime { get; private set; }

	void Awake()
	{
		CurrentTime = 0f;
	}

	void Update()
	{
		CurrentTime += Time.deltaTime;
		if (CurrentTime > lifetime)
		{
			Destroy(gameObject);
		}
	}
}
