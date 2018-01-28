using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private static readonly float groundedDistanceCheck = 1f;

	public float speed = 4f;

	private bool isAirborne = true;

	void Update()
	{
		CheckGrounded();

		//TODO: acceleration and reduced air control.
		float moveRightAmount = Input.GetAxis("Horizontal");
		if (Mathf.Abs(moveRightAmount) > 0.3f)
		{
			MoveRight(moveRightAmount);
		}

		if (Input.GetButtonDown("Jump") && !isAirborne)
		{
			//TODO jump.
		}
	}

	private void CheckGrounded()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down * groundedDistanceCheck, out hit))
		{
			//TODO
		}
	}

	private void MoveRight(float val)
	{
		transform.position += Vector3.right * val * speed;
	}
}
