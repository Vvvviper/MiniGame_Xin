using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[ExecuteInEditMode]
public class CameraInfo : MonoBehaviour
{
    [SerializeField]
    private CameraInfo parentCamera;
    [Tooltip("Order = -1 means it's a root camera")]
    [SerializeField]
    private int CameraRootOrder = -1;
    private bool isRootCamera => CameraRootOrder != -1;
    private int cameraPriority;
    private void Start()
    {
        SetPriority(GetComponent<CinemachineVirtualCamera>().Priority);
    }


    public void SetPriority(int priority)
    {
        cameraPriority = priority;
        this.GetComponent<CinemachineVirtualCamera>().Priority = priority;
    }

    public CameraInfo GetParentCamera()
    {
        return parentCamera;
    }

    public int cameraRootOrder()
    {
        if (isRootCamera)
            return CameraRootOrder;
        Debug.LogWarning("Non Root Tree is in List");
        return -1;
    }

    void Update()
    {

#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            if (parentCamera == null)
            {
                parentCamera = this.GetComponentInParent<CameraInfo>();
            }
            if (parentCamera.gameObject == this.gameObject && isRootCamera == false)
            {
                Debug.LogWarning(this.gameObject.name + " assign it self as parent");
            }
        }
#endif


    }
}
