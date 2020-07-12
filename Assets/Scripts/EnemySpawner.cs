using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple enemy spawner, similar to Blasty Boy setup
public class EnemySpawner : MonoBehaviour {
    public List<GameObject> enemyPrefabs;
    public List<GameObject> activeEnemies;
    public int numActiveEnemies, maxNumActiveEnemies;

    public int maxSpawnAttempts;
    public float minSpawnDelay;
    private float timeSinceLastSpawn;

    public List<Transform> spawnableVolumes;
    public Vector3 verticalSpawnOffset;
    public float groundCheckRaycastDistance;
    public string spawnableGroundString;

    public LayerMask groundLayermask;

    public float unregisterDelay;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (numActiveEnemies < maxNumActiveEnemies) {
            if (timeSinceLastSpawn > minSpawnDelay) {
                SpawnEnemy();
            } else {
                timeSinceLastSpawn += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy() {
        int spawnVolumeIndex;
        int spawnAttempt = 0;
        bool wasSpawnSuccessful = false;
        int enemyIndex;

        do {
            spawnAttempt++;

            // Pick a random spawn volume
            spawnVolumeIndex = Mathf.FloorToInt(Random.Range(0, spawnableVolumes.Count));

            // Find a random point in air, in this volume
            Vector3 spawnPosOffset;
            spawnPosOffset.x = Random.Range(-1 * GetSpawnDimension(0, spawnVolumeIndex), GetSpawnDimension(0, spawnVolumeIndex));
            spawnPosOffset.y = Random.Range(-1 * GetSpawnDimension(1, spawnVolumeIndex), GetSpawnDimension(1, spawnVolumeIndex));
            spawnPosOffset.z = Random.Range(-1 * GetSpawnDimension(2, spawnVolumeIndex), GetSpawnDimension(2, spawnVolumeIndex));

            Vector3 spawnPosition = spawnableVolumes[spawnVolumeIndex].position + spawnPosOffset;

            // Raycast from this point down to the ground. 
            // If we hit spawnable ground, then use that position
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundLayermask)) {
                // Confirm that we hit spawnable ground
                if (hit.collider.gameObject.CompareTag(spawnableGroundString)) {
                    Vector3 groundPosition = spawnPosition;
                    groundPosition.y = hit.point.y;

                    // Spawn the enemy
                    enemyIndex = Mathf.FloorToInt(Random.Range(0, enemyPrefabs.Count));
                    GameObject enemy = Instantiate(enemyPrefabs[enemyIndex],
                        groundPosition + verticalSpawnOffset,
                        Quaternion.Euler(0, 360, 0));
                    activeEnemies.Add(enemy);

                    // TODO: Get damageable 
                    Damageable damageable = enemy.GetComponent<Damageable>();
                    if (damageable) {
                        damageable.onHealthDepleted.AddListener(
                            delegate {
                                RegisterDeadEnemy(enemy);
                            }
                        );
                    } else {
                        Debug.LogError("No damageable component on the enemy!");
                    }

                    timeSinceLastSpawn = 0f;
                    numActiveEnemies++;
                    wasSpawnSuccessful = true;
                } else {
                    Debug.Log("Failed to spawn above ground");
                }
            } else {
                Debug.Log("Failed to raycast above anything ");
            }
        } while (!wasSpawnSuccessful && spawnAttempt < maxSpawnAttempts);
    }

    private float GetSpawnDimension(int dim, int volumeIndex) {
        Vector3 scale = spawnableVolumes[volumeIndex].localScale;
        switch (dim) {
            case 0: return scale.x / 2;
            case 1: return scale.y / 2;
            default: return scale.z / 2;
        }
    }

    public void RegisterDeadEnemy(GameObject enemy) {
        StartCoroutine(_RegisterDeadEnemy(enemy));
    }
    public IEnumerator _RegisterDeadEnemy(GameObject enemy) {
        yield return new WaitForSeconds(unregisterDelay);

        timeSinceLastSpawn = 0f;

        if (enemy) {
            numActiveEnemies--;
            activeEnemies.Remove(enemy);
            Destroy(enemy);
        } else {
            Debug.LogError("Enemy disappeared before we could destroy it!");
        }
    }
}
