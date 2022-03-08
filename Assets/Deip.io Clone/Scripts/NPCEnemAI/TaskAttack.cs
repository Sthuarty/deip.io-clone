using BehaviorTree;
using UnityEngine;

public class TaskAttack : Node {
    private Transform _transform;
    private Transform _lastTarget;
    private Character _enemyCharacter;
    private NPCEnemy _thisNPCCharacter;


    public TaskAttack(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");

        if (target != _lastTarget) {
            _enemyCharacter = target.GetComponent<Character>();
            _lastTarget = target;
        }

        if (_thisNPCCharacter == null)
            _thisNPCCharacter = _transform.GetComponent<NPCEnemy>();

        _transform.LookAt2D(target.position);
        _thisNPCCharacter.ShootHandler();

        float distanceToEnemy = Vector2.SqrMagnitude(target.position - _transform.position);
        if (distanceToEnemy > NPCEnemyBT.distanceClosestToTheEnemy) {
            _transform.position = Vector2.MoveTowards(_transform.position, target.position, NPCEnemyBT.speed * Time.deltaTime);
        }

        state = NodeState.Running;
        return state;
    }
}
