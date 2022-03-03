using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {

    private static T _instance;

    public static T Instance => _instance;


    public virtual void OnDestroy() {
        if (_instance == this)
            _instance = null;
    }

    public virtual void Awake() {
        var componentsList = this.gameObject.GetComponents<Component>();

        if (_instance == null) {
            _instance = this as T;

        } else {

            if (componentsList.Length <= 2) {
                Destroy(this.gameObject);

            } else if (componentsList.Length == 3) {
                foreach (Component component in componentsList) {
                    if (component.GetType() == typeof(DoNotDestroyOnLoad))
                        Destroy(this.gameObject);
                }

            } else
                Destroy(this);
        }
    }
}
