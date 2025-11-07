using UnityEngine;

public class ChickenLauncher : MonoBehaviour

{
    [SerializeField] GameObject freePoule;

    private void OnTriggerStay(Collider other)
    {
        Animator tryPlayerAnimator = other.gameObject.GetComponent<Animator>();
        if (tryPlayerAnimator != null && tryPlayerAnimator.GetBool("isAttacking"))
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce((Vector3.up + Vector3.forward) * 1000);
            Destroy(gameObject);
        }
    }
}
