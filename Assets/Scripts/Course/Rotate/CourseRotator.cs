using System.Collections;
using System.Collections.Generic;
using RAF.Input;
using UnityEngine;
using UniRx;

namespace RAF.Course.Rotate
{
    public class CourseRotator : MonoBehaviour
    {
        [SerializeField]
        private float m_rotateAmount;

        private float m_rotate;
        private Rigidbody2D m_rigidbody2d;

        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody2d = GetComponent<Rigidbody2D>();

            GameCommandFormatter.Instance.GameCommandInputted(GameCommand.RotateLeft)
                                .Subscribe(_ => m_rigidbody2d.MoveRotation(m_rotate += m_rotateAmount));
                                //.Subscribe(_ => transform.Rotate(new Vector3(0, 0, m_rotateAmount)));
            GameCommandFormatter.Instance.GameCommandInputted(GameCommand.RotateRigth)
                                .Subscribe(_ => m_rigidbody2d.MoveRotation(m_rotate -= m_rotateAmount));
                                //.Subscribe(_ => transform.Rotate(new Vector3(0, 0, -m_rotateAmount)));
        }
    }
}