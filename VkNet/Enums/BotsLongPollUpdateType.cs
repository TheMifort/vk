using System.Runtime.Serialization;

namespace VkNet.Enums
{
	/// <summary>
	/// Тип обновления
	/// </summary>
	public enum GroupLongPollUpdateType
	{
		/// <summary>
		/// Не поддерживается
		/// </summary>
		NotSupported,

		/// <summary>
		/// Новое сообщение
		/// </summary>
		[EnumMember(Value = "message_new")]
		MessageNew,

		/// <summary>
		/// Новое исходящее сообщение
		/// </summary>
		[EnumMember(Value = "message_reply")]
		MessageReply,

		/// <summary>
		/// Редактирование сообщения
		/// </summary>
		[EnumMember(Value = "message_edit")]
		MessageEdit,

		/// <summary>
		/// Подписка на сообщения от сообщества
		/// </summary>
		[EnumMember(Value = "message_allow")]
		MessageAllow,

		/// <summary>
		/// Новый запрет сообщений от сообщества
		/// </summary>
		[EnumMember(Value = "message_deny")]
		MessageDeny,

		/// <summary>
		/// Добавление фотографии
		/// </summary>
		[EnumMember(Value = "photo_new")]
		PhotoNew,

		/// <summary>
		/// Добавление комментария к фотографии
		/// </summary>
		[EnumMember(Value = "photo_comment_new")]
		PhotoCommentNew,

		/// <summary>
		/// Редактирование комментария к фотографии
		/// </summary>
		[EnumMember(Value = "photo_comment_edit")]
		PhotoCommentEdit,

		/// <summary>
		/// Восстановление комментария к фотографии
		/// </summary>
		[EnumMember(Value = "photo_comment_restore")]
		PhotoCommentRestore,

		/// <summary>
		/// Удаление комментария к фотографии
		/// </summary>
		[EnumMember(Value = "photo_comment_delete")]
		PhotoCommentDelete,

		/// <summary>
		/// Добавление аудио
		/// </summary>
		[EnumMember(Value = "audio_new")]
		AudioNew,

		/// <summary>
		/// Добавление видео
		/// </summary>
		[EnumMember(Value = "video_new")]
		VideoNew,

		/// <summary>
		/// Добавление комментария к видео
		/// </summary>
		[EnumMember(Value = "video_comment_new")]
		VideoCommentNew,

		/// <summary>
		/// Редактирование комментария к видео
		/// </summary>
		[EnumMember(Value = "video_comment_edit")]
		VideoCommentEdit,

		/// <summary>
		/// Восстановление комментария к видео
		/// </summary>
		[EnumMember(Value = "video_comment_restore")]
		VideoCommentRestore,

		/// <summary>
		/// Удаление комментария к видео
		/// </summary>
		[EnumMember(Value = "video_comment_delete")]
		VideoCommentDelete,

		/// <summary>
		/// Добавление записи на стене
		/// </summary>
		[EnumMember(Value = "wall_post_new")]
		WallPostNew,

		/// <summary>
		/// Репост записи на стене
		/// </summary>
		[EnumMember(Value = "wall_repost")]
		WallRepost,

		/// <summary>
		/// Добавление комментария на стене
		/// </summary>
		[EnumMember(Value = "wall_reply_new")]
		WallReplyNew,

		/// <summary>
		/// Редактирование комментария на стене
		/// </summary>
		[EnumMember(Value = "wall_reply_edit")]
		WallReplyEdit,

		/// <summary>
		/// Восстановление комментария на стене
		/// </summary>
		[EnumMember(Value = "wall_reply_restore")]
		WallReplyRestore,

		/// <summary>
		/// Удаление комментария на стене
		/// </summary>
		[EnumMember(Value = "wall_reply_delete")]
		WallReplyDelete,

		/// <summary>
		/// Добавление комментария в обсуждении
		/// </summary>
		[EnumMember(Value = "board_post_new")]
		BoardPostNew,

		/// <summary>
		/// Редактирование комментария в обсуждении
		/// </summary>
		[EnumMember(Value = "board_post_edit")]
		BoardPostEdit,

		/// <summary>
		/// Восстановление комментария в обсуждении
		/// </summary>
		[EnumMember(Value = "board_post_restore")]
		BoardPostRestore,

		/// <summary>
		/// Удаление комментария в обсуждении
		/// </summary>
		[EnumMember(Value = "board_post_delete")]
		BoardPostDelete,

		/// <summary>
		/// Добавление комментария к товару
		/// </summary>
		[EnumMember(Value = "market_comment_new")]
		MarketCommentNew,

		/// <summary>
		/// Редактирование комментария к товару
		/// </summary>
		[EnumMember(Value = "market_comment_edit")]
		MarketCommentEdit,

		/// <summary>
		/// Восстановление комментария к товару
		/// </summary>
		[EnumMember(Value = "market_comment_restore")]
		MarketCommentRestore,

		/// <summary>
		/// Удаление комментария к товару
		/// </summary>
		[EnumMember(Value = "market_comment_delete")]
		MarketCommentDelete,

		/// <summary>
		/// Удаление участника из группы
		/// </summary>
		[EnumMember(Value = "group_leave")]
		GroupLeave,

		/// <summary>
		/// Добавление участника или заявки на вступление в сообщество
		/// </summary>
		[EnumMember(Value = "group_join")]
		GroupJoin,

		/// <summary>
		/// Добавление пользователя в черный список
		/// </summary>
		[EnumMember(Value = "user_block")]
		UserBlock,

		/// <summary>
		/// Удаление пользователя из черного списка
		/// </summary>
		[EnumMember(Value = "user_unblock")]
		UserUnblock,

		/// <summary>
		/// Добавление голоса в публичном опросе
		/// </summary>
		[EnumMember(Value = "poll_vote_new")]
		PollVoteNew,

		/// <summary>
		/// Редактирование списка руководителей
		/// </summary>
		[EnumMember(Value = "group_officers_edit")]
		GroupOfficersEdit,

		/// <summary>
		/// Изменение главного фото
		/// </summary>
		[EnumMember(Value = "group_change_photo")]
		GroupChangePhoto,
	}
}