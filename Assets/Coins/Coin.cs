using UnityEngine;

public class Coin : MonoBehaviour
{
    private MeshRenderer mr;
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject ps;
    private AudioSource source;
    private BoxCollider bc;


    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        source = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        Animator tryPlayerAnimator = other.gameObject.GetComponent<Animator>();
        if (tryPlayerAnimator != null && tryPlayerAnimator.GetBool("isGathering"))
        {
            mr.enabled = false;
            bc.enabled = false;
            source.PlayOneShot(clip);
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject, clip.length);
            ScoreManager.Instance.AddScore();
        }
    }
}
