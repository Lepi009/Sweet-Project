using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    #region Private Serialized Fields

    [Tooltip("The impulse that will happen to the player")]
    [SerializeField] private Vector2 force;

    #endregion

    #region Private Fields

    private Animator animator;

    private Collision2D lastCollision;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        lastCollision = collision;
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 vector = transform.InverseTransformPoint(collision.GetContact(0).point);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Trigger") || !(vector.y >= -0.05)) return; //if the animation is still playing, there should be a waiting time
            animator.SetTrigger("Trigger");
        }
    }

    public void AddForce()
    {
        lastCollision.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
