using UnityEngine;

namespace RhinoGame
{
	/// <summary>
	/// Camera script for following the player or a different target transform.
	/// </summary>
	public class SmoothFollowTarget : MonoBehaviour
	{
		/// <summary>
		/// The camera target to follow.
		/// Automatically picked up in LateUpdate().
		/// </summary>
		public Transform target;

		/// <summary>
		/// The clamped distance in the x-z plane to the target.
		/// </summary>
		public float distance = 15.0f;

		/// <summary>
		/// The clamped height the camera should be above the target.
		/// </summary>
		public float height = 30.0f;

		/// <summary>
		/// Reference to the Camera component.
		/// </summary>
		[HideInInspector]
		public Camera cam;

		/// <summary>
		/// Reference to the camera Transform.
		/// </summary>
		[HideInInspector]
		public Transform camTransform;
		// How much we 
		public float heightDamping = 2.0f;
		public float rotationDamping = 3.0f;

		private void Start()
		{
			cam = GetComponent<Camera>();
			camTransform = transform;
		}
			private void LateUpdate()
		{
			{
				// Early out if we don't have a target
				if (!target)
					return;

				// Calculate the current rotation angles
				float wantedRotationAngle = target.eulerAngles.y;
				float wantedHeight = target.position.y + height;
				float currentRotationAngle = transform.eulerAngles.y;
				float currentHeight = transform.position.y;

				// Damp the rotation around the y-axis
				currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

				// Damp the height
				currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

				// Set the position of the camera on the x-z plane to:
				// distance meters behind the target
				transform.position = target.position;
				transform.position -= currentRotation * Vector3.forward * distance;

				// Set the height of the camera
				transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

				// Always look at the target
				transform.LookAt(target);

			}
		}
	}
}