using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField]
        private Transform m_followObject;

        // Update is called once per frame
        void Update()
        {
            this.transform.position = m_followObject.transform.position;
        }
    }
}