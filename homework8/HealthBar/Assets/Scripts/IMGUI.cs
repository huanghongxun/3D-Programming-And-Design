using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour
{
    public EntityHealth entityHealth;
    private float displayHealth;

    void Start()
    {
        displayHealth = entityHealth.health;
    }

    void OnGUI()
    {
        displayHealth = Mathf.Lerp(displayHealth, entityHealth.health, 0.05f);

        GUI.HorizontalScrollbar(new Rect(50, 50, 200, 20), 0.0f, displayHealth / entityHealth.maxHealth, 0, 1);
    }
}
