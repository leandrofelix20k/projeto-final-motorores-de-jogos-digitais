using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        indexPassos = 0;
    }

    // Update is called once per frame
    void Update()
    {
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

        if(cortaOnda != 0)
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
    }

    void SomPassos()
    {
        if (cortaOnda <= -0.95f && !audioSource.isPlaying)
        {
            audioSource.clip = audioClip[indexPassos];
            audioSource.Play();
            indexPassos++;
            if(indexPassos >= 4)
            {
                indexPassos = 0;
            }
        }
    }
}
