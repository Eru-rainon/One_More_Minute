using UnityEngine;

public class FirstPersonTerrainWalk : MonoBehaviour
{
    public Terrain terrain;
    public float moveSpeed = 3f; // Walking speed
    public float rotationSpeed = 1f; // Smooth turning
    public float minHeight = 1.5f; // Keeps camera above the ground
    public float maxHeight = 3f; // Allows slight elevation changes
    public float directionChangeInterval = 5f; // How often to turn
    public float safeZone = 20f; // Keeps camera away from terrain edges

    private Vector3 targetDirection;
    private Quaternion targetRotation;
    private float timer;

    void Start()
    {
        SetStartPosition();
        SetNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            SetNewDirection();
            timer = 0f;
        }

        // Move forward in the current direction
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Adjust height and ensure camera stays inside terrain
        AdjustHeight();
        KeepInsideTerrainBounds();
    }

    void SetStartPosition()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Start in the middle of the terrain
        float startX = terrainWidth / 2f;
        float startZ = terrainLength / 2f;
        float terrainHeight = terrain.SampleHeight(new Vector3(startX, 0, startZ));

        transform.position = new Vector3(startX, terrainHeight + minHeight, startZ);
    }

    void SetNewDirection()
    {
        // Pick a random walking direction
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0; // Keep movement horizontal
        targetDirection = randomDirection.normalized;
        targetRotation = Quaternion.LookRotation(targetDirection);
    }

    void AdjustHeight()
    {
        float terrainHeight = terrain.SampleHeight(transform.position);
        float adjustedHeight = Mathf.Clamp(terrainHeight + minHeight, minHeight, maxHeight);
        transform.position = new Vector3(transform.position.x, adjustedHeight, transform.position.z);
    }

    void KeepInsideTerrainBounds()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        float x = Mathf.Clamp(transform.position.x, safeZone, terrainWidth - safeZone);
        float z = Mathf.Clamp(transform.position.z, safeZone, terrainLength - safeZone);

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
