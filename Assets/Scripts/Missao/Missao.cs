using UnityEngine;

public class Missao : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int numeroCaixas;
    public GameObject boss;
    public bool invocaBoss;
    public Material materialBoss;
    public float valorAmount;

    InimigoLinha scriptBoss;
    public static bool bossMorto;

    void Start()
    {
        numeroCaixas = 0;
        invocaBoss = true;
        valorAmount = 0.6f;
        scriptBoss = boss.GetComponent<InimigoLinha>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numeroCaixas == 8 && invocaBoss)
        {
            invocaBoss = false;
            boss.SetActive(true);
        }

        if (!invocaBoss)
        {
            valorAmount = Mathf.Lerp(valorAmount, 0, Time.deltaTime * 2);
            materialBoss.SetFloat("_Amount", valorAmount);
        }

        bossMorto = scriptBoss.estaMorto;
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
