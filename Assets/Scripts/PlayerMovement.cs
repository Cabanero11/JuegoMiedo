using Unity.Burst.CompilerServices;
using UnityEngine;
using static PlayerAudioManager;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementSpeed = 7f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float groundDrag = 2f;
    public Transform playerObj;

            

    [Header("Salto")]
    public bool activarSalto = false;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;

    [Header("Agacharse")]
    public bool activarSlide = false;
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    private float startYScale;
    //private float crouchForce = 5f;

    [Header("KeyCodes")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode slideKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight = 2f;
    public bool grounded;
    public float extraRayDistance = 0.2f;
    [Space]
    [SerializeField]
    private TerrainType currentStandingTerrain;
    public LayerMask whatIsGround;
    public LayerMask waterLayer;

    [Header("Rampa")]
    public float maxSlopeAngle = 60f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Sliding variables")]
    private bool cieling;
    public float maxSlideTime;
    public float slideForce;
    public float slideYScale;
    private float slideStartYScale;
    public bool isSliding;
    public float downForce = 5f;
    public float slideSpeed;
    public float speedIncreaseMultiplier = 1.5f;
    public float slopeIncreaseMultiplier = 2.5f;


    //private float wallClimbSpeed = 3f;

    // variables para el momentum
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Space]
    public Transform orientation;
    public Transform cameraPosition;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    private bool keepMomentum;

    private MovementState lastState;
    public MovementState movState;
    public enum MovementState
    {
        jumping,
        sliding,
        sprinting,
        crouching,
        air,
        walking
    }

    void Start()
    {
        //if (playerCamera == null) { playerCamera = Player.camera; }

        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        isSliding = false;

        startYScale = transform.localScale.y;
        slideStartYScale = transform.localScale.y;


    }

    private void Update()
    {
        DoAllRaycasts(); // hacer raycasts para el suelo, techo, y paredes

        MyInput();
        //SpeedControl();
        MovementStateHandler();

        // aplicarle drag si esta en el suelo
        if (movState == MovementState.walking || movState == MovementState.sprinting || movState == MovementState.crouching)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (Input.GetKeyDown(slideKey)) //&& (horizontalInput != 0 || verticalInput != 0)) //&& !isSliding)
        {
            StartSlide();
        }
        if ((Input.GetKeyUp(slideKey) || !Input.GetKey(slideKey)) && isSliding)
        {
            if (!cieling)
            {
                StopSlide();
            }
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        //ChangeUi();

        if (isSliding || cieling)
        {
            SlidingMovement();
        }
    }

    //Este metodo cambia la hitbox del personaje al agacharse
    private void ChangeTransform()
    {
        if (Input.GetKeyDown(crouchKey) && grounded && rb.velocity.magnitude <= walkSpeed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        if (Input.GetKeyUp(crouchKey) || (!grounded && movState == MovementState.crouching))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        ChangeTransform();

        // Si se mueve, sonido de Andar
        if(horizontalInput != 0 || verticalInput != 0)
        {
            PlayerAudioManager.instance.PlaySonidosAndar(currentStandingTerrain);
        }

        if (Input.GetKey(jumpKey) && grounded)
        {
            Jump();
            exitingSlope = false;
        }
    }

    private void MovePlayer()
    {

        // calcular direccion de movimiento
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope && grounded)
        {
            if (movState != MovementState.sliding)
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * movementSpeed * 20f, ForceMode.Force);

            // Para que no este pegado a la rampa
            else
            {
                rb.AddForce(Vector3.down * 300f);
            }
        }
        // en el suelo
        else if (grounded && movState != MovementState.sliding)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)  // si esta en el aire * airMultiplier
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed);
        }
    }

    private void MovementStateHandler()
    {

        // si separados, Run y Crouch a la vez
        if (Input.GetKey(crouchKey) && rb.velocity.magnitude <= walkSpeed && grounded && !OnSlope())
        {

            movState = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        // Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            movState = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;

            //playerCamera.DoFov(cameraSprintFov);
        }
        // Sliding else if (isSliding) 
        else if (Input.GetKey(slideKey) && (movState != MovementState.crouching))
        {
            movState = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        // Walking 
        else if (grounded)
        {
            movState = MovementState.walking;
            desiredMoveSpeed = walkSpeed;

            //playerCamera.DoFov(cameraStartFov);
        }
        // Air
        else
        {
            movState = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }

        }

        // Por si cambia mucho, 4f seria la diferencia, y ahi deberia ser lento el cambio
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && movementSpeed != 0)
        {
            StopAllCoroutines();
            //StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            movementSpeed = desiredMoveSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        // Si queremos momentum en tipo de movimiento, dentro del if, sino fuera
        
        if(desiredMoveSpeedHasChanged)
        {   
            if(keepMomentum)
            {
                StopAllCoroutines();
                //StartCoroutine(SmoothlyLerpMoveSpeed());
            } else
            {
                StopAllCoroutines();
                movementSpeed = desiredMoveSpeed;
            }
        }

        // Guardar los anteriores
        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = movState;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            // angulo de la rampa, sabiendolo con el Raycast
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }
        // si no golpea nada
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        // aqui se calcula el angulo sobre el que esta la Rampa y el jugador, para aplicarle fuerza en la direccion de la rampa
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    // ############################################
    // ############### MISCELANEO #################
    // ############################################

    void DoAllRaycasts()
    {
        float dist = playerHeight * 0.5f + extraRayDistance;

        // Verificar si hay techo
        cieling = Physics.Raycast(transform.position, Vector3.up, dist, whatIsGround);

        // Verificar si está en el suelo y obtener información del terreno
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, dist, whatIsGround);

        // Si está en el suelo, identificar el terreno en base a su capa
        if (grounded)
        {
            int hitLayer = hit.collider.gameObject.layer; // Obtener la capa del objeto impactado

            //Debug.Log($"Layer detectado: {hitLayer}");

            // Comprobar si pertenece al Layer de agua
            if (waterLayer == (waterLayer | (1 << hitLayer)))
            {
                currentStandingTerrain = TerrainType.Water;
            }
            // Comprobar si pertenece al Layer de tierra
            else if (whatIsGround == (whatIsGround | (1 << hitLayer)))
            {
                currentStandingTerrain = TerrainType.Ground;
            }
            else
            {
                Debug.LogWarning("El layer detectado no está asignado en los LayerMask.");
            }
        }
    }

    // ############################################
    // #############      JUMP     ################
    // ############################################

    private void Jump()
    {
        // ESTA FALSE; DEFAULT, asi que no salta :D
        if(activarSalto == true)
        {
            // Efecto de sonido del salto
            //PlayerAudioManager.instance.PlayJumpSound();
            exitingSlope = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    // ############################################
    // ############     SLIDING    ################
    // ############################################

    private void StartSlide()
    {
        // SI ESTA ACTIVADO LO DE SLIDE
        if (activarSlide == true)
        {
            isSliding = true;

            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);

            //playerCamera.DoFov(cameraSlideFov);
        }
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }

    private void StopSlide()
    {
        isSliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideStartYScale, playerObj.localScale.z);

        // playerCamera.DoFov(cameraStartFov);
    }




    // ############################################
    // ############ CUARENTENA DE CODIGO ##########
    // ############################################
}
