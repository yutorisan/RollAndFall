using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace RAF.Input
{
    public interface IInputProvider
    {
        IObservable<Unit> InputtedKeyCode(KeyCode keyCode);
    }
    public class UserInputProvider : IInputProvider
    {
        public IObservable<Unit> InputtedKeyCode(KeyCode keyCode) =>
            Observable.EveryUpdate()
                      .Where(_ => UnityEngine.Input.GetKey(keyCode))
                      .AsUnitObservable();
    }
}