using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Maps.Level_1
{
    class Level_1
    {

        private static final String TILESET = "/Game/Src/Map/Level1/tileset.png";
	    private static final String LEVEL = "/Game/Src/Map/Level1/level1.map";
	    private static final String BACKGROUND = "/Game/Src/Assets/tlo.png";

	    private Background back;
        private ArrayList<Rectangle> tb;
        private Player player;
        private TileMap tileMap;
        private ArrayList<Enemy> enemies;
        private ArrayList<FireBall> fireballs;
        private ArrayList<EnergyParticle> energyParticles;
        private ArrayList<ItemParent> items;
        private HUD hud;
        private Teleport teleport;


        private EnemyBoss1 eb;
        private DebugInfo debug;

        private int eventCount = 0;
        private boolean eventStart;
        private boolean eventDead;
        private boolean eventFinish;
        private boolean blockInput = false;

        public Level1(GameStateManager gsm)
        {
            super(gsm);
            init();
            blockInput = false;
        }

        public void init()
        {
            back = new Background(BACKGROUND, 0.5);

            tileMap = new TileMap(30);
            tileMap.loadTiles(TILESET);
            tileMap.loadMap(LEVEL);
            tileMap.setPosition(0, 0);
            tileMap.setTween(0.025);

            player = new Player(tileMap);
            player.setPosition(180, 1115);

            items = new ArrayList<ItemParent>();
            putHereItems();

            enemies = new ArrayList<Enemy>();
            populateEnemies();

            fireballs = new ArrayList<FireBall>();
            energyParticles = new ArrayList<EnergyParticle>();

            teleport = new Teleport(tileMap);
            teleport.setPosition(3100, 2300);

            player.init(enemies, energyParticles, items);

            debug = new DebugInfo(tileMap, player);
            hud = new HUD(tileMap);
            hud.init(player);

            eventStart = true;
            tb = new ArrayList<Rectangle>();
            eventStart();
        }

        private void putHereItems()
        {
            items.clear();
            ItemDoubleJump idj;
            ItemSword is;
            ItemDash id;
            ItemFireball ifb;

            idj = new ItemDoubleJump(tileMap, player); // pierwsza instancja do umieszczenia na mapie
            idj.setPosition(272, 1530);
            items.add(idj);
            id = new ItemDash(tileMap, player);
            id.setPosition(3080, 1000);
            items.add(id);

        is = new ItemSword(tileMap, player);

        is.setPosition(1000, 1500);
            items.add(is);
            ifb = new ItemFireball(tileMap, player);
            ifb.setPosition(210, 2140);
            items.add(ifb);

            idj = new ItemDoubleJump(tileMap, player); // druga instancja do wyswietlania w hud
            idj.setPosition(0, 0);
            items.add(idj);
            id = new ItemDash(tileMap, player);
            id.setPosition(0, 0);
            items.add(id);

        is = new ItemSword(tileMap, player);

        is.setPosition(0, 0);
            items.add(is);
            ifb = new ItemFireball(tileMap, player);
            ifb.setPosition(0, 0);
            items.add(ifb);
        }

        private void populateEnemies()
        {
            enemies.clear();
            EnemySkeleton es;
            EnemyGhost eg;

            es = new EnemySkeleton(tileMap, player);
            es.setPosition(660, 1175);
            enemies.add(es);

            es = new EnemySkeleton(tileMap, player);
            es.setPosition(1035, 1118);
            enemies.add(es);

            es = new EnemySkeleton(tileMap, player);
            es.setPosition(808, 1118);
            enemies.add(es);

            es = new EnemySkeleton(tileMap, player);
            es.setPosition(340, 200);
            enemies.add(es);

            es = new EnemySkeleton(tileMap, player);
            es.setPosition(1764, 1088);
            enemies.add(es);


            eg = new EnemyGhost(tileMap, player);
            eg.setPosition(1464, 1088);
            enemies.add(eg);

            eg = new EnemyGhost(tileMap, player);
            eg.setPosition(1956, 1088);
            enemies.add(eg);

            eg = new EnemyGhost(tileMap, player);
            eg.setPosition(1720, 2250);
            enemies.add(eg);



            eb = new EnemyBoss1(tileMap, player);
            eb.setPosition(2550, 1750);
            enemies.add(eb);
        }

        public void update()
        {
            handleInput();

            if (teleport.contains(player))
            {
                eventFinish = blockInput = true;
            }

            if (player.getHealth() == 0 || player.gety() > tileMap.getHeight()) { eventDead = blockInput = true; }

            if (eventStart) eventStart();
            if (eventDead) eventDead();
            if (eventFinish) eventFinish();

            back.setPosition(tileMap.getx(), tileMap.gety());

            player.update();

            if (player.getx() > 2240 && player.gety() > 1530 && player.gety() < 1870)
            {
                tileMap.setPosition(
                    GamePanel.WIDTH / 2 - player.getx() - 200,
                    GamePanel.HEIGHT / 2 - player.gety() + 100
                );
            }
            else
            {
                tileMap.setPosition(
                        GamePanel.WIDTH / 2 - player.getx() - (70 * player.setViewLeftRight()),
                        GamePanel.HEIGHT / 2 - player.gety() - (150 * player.setViewDown()) + 10
                );

            }

            tileMap.update();
            tileMap.fixBounds();

            for (int i = 0; i < fireballs.size(); i++)
            {
                FireBall f = fireballs.get(i);
                f.update(enemies);
                if (f.isHit())
                {
                    fireballs.remove(i);
                    i--;
                }
            }

            for (int i = 0; i < items.size(); i++)
            {
                ItemParent e = items.get(i);
                e.update();
                if (e.shouldRemove())
                {
                    items.remove(i);
                    i--;
                }
            }

            for (int i = 0; i < enemies.size(); i++)
            {
                Enemy e = enemies.get(i);
                e.update();
                if (e.shouldRemove())
                {
                    enemies.remove(i);
                    i--;
                }
            }
            teleport.update();
            debug.update();
        }

        public void handleInput()
        {
            FireBall fb;

            if (!blockInput)
            {
                player.setJumping(Keys.keyState[Keys.UP]);
                player.setLeft(Keys.keyState[Keys.LEFT]);
                player.setRight(Keys.keyState[Keys.RIGHT]);
                player.setDown(Keys.keyState[Keys.DOWN]);

                if (Keys.isPressed(Keys.BUTTON3)) player.setAttacking();
                if (Keys.isPressed(Keys.BUTTON4)) debug.setReady();
                if (Keys.isPressed(Keys.ENTER)) reset();

                if (Keys.isPressed(Keys.ESCAPE)) gsm.setPaused(true);

                if (Keys.isPressed(Keys.BUTTON2))
                {
                    if (player.isDashingReady() && player.getSkill(1)) player.setDashing();
                }

                if (Keys.isPressed(Keys.BUTTON1))
                {
                    if (player.isFireballReady() && player.getSkill(3))
                    {
                        fb = new FireBall(tileMap, player.getFacing());
                        fb.shootFireball(player.getx(), player.gety(), player.getFacing());
                        fireballs.add(fb);

                        player.setAttacking();
                        player.setFireballCooldown(0);
                    }
                }
            }
        }

        public void draw(Graphics2D g)
        {
            g.setColor(java.awt.Color.GREEN);
            Rectangle r = new Rectangle(0, 0, GamePanel.WIDTH, GamePanel.HEIGHT);
            g.fill(r);

            back.draw(g);

            player.draw(g);
            teleport.draw(g);

            for (int i = 0; i < fireballs.size(); i++) fireballs.get(i).draw(g);

            for (int i = 0; i < enemies.size(); i++) enemies.get(i).draw(g);

            tileMap.draw(g);

            hud.draw(g);
            for (int i = 0; i < items.size(); i++) items.get(i).draw(g);




            debug.draw(g);
            eb.drawHPBar(g);

            g.setColor(java.awt.Color.BLACK);
            for (int i = 0; i < tb.size(); i++)
            {
                g.fill(tb.get(i));
            }
        }

        private void reset()
        {
            player.reset();
            player.setPosition(180, 1115);
            player.setSkill(666, false);
            populateEnemies();
            putHereItems();
            blockInput = false;
            eventCount = 0;
            tileMap.setShaking(false, 0);
            eventStart = true;
            eventStart();
        }

        private void eventStart()
        {
            eventCount++;
            if (eventCount == 1)
            {
                tb.clear();
                tb.add(new Rectangle(0, 0, GamePanel.WIDTH, GamePanel.HEIGHT / 2));
                tb.add(new Rectangle(0, 0, GamePanel.WIDTH / 2, GamePanel.HEIGHT));
                tb.add(new Rectangle(0, GamePanel.HEIGHT / 2, GamePanel.WIDTH, GamePanel.HEIGHT / 2));
                tb.add(new Rectangle(GamePanel.WIDTH / 2, 0, GamePanel.WIDTH / 2, GamePanel.HEIGHT));
            }
            if (eventCount > 1 && eventCount < 60)
            {
                tb.get(0).height -= 4;
                tb.get(1).width -= 6;
                tb.get(2).y += 4;
                tb.get(3).x += 6;
            }
            if (eventCount == 60)
            {
                eventStart = false;
                eventCount = 0;
                tb.clear();
            }
        }

        private void eventDead()
        {
            eventCount++;
            if (eventCount == 1)
            {
                player.setDead();
                player.stop();
            }
            if (eventCount == 60)
            {
                tb.clear();
                tb.add(new Rectangle(
                    GamePanel.WIDTH / 2, GamePanel.HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                tb.get(0).x -= 6;
                tb.get(0).y -= 4;
                tb.get(0).width += 12;
                tb.get(0).height += 8;
            }
            if (eventCount >= 120)
            {
                if (player.getHealth() == 0)
                {
                    gsm.setState(GameStateManager.MENUSTATE);
                }
                else
                {
                    eventDead = blockInput = false;
                    eventCount = 0;
                    reset();
                }
            }
        }

        private void eventFinish()
        {
            eventCount++;
            if (eventCount == 30)
            {
                player.setTeleporting(true);
                player.stop();
            }
            else if (eventCount == 45)
            {
                tb.clear();
                tb.add(new Rectangle(
                        GamePanel.WIDTH / 2, GamePanel.HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                tb.get(0).x -= 6;
                tb.get(0).y -= 4;
                tb.get(0).width += 12;
                tb.get(0).height += 8;
            }
            if (eventCount == 120)
            {

                gsm.setState(GameStateManager.LEVEL2);
            }

        }
    }
}
