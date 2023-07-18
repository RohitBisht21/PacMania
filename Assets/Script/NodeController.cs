using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] private NodeType nodeType;
    
   [HideInInspector] public GameObject[] nearbyNodes= new GameObject[4];
    public int row;
    public int col;
    public bool isWarpNodeRight = false;
    public bool isWarpNodeLeft = false;

    public bool isPelletNode = false;
    public bool hasPelletNode = false;
    public SpriteRenderer pelletSprite;

    public GameManager gameManager;

    #region Properties
    public NodeType NodeType => nodeType; 
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (transform.childCount > 0)
        {
            isPelletNode = true;
            hasPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }

        nearbyNodes[(int)Direction.LEFT] = LevelManager.Instance.GetNode(row,col,Direction.LEFT);
        nearbyNodes[(int)Direction.RIGHT]= LevelManager.Instance.GetNode(row,col,Direction.RIGHT);
         nearbyNodes[(int)Direction.UP] = LevelManager.Instance.GetNode(row,col,Direction.UP);
         nearbyNodes[(int)Direction.DOWN] = LevelManager.Instance.GetNode(row,col,Direction.DOWN);
        
    }

    public GameObject GetNodeFromDirection(Direction direction)
    {
        return nearbyNodes[(int)direction];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player" && hasPelletNode)
        {
            hasPelletNode = false;
            pelletSprite.enabled = false;
            gameManager.collectedPellet(this);
        }
    }
}

public enum NodeType{
    NORMAL,
    OBSTACLE,
    EMPTY
}
