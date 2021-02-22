using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtility;

namespace RAF
{
    public interface ICourseGenerator
    {
        /// <summary>
        /// n角形のコースを生成する
        /// </summary>
        /// <param name="n"></param>
        void Generate(int n);
    }
    public class CourseGenerator : MonoBehaviour, ICourseGenerator
    {
        private static readonly float PartLength = 1f;
        private static readonly float HalfLength = .5f;

        [SerializeField]
        private GameObject m_partPrefab;
        [SerializeField]
        private float m_initialRadius;
        [SerializeField]
        private float m_intervalLength;
        [SerializeField]
        private Vector2 m_initialPosition;

        /// <summary>
        /// n角形の内角の大きさを取得する
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Angle GetOuterAngle(int n) => Angle.Round / n;

        public void Generate(int n)
        {
            //1つの内角の角度
            Angle outerAngle = GetOuterAngle(n);
            //オブジェクトを生成する座標
            Vector2 pos = m_initialPosition;
            //現在のオブジェクトの角度
            Angle direction = Angle.Zero;

            for (int i = 0; i < n; i++)
            {
                //オブジェクトを生成
                Instantiate(m_partPrefab, pos, Quaternion.Euler(0, 0, direction.TotalDegree));

                //配置した座標から次のパーツの始点を求める
                pos += HalfLength * direction.Point;
                //次のオブジェクトの角度を求める
                direction += outerAngle;
                //次のオブジェクトの中心座標を求める
                pos += HalfLength * direction.Point;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Generate(80);
        }
    }
}