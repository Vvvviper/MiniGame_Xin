using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointOfInterest : MonoBehaviour
{
    //This Component make Player zoom in when click on it
    public UnityEvent ZoomIn;
    private void OnMouseDown()
    {
        ZoomIn.Invoke();
    }
}
