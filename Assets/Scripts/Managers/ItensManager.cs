using System.Collections.Generic;
using BASA;
using NUnit.Framework;
using UnityEngine;

public class ItensManager : MonoBehaviour
{
    public GameObject[] itens;
    public int index;
    UiManager uiScript;
    float tempo;

    public List<Animator> animiItens = new List<Animator>();
    public static ItensManager instance;

    public bool mira;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = 0;
        tempo = 0;
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UiManager>();

        for(int i=0; i<itens.Length; i++)
        {
            animiItens.Add(itens[i].GetComponent<Animator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mira)
        {
            MudaArma();
        }
    }

    void MudaArma()
    {
        tempo += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && !animiItens[index].GetBool("ocorreAcao"))
        {
            itens[index].SetActive(false);

            index--;

            if (index < 0)
            {
                index = itens.Length - 1;
            }

            itens[index].SetActive(true);
            uiScript.imagem.sprite = uiScript.spriteItens[index];
            tempo = 0;
        }

        if (index == 0 && tempo > 0.05f)
        {
            uiScript.municao.enabled = true;
            uiScript.imagemModotiro.enabled = true;
        }
        else
        {
            uiScript.municao.enabled = false;
            uiScript.imagemModotiro.enabled = false;
        }
    }
}
