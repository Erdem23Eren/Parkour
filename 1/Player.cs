using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace _1;

public class Player
{
    private List<Texture2D> _walkTextures;
    private List<Texture2D> _jumpTextures;
    private List<Texture2D> _attackTextures;
    private SoundEffect _jumpSound;
    private SoundEffect _attackSound;
    private Texture2D _idleTexture;
    private Vector2 _position;
    private Vector2 _velocity;
    private bool _isJumping;
    private bool _isAttacking;
    private int _currentFrameIndex;
    private double _timer;
    private float _characterScale = 4.0f;
    private bool _isMoving;

    public Vector2 Position => _position;
    public Vector2 Velocity => _velocity; 

    public Player(Texture2D idle, List<Texture2D> walk, List<Texture2D> jump, List<Texture2D> attack, SoundEffect jumpSnd, SoundEffect attackSnd, Vector2 startPos)
    {
        _idleTexture = idle;
        _walkTextures = walk;
        _jumpTextures = jump;
        _attackTextures = attack;
        _jumpSound = jumpSnd;
        _attackSound = attackSnd;
        _position = startPos;
    }

    public void Update(GameTime gameTime, float groundY)
    {
        var kState = Keyboard.GetState();
        var mState = Mouse.GetState();
        bool wasJumping = _isJumping;
        _isMoving = false;

        if (mState.LeftButton == ButtonState.Pressed && !_isAttacking)
        {
            _isAttacking = true;
            _currentFrameIndex = 0;
            _timer = 0;
            _attackSound.Play();
        }

        if (!_isAttacking)
        {
            if (kState.IsKeyDown(Keys.D)) { _velocity.X = 5f; _isMoving = true; }
            else if (kState.IsKeyDown(Keys.A)) { _velocity.X = -5f; _isMoving = true; }
            else { _velocity.X = 0; }

            if (kState.IsKeyDown(Keys.W) && !_isJumping) 
            { 
                _velocity.Y = -20f; 
                _isJumping = true; 
                _currentFrameIndex = 0;
                _jumpSound.Play();
            }
        }

        _velocity.Y += 0.55f; 
        _position += _velocity;

       
        if (_position.Y >= groundY)
        {
            _position.Y = groundY;
            _velocity.Y = 0;
            _isJumping = false;
        }

        if (wasJumping != _isJumping && !_isAttacking) _currentFrameIndex = 0;

        _timer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer > 0.1)
        {
            _currentFrameIndex++;
            _timer = 0;
            if (_isAttacking)
            {
                if (_currentFrameIndex >= _attackTextures.Count) { _isAttacking = false; _currentFrameIndex = 0; }
            }
            else if (_isJumping)
            {
                if (_currentFrameIndex >= _jumpTextures.Count) _currentFrameIndex = 0;
            }
            else if (_isMoving)
            {
                if (_currentFrameIndex >= _walkTextures.Count) _currentFrameIndex = 0;
            }
            else _currentFrameIndex = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D textureToDraw;
        if (_isAttacking) textureToDraw = _attackTextures[_currentFrameIndex];
        else if (_isJumping) textureToDraw = _jumpTextures[_currentFrameIndex];
        else if (_isMoving) textureToDraw = _walkTextures[_currentFrameIndex];
        else textureToDraw = _idleTexture;

       
        Vector2 origin = new Vector2(textureToDraw.Width / 2f, textureToDraw.Height);
        SpriteEffects flip = _velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.Draw(textureToDraw, _position, null, Color.White, 0f, origin, _characterScale, flip, 0f);
    }
}