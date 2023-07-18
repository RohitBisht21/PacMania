using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class PacManiaEditor : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MyMenu/Create Level")]
    static void CreateLevel()
    {
        int col= 29, row=31;
        float space= 0.3f;
        GameObject node = Selection.activeGameObject;
        int[] hideNodes= new int[]{
                // Add nodes to be hidden
               30, 31, 32,33,34,35,36,37,38,39,40,42,43,44,46,47,48,49,50,51,52,53,54,56,59,88,117,146
         };
         int cnt=0;
        for(int i = 0; i < row; i++)
        {
            Vector3 pos= node.transform.position;
             pos.y += (space*i);
            for(int j = 0; j < col; j++)
            {
            
                int index = ArrayUtility.IndexOf(hideNodes, cnt);
                
                
                Debug.Log(index+ " ,"+ cnt);
                if(index == -1)
                {
                    GameObject newNode= Instantiate(node,pos, Quaternion.identity, node.transform.parent);
                    NodeController nodeController= newNode.GetComponent<NodeController>();
                    nodeController.row= i;
                    nodeController.col= j;
                }
                
                
                pos.x += space;
                cnt++;
                
            }
           
        }
        Debug.Log("Doing Something...");
    }
}