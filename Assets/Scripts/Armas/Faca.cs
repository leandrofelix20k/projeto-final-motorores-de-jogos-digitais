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
        if(Input.GetButtonDown("Fire1"))
        {
            anim.Play("AtiraFaca");
        }
    }

    public void jogaFaca()
    {
        GameObject faca = Instantiate(facaJoga, transform);
        faca.transform.parent = null;
    }
}
