using System.Collections;
using UnityEngine;

public class oreSpawner : MonoBehaviour
{
    [SerializeField] private GameObject OreSource;
    [SerializeField] private  Transform player;
    public float spawnInterval = 20;

    public float Spawnradius = 10f;
    public float spawnheight = 8f;

    void Start()
    {
        StartCoroutine(oreSourceSpawningCoroutine());
    }

    private IEnumerator oreSourceSpawningCoroutine(){
        while(true){
            yield return new WaitForSeconds(spawnInterval);
            SpawnOreSource();
        }
    }
    void SpawnOreSource(){

        if(player == null) return;

        Vector2 randomCordinates = Random.insideUnitCircle*Spawnradius;
        Vector3 spawnposition = new Vector3( player.position.x + randomCordinates.x,
                                            player.position.y + spawnheight,
                                            player.position.z + randomCordinates.y
        );

        Instantiate(OreSource,spawnposition,Quaternion.identity);

    }
}
