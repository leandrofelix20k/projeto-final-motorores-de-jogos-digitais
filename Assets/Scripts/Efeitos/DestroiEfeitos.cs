using UnityEngine;

public class DestroiEfeitos : MonoBehaviour
{
    public float tempo = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, tempo);
    }

}
