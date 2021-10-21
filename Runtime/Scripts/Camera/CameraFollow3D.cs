using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow3D : SimpleSingleton<CameraFollow3D>
{
    public enum FollowTypes
    {
        LocatedAtTarget, // LT
        FollowForTarget  // FT
    }

    public FollowTypes FollowType = FollowTypes.LocatedAtTarget;
    
    // Located at target (LT)
    [Range(0,100)] public float LT_MoveSpeed;
    public bool LT_Rotating = true;
    [Range(0,100)] public float LT_RotateSpeed;


    private Transform target;

    private void Update()
    {
        if(!target)
            return;
        
        switch (FollowType)
        {
            case FollowTypes.LocatedAtTarget:
                LT_UpdatePosition();
                break;
            case FollowTypes.FollowForTarget:
                break;
        }
    }

    public void LT_UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, LT_MoveSpeed * Time.deltaTime / 2f );
        if(LT_Rotating)
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, LT_RotateSpeed* Time.deltaTime / 2f);
    }

    #region Target

    public void SetTarget(Transform _target) => target = _target;
    public void SetTarget(MonoBehaviour _target) => target = _target.transform;
    public void ResetTarget() => target = null;

    public void SetCameraToTarget()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    #endregion
    

}
