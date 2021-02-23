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

        private readonly CoursePart[] m_parts;
        private readonly CourseItem[] m_items;

        public Course(int partCount)
        {
            UniqueRandom uniqueRandom = new UniqueRandom(0, partCount);
            //まず全部床埋め
            m_parts = Enumerable.Repeat(CoursePart.Floor, partCount).ToArray();
            //どこかに穴を作る
            m_parts[uniqueRandom.Pick()] = CoursePart.Hole;
            //どこかに分断壁を作る
            if (partCount > 10)            
                m_parts[uniqueRandom.Pick()] = CoursePart.BigWall;

            uniqueRandom.Reset();
            //すべてアイテムなしで埋める
            m_items = Enumerable.Repeat(CourseItem.None, partCount).ToArray();
            //金貨を配置
            if (partCount > 10)
                Enumerable.Range(1, 5).ForEach(_ => m_items[uniqueRandom.Pick()] = CourseItem.GoldItem);
            //時間延長アイテムを配置
            if (partCount > 20)
                m_items[uniqueRandom.Pick()] = CourseItem.TimeItem;

        }

        /// <summary>
        /// 何角形か
        /// </summary>
        public int PartCount { get; }

        public IReadOnlyList<CoursePart> PartMap => m_parts;
        public IReadOnlyList<CourseItem> ItemMap => m_items;

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