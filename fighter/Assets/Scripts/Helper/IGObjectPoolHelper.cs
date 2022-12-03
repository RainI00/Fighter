using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public static class IGObjectPoolHelper
    {
        private static List<Entity> _objectPool = new List<Entity>();
        private static List<Entity> _monsterPool = new List<Entity>();
        
        public static void Add(Entity inEntity)
        {
            _objectPool.Add(inEntity);
            if(inEntity is Monster)
            {
                _monsterPool.Add(inEntity);
            }
        }

        public static void Delete(Entity inEntity)
        {
            _objectPool.Remove(inEntity);
        }

        public static List<Entity> GetAllObject()
        {
            return _objectPool;
        }
        public static Entity GetClosestMonster(Transform inTransform)
        {
            Entity entity = null;
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
