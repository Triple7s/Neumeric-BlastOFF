using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Equations", menuName = "Scriptable Objects/Equations", order = 1)]
public class SO_Equations : ScriptableObject
{
    public List<Question> questions = new List<Question>();
}
