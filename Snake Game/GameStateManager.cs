using gamestates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gamestatemanager;

class StateManager
{
    private int screenWidth;
    private int screenHeight;
    private int blockSize;
    private MenuState menuState;
    private GameState gameState;
    private GameEndState gameEndState;
    private string gamestate;
    public StateManager(int sw, int sh, int bs, string startingState)
    {
        screenWidth = sw;
        screenHeight = sh;
        blockSize = bs;
        menuState = new MenuState(screenWidth, screenHeight);
        gameState = new GameState(screenWidth, screenHeight, blockSize);
        gameEndState = new GameEndState(screenWidth, screenHeight);
        gamestate = startingState;
    }

    public void Update()
    {
        if (gamestate == "menu")
        {
            gamestate = menuState.Update(gamestate);
            if (gamestate == "game")
            {
                gameState.Reset();
            }
        }

        if (gamestate == "game")
        {
            gamestate = gameState.Update(gamestate);
        }

        if (gamestate == "end")
        {
            gamestate = gameEndState.Update(gamestate);
            if (gamestate == "game")
            {
                gameState.Reset();
            }
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch _spriteBatch, Texture2D squareTexture, SpriteFont font)
    {
        if (gamestate == "menu")
        {
            graphicsDevice.Clear(Color.Black);
            menuState.Draw(_spriteBatch, squareTexture, font);
        }

        if (gamestate == "game")
        {
            graphicsDevice.Clear(new Color(108, 187, 60));
            gameState.Draw(_spriteBatch, squareTexture, font);
        }

        if (gamestate == "end")
        {
            graphicsDevice.Clear(Color.Black);
            gameEndState.Draw(_spriteBatch, squareTexture, font);
        }
    }
}