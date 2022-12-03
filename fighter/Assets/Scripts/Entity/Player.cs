using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Player : Entity
    {
        private Entity Target;
        private Transform _tr;

        private float range = 10f;
        public override IEnumerator Co_ReadyToStartGame()
        {
            _tr = this.GetComponent<Transform>();
            yield return base.Co_ReadyToStartGame();
            
        }
        public override void AdvancedTime(float inDeltatime)
        {
            base.AdvancedTime(inDeltatime);
            if(Target == null)
            {
                Target =  IGObjectPoolHelper.GetClosestMonster(_tr);
            }
            else
            {
                _tr.LookAt(Target.transform);
            }
        }

        public void Move(int inDir)
        {
            if (Target == null) return;
            if(inDir == 0)
            {

            }
            else if(inDir == 1)
            {

            }
        }
    }
}
