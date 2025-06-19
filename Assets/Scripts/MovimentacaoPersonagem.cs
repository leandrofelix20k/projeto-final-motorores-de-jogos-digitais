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

    public bool isGrounded;

    Vector3 velocidadeQueda;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent< CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, raioEsfera, chaoMask);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * y;
        player.Move(move * velocidade * Time.deltaTime);
    }

}
