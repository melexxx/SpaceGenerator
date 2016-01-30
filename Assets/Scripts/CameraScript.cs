using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float yawSpeed;
    private float pitchSpeed;
    private Vector3 lastMousePos;

	private void Start() { }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            yawSpeed = -45f*Input.GetAxis("Mouse X")*Time.deltaTime;
            pitchSpeed = 45f*Input.GetAxis("Mouse Y")*Time.deltaTime;
        }
        else
        {
            yawSpeed = 0f;
            pitchSpeed = 0f;
        }
    }

    private void LateUpdate()
    {
        transform.rotation *= Quaternion.Euler(pitchSpeed, yawSpeed, 0f);
    }
}
