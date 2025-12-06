using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InGameUILerp : MonoBehaviour
    {
        private Vector2 _vel = Vector2.zero;
        private Vector2 _lastPos = Vector2.zero;

        private RectTransform _rect;

        public Transform TargetCam;
        public float Friction = 0.15f;
        public float Spring = 0.8f;

        void Start()
        {
            _rect = GetComponent<RectTransform>();
        }
        
        void Update()
        {
            _vel += ((Vector2)TargetCam.localPosition - _lastPos) * Friction;
            _vel *= Spring;
            _lastPos = (Vector2)TargetCam.localPosition;

            _rect.localPosition = _vel;
        }
    }
}
