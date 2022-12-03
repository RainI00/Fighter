using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Player : Entity
    {
        private Entity Target;

        public override void AdvancedTime(float inDeltatime)
        {
            base.AdvancedTime(inDeltatime);
            if(Target == null)
            {

            }
        }

        public void Move(int inDir)
        {
            if (Target == null) return;

        }
    }
}
