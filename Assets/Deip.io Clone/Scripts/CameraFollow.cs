using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;

    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset;

    private void FixedUpdate() {
        if (target != null) {
            Vector3 desiredPosition = target.position + _offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
