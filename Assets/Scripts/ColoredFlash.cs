using System.Collections;
using UnityEngine;


// INHERITANCE
public class ColoredFlash : MultipleSpriteRendererMaterialHandler
{

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;
    // The material that was in use, when the script started.
    [SerializeField] private Material originalMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;


    // The currently running coroutine.
    private Coroutine flashRoutine;

    private Color damageColor = Color.red;
    private Color healColor = Color.green;

    // POLYMORPHISM
    new void Start()
    {
        // Copy the flashMaterial material, this is needed, 
        // so it can be modified without any side effects.

        flashMaterial = new Material(flashMaterial);
        base.Start();
    }



    public void Flash(string damageType = null)
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }
        Color color;
        switch (damageType)
        {
            default:
            case "damage":
                color = damageColor;
                break;
            case "heal":
                color = healColor;
                break;
        }


        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        
        setMaterial(flashMaterial);
        setMaterialColor(color);
        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);
        setMaterial(originalMaterial);
        // Set the flashRoutine to null, signaling that it's finished.
        flashRoutine = null;
    }


}
