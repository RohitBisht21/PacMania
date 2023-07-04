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
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if(ghostType==GhostType.red)
        {
            ghostNodesState = GhostNodesStatesEnum.startNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if(ghostType==GhostType.pink)
        {
            ghostNodesState = GhostNodesStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesState = GhostNodesStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesState = GhostNodesStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            readyToLeaveHome = true;
        }
        movementController.currentNode = startingNode;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodesState == GhostNodesStatesEnum.movingInNodes)
        {
                if(ghostType == GhostType.red)
                {
                    DetermineRedGhostDirection();
                }
                else if(ghostType == GhostType.pink)
                {
                    DeterminePinkGhostDirection();
                }
                else if(ghostType == GhostType.blue)
                {
                    DetermineBlueGhostDirection();
                }
                else if(ghostType == GhostType.orange)
                {
                    DetermineOrangeGhostDirection();
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
   
        public void OnTriggerEnter2D(Collider2D collision){
            if(collision.tag== "Player"){
                if(gameManager.redGhost || gameManager.blueGhost || gameManager.pinkGhost || gameManager.OrangeGhost){
                    gameManager.death.Play();
                    Time.timeScale = 0;
                }
            }
            else{
                Debug.Log("NOT DETECTED");
            }
        }
}
