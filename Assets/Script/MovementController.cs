using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject currentNode;
    public float speed = 5f;

    public string direction = "";
    public string lastMovngDirection = "";

    public GameManager gameManager;
    public bool canWarp = true;

    public bool isGhost = false;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if((direction=="left" && lastMovngDirection == "right") || (direction=="right"&& lastMovngDirection=="left") || (direction == "up" && lastMovngDirection == "down") || (direction == "down" && lastMovngDirection == "up"))
        {
            reverseDirection = true;
        }
        if((transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y) || reverseDirection)
        {
            if (isGhost)
            {
                GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);   
            }

           if(currentNodeController.isWarpNodeLeft && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovngDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else if (currentNodeController.isWarpNodeRight && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovngDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else
            {
                if (currentNodeController.isGhostStartingNode && direction == "down" && (!isGhost || GetComponent<EnemyController>().ghostNodesState != EnemyController.GhostNodesStatesEnum.respawning))
                {
                    direction = lastMovngDirection;
                }
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovngDirection = direction;
                }
                else
                {
                    direction = lastMovngDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }
            }
        }
        else
        {
            canWarp = true;
        }
    }

    public void SetSpeed(float newspeed)
    {
        speed = newspeed;
    }

    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }
}
