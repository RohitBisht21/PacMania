using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoremanager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Update()
    {
        scoreText.text = " Score : " + Mathf.Round(scoreCount);
    }
}
