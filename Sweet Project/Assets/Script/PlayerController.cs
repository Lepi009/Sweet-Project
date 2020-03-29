using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.LepiStudios.myChatConsole;
using Photon.Pun;

namespace Com.LepiStudios.SweetProject {

	public class PlayerController : MonoBehaviourPun
	{
        #region Public Fields
        /// <summary>the actual respawn position of the player</summary>
        public Vector3 respawnPosition;

        /// <summary>var which is true if the player is grounded, otherwise false</summary>
        public bool isGrounded = false;

        #endregion

        #region Private Serialized Fields

        [Tooltip("The speed with which the player moves")]
        [SerializeField]
        private float speed;

        [Tooltip("The force with which the player jumps in the height")]
        [SerializeField]
        private float jumpForce;

        [Tooltip("The jumpforces for every air jump")]
        [SerializeField]
        private float airJumpForce;

        [Tooltip("The transform component of the object for checking if the player is grounded")]
        [SerializeField]
        private Transform groundChecker;

        [Tooltip("The layermask of the ground")]
        [SerializeField]
        private LayerMask layerMaskGround;

        [Tooltip("The radius of the ground checker")]
        [SerializeField]
        private float radius;

        [Tooltip("The maximum amount of jumps in a row")]
        [SerializeField]
        private int maxJumps = 1;

        [Tooltip("The minimum line of exist")]
        [SerializeField]
        private float minPosition;

        [Tooltip("The player name text field")]
        [SerializeField]
        private Text playerNameText;

        #endregion

        #region Private Fields

        /// <summary>var saves the ridigbody of the player</summary>
        private Rigidbody2D rb;

        /// <summary>var saves the sprite renderer of the player</summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>var which saves the actual amount of jumps in a row</summary>
        private int actualJumps = 0;

        private Animator animator;

        private int bananas;


        private bool blockingMovement = false;

        private GeneralChatController chatController;

        #endregion


        private void Start()
        {
            playerNameText.text = photonView.Owner.NickName;

            respawnPosition = GameObject.FindGameObjectWithTag("Respawn").transform.position;

            transform.position = respawnPosition;

            //get all important components of the player
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            actualJumps = 0;

            //subscribe the events
            chatController = GameObject.FindGameObjectWithTag("Chat").GetComponent<GeneralChatController>();
            chatController.OnChat.AddListener(BlockMovements);
            chatController.OnDisableChat.AddListener(ReleaseMovements);

            //assign the camera to the player if the player controlls this GO
            if(photonView.IsMine)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().AssignCameraToLocalPlayer(this.gameObject);
            }
        }

        private void Update()
        {
            if (transform.position.y < minPosition) transform.position = respawnPosition;
            
            //proofes first if the player is grounded
            isGrounded = Physics2D.OverlapCircle(groundChecker.position, radius, layerMaskGround);

            if (!photonView.IsMine) return;
            Vector2 newVelocity = Vector2.zero;

            if (Input.GetKey(KeyCode.D) && !blockingMovement)
            {
                spriteRenderer.flipX = false;
                newVelocity = new Vector2(speed, rb.velocity.y);
                animator.SetBool("Walking", true);
            } else if (Input.GetKey(KeyCode.A) && !blockingMovement)
            {
                spriteRenderer.flipX = true;
                newVelocity = new Vector2(-speed, rb.velocity.y);
                animator.SetBool("Walking", true);
            }
            else
            {
                newVelocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("Walking", false);
            }

            rb.velocity = newVelocity;


            if (isGrounded && rb.velocity.y >= 0)
            {
                actualJumps = 0;
                animator.SetBool("Fall", false);
            } else if(!isGrounded && rb.velocity.y <= 0)
            {
                //the player is falling
                animator.SetBool("Fall", true);
            }

            if (Input.GetKeyDown(KeyCode.Space) && actualJumps < maxJumps && !blockingMovement)
            {
                animator.SetTrigger("Jump");
                GetComponent<PlayerNetworkSync>().JumpTriggerUpdate();
                rb.velocity = new Vector2(rb.velocity.x, 0); //reset the y velocity of the rigidbody so that the player jumps again

                actualJumps++;
                float jumpForceTemp = airJumpForce;

                if (actualJumps == 1) jumpForceTemp = jumpForce;

                rb.AddForce(new Vector2(0, jumpForceTemp), ForceMode2D.Impulse);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(groundChecker.position, radius);
            Gizmos.DrawRay(new Vector3(0, minPosition, 0), Vector3.right*100);
            Gizmos.DrawRay(new Vector3(0, minPosition, 0), Vector3.right * -100);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine) return;

            if (other.gameObject.name == "Banana") bananas++;
            GameObject.Destroy(other.gameObject);
        }

        void BlockMovements()
        {
            blockingMovement = true;
        }

        void ReleaseMovements()
        {
            blockingMovement = false;
        }

    }

}
