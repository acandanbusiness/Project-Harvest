using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoom : MonoBehaviour
{
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float upperLimit;
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;

    private Camera cam;
    private bool moveAlloved;
    private Vector3 touchPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 2)
            {
                //zoomlama buraya
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId) || EventSystem.current.IsPointerOverGameObject(touchZero.fingerId))
                {
                    return;
                }

                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                float difference = currentDistTouch - distTouch;

                Zoom(difference * 0.8f);
            }
            else
            {
                Touch touch = Input.GetTouch(0);

                switch(touch.phase)
                {
                    case TouchPhase.Began:
                        if(EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            moveAlloved = false;
                        }
                        else
                        {
                            moveAlloved = true;
                        }

                        touchPos = cam.ScreenToWorldPoint(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if(moveAlloved)
                        {
                            Vector3 direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                            cam.transform.position += direction;

                            transform.position = new Vector3
                                (
                                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                                transform.position.z
                                );


                        }
                        break;
                
                }
            }
        }
    }


    private void Zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomMin, zoomMax);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3
            ((rightLimit - Mathf.Abs(leftLimit) / 2.0f), (upperLimit - Mathf.Abs(bottomLimit) / 2.0f)),
            new Vector3(rightLimit - leftLimit, upperLimit - bottomLimit));
    }


}
