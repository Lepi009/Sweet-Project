using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LepiStudios.SweetProject;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerNetworkSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody2D rb;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private PlayerController playerController;

    private Vector2 previousVelocity = Vector2.zero;

    private bool jumpTrigger = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (!photonView.IsMine) rb.gravityScale = 0;
        else
        {
            //StartCoroutine(UpdateNetworkFixed());
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(animator.GetBool("Walking"));
            stream.SendNext(animator.GetBool("Fall"));
            stream.SendNext(jumpTrigger);
            jumpTrigger = false;
        } else
        {
            //update position
            Vector2 newPosition = (Vector2) stream.ReceiveNext();
            Vector2 velocity = (Vector2) stream.ReceiveNext();
            rb.velocity = velocity;
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position = newPosition + velocity * lag;

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

    public void JumpTriggerUpdate()
    {
        jumpTrigger = true;
    }

    /*IEnumerator UpdateNetworkFixed()
    {
        while(true)
        {
            object[] data = new object[]
            {

            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.DoNotCache };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(3, false, raiseEventOptions, sendOptions);

            photonView.RPC(nameof(RPC_RecieveNewPosition), RpcTarget.OthersBuffered, rb.position, rb.velocity);
            photonView.RPC(nameof(RPC_RecieveAnimator), RpcTarget.OthersBuffered, animator.GetBool("Walking"), animator.GetBool("Fall"));
            yield return new WaitForSeconds(0.025f);
        }
    }

     [PunRPC]
     public void RPC_RecieveNewPosition(Vector2 newPosition, Vector2 velocity, PhotonMessageInfo info)
     {
        
        rb.velocity = velocity;
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        rb.position = newPosition + velocity * lag;


        if (velocity.x < 0) spriteRenderer.flipX = true;
        else if(velocity.x > 0) spriteRenderer.flipX = false;
    }

    [PunRPC]
    public void RPC_RecieveAnimator(bool walking, bool fall)
    {
        if (animator.GetBool("Walking") != walking) animator.SetBool("Walking", walking);
        if (animator.GetBool("Fall") != fall) animator.SetBool("Fall", fall);
    }


    [PunRPC]
    public void RPC_JumpTriggerUpdate()
    {
        animator.SetTrigger("Jump");
    }*/

}
