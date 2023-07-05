using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pacman;
    
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource munch1;
    public AudioSource munch2;
    public AudioSource death;
    public int currentMunch;

    public int score;
    public Text scoreText;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject blueGhost;
    public GameObject pinkGhost;
    public GameObject OrangeGhost;
    // Start is called before the first frame update
    void Awake()
    {
        pacman = GameObject.Find("Player");
        score = 0;
        currentMunch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }

    public void collectedPellet(NodeController nodecontroller)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if(currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }
        AddToScore(10); 
        Scoremanager.scoreCount+=10;
    }
  
}
