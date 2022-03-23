using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class ShaderGraphUnit : MultipleSpriteRendererMaterialHandler
{
    // The material that was in use, when the script started.
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveSpeed = 0.5f;
    
    bool isDissolving = false;
    bool isRestoring = false;
    float fade = 1f;


    void Update()
    {

        //DissolveControls();
        if (isDissolving)
        {
            fade = Mathf.Clamp01(fade - Time.deltaTime / 2);
            setMaterialFloatValue("_Fade", fade);
        }
        if (isRestoring && !isDissolving)
        {
            fade = Mathf.Clamp01(fade + Time.deltaTime / 2);
            setMaterialFloatValue("_Fade", fade);
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
        isDissolving = false;
        isRestoring = true;
        setMaterial(dissolveMaterial);
    }

    private void DissolveControls()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            startDissolving();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            startRestoring();
        }
    }

}
