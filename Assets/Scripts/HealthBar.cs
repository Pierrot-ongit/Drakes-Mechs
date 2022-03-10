using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    private Transform bar;
    private float maxScale = 1f;
    private float minScale = 0f;

	private void Start () {
        bar = transform.Find("Bar");
	}

    public void SetSize(float sizeNormalized) {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor(Color color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }

    public void SetHealth(float health)
    {
        float size = Mathf.Clamp((health / 100), minScale, maxScale);
        SetSize(size);
    }

    public void SetMaxHealth()
    {
        SetSize(maxScale);
    }

}
