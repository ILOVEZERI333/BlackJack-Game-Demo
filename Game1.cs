using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading;
using static REAL_FIRST_PROJ_PLS.CardClasses;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Diagnostics.Eventing.Reader;





namespace REAL_FIRST_PROJ_PLS
{
    public class Game1 : Game
    {

        #region fields
        Dictionary<string, Vector2> cardNameToLocation = new Dictionary<string, Vector2>();

        CardClasses.GameState cardClasses = new CardClasses.GameState();

        Dictionary<string, Vector2> dealerCardNameToLocation = new Dictionary<string, Vector2>();

        Random random = new Random();

        int playerScore = 0;

        Song music;

        bool gameWon = false;

        Vector2 defaultCardHitPos;

        public event Action OnHit;

        private GraphicsDeviceManager _graphics;
        
        private SpriteBatch _spriteBatch;
        
        Texture2D hitButton;
        
        MouseState mouseState;
        
        Vector2 hitPosition;

        string randomName;

        Vector2 dummyLocation;

        private int dealerScore = 0;

        private MouseState oldMouseState;

        private Texture2D loseText;

        public bool playerTurnDone;

        private Texture2D winText;

        public bool gameLost;

        Texture2D standButton;

        Vector2 standPosition;

        public bool stand;

        private bool dealerStand;

        Texture2D blankCard;

        List<string> dealerCards = new List<string>();

        #endregion

        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            string randomCard = CardClasses.GameState.CardName[random.Next(CardClasses.GameState.CardName.Count)];
            CardClasses.GameState.CardName.Remove(randomCard);
            // TODO: Add your initialization logic here
            defaultCardHitPos = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight /2 + 200) / 3 + new Vector2(0, 50);
            hitPosition = new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_graphics.PreferredBackBufferWidth / 6), (7*(_graphics.PreferredBackBufferHeight / 8)));
            standPosition = hitPosition + new Vector2(144, 3);
            base.Initialize();
            dealerCardNameToLocation.Add(randomCard, new Vector2(115,15));
            dealerScore += CardClasses.GameState.Cards[randomCard];
            randomCard = CardClasses.GameState.CardName[random.Next(CardClasses.GameState.CardName.Count)];
            CardClasses.GameState.CardName.Remove(randomCard);
            dealerCardNameToLocation.Add(randomCard, new Vector2(140,15));
            dealerScore += CardClasses.GameState.Cards[randomCard];
        }

        protected override void LoadContent()
        {
            winText = this.Content.Load<Texture2D>("YOU WIN");
            blankCard = this.Content.Load<Texture2D>("BLANK CARD");
            standButton = this.Content.Load<Texture2D>("STAND BUTTON");
            loseText = this.Content.Load<Texture2D>("YOU LOSE ");
            hitButton = this.Content.Load<Texture2D>("HIT BUTTON");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            music = this.Content.Load<Song>("How Sweet");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.05f;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            oldMouseState = mouseState;




            if (Keyboard.GetState().IsKeyDown(Keys.F)) 
            {
                MediaPlayer.Pause();
                
            }



            mouseState = Mouse.GetState();
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"X : {mouseState.X} Y : {mouseState.Y}");
#endif

            #region hitbutton logic
            //if mouse within hit button and button presed
            if ((mouseState.X > 270 && mouseState.X < 385) && (mouseState.Y > 423 && mouseState.Y < 461 ) && !gameLost && !stand)   
            {
                
                if ( oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                {


                    int x = 0;
                    dummyLocation = defaultCardHitPos;
                    randomName = CardClasses.GameState.CardName[random.Next(CardClasses.GameState.CardName.Count)];
                    CardClasses.GameState.CardName.Remove(randomName);
                    playerScore += CardClasses.GameState.Cards[randomName];
                    if (playerScore > 21)
                    {
                        gameLost = true;
                        playerTurnDone = true;
                    }
                    cardNameToLocation.Add(randomName, dummyLocation);

                    foreach (var key in cardNameToLocation.Keys) 
                    {
                        Vector2 dummyVector = cardNameToLocation[key];
                        if (x == cardNameToLocation.Keys.Count - 1)
                            {
                                
                                for (int i = 0; i < 30; i++)
                                {
                                    dummyVector.Y -= (i * 0.35f) / 2;
                                }
                                //anim starts to slow down at end
                                for (int i = 1; i < 10; i++)
                                {
                                    dummyVector.Y -= ((54 / i) * 0.35f / 2);
                                }
                            }
                        else 
                        {
                            dummyVector.X -= 12;
                        }
                        cardNameToLocation[key] = dummyVector;
                        x += 1;
                    }
                    x = 0;    
                    
              


                }
            }
            #endregion
            
            #region standbutton and dealer logic
            if ((mouseState.X > 411 && mouseState.X < 527 ) && (mouseState.Y > 423 && mouseState.Y < 463 ) && !gameLost)
            { 
                if (oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed) 
                {
                    stand = true;
                    playerTurnDone = true;
                    string randomCard;
                    
                    
                    while (dealerScore <= 21)
                    {

                        randomCard = CardClasses.GameState.CardName[random.Next(CardClasses.GameState.CardName.Count)];
                        CardClasses.GameState.CardName.Remove(randomCard);
                        //if dealer closer, game lost
                        if (dealerScore == 21)
                        {
                            gameLost = true;
                            break;
                        }
                        if (21 - dealerScore < 21 - playerScore)
                        {
                            gameLost = true;
                            break;
                        }
                        //dealer hit logic
                        else if (21 - dealerScore > 21 - playerScore || (dealerScore <= 16) || (dealerScore == playerScore && dealerScore <= 17))
                        {
                            if (playerScore == 21)
                            {
                                gameWon = true;
                                break;
                            }

                            foreach (var key in dealerCards)
                            {
                                dealerCardNameToLocation[key] += new Vector2(-25, 0);
                            }
                            dealerCards.Add(randomCard);
                            dealerCardNameToLocation.Add(randomCard, new Vector2(140, 15));
                            dealerScore += CardClasses.GameState.Cards[randomCard];
                        }

                                
                    }
                    if (dealerScore > 21)
                    {
                        gameWon = true;
                    }

                    
                    
                    



                }
            }
            #endregion

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();




            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(3f));
            //account for the scaled matrix


            foreach (var key in cardNameToLocation.Keys)
            {
                _spriteBatch.Draw(this.Content.Load<Texture2D>(key),cardNameToLocation[key], Color.White);
            }

            for (int i = 0; i <= dealerCardNameToLocation.Count - 1; i++) 
            {
                dealerCards = dealerCardNameToLocation.Keys.ToList();
                if (playerTurnDone)
                {
                    _spriteBatch.Draw(this.Content.Load<Texture2D>(dealerCards[i]), dealerCardNameToLocation[dealerCards[i]], Color.White);
                }
                else 
                {

                    if (i == 1)
                    {
                        _spriteBatch.Draw(blankCard, new Vector2(140, 15), null, Color.White, 0, new Vector2(0, 0), 0.338f, 0, 0);
                    }
                    else
                    {
                        _spriteBatch.Draw(this.Content.Load<Texture2D>(dealerCards[i]), dealerCardNameToLocation[dealerCards[i]], Color.White);
                    }
                    
                }
            }





            if (gameWon)
            {
                _spriteBatch.Draw(winText, new Vector2(96, 55), Color.White);
            }


            if (gameLost)
            {
                _spriteBatch.Draw(loseText,new Vector2(76,55), Color.White);
            }
            

            _spriteBatch.Draw(hitButton, hitPosition/3 ,Color.White);
            _spriteBatch.Draw(standButton, standPosition/3, Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
