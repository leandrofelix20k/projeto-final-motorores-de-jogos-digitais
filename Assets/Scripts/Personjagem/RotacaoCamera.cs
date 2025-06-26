using UnityEngine;

public class RotacaoCamera : MonoBehaviour
{
    public float sensMouse = 100f;
    public float anguloMin = -75f, anguloMax = 75f;

    public Transform playerBody; // Referência ao objeto do jogador (rotação horizontal)

    float rotacaoX = 0f; // Rotação vertical da câmera

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Garante que playerBody não seja nulo (se não for atribuído no Inspector)
        if (playerBody == null)
        {
            playerBody = transform.parent; // Assume que o pai é o jogador
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensMouse * Time.deltaTime;

        // Rotação VERTICAL (câmera)
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, anguloMin, anguloMax);
        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        // Rotação HORIZONTAL (jogador)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}