using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RAF.Camera
{
    public interface ISubCameraFollower
    {
        UniTask ChangeRange(float radius);
    }
    public class SubCameraFollower : MonoBehaviour, ISubCameraFollower
    {
        private UnityEngine.Camera m_camera;

        private void Start()
        {
            m_camera = GetComponent<UnityEngine.Camera>();
        }
        public UniTask ChangeRange(float radius)
        {
            return DOTween.To(() => m_camera.orthographicSize,
                              v => m_camera.orthographicSize = v,
                              radius, 1f).Play().AsyncWaitForCompletion().AsUniTask();
        }
    }
}