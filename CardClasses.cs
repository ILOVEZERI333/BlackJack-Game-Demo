using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace REAL_FIRST_PROJ_PLS
{
    internal class CardClasses
    {

        public class GameState
        {

            #region fields

            static readonly Dictionary<string, int> cards = new Dictionary<string, int>() 
            {
                {"French-Clover-2", 2 },
                {"French-Clover-3", 3 },
                {"French-Clover-4", 4 },
                {"French-Clover-5", 5 },
                {"French-Clover-6", 6 },
                {"French-Clover-7", 7 },
                {"French-Clover-8", 8 },
                {"French-Clover-9", 9 },
                {"French-Clover-10", 10 },
                {"French-Clover-A", 11 },
                {"French-Clover-K", 10 },
                {"French-Clover-Q", 10 },
                {"French-Clover-J", 10 },
                {"French-Diamond-2", 2 },
                {"French-Diamond-3", 3 },
                {"French-Diamond-4", 4 },
                {"French-Diamond-5", 5 },
                {"French-Diamond-6", 6 },
                {"French-Diamond-7", 7 },
                {"French-Diamond-8", 8 },
                {"French-Diamond-9", 9 },
                {"French-Diamond-10", 10 },
                {"French-Diamond-A", 11 },
                {"French-Diamond-Q", 10 },
                {"French-Diamond-K", 10 },
                {"French-Diamond-J", 10 },
                {"French-Heart-2", 2 },
                {"French-Heart-3", 3 },
                {"French-Heart-4", 4 },
                {"French-Heart-5", 5 },
                {"French-Heart-6", 6 },
                {"French-Heart-7", 7 },
                {"French-Heart-8", 8 },
                {"French-Heart-9", 9 },
                {"French-Heart-10", 10 },
                {"French-Heart-A", 11 },
                {"French-Heart-K", 10 },
                {"French-Heart-Q", 10 },
                {"French-Heart-J", 10 },
                {"French-Spade-2", 2 },
                {"French-Spade-3", 3 },
                {"French-Spade-4", 4 },
                {"French-Spade-5", 5 },
                {"French-Spade-6", 6 },
                {"French-Spade-7", 7 },
                {"French-Spade-8", 8 },
                {"French-Spade-9", 9 },
                {"French-Spade-10", 10 },
                {"French-Spade-A", 11 },
                {"French-Spade-K", 10 },
                {"French-Spade-Q", 10 },
                {"French-Spade-J", 10 },
            };

            static List<string> CardNames = new List<string>(cards.Keys);

            public static List<string> CardName { get { return CardClasses.GameState.CardNames; } }

            public static Dictionary<string, int> Cards
            {
                get { return cards; }
            }


            #endregion

            #region methods

            /// <summary>
            /// smooth upwards anim for hitting (always hit on 20)
            /// </summary>
            #endregion



        }

        public struct Card
        {

            #region fields

            private Microsoft.Xna.Framework.Vector2 position;

            readonly private int value;

            private bool drawn = false;

            Texture2D texture;

            readonly string name;

            #endregion

            #region properties

            public bool Drawn
            {
                get { return drawn; }
                set { drawn = value; }
            }
            public Texture2D cardTexture
            {
                get { return texture; }
            }

            public int Value
            {
                get { return value; }
            }

            public string Name
            {
                get { return name; }
            }

            #endregion

            #region constructor
            public Card(string name, Microsoft.Xna.Framework.Vector2 position, Texture2D texture)
            {
                this.name = name;
                value = GameState.Cards[name];
                this.position = position;
                this.texture = texture;
            }
            #endregion

            #region methods
            public void moveLeft()
            {
                for ( int i =  0; i < 20;  i++ ) 
                {
                    position.X -= 0.3f*i;
                }
                
            }
            

            public void HitAnim()
            {
                //anim speeds up as i goes up
                for (int i = 0; i < 30;  i++) 
                {
                    position.Y -= (i * 0.35f) /4;
                }
                //anim starts to slow down at end
                for (int i = 1; i < 10; i++)
                {
                    position.Y -= ((54 / i) * 0.35f) / 4;
                }
                
            }
            #endregion
        }
    }


} 
    


