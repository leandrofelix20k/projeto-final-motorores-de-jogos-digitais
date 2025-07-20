using UnityEngine;

public class Faca : MonoBehaviour
{
    Animator anim;
    public GameObject facaJoga;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("Fire1"))
        {
            anim.Play("AtiraFaca");
        }
    }

    public void JogaFaca()
    {
        GameObject faca = Instantiate(facaJoga, transform);
        faca.transform.parent = null;
    }
}
