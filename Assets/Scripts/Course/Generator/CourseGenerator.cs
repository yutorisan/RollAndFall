using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityUtility;
using Zenject;

namespace RAF.Course
{
    public interface ICourseGenerator
    {
        /// <summary>
        /// n角形のコースを生成する
        /// </summary>
        /// <param name="n"></param>
        CourseConstractor Generate(int verts, float radius);
    }
    public class CourseGenerator : MonoBehaviour, ICourseGenerator
    {
        [Inject]
        private DiContainer _container;

        [SerializeField]
        private Transform m_parentObject;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_floorPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_smallWallPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_bigWallPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_holePrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_goldItemPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_timeItemPrefab;

        /// <summary>
        /// 地面からアイテムまでの高さ
        /// </summary>
        [SerializeField]
        private float m_itemHeight;

        [SerializeField]
        private int m_num;


        private IReadOnlyDictionary<CoursePart, GameObject> m_partPrefabTable;
        private IReadOnlyDictionary<CourseItem, GameObject> m_itemPrefabTable;

        // Start is called before the first frame update
        void Start()
        {
            m_partPrefabTable = new Dictionary<CoursePart, GameObject>()
            {
                {CoursePart.Hole, m_holePrefab },
                {CoursePart.Floor, m_floorPrefab },
                {CoursePart.SmallWall, m_smallWallPrefab },
                {CoursePart.BigWall, m_bigWallPrefab },
            };
            m_itemPrefabTable = new Dictionary<CourseItem, GameObject>()
            {
                {CourseItem.None, null },
                {CourseItem.GoldItem, m_goldItemPrefab },
                {CourseItem.TimeItem, m_timeItemPrefab }
            };
        }

        public CourseConstractor Generate(int verts, float radius)
        {
            Course course = new Course(verts, radius);

            InstantiateInfo[] instantiateInfos = new InstantiateInfo[verts];
            for (int i = 0; i < verts; i++)
            {
                if (m_partPrefabTable[course.PartMap[i]] != null)
                {
                    instantiateInfos[i] = new InstantiateInfo()
                    {
                        Prefab = m_partPrefabTable[course.PartMap[i]],
                        Position = course.EdgeCenterPoints[i],
                        Rotation = Quaternion.Euler(0, 0, course.EdgeAngles[i].TotalDegree),
                        Parent = m_parentObject,
                        Scale = new Vector3(course.EdgeLength, 0.1f)
                    };
                }
            }

            InstantiateInfo[] instantiateInfo_item = course.ItemPoss.Select(pair => new InstantiateInfo()
            {
                Position = pair.Item1,
                Prefab = pair.Item2 == CourseItem.GoldItem ? m_goldItemPrefab : m_timeItemPrefab,
                Rotation = Quaternion.identity,
                Scale = pair.Item2 == CourseItem.GoldItem ? new Vector3(0.5f,0.5f) : new Vector3(2f, 2f),
                Parent = m_parentObject
            }).ToArray();

            return new CourseConstractor(instantiateInfos, instantiateInfo_item, _container);
        }

        [Button]
        private void Constract(int verts) => Generate(verts, verts * 2).Constract();

    }

    public class InstantiateInfo
    {
        public GameObject Prefab { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Transform Parent { get; set; }
        public Vector3 Scale { get; set; }
    }
}