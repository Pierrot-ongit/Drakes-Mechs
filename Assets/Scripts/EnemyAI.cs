using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
        {
          seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

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
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            return;
        } else
        {
            reachEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Enemy moving to the right
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        // Enemy moving to the left
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
