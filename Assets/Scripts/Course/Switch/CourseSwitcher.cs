using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RAF.Camera;
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
        private IPlayerPauser m_playerPauser;
        [Inject]
        private ISubCameraFollower m_subCamFollower;

        private int n = 3;
        private float radiusWeight = 1;

        private CourseConstractor disposable;

        private async void Start()
        {
            await Switch();
        }

        public async UniTask Switch()
        {
            //disposable?.DisConstract();
            ++n;
            int verts = n;
            //if (n % 4 == 0) radiusWeight += .5f;
            float radius = n * 2 * radiusWeight;

            disposable = m_courseGenerator.Generate(verts, radius);

            print(n);

            m_playerPauser.Pause();

            await UniTask.WhenAll(disposable.Constract(), m_subCamFollower.ChangeRange(radius));

            m_playerPauser.Resume();
        }
    }
}