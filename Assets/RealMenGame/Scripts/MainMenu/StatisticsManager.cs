using RealMenGame.Scripts.Common;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace RealMenGame.Scripts.MainMenu {

	public class StatisticsManager : IInitializable {

		private readonly GameDataSerializerSimple _gameDataSerializerSimple = new GameDataSerializerSimple();
		private LocalStatistics _localStatistics;
		public LocalStatistics LocalStatistics => _localStatistics;
		private const string Key = "GameScore";

		public void Initialize() {
			Load();
		}


		internal bool TryToAddNewRecord(GameMode gameMode, ScoreData scoreData, out int index) {
			bool success = _localStatistics.GameModeStatistics[gameMode].TryToAddNewRecord(scoreData, out index);
			return success;
		}

		internal void Save() {
			PlayerPrefs.SetString(Key, _gameDataSerializerSimple.Serialize(_localStatistics));
			PlayerPrefs.Save();
		}

		internal void Load() {
			string dataStr = PlayerPrefs.GetString(Key, null);
			if (string.IsNullOrEmpty(dataStr)) {
				_localStatistics = new LocalStatistics();
			} else {
				_localStatistics = _gameDataSerializerSimple.Deserialize<LocalStatistics>(dataStr);
				if (_localStatistics == null) {
					_localStatistics = new LocalStatistics();
				} else {
					_localStatistics.Validate();
				}
			}
			Save();
		}

#if UNITY_EDITOR
		[MenuItem("RealMen/Game data/Clear statistics")]
		public static void ClearAllData() {
			PlayerPrefs.DeleteKey(Key);
			PlayerPrefs.Save();
		}
#endif

	}

}