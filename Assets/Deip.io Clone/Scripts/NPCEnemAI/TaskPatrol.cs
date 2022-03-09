using BehaviorTree;
using Photon.Pun;
using UnityEngine;

public class TaskPatrol : Node, IPunObservable {
    private Transform _transform;
    private Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _isWaiting = false;


    public TaskPatrol(Transform transform) {
        _transform = transform;
        _waypoints = Waypoints.Instance.list;
        _currentWaypointIndex = Random.Range(0, _waypoints.Length);
    }

    public override NodeState Evaluate() {
        if (_isWaiting) {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
                _isWaiting = false;
        } else {
            Transform waypoint = _waypoints[_currentWaypointIndex];
            if (waypoint != null) {
                if (Vector2.SqrMagnitude(waypoint.position - _transform.position) < 0.01f) {
                    _transform.position = waypoint.position;
                    _waitCounter = 0f;
                    _isWaiting = true;

                    _currentWaypointIndex = Random.Range(0, _waypoints.Length);
                } else {
                    _transform.position = Vector2.MoveTowards(_transform.position, waypoint.position, NPCEnemyBT.speed * Time.deltaTime);
                    _transform.LookAt2D(waypoint.position);
                }
            }
        }

        state = NodeState.Running;
        return state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) stream.SendNext(_currentWaypointIndex);
        else _currentWaypointIndex = (int)stream.ReceiveNext();
    }
}
