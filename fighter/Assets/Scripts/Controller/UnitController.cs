using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class UnitController :AbstractController
    {
        public override void InitializeController(AbstractController inParentController = null)
        {
            base.InitializeController(inParentController);
            SubscribeEventData<EvtDrag>(OnDrag);
        }
        public override IEnumerator Co_ReadyToStartGame(params object[] inData)
        { 
            foreach(var entity in IGObjectPoolHelper.GetAllObject())
            {
                yield return entity.Co_ReadyToStartGame();
            }
        }
        public override void PreAdvancedTime(float inDeltaTime)
        {
            base.PreAdvancedTime(inDeltaTime);
            foreach (var entity in IGObjectPoolHelper.GetAllObject())
            {
                entity.PreAdvancedTime(inDeltaTime);
            }
        }
        public override void AdvancedTime(float inDeltaTime)
        {
            base.AdvancedTime(inDeltaTime);
            foreach(var entity in IGObjectPoolHelper.GetAllObject())
            {
                entity.AdvancedTime(inDeltaTime);
            }
        }
        public override void LateAdvancedTime(float inDeltaTime)
        {
            base.PreAdvancedTime(inDeltaTime);
            foreach (var entity in IGObjectPoolHelper.GetAllObject())
            {
                entity.AdvancedTime(inDeltaTime);
            }
        }

        private void OnDrag(List<IEventData> inData)
        {
            if(inData == null)
            {
                return;
            }

            for(int i=0;i<inData.Count; ++i)
            {
                if(inData[i] is EvtDrag dt)
                {
                    IGObjectPoolHelper.Player.Move(dt.TouchDirection);
                }
            }
        }
    }
}
