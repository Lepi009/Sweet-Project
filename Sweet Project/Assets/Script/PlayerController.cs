using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios.SweetProject {

	public class PlayerController : MonoBehaviour
	{
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

        #endregion

        #region Private Fields

        /// <summary>var saves the ridigbody of the player</summary>
        private Rigidbody2D rb;

        /// <summary>var saves the sprite renderer of the player</summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>var which is true if the player is grounded, otherwise false</summary>
        bool isGrounded = false;

        /// <summary>var which saves the actual amount of jumps in a row</summary>
        private int actualJumps = 0;

        private Animator animator;

        #endregion


        private void Start()
        {
            //get all important components of the player
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            actualJumps = 0;
        }

        private void Update()
        {
            Vector2 newVelocity = Vector2.zero;

            if (Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                newVelocity = new Vector2(speed, rb.velocity.y);
                animator.SetBool("Walking", true);
            } else if (Input.GetKey(KeyCode.A))
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

            //proofes first if the player is grounded
            isGrounded = Physics2D.OverlapCircle(groundChecker.position, radius, layerMaskGround);

            if (isGrounded && rb.velocity.y >= 0)
            {
                actualJumps = 0;
                animator.SetBool("Fall", false);
            } else if(!isGrounded && rb.velocity.y <= 0)
            {
                //the player is falling
                animator.SetBool("Fall", true);
            }

            if (Input.GetKeyDown(KeyCode.Space) && actualJumps < maxJumps)
            {
                animator.SetTrigger("Jump");
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
        }

    }

}
