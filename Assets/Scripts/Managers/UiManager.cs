using UnityEngine;
using UnityEngine.UI;

namespace BASA
{
    public class UiManager : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public MovimentacaoPersonagem scriptMovimenta;
        public Text municao;
        public Image imagemModotiro;
        public Sprite[] spriteModoTiro;
        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentacaoPersonagem>();
            municao.enabled = true;
            imagemModotiro.enabled = true;
        }

        void Update()
        {
            sliderHP.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
        }
    }
}

