using UnityEngine;
using BASA;

public class JogaPedra : MonoBehaviour
{

    Rigidbody rigid;
    public float hVelocidade = 15;
    public float vVelocidade = 4;
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Destroy(this.gameObject, 8);
    }

    public void Joga()
    {
        rigid = GetComponent<Rigidbody>();
        Vector3 targetForca = transform.forward * hVelocidade;
        targetForca += transform.up * vVelocidade;
        rigid.AddForce(targetForca, ForceMode.Impulse);

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<MovimentacaoPersonagem>().hp -= 30;
        }

        Destroy(this.gameObject);
    }
}
