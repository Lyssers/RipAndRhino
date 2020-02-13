using System;
using UnityEngine;
using UnityEngine.UI;

namespace RhinoGame
{
	public class Player : Unit
	{       
		/// <summary>
		/// Movement speed in all directions.
		/// </summary>
		public float moveSpeed = 8f;
        
		/// <summary>
		/// Reference to the camera following component.
		/// </summary>
		[HideInInspector]
		public SmoothFollowTarget camFollow;

		private Rigidbody rb;

		public void Awake ()
		{
			rb = GetComponent<Rigidbody> ();

			camFollow = Camera.main.GetComponent<SmoothFollowTarget>();
			camFollow.target = transform;
		}

		private void FixedUpdate()
		{
			


			if ((rb.constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.FreezePositionY)
			{
				if(transform.position.y > 0)
				{
					rb.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
				}
			}

				Vector2 moveDir;

			if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
		{
		moveDir.x = 0;
		moveDir.y = 0;
}
else
	{
				moveDir.x = Input.GetAxis("Horizontal");
				moveDir.y = Input.GetAxis("Vertical");
				//Vector3 movementDir = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0.0f, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);

				if (Input.GetAxisRaw("Vertical") > 0)
				{
					
					Vector3 movementDir = transform.forward * moveSpeed * Time.deltaTime;
					rb.MovePosition(rb.position + movementDir);
				}
				if (Input.GetAxisRaw("Vertical") < 0)
				{
					Vector3 movementDir = -transform.forward * moveSpeed * Time.deltaTime;
					rb.MovePosition(rb.position + movementDir);
				}
				if (Input.GetAxisRaw("Horizontal") > 0)
				{
					
					Vector3 movementDir = transform.right * moveSpeed * Time.deltaTime;
					rb.MovePosition(rb.position + movementDir);
				}
				if (Input.GetAxisRaw("Horizontal") < 0)
				{
					Vector3 movementDir =- transform.right * moveSpeed * Time.deltaTime;
					rb.MovePosition(rb.position + movementDir);
				}
					
				//rb.MovePosition(rb.position + movementDir);
			
				

				//Move(moveDir);
			}


				//rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, moveSpeed * Input.GetAxis("Mouse X"), 0f)));
			rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, moveSpeed * Input.GetAxis("Mouse X"), 0f));
			if (Input.GetKey(KeyCode.Space))
			{
				Shoot();
			}
		}

		public override void UnitDied()
		{
			base.UnitDied();
			LevelManager.Instance.GameOverCanvas.ShowGameOver("You lost!");
		}

		void Move(Vector2 direction = default(Vector2))
		{
			
			if(direction != Vector2.zero)
			{
			transform.rotation = Quaternion.LookRotation(new Vector3( direction.x, 0, direction.y))
			* Quaternion.Euler(0, camFollow.camTransform.eulerAngles.y, 0);
			}

			Vector3 movementDir = transform.forward * moveSpeed * Time.deltaTime;
			rb.MovePosition(rb.position + movementDir);
		}


	}
}