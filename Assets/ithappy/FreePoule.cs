using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(AudioSource))]
public class FreePoule : MonoBehaviour
{ private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_Clip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.GetComponent<Animator>() && other.gameObject.GetComponent<Animator>().GetBool("isAttacking"))
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            rb.AddForce((transform.forward + transform.up) * 50, ForceMode.Impulse);
            m_AudioSource.PlayOneShot(m_Clip);
        }
    }
}
