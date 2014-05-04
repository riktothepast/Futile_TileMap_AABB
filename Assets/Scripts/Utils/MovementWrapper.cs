﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    public struct MovementWrapper
    {
        public Vector2 MovementToTry { get; private set; }
        public Vector2 FurthestAvailableLocationSoFar { get; set; }
        public int NumberOfStepsToBreakMovementInto { get; private set; }
        public bool IsDiagonalMove { get; private set; }
        public Vector2 OneStep { get; private set; }
        public Rectangle BoundingRectangle { get; set; }

        public MovementWrapper(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle) : this()
        {
            MovementToTry = destination - originalPosition;
            FurthestAvailableLocationSoFar = originalPosition;
            NumberOfStepsToBreakMovementInto = (int)(MovementToTry.magnitude * 2) + 1;
            IsDiagonalMove = MovementToTry.x != 0 && MovementToTry.y != 0;
            OneStep = MovementToTry / NumberOfStepsToBreakMovementInto;
            BoundingRectangle = boundingRectangle;
        }

        public static bool AABBCheck(Rectangle rect1, Rectangle rect2)
        {
            return !(rect1.x + rect1.w < rect2.x || rect1.x > rect2.x + rect2.w || rect1.y + rect1.h < rect2.y || rect1.y > rect2.y + rect2.h);
        }
    }
}
