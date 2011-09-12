using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public class Geass
{
    Vector2 myPosition;
    Boolean off = false;
    public Geass(float X, float Y)
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
    public void UpdateGeassPosition()
    {
        myPosition.Y += 1;
    }
    public Vector2 GetGeassPosition()
    {
        return myPosition;
    }
    public float GetGeassPositionY()
    {
        return myPosition.Y;
    }
    public float GetGeassPositionX()
    {
        return myPosition.X;
    }
}
