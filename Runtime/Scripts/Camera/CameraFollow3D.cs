using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow3D : SimpleSingleton<CameraFollow3D>
{
    public enum FollowTypes
    {
        LocatedAtTarget, // LT
        FollowForTarget  // FT
    }

    public Transform Target;
    
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
    
    public float FT_MoveSpeed;
    public float FT_MinDistanceToTarget;
    public float FT_RotateSpeed;
    
    


    private void Update()
    {
        if(!Target)
            return;
        
        switch (FollowType)
        {
            case FollowTypes.LocatedAtTarget:
                LT_UpdatePosition();
                break;
            case FollowTypes.FollowForTarget:
                UpdateLT();
                break;
        }
    }

    public void LT_UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, LT_MoveSpeed * Time.deltaTime / 2f );
        if(LT_Rotating)
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, LT_RotateSpeed* Time.deltaTime / 2f);
    }

    public void UpdateLT()
    {
        var direction = (Target.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,lookRotation,FT_MoveSpeed * Time.deltaTime / 2f);

        if (!FT_LockY && FT_SetHeightByTarget)
        {
            transform.position += Vector3.up * (Target.position.y - transform.position.y + FT_HeightByTarget);
        }
        
        var distanceToTarget = (transform.position - Target.transform.position).magnitude;
        if (distanceToTarget > FT_MinDistanceToTarget)
        {
            transform.position += new Vector3(direction.x * FT_MoveAxis.x,
                                      FT_LockY ? 0f : direction.y* FT_MoveAxis.y,
                                      direction.z* FT_MoveAxis.z) *
                                  FT_MoveSpeed  *
                                  (distanceToTarget - FT_MinDistanceToTarget) *
                                  Time.deltaTime;
        }
    }

    #region Target

    public void SetTarget(Transform _target) => Target = _target;
    public void SetTarget(MonoBehaviour _target) => Target = _target.transform;
    public void ResetTarget() => Target = null;

    public void SetCameraToTarget()
    {
        if(FollowType != FollowTypes.LocatedAtTarget)
            return;
        
        transform.position = Target.position;
        transform.rotation = Target.rotation;
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
            T.Target = (Transform)EditorGUILayout.ObjectField("Target", T.Target, typeof(Transform));
            EditorGUILayout.Space();
            T.FollowType = (FollowTypes) EditorGUILayout.EnumPopup("Follow Type", T.FollowType);
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
                    T.FT_MoveSpeed = EditorGUILayout.Slider("Move Speed", T.FT_MoveSpeed,0f,100f);
                    T.FT_MinDistanceToTarget =
                        EditorGUILayout.FloatField("Min Distance To Target", T.FT_MinDistanceToTarget);
                    EditorGUILayout.Space();
                    T.FT_RotateSpeed = EditorGUILayout.Slider("Rotate Speed", T.FT_RotateSpeed,0f,100f);
                    break;
            }        
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
    
    #endregion
}
