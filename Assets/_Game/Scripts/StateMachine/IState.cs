using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void onEnter(Enemy enemy);
    void onExecute(Enemy enemy);
    void onExit(Enemy enemy);
}
