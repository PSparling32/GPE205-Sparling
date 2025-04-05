using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//interface for all actions
public interface IAction
{
    void Execute(AIController controller, AIContext context);
    bool IsDone { get; }
    void Reset();
}
