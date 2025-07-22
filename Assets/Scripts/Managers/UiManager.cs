using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BASA
{
    public class UiManager : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public MovimentacaoPersonagem scriptMovimenta;
        public Text municao;
        public Image imagemModotiro;
        public Sprite[] spriteModoTiro;
        public RectTransform mira;
        public Sprite[] spriteItens;
        public Image imagem;
        public Image imgMachuca;
        public Text txtFrase;
        public Button[] botoes;
        float tempo;

        public bool fimjogo;
        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentacaoPersonagem>();
            municao.enabled = true;
            imagemModotiro.enabled = true;
            fimjogo = false;
        }

        void Update()
        {
            sliderHP.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;

            if(scriptMovimenta.hp <= 0 && !fimjogo)
            {
                tempo = 0;
                txtFrase.text = "Game Over!";
                StartCoroutine(FimDoJogo());
            }
            else if(Missao.bossMorto && !fimjogo)
            {
                tempo = 5;
                txtFrase.text = "Voc� Ganhou!";
                StartCoroutine(FimDoJogo());
            }
        }

        IEnumerator FimDoJogo()
        {
            yield return new WaitForSeconds(tempo);
            fimjogo = true;
            imgMachuca.GetComponent<Animator>().Play("FimJogo");
            Time.timeScale = 0;
            Camera.main.gameObject.GetComponent<AudioListener>().enabled = false;
            for (int i = 0; i < botoes.Length; i++)
            {
                botoes[i].gameObject.SetActive(true);
            }

            txtFrase.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ReiniciaJogo()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }

        public void SairJogo()
        {
            Application.Quit();
        }
    }
}

