using UnityEngine;
using BASA;

public class MovimentoCabeca : MonoBehaviour
{
    private float tempo = 0.0f;
    public float velocidade = 0.05f;
    public float forca = 0.1f;
    public float pontoOrigem = 0.0f;

    float cortaOnda;
    float horizontal;
    float vertical;
    Vector3 salvaPosicao;

    AudioSource audioSource;
    public AudioClip[] audioClip;
    public int indexPassos;

    MovimentacaoPersonagem scriptMovimenta;

    private float forcaNormal;
    private float velocidadeNormal;
    private bool pulando = false;
    private MovimentacaoPersonagem movPersonagem;

    void Start()
    {
        scriptMovimenta = GetComponentInParent<MovimentacaoPersonagem>();
        audioSource = GetComponent<AudioSource>();
        indexPassos = 0;
        forcaNormal = forca;
        velocidadeNormal = velocidade;
        movPersonagem = GetComponentInParent<MovimentacaoPersonagem>();
    }

    void Update()
    {
        if (pulando)
        {
            if (movPersonagem != null && movPersonagem.estaNoChao)
            {
                pulando = false;
            }
            return;
        }

        cortaOnda = 0.0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        salvaPosicao = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            tempo = 0.0f;
        }
        else
        {
            cortaOnda = Mathf.Sin(tempo);
            tempo = tempo + velocidade;

            if (tempo > Mathf.PI * 2)
            {
                tempo = tempo - (Mathf.PI * 2);
            }
        }

        if (cortaOnda != 0)
        {
            float mudaMovimentacao = cortaOnda * forca;
            float eixosTotais = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            eixosTotais = Mathf.Clamp(eixosTotais, 0.0f, 1.0f);
            mudaMovimentacao = eixosTotais * mudaMovimentacao;
            salvaPosicao.y = pontoOrigem + mudaMovimentacao;
        }
        else
        {
            salvaPosicao.y = pontoOrigem;
        }

        transform.localPosition = salvaPosicao;

        SomPassos();
        AtualizaCabeca();
    }

    void SomPassos()
    {
        if (pulando) return;

        if (cortaOnda <= -0.95f && !audioSource.isPlaying && scriptMovimenta.estaNoChao)
        {
            audioSource.clip = audioClip[indexPassos];
            audioSource.Play();
            indexPassos = (indexPassos + 1) % audioClip.Length;
        }
    }

    void AtualizaCabeca()
    {
        if (scriptMovimenta.estaCorrendo)
        {
            velocidade = 0.25f;
            forca = 0.25f;
        } 
        else if(scriptMovimenta.estaAbaixado)
        {
            velocidade = 0.15f;
            forca = 0.11f;
        }
        else
        {
            velocidade = 0.18f;
            forca = 0.15f;
        }
    }

    public void PararPassos()
    {
        pulando = true;
        audioSource.Stop();
    }

    public void ReduzirOscilacao(float fatorReducao)
    {
        forca = forcaNormal * fatorReducao;
        velocidade = velocidadeNormal * fatorReducao;
    }

    public void RestaurarOscilacao()
    {
        forca = forcaNormal;
        velocidade = velocidadeNormal;
    }
}