using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public bool lockX = false;
	public bool lockY = false;
	public bool lockZ = false;

	public float followSphereRadius = 1f;

	public float followSpeed = 2f;

	public GameObject followTarget;

	private Vector3 initialOffset;

	private Vector3 followOriginToTargetReal;

	void Awake()
	{
		initialOffset = followTarget.transform.position - transform.position;
	}
	
	void Update()
	{
		followOriginToTargetReal = followTarget.transform.position - (transform.position + initialOffset);

		float overExtendedAmount = followOriginToTargetReal.magnitude - followSphereRadius;
		if (overExtendedAmount > 0f)
		{
			transform.position += followOriginToTargetReal.normalized * overExtendedAmount;
			followOriginToTargetReal = followTarget.transform.position - (transform.position + initialOffset);
			overExtendedAmount = 0f;
		}

		float distanceToPerfectFollow = followOriginToTargetReal.magnitude;
		float followFrac = 1 - (distanceToPerfectFollow / followSphereRadius); // 0-1 where 1 is edge of follow radius
		
		float speedThisFrame = Mathf.Lerp(followSpeed/2f, followSpeed, followFrac);

		Vector3 translationThisFrame = followOriginToTargetReal;
		if (distanceToPerfectFollow > speedThisFrame * Time.deltaTime)
		{
			translationThisFrame = followOriginToTargetReal.normalized * 
				speedThisFrame * Time.deltaTime;
		}
		transform.position += translationThisFrame;
	}
}
