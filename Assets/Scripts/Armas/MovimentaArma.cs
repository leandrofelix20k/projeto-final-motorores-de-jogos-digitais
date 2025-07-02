using UnityEngine;

namespace BASA
{
    public class MovimentaArma : MonoBehaviour
    {
        public float valor;
        public float suavizarValor;
        public float valorMaximo;
        Vector3 posicaoInicial;

        void Start()
        {
            posicaoInicial =transform.localPosition;
        }

        void Update()
        {
            float movimentoX = -Input.GetAxis("Mouse X") * valor;
            float movimentoY = -Input.GetAxis("Mouse Y") * valor;

            movimentoX = Mathf.Clamp(movimentoX, -valorMaximo, valorMaximo);
            movimentoY = Mathf.Clamp(movimentoY, -valorMaximo, valorMaximo);

            Vector3 finalPosition = new Vector3(movimentoX, movimentoY, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + posicaoInicial, Time.deltaTime * suavizarValor);
        }
    }
}
