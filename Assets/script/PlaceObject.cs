using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject _placementIndicator;
    [SerializeField] private GameObject _placedPrefab;
    [SerializeField] private InputAction touchInput;

    ARRaycastManager _aRRaycastManager;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    bool _objectPlaced = false; // Flag to track if the object has been placed

    private void Awake()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
        touchInput.performed += _ => TryPlaceObject(); // Subscribe to the touch input
        _placementIndicator.SetActive(false); // Initially hide the placement indicator
        _placedPrefab.SetActive(false); // Ensure _placedPrefab starts inactive
    }

    private void OnEnable()
    {
        touchInput.Enable();
    }

    private void OnDisable()
    {
        touchInput.Disable();
    }

    private void Update()
    {
        // Only perform the raycast if the object has not been placed yet
        if (!_objectPlaced && _aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), _hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;
            _placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation); // Update placement indicator position and rotation
            if (!_placementIndicator.activeInHierarchy)
                _placementIndicator.SetActive(true); // Show the placement indicator if it's not already active
        }
    }

    private void TryPlaceObject()
    {
        if (!_objectPlaced && _placementIndicator.activeInHierarchy)
        {
            _placedPrefab.transform.SetPositionAndRotation(_placementIndicator.transform.position, _placementIndicator.transform.rotation);
            _placedPrefab.SetActive(true);
            _objectPlaced = true;

            _placementIndicator.SetActive(false);
        }
    }
}

