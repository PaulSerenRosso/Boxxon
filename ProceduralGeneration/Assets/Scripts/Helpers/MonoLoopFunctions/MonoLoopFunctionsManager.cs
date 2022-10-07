using System.Collections.Generic;
using UnityEngine;

namespace MonoLoopFunctions
{
    public abstract class MonoLoopFunctionsManager<I> : MonoBehaviour where I : class
    {
        static private HashSet<I> IMonoLoopFunctions = new();

        // Start is called before the first frame update
        static public void Register(I mono)
        {
            IMonoLoopFunctions.Add(mono);
        }

        static public void UnRegister(I mono)
        {
            IMonoLoopFunctions.Remove(mono);
        }

        public void LaunchLoop()
        {
            HashSet<I>.Enumerator e = IMonoLoopFunctions.GetEnumerator();
            while (e.MoveNext())
            {
                UpdateElement(e);
            }

            e.Dispose();
        }

        abstract public void UpdateElement(HashSet<I>.Enumerator e);
    }
}
