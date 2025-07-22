using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject itemDrop, itemMissao;
    
    public void Dropa()
    {
        int n = Random.Range(0, 10);

        if (n > 5)
        {
            GameObject item = Instantiate(itemDrop, transform);
            item.GetComponent<Rigidbody>().AddForce((transform.up * 5 + transform.forward * 3), ForceMode.Impulse);
            item.transform.parent = null;
        }
        if (n < 6)
        {
            GameObject item = Instantiate(itemMissao, transform);
            item.GetComponent<Rigidbody>().AddForce((transform.up * 5 + transform.forward * 3), ForceMode.Impulse);
            item.transform.parent = null;
            
        }
    }
}
