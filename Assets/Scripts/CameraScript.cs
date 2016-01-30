using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float TurnSpeed = 45f;
    private float yawSpeed;
    private float pitchSpeed;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            yawSpeed = -TurnSpeed*Input.GetAxis("Mouse X")*Time.deltaTime;
            pitchSpeed = TurnSpeed*Input.GetAxis("Mouse Y")*Time.deltaTime;
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
