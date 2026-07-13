using Cysharp.Threading.Tasks;
using TMPro;
using TripleMatch.Application.Services;
using TripleMatch.Application.StateMachine;
using TripleMatch.Application.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TripleMatch.Presentation.Meta
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;

        private int _levelIndex;
        private ILevelSelectionService _levelSelection;
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Construct(ILevelSelectionService levelSelection, IGameStateMachine stateMachine)
        {
            _levelSelection = levelSelection;
            _stateMachine = stateMachine;
            button.onClick.AddListener(OnClick);
        }

        // Set by LevelMapPresenter right after the factory creates this button - which
        // level it represents (levels are 0-indexed internally, shown 1-indexed to players).
        public void SetLevel(int levelIndex)
        {
            _levelIndex = levelIndex;
            label.text = (levelIndex + 1).ToString();
        }

        private void OnClick()
        {
            _levelSelection.SelectLevel(_levelIndex);
            _stateMachine.Enter<GameplayState>().Forget();
        }

        // Zero-argument factory: MetaInstaller decides how it's produced (spawn prefab + inject).
        public class Factory : PlaceholderFactory<LevelButtonView> { }
    }
}
