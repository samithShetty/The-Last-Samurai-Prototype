using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spawnInterval;
    public float spawnIntervalDecayRate;
    public int killsToWin;

    public GameObject Player;
    public GameObject EnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy() {
        Vector2 random = Random.insideUnitCircle * 50;
        Vector3 location = new Vector3(Player.transform.position.x + random.x, Player.transform.position.y, Player.transform.position.z + random.y);
        Instantiate(EnemyPrefab, location, Quaternion.identity);
        spawnInterval = Mathf.Max(1f, spawnInterval*spawnIntervalDecayRate);
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnEnemy());
    }
}
