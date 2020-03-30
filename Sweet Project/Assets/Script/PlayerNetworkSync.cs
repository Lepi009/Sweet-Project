using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Com.LepiStudios.SweetProject
{

    ///<summary>The scripts should be the component of the player, it syncs the player through the internet, should be unique for every project</summary>
    public class PlayerNetworkSync : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        ///<summary>The rigidbody of the player</summary>
        private Rigidbody2D rb;

        ///<summary>The animator of the player</summary>
        private Animator animator;

        ///<summary>The sprite renderer of the player</summary>
        private SpriteRenderer spriteRenderer;

        ///<summary>The player controller of the player</summary>
        private PlayerController playerController;

        ///<summary>var safes the previous velocity of the player, in case that the player does not have authentication on this game object</summary>
        private Vector2 previousVelocity = Vector2.zero;

        ///<summary>var safes if the animator's jump trigger was pressed last frame</summary>
        private bool jumpTrigger = false;

        #endregion

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();

            if (!photonView.IsMine) rb.gravityScale = 0; //otherwise there are some weird behaviours because the synced velocity + in-game gravity is false
        }

        #region Pun Callbacks

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if(stream.IsWriting) //upload all important information
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(animator.GetBool("Walking"));
            stream.SendNext(animator.GetBool("Fall"));
            stream.SendNext(jumpTrigger);
            jumpTrigger = false;
        } 
        else //download all important information
        {
            //update position
            Vector2 newPosition = (Vector2) stream.ReceiveNext();
            Vector2 velocity = (Vector2) stream.ReceiveNext();
            rb.velocity = velocity;
            //calculates the lag, the difference is the delay between the sending and the receiving
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position = newPosition + velocity * lag;
            
            //update rotation
            if (velocity.x < 0) spriteRenderer.flipX = true;
            else if (velocity.x > 0) spriteRenderer.flipX = false;

            //update animator
            bool walking = (bool)stream.ReceiveNext();
            bool fall = (bool)stream.ReceiveNext();
            bool jump = (bool)stream.ReceiveNext();
            if (animator.GetBool("Walking") != walking) animator.SetBool("Walking", walking);
            if (animator.GetBool("Fall") != fall) animator.SetBool("Fall", fall);
            if(jump) animator.SetTrigger("Jump");
        }
    }
    
    #endregion
    
    #region Public Methods
    
    ///<summary>Gets called by the controller script to safe if the jump trigger was triggered</summary>
    public void JumpTriggerUpdate()
    {
        jumpTrigger = true;
    }
    
    #endregion

    }

}
