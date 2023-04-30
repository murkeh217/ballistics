using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject panel;
    public Button menu;
    public TMP_InputField waitTime;
    public Slider mySlider;
    public TMP_Text speed;
    
    public void OpenMenu()
    {
        panel.SetActive(true);
        menu.enabled = false;
    }

    public void CloseMenu()
    {
        panel.SetActive(false);
        menu.enabled = true;
    }

    public void ChangeText()
    {
        TurretFunction.fireInterval = Single.Parse(waitTime.text);
    }
    
    void Update() {
        Projectile.speed = Single.Parse("Speed: " + mySlider.value);
    }
}