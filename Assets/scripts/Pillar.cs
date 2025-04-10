using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI activationText;
    [SerializeField] private float ActivationRadius;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform Player;
    [SerializeField] private ParticleSystem LaserBeam;

    private float countdownTime = 5f;
    private bool hasEnemyBeenSpawned = false;
    private GameObject SpawnedEnemy;
    private float currentCountdown;
    private Coroutine countdownRoutine;
    public GameObject enemySpawner;
    public Gamelogic gamelogic;

    void Start()
    {
        activationText.gameObject.SetActive(false);
        currentCountdown = countdownTime;
    }

    void Update()
    {
        if (hasEnemyBeenSpawned) return;

        if (IsPlayerInsideRadius())
        {
            if (countdownRoutine == null)
            {
                countdownRoutine = StartCoroutine(StartCountdown());
                activationText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
                countdownRoutine = null;
                ResetCountdown();
            }
        }
    }

    private bool IsPlayerInsideRadius()
    {
        if(Player != null){
            return Vector3.Distance(transform.position, Player.position) <= ActivationRadius;
        }
            return false;
        
    }

    private IEnumerator StartCountdown()
    {
        currentCountdown = countdownTime;

        while (currentCountdown > 0)
        {
            activationText.text = $"Activating: {currentCountdown:F1}s";
            yield return new WaitForSeconds(0.1f); // Smoother countdown
            currentCountdown -= 0.1f;

            if (!IsPlayerInsideRadius()) // Stop if player leaves
            {
                ResetCountdown();
                countdownRoutine = null;
                yield break;
            }
        }

        SpawnEnemy();
        countdownRoutine = null;
    }

    private void ResetCountdown()
    {
        currentCountdown = countdownTime;
        activationText.gameObject.SetActive(false);
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-ActivationRadius, ActivationRadius),
            0f, 
            Random.Range(-ActivationRadius, ActivationRadius)
        );

        // Raycast from above to detect terrain height
        RaycastHit hit;
        float terrainHeight = transform.position.y; // Default to pillar height

        if (Physics.Raycast(new Vector3(spawnPosition.x, transform.position.y + 10f, spawnPosition.z), Vector3.down, out hit))
        {
            terrainHeight = hit.point.y;
        }

        spawnPosition.y = terrainHeight; // Ensure it spawns on terrain

        SpawnedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        if(SpawnedEnemy.TryGetComponent(out temporarybossScript temporaryboss)){
            temporaryboss.setPillar(this);
        }
        activationText.gameObject.SetActive(false); // Hide text after spawning
        hasEnemyBeenSpawned = true;

        enemy enemyScript = SpawnedEnemy.GetComponent<enemy>();
        enemyScript.health *= gamelogic.bossHealthmultiplier;
        enemyScript.bossScoremultiplier = gamelogic.bossScoremultiplier;
        SpawnedEnemy.GetComponent<enemyController>().attackDamage *= gamelogic.bossDamagemultiplier;

        Debug.Log($"Enemy Health: {enemyScript.health}");
        Debug.Log($"Boss Score Multiplier: {enemyScript.bossScoremultiplier}");
        Debug.Log($"Enemy Attack Damage: { SpawnedEnemy.GetComponent<enemyController>().attackDamage}");

        enemySpawner.GetComponent<EnemySpawner>().shouldSpawnEnemy = false; //normal enemies dont spawn during a bossFight
    }
    public void BossDefeated(){
        var mainModule = LaserBeam.main;
        mainModule.startColor = Color.green;

        Renderer laserRenderer = LaserBeam.GetComponent<Renderer>();
        laserRenderer.material.EnableKeyword("_EMISSION");
        laserRenderer.material.SetColor("_EmissionColor",Color.green * 4.7f);

         enemySpawner.GetComponent<EnemySpawner>().shouldSpawnEnemy = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ActivationRadius);
    }
}
