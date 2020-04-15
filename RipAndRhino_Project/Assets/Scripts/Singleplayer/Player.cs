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
		public FollowTarget camFollow;

		private Rigidbody rb;

		public void Awake ()
		{
			rb = GetComponent<Rigidbody> ();

			camFollow = Camera.main.GetComponent<FollowTarget>();
			camFollow.target = transform;
		}

		private void FixedUpdate()
		{
			if((rb.constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.FreezePositionY)
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
				Move(moveDir);
			}

			if(Input.GetKey(KeyCode.Space))
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
				transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y))
					* Quaternion.Euler(0, camFollow.camTransform.eulerAngles.y, 0);
			}

			Vector3 movementDir = transform.forward * moveSpeed * Time.deltaTime;
			rb.MovePosition(rb.position + movementDir);
		}


	}
}