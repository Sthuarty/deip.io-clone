using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook : MonoBehaviour {
    private Camera m_Camera;
    private Vector2 m_Input;


    private void Start() => m_Camera = Camera.main;

    private void Update() {
        Vector2 inputWorldPosition = m_Camera.ScreenToWorldPoint(m_Input);
        LookAt(inputWorldPosition);
    }

    private void LookAt(Vector2 inputWorldPosition) {
        Vector2 direction = transform.InverseTransformPoint(inputWorldPosition);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }

    public void LookInputEvent(InputAction.CallbackContext context) => m_Input = context.ReadValue<Vector2>();
}
