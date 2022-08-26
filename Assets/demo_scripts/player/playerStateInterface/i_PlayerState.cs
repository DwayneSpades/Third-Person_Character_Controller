using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerStates
{
    idle,
    walking,
    running,
    midAir,
    activeAir,
    landing,
    melee,
    dash
}

public interface i_PlayerState 
{
    void onEnter(player p);
    void onExit(player p);
    void update(player p);
}
