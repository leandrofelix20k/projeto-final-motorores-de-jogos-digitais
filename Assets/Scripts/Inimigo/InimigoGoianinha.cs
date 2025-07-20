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
    public bool bravo;
    public Renderer[] renderers;
    public bool invencivel;

    public AudioClip[] sonsMonstro;
    public AudioSource audioMonstro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragscript = GetComponent<Ragdoll>();
        renderers = GetComponentsInChildren<Renderer>();
        audioMonstro = GetComponentInChildren<AudioSource>();

        ragscript.DesativaRagdoll();
        estaMorto = false;
        invencivel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!estaMorto)
        {
            distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

            VaiAtrasJogador();
            OlhaParaPlayer();

            if(hp <= 50 && !bravo)
            {
                bravo = true;
                anim.ResetTrigger("levouTiro");
                ParaDeAndar();
                anim.CrossFade("ZombieScrean", 0.2f);
                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        mat.color = Color.red;
                    }
                }
                velocidade = 9;
            }

            if (hp <= 0 && !estaMorto)
            {
                Morrer();
                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        mat.color = Color.white;
                    }
                }
                objDesliza.SetActive(false);
                estaMorto = true;
                ParaDeAndar();
                navMesh.enabled = false;
                ragscript.AtivaRagdoll();
                StartCoroutine(ragscript.SomeMorto());
            }
        }
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
        ragscript.rigid.isKinematic = true;
        ragscript.rigid.linearVelocity = Vector3.zero;
    }

    void CorrigiRigSai()
    {
        ragscript.rigid.isKinematic = false;
    }

    public void LevouDano(int dano)
    {
        int n;
        n = Random.Range(0, 10);

        if(n % 2 == 0 && !bravo)
        {
            anim.SetTrigger("levouTiro");
            ParaDeAndar();
        }

        if (!invencivel)
        {
            hp -= dano;
        }
    }

    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        anim.SetBool("podeAndar", false);
        CorrigiRigEntra();
    }

    public void daDano()
    {
        player.GetComponent<MovimentacaoPersonagem>().hp -= 13;
    }

    public void FicaInvencivel()
    {
        invencivel = true;
    }

    public void SaiInvencivel()
    {
        invencivel = false;
        anim.speed = 2;
    }

    public void PassoMonstro()
    {
        audioMonstro.volume = 0.5f;
        audioMonstro.PlayOneShot(sonsMonstro[0]);
    }

    public void SenteDor()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[1];
        audioMonstro.Play();
    }

    public void Grita()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[2];
        audioMonstro.Play();
    }

    public void Morrer()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[3];
        audioMonstro.Play();
    }
}
