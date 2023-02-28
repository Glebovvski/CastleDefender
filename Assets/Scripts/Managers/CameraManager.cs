using System;
using Models;
using UnityEngine;
using ViewModels;
using Zenject;

public class CameraManager : MonoBehaviour
{
    private GameTimeModel GameTimeModel { get; set; }
    private DefensesViewModel DefensesViewModel { get; set; }

    [Inject]
    private void Construct(GameTimeModel gameTimeModel, DefensesViewModel defensesViewModel)
    {
        GameTimeModel = gameTimeModel;
        DefensesViewModel = defensesViewModel;
    }

    private float dragSpeed = 10;

    private Vector2 worldStartPoint;

    private float zoomSpeed = 0.01f;

    private void Start()
    {
        DefensesViewModel.OnResetCameraClick += Reset;
    }



    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(1))
        {
            float speed = dragSpeed * (Time.deltaTime / Time.timeScale);
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, 0, Input.GetAxis("Mouse Y") * speed);
        }

#elif PLATFORM_IOS || PLATFORM_ANDROID

        // if (Input.touchCount == 1)
        // {
        //     Touch currentTouch = Input.GetTouch(0);

        //     if (currentTouch.phase == TouchPhase.Began)
        //     {
        //         this.worldStartPoint = this.GetWorldPoint(currentTouch.position);
        //     }

        //     if (currentTouch.phase == TouchPhase.Moved)
        //     {
        //         Vector2 worldDelta = this.GetWorldPoint(currentTouch.position) - this.worldStartPoint;

        //         Camera.main.transform.position -= new Vector3(
        //             -worldDelta.x,
        //             0,
        //             -worldDelta.y
        //         );
        //     }
        // }
        if (Input.touchCount == 1)
        {
            //MOVING
            var touch = Input.GetTouch(0);
           // if (touch.phase == TouchPhase.Moved)
            {
                // float speed = dragSpeed * (Time.deltaTime / Time.timeScale);
                float speed = (Time.deltaTime / Time.timeScale);
                Camera.main.transform.position -= new Vector3(touch.deltaPosition.x * speed, 0, touch.deltaPosition.y * speed);
            }
        }
        else
         if (Input.touchCount == 2)
        {
            //ZOOMING
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            Vector2 touch0_prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1_prev = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0_prev - touch1_prev).sqrMagnitude;
            float currMagnitude = (touch0.position - touch1.position).sqrMagnitude;

            float diff = currMagnitude - prevMagnitude;

            Zoom(diff * 0.01f);

        }
#endif

    }

    private void Reset()
    {
        Camera.main.fieldOfView = 90;
        Camera.main.transform.position = new Vector3(0, 7, 0);
    }

    private void Zoom(float v)
    {
        Camera.main.fieldOfView -= v * zoomSpeed;

        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
    }

    private Vector2 GetWorldPoint(Vector2 screenPoint)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out var hit);
        return hit.point;
    }
}
