using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snakecomponents;

namespace gamestates;

interface IState
{
    void Reset();
    string Update(string gamestate);
    void Draw(SpriteBatch _spriteBatch, Texture2D squareTexture, SpriteFont font);
}

class MenuState : IState
{
    private int screenWidth;
    private int screenHeight;
    public MenuState(int sw, int sh)
    {
        screenWidth = sw;
        screenHeight = sh;
    }

    public void Reset()
    {
    }

    public string Update(string gamestate)
    {
        var keystate = Keyboard.GetState();
        if (keystate.IsKeyDown(Keys.Space))
        {
            gamestate = "game";
        }
        return gamestate;
    }

    public void Draw(SpriteBatch _spriteBatch, Texture2D squareTexture, SpriteFont font)
    {
        _spriteBatch.DrawString(font, "press space", new Vector2(screenWidth / 2, screenHeight / 2) - font.MeasureString("press space") / 2, Color.White);
    }
}

class GameState : IState
{
    private int screenWidth;
    private int screenHeight;
    private int blockSize;
    private Rectangle topBorder;
    private PlayerSnake player;
    private int gameCounter;
    private AppleGenerator appleGenerator;
    private int score;
    public GameState(int sw, int sh, int bs)
    {
        screenWidth = sw;
        screenHeight = sh;
        blockSize = bs;
        topBorder = new Rectangle(0, 0, screenWidth, blockSize);
        player = new PlayerSnake(screenWidth, screenHeight, blockSize);
        appleGenerator = new AppleGenerator(blockSize);
    }

    public void Reset()
    {
        player = new PlayerSnake(screenWidth, screenHeight, blockSize);
        gameCounter = 0;
        appleGenerator = new AppleGenerator(blockSize);
        score = 0;
    }

    public string Update(string gamestate)
    {
        //taking player inputs and moving the player
        if (gameCounter % 6 == 0)
        {
            var keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.Right) && player.moveQueue[0] != 'l' && player.moveQueue[0] != 'r')
            {
                player.AddMoveQueue('r');
            }
            if (keystate.IsKeyDown(Keys.Left) && player.moveQueue[0] != 'r' && player.moveQueue[0] != 'l')
            {
                player.AddMoveQueue('l');
            }
            if (keystate.IsKeyDown(Keys.Up) && player.moveQueue[0] != 'd' && player.moveQueue[0] != 'u')
            {
                player.AddMoveQueue('u');
            }
            if (keystate.IsKeyDown(Keys.Down) && player.moveQueue[0] != 'u' && player.moveQueue[0] != 'd')
            {
                player.AddMoveQueue('d');
            }
            player.Move();
        }
        //incrementing game counter
        gameCounter += 1;

        //checking if the player has collided with the apple
        if (player.head.Intersects(appleGenerator.apple))
        {
            score += 1;
            player.AddSegment();
            appleGenerator.GenerateApple(player.body);
        }

        //checking if the player is dead
        if (player.IsDead())
        {
            gamestate = "end";
        }
        return gamestate;
    }

    public void Draw(SpriteBatch _spriteBatch, Texture2D squareTexture, SpriteFont font)
    {
        //drawing the snake
        foreach (Rectangle segment in player.body)
        {
            _spriteBatch.Draw(squareTexture, new Vector2(segment.X, segment.Y), segment, Color.RoyalBlue, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
        //drawing the head
        _spriteBatch.Draw(squareTexture, new Vector2(player.head.X, player.head.Y), player.head, Color.MidnightBlue, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        //drawing the apple
        _spriteBatch.Draw(squareTexture, new Vector2(appleGenerator.apple.X, appleGenerator.apple.Y), appleGenerator.apple, Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        //displaying score
        _spriteBatch.Draw(squareTexture, new Vector2(0, 0), topBorder, Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        _spriteBatch.DrawString(font, $"score: {score}", new Vector2(0, 0), Color.White);
    }
}

class GameEndState : IState
{
    private int screenWidth;
    private int screenHeight;
    public GameEndState(int sw, int sh)
    {
        screenWidth = sw;
        screenHeight = sh;
    }

    public void Reset()
    {
    }

    public string Update(string gamestate)
    {
        var keystate = Keyboard.GetState();
        if (keystate.IsKeyDown(Keys.Space))
        {
            gamestate = "game";
        }
        return gamestate;
    }

    public void Draw(SpriteBatch _spriteBatch, Texture2D squareTexture, SpriteFont font) {
        _spriteBatch.DrawString(font, "game over", new Vector2(screenWidth / 2, screenHeight / 2) - font.MeasureString("game over") / 2, Color.Red);
        _spriteBatch.DrawString(font, "press space", new Vector2(screenWidth / 2 - font.MeasureString("press space").X / 2, screenHeight / 2 + font.MeasureString("game over").Y / 2), Color.Red);
    }
}