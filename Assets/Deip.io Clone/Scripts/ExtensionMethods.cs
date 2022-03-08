using UnityEngine;

static class ExtensionMethods {
    public static void LookAt2D(this Transform trans, Vector2 targetPosition) {
        Vector2 direction = trans.InverseTransformPoint(targetPosition);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trans.Rotate(0, 0, angle);
    }
}