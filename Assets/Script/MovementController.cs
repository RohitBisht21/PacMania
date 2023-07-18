using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject currentNode;
    public float speed = 5f;

    public Direction direction;
    public Direction lastMovngDirection;

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
            if(currentNodeController==null)
            {
                Debug.Log(currentNode.transform.position + "  "+ currentNode.gameObject.name);
            }
        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if((direction==Direction.LEFT && lastMovngDirection == Direction.RIGHT) || (direction==Direction.RIGHT&& lastMovngDirection==Direction.LEFT) || (direction == Direction.UP&& lastMovngDirection == Direction.DOWN) || (direction == Direction.DOWN && lastMovngDirection == Direction.UP))
        {
            reverseDirection = true;
        }
        if((transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y) || reverseDirection)
        {
            if (isGhost)
            {
                GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);   
            }

           if (currentNodeController.isWarpNodeLeft && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = Direction.LEFT;
                lastMovngDirection = Direction.LEFT;
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
           else if (currentNodeController.isWarpNodeRight && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = Direction.RIGHT;
                lastMovngDirection = Direction.RIGHT;
                transform.position = currentNode.transform.position;
                canWarp = false;
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
                newNode= currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                }
            }
        }
        else
        {
            canWarp = true;
        }
    }

    public void SetDirection(Direction newDirection)
    {
        direction = newDirection;
    }
}
