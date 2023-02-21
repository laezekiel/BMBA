using Godot;
using System;
using System.Collections.Generic;

/* task list = Ctrl + \,T
 * 
 * enum:
 * TODO: add upgraded tools
 * 
 * assets:
 * TODO: find radio assets
 * TODO: isolate seing glasses assets
 * 
 * Scenes:
 * TODO: Set all tools scenes
 * TODO: Create missing Scenes
 * 
 * toolbelt:
 * TODO: Incorparate signal to open the belt and change tool
 * TODO: create ToolBelt Visual feedback
 * 
 */

/// <summary>
/// Stock all "value" of tools
/// </summary>
public enum AllTools
{
    Cane,
    PouchStone,
    Dog,
    Radio,
    SeeingGlasses,
    Empty,
}

/// <summary>
/// contains the method for the translation of the "value" of every enum of AllTools
/// </summary>
public static class ExtensionAllTools
{
    /// <summary>
    /// Get the PackedScene for the visual feedback of barry
    /// </summary>
    /// <param name="me"> is the enum tested </param>
    /// <returns></returns>
    public static PackedScene GetTool(this AllTools me)
    {
        PackedScene toolScene = (PackedScene)GD.Load("res://Scenes/Player/Tools/Empty.tscn");
        switch (me)
        {
            case AllTools.Cane:
                toolScene = (PackedScene)GD.Load("res://Scenes/Player/Tools/Cane.tscn");
                return toolScene;
            case AllTools.PouchStone:
                toolScene = (PackedScene)GD.Load("res://Scenes/Player/Tools/StonePouch.tscn");
                return toolScene;
            case AllTools.Dog:
                toolScene = (PackedScene)GD.Load("res://Scenes/Player/Tools/Dog.tscn");
                return toolScene;
            case AllTools.Radio:
                return toolScene;
            case AllTools.SeeingGlasses:
                return toolScene;
            default:
                return toolScene;
        }
    }
}

/// <summary>
/// Tool container
/// </summary>
public class ToolBelt
{
    public List<Enum> toolList = new List<Enum>();
    public Enum currentTool;

    public void Init()
    {
        for (int i = 0; i < Enum.GetNames(typeof(AllTools)).Length-1; i++)
        {
            toolList.Add((AllTools)i);
        }
    }

    

    /// <summary>
    /// set the first tool in tool belt as current tool
    /// if tool belt is empty set as such
    /// </summary>
    /// <param name="playerToolsList">player active tool list</param>
    public void CheckToolBelt(List<bool> playerToolsList)
    {
        foreach (bool tool in playerToolsList)
        {
            if (tool)
            {
                currentTool = (AllTools)playerToolsList.IndexOf(tool);
                break;
            }
            else
            {
                currentTool = AllTools.Empty;
            }
        }
    }
    
    
}
