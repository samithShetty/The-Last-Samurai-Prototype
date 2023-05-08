using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;
    [SerializeField] private Image healthbar;
    [SerializeField] private Image speedbar;

    private void Start() {
    }

    public void UpdateHealthBar(float healthPercentage) {
        healthbar.fillAmount = healthPercentage;
    }

    public void UpdateSpeedBar(float speedPercentage){
        speedbar.fillAmount = speedPercentage;
    }

}
