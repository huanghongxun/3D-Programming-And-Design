using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public EntityHealth entityHealth;
    private float displayHealth;
    public Slider slider;

    void Start()
    {
        displayHealth = entityHealth.health;
    }

    void Update()
    {
        displayHealth = Mathf.Lerp(displayHealth, entityHealth.health, 0.05f);

        slider.value = displayHealth / entityHealth.maxHealth;
    }
}
