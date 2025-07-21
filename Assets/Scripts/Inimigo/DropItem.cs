using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject itemDrop;
    
    public void Dropa()
    {
        int n = Random.Range(0, 10);

        if(n < 5)
        {
            GameObject item = Instantiate(itemDrop, transform);
            item.transform.parent = null;
        }
        
    }
}
