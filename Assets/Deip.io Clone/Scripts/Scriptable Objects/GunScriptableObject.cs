using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/GunSO", fileName = "GunSO_.asset")]
public class GunScriptableObject : ScriptableObject {
    public string gunName;
    public GameObject prefab;
    public int damageAmount = 20;
    public float attackCullDown = 0.5f;
}
