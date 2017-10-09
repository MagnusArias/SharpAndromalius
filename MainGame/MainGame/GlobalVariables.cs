using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame
{
    class GlobalVariables : Game1
    {
        public const String SKELETON_SPRITEMAP = "Content/Assets/enemy-skeleton-spritemap.png";
        public const String DEAD_SKELETON_SPRITEMAP = "Content/Assets/enemy-skeleton-dead-spritemap.png";
        public const String GHOST_SPRITEMAP = "Content/Assets/enemy-ghost-spritemap.png";
        public const String PARTICLES_SPRITEMAP = "Content/Assets/energy-particle.png";
        public const String BOSS1_SPRITEMAP = "Content/Assets/boss-1.png";

        public const String SWORD = "Content/Assets/Icons/sword3.png";
        public const String DOUBLE_JUMP = "Content/Assets/Icons/armor5.png";
        public const String DASH = "Content/Assets/Icons/robe2.png";
        public const String FIREBALL = "Content/Assets/Icons/book7.png";

        public const String PLAYER = "Content/Assets/Player/player-spritemap-v9.png";

        public const String BACKGROUND = "Content/Assets/Misc/background.png";

        public const String BOSSHPBAR = "/Content/Assets/boss-hp-bar.png";
        public const String BOSSBAROUTLINE = "/Content/Assets/boss-hp-bar-outline.png";

        public Texture2D Enemy_Skeleton;
        public Texture2D Enemy_DeadSkeleton;
        public Texture2D Enemy_Ghost;
        public Texture2D Particles;
        public Texture2D Enemy_Boss1;

        public Texture2D Item_Sword;
        public Texture2D Item_DoubleJump;
        public Texture2D Item_Dash;
        public Texture2D Item_Fireball;

        public Texture2D Player;
        public Texture2D Background;


        public void LoadContentFromSource()
        {
            Enemy_Skeleton = this.Content.Load<Texture2D>(GlobalVariables.SKELETON_SPRITEMAP);
            Enemy_DeadSkeleton = this.Content.Load<Texture2D>(GlobalVariables.DEAD_SKELETON_SPRITEMAP);
            Enemy_Ghost = this.Content.Load<Texture2D>(GlobalVariables.GHOST_SPRITEMAP);
            Particles = this.Content.Load<Texture2D>(GlobalVariables.PARTICLES_SPRITEMAP);
            Enemy_Boss1 = this.Content.Load<Texture2D>(GlobalVariables.BOSS1_SPRITEMAP);

            Item_Sword = this.Content.Load<Texture2D>(GlobalVariables.SWORD);
            Item_Dash = this.Content.Load<Texture2D>(GlobalVariables.DASH);
            Item_DoubleJump = this.Content.Load<Texture2D>(GlobalVariables.DOUBLE_JUMP);
            Item_Fireball = this.Content.Load<Texture2D>(GlobalVariables.FIREBALL);

            Player = this.Content.Load<Texture2D>(GlobalVariables.PLAYER);
            Background = this.Content.Load<Texture2D>(GlobalVariables.BACKGROUND);
        }
    }
}
