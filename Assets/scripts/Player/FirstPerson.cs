
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;



public class FirstPerson : MonoBehaviour
{
    public bool canMove = true;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    private bool shouldJump => playerisGrounded() && Input.GetKeyDown(jumpKey);
    [Header("Functional Options")]
    public bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool headbobEnabled = true;
    


    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;


    
    [Header("Movement parameters")]
    public float walkspeed = 3.5f;
    public float SprintSpeed = 7f;
    public float gravity = 30f;
    public float jupmForce = 8f;
    [SerializeField] private AudioClip JumpStartClip;
    [SerializeField] private AudioClip JumpEndClip;

    

    [Header("look parameters")]
    public float lookSpeedX = 2.0f;
    public float lookSpeedY = 2.0f;


    [Header("HeadBob parameters")]
    [SerializeField]private float walkBobSpeed = 14f;
    [SerializeField]private float walkbobAmount = 0.05f;
    [SerializeField]private float SprintbobSpeed = 18f;
    [SerializeField]private float sprintbobAmount = 0.1f;
    private float defaultYpos = 0;
    private float timer;

    [Header("Health & Stamina parameters")]
    
    //Stamina
    public float maxStamina = 100f;
    public float StaminaUsageMultiplier = 5f;
    public float timeBeforeStaminaRegen = 2f;
    public float StaminaIncrement = 2f;
    public float StaminaIncrementTime = 0.1f;
    public float currentStamina;
    private Coroutine regeneratingStamina;


    [Header("FootSteps")]
    public float BaseStepSpeed = 0.5f;
    [SerializeField] private float SprintFootStepMultiplier = 0.6f;
    [SerializeField] private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] grounndSteps = default;
    [SerializeField] private AudioClip[] grassSteps = default;
    private float footStepTimer = 0f;
    private float getCurrentOffset => isSprinting ? BaseStepSpeed*SprintFootStepMultiplier : BaseStepSpeed;

    
    //stamina Bar
    public Slider staminaSlider;





    [SerializeField]private float upperLookLimit = 80f;
    [SerializeField]private float lowerLookLimit = 80f;
    private Camera playerCamera;
    private CharacterController playerController;
    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;


    public UImanager uImanager;

    
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        defaultYpos = playerCamera.transform.localPosition.y;
        
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        
        currentStamina = maxStamina;
        uImanager.updateStamina(staminaSlider,currentStamina,maxStamina);

        

        
      
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            handleMovementInput();
            handleMouseLook();
            if(canJump)
                HandleJump();
            if(headbobEnabled)
                bob();
            handleFootSteps();
            HandleStamina();
            applyMovement();


            
            
        }
    }
    private void handleMovementInput(){
        currentInput = new Vector2((isSprinting ? SprintSpeed : walkspeed) * Input.GetAxis("Vertical"),walkspeed*Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
    private void HandleStamina(){
        if(isSprinting && currentInput != Vector2.zero){
            if(regeneratingStamina != null){
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= StaminaUsageMultiplier * Time.deltaTime;
            uImanager.updateStamina(staminaSlider,currentStamina,maxStamina);
            if(currentStamina <= 0){
                currentStamina = 0;
                uImanager.updateStamina(staminaSlider,currentStamina,maxStamina);
                canSprint = false;
            }
        }
        if(!isSprinting && currentStamina < maxStamina && regeneratingStamina == null){
            regeneratingStamina = StartCoroutine(regenerateStamina());
        }
    }
    private void HandleJump(){
        if(shouldJump){
            playerAudioSource.PlayOneShot(JumpStartClip);
            moveDirection.y = jupmForce;

        }
    }
    private void bob(){
        if(!playerController.isGrounded) return;
        if(MathF.Abs(moveDirection.x) > 0.1f || MathF.Abs(moveDirection.z) > 0.1f){
            timer += Time.deltaTime * (isSprinting ? SprintbobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYpos + MathF.Sin(timer) * (isSprinting ? sprintbobAmount : walkbobAmount),
                playerCamera.transform.localPosition.z
            );
        }
    }
    private void handleMouseLook(){
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit,lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX,0,0);
        transform.rotation *= Quaternion.Euler(0,Input.GetAxis("Mouse X")*lookSpeedX,0);
    }
    private void applyMovement(){
        if(!playerController.isGrounded){
            moveDirection.y -=gravity * Time.deltaTime;

        }
        if(playerController.velocity.y < -1 && playerController.isGrounded){
            moveDirection.y = 0;
        }
        playerController.Move(moveDirection * Time.deltaTime);
    }

    
   

    private void handleFootSteps(){
        if(!playerisGrounded()) return;
        if(currentInput == Vector2.zero) return;

        footStepTimer -= Time.deltaTime;
        if(footStepTimer <= 0){
            if(Physics.SphereCast(playerCamera.transform.position,0.5f, Vector3.down, out RaycastHit hit, 6)){
                Terrain terrain = hit.collider.GetComponent<Terrain>();
                if(terrain != null){
                    int dominantIndex = getDominantTerrainTexture(hit.point,terrain);
                    switch(dominantIndex){
                        case 0: playerAudioSource.PlayOneShot(grassSteps[UnityEngine.Random.Range(0,grassSteps.Length-1)]);
                        break;
                        case 1: playerAudioSource.PlayOneShot(grounndSteps[UnityEngine.Random.Range(0,grounndSteps.Length-1)]);
                        break;
                    }
                }
            }
            footStepTimer = getCurrentOffset;
        }
    }
    
    
    private IEnumerator regenerateStamina(){
        yield return new WaitForSeconds(timeBeforeStaminaRegen);
        WaitForSeconds timetoWait = new  WaitForSeconds(StaminaIncrementTime);
        while(currentStamina < maxStamina){
            if(currentStamina > 0)
                canSprint = true;

            currentStamina += StaminaIncrement;

            if(currentStamina > maxStamina)
                currentStamina = maxStamina;
            
            uImanager.updateStamina(staminaSlider,currentStamina,maxStamina);
            yield return timetoWait;
        }
        regeneratingStamina = null;
    }

    private int getDominantTerrainTexture(Vector3 worldPos, Terrain terrain){
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        //getting normalised positon
        float relativeX = (worldPos.x - terrainPos.x)/terrainData.size.x;
        float relativeZ = (worldPos.z - terrainPos.z)/terrainData.size.z;

        // Convert the normalized coordinates to splat map indices.
        int mapX = Mathf.Clamp(Mathf.RoundToInt(relativeX * terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
        int mapZ = Mathf.Clamp(Mathf.RoundToInt(relativeZ * terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);

         // Retrieve the splat map data at this point (1x1 sample).
        float[,,] alphaMap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        int numTextures = alphaMap.GetLength(2);

        int dominantIndex = 0;
        float maxMix = 0f;
        for (int i = 0; i < numTextures; i++)
        {
            if (alphaMap[0, 0, i] > maxMix)
            {
                maxMix = alphaMap[0, 0, i];
                dominantIndex = i;
            }
        }

        return dominantIndex;




    }

    private bool playerisGrounded(){
         return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerController.height / 2 + 0.5f);
    }


    
}
