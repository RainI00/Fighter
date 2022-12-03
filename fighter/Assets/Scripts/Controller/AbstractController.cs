using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDestroy()
        {
            FinalizeController();
        }
        public virtual void AdvancedTime(float inDeltaTime)
        {

        }

        public virtual void LateAdvancedTime(float inDeltaTime)
        {

        }

        public virtual void PreAdvancedTime(float inDeltaTime)
        {

        }

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
