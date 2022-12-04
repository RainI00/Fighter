using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    
    public class Player : Entity
    {
        private Entity Target;
        private Transform _tr;
        private Vector3 _targetPosition;
        [SerializeField] private float range = 5f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float MaxDegree = 3f;
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
                if(Input.GetKeyDown(KeyCode.A))
                {
                    if(state != State.Move)
                        Move(0);
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (state != State.Move)
                        Move(1);
                }

                _targetPosition = new Vector3(Target.transform.position.x, _tr.position.y, Target.transform.position.z);
                _tr.LookAt(_targetPosition);
            }
        }

        private void Move(int inDir)
        {
            if (Target == null) return;
            StartCoroutine(Co_Move(inDir));

        }

        private IEnumerator Co_Move(int inDir)
        {
            state = State.Move;
            
            float dir = 1;
            float angle = 0f;
            if (inDir == 1) dir = -1;
            while(angle < MaxDegree)
            {
                angle += Time.deltaTime * speed;
                _tr.RotateAround(Target.transform.position, Vector3.up, angle * dir);
                yield return new WaitForFixedUpdate();
            }
            state = State.None;
        }
    }
}
