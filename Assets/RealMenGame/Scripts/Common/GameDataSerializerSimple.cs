using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace RealMenGame.Scripts.Common {

	public class GameDataSerializerSimple {

		private readonly JsonSerializer _serializer;

		public GameDataSerializerSimple() {
			_serializer = JsonSerializer.Create();
		}

		[CanBeNull]
		public string Serialize<T>(T gameSessionData) where T : class {
			try {
				var stringBuilder = new StringBuilder();
				_serializer.Serialize(new JsonTextWriter(new StringWriter(stringBuilder)), gameSessionData);
				return stringBuilder.ToString();
			} catch (Exception e) {
				Debug.LogError($"Can not serialize game data: {e}");
			}
			return null;
		}

		[CanBeNull]
		public T Deserialize<T>(string str) where T : class {
			try {
				var gameData = _serializer.Deserialize<T>(new JsonTextReader(new StringReader(str)));
				return gameData;
			} catch (Exception e) {
				Debug.LogError($"Can not deserialize game data: {e}");
			}
			return null;
		}

	}

}