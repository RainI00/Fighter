using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public static class IGObjectPoolHelper
    {
        private static List<Unit> _objectPool = new List<Unit>();
        private static List<Unit> _monsterPool = new List<Unit>();
        public static Player Player;
        public static void Add(Unit inUnit)
        {
            _objectPool.Add(inUnit);
            if(inUnit is Monster)
            {
                _monsterPool.Add(inUnit);
            }

            if(inUnit is Player)
            {
                Player = inUnit as Player;
            }
        }

        public static void Delete(Unit inUnit)
        {
            _objectPool.Remove(inUnit);
        }

        public static List<Unit> GetAllObject()
        {
            return _objectPool;
        }
        public static Unit GetClosestMonster(Transform inTransform)
        {
            Unit unit = null;
            float dis = 0f;
            foreach(var monster in _monsterPool)
            {
                if(unit == null)
                {
                    unit = monster;
                    dis = GetDistance(inTransform, unit.transform);
                    continue;
                }
                if(unit != null)
                {
                    float tempDis = GetDistance(inTransform, monster.transform);
                    if (tempDis < dis)
                    {
                        unit = monster;
                        dis = tempDis;
                    }
                }
            }

            return unit;
        }
        private static float GetDistance(Transform inFrom, Transform inTo)
        {
            return Vector3.Distance(inFrom.position, inTo.position);
        }
    }
}
