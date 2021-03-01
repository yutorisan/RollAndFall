using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RAF.Player;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RAF.Item
{
    public interface IEffectPlay
    {
        void PlayEffect(Vector2 bornPosition);
    }
    public class GoldEffect : SerializedMonoBehaviour, IEffectPlay
    {
        [Inject]
        private IObservableGetItem observableGetItem;
        [SerializeField, Required]
        private Image m_image;
        [SerializeField, Required]
        private Canvas canvas;
        [SerializeField, Required]
        private RectTransform targetTransform;

        private ImagePool pool;

        public void PlayEffect(Vector2 bornPosition)
        {
            var img = pool.Rent(bornPosition);
            img.transform
               .DOMove(targetTransform.position, .5f)
               .OnComplete(() => pool.Return(img))
               .Play();
        }

        // Start is called before the first frame update
        void Start()
        {
            pool = new ImagePool(m_image, canvas);

            observableGetItem.GettedItem
                             .Where(item => item is GoldItem)
                             .Cast<IItem, GoldItem>()
                             .Subscribe(g => PlayEffect(RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, g.transform.position)));
        }

        private class ImagePool : ObjectPool<Image>
        {
            private readonly Image image;
            private readonly Canvas parentCanvas;

            public ImagePool(Image prefab, Canvas parent)
            {
                this.image = prefab;
                this.parentCanvas = parent;
            }

            public Image Rent(Vector2 position)
            {
                var obj = Rent();
                obj.rectTransform.position = position;
                return obj;
            }


            protected override Image CreateInstance()
            {
                return Instantiate(image, parentCanvas.transform);
            }
        }
    }
}