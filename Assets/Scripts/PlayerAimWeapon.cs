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
    private Animator playerAnimator;
    private bool isFacingRight = true;
    [SerializeField] private float maxUpAngle = 60f;
    [SerializeField] private float maxDownAngle = -60f;

    public float fireRate;
    private float nextFire;

    private GameManager gameManager;

    private void Awake()
    {
        aimTransform = GameObject.Find("Aim Transform").transform;
        aimGunEndPointTransform = GameObject.Find("WeaponEndPointPosition").transform;
        gameManager = FindObjectOfType<GameManager>();


        playerAnimator = GetComponent<Animator>();
        //   aimShellPositionTransform = aimTransform.Find("ShellPosition");
    }

    private void Update()
    {
        if (!gameManager.isGameActive)
        {
            return;
        }
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        float aimAngle = getAimAngle(angle);
        aimTransform.eulerAngles = new Vector3(0, 0, aimAngle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 100 || angle < -100)
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
            Vector3 shootDirection = mousePosition;

            float distance = Vector3.Distance(aimTransform.position, aimGunEndPointTransform.position);
            float distance2 = Vector3.Distance(aimTransform.position, mousePosition);
            if (distance2 < distance)
            {
                // Mouse is inside character radius. Bullets will go toward character if no change.
                shootDirection = aimGunEndPointTransform.position + (aimGunEndPointTransform.position - aimTransform.position).normalized;
            }


            playerAnimator.SetTrigger("Shoot");

            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                weaponEndPointPosition = aimGunEndPointTransform.position,
                shootPosition = shootDirection,
              //  shellPosition = aimShellPositionTransform.position,
            });
        }
    }

    // ABSTRACTION
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
        aimTransform.Rotate(0f, 180f, 0f);
    }

    private float getAimAngle(float angle)
    {
        float aimAngle = angle;
        if (isFacingRight) {
            if (aimAngle > maxUpAngle)
            {
                aimAngle = maxUpAngle;
            }
            if (aimAngle < maxDownAngle)
            {
                aimAngle = maxDownAngle;
            }
        }

        return aimAngle;
    }

    // ABSTRACTION
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    // ABSTRACTION
    public Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    // ABSTRACTION
    public Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    // ABSTRACTION
    public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

}
