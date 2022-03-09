using System.Collections;
using UnityEngine;

public class UIShowHide : MonoBehaviour {

    [SerializeField] private Vector2 positionShow;
    [SerializeField] private Vector2 positionHide;
    [SerializeField] private float travelTime = 2f;

    private RectTransform rect;


    private void Awake() => rect = GetComponent<RectTransform>();
    public void Show(float startDelay = 0.0f) {
        StopAllCoroutines();
        StartCoroutine(Move(positionShow, startDelay));
    }

    public void Hide(float startDelay = 0.0f) {
        StopAllCoroutines();
        StartCoroutine(Move(positionHide, startDelay));
    }

    private IEnumerator Move(Vector2 endTransform, float startDelay = 0.0f) {
        yield return new WaitForSeconds(startDelay);

        Vector2 startTransform = rect.anchoredPosition;

        float t = 0f;
        while (t <= 1.0f) {
            t += Time.deltaTime / travelTime;
            rect.anchoredPosition = Vector2.Lerp(startTransform, endTransform, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


#if UNITY_EDITOR
    public bool debugPosition = false;
    private void Update() { if (debugPosition) Debug.Log($"{rect.anchoredPosition.x}, {rect.anchoredPosition.y}"); }
#endif
}
