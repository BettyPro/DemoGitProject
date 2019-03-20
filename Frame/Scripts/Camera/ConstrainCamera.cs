using UnityEngine;

namespace WinterCamera
{
    public class ConstrainCamera : MonoBehaviour
    {

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

        void LateUpdate()
        {
            Vector3 goalPoint = target.position + offset;
            //goalPoint.x = Mathf.Clamp(goalPoint.x, min.x, max.x);
            //goalPoint.y = Mathf.Clamp(goalPoint.y, min.y, max.y);
            //goalPoint.z = Mathf.Clamp(goalPoint.z, min.z, max.z);

            transform.position = Vector3.Lerp(transform.position, goalPoint, smoothing * Time.deltaTime);
            CameraEffects.Instance._cameraScale.SmoothChangeFieldOfViewUpdate();
            CameraEffects.Instance._cameraShake.SmoothChangeCameraShakeUpdate();
        }
    }
}