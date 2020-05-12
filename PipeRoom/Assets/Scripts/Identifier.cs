using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identifier : MonoBehaviour
{
    public enum PipeType { Long, Medium, Short, Left, Right, T, Outlet };
    public PipeType pipeType = PipeType.Medium;
}
