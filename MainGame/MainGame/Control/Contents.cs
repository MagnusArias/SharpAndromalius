using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Sharp_Andromalius.GameData.Control
{
    class Contents
    {
        
        // TO-DO: znalezc zaminnik buffered image
        private const String SKELETON_SPRITEMAP = "GameData/Assets/enemy-skeleton-spritemap.png";
        private const String DEAD_SKELETON_SPRITEMAP = "GameData/Assets/enemy-skeleton-dead-spritemap.png";
        private const String GHOST_SPRITEMAP = "GameData/Assets/enemy-ghost-spritemap.png";
        private const String PARTICLES_SPRITEMAP = "GameData/Assets/energy-particle.png";
        private const String BOSS1_SPRITEMAP = "GameData/Assets/boss-1.png";

        private const String SWORD = "GameData/Assets/Icons/sword3.png";
        private const String DOUBLE_JUMP = "GameData/Assets/Icons/armor5.png";
        private const String DASH = "GameData/Assets/Icons/robe2.png";
        private const String FIREBALL = "GameData/Assets/Icons/book7.png";

        public static Texture2D EnemySkeleton = Content.Load<Texture2D>(@SKELETON_SPRITEMAP);

        protected override void LoadContent()
        {
            Texture2D ret;

            try
            {
                Texture2D ss = Texture2D()
                int width = ss.getWidth() / w;
                int height = ss.getHeight() / h;
                ret = new BufferedImage[height][width];
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++) {
                        ret[i][j] = ss.getSubimage(j * w, i * h, w, h);
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                Console.WriteLine("Error loading graphics from Content class.");
                Environment.Exit(0);

            }
            return null;
        }

    }
}
