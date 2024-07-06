using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private bool _canFollow;
    private IEnumerator _move;
    private Camera _mainCamera;

    private void Awake() => _mainCamera = Camera.main;

    private void OnMouseDown() {
        _move = MoveToMouse();
        StartCoroutine(_move);
    }

    private void OnMouseUp()
    {
        if(_move != null) StopCoroutine(_move);
    }

    private IEnumerator MoveToMouse() {
        while (true) {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
            yield return null;
        }
    }
}

