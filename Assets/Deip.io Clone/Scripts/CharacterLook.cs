using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook : MonoBehaviour {
    private Camera m_Camera;
    private Vector2 targetPosition = Vector2.zero;


    private void Awake() => m_Camera = Camera.main;

    private void Update() => transform.LookAt2D(targetPosition);

    public void LookInputEvent(InputAction.CallbackContext context) {
        if (m_Camera != null)
            targetPosition = m_Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }
}
