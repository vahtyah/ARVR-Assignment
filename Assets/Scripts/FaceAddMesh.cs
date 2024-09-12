using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

public class FaceAddMesh : MonoBehaviour
{
    [SerializeField] private ARFace face = null;

    [FormerlySerializedAs("indices")]
    [SerializeField]
    private int[] mousePointsIndices = new[] { 1, 14, 78, 292 };

    [SerializeField] private float pointerScale = .01f;
    [SerializeField] private GameObject optionalPointerPrefab = null;
    readonly Dictionary<int, Transform> pointers = new Dictionary<int, Transform>();

    private void Awake()
    {
        face.updated += delegate
        {
            for (var i = 0; i < mousePointsIndices.Length; i++)
            {
                var vertexIndex = mousePointsIndices[i];
                var pointer = GetPointer(i);
                pointer.position = face.transform.TransformPoint(face.vertices[vertexIndex]);
            }
        };
    }

    Transform GetPointer(int id)
    {
        if (pointers.TryGetValue(id, out var existing))
        {
            return existing;
        }
        else
        {
            var newPointer = CreateNewPointer();
            pointers[id] = newPointer;
            return newPointer;
        }
    }

    Transform CreateNewPointer()
    {
        if (optionalPointerPrefab == null)
            return Instantiate(optionalPointerPrefab).transform;

        var result = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        result.localScale = Vector3.one * pointerScale;
        return result;
    }
}