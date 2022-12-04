using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Entity Target;

        [SerializeField] private float Dist = 10f;
        [SerializeField] private float Height = 5f;
        [SerializeField] private float SmoothRotate = 5f;
        private Transform _tr;
        private Transform _trTarget;
        // Start is called before the first frame update
        void Start()
        {
            _tr = this.GetComponent<Transform>();
            _trTarget = Target.transform;
        }


        private void LateUpdate()
        {
            float currYAngle = Mathf.LerpAngle(_tr.eulerAngles.y, _trTarget.eulerAngles.y, SmoothRotate * Time.deltaTime);
            Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

            _tr.position = _trTarget.position - (rot * Vector3.forward * Dist) + (Vector3.up * Height);

            _tr.LookAt(_trTarget);
        }
    }
}
