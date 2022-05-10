using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : IInputHandling
{
    private Vector3 _dragStartPos;
    private Touch _touch;
    [SerializeField] private Camera mainCam;

    public bool EnableInput = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (mainCam == null)
        {
            Debug.LogError("camera is empty in the InputHandler");
        }
    }
    

    public override Vector3 GetDirectionToTarget(Rigidbody2D rb)
    {
        if(Input.touchCount > 0 && EnableInput)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            else if (_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Stationary)
            {
                return Dragging();
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                var dragDir = DragEnd();
                ONShootFired?.Invoke();
                return DragEnd();
            }
        }
        return DragEnd();
    }

    private Vector3 DragEnd()
    {
        Vector3 dragReleasePos = mainCam.ScreenToWorldPoint(_touch.position);
        dragReleasePos.z = 0f;

        return _dragStartPos - dragReleasePos;
    }

    private Vector3 Dragging()
    {
        Vector3 draggingPos = mainCam.ScreenToWorldPoint(_touch.position);
        draggingPos.z = 0f;

        return _dragStartPos - draggingPos;
    }

    private void DragStart()
    {
        _dragStartPos = mainCam.ScreenToWorldPoint(_touch.position);
        _dragStartPos.z = 0;
    }

  
}
