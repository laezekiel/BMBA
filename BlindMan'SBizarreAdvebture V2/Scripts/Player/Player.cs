using Godot;
using System;
using System.Collections.Generic;

/* task list = Ctrl + \,T
 * 
 * tools:
 * TODO: player movement effect on animation 
 * TODO : player movement on ladder (get Ladder for this)
 * 
 * assets
 * TODO: define a distance for the side of a square : X
 * TODO: Resize outside godot all Barry Assets to equale 3X in height (careful tool assets need to be resize too) and center Barry
 * TODO: Create jump and climb assets
 * TODO: Kidnap someone to make all the needed animation
 * 
 * player:
 * TODO: move player According to input
 * 
 */

public class Player : KinematicBody2D
{
    // Barry's Camera
    [Export] NodePath cameraPath;
    public BarryCamera camera;

    // Barry's animated Sprite
    [Export] NodePath bodyPath;
    public AnimatedSprite body;

    #region Barry's animation
    // animation and list of animation
    public const string BARRY_IDLE = "idle";
    public const string BARRY_IDLE_POUCH = "idle with pouch";
    public List<string> idleList = new List<string>
    {
        BARRY_IDLE,
        BARRY_IDLE_POUCH,
    };
    public const string BARRY_WALK = "walk";
    public const string BARRY_WALK_CANE = "walk with cane";
    public const string BARRY_WALK_POUCH_LEFT = "walk with pouch left";
    public const string BARRY_WALK_POUCH_RIGHT = "walk with pouch right";
    public const string BARRY_WALK_POUCH_LEFT_CANE = "walk with pouch left and cane";
    public const string BARRY_WALK_POUCH_RIGHT_CANE = "walk with pouch right and cane";
    public const string BARRY_WALK_DOG = "walk with dog";
    public const string BARRY_WALK_DOG_CANE = "walk with dog and cane";
    public const string BARRY_WALK_DOG_POUCH_LEFT = "walk with dog and pouch left";
    public const string BARRY_WALK_DOG_POUCH_RIGHT = "walk with dog and pouch right";
    public const string BARRY_WALK_DOG_POUCH_LEFT_CANE = "walk with dog and pouch left and cane";
    public const string BARRY_WALK_DOG_POUCH_RIGHT_CANE = "walk with dog and pouch right and cane";
    public List<string> walkList = new List<string>
    {
        BARRY_WALK,
        BARRY_WALK_CANE,
        BARRY_WALK_POUCH_LEFT,
        BARRY_WALK_POUCH_RIGHT,
        BARRY_WALK_POUCH_LEFT_CANE,
        BARRY_WALK_POUCH_RIGHT_CANE,
        BARRY_WALK_DOG,
        BARRY_WALK_DOG_CANE,
        BARRY_WALK_DOG_POUCH_LEFT,
        BARRY_WALK_DOG_POUCH_RIGHT,
        BARRY_WALK_DOG_POUCH_LEFT_CANE,
        BARRY_WALK_DOG_POUCH_RIGHT_CANE,
    };

    // Animation in effect
    public string currentIdleAnimation = BARRY_IDLE;
    public string currentWalkAnimation = BARRY_WALK;
    #endregion

    // Instance of carried tools
    public Cane currentCane = null;
    public StonePouch currentStonePouch = null;
    public Dog currentDog = null;
    public Radio currentRadio = null;
    public SeeingGlasses currentSeeingGlasses = null;

    #region Barry's Input and Properties
    //// INPUTS
    //public const string MOVE_LEFT = "Move_Left";
    //public const string MOVE_RIGHT = "Move_Right";
    //public const string MOVE_UP = "Climb_Up";

    //// BARRY PROPRIETIES

    //public const float BARRY_SPEED = 0.3f;
    #endregion


    //Vector2 velocity;


    // toolBelt
    public ToolBelt toolBelt;
    public List<bool> activeToolList = new List<bool>();

    public void Init()
    {
        camera = (BarryCamera)GetNode(cameraPath);
        body = (AnimatedSprite)GetNode(bodyPath);

        #region initialisation of all possible tools
        for (int i = 0; i < Enum.GetNames(typeof(AllTools)).Length - 1; i++)
        {
            activeToolList.Add(false);
        }
        //test
        activeToolList[2] = true;
        activeToolList[0] = true;
        activeToolList[1] = true;

        toolBelt = new ToolBelt();
        toolBelt.Init();
        toolBelt.CheckToolBelt(activeToolList);
        #endregion

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Init();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        SetAllToolsAndAniamtion();
    }

