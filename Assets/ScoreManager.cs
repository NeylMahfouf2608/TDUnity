using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] GameObject end;
    int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore()
    {
        score += 50;
        textMesh.text = "Score : " + score;
        if(score == 150)
        {
            end.SetActive(true);
        }
    }
}
