using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public static class IGObjectPoolHelper
    {
        private static List<Unit> _objectPool = new List<Unit>();
        private static List<Unit> _monsterPool = new List<Unit>();
        
        public static void Add(Unit inEntity)
        {
            _objectPool.Add(inEntity);
            if(inEntity is Monster)
            {
                _monsterPool.Add(inEntity);
            }
        }

        public static void Delete(Unit inEntity)
        {
            _objectPool.Remove(inEntity);
        }

        public static List<Unit> GetAllObject()
        {
            return _objectPool;
        }
        public static Unit GetClosestMonster(Transform inTransform)
        {
            Unit entity = null;
            float dis = 0f;
            foreach(var monster in _monsterPool)
            {
                if(entity == null)
                {
                    entity = monster;
                    dis = GetDistance(inTransform, entity.transform);
                    continue;
                }
                if(entity != null)
                {
                    float tempDis = GetDistance(inTransform, monster.transform);
                    if (tempDis < dis)
                    {
                        entity = monster;
                        dis = tempDis;
                    }
                }
            }

            return entity;
        }
        private static float GetDistance(Transform inFrom, Transform inTo)
        {
            return Vector3.Distance(inFrom.position, inTo.position);
        }
    }
}
