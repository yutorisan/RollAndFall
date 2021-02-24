using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace RAF.Course
{
    public interface ICourseSwitcher
    {
        void Switch();
    }
    public class CourseSwitcher : MonoBehaviour, ICourseSwitcher
    {
        [Inject]
        private ICourseGenerator m_courseGenerator;

        private int n = 3;

        public void Switch()
        {
            m_courseGenerator.Generate(++n);
        }
    }
}