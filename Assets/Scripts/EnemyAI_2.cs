using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_2 : MonoBehaviour
{


    private Vector3 startingPosition;
    private Vector3 roamPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetRoamingPosition()
    {
       return  startingPosition + GetRandomDir() * Random.Range(10f, 70f);
    }

    Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
