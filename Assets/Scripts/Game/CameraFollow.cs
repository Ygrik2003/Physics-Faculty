using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;

    private void Update()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.transform.position + offset;
        Vector3 smoothCam = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothCam;
    }
}
