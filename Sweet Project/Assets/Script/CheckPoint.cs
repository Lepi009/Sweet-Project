using Com.LepiStudios.SweetProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("GetFlag");
            collision.gameObject.GetComponent<PlayerController>().respawnPosition = transform.position;
        }
    }
}
