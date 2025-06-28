using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 3f;
    public float gravidade = -20f;

    public Transform groundCheck;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    public Transform cameraTransform; // Refer�ncia � transforma��o da c�mera
    public bool estaAbaixado;
    public bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
    RaycastHit hit;

    void Start()
    {
        controle = GetComponent<CharacterController>();
        estaAbaixado = false;
        cameraTransform = Camera.main.transform;

        // Se cameraTransform n�o for atribu�do no Inspector, pega a c�mera principal
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
        float z = Input.GetAxis("Vertical"); // Use "z" em vez de "y" (eixo Z = frente/tr�s)

        // **Movimento baseado na rota��o da c�mera**:
        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0; // Remove inclina��o vertical (para evitar subir/descer ao olhar para cima/baixo)
        move = move.normalized;

        controle.Move(move * velocidade * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        velocidadeCai.y += gravidade * Time.deltaTime;
        controle.Move(velocidadeCai * Time.deltaTime);

        if(estaAbaixado)
        {
            checarBloqueioAbaixado();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Abaixar();
        }
    }

    void Abaixar()
    {
        if(levantarBloqueado || estaNoChao == false)
        {
            return ;
        }

        estaAbaixado = !estaAbaixado;
        if (estaAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
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