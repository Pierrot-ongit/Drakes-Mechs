using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        ShootingTarget,
        GoingBackToStart,
    }
    private State state;

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 1f;
    public Transform enemyGFX;
    [SerializeField] private Transform pfProjectile;
    [SerializeField] private Transform shootTransform;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] float targetRange = 50f;
    public float fireRate;
    private float nextFire;

    Rigidbody2D rb;
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private Vector3 pathEndPosition;
    private List<GraphNode> nodesRoaming;
    private GameManager gameManager;

    // TODO Timer to change roaming.

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        state = State.Roaming;
        startingPosition = transform.position;
        // Box of nodes where the enemy is roaming.
        var gg = AstarPath.active.data.gridGraph;
        IntRect rect = new IntRect((int)startingPosition.x, (int)startingPosition.y - 3, (int)startingPosition.x + 10, (int)startingPosition.y + 15);
        nodesRoaming = gg.GetNodesInRegion(rect);
    }

    void Start()
    {
        roamPosition = GetRoamingPosition();
        pathEndPosition = roamPosition;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    void UpdatePath()
    {

        if (seeker.IsDone())
        {

            seeker.StartPath(rb.position, pathEndPosition, OnPathComplete);
        }

    }

    // Invoked when the seeker is finished.
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isGameActive)
        {
            return;
        }
        switch (state)
        {
            default:
            case State.Roaming:
                float reachedPositionDistance = 5f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    // Reached Roam Position
                    roamPosition = pathEndPosition = GetRoamingPosition();
                }
                FindTarget();
                break;
            case State.ChaseTarget:
                // Chasing player.
                pathEndPosition = target.position;
                //aimShootAnims.SetAimTarget(Player.Instance.GetPosition());
                if (Vector3.Distance(transform.position, target.position) < attackRange)
                {
                    path = null;
                    currentWaypoint = 0;
                    state = State.ShootingTarget;
                    /* 
                    aimShootAnims.ShootTarget(target.position, () =>
                    {
                        state = State.ChaseTarget;
                    });
                    */
                }
                float stopChaseDistance = 80f;
                if (Vector3.Distance(transform.position, target.position) > stopChaseDistance)
                {
                    // Too far, stop chasing
                    state = State.GoingBackToStart;
                }
                break;

            case State.ShootingTarget:
                // TODO. handle facing.
                HandleFacing(target.position);
                HandleShooting(target.position);
                // Continue attacking until out of range.
                // Or go Back to Roaming  ?
                 roamPosition = pathEndPosition = GetRoamingPosition();
                 state = State.Roaming;
                break;

            case State.GoingBackToStart:
                reachedPositionDistance = 10f;
                if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
                {
                    // Reached Start Position
                    state = State.Roaming;
                }
                break;
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (path == null || path.vectorPath.Count <= currentWaypoint)
        {
            return;
        }

        // Arrow from current player position to nextWaypoint with a lenght of one.
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (state != State.ShootingTarget)
        {
            HandleFacing(new Vector3(force.x, force.y, 0f));
        }
    }


    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, target.position) < targetRange)
        {
            // Player within target range
            state = State.ChaseTarget;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        if ((nodesRoaming != null) && (nodesRoaming.Count > 0))
        {
            int index = Random.Range(0, nodesRoaming.Count);
            return (Vector3)nodesRoaming[index].position;
        }

        // Backup method.
        return startingPosition + GetRandomDir() * Random.Range(3f, 10f);
    }

    Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


    void HandleFacing(Vector3 positionToLook)
    {
        if (positionToLook.x > transform.position.x)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void HandleShooting(Vector3 targetPosition)
    {
        if (Time.time > nextFire)
        {
            Shoot(targetPosition);
            nextFire = Time.time + fireRate;
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        Transform bulletTransform = Instantiate(pfProjectile, shootTransform.position, Quaternion.identity);
        // Direction toward the targetPosition.
        Vector3 shootDir = (targetPosition - shootTransform.position).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);
    }

    public void setChasingTarget()
    {
        state = State.ChaseTarget;
    }
}
