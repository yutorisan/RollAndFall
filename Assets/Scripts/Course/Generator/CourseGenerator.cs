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

        /// <summary>
        /// n角形の外角の大きさを取得する
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Angle GetOuterAngle(int n) => Angle.Round / n;
        /// <summary>
        /// n角形の内角の大きさを取得する
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Angle GetInnerAngle(int n) => Angle.FromDegree(180f * (n - 2) / n);
        /// <summary>
        /// 1辺の長さを取得する
        /// </summary>
        /// <param name="n"></param>
        /// <param name="radius">頂点から重心までの距離</param>
        /// <returns></returns>
        private static float GetOneSideLength(int n, float radius) => 2 * radius * (GetInnerAngle(n) / 2).Cos;

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
            Course course = new Course(verts);

            //頂点の座標
            Vector2[] vertexs = new Vector2[verts];
            //各辺の中心の座標（辺オブジェクトの生成位置）
            Vector2[] edgeCenterPoints = new Vector2[verts];
            //各辺の角度
            Angle[] edgeAngles = new Angle[verts];
            //1つの内角の角度
            Angle outerAngle = GetOuterAngle(verts);
            //現在のオブジェクトの角度
            Angle direction = Angle.Zero;
            //1辺の長さ
            float edgeLength = GetOneSideLength(verts, radius);
            float halfLengeh = edgeLength / 2;

            Vector2 point = Vector2.zero;
            for (int i = 0; i < verts; i++)
            {
                //オブジェクトを配置する座標を記録
                edgeCenterPoints[i] = point;
                //オブジェクトの角度を記録
                edgeAngles[i] = direction;
                //配置座標から次のパーツの始点を求める（頂点）
                point += halfLengeh * direction.Point;
                //頂点座標を記録
                vertexs[i] = point;
                //次のオブジェクトの角度を求める
                direction += outerAngle;
                //次のオブジェクトの中心座標を求める
                point += halfLengeh * direction.Point;
            }

            //すべての頂点ベクトルから重心の座標を求める
            Vector2 centerOfGravity = vertexs.Aggregate((sum, v) => sum + v) / verts;
            //重心分だけずらして回転の中心座標を合わせる
            edgeCenterPoints = edgeCenterPoints.Select(pt => pt - centerOfGravity).ToArray();


            InstantiateInfo[] instantiateInfos = new InstantiateInfo[verts];
            for (int i = 0; i < verts; i++)
            {
                if (m_partPrefabTable[course.PartMap[i]] != null)
                {
                    instantiateInfos[i] = new InstantiateInfo()
                    {
                        Prefab = m_partPrefabTable[course.PartMap[i]],
                        Position = edgeCenterPoints[i],
                        Rotation = Quaternion.Euler(0, 0, edgeAngles[i].TotalDegree),
                        Parent = m_parentObject,
                        Scale = new Vector3(edgeLength, 0.1f)
                    };
                }
            }

            return new CourseConstractor(instantiateInfos, _container);
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