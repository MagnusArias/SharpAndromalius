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
        public static int GAME_WINDOW_WIDTH;
        public static int GAME_WINDOW_HEIGHT;
        public static int GAME_WINDOW_SCALE;

        //
        //
        //  
        //          PLAYER SPRITES
        //
        public static Texture2D Player_Main;
        public const String PLAYER_MAIN = "Content/Player/player-main.png";
        public static Texture2D Player_Red;
        public const String PLAYER_RED = "Content/Player/player-spritemap-red.png";
        public static Texture2D Player_Blue;
        public const String PLAYER_BLUE = "Content/Player/player-spritemap-blue.png";
        public static Texture2D Player_Green;
        public const String PLAYER_GREEN = "Content/Player/player-spritemap-green.png";
        public static Texture2D Player_Grey;
        public const String PLAYER_GREY = "Content/Player/player-spritemap-grey.png";
        //
        //
        //  
        //          ARMOR SPRITES
        //
        public const String ARMOR_RED = "Content/Arnor/Armor/red.png";
        //
        //
        //  
        //          ROBE SPRITES
        //
        public const String ROBE_BLACK = "Content/Armor/Robe/black.png";
        //
        //
        //  
        //          ENEMY SPRITES
        //
        //             
        //  Skeletons:  36  48
        public static Texture2D E_SkeletonGreenWalk;
        public const String SKELETON_GREEN_WALK = "Content/Enemies/Skeleton/green-walk.png";
        public static Texture2D E_SkeletonGreenDead;
        public const String SKELETON_GREEN_DEAD = "Content/Enemies/Skeleton/green-dead.png";
        //
        //  Ghosts:     25  40
        public static Texture2D E_GhostBlueWalk;
        public const String GHOST_BLUE_WALK = "Content/Enemies/Ghost/blue-walk.png";
        //
        //  Boss 1:     114 176
        public static Texture2D E_BossWalk;
        public const String BOSS_WALK = "Content/Enemies/Bosses/boss-1.png";
        //
        //
        //  
        //          ITEMS SPRITES
        //
        public const String SWORD = "Content/Assets/Icons/sword3.png";
        public const String DOUBLE_JUMP = "Content/Assets/Icons/armor5.png";
        public const String DASH = "Content/Assets/Icons/robe2.png";
        public const String FIREBALL = "Content/Assets/Icons/book7.png";
        //
        //
        //  
        //          PROJECTILE SPRITES
        //
        public const String PARTICLES_SPRITEMAP = "Content/energy-particle.png";
        //
        //
        //  
        //          TILESETS SPRITES
        //
        public static Texture2D Tileset_1;
        public const String TILESET_L1 = "Content/Tilesets/tileset.png";
        public const String TILESET_L2 = "Content/Tilesets/tileset.png";
        //
        //
        //  
        //          BACKGROUND SPRITES
        //
        public static Texture2D Background;
        public const String BACKGROUND = "Backgrounds/tlo.png";
        //
        //
        //  
        //          MAPS
        //
        public const String MAP_1 = "Content/Assets/Misc/background.png";
        public const String MAP_2 = "Content/Assets/Misc/background.png";
        //
        //
        //  
        //          MISC SPRITES
        //
        public const String BOSSHPBAR = "/Content/Assets/boss-hp-bar.png";
        public const String BOSSBAROUTLINE = "/Content/Assets/boss-hp-bar-outline.png";
        public const String HPBAR = "/Content/Assets/hp-bar.png";
        public const String FIREBAR = "/Content/Assets/fireball-bar.png";
        public const String DASHBAR = "/Content/Assets/dash-bar.png";
        public const String HUD = "/Content/Assets/hud.png";
        public const String PORTALSPRITEMAP = "/Content/Assets/penis.gif";


        public const String FONT_TITLE = "Fonts/Title";
        public static SpriteFont fontTitle;

        public const String FONT_SIMPLE = "Fonts/Plain";
        public static SpriteFont fontSimple;


        

        public static Texture2D blackRect;
        public static Texture2D whiteRect;
        public static Texture2D Particles;
        

        public static Texture2D Item_Sword;
        public static Texture2D Item_DoubleJump;
        public static Texture2D Item_Dash;
        public static Texture2D Item_Fireball;
        public static Texture2D hpBar;
        public static Texture2D mpBar;
        public static Texture2D staBar;
        public static Texture2D hudBar;
        
        public static Texture2D Teleport;

        public static Texture2D bossHPBar;
        public static Texture2D bossHPBarOutline;

        public static Boolean DEBUG_READY;

    }
}
