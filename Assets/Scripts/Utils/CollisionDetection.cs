using UnityEngine;
using System;
using System.Collections;

/*
 * Code adapted from:
 * http://www.gamedev.net/page/resources/_/technical/game-programming/swept-aabb-collision-detection-and-response-r3084
 */

public class CollisionDetection
{

    // returns true if the Rectanglees are colliding (velocities are not used)
    public static bool AABBCheck(Rectangle rect1, Rectangle rect2)
    {
        return !(rect1.x + rect1.w < rect2.x || rect1.x > rect2.x + rect2.w || rect1.y + rect1.h < rect2.y || rect1.y > rect2.y + rect2.h);
    }

    // returns true if the Rectanglees are colliding (velocities are not used)
    // moveX and moveY will return the movement the rect1 must move to avoid the collision
    public static bool AABB(Rectangle rect1, Rectangle rect2, out float moveX, out float moveY)
    {
        moveX = moveY = 0.0f;

        float l = rect2.x - (rect1.x + rect1.w);
        float r = (rect2.x + rect2.w) - rect1.x;
        float t = rect2.y - (rect1.y + rect1.h);
        float b = (rect2.y + rect2.h) - rect1.y;

        // check that there was a collision
        if (l > 0 || r < 0 || t > 0 || b < 0)
            return false;

        // find the offset of both sides
        moveX = Math.Abs(l) < r ? l : r;
        moveY = Math.Abs(t) < b ? t : b;

        // only use whichever offset is the smallest
        if (Math.Abs(moveX) < Math.Abs(moveY))
			moveX = 0.0f;
        else
            moveY = 0.0f;

        return true;
    }

    // returns a Rectangle the spans both a current Rectangle and the destination Rectangle
    public static Rectangle GetSweptBroadphaseRectangle(Rectangle b)
    {
        Rectangle broadphaseRectangle = new Rectangle(0.0f, 0.0f, 0.0f, 0.0f);

        broadphaseRectangle.x = b.vx > 0 ? b.x : b.x + b.vx;
        broadphaseRectangle.y = b.vy > 0 ? b.y : b.y + b.vy;
        broadphaseRectangle.w = b.vx > 0 ? b.vx + b.w : b.w - b.vx;
        broadphaseRectangle.h = b.vy > 0 ? b.vy + b.h : b.h - b.vy;

        return broadphaseRectangle;
    }

    // performs collision detection on moving Rectangle rect1 and static Rectangle rect2
    // returns the time that the collision occured (where 0 is the start of the movement and 1 is the destination)
    // getting the new position can be retrieved by Rectangle.x = Rectangle.x + Rectangle.vx * collisiontime
    // normalx and normaly return the normal of the collided surface (this can be used to do a response)
    public static float SweptAABB(Rectangle rect1, Rectangle rect2, out float normalx, out float normaly)
    {
        float xInvEntry, yInvEntry;
        float xInvExit, yInvExit;

        // find the distance between the objects on the near and far sides for both x and y
        if (rect1.vx > 0.0f)
        {
            xInvEntry = rect2.x - (rect1.x + rect1.w);
            xInvExit = (rect2.x + rect2.w) - rect1.x;
        }
        else
        {
            xInvEntry = (rect2.x + rect2.w) - rect1.x;
            xInvExit = rect2.x - (rect1.x + rect1.w);
        }

        if (rect1.vy > 0.0f)
        {
            yInvEntry = rect2.y - (rect1.y + rect1.h);
            yInvExit = (rect2.y + rect2.h) - rect1.y;
        }
        else
        {
            yInvEntry = (rect2.y + rect2.h) - rect1.y;
            yInvExit = rect2.y - (rect1.y + rect1.h);
        }

        // find time of collision and time of leaving for each axis (if statement is to prevent divide by zero)
        float xEntry, yEntry;
        float xExit, yExit;

        if (rect1.vx == 0.0f)
        {
            xEntry = -float.PositiveInfinity;
            xExit = float.PositiveInfinity;
        }
        else
        {
            xEntry = xInvEntry / rect1.vx;
            xExit = xInvExit / rect1.vx;
        }

        if (rect1.vy == 0.0f)
        {
            yEntry = -float.PositiveInfinity;
            yExit = float.PositiveInfinity;
        }
        else
        {
            yEntry = yInvEntry / rect1.vy;
            yExit = yInvExit / rect1.vy;
        }

        // find the earliest/latest times of collision
        float entryTime = Math.Max(xEntry, yEntry);
        float exitTime = Math.Min(xExit, yExit);

        // if there was no collision
        if (entryTime > exitTime || xEntry < 0.0f && yEntry < 0.0f || xEntry > 1.0f || yEntry > 1.0f)
        {
            normalx = 0.0f;
            normaly = 0.0f;
            return 1.0f;
        }
        else // if there was a collision
        {
            // calculate normal of collided surface
            if (xEntry > yEntry)
            {
                if (xInvEntry < 0.0f)
                {
                    normalx = 1.0f;
                    normaly = 0.0f;
                }
                else
                {
                    normalx = -1.0f;
                    normaly = 0.0f;
                }
            }
            else
            {
                if (yInvEntry < 0.0f)
                {
                    normalx = 0.0f;
                    normaly = 1.0f;
                }
                else
                {
                    normalx = 0.0f;
                    normaly = -1.0f;
                }
            }

            // return the time of collision
            return entryTime;
        }
    }
}