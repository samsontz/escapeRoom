using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterControl : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        // Check for mouse input (useful for desktop/editor)
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000f))
            {
                _agent.destination = _hit.point;
            }
        }

        // Check for touch input (useful for mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Perform action only on touch begin phase (like a "tap")
            if (touch.phase == TouchPhase.Began)
            {
                _ray = _camera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(_ray, out _hit, 1000f))
                {
                    _agent.destination = _hit.point;
                }
            }
        }
    }
}
