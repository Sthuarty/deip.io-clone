using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour {
    private void Awake() {
        gameObject.transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
}