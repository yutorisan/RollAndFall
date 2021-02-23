using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtility;

namespace RAF.Course
{
    public interface ICourseGenerator
    {
        /// <summary>
        /// n角形のコースを生成する
        /// </summary>
        /// <param name="n"></param>
        Course Generate(int n);
    }
    public class CourseGenerator : MonoBehaviour, ICourseGenerator
    {
        private static readonly float PartLength = 1f;
        private static readonly float HalfLength = .5f;

        [SerializeField]
        private Transform m_parentObject;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_floorPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_smallWallPrefab;
        [SerializeField, TitleGroup("Prefabs")]
        private GameObject m_bigWallPrefab;
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

        //[SerializeField]
        //private int m_bigWallCount;
        //[SerializeField]
        //private int m_smallWallCount;
        //[SerializeField]
        //private int m_goldCount;

        /// <summary>
        /// n角形の内角の大きさを取得する
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Angle GetOuterAngle(int n) => Angle.Round / n;

        private IReadOnlyDictionary<CoursePart, GameObject> m_partPrefabTable;
        private IReadOnlyDictionary<CourseItem, GameObject> m_itemPrefabTable;

        // Start is called before the first frame update
        void Start()
        {
            m_partPrefabTable = new Dictionary<CoursePart, GameObject>()
            {
                {CoursePart.Hole, null },
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


            Generate(m_num);
        }

        public Course Generate(int n)
        {
            Course course = new Course(n);

            //頂点の座標
            Vector2[] vertexs = new Vector2[n];
            //各辺の中心の座標（辺オブジェクトの生成位置）
            Vector2[] edgeCenterPoints = new Vector2[n];
            //各辺の角度
            Angle[] edgeAngles = new Angle[n];
            //1つの内角の角度
            Angle outerAngle = GetOuterAngle(n);
            //現在のオブジェクトの角度
            Angle direction = Angle.Zero;


            Vector2 point = Vector2.zero;
            for (int i = 0; i < n; i++)
            {
                //オブジェクトを配置する座標を記録
                edgeCenterPoints[i] = point;
                //オブジェクトの角度を記録
                edgeAngles[i] = direction;
                //配置座標から次のパーツの始点を求める（頂点）
                point += HalfLength * direction.Point;
                //頂点座標を記録
                vertexs[i] = point;
                //次のオブジェクトの角度を求める
                direction += outerAngle;
                //次のオブジェクトの中心座標を求める
                point += HalfLength * direction.Point;
            }

            //すべての頂点ベクトルから重心の座標を求める
            Vector2 centerOfGravity = vertexs.Aggregate((sum, v) => sum + v) / n;
            //重心分だけずらして回転の中心座標を合わせる
            edgeCenterPoints = edgeCenterPoints.Select(pt => pt - centerOfGravity).ToArray();

            for (int i = 0; i < n; i++)
            {
                if(m_partPrefabTable[course.PartMap[i]] != null)
                    Instantiate(m_partPrefabTable[course.PartMap[i]],
                                edgeCenterPoints[i],
                                Quaternion.Euler(0, 0, edgeAngles[i].TotalDegree),
                                m_parentObject);
                if (m_itemPrefabTable[course.ItemMap[i]] != null)
                    Instantiate(m_itemPrefabTable[course.ItemMap[i]],
                                edgeCenterPoints[i] + m_itemHeight * (edgeAngles[i] + Angle.FromDegree(90)).Point,
                                Quaternion.Euler(0, 0, edgeAngles[i].TotalDegree),
                                m_parentObject);
            }


            return course;
        }


    }
}