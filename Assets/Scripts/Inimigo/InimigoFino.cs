using UnityEngine;
using UnityEngine.AI;

public class InimigoLinha : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaDoAtaque;
    public float distanciaPlayer;
    public float velocidade = 5;
    Animator anim;
    public int hp = 100;
    public bool estaMorto;

    public Rigidbody rigid;

    public GameObject pedraPermanente;
    public Transform pontoDeArremesso;
    public GameObject pedraInstancia;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        estaMorto = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!estaMorto)
        {
            distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);
            VaiAtrasJogador();
            OlhaParaPlayer();

            if (hp <= 0)
            {
                estaMorto = true;
                navMesh.isStopped = true;
                navMesh.enabled = false;
                CorrigiRigEntra();
            }
        }
    }

    public void InstanciaPedra()
    {
        pedraPermanente.SetActive(false);
        GameObject pedra = Instantiate(pedraInstancia, pontoDeArremesso.position, pontoDeArremesso.rotation);
        pedra.transform.parent = null;
        pedra.transform.LookAt(player.transform.position);
        JogaPedra jogaScript = pedra.GetComponent<JogaPedra>();
        jogaScript.Joga();
    }


    public void AparecePedraPermanente()
    {
        pedraPermanente.SetActive(true);

    }
    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        if (distanciaPlayer < distanciaDoAtaque)
        {
            navMesh.isStopped = true;
            anim.SetBool("joga", true);
            CorrigiRigEntra();
        }
        else
        {
            anim.SetBool("joga", false);
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            CorrigiRigSai();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CorrigiRigEntra();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CorrigiRigSai();
        }
    }
    
    void CorrigiRigEntra()
    {
        rigid.isKinematic = true;
        rigid.linearVelocity = Vector3.zero;
    }

    void CorrigiRigSai()
    {
        rigid.isKinematic = false;
    }
}
