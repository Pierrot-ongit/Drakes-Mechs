using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpriteRendererMaterialHandler : MonoBehaviour
{

    [SerializeField] private GameObject parentGameObject;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    // Start is called before the first frame update
    protected void Start()
    {
        foreach (Transform child in parentGameObject.transform)
        {
            SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
            if (sp != null)
            {
                spriteRenderers.Add(sp);
            }
        }
    }

    protected void setMaterial(Material material)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material = material;
        }
    }

    protected void setMaterialFloatValue(string propertyName, float value)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetFloat(propertyName, value);
        }
    }

    protected void setMaterialColor(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.color = color;
        }
    }

}
