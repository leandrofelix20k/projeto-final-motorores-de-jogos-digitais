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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = 0;
        tempo = 0;
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UiManager>();

    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            itens[index].SetActive(false);

            index--;

            if(index < 0)
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
