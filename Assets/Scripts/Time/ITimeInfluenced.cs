using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Time
{
    public interface ITimeInfluenced
    {
        void Pause();
        void Resume();
    }
}
