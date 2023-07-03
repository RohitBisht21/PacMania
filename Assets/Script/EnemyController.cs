using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodesStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes,
    }
    public GhostNodesStatesEnum ghostNodesState;
    public GhostNodesStatesEnum respawnState;
    
    public enum GhostType
    {
        red,
        blue,
        pink, 
        orange
    }
    public GhostType ghostType;
    

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;
    public bool ReadyToLeaveHome = false;

    public GameManager gameManager;

    public bool testRespawn = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType==GhostType.red)
        {
            ghostNodesState = GhostNodesStatesEnum.startNode;
            startingNode = ghostNodeStart;
            respawnState = GhostNodesStatesEnum.centerNode;
            ReadyToLeaveHome =true;
        }
        else if(ghostType==GhostType.pink)
        {
            ghostNodesState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodesStatesEnum.centerNode;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesState = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodesStatesEnum.leftNode;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesState = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodesStatesEnum.rightNode;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(testRespawn== true)
        {
            ReadyToLeaveHome = false;
            ghostNodesState = GhostNodesStatesEnum.respawning;
            testRespawn = false;
        }
    }

    public void reachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesState == GhostNodesStatesEnum.movingInNodes)
        {   
                if (ghostType == GhostType.red)
                {
                DetermineRedGhostDirection();
                } 
              
        }
        else if(ghostNodesState == GhostNodesStatesEnum.respawning)
        {

            string direction = "";
            if(transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }
            else if(transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if(respawnState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesState = respawnState;
                }
                else if(respawnState == GhostNodesStatesEnum.leftNode)
                {
                    direction= "left";
                }
                else if (respawnState == GhostNodesStatesEnum.rightNode)
                {
                    direction= "right";
                }
            }
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y) || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodesState= respawnState;
            }
            else
            {
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }
            movementController.SetDirection(direction);
        }
        else
        {
            if (ReadyToLeaveHome)
            {
                if(ghostNodesState == GhostNodesStatesEnum.leftNode)
                {
                    ghostNodesState= GhostNodesStatesEnum.centerNode;   
                    movementController.SetDirection("right");
                }
                else if (ghostNodesState == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.centerNode;
                         movementController.SetDirection("left");
                }
                else if(ghostNodesState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesState= GhostNodesStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                else if(ghostNodesState == GhostNodesStatesEnum.startNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }
    void DeterminePinkGhostDirection()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovngDirection;
        float distanceBetweennodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position; 
        if(pacmansDirection == "left")
        {
            target.x -= distanceBetweennodes*2;
        }
        else if (pacmansDirection == "right")
        {
            target.x += distanceBetweennodes*2;
        }
        else if(pacmansDirection == "up")
        {
            target.y += distanceBetweennodes*2;
        }
        else if(pacmansDirection== "down")
        {
            target.y -= distanceBetweennodes*2;
        }
        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);

    }
    void DetermineBlueGhostDirection()
    {

    }
    void DetermineOrangeGhostDirection()
    {

    }
    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovngDirection = movementController.lastMovngDirection;
        string newDirection = "";
        NodeController nodeController= movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveUp && lastMovngDirection!= "down")
        {
            GameObject nodeUp = nodeController.nodeUp;
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if(distance< shortestDistance || shortestDistance==0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }
        if (nodeController.canMoveDown && lastMovngDirection != "up")
        {
            GameObject nodeDown = nodeController.nodeDown;
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }
        if (nodeController.canMoveRight && lastMovngDirection != "left")
        {
            GameObject nodeRight = nodeController.nodeRight;
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }
        if (nodeController.canMoveLeft && lastMovngDirection != "right")
        {
            GameObject nodeLeft = nodeController.nodeLeft;
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }
        return newDirection;
    }
}
