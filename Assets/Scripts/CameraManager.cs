using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq.Expressions;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Tooltip("Drag Root Camera in order")]
    private List<CameraTreeNode<CameraInfo>> cameraList;
    private int currentCameraRoot = 0;
    private CameraTreeNode<CameraInfo> currentCameraTree;

    [SerializeField]
    private InputAction leftAction;
    [SerializeField]
    private InputAction rightAction;

    void Start()
    {
        CreateCameraTree();

        leftAction.performed += _ => TurnLeft();
        rightAction.performed += _ => TurnRight();
    }

    private void OnEnable()
    {
        leftAction.Enable();
        rightAction.Enable();
    }
    private void OnDisable()
    {
        leftAction.Disable();   
        rightAction.Disable();
    }

    private List<CameraInfo> FindAllCamera() {
        return FindObjectsOfType<CameraInfo>().ToList();
    }

    private void CreateCameraTree()
    {
        CameraTree<CameraInfo> cameraTreeManager = new CameraTree<CameraInfo>();
        foreach (CameraInfo camera in FindAllCamera())
        {
            CameraTreeNode<CameraInfo> current_camera_node = new CameraTreeNode<CameraInfo>(camera, camera.GetParentCamera());
            if (current_camera_node != null)
            {
                cameraTreeManager.AddChild(current_camera_node);
            }
        }
        cameraList = cameraTreeManager.GetTreeList();
        cameraList = OrderList(cameraList);
        
        //cameraTreeManager.PrintTree(cameraList);
    }

    private List<CameraTreeNode<CameraInfo>> OrderList(List<CameraTreeNode<CameraInfo>> list)
    {
        return list.OrderBy(o => o.GetRoot().cameraRootOrder()).ToList();
    }

    private void TurnRight()
    {
        TurnCameraChangePriority(currentCameraRoot, currentCameraRoot = (currentCameraRoot + 1) % cameraList.Capacity);
    }

    private void TurnLeft()
    {
        TurnCameraChangePriority(currentCameraRoot, currentCameraRoot = (currentCameraRoot - 1 + cameraList.Capacity) % cameraList.Capacity);
    }

    private void ZoomIn(CameraInfo zoomedCamera)
    {
        CameraInfo b = currentCameraTree.FindInChild(zoomedCamera);
    }

    private void TurnCameraChangePriority(int previousIndex, int currentIndex)
    {
        cameraList[previousIndex].GetRoot().SetPriority(0);
        cameraList[currentIndex].GetRoot().SetPriority(1);
        currentCameraTree = cameraList[currentIndex];
    }


    void Update()
    {
        
    }
}
