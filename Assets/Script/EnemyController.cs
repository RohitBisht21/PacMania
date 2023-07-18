using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool isVisible=true;
    public SpriteRenderer ghostSprite;
    public SpriteRenderer eyesSprite;
    void Awake()
    {
        ghostSprite = GetComponent<SpriteRenderer>();
        eyesSprite = GetComponentInChildren<SpriteRenderer>();

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
        if(isVisible)
        {
            ghostSprite.enabled = true;
            eyesSprite.enabled = true;
        }
        else{
            ghostSprite.enabled = false;
            eyesSprite.enabled = false;
        }
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
                    movementController.SetDirection(Direction.RIGHT);
                }
                else if (ghostNodesState == GhostNodesStatesEnum.rightNode)
                {
                    ghostNodesState= GhostNodesStatesEnum.centerNode;
                    movementController.SetDirection(Direction.LEFT);
                }
                else if (ghostNodesState == GhostNodesStatesEnum.centerNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.startNode;
                    movementController.SetDirection(Direction.UP);
                }
                else if(ghostNodesState == GhostNodesStatesEnum.startNode)
                {
                    ghostNodesState = GhostNodesStatesEnum.movingInNodes;
                    movementController.SetDirection(Direction.RIGHT);
                }

            }
        }
    }
    void DetermineRedGhostDirection()
    {
           Direction direction = GetClosestDirection(gameManager.pacman.transform.position);
            movementController.SetDirection(direction);
    }
    void DeterminePinkGhostDirection()
    {
           Direction pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovngDirection;
            float distanceBetweenNodes = 0.31f;

            Vector2 target = gameManager.pacman.transform.position;
            if(pacmanDirection == Direction.LEFT)
            {
                target.x -= (distanceBetweenNodes*2);
            }
            else if (pacmanDirection == Direction.RIGHT)
            {
                target.x += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection ==Direction.UP)
            {
                target.y += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection ==Direction.DOWN)
            {
                target.y -= (distanceBetweenNodes*2);
            }
           Direction direction = GetClosestDirection(target);
            movementController.SetDirection(direction);
    }
    
    void DetermineBlueGhostDirection()
    {
         Direction pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovngDirection;
            float distanceBetweenNodes = 0.31f;

            Vector2 target = gameManager.pacman.transform.position;
            if(pacmanDirection == Direction.LEFT)
            {
                target.x -= (distanceBetweenNodes*2);
            }
            else if (pacmanDirection == Direction.RIGHT)
            {
                target.x += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection == Direction.UP)
            {
                target.y += (distanceBetweenNodes*2);
            }
            else if(pacmanDirection == Direction.DOWN)
            {
                target.y -= (distanceBetweenNodes*2);
            }

            GameObject redGhost = gameManager.redGhost;
            float xDistance = target.x - redGhost.transform.position.x;
            float yDistance = target.y - redGhost.transform.position.y;

            Vector2 blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
            Direction direction = GetClosestDirection(blueTarget);
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
   Direction GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        Direction lastMovngDirection = movementController.lastMovngDirection;
        Direction newDirection=0;
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if(nodeController.nearbyNodes[(int)Direction.UP] && lastMovngDirection != Direction.DOWN)
        {
            GameObject nodeUp= nodeController.nearbyNodes[(int)Direction.UP];
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= Direction.UP;
            }
        }
        if(nodeController.nearbyNodes[(int)Direction.DOWN]&& lastMovngDirection != Direction.UP)
        {
            GameObject nodeDown= nodeController.nearbyNodes[(int)Direction.DOWN];
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= Direction.DOWN;
            }
        }
        if(nodeController.nearbyNodes[(int)Direction.LEFT] && lastMovngDirection != Direction.RIGHT)
        {
            GameObject nodeLeft= nodeController.nearbyNodes[(int)Direction.LEFT];
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= Direction.LEFT;
            }
        }
        if(nodeController.nearbyNodes[(int)Direction.RIGHT] && lastMovngDirection != Direction.LEFT)
        {
            GameObject nodeRight=nodeController.nearbyNodes[(int)Direction.RIGHT];
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection= Direction.RIGHT;
            }
        }
        return newDirection;

    }

    public void SetVisible(bool newVisible){
        isVisible = newVisible;
    }

      public void OnTriggerEnter2D(Collider2D collision){
            if(collision.tag== "Player"){
                if(gameManager.redGhost || gameManager.blueGhost || gameManager.pinkGhost || gameManager.OrangeGhost){
                    gameManager.death.Play();
                }
                 SceneManager.LoadScene("Restart");
            }
        }
   
      
}
