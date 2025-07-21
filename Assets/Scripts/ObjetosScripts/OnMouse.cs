using UnityEngine;

public class OnMouse : MonoBehaviour
{
    public Material selecionado, naoSelecionado;
    private Renderer objectRenderer;  // Renomeado para evitar conflito com a propriedade herdada

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer não encontrado no GameObject " + gameObject.name);
        }
    }

    private void OnMouseEnter()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = selecionado;
        }
    }

    void OnMouseExit()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = naoSelecionado;
        }
    }
}
