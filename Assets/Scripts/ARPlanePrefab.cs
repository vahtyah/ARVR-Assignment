using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlanePrefab : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    public GameObject PlaceablePrefab;
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    public TextMeshProUGUI countTounch;
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        countTounch.SetText(Input.touchCount.ToString());
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition)) return;
        print("Get touch");
        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(PlaceablePrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
