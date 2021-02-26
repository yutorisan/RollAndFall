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
            this.OnTriggerExit2DAsObservable()
                .Where(col => col.transform.tag == "Player")
                .Take(1)
                .Subscribe(_ => m_courseSwitcher.Switch());
        }
    }
}