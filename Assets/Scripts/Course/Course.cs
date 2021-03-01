using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtility;
using UnityUtility.Classes;

namespace RAF.Course
{
    public class Course
    {
        private static readonly int SmallWallCount = 4;
        private static readonly int GoldCount = 20;
        private static readonly float ItemInterval = 1.5f;
        private static readonly float ItemHeight = .2f;

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


        private readonly CoursePart[] m_parts;
        private readonly CourseItem[] m_items;

        public Course(int verts, float radius)
        {
            UniqueRandom uniqueRandom = new UniqueRandom(0, verts);
            //まず全部床埋め
            m_parts = Enumerable.Repeat(CoursePart.Floor, verts).ToArray();
            //どこかに穴を作る
            m_parts[uniqueRandom.Pick()] = CoursePart.Hole;
            //どこかに分断壁を作る
            if (verts > 10)            
                m_parts[uniqueRandom.Pick()] = CoursePart.BigWall;

            //uniqueRandom.Reset();
            ////すべてアイテムなしで埋める
            //m_items = Enumerable.Repeat(CourseItem.None, verts).ToArray();
            ////金貨を配置
            //if (verts > 10)
            //    Enumerable.Range(1, 5).ForEach(_ => m_items[uniqueRandom.Pick()] = CourseItem.GoldItem);
            ////時間延長アイテムを配置
            //if (verts > 20)
            //    m_items[uniqueRandom.Pick()] = CourseItem.TimeItem;


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
            EdgeCenterPoints = edgeCenterPoints.Select(pt => pt - centerOfGravity).ToArray();
            EdgeAngles = edgeAngles.ToList();
            EdgeLength = edgeLength;

            int puttableCnt = (int)(edgeLength / ItemInterval);
            float[] putPoints = Enumerable.Range(0, puttableCnt)
                                          .Select(n => n * ItemInterval - edgeLength / 2).ToArray();

            var items = edgeCenterPoints
                                       .Select((v, i) => putPoints.Select(p => v + p * edgeAngles[i].Point))
                                       .SelectMany((v, i) => v.Select(a => ((a + ItemHeight * (edgeAngles[i] + Angle.FromDegree(90)).Point), PartMap[i] == CoursePart.Floor)))
                                       .Where(p => p.Item2)
                                       .Select(v => (v.Item1 - centerOfGravity, CourseItem.GoldItem))
                                       .ToList();

            //if(UnityEngine.Random.value < .3)
            //{
                items[UnityEngine.Random.Range(0, items.Count - 1)] = (items[UnityEngine.Random.Range(0, items.Count - 1)].Item1, CourseItem.TimeItem);
            //}

            ItemPoss = items;
        }

        /// <summary>
        /// 何角形か
        /// </summary>
        public int verts { get; }

        public IReadOnlyList<CoursePart> PartMap => m_parts;
        public IReadOnlyList<CourseItem> ItemMap => m_items;

        public IReadOnlyList<Vector2> EdgeCenterPoints { get; private set; }
        public IReadOnlyList<Angle> EdgeAngles { get; private set; }
        public float EdgeLength { get; private set; }
        public IReadOnlyList<(Vector2, CourseItem)> ItemPoss;

    }

    public enum CoursePart
    {
        Hole,
        Floor,
        SmallWall,
        BigWall
    }

    public enum CourseItem
    {
        None,
        GoldItem,
        TimeItem
    }
}