using System.Collections;
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
    public CapsuleCollider capsuleCollider;
    public GameObject cabecaDesliza;
    public AudioSource audios;
    public AudioClip[] sons;

    public bool usaCurvaAnimacao;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        estaMorto = false;
        usaCurvaAnimacao = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
        audios = GetComponent<AudioSource>();
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
                audios.volume = 1f;
                audios.clip = sons[0];
                audios.Play();
                cabecaDesliza.SetActive(false);
                estaMorto = true;
                navMesh.isStopped = true;
                navMesh.enabled = false;
                CorrigiRigEntra();
                anim.applyRootMotion = true;
                anim.CrossFade("Zombie Death", 0.2f);
                transform.gameObject.layer = 7; 
                capsuleCollider.direction = 2;
                usaCurvaAnimacao = false;
                GetComponent<DropItem>().Dropa();
                pedraPermanente.SetActive(false);
                StartCoroutine(SomeMorto());
            }

            if(usaCurvaAnimacao && !anim.IsInTransition(0))
            {
                capsuleCollider.height = anim.GetFloat("alturaCollider");
                capsuleCollider.center = new Vector3(0, anim.GetFloat("centroColliderY"), 0);
            }

            else
            {
                capsuleCollider.height = 2.1f;
                capsuleCollider.center = new Vector3(0, 1, 0);
            }
                
        }
    }

    IEnumerator SomeMorto()
    {
        yield return new WaitForSeconds(10);
        capsuleCollider.enabled = false;
        rigid.isKinematic = false;
        anim.enabled = false;
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    public void InstanciaPedra()
    {
        if (!estaMorto)
        {
            pedraPermanente.SetActive(false);
            GameObject pedra = Instantiate(pedraInstancia, pontoDeArremesso.position, pontoDeArremesso.rotation);
            pedra.transform.parent = null;
            pedra.transform.LookAt(player.transform.position);
            JogaPedra jogaScript = pedra.GetComponent<JogaPedra>();
            jogaScript.Joga();
        }
    }


    public void AparecePedraPermanente()
    {
        if (!estaMorto)
        {
            pedraPermanente.SetActive(true);
        }
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
            usaCurvaAnimacao = true;
        }
        else
        {
            pedraPermanente.SetActive(false);
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

    public void LevouDano(int dano)
    {
        hp -= dano;
    }

    public void SomPassos()
    {
        audios.volume = 5f;
        audios.PlayOneShot(sons[1]);
    }
}