    #region AJOUTS OWEN

    //public override void _PhysicsProcess(float delta)
    //{
    //    BarryMovementOnFloor()
    //    BarryMovementAnimation()
    //    BarryMovementOnLadder()
    //}

    
    //public void BarryMovementOnFloor()
    //{
    //    if (Input.IsActionPressed(MOVE_LEFT))
    //    {
    //        velocity = new Vector2(-BARRY_SPEED, 0f);
    //        player.Position += velocity;
    //    }
    //    else if (Input.IsActionPressed(MOVE_RIGHT))
    //    {
    //        velocity = new Vector2(BARRY_SPEED, 0.3f);
    //        player.Position += velocity;
    //    }
    //}


    //public void BarryMoveAnimation()
    //{
    //    if(Input.IsActionPressed(MOVE_LEFT) || Input.IsActionPressed(MOVE_RIGHT))
    //    {
    //        if (body.Animation != currentIdleAnimation) body.Animation = currentIdleAnimation;

    //        if (body.Animation != currentWalkAnimation) body.Animation = currentWalkAnimation;
    //    }
    //}


    //public void BarryMovementOnLadder() {}

    #endregion

    #region tool methods
    public void SetAllToolsAndAniamtion()
    {
        for (int i = 0; i < activeToolList.Count-1; i++)
        {
            SetToolToPlayerBody(i);
        }
        SetCurrentAnimation();
        SetToolAnimation();
    }

