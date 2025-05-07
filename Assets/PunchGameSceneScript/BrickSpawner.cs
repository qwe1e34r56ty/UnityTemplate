using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] GameObject brick;
    private Vector2[] spawnPosition = new Vector2[2];
    private float minSpawnDelay = 1.5f;
    private float maxSpawnDelay = 2f;
    private Vector2[] spawnVelocity = new Vector2[2];
    private float nextSpawnTime;
    private PunchGameManager punchGameManager;
    // Start is called before the first frame update
    private void Awake()
    {
        spawnPosition[0] = new Vector2(0.8f, 5);
        spawnPosition[1] = new Vector2(9, 4);
        spawnVelocity[0] = new Vector2(0, -5);
        spawnVelocity[1] = new Vector2(-20, 0);
    }
    void Start()
    {
        punchGameManager = PunchGameManager.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (punchGameManager.IsPlaying())
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnBrick();
                SetNextSpawnTime();
            }
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + 
            Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    void SpawnBrick()
    {
        int pattern = Random.Range(0, 2);
        GameObject spawnBrick = Instantiate(brick,
            spawnPosition[pattern],
            Quaternion.identity);
        Rigidbody2D rigidbody2D = spawnBrick.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = spawnVelocity[pattern];
    }
}
