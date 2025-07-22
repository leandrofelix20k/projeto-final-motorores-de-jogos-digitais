using System;
using UnityEngine;
using UnityEngine.InputSystem;
using BASA;


public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Configuracao Personagem")]
    public CharacterController controle;
    public float velocidade = 6f;
    public float velocidadeAbaixado = 3f; // Nova vari�vel para velocidade agachado
    public float alturaPulo = 3f;
    public float gravidade = -20f;
    public bool estaCorrendo;
    public AudioClip[] audiosGerais;
    AudioSource audioPersonagem;
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

    [Header("TravaPulo")]
    public Vector3 pontoContato;
    public bool podePular;
    RaycastHit hitContato;
    public float anguloLimitePulo;
    public float distanciaRaio;

    UiManager uiScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        audioPersonagem = GetComponent<AudioSource>();
        noAr = false;
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UiManager>();


        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        pontoContato = hit.point;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hitContato, distanciaRaio))
        {
            if (Vector3.Angle(hitContato.normal, Vector3.up) > anguloLimitePulo)
            {
                podePular = false;
            }
            else
            {
                podePular = true;
            }
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

        if (estaNoChao && noAr)
        {
            noAr = false;
            audioPersonagem.clip = audiosGerais[0];
            audioPersonagem.Play();
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

        if (estaAbaixado)
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

        if (Input.GetButtonDown("Jump") && estaNoChao && podePular)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            audioPersonagem.clip = audiosGerais[0];
            audioPersonagem.Play();
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
        if (stamina == 0)
        {
            cansado = true;
            scriptRespiracao.forcaResp = 5;
        }

        if (stamina > 20)
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

    public void LevouDano(int dano)
    {
        hp -= dano;
        audioPersonagem.clip = audiosGerais[2];
        audioPersonagem.Play();
        uiScript.imgMachuca.GetComponent<Animator>().Play("MachucaImg");
    }
}