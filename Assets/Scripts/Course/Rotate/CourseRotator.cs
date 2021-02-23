using System.Collections;
using System.Collections.Generic;
using RAF.Input;
using UnityEngine;
using UniRx;

namespace RAF.Course.Rotate
{
    public class CourseRotator : MonoBehaviour
    {
        private Rigidbody2D m_rigidbody;
        private float m_rotation;

        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();

            GameCommandFormatter.Instance.GameCommandInputted(GameCommand.RotateLeft)
                                .Subscribe(_ => m_rigidbody.MoveRotation(++m_rotation));
            GameCommandFormatter.Instance.GameCommandInputted(GameCommand.RotateRigth)
                                .Subscribe(_ => m_rigidbody.MoveRotation(--m_rotation));
        }
    }
}