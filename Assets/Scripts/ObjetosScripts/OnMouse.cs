using UnityEngine;

public class OnMouse : MonoBehaviour
{
    public Material selecionado, naoSelecionado;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    
    void onMouseEnter()
    {
        renderer.material = selecionado;
    }

    void OnMouseExit()
    {
        renderer.material = naoSelecionado;
    }
}
