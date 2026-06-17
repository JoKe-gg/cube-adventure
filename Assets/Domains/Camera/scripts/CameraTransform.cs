using UnityEngine;
public class CameraTransform : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _rotatingSens;
    private float _pitch;
    private void FixedUpdate()
    {
        RotationUpdate();
    }
    private void RotationUpdate()
    {
        _pitch += Input.GetAxis("Mouse Y") * _rotatingSens;
        _pitch = Mathf.Clamp(_pitch, -80, 20);
        transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
    }
}
