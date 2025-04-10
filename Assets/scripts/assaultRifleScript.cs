
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class assaultRifleScript : MonoBehaviour
{
    
    public float damage = 10f;
    public float range = 40f;
    public float fireRate = 10f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    private float nextTimetoFire = 0f;
    [SerializeField] private AudioSource gunAudio;
    [SerializeField] private AudioClip gunshotClip;

    //ADS parameters

    public float timetoZoom = 0.3f;
    public float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private Transform gunObject;
    [SerializeField] private Vector3 gunDefaultPosition;
    [SerializeField] private Vector3 gunDefaultRotation;
    [SerializeField] private Vector3 gunZoomedPosition;
    [SerializeField] private Vector3 gunZoomedRotation;

    

    //recoil parameters for the gun

    public float recoilAmountGun = 0.2f;
    public float RecoilSpeedGun = 10f;
    public float RecoilRecoverSpeedGun = 5f;

    private Vector3 gunOriginalPosition;
    private Vector3 gunRecoiledPosition;

    public bool CanFireBurningBullets = false;
    public float FlamingBulletChance = 0f;
    public float fireDuration;
    public float fireDamage;
    public enemy enemy;

    
    



    void Awake(){
        defaultFOV = fpsCam.fieldOfView;
        gunObject.localPosition = gunDefaultPosition;
        gunObject.localRotation = Quaternion.Euler(gunDefaultRotation);
        gunOriginalPosition = gunObject.localPosition;
        gunRecoiledPosition = gunOriginalPosition;
    }
    


    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimetoFire){
            nextTimetoFire = Time.time + 1f/fireRate;
            Shoot();
        }
        HAndleZoom();
        HandleRecoil();
    }
    void Shoot(){
        muzzleFlash.Play();
        gunAudio.PlayOneShot(gunshotClip);
        float flamingbullet = Random.value;
        bool isFlamingBullet = flamingbullet <= FlamingBulletChance;
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, range)){
            
             enemy = hit.transform.GetComponent<enemy>();
            if(enemy != null){
                if(isFlamingBullet && CanFireBurningBullets){
                    enemy.fireDuration = fireDuration;
                    enemy.fireDamage= fireDamage;

                    enemy.ApplyFire();
                }
                    
                enemy.TakeDamage(damage);
                
    
            }
            GameObject impactobject = Instantiate(hitEffect,hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactobject,2f);
        }
        addRecoil();

    }
    private void HAndleZoom(){
        if(Input.GetKeyDown(zoomKey)){
            if(zoomRoutine != null){
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(toggleZoom(true));
        }
        if(Input.GetKeyUp(zoomKey)){
            if(zoomRoutine != null){
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(toggleZoom(false));
        }
    }
    private void addRecoil(){
        

        //adding recoil to Gun
        Vector3 GunCurrentRestingPosition = fpsCam.fieldOfView == zoomFOV ? gunZoomedPosition : gunDefaultPosition;
        gunRecoiledPosition = GunCurrentRestingPosition - new Vector3(0,0,recoilAmountGun);

        
    }
    private void HandleRecoil(){


        //Gun
        Vector3 currentRestingPosition = fpsCam.fieldOfView == zoomFOV ? gunZoomedPosition : gunDefaultPosition;
        gunObject.localPosition = Vector3.Lerp(gunObject.localPosition,gunRecoiledPosition,Time.deltaTime * RecoilSpeedGun);
        gunRecoiledPosition = Vector3.Lerp(gunRecoiledPosition,currentRestingPosition,Time.deltaTime * RecoilRecoverSpeedGun);
    }


    private IEnumerator toggleZoom(bool isEnter){
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = fpsCam.fieldOfView;
        Vector3 targetgunPosition = isEnter ? gunZoomedPosition : gunDefaultPosition;
        Quaternion targetGunRotation = isEnter? Quaternion.Euler(gunZoomedRotation) : Quaternion.Euler(gunDefaultRotation);
        Vector3 startingGunPosition = gunObject.localPosition;
        Quaternion startingGunRotation = gunObject.localRotation;
        float timeElapsed = 0f;
        while(timeElapsed < timetoZoom){
            fpsCam.fieldOfView = Mathf.Lerp(startingFOV, targetFOV,timeElapsed / timetoZoom);
            gunObject.localPosition = Vector3.Lerp(startingGunPosition, targetgunPosition,timeElapsed / timetoZoom);
            gunObject.localRotation = Quaternion.Lerp(startingGunRotation,targetGunRotation,timeElapsed/timetoZoom);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fpsCam.fieldOfView = targetFOV;
        gunObject.localPosition = targetgunPosition;
        gunObject.localRotation = targetGunRotation;
        zoomRoutine = null;
    }
}
