using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook : MonoBehaviour {
    private Camera m_Camera;
    private Vector2 targetPosition = Vector2.zero;


    private void Awake() => m_Camera = Camera.main;

    private void Update() => LookAt(targetPosition);

    private void LookAt(Vector2 position) {
        Vector2 direction = transform.InverseTransformPoint(position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }

    public void LookInputEvent(InputAction.CallbackContext context) { targetPosition = m_Camera.ScreenToWorldPoint(context.ReadValue<Vector2>()); }
}
