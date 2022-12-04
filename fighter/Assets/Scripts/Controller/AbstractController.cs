using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace InGame
{
    public abstract class AbstractController : MonoBehaviour
    {
        /// <summary>
        /// 활성화 상태
        /// </summary>
        public bool IsActivated { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected AbstractController _parentController;

        public AbstractController ParentController => _parentController;

        /// <summary>
        /// 엔티티 배열
        /// </summary>
        protected List<Entity> _entities = new List<Entity>();

        /// <summary>
        /// 이벤트 데이터 타임
        /// </summary>
        protected List<IEventData> _eventDatas = new List<IEventData>();

        /// <summary>
        /// 이벤트 데이터 타입
        /// </summary>
        protected List<Type> _eventDataTypes = new List<Type>();

        /// <summary>
        /// 이벤트 데이터 맵
        /// </summary>
        protected Dictionary<Type, Action<List<IEventData>>> _eventDataDict = new Dictionary<Type, Action<List<IEventData>>>();

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="inParentController"></param>
        public virtual void InitializeController(AbstractController inParentController = null)
        {
            _parentController = inParentController;
        }

        /// <summary>
        /// 컨트롤러 리셋
        /// </summary>
        public virtual void ClearController()
        {
            SetActiveController(false);
            StopAllCoroutines();
        }

        /// <summary>
        /// 제거 (메모리 반환 처리)
        /// </summary>
        public virtual void FinalizeController()
        {
            ClearController();
            StopAllCoroutines();
            _entities.Clear();
            _eventDatas.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDestroy()
        {
            FinalizeController();
        }

        #region Update
        public virtual void PreAdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }
        }

        public virtual void AdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }

            UpdateEventDatas();
        }

        public virtual void LateAdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }
        }

        #endregion

        #region Event
        /// <summary>
        /// EventData 업데이트
        /// 데이터 구독했다면 지정 메서드로 해당 데이터 수신
        /// </summary>
        private void UpdateEventDatas()
        {
            if (!IsActivated)
            {
                return;
            }

            foreach (Type type in _eventDataTypes)
            {
                if (EventManager.HasData(type))
                {
                    EventManager.GetEntityDataList(type, ref _entities, ref _eventDatas);

                    if (_eventDatas.Count < 0)
                    {
                        continue;
                    }

                    if (_eventDataDict.ContainsKey(type))
                    {
                        _eventDataDict[type]?.Invoke(_eventDatas);
                    }

                    // NOTE @hanwoong 이벤트는 메서드호출후 무조건 삭제 하기때문에 1프레임안에 무조건 끝나야함
                    EventManager.RemoveDataAll(type);
                }
            }
        }
        /// <summary>
        /// 이벤트 데이터의 이벤트 수신
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inAction"></param>
        protected void SubscribeEventData<T>(Action<List<IEventData>> inAction) where T : IEventData
        {
            Type type = typeof(T);
            if (!_eventDataDict.ContainsKey(type))
            {
                _eventDataTypes.Add(type);
                _eventDataDict.Add(type, inAction);
            }
        }

        /// <summary>
        /// 이벤트 데이터의 이벤트 수신 해지
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void UnSubscribeEventData<T>(Action<List<IEventData>> inAction) where T : IEventData
        {
            Type type = typeof(T);
            if (_eventDataDict.ContainsKey(type))
            {
                _eventDataDict.Remove(type);
            }
        }

        #endregion

        #region Start Game
        /// <summary>
        /// 게임 시작 전 준비단계
        /// </summary>
        /// <param name="inData"></param>
        public virtual IEnumerator Co_ReadyToStartGame(params object[] inData) { yield break; }

        /// <summary>
        /// 게임 시작
        /// </summary>
        public virtual void StartGame() { }

        /// <summary>
        /// 게임 시작 직전
        /// </summary>
        public virtual void OnPreStartGame() { }

        /// <summary>
        /// 게임 시작 직후
        /// </summary>
        public virtual void OnPostStartGame() { }
        #endregion 


        /// <summary>
        /// 컨트롤러 활성화 상태 셋
        /// </summary>
        /// <param name="inIsActive"></param>
        public void SetActiveController(bool inIsActive)
        {
            IsActivated = inIsActive;
        }

        /// <summary>
        /// 컨트롤러 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T CreateController<T>() where T : AbstractController
        {
            GameObject go = new GameObject(typeof(T).Name);
            go.transform.SetParent(this.transform);
            return go.AddComponent<T>();
        }

    }
}
