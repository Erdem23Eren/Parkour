using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _1;

public class Menu
{
    private SpriteFont _font;
    private Vector2 _screenCenter;
        private string[] _options = { "START", "EXIT" };
         private int _selectedIndex = 0;
    private KeyboardState   _previousState;
    public Menu(SpriteFont font,
     Viewport viewport)
    {
        _font = font;
        _screenCenter = new Vector2(viewport.Width / 
        2f, viewport.Height / 2f);
    }

    public int Update()
    {
        var kState = Keyboard.GetState();
        int result = -1; 

        if (kState.IsKeyDown(Keys.W) &&
         _previousState.IsKeyUp(Keys.W))
            _selectedIndex = (_selectedIndex - 1 + _options.Length)
             % _options.Length;
        
        if (kState.IsKeyDown(Keys.S) && _previousState.IsKeyUp(Keys.S))
            _selectedIndex = (_selectedIndex + 1) % _options.Length;

        if (kState.IsKeyDown(Keys.Enter) && _previousState.IsKeyUp(Keys.Enter))
            result = _selectedIndex;

        _previousState = kState;
        return result;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i 
        < _options.Length; i++)
        {
            Color color = (i == _selectedIndex) ? Color.Yellow :
                Color.White;
            Vector2 size = _font.MeasureString(_options[i]);
                Vector2 position = new Vector2(_screenCenter.X - (size.X / 2f),
             _screenCenter.Y - 50 + (i * 60));
            
            spriteBatch.DrawString(_font, _options[i], 
            position, color);
        }
    }
}

#region  Erdem için not




#endregion