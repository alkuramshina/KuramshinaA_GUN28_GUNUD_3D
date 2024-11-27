using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera cameraToLookAt;
    [SerializeField] private float rotationSpeed = 3f;

    private void Update()
    {
        transform.rotation =
            Quaternion.Slerp(transform.rotation, cameraToLookAt.transform.rotation, rotationSpeed * Time.deltaTime);
    }
}