using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 4f;

	public float jumpVelocity = 10f;

	public float groundedDistanceCheck = 1f;

	private bool isAirborne = true;

	private Rigidbody rigidbody;

	private int groundedRaycastLayerMask;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		// Defaults minus the Player layer.
		groundedRaycastLayerMask = Physics.DefaultRaycastLayers - (1 << LayerMask.NameToLayer("Player"));
	}

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
			rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
		}
	}

	private void CheckGrounded()
	{
		RaycastHit hit;
		
		if (Physics.Raycast(transform.position, Vector3.down, out hit, groundedDistanceCheck, groundedRaycastLayerMask))
		{
			isAirborne = false;
		}
		else
		{
			isAirborne = true;
		}

		Color lineColor = (isAirborne)? Color.red : Color.white;
	}

	private void MoveRight(float val)
	{
		transform.position += Vector3.right * val * speed * Time.deltaTime;
	}
}
