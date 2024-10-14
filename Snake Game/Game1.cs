using System;
using gamestatemanager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake_Game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //game variables:
    private const int blockSize = 20;
    private const int screenWidth = blockSize * 17;
    private const int screenHeight = blockSize * 16;
    private const int framerate = 60;
    private Texture2D squareTexture;
    private SpriteFont font;
    private StateManager stateManager;
    //end game variables

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //setting screen dimensions
        _graphics.PreferredBackBufferWidth = screenWidth;
        _graphics.PreferredBackBufferHeight = screenHeight;
        _graphics.ApplyChanges();

        Window.Title = "Snake";

        //capping framerate
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / framerate);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        //initializing the state manager class
        var startingState = "menu";
        stateManager = new StateManager(screenWidth, screenHeight, blockSize, startingState);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        //defining blank square texture
        squareTexture = new Texture2D(GraphicsDevice, 1, 1);
        squareTexture.SetData(new[] { Color.White });

        //defining font
        font = Content.Load<SpriteFont>("MenuFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        stateManager.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        stateManager.Draw(GraphicsDevice, _spriteBatch, squareTexture, font);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
