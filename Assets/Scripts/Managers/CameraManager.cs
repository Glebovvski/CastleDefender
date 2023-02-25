using Models;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    private GameTimeModel GameTimeModel { get; set; }

    [Inject]
    private void Construct(GameTimeModel gameTimeModel)
    {
        GameTimeModel = gameTimeModel;
    }

    private Vector3 touchStart;
    private float dragSpeed = 20;

    private float zoomSpeed = 0.5f;
    private int minZoom = 2;
    private int maxZoom = 20;

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
        if (Input.touchCount == 1)
        {
            //MOVING
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float speed = dragSpeed * (Time.deltaTime / Time.timeScale);
                Camera.main.transform.position -= new Vector3(touch.position.x * speed, 0, touch.position.y * speed);
            }
        }
        else if (Input.touchCount == 2)
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

    private void Zoom(float v)
    {
        Camera.main.fieldOfView += v * zoomSpeed;

        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
    }
}
