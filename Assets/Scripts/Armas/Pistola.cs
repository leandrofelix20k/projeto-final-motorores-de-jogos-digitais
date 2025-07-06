using UnityEngine;

public class Pistola : MonoBehaviour
{
    [Header("Referências")]
    public Animator animacao;

    [Header("Configurações")]
    public float cooldownTiro = 0.3f;

    private bool podeAtirar = true;
    private RaycastHit hitInfo;

    void Start()
    {
        if (animacao == null)
            animacao = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && podeAtirar)
        {
            Atirar();
        }
    }

    void Atirar()
    {
        podeAtirar = false;

        // Dispara animação
        animacao.SetTrigger("Atirar");

        // Lógica do tiro
        Vector3 centroTela = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray raio = Camera.main.ScreenPointToRay(centroTela);

        if (Physics.SphereCast(raio, 0.1f, out hitInfo))
        {
            Debug.Log("Acertou: " + hitInfo.transform.name);
        }

        // Reset após cooldown
        Invoke("ResetarTiro", cooldownTiro);
    }

    void ResetarTiro()
    {
        podeAtirar = true;
        animacao.ResetTrigger("Atirar");
    }
}