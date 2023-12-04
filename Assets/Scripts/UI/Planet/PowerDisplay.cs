using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    private float cooldown;

    public PowerDisplay setSprite(Sprite sprite)
    {
        image.sprite = sprite;
        return this;
    }

    public PowerDisplay setCooldown(float cooldown)
    {
        this.cooldown = cooldown;
        return this;
    }
    
}
