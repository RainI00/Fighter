using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public enum State
    {
        None,
        Move,
        Attack
    }
    public class Entity : MonoBehaviour
    {
        protected State state = State.None;
        protected virtual void Awake()
        {
            IGObjectPoolHelper.Add(this);
        }
        public virtual IEnumerator Co_ReadyToStartGame()
        {
            yield return null;
        }

        protected virtual void Clear()
        {

        }
        
        public virtual void AdvancedTime(float inDeltatime)
        {

        }
        public virtual void PreAdvancedTime(float inDeltatime)
        {

        }
        public virtual void LateAdvancedTime(float inDeltatime)
        {

        }
    }
}
