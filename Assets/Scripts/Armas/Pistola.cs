using System.Collections;
using UnityEngine;

public class Pistola : MonoBehaviour
{
    Animator animator;
    bool estaAtirando;
    RaycastHit hit;

    public GameObject faisca;
    public GameObject buraco;
    public GameObject fumaca;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;

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

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            InstanciaEfeitos();
            if (hit.transform.tag == "objArrasta")
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

    void InstanciaEfeitos()
    {
        Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        buracoObj.transform.parent = hit.transform;
    }
}
