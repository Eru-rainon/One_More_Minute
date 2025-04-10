
using System.Collections;
using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    public GameObject ore;
    public float spawnduration = 15f;
    public int oreAmount = 20;
    public float spawnRadius = 1.5f;
    [SerializeField] private Animator Spawneranimator;

    void Start()
    {
        StartCoroutine(spawnOre());
    }
    private IEnumerator spawnOre(){
        float spawninterval = spawnduration/oreAmount;
        for(int i =0; i < oreAmount; i++){

            Vector3 spawnpoint = getSpawnPoint();
            Instantiate(ore,spawnpoint,Quaternion.identity);
            yield return new WaitForSeconds(spawninterval);

        }
        Spawneranimator.SetTrigger("Destroy");
    }

    private Vector3 getSpawnPoint()
    {
        float angle = Random.Range(0,Mathf.PI * 2f);
        float radius = Mathf.Sqrt(Random.Range(0f,1f))*spawnRadius;

        float xOffset = radius * Mathf.Cos(angle);
        float zOffSet = radius * Mathf.Sin(angle);

        return new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffSet);

    }
    public void destroySpawner(){
        Destroy(gameObject);
    }
}
