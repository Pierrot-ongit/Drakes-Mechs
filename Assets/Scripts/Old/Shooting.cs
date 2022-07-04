using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject shot;
    public Transform weaponTransform;
    public Transform aimTransform;
    public float fireRate;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Shoot();
        }
    }



    void Shoot()
    {
        nextFire = Time.time + fireRate;
        Debug.Log(aimTransform.rotation);

        GameObject bullet = Instantiate(shot, weaponTransform.position, weaponTransform.rotation); // TODO Find weapon rotation.
        bullet.GetComponent<Rigidbody2D>().velocity = weaponTransform.transform.up * 10;
        /*
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
           new Vector3(0, HorizontalRotation * rotationSpeed, VericalRotation * rotationSpeed));
        */
    }
}
