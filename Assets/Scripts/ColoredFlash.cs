using System.Collections;
using UnityEngine;


public class ColoredFlash : MonoBehaviour
{

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;
    // The material that was in use, when the script started.
    [SerializeField] private Material originalMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    // The SpriteRenderer that should flash.
    [SerializeField] private SpriteRenderer[] spriteRenderers;


    // The currently running coroutine.
    private Coroutine flashRoutine;

   // const damageColor = "red";

    private Color damageColor = Color.red;
    private Color healColor = Color.green;


    void Start()
    {
        // Copy the flashMaterial material, this is needed, 
        // so it can be modified without any side effects.
        flashMaterial = new Material(flashMaterial);
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
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Swap to the flashMaterial.
            spriteRenderer.material = flashMaterial;

            // Set the desired color for the flash.
            flashMaterial.color = color;
        }
        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // After the pause, swap back to the original material.
            spriteRenderer.material = originalMaterial;
        }
        // Set the flashRoutine to null, signaling that it's finished.
        flashRoutine = null;
    }


}
