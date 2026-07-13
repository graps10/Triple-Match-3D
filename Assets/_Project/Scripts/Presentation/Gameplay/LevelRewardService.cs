using System;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using UnityEngine;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    public class LevelRewardService : IInitializable, IDisposable
    {
        private const float Three_Star_Seconds = 15f;
        private const float Two_Star_Seconds = 30f;
        private const int Coins_Per_Star = 10;

        private readonly SignalBus _signalBus;
        private readonly IEconomyService _economyService;

        private float _startTime;

        public LevelRewardService(SignalBus signalBus, IEconomyService economyService)
        {
            _signalBus = signalBus;
            _economyService = economyService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BoardBuiltSignal>(OnBoardBuilt);
            _signalBus.Subscribe<GameWonSignal>(OnGameWon);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BoardBuiltSignal>(OnBoardBuilt);
            _signalBus.Unsubscribe<GameWonSignal>(OnGameWon);
        }

        private void OnBoardBuilt(BoardBuiltSignal signal) => _startTime = Time.time;

        private void OnGameWon(GameWonSignal signal)
        {
            float elapsed = Time.time - _startTime;
            int stars = elapsed < Three_Star_Seconds ? 3 : elapsed < Two_Star_Seconds ? 2 : 1;
            int coins = stars * Coins_Per_Star;

            _economyService.AddCoins(coins);
            _signalBus.Fire(new LevelRewardSignal(stars, coins));
        }
    }
}
