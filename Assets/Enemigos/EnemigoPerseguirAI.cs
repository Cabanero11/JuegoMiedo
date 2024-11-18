using UnityEngine;
using UnityEngine.AI;

public class EnemigoPerseguirAI : MonoBehaviour
{
    public Transform player;

    [Header("Variables Enemigo")]
    [Range(1f, 40f)] public float rangoPersecucion = 15f;
    [Range(0f, 4f)] public float rangoAtaqueMortal = 2f;

    [Header("Ataque Especial")]
    public float velocidadCarga = 2.5f;
    public float duracionCarga = 2f;
    public float cooldownCarga = 5f;
    [Range(-45f, 45f)] 
    public float anguloDesviacionMin = -25f;
    [Range(-45f, 45f)] 
    public float anguloDesviacionMax = 25f;
    private float cooldownTimer;

    private bool isCharging = false;
    private float cargaTimer;

    private NavMeshAgent navMeshAgent;
    private float originalSpeed;

    // Estados de la máquina
    private enum State
    {
        Patroling,
        Chasing,
        Charging,
        Attacking
    }

    private State currentState = State.Patroling;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) Debug.LogError("No hay Navmesh");
        if (player == null) Debug.LogError("No hay Player");
    }

    void Start()
    {
        originalSpeed = navMeshAgent.speed;
        cooldownTimer = cooldownCarga;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        cooldownTimer += Time.deltaTime;

        switch (currentState)
        {
            case State.Patroling:
                Patrol();
                if (distanceToPlayer <= rangoPersecucion) currentState = State.Chasing;
                break;

            case State.Chasing:
                ChasePlayer();
                if (distanceToPlayer <= rangoAtaqueMortal) currentState = State.Attacking;
                else if (distanceToPlayer <= rangoPersecucion / 2 && cooldownTimer >= cooldownCarga && HasLineOfSight())
                {
                    currentState = State.Charging;
                }
                break;

            case State.Charging:
                ChargeAttack();
                break;

            case State.Attacking:
                Attack();
                break;
        }
    }

    private void Patrol()
    {
        // Lógica de patrullaje (agrega waypoints si es necesario)
        //GameSounds.Play("EnemigoPatrol", 0.9f, 1.1f, 1f, 20f, transform.position);
        navMeshAgent.speed = originalSpeed * 0.5f; // Mover más lento
    }

    private void ChasePlayer()
    {
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    private void ChargeAttack()
    {
        if (!isCharging)
        {
            GameSounds.Play("EnemigoCarga", 0.3f, 0.8f, 1.2f);
            isCharging = true;
            cargaTimer = duracionCarga;
            navMeshAgent.speed = velocidadCarga;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Calcular un ángulo aleatorio entre los límites especificados
            float randomAngle = Random.Range(anguloDesviacionMin, anguloDesviacionMax);
            Vector3 chargeDirection = Quaternion.Euler(0, randomAngle, 0) * directionToPlayer;

            navMeshAgent.SetDestination(transform.position + chargeDirection * rangoPersecucion);
            cooldownTimer = 0f;
        }
        else
        {
            cargaTimer -= Time.deltaTime;
            if (cargaTimer <= 0)
            {
                EndCharge();
            }
        }
    }

    private void EndCharge()
    {
        isCharging = false;
        navMeshAgent.speed = originalSpeed;
        currentState = State.Chasing;
    }

    private void Attack()
    {
        // Lógica de ataque
        Debug.Log("GRYAAHHG Te ataquie >:U ");
        GameSounds.Play("EnemigoAtaque", 0.9f, 1.1f, 1f, 20f, transform.position);

        currentState = State.Patroling; // Volver a patrullaje después de atacar (ajustar según la lógica)
    }

    private bool HasLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.position, out hit))
        {
            return hit.transform == player;
        }
        return false;
    }
}
