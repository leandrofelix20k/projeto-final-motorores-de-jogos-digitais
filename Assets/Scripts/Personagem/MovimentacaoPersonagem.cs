using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float velocidadeAbaixado = 3f; // Nova variável para velocidade agachado
    public float alturaPulo = 3f;
    public float gravidade = -20f;

    public Transform groundCheck;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    public Transform cameraTransform;
    public bool estaAbaixado;
    public bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
    RaycastHit hit;

    private float velocidadeNormal;
    private MovimentoCabeca movimentoCabeca;

    void Start()
    {
        controle = GetComponent<CharacterController>();
        estaAbaixado = false;
        cameraTransform = Camera.main.transform;
        velocidadeNormal = velocidade;
        movimentoCabeca = GetComponentInChildren<MovimentoCabeca>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        estaNoChao = Physics.CheckSphere(groundCheck.position, raioEsfera, chaoMask);

        if (estaNoChao && velocidadeCai.y < 0)
        {
            velocidadeCai.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0;
        move = move.normalized;

        // Aplica velocidade reduzida se estiver agachado
        float velocidadeAtual = estaAbaixado ? velocidadeAbaixado : velocidadeNormal;
        controle.Move(move * velocidadeAtual * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            if (movimentoCabeca != null)
                movimentoCabeca.PararPassos();
        }

        velocidadeCai.y += gravidade * Time.deltaTime;
        controle.Move(velocidadeCai * Time.deltaTime);

        if (estaAbaixado)
        {
            checarBloqueioAbaixado();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Abaixar();
        }
    }

    void Abaixar()
    {
        if (levantarBloqueado || !estaNoChao)
        {
            return;
        }

        estaAbaixado = !estaAbaixado;

        if (estaAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
            if (movimentoCabeca != null)
                movimentoCabeca.ReduzirOscilacao(0.5f);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
            if (movimentoCabeca != null)
                movimentoCabeca.RestaurarOscilacao();
        }
    }

    void checarBloqueioAbaixado()
    {
        Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);

        if (Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
        {
            levantarBloqueado = true;
        }
        else
        {
            levantarBloqueado = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, raioEsfera);
    }
}