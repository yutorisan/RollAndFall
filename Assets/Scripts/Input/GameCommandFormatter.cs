using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace RAF.Input
{
    public interface IGameCommandProvider
    {
        /// <summary>
        /// すべてのGameCommand発行通知を取得する
        /// </summary>
        /// <returns></returns>
        IObservable<GameCommand> GameCommandInputted();
        /// <summary>
        /// 特定のGameCommandの発行通知を取得する
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IObservable<Unit> GameCommandInputted(GameCommand command);
    }
    public class GameCommandFormatter : IGameCommandProvider
    {
        #region singleton
        private static IGameCommandProvider _instance;
        public static IGameCommandProvider Instance => _instance ??= new GameCommandFormatter();
        #endregion

        private static readonly IReadOnlyDictionary<KeyCode, GameCommand> KeyMap = new Dictionary<KeyCode, GameCommand>()
        {
            {KeyCode.LeftArrow, GameCommand.RotateLeft },
            {KeyCode.RightArrow, GameCommand.RotateRigth },
            {KeyCode.Space, GameCommand.PlayerJump }
        };

        private readonly IInputProvider m_inputProvider = new UserInputProvider();

        public IObservable<GameCommand> GameCommandInputted() =>
            KeyMap.Select(kvp => m_inputProvider.InputtedKeyCode(kvp.Key)
                                                .Select(_ => kvp.Value))
                  .Merge()
                  .Share();

        public IObservable<Unit> GameCommandInputted(GameCommand command) =>
            GameCommandInputted().Where(cmd => cmd == command).AsUnitObservable();
    }

    public enum GameCommand
    {
        RotateLeft,
        RotateRigth,
        PlayerJump
    }
}