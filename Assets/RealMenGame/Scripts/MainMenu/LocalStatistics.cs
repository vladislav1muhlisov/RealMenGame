using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RealMenGame.Scripts.MainMenu {

	public class LocalStatistics {

		public Dictionary<GameMode, GameModeStatistics> GameModeStatistics => _gameModeStatistics;
		private Dictionary<GameMode, GameModeStatistics> _gameModeStatistics;

		public LocalStatistics() {
			CreateNewData();
		}

		private void CreateNewData() {
			_gameModeStatistics = new Dictionary<GameMode, GameModeStatistics>();
			foreach (GameMode gameMode in Enum.GetValues(typeof(GameMode))) {
				_gameModeStatistics.Add(gameMode, new GameModeStatistics());
			}
		}

		[JsonConstructor]
		public LocalStatistics(Dictionary<GameMode, GameModeStatistics> gameModeStatistics) {
			_gameModeStatistics = gameModeStatistics;
		}

		public void Validate() {
			if (_gameModeStatistics == null) {
				CreateNewData();
				Debug.LogError("Local statistics data was invalid!");
			} else {
				foreach (GameMode gameMode in Enum.GetValues(typeof(GameMode))) {
					if (!_gameModeStatistics.ContainsKey(gameMode)) {
						_gameModeStatistics.Add(gameMode, new GameModeStatistics());
					}
				}
			}
		}

	}

}