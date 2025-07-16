using UnityEngine;
using BASA;

public class MovimentoCabeca : MonoBehaviour
{
    public AudioClip[] passos;
    private AudioSource audioSource;

    private Vector3 posicaoCabecaOrigem;
    private Vector3 posicaoCabeca;
    private float tempoCabeca;
    private int indexPassos;

    public float intensidadeAndando = 0.2f;
    public float intensidadeCorrendo = 0.15f;

    private float intensidadeAtual;
    private float multiplicadorVelocidade;

    private bool pulando = false;

    private MovimentacaoPersonagem scriptPersonagem;

    private float intensidadePadrao;
    private float multiplicadorPadrao;

    void Start()
    {
        scriptPersonagem = GetComponentInParent<MovimentacaoPersonagem>();
        audioSource = GetComponent<AudioSource>();
        indexPassos = 0;
        posicaoCabecaOrigem = transform.localPosition;

        intensidadePadrao = intensidadeAndando;
        multiplicadorPadrao = 10f;
    }

    void Update()
    {
        if (pulando)
        {
            if (scriptPersonagem != null && scriptPersonagem.estaNoChao)
                pulando = false;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        AtualizaIntensidade();

        bool estaParado = Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0;

        if (estaParado)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, posicaoCabecaOrigem, Time.deltaTime * 10);
        }
        else
        {
            float seno = Mathf.Sin(tempoCabeca);
            posicaoCabeca = posicaoCabecaOrigem + new Vector3(0, seno * intensidadeAtual, 0);
            tempoCabeca += Time.deltaTime * multiplicadorVelocidade;

            transform.localPosition = Vector3.Lerp(transform.localPosition, posicaoCabeca, Time.deltaTime * 6);
            SomPassos(seno);
        }
    }

    void SomPassos(float seno)
    {
        if (seno < -0.95f && !audioSource.isPlaying && scriptPersonagem.estaNoChao)
        {
            audioSource.clip = passos[indexPassos];
            audioSource.Play();
            indexPassos = (indexPassos + 1) % passos.Length;
        }
    }

    void AtualizaIntensidade()
    {
        if (scriptPersonagem.estaCorrendo)
        {
            intensidadeAtual = intensidadeCorrendo; 
            multiplicadorVelocidade = 15f;
        }
        else if (scriptPersonagem.estaAbaixado)
        {
            intensidadeAtual = 0.11f;
            multiplicadorVelocidade = 5f;
        }
        else
        {
            intensidadeAtual = intensidadeAndando;
            multiplicadorVelocidade = 10f;
        }
    }


    public void PararPassos()
    {
        pulando = true;
        audioSource.Stop();
    }

    public void ReduzirOscilacao(float fatorReducao)
    {
        intensidadeAtual *= fatorReducao;
        multiplicadorVelocidade *= fatorReducao;
    }

    public void RestaurarOscilacao()
    {
        intensidadeAtual = intensidadePadrao;
        multiplicadorVelocidade = multiplicadorPadrao;
    }
}
