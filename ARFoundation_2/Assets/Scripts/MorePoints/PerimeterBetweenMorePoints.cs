using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Componet calculate distance between 2 or more points
/// Author: Marinela
/// </summary>

[RequireComponent(typeof(ARRaycastManager))]
public class PerimeterBetweenMorePoints : MonoBehaviour
{
    #region Fields
    [Header("References")]
    public GameObject modularPointLibraryReference;
    public Text perimeterText;
    public LineRenderer lineRenderer;

    [Header("Settings")]
    public float inputCoolDown = 2;
    public int maximPoints = 100;

    private GameObject[] _spawnedPointPoints;
    private float _inputCoolDown;
    private int _hitPointIndex = 0;

    private ARRaycastManager _raycastManager;
    private List<ARRaycastHit> _arRayCastHits;
    private Pose _hitPose;

    #endregion

    #region Methods

    private void Awake()
    {
        _inputCoolDown = Time.time;

        _spawnedPointPoints = new GameObject[maximPoints];

        _raycastManager = GetComponent<ARRaycastManager>();
        _arRayCastHits = new List<ARRaycastHit>();

    }

    private void Update()
    {
        if (!GetTouchPosition(out Vector2 touchPosition))
            return;

        if (_raycastManager.Raycast(touchPosition, _arRayCastHits, TrackableType.PlaneWithinPolygon))
        {
            _hitPose = _arRayCastHits[0].pose;

            if (Time.time > _inputCoolDown)
            {
                DisplayPointsOnScreen();
                TryUpdatePerimeterText();

                _hitPointIndex++;

                _inputCoolDown = Time.time + inputCoolDown;
            }
        }
    }

    private bool GetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

    private void DisplayPointsOnScreen()
    {
        if (Time.time > _inputCoolDown)
        {
            _spawnedPointPoints[_hitPointIndex] = Instantiate(modularPointLibraryReference, _hitPose.position, _hitPose.rotation);

            lineRenderer.positionCount++;

            lineRenderer.SetPosition(_hitPointIndex, _spawnedPointPoints[_hitPointIndex].transform.position);

        }
    }

    private void TryUpdatePerimeterText()
    {
        float perimeter = 0;

        if (_hitPointIndex >= 2)
        {
            for (int i = 0; i <= _hitPointIndex - 1; i++)
            {
                perimeter += Vector3.Distance(_spawnedPointPoints[i].transform.position, _spawnedPointPoints[i + 1].transform.position);
            }
            perimeterText.text = perimeter.ToString();
        }

    }

    #endregion
}
