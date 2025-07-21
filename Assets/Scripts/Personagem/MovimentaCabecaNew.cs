using UnityEngine;

public class MovimentaCabecaNew : MonoBehaviour
{
    public Transform mao;
    MovimentacaoPersonagem scriptPersonagem;

    Vector3 PosicaoMaoOrigem;
    Vector3 posicaoMao;
    float paradoMao, andandoMao;

    Vector3 posicaoCabecaOrigem;
    Vector3 posicaoCabeca;
    float andandoCabeca;

    float itensidade;

    public AudioClip[] passos;
    AudioSource audioSource;
    int indexPassos;

    private bool pulando = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scriptPersonagem = GetComponentInParent<MovimentacaoPersonagem>();
        audioSource = GetComponent<AudioSource>();
        indexPassos = 0;
        PosicaoMaoOrigem = mao.localPosition;
        posicaoCabecaOrigem = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulando)
        {
            if (scriptPersonagem != null && scriptPersonagem.estaNoChao)
            {
                pulando = false;
            }
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            CalculaMovimento(paradoMao, 0.01f, 0.01f, true);
            paradoMao += Time.deltaTime;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 2);

            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, Time.deltaTime * 100);
        } else if (scriptPersonagem.estaCorrendo)
        {
            CalculaMovimento(andandoMao, 0.06f, 0.06f, true);
            andandoMao += Time.deltaTime * 4;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 6);

            CalculaMovimento(andandoCabeca, 0, 0.2f, false);
            andandoCabeca += Time.deltaTime * 15;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, posicaoCabeca, Time.deltaTime * 10);
        }
        else
        {
            CalculaMovimento(andandoMao, 0.03f, 0.03f, true);
            andandoMao += Time.deltaTime * 2;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 6);


            CalculaMovimento(andandoCabeca, 0, 0.15f, false);
            andandoCabeca += Time.deltaTime * 10;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, posicaoCabeca, Time.deltaTime * 6);
        }

        if (andandoCabeca > Mathf.PI * 2)
        {
            andandoCabeca = andandoCabeca - (Mathf.PI * 2);
        }
    }

    void CalculaMovimento(float valorTempo, float intesidadeX, float intensidadeY, bool mao)
    {
        if (mao)
        {
            posicaoMao = PosicaoMaoOrigem + new Vector3(Mathf.Cos(valorTempo) * intesidadeX, Mathf.Sin(valorTempo * 2) * intensidadeY, 0);
        }
        else
        {
            posicaoCabeca = posicaoCabecaOrigem + new Vector3(0, Mathf.Sin(valorTempo) * intensidadeY, 0);
            if(Mathf.Sin(valorTempo) < -0.95)
            {
                SomPassos();
            }
        }
    }
    void SomPassos()
    {
        if (!audioSource.isPlaying && scriptPersonagem.estaNoChao)
        {
            audioSource.clip = passos[indexPassos];
            audioSource.Play();
            indexPassos = (indexPassos + 1) % passos.Length;
        }
    }
    public void PararPassos()
    {
        pulando = true;
        audioSource.Stop();
    }
}
