using UnityEngine;
using System.Collections.Generic;


public class InimigosManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] pontos;
    public GameObject[] inimigos;
    public float tempo;
    public List<GameObject> listaInimigos = new List<GameObject>();
    void Start()
    {
        tempo = 0;
        CriaInimigos();
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;
        if (tempo > 60)
        {
            tempo = 0;

            for (int i = 0; i < listaInimigos.Count; i++)
            {
                if (listaInimigos[i] == null)
                {
                    listaInimigos.RemoveAt(i);
                }
            }

            if (listaInimigos.Count < 20)
            {
                CriaInimigos();
            }

        }
    }

    void CriaInimigos()
    {
        for (int i = 0; i < pontos.Length; i++)
        {
            int random = Random.Range(0, 10);
            if (random > 5)
            {
                GameObject inimigo = Instantiate(inimigos[0], pontos[i].transform.position, Quaternion.identity);
                listaInimigos.Add(inimigo);

            }
            else
            {
                GameObject inimigo = Instantiate(inimigos[1], pontos[i].transform.position, Quaternion.identity);
                listaInimigos.Add(inimigo);   
            }
        }
    }

}
