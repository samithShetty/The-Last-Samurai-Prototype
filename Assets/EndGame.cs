using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(endGame());   
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0); //Load Start Menu
    }
}
