using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    MovementController movementController;
    EnemyController enemyController;

    public SpriteRenderer sprite;
    public Animator animator;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyController=GetComponent<EnemyController>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        movementController = GetComponent<MovementController>();
        movementController.lastMovngDirection = "left";
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("moving", true);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }

        bool flipX = false;
        bool flipY = false;
        if (movementController.lastMovngDirection == "left")
        {
            animator.SetInteger("direction",0);
        }
        else if (movementController.lastMovngDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }
        else if(movementController.lastMovngDirection== "up")
        {
            animator.SetInteger("direction", 1);
        }
        else if(movementController.lastMovngDirection== "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }
        sprite.flipY = flipY;
        sprite.flipX = flipX;
    }

      
}
