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

    public FollowTypes FollowType = FollowTypes.LocatedAtTarget;
    
    // Located at target (LT)
    public float LT_MoveSpeed;
    public bool LT_Rotating = true;
    public float LT_RotateSpeed;
    
    // Follow For Target (FT)
    
    
    

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
                    break;
                /*case PanelOpenCloseMethods.Instantly:
                    break;
                    
                case PanelOpenCloseMethods.Animator:
                    EditorGUI.indentLevel++;
                    T.Animator = (Animator)EditorGUILayout.ObjectField("Animator", T.Animator, typeof(Animator));
                    T.OpenTriggerAnimatorParameter = EditorGUILayout.TextField("Open", T.OpenTriggerAnimatorParameter);
                    T.CLoseTriggerAnimatorParameter = EditorGUILayout.TextField("Close", T.CLoseTriggerAnimatorParameter);
                    EditorGUI.indentLevel--;
                    break;
                
                case PanelOpenCloseMethods.Slowly:
                    EditorGUI.indentLevel++;
                    T.ShowingSpeed = EditorGUILayout.FloatField("Showing Speed", T.ShowingSpeed);
                    T.Curve = EditorGUILayout.CurveField("Curve", T.Curve);
                    EditorGUI.indentLevel--;
                    break;*/
            }        
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
    
    #endregion
}
