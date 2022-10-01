using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow3D : SimpleSingleton<CameraFollow3D>
{
    public enum UpdateTypes
    {
        Update,
        LateUpdate,
        LateUpdateAndUpdate,
        FixedUpdate
    }
    
    public enum FollowTypes
    {
        LocatedAtTarget, // LT
        FollowForTarget  // FT
    }

    public Transform Target;
    public UpdateTypes UpdateType;
    public FollowTypes FollowType = FollowTypes.LocatedAtTarget;
    
    // Located at target (LT)
    public float LT_MoveSpeed;
    public bool LT_Rotating = true;
    public float LT_RotateSpeed;
    
    // Follow For Target (FT)
    public Vector3 FT_MoveAxis;
    public bool FT_LockY;
    public bool FT_SetHeightByTarget;
    public float FT_HeightByTarget;
    public float FT_SpeedChangeHeight = 1f;
    
    public float FT_MoveSpeed;
    public float FT_MinDistanceToTarget;
    public float FT_MaxDistanceToTarget;
    public float FT_RotateSpeed;
    
    


    #region Updates
    
    private void Update()
    {
        if (UpdateType == UpdateTypes.Update || UpdateType == UpdateTypes.LateUpdateAndUpdate)
            UpdateCamera(Time.deltaTime);
    }
    private void LateUpdate()
    {
        if (UpdateType == UpdateTypes.LateUpdate || UpdateType == UpdateTypes.LateUpdateAndUpdate)
            UpdateCamera(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (UpdateType == UpdateTypes.FixedUpdate)
            UpdateCamera(Time.fixedDeltaTime);
    }

    #endregion

    private void UpdateCamera(float deltaTime)
    {
        if(!Target)
            return;
        
        switch (FollowType)
        {
            case FollowTypes.LocatedAtTarget:
                LT_UpdatePosition(deltaTime);
                break;
            case FollowTypes.FollowForTarget:
                UpdateLT(deltaTime);
                break;
        }
    }

    public void LT_UpdatePosition(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, LT_MoveSpeed * deltaTime / 2f );
        if(LT_Rotating)
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, LT_RotateSpeed* deltaTime / 2f);
    }

    public void UpdateLT(float deltaTime)
    {
        var direction = (Target.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,lookRotation,FT_MoveSpeed * deltaTime / 2f);

        if (FT_LockY && FT_SetHeightByTarget)
        {
            transform.position += Vector3.up * (Target.position.y - transform.position.y + FT_HeightByTarget) * deltaTime * FT_SpeedChangeHeight;
        }
        
        var distanceToTarget = (transform.position - Target.transform.position).magnitude;
        if (distanceToTarget > FT_MaxDistanceToTarget)
        {
            transform.position += new Vector3(direction.x * FT_MoveAxis.x, FT_LockY ? 0f : direction.y* FT_MoveAxis.y, direction.z* FT_MoveAxis.z) *
                                  FT_MoveSpeed  *
                                  (distanceToTarget - FT_MaxDistanceToTarget) *
                                  Time.deltaTime;
        }
        else if (distanceToTarget < FT_MinDistanceToTarget)
        {
            transform.position += new Vector3(direction.x * FT_MoveAxis.x, FT_LockY ? 0f : direction.y * FT_MoveAxis.y, direction.z* FT_MoveAxis.z) *
                                  FT_MoveSpeed  *
                                  (distanceToTarget - FT_MinDistanceToTarget) *
                                  deltaTime;
        }
    }

    #region Target

    public void SetTarget(Transform _target) => Target = _target;
    public void SetTarget(MonoBehaviour _target) => Target = _target.transform;
    public void ResetTarget() => Target = null;

    public void SetCameraToTarget()
    {
        if(FollowType == FollowTypes.LocatedAtTarget)
        {
            transform.position = Target.position;
            if (LT_Rotating)
                transform.rotation = Target.rotation;
        }
        else if (FollowType == FollowTypes.FollowForTarget)
        {

            var direction = -Target.forward;

            transform.position = Target.position + (new Vector3(direction.x * FT_MoveAxis.x,
                                                        FT_LockY ? 0f : direction.y * FT_MoveAxis.y,
                                                        direction.z * FT_MoveAxis.z) *
                                                    ((FT_MaxDistanceToTarget + FT_MinDistanceToTarget) / 2));
            transform.position += Vector3.up * (Target.position.y - transform.position.y + FT_HeightByTarget);
        }
    }

    #endregion
    
    
    #region EDITOR
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraFollow3D))]
    public class CameraFollow3DEditor : Editor
    {
        private CameraFollow3D T
        {
            get
            {
                if (_t == null)
                    _t = (CameraFollow3D)target;
                return _t;
            }
        }
        private CameraFollow3D _t;


        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            T.Target = (Transform)EditorGUILayout.ObjectField("Target", T.Target, typeof(Transform));
            EditorGUILayout.Space();
            T.FollowType = (FollowTypes) EditorGUILayout.EnumPopup("Follow Type", T.FollowType);
            T.UpdateType = (UpdateTypes) EditorGUILayout.EnumPopup("Update Type", T.UpdateType);
            EditorGUI.indentLevel++;
            switch (T.FollowType)
            {
                case FollowTypes.LocatedAtTarget:
                    T.LT_MoveSpeed = EditorGUILayout.Slider("Move Speed", T.LT_MoveSpeed,0f,100f);
                    T.LT_Rotating = EditorGUILayout.Toggle("Rotating", T.LT_Rotating);
                    if(T.LT_Rotating)
                        T.LT_RotateSpeed = EditorGUILayout.Slider("Rotate Speed", T.LT_RotateSpeed,0f,100f);
                    
                    break;
                case FollowTypes.FollowForTarget:
                    T.FT_MoveAxis = EditorGUILayout.Vector3Field("Move Axis", T.FT_MoveAxis);
                    T.FT_LockY = EditorGUILayout.Toggle("Lock Y", T.FT_LockY);
                    if (T.FT_LockY)
                    {
                        EditorGUI.indentLevel++;
                        T.FT_SetHeightByTarget = EditorGUILayout.Toggle("Height By Target", T.FT_SetHeightByTarget);
                        if (T.FT_SetHeightByTarget)
                        {
                            EditorGUI.indentLevel++;
                            T.FT_HeightByTarget = EditorGUILayout.FloatField("Height",T.FT_HeightByTarget);
                            T.FT_SpeedChangeHeight = EditorGUILayout.FloatField("Speed",T.FT_SpeedChangeHeight);
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;
                    }
                    T.FT_MoveSpeed = EditorGUILayout.Slider("Move Speed", T.FT_MoveSpeed,0f,100f);
                    T.FT_MinDistanceToTarget = EditorGUILayout.FloatField("Min Distance To Target", T.FT_MinDistanceToTarget);
                    T.FT_MaxDistanceToTarget = EditorGUILayout.FloatField("Max Distance To Target", T.FT_MaxDistanceToTarget);
                    EditorGUILayout.Space();
                    T.FT_RotateSpeed = EditorGUILayout.Slider("Rotate Speed", T.FT_RotateSpeed,0f,100f);
                    break;
            }        
            EditorGUI.indentLevel--;
            if (GUILayout.Button("Set camera to Target"))
            {
                T.SetCameraToTarget();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(T);
            }

        }
    }
#endif
    
    #endregion
}
