using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {

        [SerializeField] Transform target;
        [SerializeField] float rotationSensative = 1f;

        private void Update()
        {
            CameraRotation();

        }
        void LateUpdate()
        {
            this.transform.position = target.position;
        }

        void CameraRotation()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(transform.position, Vector3.up, rotationSensative * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(transform.position, Vector3.up, -rotationSensative * Time.deltaTime);
            }
        }
    }
}

