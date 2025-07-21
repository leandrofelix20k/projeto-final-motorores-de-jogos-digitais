using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Configuracao Personagem")]
    public CharacterController controle;
    public float velocidade = 6f;
    public float velocidadeAbaixado = 3f; // Nova variável para velocidade agachado
    public float alturaPulo = 3f;
    public float gravidade = -20f;
    public bool estaCorrendo;
    public AudioClip[] audiosPulo;
    AudioSource audioPulo;
    bool noAr;

    [Header("Verifica Chao")]
    public Transform groundCheck;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;
    Vector3 velocidadeCai;

    [Header("Verifica Abaixado")]
    public Transform cameraTransform;
    public bool estaAbaixado;
    public bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
    float velocidadeCorrente = 1f;
    RaycastHit hit;

    private float velocidadeNormal;
    private MovimentoCabeca movimentoCabeca;
    private MovimentaCabecaNew movimentoCabecaNew;

    [Header("Status Personagem")]
    public float hp = 100f;
    public float stamina = 100f;
    public bool cansado;
    public Respiracao scriptRespiracao;

    void Start()
    {
        cansado = false;
        estaCorrendo = false;
        controle = GetComponent<CharacterController>();
        estaAbaixado = false;
        cameraTransform = Camera.main.transform;
        velocidadeNormal = velocidade;
        movimentoCabeca = GetComponentInChildren<MovimentoCabeca>();
        movimentoCabecaNew = GetComponentInChildren<MovimentaCabecaNew>();
        audioPulo = GetComponent<AudioSource>();
        noAr = false;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        Verificacoes();
        MovimentoAbaixa();
        Inputs();
        CondicaoPlayer();
        somPulo();
    }

    void somPulo()
    {
        if (!estaNoChao)
        {
            noAr = true;
        }

        if(estaNoChao && noAr)
        {
            noAr = false;
            audioPulo.clip = audiosPulo[0];
            audioPulo.Play();
        }
    }

    void Verificacoes()
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


        velocidadeCai.y += gravidade * Time.deltaTime;
        controle.Move(velocidadeCai * Time.deltaTime);
    }

    void MovimentoAbaixa()
    {
        controle.center = Vector3.down * (alturaLevantado - controle.height) / 2f;

        if(estaAbaixado)
        {
            controle.height = Mathf.Lerp(controle.height, alturaAbaixado, Time.deltaTime * 3);
            float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaoCameraAbaixado, ref velocidadeCorrente, Time.deltaTime * 3);
            cameraTransform.localPosition = new Vector3(0, novoY, 0);
            velocidade = 3f;
            checarBloqueioAbaixado();
        }
        else
        {
            controle.height = Mathf.Lerp(controle.height, alturaLevantado, Time.deltaTime * 3);
            float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaoCameraEmPe, ref velocidadeCorrente, Time.deltaTime * 3);
            cameraTransform.localPosition = new Vector3(0, novoY, 0);
            velocidade = 6f;
        }
    }

    void Inputs()
    {
        if (Input.GetKey(KeyCode.LeftShift) && estaNoChao && !estaAbaixado && !cansado)
        {
            estaCorrendo = true;
            velocidade = 9;
            stamina -= 0.1f;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }
        else
        {
            estaCorrendo = false;
            stamina += 0.1f;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            audioPulo.clip = audiosPulo[0];
            audioPulo.Play();
            if (movimentoCabecaNew != null)
                movimentoCabecaNew.PararPassos();
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

    void CondicaoPlayer()
    {
        if(stamina == 0)
        {
            cansado = true;
            scriptRespiracao.forcaResp = 5;
        }

        if(stamina > 20)
        {
            cansado = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("cabecaDesliza"))
        {
            controle.SimpleMove(transform.forward * 1000 * Time.deltaTime);
        }
    }
}