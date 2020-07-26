using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Componet calculate distance between 2 point, the second one is movable
/// Author: Marinela
/// </summary>

[RequireComponent(typeof(ARRaycastManager))]
public class DistanceBetweenTwoPointsCalculator : MonoBehaviour
{
    #region Fields
    [Header("References")]
    public GameObject startPointLibraryReference;
    public GameObject endPointLibraryReference;
    public Text distanceText;

    [Header("Settings")]
    public float delayToPlaceNextPoint = 2;
    
    private GameObject _spawnedStartPoint;
    private GameObject _spawnedEndPoint;

    private ARRaycastManager _raycastManager;

    private float _timeToPlaceNextPoint;
    private LineRenderer _lineRendererBetweenPoints;
    private List<ARRaycastHit> _arRayCastHits;//static
    private Pose hitPose;
    #endregion

    #region Methods

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _arRayCastHits = new List<ARRaycastHit>();
        
    }

    private void Update()
    {
        if (!GetTouchPosition(out Vector2 touchPosition))
            return;


        if(_raycastManager.Raycast(touchPosition, _arRayCastHits, TrackableType.PlaneWithinPolygon))
        {
            hitPose = _arRayCastHits[0].pose;

            DisplayPointsOnScreen();

            TryUpdateLineBetweenPoints();

            TryUpdateDistanceText();
        }
    }

    private bool GetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount>0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

    private void DisplayPointsOnScreen()
    {
        if (_spawnedStartPoint == null)
        {
            _spawnedStartPoint = Instantiate(startPointLibraryReference, hitPose.position, hitPose.rotation);

            _lineRendererBetweenPoints = _spawnedStartPoint.GetComponent<LineRenderer>();

            _timeToPlaceNextPoint = Time.time + delayToPlaceNextPoint;

        }
        else
        {
            if (Time.time > _timeToPlaceNextPoint)
            {
                if (_spawnedEndPoint == null)
                {
                    _spawnedEndPoint = Instantiate(endPointLibraryReference, hitPose.position, hitPose.rotation);

                }
                else
                    _spawnedEndPoint.transform.position = hitPose.position;
            }

        }
    }

    private void TryUpdateDistanceText()
    {
        if (_spawnedStartPoint & _spawnedEndPoint)
            distanceText.text = Vector3.Distance(_spawnedStartPoint.transform.position, _spawnedEndPoint.transform.position).ToString();
    }

    private void TryUpdateLineBetweenPoints()
    {
        if (_lineRendererBetweenPoints && _spawnedStartPoint && _spawnedEndPoint)
        {
            _lineRendererBetweenPoints.SetPosition(0, _spawnedStartPoint.transform.position);
            _lineRendererBetweenPoints.SetPosition(1, _spawnedEndPoint.transform.position);
        }
        
    }

    #endregion
}
