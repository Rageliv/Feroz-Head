using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FerozHead
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FerozHead : Microsoft.Xna.Framework.Game
    {
        #region MFW
        struct Bullet
        {
            public Vector2 position;
        }
        Random rand = new Random();
        int totalGame;
        Boolean heibo = false;
        Boolean heibodrop = false;
        Boolean cg = false;
        Boolean cgdrop = false;
        float LeftThumbY;
        float LeftThumbX;
        float RightThumbX;
        float RightThumbY;
        float RotationAngle;
        int allowedEnemy = 500;
        int oldKills;
        Vector2 origin;
        Vector2 center;
        Stopwatch mundotime = new Stopwatch();
        SoundEffect enemydeath;
        SoundEffect grunt1;
        SoundEffect mundo;
        Song yes;
        Song master;
        SpriteFont font;
        Song galaxytrain;
        Texture2D bg;
        float fireInterval;
        int kills;
        Texture2D charles;
        Texture2D lelouch;
        Texture2D geass;
        Texture2D spinzaku;
        Texture2D hei;
        Texture2D heimask;
        Texture2D vodka;
        Texture2D bulletTexture;
        Texture2D enemy;
        Texture2D heiback;
        Texture2D original;
        Texture2D originalBullet;
        Texture2D headopen;
        List<Bullet> bulletList = new List<Bullet>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Heibo> heiboList = new List<Heibo>();
        List<Geass> geassList = new List<Geass>();
        double lastBulletTime = 0;
        double lastEnemyTime = 0;
        Vector2 ferozPosition = new Vector2(0, 0);
        Texture2D ferozTexture;
        Texture2D ferozback;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        byte redIntensity = 0;
        bool redCountingUp = true;
        byte blueIntensity = 80;
        bool blueCountingUp = false;
        byte greenIntensity = 160;
        bool greenCountingUp = true;
        #endregion
        public FerozHead()
        {
            mundotime.Start();
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ferozPosition.X = graphics.PreferredBackBufferWidth / 2;
            ferozPosition.Y = graphics.PreferredBackBufferHeight / 2;
            base.Initialize();
            MediaPlayer.Play(yes);
            MediaPlayer.IsRepeating = true;
            lastEnemyTime = 0;
            fireInterval = 500;
            kills = 0;
        }        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            grunt1 = Content.Load<SoundEffect>("attack1");
            mundo = Content.Load<SoundEffect>("MUNDO");
            yes = Content.Load<Song>("yes");
            master = Content.Load<Song>("Master");
            enemydeath = Content.Load<SoundEffect>("enemydeath");
            galaxytrain = Content.Load<Song>("GalaxyTrain");
            original = this.Content.Load<Texture2D>("head");
            ferozTexture = original;
            heimask = this.Content.Load<Texture2D>("heimask");
            ferozback = this.Content.Load<Texture2D>("head");
            originalBullet = this.Content.Load<Texture2D>("bullet");
            bulletTexture = originalBullet;
            charles = this.Content.Load<Texture2D>("charles");
            enemy = this.Content.Load<Texture2D>("enemy");
            hei = this.Content.Load<Texture2D>("hei");
            bg = this.Content.Load<Texture2D>("bg");
            headopen = this.Content.Load<Texture2D>("headopen");
            vodka = this.Content.Load<Texture2D>("vodka");
            heiback = this.Content.Load<Texture2D>("heiback");
            font = this.Content.Load<SpriteFont>("gameFont");
            geass = this.Content.Load<Texture2D>("geass");
            spinzaku = this.Content.Load<Texture2D>("spinzaku");
            lelouch = this.Content.Load<Texture2D>("lelouch");
            origin.X = bulletTexture.Width / 2;
            origin.Y = bulletTexture.Height / 2;
            center.X = 0;
            center.Y = 0;
            // TODO: use this.Content to load your game content here
        }        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }        
        protected override void Update(GameTime gameTime)
        {
            MakeEnemy(gameTime);
            totalGame = gameTime.TotalGameTime.Milliseconds;
            LeftThumbX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X * 15;
            LeftThumbY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y * -15;
            RightThumbX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
            RightThumbY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
            // Allows the game to exit
            RotationAngle -= .15f;
            //float circle = MathHelper.Pi;
            //RotationAngle = (RotationAngle % circle)/100;
            if (heibo == false && kills >= 100)
            {
                MakeHeibo();
            }
            if (cg == false && kills >= 300)
            {
                MakeGeass();
            }
            UpdateInput(gameTime);
            if (ferozPosition.X < -60 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left) || ferozPosition.X <-60 && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
            {
                ferozPosition.X = graphics.PreferredBackBufferWidth + 100;
            }
            if (ferozPosition.X > graphics.PreferredBackBufferWidth + 100 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right) || ferozPosition.X > graphics.PreferredBackBufferWidth && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
            {
                ferozPosition.X = -100;
            }
            if (redIntensity == 255) redCountingUp = false;
            if (redIntensity == 0) redCountingUp = true;
            if (redCountingUp) redIntensity++; else redIntensity--;

            if (blueIntensity == 255) blueCountingUp = false;
            if (blueIntensity == 0) blueCountingUp = true;
            if (blueCountingUp) blueIntensity++; else blueIntensity--;

            if (greenIntensity == 255) greenCountingUp = false;
            if (greenIntensity == 0) greenCountingUp = true;
            if (greenCountingUp) greenIntensity++; else greenIntensity--;
            // TODO: Add your update logic here
            UpdateCollision();
            UpdateBulletPositions();
            base.Update(gameTime);
        }
        private void UpdateCollision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;
            Rectangle player;
            Rectangle rectangle4;
            Rectangle geassrec;
            player = new Rectangle((int)ferozPosition.X - ferozTexture.Width / 2, (int)ferozPosition.Y - ferozTexture.Height / 2, ferozTexture.Width, ferozTexture.Height);
            for (int i = 0; i < geassList.Count; i++)
            {
                geassrec = new Rectangle((int)geassList[i].GetGeassPositionX() - geass.Width / 2, (int)geassList[i].GetGeassPositionY() - geass.Height / 2, geass.Width, geass.Height);
                if (player.Intersects(geassrec))
                {
                    heibo = false;
                    cg = true;
                    allowedEnemy = 50;
                    geassList.RemoveAt(i);
                    fireInterval = 200;
                    MediaPlayer.Play(master);
                }
            }
            for (int i = 0; i < heiboList.Count; i++)
            {
                rectangle4 = new Rectangle((int)heiboList[i].GetHeiboPositionX() - heimask.Width / 2, (int)heiboList[i].GetHeiboPositionY() - heimask.Height / 2, heimask.Width, heimask.Height);
                if (player.Intersects(rectangle4))
                {
                    heibo = true;
                    heiboList.RemoveAt(i);
                    fireInterval = 150;
                    MediaPlayer.Play(galaxytrain);
                }
            }
            for (int i = 0; i < bulletList.Count; i++)
            {
                for (int j = 0; j < enemyList.Count; j++)
                {
                    rectangle1 = new Rectangle((int)bulletList[i].position.X -
                        bulletTexture.Width / 2, (int)bulletList[i].position.Y -
                        bulletTexture.Height / 2, bulletTexture.Width, bulletTexture.Height);

                    rectangle2 = new Rectangle((int)enemyList[j].GetEnemyPositionX() - enemy.Width / 2,
                        (int)enemyList[j].GetEnemyPositionY() - enemy.Height / 2,
                        enemy.Width, enemy.Height);
                    // this is where shite happens
                    if (rectangle1.Intersects(rectangle2))
                    {
                        if (kills - oldKills == 50)
                        {
                            allowedEnemy -= 100;
                            oldKills = kills;
                        }
                        enemyList.RemoveAt(j);
                        kills++;
                        if (cg == false)
                        {
                            bulletList.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
            

        }
        private void MakeGeass()
        {
            if (kills > 1000 && cgdrop == false)
            {
                Random rand = new Random();
                float geassXPos = rand.Next(150, graphics.PreferredBackBufferWidth - 150);
                Geass newGeass = new Geass(geassXPos, -150);
                geassList.Add(newGeass);
                cgdrop = true;
            }
        }
        private void DrawGeass()
        {
            if (geassList.Count > 0)
            {
                VertexPositionTexture[] geassVertices = new VertexPositionTexture[geassList.Count];
                int i = 0;
                foreach (Geass currentGeass in geassList)
                {
                    currentGeass.UpdateGeassPosition();
                    if (currentGeass.GetGeassPositionY() < graphics.PreferredBackBufferHeight + 100)
                    { spriteBatch.Draw(geass, currentGeass.GetGeassPosition(), null, Color.White); }
                    geassVertices[i++] = new VertexPositionTexture();
                }
            }
        }
        #region Heibo
        private void MakeHeibo()
        {
            if (kills > 500 && heibodrop == false)
            {
                Random rand = new Random();
                float heiboXPos = rand.Next(150, graphics.PreferredBackBufferWidth - 150);
                Heibo newHeibo = new Heibo(heiboXPos, -150);
                heiboList.Add(newHeibo);
                heibodrop = true;
            }
        }
        private void DrawHeibo()
        {
            if (heiboList.Count > 0)
            {
                VertexPositionTexture[] heiboVertices = new VertexPositionTexture[heiboList.Count];
                int i = 0;
                foreach (Heibo currentHeibo in heiboList)
                {
                    currentHeibo.UpdateHeiboPosition();
                    if (currentHeibo.GetHeiboPositionY() < graphics.PreferredBackBufferHeight + 100)
                    { spriteBatch.Draw(heimask, currentHeibo.GetHeiboPosition(), null, Color.White); }
                    heiboVertices[i++] = new VertexPositionTexture();
                }
            }
        }
        #endregion
        #region Enemy
        private void MakeEnemy(GameTime gameTime)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (currentTime - lastEnemyTime > allowedEnemy)
            {
                float enemyXPos = rand.Next(1, graphics.PreferredBackBufferWidth);
                Enemy newEnemy = new Enemy(enemyXPos , -150);
                enemyList.Add(newEnemy);
                lastEnemyTime = currentTime;

            }
        }
        private void DrawEnemies()
        { 
            if (enemyList.Count > 0)
            {
                VertexPositionTexture[] enemyVertices = new VertexPositionTexture[enemyList.Count];
                int i = 0;
                foreach (Enemy currentEnemy in enemyList)
                {

                        currentEnemy.UpdateEnemyPosition();
                        Color backColor;
                        backColor = new Color(redIntensity, blueIntensity, greenIntensity);
                        if (currentEnemy.GetEnemyPositionY() < graphics.PreferredBackBufferHeight + 100)
                        { spriteBatch.Draw(enemy, currentEnemy.GetEnemyPosition(), null, backColor); }
                        enemyVertices[i++] = new VertexPositionTexture();


                }
            }

        }
        #endregion Enemy
        void UpdateInput(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back) == true)
            { this.Exit(); }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Q))
            {
                kills = 499;
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
            {
                kills = 1000;
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger) == true)
            {
                ferozTexture = headopen;
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (currentTime - lastBulletTime > fireInterval)
                {

                    Bullet newBullet = new Bullet();
                    newBullet.position = ferozPosition;
                    newBullet.position.X = ferozPosition.X + 50;
                    bulletList.Add(newBullet);
                    grunt1.Play();
                    lastBulletTime = currentTime;
                }
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.X) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y) == true)
            {
                kills += enemyList.Count();
                enemyList.RemoveRange(0, enemyList.Count());
            }
            else
            {
                ferozTexture = ferozback;
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter) && mundotime.ElapsedMilliseconds > 1000 || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X) == true && mundotime.ElapsedMilliseconds > 1000)
            {
                ferozTexture = headopen;
                mundo.Play();
                mundotime.Reset();
                mundotime.Start();
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
            {
                if (ferozPosition.Y < graphics.PreferredBackBufferHeight)
                {
                    ferozPosition.Y = ferozPosition.Y + 16;
                }
            }
            if (LeftThumbY != 0)
            {
                if (ferozPosition.Y > -50 || ferozPosition.Y < graphics.PreferredBackBufferHeight)
                {
                    ferozPosition.Y = ferozPosition.Y + LeftThumbY;
                }
                if (ferozPosition.Y < -50)
                { ferozPosition.Y = -50; }
                if (ferozPosition.Y > graphics.PreferredBackBufferHeight)
                { ferozPosition.Y = graphics.PreferredBackBufferHeight; }
            }
            if (LeftThumbX != 0)
            {
                ferozPosition.X = ferozPosition.X + LeftThumbX;
            } 
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
            {
                if (ferozPosition.Y > -50)
                {
                    ferozPosition.Y = ferozPosition.Y - 16;
                }
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            {
                ferozPosition.X = ferozPosition.X - 16;
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
            {
                ferozPosition.X = ferozPosition.X + 16;
            }

            
        }
        #region BOOLITS
        private void DrawBullets()
        {
            if (bulletList.Count > 0)
            {
                VertexPositionTexture[] bulletVertices = new VertexPositionTexture[bulletList.Count];
                int i = 0;
                foreach (Bullet currentBullet in bulletList)
                {
                    Vector2 center = currentBullet.position;
                    Color backColor;
                    backColor = new Color(redIntensity, blueIntensity, greenIntensity);
                    if (heibo == false)
                    {
                        if (cg == true)
                        {
                            spriteBatch.Draw(spinzaku, currentBullet.position, null, Color.White, RotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            spriteBatch.Draw(originalBullet, currentBullet.position, null, Color.White, RotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(vodka, currentBullet.position, null, Color.White);
                    }
                    bulletVertices[i++] = new VertexPositionTexture();
                }
            }
        }
        private void UpdateBulletPositions()
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet currentBullet = bulletList[i];
                if (currentBullet.position.Y >= -150)
                {
                    currentBullet.position.Y = MoveForward(currentBullet.position);
                    
                    bulletList[i] = currentBullet;
                }
                else
                {
                    bulletList.RemoveAt(i);
                }    
            }
        }
        #endregion
        private float MoveForward(Vector2 pos)
        {
            if (heibo == false)
            {
                if (cg == true)
                {
                    pos.Y = pos.Y - 30;
                    return pos.Y;
                }
                pos.Y = pos.Y - 20;
                return pos.Y;
            }
            else
            {
                pos.Y = pos.Y - 10;
                return pos.Y;
            }
        }        
        protected override void Draw(GameTime gameTime)
        {
            
            Color backgroundColor;
            backgroundColor = new Color(redIntensity, blueIntensity, greenIntensity);
            GraphicsDevice.Clear(backgroundColor);
            
            
            spriteBatch.Begin();
            if (heibo == false)
            {
                if (cg == true)
                {
                    spriteBatch.Draw(charles, center, Color.White);
                }
                else
                { spriteBatch.Draw(bg, center, Color.White); }
            }
            else
            {
                spriteBatch.Draw(heiback, center, Color.DarkGray);
            }
            if (heibo == false)
            {
                if (cg == true)
                {
                    spriteBatch.Draw(lelouch, ferozPosition, Color.White);
                }
                else
                {
                    spriteBatch.Draw(ferozTexture, ferozPosition, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(hei, ferozPosition, Color.White);
            }
            DrawBullets();
            DrawEnemies();
            DrawHeibo();
            DrawGeass();
            spriteBatch.DrawString(font, "kills: " + kills, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            spriteBatch.End();
            
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
