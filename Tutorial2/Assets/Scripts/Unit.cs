using RhinoGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

	/// <summary>
	/// Unit's health
	/// </summary>
	[HideInInspector]
	public int Health = 100;

	/// <summary>
	/// Health bar.
	/// </summary>
	public Slider HealthBar;

	/// <summary>
	/// Clip to play when a shot has been fired.
	/// </summary>
	public AudioClip shotClip;

	/// <summary>
	/// Object to spawn on shooting.
	/// </summary>
	public GameObject shotFX;

	//timestamp when next shot should happen
	private float nextFire;

	/// <summary>
	/// Delay between shots.
	/// </summary>
	public float fireRate = 0.75f;

	/// <summary>
	/// Bullet object for shooting.
	/// </summary>
	public GameObject bullet;

	/// <summary>
	/// Position to spawn new bullets at.
	/// </summary>
	public Transform shotPos;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			var bullet = collision.gameObject.GetComponent<Bullet>();
			TakeDamage(bullet.damage);
		}
	}

	public virtual void TakeDamage(int damage)
	{
		Health -= damage;
		HealthBar.value = Health;
		if (Health <= 0)
		{
			UnitDied();
		}
	}

	public void Shoot(Vector2 direction = default(Vector2))
	{
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;

			GameObject obj = PoolManager.Spawn(bullet, shotPos.position, transform.rotation);
			Bullet blt = obj.GetComponent<Bullet>();

			if (shotFX)
				PoolManager.Spawn(shotFX, shotPos.position, Quaternion.identity);
			if (shotClip)
				AudioManager.Play3D(shotClip, shotPos.position, 0.1f);
		}
	}

	public virtual void UnitDied()
	{
		gameObject.SetActive(false);
	}
}
