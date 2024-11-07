using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

namespace InGame
{
    public class ExampleTMWithRaycast : MonoBehaviour
    {
        private void Start()
        {
            var s=ServiceManager.Instance;
            var touchManager=s.GetCoreManager<TouchManager>();

            touchManager.Down -= Down;
            touchManager.Down += Down;

            touchManager.Set -= Set;
            touchManager.Set += Set;

            touchManager.Up -= Up;
            touchManager.Up += Up;
        }

        private void Down(TouchData obj)
        {
            if (obj.HitCheck)
            {
                Debug2.LogEditor("Down",obj.Hit.point," name : "+ obj.Hit.transform.name);
            }
        }
        private void Set(TouchData obj)
        {
            if (obj.HitCheck)
            {
                //Debug2.LogEditor("Set", obj.Hit.point, " name : " + obj.Hit.transform.name);
            }
        }
        private void Up(TouchData obj)
        {
            if (obj.HitCheck)
            {
                Debug2.LogEditor("Up", obj.Hit.point, " name : " + obj.Hit.transform.name);
            }
        }
    }
}
