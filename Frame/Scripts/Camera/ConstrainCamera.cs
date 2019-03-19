using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WinterCamera;

public class ConstrainCamera : MonoBehaviour {

    public static ConstrainCamera instance;
    public Transform target;
    public Vector3 offset;
    public Vector3 min;
    public Vector3 max;
    public float smoothing = 0.5f;

    private void Awake()
    {
        instance = this;
        CameraEffects.Instance._ConstrainCamera = GetComponent<ConstrainCamera>();
        CameraEffects.Instance._Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 goalPoint = target.position + offset;
        //goalPoint.x = Mathf.Clamp(goalPoint.x, min.x, max.x);
        //goalPoint.y = Mathf.Clamp(goalPoint.y, min.y, max.y);
        //goalPoint.z = Mathf.Clamp(goalPoint.z, min.z, max.z);

        transform.position = Vector3.Lerp(transform.position, goalPoint, smoothing * Time.deltaTime);
    }
}
