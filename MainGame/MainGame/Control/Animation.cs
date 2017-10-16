#region Using statements and file description
using Microsoft.Xna.Framework.Graphics;
using System;
//-----------------------------------------------------------------------------
// Animation.cs
//
// Originally created: 10.07.2016, 20:43 by Przemysław Dębiec
// 
// Main controller for animations
//-----------------------------------------------------------------------------
#endregion

namespace MainGame.Control
{
    class Animation
    {
        private Texture2D[] frames;
        private int currentFrame;
        private int numFrames;

        private int count;
        private int delay;

        private int timesPlayed;

        public Animation() => timesPlayed = 0;

        public void SetFrames(Texture2D[] f)
        {
            this.frames = f;
            currentFrame = 0;
            count = 0;
            timesPlayed = 0;
            delay = 5;
            numFrames = f.Length;
        }

        public void SetDelay(int i) => delay = i;

        public void Update()
        {
            if (delay == -1) return;
            count++;

            if (count == delay)
            {
                currentFrame++;
                count = 0;
            }
            if (currentFrame == numFrames)
            {
                currentFrame = 0;
                timesPlayed++;
            }
        }

        public Texture2D GetImage() => frames[currentFrame];

        public Boolean HasPlayedOnce() => timesPlayed > 0;

        public Boolean HasPlayed(int i) => timesPlayed == i;
    }
}
