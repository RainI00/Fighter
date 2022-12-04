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
                        Move(TouchDir.LEFT);
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (state != State.Move)
                        Move(TouchDir.RIGHT);
                }

                _targetPosition = new Vector3(Target.transform.position.x, _tr.position.y, Target.transform.position.z);
                _tr.LookAt(_targetPosition);
            }
        }

        private void Move(TouchDir inDir)
        {
            if (Target == null) return;
            StartCoroutine(Co_Move(inDir));

        }

        private IEnumerator Co_Move(TouchDir inDir)
        {
            state = State.Move;
            
            float dir = 1;
            float time = 0f;
            if (inDir == TouchDir.RIGHT)
            {
                dir = -1;
            }
            while(time < 1f)
            {
                time += Time.deltaTime * speed;
                _tr.RotateAround(Target.transform.position, Vector3.up, MaxDegree * speed * Time.deltaTime * dir);
                yield return new WaitForFixedUpdate();
            }
            state = State.None;
        }
    }
}
