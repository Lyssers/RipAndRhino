using UnityEngine;

namespace RhinoGame
{
	/// <summary>
	/// Projectile script for player shots with collision/hit logic.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		/// <summary>
		/// Projectile travel speed in units.
		/// </summary>
		public float speed = 10;
        
		/// <summary>
		/// Damage to cause on a player that gets hit.
		/// </summary>
		public int damage = 3;
        
		/// <summary>
		/// Delay until despawned automatically when nothing gets hit.
		/// </summary>
		public float despawnDelay = 1f;
        
		/// <summary>
		/// Clip to play when this projectile gets despawned.
		/// </summary>
		public AudioClip explosionClip;
        
		/// <summary>
		/// Object to spawn when this projectile gets despawned.
		/// </summary>
		public GameObject explosionFX;

		//reference to rigidbody component
		private Rigidbody myRigidbody;
		//reference to collider component
		private SphereCollider sphereCol;

		//get component references
		void Awake ()
		{
			myRigidbody = GetComponent<Rigidbody> ();
			sphereCol = GetComponent<SphereCollider> ();
		}

		void OnSpawn()
		{
			myRigidbody.velocity = speed * transform.forward;
		}

		private void OnCollisionEnter(Collision collision)
		{
			PoolManager.Despawn(gameObject);
		}

		void OnDespawn()
		{
			if (explosionFX)
				PoolManager.Spawn(explosionFX, transform.position, transform.rotation);
			if (explosionClip)
				AudioManager.Play3D(explosionClip, transform.position);

			myRigidbody.velocity = Vector3.zero;
			myRigidbody.angularVelocity = Vector3.zero;
		}

	}
}