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

    public Transform cameraTransform; // Referência à transformação da câmera
    public bool estaAbaixado;
    public bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
    RaycastHit hit;

    void Start()
    {
        controle = GetComponent<CharacterController>();
        estaAbaixado = false;
        cameraTransform = Camera.main.transform;

        // Se cameraTransform não for atribuído no Inspector, pega a câmera principal
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
        float z = Input.GetAxis("Vertical"); // Use "z" em vez de "y" (eixo Z = frente/trás)

        // **Movimento baseado na rotação da câmera**:
        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0; // Remove inclinação vertical (para evitar subir/descer ao olhar para cima/baixo)
        move = move.normalized;

        controle.Move(move * velocidade * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        velocidadeCai.y += gravidade * Time.deltaTime;
        controle.Move(velocidadeCai * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Abaixar();
        }
    }

    void Abaixar()
    {
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, raioEsfera);
    }
}