using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RAF.Camera;
using RAF.Inventory;
using RAF.Item;
using RAF.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace RAF.Course
{
    public interface ICourseSwitcher
    {
        UniTask Switch();
    }
    public class CourseSwitcher : MonoBehaviour, ICourseSwitcher
    {
        [Inject]
        private ICourseGenerator m_courseGenerator;
        [Inject]
        private ITimeOperator m_timeOperator;
        [Inject]
        private ISubCameraFollower m_subCamFollower;
        //[Inject]
        //private ICoinInventoryAddable coinaddable;
        //[Inject]
        //private IEffectPlay effectPlay;

        private int n = 4;
        private float radiusWeight = 1;

        private CourseConstractor disposable;

        private void Start()
        {
            Switch().Forget();
        }

        public async UniTask Switch()
        {
            //disposable?.DisConstract();
            ++n;
            int verts = n;
            //if (n % 4 == 0) radiusWeight += .5f;
            float radius = n * 2.5f;

            disposable = m_courseGenerator.Generate(verts, radius);

            m_timeOperator.Pause();

            //Observable.Interval(TimeSpan.FromSeconds(.3f))
            //          .Take(10)
            //          .Subscribe(_ =>
            //          {
            //              coinaddable.Add();
            //              effectPlay.PlayEffect()
            //          });
            await UniTask.WhenAll(disposable.Constract(), m_subCamFollower.ChangeRange(radius));

            m_timeOperator.Resume();
        }
    }
}