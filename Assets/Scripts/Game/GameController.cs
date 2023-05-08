using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public float spawnInterval;
    public float spawnIntervalDecayRate;
    public int killsToWin;

    public GameObject Player;
    public GameObject EnemyPrefab;
    public int currentLevel;

    public void WinGame() 
    {
        SceneManager.LoadScene("Victory");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy() {
        if (Player != null) 
        { 
            Vector2 random = Random.insideUnitCircle * 50;
            Vector3 location = new Vector3(Player.transform.position.x + random.x, Player.transform.position.y, Player.transform.position.z + random.y);
            Instantiate(EnemyPrefab, location, Quaternion.identity);
            spawnInterval = Mathf.Max(1.5f, spawnInterval*spawnIntervalDecayRate);
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(SpawnEnemy());
        }
    }
}