    public void SetToolToPlayerBody(int pIndex)
    {
        switch (pIndex)
        {
            case 0:
                if (activeToolList[pIndex])
                {
                    currentCane = (Cane)((AllTools)pIndex).GetTool().Instance();
                    body.AddChild(currentCane);
                }
                else if (!activeToolList[pIndex])
                {
                    if (currentCane != null)
                    {
                        body.RemoveChild(currentCane);
                    }
                    currentCane = null;
                }
                break;
            case 1:
                if (activeToolList[pIndex])
                {
                    currentStonePouch = (StonePouch)((AllTools)pIndex).GetTool().Instance();
                    body.AddChild(currentStonePouch);
                }
                else if (!activeToolList[pIndex])
                {
                    if (currentStonePouch != null)
                    {
                        body.RemoveChild(currentStonePouch);
                    }
                    currentStonePouch = null;
                }
                break;
            case 2:
                if (activeToolList[pIndex])
                {
                    currentDog = (Dog)((AllTools)pIndex).GetTool().Instance();
                    body.AddChild(currentDog);
                }
                else if (!activeToolList[pIndex])
                {
                    if (null != currentDog)
                    {
                        body.RemoveChild(currentDog);
                    }
                    currentDog = null;
                }
                break;
            case 3:
                if (activeToolList[pIndex])
                {
                    //currentRadio = (Radio)((AllTools)pIndex).GetTool().Instance();
                    //body.AddChild(currentRadio);
                }
                else if (!activeToolList[pIndex])
                {
                    if (null != currentRadio)
                    {
                        body.RemoveChild(currentRadio);
                    }
                    currentRadio = null;
                }
                break;
            case 4:
                if (activeToolList[pIndex])
                {
                    //currentSeeingGlasses = (SeeingGlasses)((AllTools)pIndex).GetTool().Instance();
                    //body.AddChild(currentSeeingGlasses);
                }
                else if (!activeToolList[pIndex])
                {
                    if (null != currentSeeingGlasses)
                    {
                        body.RemoveChild(currentSeeingGlasses);
                    }
                    currentSeeingGlasses = null;
                }
                break;
            default:
                break;
        }
        
    }
    public void SetCurrentAnimation()
    {
        if (currentCane == null && currentStonePouch == null && currentDog == null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE;
            currentWalkAnimation = BARRY_WALK;
        }
        else if (currentCane != null && currentStonePouch == null && currentDog == null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE;
            currentWalkAnimation = BARRY_WALK_CANE;
        }
        else if (currentCane == null && currentStonePouch != null && currentDog == null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE_POUCH;
            currentWalkAnimation = BARRY_WALK_POUCH_RIGHT;
        }
        else if (currentCane == null && currentStonePouch == null && currentDog != null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE;
            currentWalkAnimation = BARRY_WALK_DOG;
        }
        //else if (currentCane == null && currentStonePouch == null && currentDog == null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch == null && currentDog == null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        else if (currentCane != null && currentStonePouch != null && currentDog == null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE_POUCH;
            currentWalkAnimation = BARRY_WALK_POUCH_RIGHT_CANE;
        }
        else if (currentCane != null && currentStonePouch == null && currentDog != null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE;
            currentWalkAnimation = BARRY_WALK_DOG_CANE;
        }
        //else if (currentCane != null && currentStonePouch == null && currentDog == null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane != null && currentStonePouch == null && currentDog == null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        else if (currentCane == null && currentStonePouch != null && currentDog != null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE_POUCH;
            currentWalkAnimation = BARRY_WALK_DOG_POUCH_RIGHT;
        }
        //else if (currentCane == null && currentStonePouch != null && currentDog == null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch != null && currentDog == null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch == null && currentDog != null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch == null && currentDog != null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch == null && currentDog == null && currentRadio != null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        else if (currentCane != null && currentStonePouch != null && currentDog != null && currentRadio == null && currentSeeingGlasses == null)
        {
            currentIdleAnimation = BARRY_IDLE_POUCH;
            currentWalkAnimation = BARRY_WALK_DOG_POUCH_RIGHT_CANE;
        }
        //else if (currentCane != null && currentStonePouch != null && currentDog == null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane != null && currentStonePouch != null && currentDog == null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane != null && currentStonePouch == null && currentDog != null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane != null && currentStonePouch == null && currentDog != null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane != null && currentStonePouch == null && currentDog == null && currentRadio != null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch != null && currentDog != null && currentRadio != null && currentSeeingGlasses == null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch != null && currentDog != null && currentRadio == null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch != null && currentDog == null && currentRadio != null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation = 
        //    currentWalkAnimation = 
        //}
        //else if (currentCane == null && currentStonePouch == null && currentDog != null && currentRadio != null && currentSeeingGlasses != null)
        //{
        //    currentIdleAnimation =
        //    currentWalkAnimation = 
        //}
    }
    public void SetToolAnimation()
    {
        if (currentCane != null) 
        {
            if (body.Animation == BARRY_IDLE || body.Animation == BARRY_IDLE_POUCH)
            {
                currentCane.Body.Animation = BARRY_IDLE;
            }
            else if (body.Animation == BARRY_WALK_CANE || body.Animation == BARRY_WALK_POUCH_LEFT_CANE || body.Animation == BARRY_WALK_POUCH_RIGHT_CANE)
            {
                currentCane.Body.Animation = BARRY_WALK_CANE;
            }
            else if (body.Animation == BARRY_WALK_DOG_CANE || body.Animation == BARRY_WALK_DOG_POUCH_LEFT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_RIGHT_CANE)
            {
                currentCane.Body.Animation = BARRY_WALK_DOG_CANE;
            }
        }
        if (currentStonePouch != null) 
        {
            if (body.Animation == BARRY_IDLE_POUCH)
            {
                currentStonePouch.Body.Animation = BARRY_IDLE_POUCH;
            }
            else if (body.Animation == BARRY_WALK_POUCH_LEFT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_LEFT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_LEFT)
            {
                currentStonePouch.Body.Animation = BARRY_WALK_POUCH_LEFT;
            }
            else if (body.Animation == BARRY_WALK_POUCH_RIGHT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_RIGHT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_RIGHT)
            {
                currentStonePouch.Body.Animation = BARRY_WALK_POUCH_RIGHT;
            }
        }
        if (currentDog != null) 
        {
            if (body.Animation == BARRY_IDLE || body.Animation == BARRY_IDLE_POUCH)
            {
                currentDog.Body.Animation = BARRY_IDLE;
            }
            else if (body.Animation == BARRY_WALK_DOG_CANE || body.Animation == BARRY_WALK_DOG_POUCH_RIGHT || body.Animation == BARRY_WALK_DOG_POUCH_LEFT || body.Animation == BARRY_WALK_DOG_POUCH_LEFT_CANE || body.Animation == BARRY_WALK_DOG_POUCH_RIGHT_CANE)
            {
                currentDog.Body.Animation = BARRY_WALK_DOG;
            }
        }
        //if (currentRadio != null) { }
        //if (currentSeeingGlasses != null) { }
    }
    #endregion
}
