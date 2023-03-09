using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;
    [SerializeField] private TextMeshProUGUI health_txt;
    [SerializeField] private Image healthbar;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else {
            Destroy (gameObject);
        }
    }

    private void Start() {
        setHealthDisplay();
    }

    private void setHealthDisplay(){
        //health_txt.text = health.ToString();
    }

    public void UpdateHealthBar(float healthPercentage) {
        healthbar.fillAmount = healthPercentage;
    }

}
