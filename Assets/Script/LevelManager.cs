using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<NodeController> nodes;
    public static LevelManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNode(int row, int column, Direction direction){
        
        switch (direction){
            case Direction.UP: row++;
            break;
            case Direction.DOWN: row--;
            break;
            case Direction.LEFT: column--;
            break;
            case Direction.RIGHT: column++;
            break;
        }
        NodeController node = nodes.Find(x => x.row == row && x.col == column && x.NodeType == NodeType.NORMAL);
        if(node != null){
             return node.gameObject;
        }

        return null;
       
    }
}

public enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT

}