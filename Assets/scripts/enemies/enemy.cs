

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using JetBrains.Annotations;

public class enemy : MonoBehaviour
{
    public float health = 100f;
    public float XPamount = 3f;
    public  bool isBossEnemy;
    [SerializeField] private GameObject XPObject;
    [SerializeField] private Animator EnemyRenderer;
    

    //Burning Effect
    [SerializeField] private GameObject burningEffect;
     private GameObject fireEffectInstance;
    private bool isBurning = false;
    public float fireDamage = 5f;
    public float fireDuration = 1f; 
    private bool isDead = false;

    //Explosion  on Death
    public bool explosionOnDeath = false;
    public float explosionChance = 0.3f; 
    public float explosionRadius = 3f;
    public float explosionDamage = 100f;
    private GameObject player;
    public GameObject HealthBar;
    public Slider slider;
    EnemyHealthBar enemyHealthBar;
    private float maxHealth;
    private Transform MianCamera;
    [SerializeField] private GameObject explosionEffect;

    public  float regularscoremultiplier;
    public float bossScoremultiplier;
    public int score = 100;

    //audio
    public AudioSource enemyaudio;
    public AudioClip bullethit;
    public AudioClip deathsound;

    void Start()
    {
        maxHealth = health;
        player = playerFinder.instance.player;
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        MianCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        HealthBar.transform.LookAt(HealthBar.transform.position + MianCamera.forward);
    }
    public delegate void DeathAction();
    public event DeathAction onDeath;

    public void TakeDamage(float damage){
        enemyaudio.PlayOneShot(bullethit);
        health -= damage;
        if(!HealthBar.activeSelf)
            HealthBar.SetActive(true);
        enemyHealthBar.updateHealth(slider,health,maxHealth);
        if(health <= 0){
            HealthBar.SetActive(false);
            die();
        }
    }
    public void ApplyFire(){
        if(isBurning) return;
        else{
            isBurning = true;
            fireEffectInstance = Instantiate(burningEffect,transform.position,Quaternion.identity);
            fireEffectInstance.transform.SetParent(transform);
            InvokeRepeating(nameof(ApplyFireDamage), 0f, 1f);
            StartCoroutine(StopFireAfterDuration());   

        }
    }
    public void ApplyFireDamage(){
        TakeDamage(fireDamage);
    }
    void die(){

        if(isDead) return;
        isDead = true;
        if(isBossEnemy){
            GetComponent<temporarybossScript>().onBossDeath();
        }else{
            onDeath?.Invoke();

        }

        for(int i =0;i<XPamount;i++){
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.5f,0.5f),0,UnityEngine.Random.Range(-0.5f,0.5f));
            Instantiate(XPObject,transform.position + randomOffset,Quaternion.identity);
        }
        


        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }
        if(explosionOnDeath && Random.value <= explosionChance){
            //explosionCHnace % chance to explode
            Explode();
        }
        
        EnemyRenderer.SetTrigger("Dead");
        Destroy(gameObject,0.5f);
        float multiplier = isBossEnemy ? bossScoremultiplier : regularscoremultiplier;
        player.GetComponent<PlayerScore>().addscore(Mathf.RoundToInt(score * multiplier));
        enemyaudio.PlayOneShot(deathsound);
    }

    private void Explode(){
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
       

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in hitEnemies)
        {
            enemy otherEnemy = col.GetComponent<enemy>();
            if (otherEnemy != null && otherEnemy != this)
            {
                otherEnemy.TakeDamage(explosionDamage);
            }
        }

    }

    private IEnumerator StopFireAfterDuration(){
        yield return new WaitForSeconds(fireDuration);
        CancelInvoke(nameof(ApplyFireDamage));
        isBurning = false;
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }
    }
}
