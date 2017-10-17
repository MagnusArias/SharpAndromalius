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
        public const String PLAYER_MAIN = "Player/player-main";
        public static Texture2D Player_Red;
        public const String PLAYER_RED = "Player/player-spritemap-red";
        public static Texture2D Player_Blue;
        public const String PLAYER_BLUE = "Player/player-spritemap-blue";
        public static Texture2D Player_Green;
        public const String PLAYER_GREEN = "Player/player-spritemap-green";
        public static Texture2D Player_Grey;
        public const String PLAYER_GREY = "Player/player-spritemap-grey";
        //
        //
        //  
        //          ARMOR SPRITES
        //
        public static Texture2D Armor_Red;
        public const String ARMOR_RED = "Armor/Armor/red";
        //
        //
        //  
        //          ROBE SPRITES
        //
        public static Texture2D Robe_Black;
        public const String ROBE_BLACK = "Armor/Robe/black";
        //
        //
        //  
        //          WEAPONS SPRITES
        //
        public static Texture2D Skill_Sword;
        public const String SKILL_SWORD = "Weapons/sword-slash";
        //
        //
        //  
        //          ENEMY SPRITES
        //
        //             
        //  Skeletons:  36  48
        public static Texture2D E_SkeletonGreenWalk;
        public const String SKELETON_GREEN_WALK = "Enemies/Skeleton/green-walk";
        public static Texture2D E_SkeletonGreenDead;
        public const String SKELETON_GREEN_DEAD = "Enemies/Skeleton/green-dead";
        //
        //  Ghosts:     25  40
        public static Texture2D E_GhostBlueWalk;
        public const String GHOST_BLUE_WALK = "Enemies/Ghost/blue-walk";
        //
        //  Boss 1:     114 176
        public static Texture2D E_BossWalk;
        public const String BOSS_WALK = "Enemies/Bosses/boss-1";
        //
        //
        //  
        //          ITEMS ICONS SPRITES
        //
        public static Texture2D Icon_Sword;
        public const String SWORD = "Icons/sword3";
        public static Texture2D Icon_DoubleJump;
        public const String DOUBLE_JUMP = "Icons/armor5";
        public static Texture2D Icon_Dash;
        public const String DASH = "Icons/robe2";
        public static Texture2D Icon_Fireball;
        public const String FIREBALL = "Icons/book7";
        //
        //
        //  
        //          PROJECTILE SPRITES
        //
        public static Texture2D Particle;
        public const String PARTICLES_SPRITEMAP = "Misc/energy-particle";
        //
        //
        //  
        //          TILESETS SPRITES
        //
        public static Texture2D Tileset_1;
        public const String TILESET_L1 = "Tilesets/tileset3";
        public static Texture2D Tileset_2;
        public const String TILESET_L2 = "Tilesets/ruintileset";
        //
        //
        //  
        //          BACKGROUND SPRITES
        //
        public static Texture2D Background;
        public const String BACKGROUND = "Backgrounds/tlo";
        //
        //
        //  
        //          MAPS
        //
        public const String MAP_1 = "Content/Maps/level1.map";
        public const String MAP_2 = "Content/Maps/level2.map";
        //
        //
        //  
        //          HUD
        //
        public static Texture2D bossHPBar;
        public const String BOSSHPBAR = "Misc/boss-hp-bar";
        public static Texture2D bossHPBarOutline;
        public const String BOSSBAROUTLINE = "Miscboss-hp-bar-outline";
        public static Texture2D hpBar;
        public const String HPBAR = "Misc/hp-bar";
        public static Texture2D mpBar;
        public const String FIREBAR = "Misc/fireball-bar";
        public static Texture2D staBar;
        public const String DASHBAR = "Misc/dash-bar";
        public static Texture2D hudBar;
        public const String HUD = "Misc/hud";
        public static Texture2D Teleport;
        public const String PORTALSPRITEMAP = "Misc/p";
        //
        //
        //  
        //          FONTS
        //
        public static SpriteFont fontTitle;
        public const String FONT_TITLE = "Fonts/Title";
        public static SpriteFont fontSimple;
        public const String FONT_SIMPLE = "Fonts/Plain";
        //
        //
        //  
        //          MISC ITEMS
        //
        public static Texture2D blackRect;
        public static Texture2D whiteRect;
        public static Texture2D Particles;        
 
        public static Boolean DEBUG_READY;

    }
}
