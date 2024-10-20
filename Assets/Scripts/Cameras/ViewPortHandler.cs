using System;
using System.Collections;
using UnityEngine;


namespace BallBlast.Cameras
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ViewPortHandler : MonoBehaviour
    {
        public static ViewPortHandler Instance { get; private set; }
        public enum Constraint { Landscape, Portrait }

        [SerializeField]
        private float heightOffset;
        [SerializeField]
        private Color wireColor = Color.white;
        [SerializeField]
        private float unitsSize = 1;
        [SerializeField]
        private Constraint constraint = Constraint.Portrait;
        [SerializeField]
        private new Camera camera;

        private float _width;
        private float _height;
        //*** bottom screen
        private Vector3 _bl;
        private Vector3 _bc;
        private Vector3 _br;
        //*** middle screen
        private Vector3 _ml;
        private Vector3 _mc;
        private Vector3 _mr;
        //*** top screen
        private Vector3 _tl;
        private Vector3 _tc;
        private Vector3 _tr;

        public float Width => _width;
        public float Height => _height;
        public Vector3 BottomLeft => _bl;
        public Vector3 BottomCenter => _bc;
        public Vector3 BottomRight => _br;
        public Vector3 MiddleLeft => _ml;
        public Vector3 MiddleCenter => _mc;
        public Vector3 MiddleRight => _mr;
        public Vector3 TopLeft => _tl;
        public Vector3 TopCenter => _tc;
        public Vector3 TopRight => _tr;

        private void Awake()
        {
            Instance = this;
            transform.localPosition = Vector3.zero;
            camera = GetComponent<Camera>();
            ComputeResolution();
        }

        private void ComputeResolution()
        {
            float leftX, rightX, topY, bottomY;

            if (constraint == Constraint.Landscape)
            {
                camera.orthographicSize = 1f / camera.aspect * unitsSize / 2f;
            }
            else
            {
                camera.orthographicSize = unitsSize / 2f;
            }

            _height = 2f * camera.orthographicSize;
            _width = _height * camera.aspect;

            float cameraX, cameraY;
            cameraX = 0f;
            cameraY = _height / 2 + heightOffset;
            transform.localPosition = new Vector3(cameraX, cameraY, -10f);

            leftX = cameraX - _width / 2;
            rightX = cameraX + _width / 2;
            topY = cameraY + _height / 2;
            bottomY = cameraY - _height / 2;

            //*** bottom
            _bl = new Vector3(leftX, bottomY, 0);
            _bc = new Vector3(cameraX, bottomY, 0);
            _br = new Vector3(rightX, bottomY, 0);
            //*** middle
            _ml = new Vector3(leftX, cameraY, 0);
            _mc = new Vector3(cameraX, cameraY, 0);
            _mr = new Vector3(rightX, cameraY, 0);
            //*** top
            _tl = new Vector3(leftX, topY, 0);
            _tc = new Vector3(cameraX, topY, 0);
            _tr = new Vector3(rightX, topY, 0);
        }

#if UNITY_EDITOR
        private void Update()
        {
            ComputeResolution();
        }
#endif

        void OnDrawGizmos()
        {
            Gizmos.color = wireColor;

            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            if (camera.orthographic)
            {
                float spread = camera.farClipPlane - camera.nearClipPlane;
                float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
            }
            Gizmos.matrix = temp;
        }

    }
}