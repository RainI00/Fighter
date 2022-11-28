using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Main : AbstractController
    {
        protected InputController _inputController;

        public virtual void Initialize()
        {
            _inputController = CreateController<InputController>();

            _inputController?.InitializeController();

        }

        protected virtual IEnumerator Co_ReadyToStartGame(params object[] inData)
        {
            yield return _inputController?.Co_ReadyToStartGame();
        }

        public virtual IEnumerator Co_StartNewStage()
        {
            yield return Co_ReadyToStartGame();
        }

        protected virtual IEnumerator Co_StartGame()
        {
            _inputController?.OnPreStartGame();

            _inputController?.StartGame();

            _inputController?.OnPostStartGame();

            SetActiveController(true);

            yield return null;
        }

        private void Update()
        {
            if(IsActivated)
            {
                PreAdvancedTime(Time.deltaTime);
                AdvancedTime(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if(IsActivated)
            {
                LateAdvancedTime(Time.deltaTime);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDeltaTime"></param>
        public override void PreAdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }

            _inputController?.PreAdvancedTime(inDeltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDeltaTime"></param>
        public override void AdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }

            base.AdvancedTime(inDeltaTime);

            _inputController?.AdvancedTime(inDeltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDeltaTime"></param>
        public override void LateAdvancedTime(float inDeltaTime)
        {
            if (!IsActivated)
            {
                return;
            }

            _inputController?.LateAdvancedTime(inDeltaTime);
        }

    }
}
