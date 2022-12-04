using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Main : AbstractController
    {
        protected InputController _inputController;
        protected UnitController _entityController;

        private List<AbstractController> _controllers = new List<AbstractController>();
        public virtual void Initialize()
        {
            _inputController = CreateController<InputController>();
            _entityController = CreateController<UnitController>();

            _controllers.Add(_inputController);
            _controllers.Add(_entityController);


            foreach(var controller in _controllers)
            {
                controller?.InitializeController();
            }
        }

        protected virtual IEnumerator Co_ReadyToStartGame(params object[] inData)
        {
            foreach(var controller in _controllers)
            {
                yield return controller?.Co_ReadyToStartGame();
            }
        }

        public virtual IEnumerator Co_StartNewStage()
        {
            yield return Co_ReadyToStartGame();
        }

        public virtual IEnumerator Co_StartGame()
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

            foreach (var controller in _controllers)
            {
                controller?.PreAdvancedTime(inDeltaTime);
            }
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

            foreach (var controller in _controllers)
            {
                controller?.AdvancedTime(inDeltaTime);
            }
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

            foreach (var controller in _controllers)
            {
                controller?.LateAdvancedTime(inDeltaTime);
            }
        }

    }
}
