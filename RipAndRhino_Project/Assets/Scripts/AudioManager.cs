using UnityEngine;
using UnityEngine.SceneManagement;

namespace RhinoGame
{
	/// <summary>
	/// Handles playback of background music, 2D and 3D one-shot clips during the game.
	/// Makes use of the PoolManager for activating 3D AudioSources at desired world positions.
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		//reference to this script instance
		private static AudioManager instance;

		/// <summary>
		/// Prefab instantiated for playing back one-shot 3D clips.
		/// </summary>
		public GameObject oneShotPrefab;
        

		// Sets the instance reference, if not set already,
		// and keeps listening to scene changes.
		void Awake ()
		{
			if (instance != null)
				return;

			instance = this;
		}


		/// <summary>
		/// Returns a reference to this script instance.
		/// </summary>
		public static AudioManager GetInstance ()
		{
			return instance;
		}

		/// <summary>
		/// Play sound clip passed in in 3D space, with optional random pitch (0-1 range).
		/// Automatically creates an audio source for playback using our PoolManager.
		/// </summary>
		public static void Play3D (AudioClip clip, Vector3 position, float pitch = 0f)
		{
			//cancel execution if clip wasn't set
			if (clip == null)
				return;
			//calculate random pitch in the range around 1, up or down
			pitch = UnityEngine.Random.Range (1 - pitch, 1 + pitch);

			//activate new audio gameobject from pool
			GameObject audioObj = PoolManager.Spawn (instance.oneShotPrefab, position, Quaternion.identity);
			//get audio source for later use
			AudioSource source = audioObj.GetComponent<AudioSource> ();
            
			//assign properties, play clip
			source.clip = clip;
			source.pitch = pitch;
			source.Play ();
            
			//deactivate audio gameobject when the clip stops playing
			PoolManager.Despawn (audioObj, clip.length);
		}
	}
}