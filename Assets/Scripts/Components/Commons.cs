using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi
{
    public enum GameState
    {
        OnReady,
        Racing,
        RaceLost,
        RaceWon,
        RaceFinished,
        Wallpaint,
        Endgame,
        MainMenu
    }

    public enum UICommand
    {
        StartGame,
        StartRace,
        NextLevel,
        ResetLevel,
        RestartGame
    }

    public enum GameMode
    {
        Single,
        PVE
    }

    public enum ObjTags
    {
        Player,
        Character,
        Obstacle        
    }

}
