using System.Collections;
using UnityEngine;

public class Pistola : MonoBehaviour
{
    Animator animator;
    bool estaAtirando;
    RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        estaAtirando = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!estaAtirando)
            {
                estaAtirando = true;
                StartCoroutine(Atirando());
            }
        }
    }

    IEnumerator Atirando()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        animator.Play("Atira");

        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            if(hit.transform.tag == "objArrasta")
            {
                Vector3 direcaoBala = ray.direction;

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
                }
            }
        }

        yield return new WaitForSeconds(0.3f); 
        estaAtirando = false;
    }
}
