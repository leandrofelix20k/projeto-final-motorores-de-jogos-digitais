using UnityEngine;

namespace BASA
{
    [RequireComponent(typeof(RayCastScript))]

    public class AcoesRayCast : MonoBehaviour
    {
        RayCastScript rayCastScript;
        public bool pegou;
        float distancia;
        public GameObject salvarObjeto;

        void Start()
        {
            rayCastScript = GetComponent<RayCastScript>();
            pegou = false;
        }


        void Update()
        {
            distancia = rayCastScript.distanciaAlvo;

            if(distancia <= 3)
            {
                if(Input.GetKeyDown(KeyCode.E) && rayCastScript.objPega != null)
                {
                    Pegar();
                }

                if (Input.GetKeyDown(KeyCode.E) && rayCastScript.obejArrasta != null)
                {
                    if(!pegou)
                    {
                        pegou = true;
                        Arrastar();
                    }
                    else
                    {
                        pegou= false;
                        Soltar();
                    }
                }
            }
        }

        void Arrastar()
        {
            rayCastScript.obejArrasta.GetComponent<Rigidbody>().isKinematic = true;
            rayCastScript.obejArrasta.GetComponent <Rigidbody>().useGravity = false;
            rayCastScript.obejArrasta.transform.SetParent(transform);
            rayCastScript.obejArrasta.transform.localPosition = new Vector3(0, 0, 1.5f);
            rayCastScript.obejArrasta.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        void Soltar()
        {
            rayCastScript.obejArrasta.transform.localPosition = new Vector3(0, 0, 1.5f);
            rayCastScript.obejArrasta.transform.SetParent(null);
            rayCastScript.obejArrasta.GetComponent<Rigidbody>().isKinematic = false;
            rayCastScript.obejArrasta.GetComponent<Rigidbody>().useGravity = true;
        }

        void Pegar()
        {
            Destroy(rayCastScript.objPega);
        }
    }
}
