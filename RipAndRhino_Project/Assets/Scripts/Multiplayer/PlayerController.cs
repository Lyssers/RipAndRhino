using UnityEngine;
using Photon.Pun;

namespace RhinoGame
{
    public class PlayerController : MonoBehaviour
    {
        public int Health = 100;
        public float RotationSpeed = 8.0f;
        public float moveSpeed = 10f;

        /// <summary>
        /// Delay between shots.
        /// </summary>
        public float FireRate = 0.75f;
        public GameObject BulletPrefab;
        [HideInInspector]
        public SmoothFollowTarget camFollow;
        [HideInInspector]
        public bool controllable = true;
        public Transform ShootingPos;

        private PhotonView photonView;
        private new Rigidbody rb;
        //timestamp when next shot should happen
        private float nextFire;

        public void Awake()
        {

            photonView = GetComponent<PhotonView>();
            rb = GetComponent<Rigidbody>();


            if (photonView.IsMine)
            {
                camFollow = Camera.main.GetComponent<SmoothFollowTarget>();
                camFollow.target = transform;
            }
        }

        public void Update()
        {
            if (!photonView.IsMine || !controllable)
            {
                return;
            }

            if ((rb.constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.FreezePositionY)
            {
                if (transform.position.y > 0)
                {
                    rb.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
                }
            }
            

            Vector2 moveDir;

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
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
                    Vector3 movementDir = -transform.right * moveSpeed * Time.deltaTime;
                    rb.MovePosition(rb.position + movementDir);
                }

            }

            if (transform.rotation.x > 15  || transform.rotation.x < -15 || transform.rotation.z > 15 || transform.rotation.z < -15)
            {
                rb.rotation = Quaternion.Euler(0, 0, 0);
            }

            rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, moveSpeed * Input.GetAxis("Mouse X"), 0f));
            if (Input.GetKey(KeyCode.Space))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, transform.rotation);
            }
        }

        


        [PunRPC]
        public void Fire(Quaternion rotation, PhotonMessageInfo info)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + FireRate;
                float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

                GameObject bullet = Instantiate(BulletPrefab, ShootingPos.position, Quaternion.identity) as GameObject;
                bullet.GetComponent<PlayerBullet>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));

            }
        }

    }
}