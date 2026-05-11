using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace _1;

public class Player
{
    private List<Texture2D> walk_anim;
    private List<Texture2D> jump_anim;
private List<Texture2D> atk_anim;
private SoundEffect sound_j, sound_a;
private Texture2D idle_p;
    private Vector2 p_pos;
    private Vector2 p_vel;
private bool is_air;
    private bool is_atk;
 private int cur_f;
    private double 
    t_val;
    private float my_size =
     4.0f;
    private bool is_run;

    public Vector2 Position => p_pos;
    public Vector2 Velocity => p_vel; 

    public Player(Texture2D s0, 
    List<Texture2D> s1, 
    List<Texture2D> s2, 
    List<Texture2D> s3, SoundEffect s_j, 
    SoundEffect s_a, Vector2 start)
    {
        idle_p = s0;
     walk_anim = s1;
        jump_anim = s2;
       
        atk_anim = s3;
        sound_j = s_j;
        
        sound_a = s_a;
        p_pos = start;
    }
 
    public void Update(GameTime gt, float floor_y)
    {
        var keys = Keyboard.GetState();
       
        var ms = Mouse.GetState();
        bool prev_air = is_air;
        is_run = false;

        if (ms.LeftButton == ButtonState.Pressed && !is_atk)
        {
            is_atk = true;
            cur_f = 0;
            t_val = 0;
            sound_a.Play();
        }
        

        if (!is_atk)
        {
            if (keys.IsKeyDown(Keys.D)) 
            { 
                p_vel.X = 5.15f; 
                is_run = true; 
            }
            else if (keys.IsKeyDown(Keys.A)) 
            { 
                p_vel.X = -5.15f; 
                is_run = true; 
            }
            else 
            { 
                p_vel.X = 0; 
            }

            if (keys.IsKeyDown(Keys.W) && !is_air) 
            { 
                p_vel.Y = -19.3f; 
                is_air = true; 
                cur_f = 0;
                sound_j.Play();
            }
        }

        p_vel.Y += 0.61f; 
        p_pos += p_vel;

        if (p_pos.Y >= floor_y)
        {
            p_pos.Y = floor_y;
            p_vel.Y = 0;
            is_air = false;
        }

        if (prev_air != is_air && !is_atk) 
            cur_f = 0;

        t_val +=
         gt.ElapsedGameTime.TotalSeconds;

        if (t_val > 0.088)
        {
            cur_f++;
            t_val = 0;

            if (is_atk)
            {
                if (cur_f >= 
                atk_anim.Count) 
                { 
                    is_atk = false; 
                    cur_f = 0; 
                }
            }
            else if (is_air)
            {
                if (cur_f >= 
                jump_anim.Count)
                 cur_f = 0;
            }
            else if (is_run)
            {
                if (cur_f >=
                 walk_anim.Count) 
                cur_f = 0;
            }
            else 
            {
                cur_f = 0;
            }
        }
    }
    #region DRAW
    public void Draw(SpriteBatch batch)
    {
        Texture2D sprite;

        if (is_atk) sprite = atk_anim[cur_f];
        else if (is_air) sprite = jump_anim[cur_f];
        else if (is_run) sprite = walk_anim[cur_f];
        else sprite = idle_p;

        Vector2 pivot = new Vector2(sprite.Width / 2f, sprite.Height);
        SpriteEffects fx = p_vel.X < 0 ? SpriteEffects.
        FlipHorizontally :  SpriteEffects.None;
        
        batch.Draw
        (sprite, p_pos, null, Color.White, 0f, pivot, my_size, 
        fx, 0f);
    }
    #endregion
}