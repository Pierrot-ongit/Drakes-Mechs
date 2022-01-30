/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{

    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private Transform aimShellPositionTransform;
    private Animator aimAnimator;
    private bool isFacingRight = true;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("WeaponEndPointPosition");
      //  aimAnimator = aimTransform.GetComponent<Animator>();
     //   aimShellPositionTransform = aimTransform.Find("ShellPosition");
    }

    private void Update()
    {
        HandleAiming();
       // HandleShooting();
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
