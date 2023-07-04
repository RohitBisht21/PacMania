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
        movingInNodes
    }
    public GhostNodesStatesEnum ghostNodesState;
    
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

    public bool readyToLeaveHome = false;

    public GameManager gameManager;

    public bool testRespawn = false;

    public bool isFrightened = false;

    public GameObject[] scatterNodes;
    public int scatterNodesIndex;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if(ghostType==GhostType.red)
        {
            ghostNodesState = GhostNodesStatesEnum.startNode;
            startingNode = ghostNodeStart;
        }
        else if(ghostType==GhostType.pink)
        {
            ghostNodesState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesState = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesState = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;  
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
        if (movementController.currentNode.GetComponent<NodeController>().isSideNode)
        {
            movementController.SetSpeed(1);
        }
        else
        {
            movementController.SetSpeed(3);
        }
        
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesState == GhostNodesStatesEnum.movingInNodes)
        {
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                if(transform.position.x == scatterNodes[scatterNodesIndex].transform.position.x && transform.position.y == scatterNodes[scatterNodesIndex].transform.position.y)
                {
                    scatterNodesIndex++;

                    if(scatterNodesIndex == scatterNodes.Length - 1)
                    {
                        scatterNodesIndex = 0;
                    }
                }
                string direction = GetClosestDirection(scatterNodes[scatterNodesIndex].transform.position);
                movementController.SetDirection(direction);
            }
            else if (isFrightened)
            {

            }
            else 
            { 
                if (ghostType == GhostType.red)
                {
                DetermineRedGhostDirection();
                }
            }
        }
        else if(ghostNodesState == GhostNodesStatesEnum.respawning)
        {

        }
        else
        {
            if(readyToLeaveHome)
            {
                if (ghostNodesState == GhostNodesStatesEnum.leftNode) 
                {
                    ghostNodesState = GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                else if (ghostNodesState == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodesState= GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                else if (ghostNodesState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                else if(ghostNodesState == GhostNodesStatesEnum.startNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.movingInNodes;
                    movementController.SetDirection("right");
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
            string pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovngDirection;
            float distanceBetweenNodes = 0.31f;

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
        else if(pacmansDirection== "dpwn")
        {
            target.y -= distanceBetweennodes*2;
        }
        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);

    }
    
    void DetermineBlueGhostDirection()
    {
         string pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovngDirection;
            float distanceBetweenNodes = 0.31f;

            Vector2 target = gameManager.pacman.transform.position;
            if(pacmanDirection == "left")
            {
                target.x -= (distanceBetweenNodes*2);
            }
            else if (pacmanDirection == "right")
            {
                target.x += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection == "up")
            {
                target.y += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection == "down")
            {
                target.y -= (distanceBetweenNodes*2);
            }

            GameObject redGhost = gameManager.redGhost;
            float xDistance = target.x - redGhost.transform.position.x;
            float yDistance = target.y - redGhost.transform.position.y;

            Vector2 blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
            string direction = GetClosestDirection(blueTarget);
            movementController.SetDirection(direction);
    }
    
    void DetermineOrangeGhostDirection()
    {
            float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
            float distanceBetweenNodes = 0.31f;

            if(distance<0)
            {
                distance = -1;
            }
            if(distance <= distanceBetweenNodes * 8)
            {
                DetermineRedGhostDirection();
            }
            else{
                
            }
    }
    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovngDirection = movementController.lastMovngDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if(nodeController.canMoveUp && lastMovngDirection != "down")
        {
            GameObject nodeUp= nodeController.nodeUp;
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= "up";
            }
        }
        if(nodeController.canMoveDown&& lastMovngDirection != "up")
        {
            GameObject nodeDown= nodeController.nodeDown;
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= "down";
            }
        }
        if(nodeController.canMoveLeft && lastMovngDirection != "right")
        {
            GameObject nodeLeft= nodeController.nodeLeft;
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= "left";
            }
        }
        if(nodeController.canMoveRight && lastMovngDirection != "left")
        {
            GameObject nodeRight= nodeController.nodeRight;
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= "right";
            }
        }
        return newDirection;

    }
}
