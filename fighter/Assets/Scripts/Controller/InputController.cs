using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public enum TouchDir
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }
    public class InputController : AbstractController
    {
        private Vector2 clickPoint;
        private TouchDir dir;
        
        public override void AdvancedTime(float inDeltaTime)
        {
            base.AdvancedTime(inDeltaTime);

            if (Input.GetKeyDown(KeyCode.A))
            {
                EventManager.Create<EvtDrag>((data, entity) =>
                {
                    data.TouchDirection = TouchDir.LEFT;
                });
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                EventManager.Create<EvtDrag>((data, entity) =>
                {
                    data.TouchDirection = TouchDir.RIGHT;
                });
            }


            if (Input.GetMouseButtonDown(0))
            {
                clickPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if(Input.GetMouseButtonUp(0))
            {
                TouchDir dir = GetTouchDir(clickPoint, Input.mousePosition);

                EventManager.Create<EvtDrag>((data, entity) =>
                {
                    data.TouchDirection = dir;
                });
            }
        }

        private TouchDir GetTouchDir(Vector2 inFrom, Vector2 inTo)
        {
            Vector2 dir = new Vector2(inTo.x - inFrom.x, inTo.y - inFrom.y);
            if(dir.x < 0)
            {
                return TouchDir.LEFT;
            }
            if(dir.x >0)
            {
                return TouchDir.RIGHT;
            }
            return TouchDir.UP;
        }
    }
}
