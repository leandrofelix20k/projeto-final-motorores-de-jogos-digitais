using System.Collections;
using BASA;
using UnityEngine;

public class Pistola : MonoBehaviour
{
    Animator animator;
    bool estaAtirando;
    RaycastHit hit;

    public GameObject faisca;
    public GameObject buraco;
    public GameObject fumaca;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject particulaSangue;

    public ParticleSystem rastroBala;

    public AudioSource audioArma;
    public AudioClip[] sonsArma;


    public int carregador = 3;
    public int municao = 12;

    UiManager uiScript;
    public GameObject posUI;
    MovimentaArma movimentaArmaScript;

    public bool automatico;
    public float numeroAleatorioMira;

    public float valorMira;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        automatico = false;
        estaAtirando = false;
        animator = GetComponent<Animator>();
        audioArma = GetComponent<AudioSource>();
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UiManager>();
        movimentaArmaScript = GetComponentInParent<MovimentaArma>();
        valorMira = 280;
    }

    // Update is called once per frame
    void Update()
    {
        uiScript.municao.text = municao.ToString() + "/" + carregador.ToString();

        ModificaMira();

        if (animator.GetBool("ocorreAcao"))
        {
            return;
        }

        Atira();
        Recarrega();
        Mira();
    }

    void ModificaMira()
    {
        if (estaAtirando)
        {
            valorMira = Mathf.Lerp(valorMira, 420, Time.deltaTime * 20);
            uiScript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
        else
        {
            valorMira = Mathf.Lerp(valorMira, 280, Time.deltaTime * 20);
            uiScript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
    }

    void Atira()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!estaAtirando && municao > 0)
            {
                municao--;
                audioArma.clip = sonsArma[0];
                audioArma.Play();
                rastroBala.Play();
                estaAtirando = true;
                StartCoroutine(Atirando());
            }
            else if (!estaAtirando && municao == 0 && carregador > 0)
            {
                animator.Play("Recarrega");
                carregador--;
                municao = 12;
            }
            else if (municao == 0 && carregador == 0)
            {
                audioArma.clip = sonsArma[3];
                audioArma.Play();
            }
        }
    }

    void Recarrega()
    {
        if (Input.GetKeyDown(KeyCode.R) && carregador > 0 && municao < 12)
        {
            animator.Play("Recarrega");
            carregador--;
            municao = 12;
        }
    }

    void Mira()
    {
        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("Mira", true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
            uiScript.mira.gameObject.SetActive(false);
            movimentaArmaScript.valor = 0.01f;
            numeroAleatorioMira = 0;
        }
        else
        {
            animator.SetBool("Mira", false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
            uiScript.mira.gameObject.SetActive(true);
            movimentaArmaScript.valor = 0.1f;
            numeroAleatorioMira = 0.05f;
        }
    }

    IEnumerator Atirando()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        animator.Play("Atira");

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        if (Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-numeroAleatorioMira, numeroAleatorioMira), ray.origin.y + Random.Range(-numeroAleatorioMira, numeroAleatorioMira), ray.origin.z), Camera.main.transform.forward, out hit))
        {
            if(hit.transform.tag == "Inimigo"){
                hit.transform.GetComponent<InimigoGoianinha>().LevouDano(20);
                GameObject particulaCriada = Instantiate(particulaSangue, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                particulaCriada.transform.parent = hit.transform;
            }
            else
            {
                InstanciaEfeitos();
                if (hit.rigidbody != null)
                {
                    AdicionaForca(ray, 400);
                }
            }
        }

        yield return new WaitForSeconds(0.3f); 
        estaAtirando = false;
    }

    void InstanciaEfeitos()
    {
        Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        buracoObj.transform.parent = hit.transform;
    }

    void SomMagazine()
    {
        audioArma.clip = sonsArma[1];
        audioArma.Play();
    }

    void somUp()
    {
        audioArma.clip = sonsArma[2];
        audioArma.Play();
    }

    void AdicionaForca(Ray ray, float forca)
    {
        Vector3 direcaoBala = ray.direction;

        hit.rigidbody.AddForceAtPosition(direcaoBala * forca, hit.point);
    }
}
