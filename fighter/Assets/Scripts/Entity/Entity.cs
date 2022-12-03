using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Entity : MonoBehaviour
    {
        protected virtual IEnumerator Co_ReadyToStartGame()
        {
            yield return null;
        }

        protected virtual void Clear()
        {

        }
        
        public virtual void AdvancedTime(float inDeltatime)
        {

        }

    }
}
