using UnityEngine;

public class FacaJoga : MonoBehaviour
{
    public float velocidade = 1000;
    public float forcaJoga = 20;
    Rigidbody rig;

    public GameObject sangue;
    public GameObject som;

    void Start()
    {
        Joga();
    }

    void Joga()
    {
        rig = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, 90 + Camera.main.transform.eulerAngles.y, 0);
        Vector3 direcao = Camera.main.transform.forward * forcaJoga + Camera.main.transform.up * 4 - (Camera.main.transform.right / 2.5f);
        rig.AddForce(direcao, ForceMode.Impulse);
    }

    void Update()
    {
        transform.eulerAngles += Vector3.forward * velocidade * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Inimigo"))
        {
            if(col.transform.gameObject.GetComponent<InimigoGoianinha>()) 
            {
                col.transform.gameObject.GetComponent<InimigoGoianinha>().LevouDano(10);
            }

            if (col.transform.gameObject.GetComponent<InimigoLinha>())
            {
                col.transform.gameObject.GetComponent<InimigoLinha>().LevouDano(10);
            }

            Instantiate(sangue, transform.position, Quaternion.Euler(0, 90, 0));
            GameObject somFaca = Instantiate(som, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(somFaca, 2);
        }

        // destruir o objeto faca após a colisão
        Destroy(this.gameObject);
    }
}
