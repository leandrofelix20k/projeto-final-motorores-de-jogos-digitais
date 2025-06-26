using UnityEngine;

public class RotacaoCamera : MonoBehaviour
{
    public float sensMouse = 100f;
    public float anguloMin = -75f, anguloMax = 75f;

    public Transform playerBody; // Refer�ncia ao objeto do jogador (rota��o horizontal)

    float rotacaoX = 0f; // Rota��o vertical da c�mera

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Garante que playerBody n�o seja nulo (se n�o for atribu�do no Inspector)
        if (playerBody == null)
        {
            playerBody = transform.parent; // Assume que o pai � o jogador
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensMouse * Time.deltaTime;

        // Rota��o VERTICAL (c�mera)
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, anguloMin, anguloMax);
        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        // Rota��o HORIZONTAL (jogador)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}