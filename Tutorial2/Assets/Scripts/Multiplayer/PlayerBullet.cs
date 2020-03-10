using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace RhinoGame
{
    public class PlayerBullet : MonoBehaviour
    {
        public int Damage = 10;
        public GameObject Hit;
        public GameObject Smoke;
        private PhotonView photonView;
        private Rigidbody rigidbody;
        private Renderer renderer;

        public Photon.Realtime.Player Owner { get; private set; }

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            rigidbody.velocity = Vector3.zero;
            renderer.enabled = false;
            Hit.SetActive(true);
            Smoke.SetActive(true);
            Destroy(gameObject, 0.5f);

            if (collision.gameObject.CompareTag("Player"))
            {
                var playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.Health -= Damage;
                if (playerController.Health <= 0)
                {
                    collision.gameObject.GetComponent<PhotonView>().RPC("DestroyPlayer", RpcTarget.All);
                    Owner.AddScore(1);
                }
            }
        }

        public void InitializeBullet(Photon.Realtime.Player owner, Vector3 originalDirection, float lag)
        {
            Owner = owner;

            transform.forward = originalDirection;

            rigidbody.velocity = originalDirection * 18f;
            rigidbody.position += rigidbody.velocity * lag;

        }
    }
}