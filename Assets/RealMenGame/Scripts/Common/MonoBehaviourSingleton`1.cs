// -------------------------------------------------------------------------
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
// Copyright (c) 2019-2020 Gear Games, LTD. All rights reserved.
// -------------------------------------------------------------------------

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RealMenGame.Scripts.Common
{

	public abstract class MonoBehaviourSingleton<TMonoBehaviour> : MonoBehaviour where TMonoBehaviour : MonoBehaviourSingleton<TMonoBehaviour>
	{
		private static TMonoBehaviour m_instance;
		private bool m_isPrimary;

		public static TMonoBehaviour Instance
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
			m_instance = (TMonoBehaviour) this;
#if UNITY_EDITOR
			if (EditorApplication.isPlaying)
#endif
			{
				DontDestroyOnLoad(gameObject);
			}
			OnSingletonAwake();
		}

		[UsedImplicitly]
		protected void Start()
		{
			if (m_isPrimary)
			{
				OnSingletonStart();
			}
		}

		[UsedImplicitly]
		protected virtual void OnDestroy()
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

		protected virtual void OnSingletonStart() { }

		protected virtual void OnSingletonDestroy() { }
	}

}