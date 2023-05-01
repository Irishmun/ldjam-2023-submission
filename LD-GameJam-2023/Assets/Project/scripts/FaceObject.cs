using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FaceObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform personalTransformOverride;

    private Transform _transform;

    private void Awake()
    {
        _transform = personalTransformOverride == null ? transform : personalTransformOverride;
    }

    private void LateUpdate()
    {
        Vector3 dir = _transform.position - target.position;

        Vector3 newRotation = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;

        newRotation.x = 0;
        newRotation.z = 0;
        _transform.eulerAngles = newRotation;
    }
}
