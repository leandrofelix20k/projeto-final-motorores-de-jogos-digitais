using UnityEngine;
using UnityEngine.UI;

namespace BASA
{
    public class UiManager : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public MovimentacaoPersonagem scriptMovimenta;
        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentacaoPersonagem>();
        }

        void Update()
        {
            sliderHP.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
        }
    }
}

