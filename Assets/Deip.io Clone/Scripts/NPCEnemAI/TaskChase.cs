using BehaviorTree;
using UnityEngine;

public class TaskChase : Node {
    private Transform _transform;

    public TaskChase(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");

        if (target != null) {
            float distanceToEnemy = Vector2.SqrMagnitude(target.position - _transform.position);

            if (distanceToEnemy > NPCEnemyBT.distanceClosestToTheEnemy) {
                _transform.position = Vector2.MoveTowards(_transform.position, target.position, NPCEnemyBT.speed * Time.deltaTime);
                _transform.LookAt2D(target.position);
            }

            if (distanceToEnemy > NPCEnemyBT.distanceToStopChasing)
                ClearData("target");
        }

        state = NodeState.Running;
        return state;
    }
}
