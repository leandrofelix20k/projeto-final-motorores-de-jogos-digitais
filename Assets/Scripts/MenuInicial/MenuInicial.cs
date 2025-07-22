using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInicial : MonoBehaviour
{
    
    public void Jogar()
    {
        SceneManager.LoadScene(1); 
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Jogo fechado"); 
    }
}
