using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign in Inspector
    public Transform player;       // Assign player in Inspector
    public float minSpawnDistance = 10f; // Minimum spawn distance
    public float maxSpawnDistance = 20f; // Maximum spawn distance
    public float spawnInterval = 0.5f; // Time between spawns
    public int maxEnemyThreshold = 150;
    private int currentEnemyCount = 0;

    public float  CurrentHealth = 50f;
    public float currentDamage = 10f;
    public float CurrentXPAmount = 3f;
    public float CurrentLookRadius ;

    public float currentWalkSpeed = 2.5f;
    public float currentRunSpeed = 5f;
    GameObject Enemy;
    public float currentSpeedpercentage = 0f; //for the legendary speed-damge reduction upgrade


    //explosion

    public bool explosionOnDeath = false;
    public float explosionChance = 0f;
    public float explosionRadius = 0f;
    public float explosionDamage = 0f;
    public bool shouldSpawnEnemy
    {
        get { return _shouldSpawnEnemy; }
        set
        {
            if (_shouldSpawnEnemy == value) return; // Avoid redundant calls

            _shouldSpawnEnemy = value;

            if (_shouldSpawnEnemy)
            {
                StartCoroutine(SpawnEnemyRoutine()); // Restart the coroutine when enabled
            }
        }
    }
    private bool _shouldSpawnEnemy = true;


    public float regularscoremultiplier;
    private void OnEnable()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (shouldSpawnEnemy)
        {
            if(currentEnemyCount < maxEnemyThreshold)
                SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (player == null) return;

        Vector3 spawnPosition = GetValidSpawnPosition();

        if(spawnPosition == Vector3.zero){      //valid position not found, Spawning failed

            Debug.Log("spawn failed");
            return;

        }       

        Enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
        enemy SpawnedEnemy = Enemy.GetComponent<enemy>();
        SpawnedEnemy.health = CurrentHealth;
        SpawnedEnemy.XPamount = CurrentXPAmount;
        SpawnedEnemy.onDeath += DecreaseEnemyCount;
        SpawnedEnemy.explosionOnDeath = explosionOnDeath;
        SpawnedEnemy.explosionChance = explosionChance;
        SpawnedEnemy.explosionDamage = explosionDamage;
        SpawnedEnemy.explosionRadius = explosionRadius;
        SpawnedEnemy.regularscoremultiplier = regularscoremultiplier;

        enemyController controller = Enemy.GetComponent<enemyController>();
        controller.attackDamage   =currentDamage;
        controller.LookRadius = CurrentLookRadius;
        controller.walkspeed = currentWalkSpeed;
        controller.runspeed= currentRunSpeed;


    }

    void DecreaseEnemyCount(){
        currentEnemyCount--;
    }

   private Vector3 GetValidSpawnPosition()
    {
        int safetyCheck = 5;
        while(safetyCheck <= 10){
            
            float angle = Random.Range(0f,360f);
            float distance = Random.Range(minSpawnDistance,maxSpawnDistance);

            float spawnX = player.position.x + distance * Mathf.Cos(angle*Mathf.Deg2Rad);
            float spawnZ = player.position.z + distance * Mathf.Sin(angle*Mathf.Deg2Rad);

            Vector3 randomPosition= new Vector3(spawnX,player.position.y + 3f, spawnZ);

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                return hit.position; //  Valid position found
            }
            safetyCheck++;
        }

        return Vector3.zero; //valid position not found;

        
    }
    private void Start()
    {
        LogEnemyStats();
    }

    public void LogEnemyStats()
    {
        Debug.Log($"[EnemySpawner] Initial Stats:\n" +
                  $"Health: {CurrentHealth}\n" +
                  $"Damage: {currentDamage}\n" +
                  $"Look Radius: {CurrentLookRadius}\n" +
                  $"Spawn Interval: {spawnInterval}\n" +
                  $"XP Amount: {CurrentXPAmount}");
    }

}
