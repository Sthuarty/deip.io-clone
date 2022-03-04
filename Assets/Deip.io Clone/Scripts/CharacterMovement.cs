using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour {
    [HideInInspector] public bool isFacingRight = true;

    [SerializeField] private float m_MoveSpeed = 5.0f;

    private Vector2 m_Input;
    private Rigidbody2D m_Rigidbody2D;


    private void Start() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        m_Rigidbody2D.velocity = m_Input * m_MoveSpeed;
    }

    public void MoveInputEvent(InputAction.CallbackContext context) => m_Input = context.ReadValue<Vector2>();
}