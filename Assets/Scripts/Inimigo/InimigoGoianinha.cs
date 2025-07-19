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
    Ragdoll ragscript;

    public GameObject objDesliza;
    public bool estaMorto;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragscript = GetComponent<Ragdoll>();
        objDesliza = GameObject.FindWithTag("cabecaDesliza");

        ragscript.DesativaRagdoll();
        estaMorto = false;
    }

    void Update()
    {
        if(!estaMorto)
        {
            if (estaMorto) return;

            distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

            VaiAtrasJogador();
            OlhaParaPlayer();

            if (hp <= 0 && !estaMorto)
            {
                Morre();
            }
        }
       
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        if (distanciaPlayer < distanciaDoAtaque)
        {
            navMesh.isStopped = true;
            anim.SetTrigger("ataca");
            anim.SetBool("podeAndar", false);
            anim.SetBool("paraAtaque", false);
            CorrigiRigEntra();
        }

        if (distanciaPlayer >= 3)
        {
            anim.SetBool("paraAtaque", true);
        }

        if (anim.GetBool("podeAndar"))
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

    void CorrigiRigEntra()
    {
        ragscript.rigid.linearVelocity = Vector3.zero;
        ragscript.rigid.angularVelocity = Vector3.zero;
        ragscript.rigid.isKinematic = true;
    }

    void CorrigiRigSai()
    {
        ragscript.rigid.isKinematic = false;
    }

    public void LevouDano(int dano)
    {
        if (estaMorto) return;

        hp -= dano;
        if (hp <= 0)
        {
            Morre();
        }
        else
        {
            ParaDeAndar();
        }
    }

    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
        CorrigiRigEntra();
    }

    void Morre()
    {
        objDesliza.SetActive(false);
        estaMorto = true;

        navMesh.isStopped = true;
        anim.enabled = false;

        navMesh.enabled = false;
        ragscript.AtivaRagdoll();
    }

    public void DaDano()
    {
        player.GetComponent<MovimentacaoPersonagem>().hp -= 10;
    }
}
