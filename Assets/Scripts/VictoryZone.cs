using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    [SerializeField] private GameController gc;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            gc.WinGame();
        }
    }
}
