using System.Collections.Generic;
using BehaviorTree;
public class NPCEnemyBT : Tree {
    public UnityEngine.Transform[] waypoints;
    public static float speed = 2f;
    public static float distanceToStartChasing = 5.0f;
    public static float distanceClosestToTheEnemy = 5f;
    public static float distanceToStartAttacking = 13.25f;
    public static float distanceToStopChasing = 15.0f;


    protected override Node SetupTree() {
        Node root = new Selector(new List<Node> {
            new Sequence( new List<Node> {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),

            new Sequence( new List<Node> {
                new CheckEnemyInFOVRange(transform),
                new TaskChase(transform),
            }),

            new TaskPatrol(transform, waypoints),
        });

        return root;
    }
}
