using UnityEngine;

public class PortalScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Animator>() != null)
        {
            canvas.SetActive(true);
        }
    }
}
