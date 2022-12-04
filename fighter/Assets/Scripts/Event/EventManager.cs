using System;
using System.Collections.Generic;
using System.Linq;

namespace InGame
{
    /// <summary>
    /// 
    /// </summary>
    public struct Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public int id;

        /// <summary>
        /// 빈 정보
        /// </summary>
        public static Entity Null => new Entity(-1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inId"></param>
        public Entity(int inId)
        {
            id = inId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        public override bool Equals(object compare)
        {
            return this == (Entity)compare;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inOther"></param>
        /// <returns></returns>
        public bool Equals(Entity inOther)
        {
            return inOther.id == id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inL"></param>
        /// <param name="inR"></param>
        /// <returns></returns>
        public static bool operator ==(Entity inL, Entity inR)
        {
            return inL.id == inR.id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inL"></param>
        /// <param name="inR"></param>
        /// <returns></returns>
        public static bool operator !=(Entity inL, Entity inR)
        {
            return inL.id != inR.id;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IEventData
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISharedEventData : IEventData
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Entity 생성과 IEventData 적재를 위한 편의함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static Entity CreateEntityData<T>(this T inData) where T : IEventData
        {
            Entity e = EventManager.CreateEntity();

            if (EventManager.IsEnabled)
            {
                EventManager.AddData(e, inData);
            }

            return e;
        }
    }

    /// <summary>
    /// Mono <-> Entities <-> Engine
    /// 
    /// Mono와 Engine과의 공유공간 데이터들을 관리
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// 
        /// </summary>
        protected static int _nextid = 0;

        /// <summary>
        /// 
        /// </summary>
        public static bool IsEnabled { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected static List<Entity> _removeEntityList = new List<Entity>();

        /// <summary>
        /// entityid, Type, IEventData
        /// </summary>
        protected static Dictionary<Entity, Dictionary<Type, IEventData>> _entityMap;

        /// <summary>
        /// entityid, list of IEventData
        /// </summary>
        protected static Dictionary<Entity, List<IEventData>> _entityList;

        /// <summary>
        /// type, list of IEventData
        /// </summary>
        protected static Dictionary<Type, List<IEventData>> _dataListByType;

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            _entityMap = new Dictionary<Entity, Dictionary<Type, IEventData>>(256);
            _entityList = new Dictionary<Entity, List<IEventData>>(256);
            _dataListByType = new Dictionary<Type, List<IEventData>>(256);

            Clear();
        }

        /// <summary>
        /// 이벤트 데이터를 저장, 실행 여부 설정
        /// </summary>
        /// <param name="inIsEnabled"></param>
        public static void SetEnabled(bool inIsEnabled)
        {
            IsEnabled = inIsEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            if (_entityList == null)
            {
                return;
            }

            _entityMap.Clear();
            _entityList.Clear();
            _dataListByType.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Dispose()
        {
            if (_entityList == null)
            {
                return;
            }

            List<Entity> entityKeys = new List<Entity>(_entityList.Keys);
            foreach (Entity e in entityKeys)
            {
                DestroyEntity(e);
            }

            _entityMap = null;
            _entityList = null;
            _dataListByType = null;
            _nextid = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Entity CreateEntity()
        {
            return new Entity()
            {
                id = _nextid++
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEntity"></param>
        public static void DestroyEntity(Entity inEntity)
        {
            if (!_entityMap.ContainsKey(inEntity))
            {
                return;
            }

            Dictionary<Type, IEventData> dataMap = _entityMap[inEntity];
            List<IEventData> dataList = _entityList[inEntity];

            for (int i = 0; i < dataList.Count; ++i)
            {
                IEventData data = dataList[i];
                Type type = data.GetType();

                List<IEventData> listByType = _dataListByType[type];
                listByType.Remove(data);

                (data as IDisposable)?.Dispose();
            }

            dataMap.Clear();
            dataList.Clear();

            _entityMap.Remove(inEntity);
            _entityList.Remove(inEntity);
        }

        /// <summary>
        /// 해당 데이터가 존재하는 여부
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasData<T>() where T : IEventData
        {
            return HasData(typeof(T));
        }

        /// <summary>
        /// 해당 데이터가 존재하는 여부
        /// </summary>
        /// <param name="inType"></param>
        /// <returns></returns>
        public static bool HasData(Type inType)
        {
            return _dataListByType != null && _dataListByType.ContainsKey(inType) && _dataListByType[inType].Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T GetData<T>() where T : IEventData
        {
            Type type = typeof(T);
            if (_dataListByType != null && _dataListByType.ContainsKey(type) && _dataListByType[type].Count > 0)
            {
                return (T)_dataListByType[type][0];
            }
            return default;
        }

        /// <summary>
        /// 주어진 타입의 모든 데이터 목록 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetDataList<T>() where T : IEventData
        {
            Type type = typeof(T);

            if (_dataListByType != null && _dataListByType.ContainsKey(type) && _dataListByType[type].Count > 0)
            {
                List<IEventData> listByType = _dataListByType[type];
                return listByType.Cast<T>().ToList();
            }

            return null;
        }

        /// <summary>
        /// 주어진 타입에 해당하는 Entity와 IEventData 목록을 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outEntities"></param>
        /// <param name="outData"></param>
        public static void GetEntityDataList<T>(ref List<Entity> outEntities, ref List<T> outData)
        {
            if (outEntities == null)
            {
                outEntities = new List<Entity>();
            }

            if (outData == null)
            {
                outData = new List<T>();
            }

            outEntities.Clear();
            outData.Clear();

            Type targetType = typeof(T);

            foreach (Entity entity in _entityMap.Keys)
            {
                Dictionary<Type, IEventData> map = _entityMap[entity];
                foreach (Type type in map.Keys)
                {
                    if (type == targetType)
                    {
                        outEntities.Add(entity);
                        outData.Add((T)map[type]);
                    }
                }
            }
        }

        /// <summary>
        /// 주어진 타입에 해당하는 Entity와 IEventData 목록을 반환
        /// </summary>
        /// <param name="inType"></param>
        /// <param name="outEntities"></param>
        /// <param name="outData"></param>
        public static void GetEntityDataList(Type inType, ref List<Entity> outEntities, ref List<IEventData> outData)
        {
            if (outEntities == null)
            {
                outEntities = new List<Entity>();
            }

            if (outData == null)
            {
                outData = new List<IEventData>();
            }

            outEntities.Clear();
            outData.Clear();

            Dictionary<Entity, Dictionary<Type, IEventData>> sortedEntityMap = _entityMap.OrderBy(x => x.Key.id).ToDictionary(x => x.Key, x => x.Value);

            foreach (Entity entity in sortedEntityMap.Keys)
            {
                Dictionary<Type, IEventData> map = sortedEntityMap[entity];
                foreach (Type type in map.Keys)
                {
                    if (type == inType)
                    {
                        outEntities.Add(entity);
                        outData.Add(map[type]);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inEntity"></param>
        /// <returns></returns>
        public static T GetData<T>(Entity inEntity) where T : class, IEventData
        {
            if (_entityMap == null)
            {
                UnityEngine.Debug.LogError($"Entity Map이 초기화되지 않았습니다.");
                return null;
            }

            if (!_entityMap.ContainsKey(inEntity))
            {
                UnityEngine.Debug.LogError($"Entity({inEntity})가 존재하지 않습니다!");
                return null;
            }

            Dictionary<Type, IEventData> dataMap = _entityMap[inEntity];
            List<IEventData> dataList = _entityList[inEntity];

            Type type = typeof(T);

            if (!dataMap.ContainsKey(type))
            {
                UnityEngine.Debug.LogError($"Entity({inEntity})에는 {type.Name}이 존재하지 않습니다!");
                return null;
            }

            return (T)dataMap[type];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEntity"></param>
        /// <param name="inData"></param>
        public static void AddData(Entity inEntity, IEventData inData)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (_entityMap == null)
            {
                return;
            }

            if (!_entityMap.ContainsKey(inEntity))
            {
                _entityMap.Add(inEntity, new Dictionary<Type, IEventData>());
                _entityList.Add(inEntity, new List<IEventData>());
            }

            Dictionary<Type, IEventData> dataMap = _entityMap[inEntity];
            List<IEventData> dataList = _entityList[inEntity];

            Type type = inData.GetType();
            if (dataMap.ContainsKey(type))
            {
                UnityEngine.Debug.LogError($"{type.Name}은 이미 존재하는 데이터입니다.");
                return;
            }

            if (!_dataListByType.ContainsKey(type))
            {
                _dataListByType.Add(type, new List<IEventData>());
            }

            List<IEventData> listByType = _dataListByType[type];

            dataMap.Add(type, inData);
            dataList.Add(inData);
            listByType.Add(inData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEntity"></param>
        /// <param name="InDataType"></param>
        public static void RemoveData<T>(Entity inEntity) where T : IEventData
        {
            if (_entityMap == null)
            {
                UnityEngine.Debug.LogError($"Entity Map이 초기화되지 않았습니다.");
                return;
            }

            if (!_entityMap.ContainsKey(inEntity))
            {
                UnityEngine.Debug.LogError($"Entity({inEntity})가 존재하지 않습니다!");
                return;
            }

            Dictionary<Type, IEventData> dataMap = _entityMap[inEntity];
            List<IEventData> dataList = _entityList[inEntity];

            Type type = typeof(T);

            if (!dataMap.ContainsKey(type))
            {
                UnityEngine.Debug.LogError($"Entity({inEntity})에는 {type.Name}이 존재하지 않습니다!");
                return;
            }

            List<IEventData> listByType = _dataListByType[type];

            listByType.Remove(dataMap[type]);
            dataList.Remove(dataMap[type]);
            dataMap.Remove(type);

            // 빈 Entity라면
            if (dataList.Count == 0)
            {
                // 제거
                DestroyEntity(inEntity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inType"></param>
        public static void RemoveDataAll(Type inType)
        {
            foreach (var entityPair in _entityMap)
            {
                Dictionary<Type, IEventData> map = entityPair.Value;

                foreach (var mapPair in map)
                {
                    if (inType == mapPair.Key)
                    {
                        _removeEntityList.Add(entityPair.Key);
                    }
                }
            }

            foreach (Entity e in _removeEntityList)
            {
                DestroyEntity(e);
            }
            _removeEntityList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inCreationCallback"></param>
        public static T Create<T>(Action<T, Entity> inCreationCallback = null) where T : IEventData, new()
        {
            if (!IsEnabled)
            {
                return default;
            }

#if UNITY_EDITOR && DEBUG_MATCH3
            UnityEngine.Debug.Log($"[IGEventManager] [Create <color=red>{typeof(T)}</color>]");
#endif

            T ret = new T();
            Entity entity = ret.CreateEntityData();
            inCreationCallback?.Invoke(ret, entity);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inData"></param>
        public static void Create(IEventData inData)
        {
            if (IsEnabled)
            {
                inData.CreateEntityData();
            }
        }
    }
}
