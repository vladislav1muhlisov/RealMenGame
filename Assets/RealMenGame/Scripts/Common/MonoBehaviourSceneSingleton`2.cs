// -------------------------------------------------------------------------
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
// Copyright (c) 2019-2020 Gear Games, LTD. All rights reserved.
// -------------------------------------------------------------------------

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RealMenGame.Scripts.Common
{

	public abstract class MonoBehaviourSceneSingleton<TMonoBehaviour, TInstance> : MonoBehaviour where TMonoBehaviour : MonoBehaviourSceneSingleton<TMonoBehaviour, TInstance> where TInstance : class
	{
		private static TInstance m_instance;
		private bool m_isPrimary;

		public static TInstance Instance
		{
			get
			{
				return m_instance;
			}
		}

		[UsedImplicitly]
		protected virtual void Awake()
		{
			if (m_instance != null)
			{
				m_isPrimary = false;
				Destroy(gameObject);
				return;
			}
			m_isPrimary = true;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
// #if UNITY_EDITOR
// 			if (UnityEditor.EditorApplication.isPlaying)
// #endif
// 			{
// 				DontDestroyOnLoad(gameObject);
// 			}
			OnSingletonAwake();
			m_instance = CreateInstance();
		}

		[UsedImplicitly]
		protected virtual void Start()
		{
			if (m_isPrimary)
			{
				OnSingletonStart();
			}
		}

		[UsedImplicitly]
		protected virtual void Destroy()
		{
			if (!m_isPrimary)
			{
				return;
			}

			try
			{
				OnSingletonDestroy();
			}
			finally
			{
				m_instance = null;
			}
		}

		protected virtual void OnSingletonAwake() { }

		[NotNull]
		protected abstract TInstance CreateInstance();

		protected virtual void OnSingletonStart() { }

		protected virtual void OnSingletonDestroy() { }
		
		private void OnSceneUnloaded(Scene scene)
		{
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			Destroy();
		}

	}

}