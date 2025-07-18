using UnityEngine;
using UnityEngine.AI;

public class InimigoGoianinha : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaDoAtaque;
    public float distanciaPlayer;
    public float velocidade = 5;
    Animator anim;
    public int hp = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

        VaiAtrasJogador();
        OlhaParaPlayer();
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        if(distanciaPlayer < distanciaDoAtaque)
        {
            navMesh.isStopped = true;
            anim.SetTrigger("ataca");
            anim.SetBool("podeAndar", false);
            anim.SetBool("paraAtaque", false);
            CorrigiRigEntra();
        }
        if(distanciaPlayer >= 3)
        {
            anim.SetBool("paraAtaque", true);
        }
        if(anim.GetBool("podeAndar"))
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("ataca");
            CorrigiRigSai();
        }
    }

    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
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
        navMesh.isStopped = true;
        anim.SetBool("podeAndar", false);
    }

    void CorrigiRigSai()
    {
        navMesh.isStopped = false;
        anim.SetBool("podeAndar", true);
    }

    public void LevouDano(int dano)
    {
        hp -= dano;
    }
}
