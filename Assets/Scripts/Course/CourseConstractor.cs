using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtility;
using UnityUtility.Linq;
using Zenject;

namespace RAF.Course
{
    public class CourseConstractor
    {
        private InstantiateInfo[] m_infos;
        private InstantiateInfo[] m_itemInfos;
        private IReadOnlyList<GameObject> m_constractedGOs;
        private DiContainer container;
        public CourseConstractor(InstantiateInfo[] info, InstantiateInfo[] itemInfo, DiContainer diContainer)
        {
            m_infos = info;
            m_itemInfos = itemInfo;
            this.container = diContainer;
        }

        public UniTask Constract()
        {
            m_constractedGOs = m_infos.Select(info =>
                                              {
                                                  var go = container.InstantiatePrefab(info.Prefab, info.Position, info.Rotation, info.Parent);
                                                  go.transform.localScale = info.Scale;
                                                  return go;
                                              })
                                      .ToList();
            return m_constractedGOs.Select(go => DOTween.Sequence()
                                                        .Join(go.transform.DOLocalRotate(90.AsZ(), 1f).SetRelative().From())
                                                        .Join(go.GetComponent<SpriteRenderer>().material.DOColor(Color.clear, 1f).SetRelative().From()))
                                   .Select(sq => sq.Play().AsyncWaitForCompletion().AsUniTask())
                                   .WhenAll()
                                   .ContinueWith(() =>
                                   {
                                       var item = m_itemInfos.Select(info =>
                                       {
                                           var go = Object.Instantiate(info.Prefab, info.Position, info.Rotation, info.Parent);
                                           go.transform.localScale = info.Scale;
                                           return go;
                                       });
                                       return item.Select(go => DOTween.Sequence()
                                                                .Join(go.transform.DOScale(new Vector2(3, 3), 1f).From())
                                                                .Join(go.GetComponent<SpriteRenderer>().material.DOColor(Color.clear, 1f).SetRelative().From())
                                                                .Play().AsyncWaitForCompletion().AsUniTask())
                                                    .WhenAll();
                                                                
                                           
                                   });
        }

        public void DisConstract()
        {
            m_constractedGOs.ForEach(go => Object.Destroy(go));
        }
    }
}