using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class InputController : AbstractController
    {
        private Vector2 clickPoint;
        enum TouchDir
        {
            UP,
            DOWN,
            RIGHT,
            LEFT
        }
        private TouchDir dir;
        public override void AdvancedTime(float inDeltaTime)
        {
            base.AdvancedTime(inDeltaTime);
            if(Input.GetMouseButtonDown(0))
            {
                clickPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if(Input.GetMouseButtonUp(0))
            {
                TouchDir dir = GetTouchDir(clickPoint, Input.mousePosition);
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
