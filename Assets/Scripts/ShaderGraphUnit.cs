using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderGraphUnit : MonoBehaviour
{

    [SerializeField] private GameObject parentGameObject;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    // The material that was in use, when the script started.
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveSpeed = 0.5f;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

   
    
    bool isDissolving = false;
    bool isRestoring = false;
    float fade = 1f;


    // The currently running coroutine.
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in parentGameObject.transform)
        {
            SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
            if( sp != null)
            {
                spriteRenderers.Add(sp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            isDissolving = true;
            isRestoring = false;
            setMaterial(dissolveMaterial);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            isDissolving = false;
            isRestoring = true;
            setMaterial(dissolveMaterial);
        }
        */

        if (isDissolving)
        {
            fade = Mathf.Clamp01(fade - Time.deltaTime);
            setMaterialFloatValue("_Fade", fade);
        }
        if (isRestoring && !isDissolving)
        {
            fade = Mathf.Clamp01(fade + Time.deltaTime);
            setMaterialFloatValue("_Fade", fade);
        }
    }

    private void setMaterial(Material material)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Swap to the flashMaterial.
            spriteRenderer.material = material;
        }
    }

    private void setMaterialFloatValue(string propertyName, float value)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Swap to the flashMaterial.
            spriteRenderer.material.SetFloat(propertyName, value);
        }
    }


    public void startDissolving()
    {
        isDissolving = true;
        isRestoring = false;
        setMaterial(dissolveMaterial);
    }

    public void stopDissolving()
    {
        isDissolving = false;
        setMaterial(dissolveMaterial);
    }

    public void startRestoring()
    {
        isDissolving = true;
        isRestoring = false;
        setMaterial(dissolveMaterial);
    }


    private IEnumerator FlashRoutine(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Swap to the flashMaterial.
            spriteRenderer.material = flashMaterial;

            // Set the desired color for the flash.
            flashMaterial.color = color;
        }
        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // After the pause, swap back to the original material.
            spriteRenderer.material = originalMaterial;
        }
        // Set the flashRoutine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
