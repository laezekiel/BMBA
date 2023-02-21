using Godot;
using System;

/* task list = Ctrl + \,T
 * 
 * Scenes
 * TODO: create collectible Scene
 * TODO: create icon Scene
 * 
 * effect
 * TODO: code it
 * 
 */

public class Cane : Node
{
    [Export] NodePath caneBodyPath;

    public AnimatedSprite Body;

    public void Init()
    {
        Body = (AnimatedSprite)GetNode(caneBodyPath);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Init();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
