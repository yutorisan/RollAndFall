using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace RAF.Course
{
    public class FallDetector : MonoBehaviour
    {
        [Inject]
        private ICourseSwitcher m_courseSwitcher;

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.transform.tag == "Player")
                .Subscribe(_ => m_courseSwitcher.Switch());
        }
    }
}