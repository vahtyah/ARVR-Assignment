using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARPlanePrefab1 : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedObjects = new List<GameObject>();
    public int maxPrefabSpawnCount = 1;
    private int placedPrefabCount;
    public GameObject PlaceablePrefab;
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
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
        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPose);
            }
        }
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(PlaceablePrefab, hitPose.position, hitPose.rotation);
        placedObjects.Add(spawnedObject);
        placedPrefabCount++;
    }
    
    public void SetPrefabType(GameObject prefab)
    {
        PlaceablePrefab = prefab;
    }
}
