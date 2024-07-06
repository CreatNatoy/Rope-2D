using System.Collections.Generic;
using UnityEngine;

public class VisibleRaycast2D : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LineRenderer _rope;
    [SerializeField] private LayerMask _collMask;
    [SerializeField] private float _minDistanceToAddPoint = 0.1f;
    [SerializeField] private float _maxDistanceToRemovePoint = 0.1f;
    [SerializeField] private float _removeAngleThreshold = 120f;

    private readonly List<Vector3> _ropePositions = new();
    private Vector3 _lastPosition;

    private void Awake() {
        _lastPosition = transform.position; 
        AddPositionToRope(transform.position);
    }

    private void Update()
    {
        UpdateRopePositions();
        DetectCollisionEnter();
        TryRemoveLastPoint();
    }

    private void DetectCollisionEnter()
    {
        var lastSegment = _rope.GetPosition(_ropePositions.Count - 1);
        RaycastHit2D hit = Physics2D.Linecast(_player.position, lastSegment, _collMask);
        if (!hit.collider) return;
        
        if (Vector3.Distance(_lastPosition, hit.point) < _minDistanceToAddPoint) return;
        AddPositionToRope(hit.point);
        _lastPosition = hit.point;
    }

    private void AddPositionToRope(Vector3 positionPoint) => _ropePositions.Add(positionPoint);

    private void UpdateRopePositions()
    {
        _rope.positionCount = _ropePositions.Count + 1;
        _rope.SetPositions(_ropePositions.ToArray());
        _rope.SetPosition(_ropePositions.Count, _player.position);
    }

    private void TryRemoveLastPoint()
    {
        if (_ropePositions.Count < 2) return;

        var lastRopePosition = _ropePositions[^1];
        var secondLastRopePosition = _ropePositions[^2];

        var directionToLastPoint = (lastRopePosition - _player.position).normalized;
        var directionToSecondLastPoint = (secondLastRopePosition - lastRopePosition).normalized;

        var angle = Vector3.Angle(directionToLastPoint, directionToSecondLastPoint);

        if (!(angle > _removeAngleThreshold) &&
            !(Vector3.Distance(_player.position, lastRopePosition) < _maxDistanceToRemovePoint)) return;
        
        _ropePositions.RemoveAt(_ropePositions.Count - 1);
        _lastPosition = _ropePositions[^1];
    }
}
