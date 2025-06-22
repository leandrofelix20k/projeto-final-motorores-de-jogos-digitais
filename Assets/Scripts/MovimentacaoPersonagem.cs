using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    public CharacterController player;
    public float velocidade = 6f;
    public float alturaPulo = 3f;
    public float gravidade = -20f;

    public Transform groundCheck;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;

    public Transform cameraTransform; // Refer�ncia � transforma��o da c�mera

    public bool isGrounded;

    Vector3 velocidadeQueda;

    void Start()
    {
        player = GetComponent<CharacterController>();
        // Se cameraTransform n�o for atribu�do no Inspector, pega a c�mera principal
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, raioEsfera, chaoMask);

        if (isGrounded && velocidadeQueda.y < 0)
        {
            velocidadeQueda.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical"); // Use "z" em vez de "y" (eixo Z = frente/tr�s)

        // **Movimento baseado na rota��o da c�mera**:
        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0; // Remove inclina��o vertical (para evitar subir/descer ao olhar para cima/baixo)
        move = move.normalized;

        player.Move(move * velocidade * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocidadeQueda.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        velocidadeQueda.y += gravidade * Time.deltaTime;
        player.Move(velocidadeQueda * Time.deltaTime);
    }
}