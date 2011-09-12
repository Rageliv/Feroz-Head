using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public class Heibo
{
    Vector2 myPosition;
    Boolean off = false;
    public Heibo(float X, float Y)
    {
        myPosition.X = X;
        myPosition.Y = Y;

    }
    public void Off()
    {
        off = true;
    }
    public Boolean isOff()
    {
        return off;
    }
    public void UpdateHeiboPosition()
    {
        myPosition.Y += 1;
    }
    public Vector2 GetHeiboPosition()
    {
        return myPosition;
    }
    public float GetHeiboPositionY()
    {
        return myPosition.Y;
    }
    public float GetHeiboPositionX()
    {
        return myPosition.X;
    }
}
