using BehaviorTree;
using UnityEngine;

public class CheckEnemyInAttackRange : Node {
    private Transform _transform;
    private static int _enemyLayerMask = 1 << 6;


    public CheckEnemyInAttackRange(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        object targetData = GetData("target");

        if (targetData == null) {
            state = NodeState.Failure;
            return state;
        }

        Transform target = (Transform)targetData;
        float distanceToEnemy = Vector2.SqrMagnitude(target.position - _transform.position);

        if (distanceToEnemy <= NPCEnemyBT.distanceToStartAttacking) {
            state = NodeState.Success;
            return state;
        }

        state = NodeState.Failure;
        return state;
    }
}
