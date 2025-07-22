using UnityEngine;

public class Missao : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int numeroCaixas;
    public GameObject boss;
    public bool invocaBoss;
    void Start()
    {
        numeroCaixas = 0;
        invocaBoss = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (numeroCaixas == 3 && invocaBoss)
        {
            invocaBoss = false;
            boss.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("objArrasta"))
        {
            numeroCaixas++;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("objArrasta"))
        {
            numeroCaixas--;
        }
    }
}
