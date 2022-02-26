using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 weaponEndPointPosition;
        public Vector3 shootPosition;
       // public Vector3 shellPosition;
    }


    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
   // private Transform aimShellPositionTransform;
   // private Animator aimAnimator;
    private bool isFacingRight = true;

    public float fireRate;
    private float nextFire;

    private void Awake()
    {
        //  aimTransform = transform.Find("IK_Manager").Find("Aim");
        aimTransform = aimGunEndPointTransform = transform.Find("IK_Manager").Find("Aim").Find("WeaponEndPointPosition");
        aimTransform = GameObject.Find("Aim Effector").transform;


      //  aimAnimator = aimTransform.GetComponent<Animator>();
      //   aimShellPositionTransform = aimTransform.Find("ShellPosition");
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
            if (isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            aimLocalScale.y = +1f;
            if (!isFacingRight)
            {
                Flip();
            }
        }
        aimTransform.localScale = aimLocalScale;

    }


    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 mousePosition = GetMouseWorldPosition();

            //aimAnimator.SetTrigger("Shoot");

            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                weaponEndPointPosition = aimGunEndPointTransform.position,
                shootPosition = mousePosition,
              //  shellPosition = aimShellPositionTransform.position,
            });
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }


    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

}
