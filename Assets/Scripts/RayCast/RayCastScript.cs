using UnityEngine;

namespace BASA
{
    public class RayCastScript : MonoBehaviour
    {
        public float distanciaAlvo;
        public GameObject obejArrasta, objPega;
        RaycastHit hit;

        void Update()
        {
            if(Time.frameCount % 5 == 0)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);

                if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5))
                {
                    distanciaAlvo = hit.distance;
                    print(hit.transform.gameObject.name);

                    if (hit.transform.gameObject.tag == "objArrasta")
                    {
                         obejArrasta = hit.transform.gameObject;
                    }

                    if (hit.transform.gameObject.tag == "objPega")
                    {
                        objPega = hit.transform.gameObject;
                    }
                }
                else
                {
                    obejArrasta = null;
                    objPega = null;
                }
            }
        }
    }
}
