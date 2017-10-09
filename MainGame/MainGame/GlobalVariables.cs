using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame
{
    public static class GlobalVariables
    {
        public static int WIDTH;
        public static int HEIGHT;
        public static int SCALE;

        public const String SKELETON_SPRITEMAP =            "Content/Assets/enemy-skeleton-spritemap.png";
        public const String DEAD_SKELETON_SPRITEMAP =       "Content/Assets/enemy-skeleton-dead-spritemap.png";
        public const String GHOST_SPRITEMAP =               "Content/Assets/enemy-ghost-spritemap.png";
        public const String PARTICLES_SPRITEMAP =           "Content/Assets/energy-particle.png";
        public const String BOSS1_SPRITEMAP =               "Content/Assets/boss-1.png";

        public const String SWORD =                         "Content/Assets/Icons/sword3.png";
        public const String DOUBLE_JUMP =                   "Content/Assets/Icons/armor5.png";
        public const String DASH =                          "Content/Assets/Icons/robe2.png";
        public const String FIREBALL =                      "Content/Assets/Icons/book7.png";

        public const String PLAYER =                        "Content/Assets/Player/player-spritemap-v9.png";

        public const String BACKGROUND =                    "Content/Assets/Misc/background.png";

        public const String BOSSHPBAR =                     "/Content/Assets/boss-hp-bar.png";
        public const String BOSSBAROUTLINE =                "/Content/Assets/boss-hp-bar-outline.png";
        public const String HPBAR =                         "/Game/Src/Assets/hp-bar.png";
        public const String FIREBAR =                       "/Game/Src/Assets/fireball-bar.png";
        public const String DASHBAR =                       "/Game/Src/Assets/dash-bar.png";
        public const String HUD =                           "/Game/Src/Assets/hud.png";
        public const String PORTALSPRITEMAP =               "/Content/Assets/penis.gif";

        public static Texture2D Enemy_Skeleton;
        public static Texture2D Enemy_DeadSkeleton;
        public static Texture2D Enemy_Ghost;
        public static Texture2D Particles;
        public static Texture2D Enemy_Boss1;

        public static Texture2D Item_Sword;
        public static Texture2D Item_DoubleJump;
        public static Texture2D Item_Dash;
        public static Texture2D Item_Fireball;
        public static Texture2D hpBar;
        public static Texture2D mpBar;
        public static Texture2D staBar;
        public static Texture2D hudBar;
        public static Texture2D Player;
        public static Texture2D Background;

        public static Texture2D bossHPBar;
        public static Texture2D bossHPBarOutline;

        public static Boolean DEBUG_READY;

        public static void LoadContentFromSource()
        {
            Enemy_Skeleton = Content.Load<Texture2D>(GlobalVariables.SKELETON_SPRITEMAP);
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

            hpBar = ImageIO.read(getClass().getResourceAsStream(BOSSHPBAR));
            hpBarOutline = ImageIO.read(getClass().getResourceAsStream(BOSSBAROUTLINE));
        }
    }
}
