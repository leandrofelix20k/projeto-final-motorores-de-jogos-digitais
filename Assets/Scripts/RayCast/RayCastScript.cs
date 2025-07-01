using UnityEngine;
using UnityEngine.UI;

namespace BASA
{
    public class RayCastScript : MonoBehaviour
    {
        public float distanciaAlvo;
        public GameObject obejArrasta, objPega;
        RaycastHit hit;
        public Text textButton, textInfo;

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
                        objPega = null;
                        textButton.text = "[E]";
                        textInfo.text = "Agarra/Solta";
                    }

                    if (hit.transform.gameObject.tag == "objPega")
                    {
                        objPega = hit.transform.gameObject;
                        obejArrasta = null;
                        textButton.text = "[E]";
                        textInfo.text = "Pegar";
                    }
                }
                else
                {
                    textButton.text = "";
                    textInfo.text = "";
                    obejArrasta = null;
                    objPega = null;
                }
            }
        }
    }
}
