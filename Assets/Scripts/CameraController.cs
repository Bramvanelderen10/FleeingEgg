using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float _dampTime = 0.15f;
    private Vector3 _velocity = Vector3.zero;
    public Transform _target;
    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            Vector3 point = _camera.WorldToViewportPoint(_target.position);
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _dampTime);
        }

    }
}
