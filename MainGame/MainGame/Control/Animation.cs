using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Control
{
    class Animation
    {
        //TO-DO: zastąpić string odpowiednikiem BUfferedImage
        private String[] frames;
        private int currentFrame;
        private int numFrames;

        private int count;
        private int delay;

        private int timesPlayed;

        public Animation()
        {
            timesPlayed = 0;
        }

        public void SetFrames(String[] frames)
        {
            this.frames = frames;
            currentFrame = 0;
            count = 0;
            timesPlayed = 0;
            delay = 5;
            numFrames = frames.Length;
        }

        public void SetDelay(int i)
        {
            delay = i;
        }

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

        public String GetImage()
        {
            return frames[currentFrame];
        }

        public Boolean HasPlayesOnce()
        {
            return timesPlayed > 0;
        }

        public Boolean HasPlayed(int i)
        {
            return timesPlayed == i;
        }
    }
}
