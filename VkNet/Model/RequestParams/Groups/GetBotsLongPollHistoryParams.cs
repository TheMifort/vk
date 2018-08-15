using System;
using VkNet.Utils;

namespace VkNet.Model.RequestParams
{
	/// <summary>
	/// Параметры BotsLongPoll API
	/// </summary>
	[Serializable]
	public class GetBotsLongPollHistoryParams
	{
		/// <summary>
		/// Сервер для подключения Long Poll группы
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Последние полученое событие
		/// </summary>
		public ulong Ts { get; set; }

		/// <summary>
		/// Ключ сессии
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Время ожидание события
		/// </summary>
		public int Wait { get; set; }

		/// <summary>
		/// Привести к типу VkParameters.
		/// </summary>
		/// <param name="p"> Параметры. </param>
		/// <returns> </returns>
		public static VkParameters ToVkParameters(GetBotsLongPollHistoryParams p)
		{
			var parameters = new VkParameters
			{
				{ "ts", p.Ts }
				, { "key", p.Key }
				, { "wait", p.Wait }
				,{"act","a_check"}
			};

			return parameters;
		}
	}
}