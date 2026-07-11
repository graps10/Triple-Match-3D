using System;
using TripleMatch.Application.Services;
using TripleMatch.Application.Signals;
using Zenject;

namespace TripleMatch.Presentation.Gameplay
{
    // Owns the Playing/Won/Lost outcome: listens to the "facts" Board/Tray report
    // (BoardBuiltSignal, MatchMadeSignal, TrayOverflowSignal) and decides what they mean
    // for the level, firing GameWonSignal/GameLostSignal. Neither Board nor Tray need to
    // know win/lose exists at all.
    public class GameplayOutcomeService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ILogService _log;

        private int _remainingItems;
        private bool _isOver;

        public GameplayOutcomeService(SignalBus signalBus, ILogService log)
        {
            _signalBus = signalBus;
            _log = log;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BoardBuiltSignal>(OnBoardBuilt);
            _signalBus.Subscribe<MatchMadeSignal>(OnMatchMade);
            _signalBus.Subscribe<TrayOverflowSignal>(OnTrayOverflow);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BoardBuiltSignal>(OnBoardBuilt);
            _signalBus.Unsubscribe<MatchMadeSignal>(OnMatchMade);
            _signalBus.Unsubscribe<TrayOverflowSignal>(OnTrayOverflow);
        }

        private void OnBoardBuilt(BoardBuiltSignal signal) => _remainingItems = signal.ItemCount;

        private void OnMatchMade(MatchMadeSignal signal)
        {
            if (_isOver)
                return;

            _remainingItems -= signal.Length;
            if (_remainingItems > 0)
                return;

            _isOver = true;
            _log.Info("Game Won! All items matched.");
            _signalBus.Fire(new GameWonSignal());
        }

        private void OnTrayOverflow(TrayOverflowSignal signal)
        {
            if (_isOver)
                return;

            _isOver = true;
            _log.Info("Game Lost! Tray is full with no match available.");
            _signalBus.Fire(new GameLostSignal());
        }
    }
}
