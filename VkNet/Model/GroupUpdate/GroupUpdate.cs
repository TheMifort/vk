using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Enums;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate
{
	/// <summary>
	/// Обновление группы
	/// </summary>
	[Serializable]
	public class GroupUpdate
	{
		/// <summary>
		/// Тип обновления
		/// </summary>
		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public GroupUpdateType Type { get; set; }

		/// <summary>
		/// Сообщение для типов событий с сообщением в ответе(MessageNew, MessageEdit, MessageReply)
		/// </summary>
		public Message Message { get; set; }

		/// <summary>
		/// Фотография для типов событий с фотографией в ответе(PhotoNew)
		/// </summary>
		public Photo Photo { get; set; }

		/// <summary>
		/// Аудиозапись
		/// </summary>
		public Audio Audio { get; set; }

		/// <summary>
		/// Видеозапись
		/// </summary>
		public Video Video { get; set; }

		/// <summary>
		/// Подписка на сообщения от сообщества
		/// </summary>
		public MessageAllow MessageAllow { get; set; }

		/// <summary>
		/// Новый запрет сообщений от сообщества(MessageDeny)
		/// </summary>
		public MessageDeny MessageDeny { get; set; }

		/// <summary>
		/// Добавление/редактирование/восстановление комментария к фотографии(PhotoCommentNew, PhotoCommentEdit, PhotoCommentRestore)
		/// </summary>
		public PhotoComment PhotoComment { get; set; }

		/// <summary>
		/// Удаление комментария к фотографии(PhotoCommentDelete)
		/// </summary>
		public PhotoCommentDelete PhotoCommentDelete { get; set; }

		/// <summary>
		/// Добавление/редактирование/восстановление комментария к видео(VideoCommentNew, VideoCommentEdit, VideoCommentRestore)
		/// </summary>
		public VideoComment VideoComment { get; set; }

		/// <summary>
		/// Удаление комментария к видео(VideoCommentDelete)
		/// </summary>
		public VideoCommentDelete VideoCommentDelete { get; set; }

		/// <summary>
		/// Добавление/редактирование/восстановление комментария в обсуждении(BoardPostNew, BoardPostEdit, BoardPostRestore)
		/// </summary>
		public BoardPost BoardPost { get; set; }

		/// <summary>
		/// Удаление комментария в обсуждении(BoardPostDelete)
		/// </summary>
		public BoardPostDelete BoardPostDelete { get; set; }

		/// <summary>
		/// Изменение главного фото
		/// </summary>
		public GroupChangePhoto GroupChangePhoto { get; set; }

		/// <summary>
		/// Добавление участника или заявки на вступление в сообщество
		/// </summary>
		public GroupJoin GroupJoin { get; set; }

		/// <summary>
		/// Удаление/выход участника из сообщества
		/// </summary>
		public GroupLeave GroupLeave { get; set; }

		/// <summary>
		/// Редактирование списка руководителей
		/// </summary>
		public GroupOfficersEdit GroupOfficersEdit { get; set; }

		/// <summary>
		/// Добавление/редактирование/восстановление комментария к товару(MarketCommentNew, MarketCommentEdit, MarketCommentRestore)
		/// </summary>
		public MarketComment MarketComment { get; set; }

		/// <summary>
		/// Удаление комментария к товару(MarketCommentDelete)
		/// </summary>
		public MarketCommentDelete MarketCommentDelete { get; set; }

		/// <summary>
		/// Добавление голоса в публичном опросе
		/// </summary>
		public PollVoteNew PollVoteNew { get; set; }

		/// <summary>
		/// Добавление пользователя в чёрный список
		/// </summary>
		public UserBlock UserBlock { get; set; }

		/// <summary>
		/// Удаление пользователя из чёрного списка
		/// </summary>
		public UserUnblock UserUnblock { get; set; }

		/// <summary>
		/// Новая запись на стене(WallPost, WallRepost)
		/// </summary>
		public WallPost WallPost { get; set; }

		/// <summary>
		/// Добавление/редактирование/восстановление комментария на стене(WallReplyNew, WallReplyEdit, WallReplyRestore)
		/// </summary>
		public WallReply WallReply { get; set; }

		/// <summary>
		/// Удаление комментария к записи(WallReplyDelete)
		/// </summary>
		public WallReplyDelete WallReplyDelete { get; set; }

		/// <summary>
		/// ID группы
		/// </summary>
		[JsonProperty("group_id")]
		public ulong? GroupId { get; set; }

		/// <summary>
		/// Идентификатор пользователя, который вызвал обновление группы
		/// </summary>
		public long? UserId { get; set; }

		/// <summary>
		/// Разобрать из json.
		/// </summary>
		/// <param name="response"> Ответ сервера. </param>
		/// <returns> </returns>
		public static GroupUpdate FromJson(VkResponse response)
		{
			var fromJson = JsonConvert.DeserializeObject<GroupUpdate>(response.ToString());

			var resObj = response["object"];

			switch (fromJson.Type)
			{
				case GroupUpdateType.MessageNew:
				case GroupUpdateType.MessageEdit:
				case GroupUpdateType.MessageReply:
					fromJson.Message = resObj;
					fromJson.UserId = fromJson.Message.FromId;

					break;
				case GroupUpdateType.MessageAllow:
					fromJson.MessageAllow = MessageAllow.FromJson(resObj);
					fromJson.UserId = fromJson.MessageAllow.UserId;

					break;
				case GroupUpdateType.MessageDeny:
					fromJson.MessageDeny = MessageDeny.FromJson(resObj);
					fromJson.UserId = fromJson.MessageDeny.UserId;

					break;
				case GroupUpdateType.PhotoNew:
					fromJson.Photo = resObj;

					break;
				case GroupUpdateType.PhotoCommentNew:
				case GroupUpdateType.PhotoCommentEdit:
				case GroupUpdateType.PhotoCommentRestore:
					fromJson.PhotoComment = PhotoComment.FromJson(resObj);
					fromJson.UserId = fromJson.PhotoComment.FromId;

					break;
				case GroupUpdateType.PhotoCommentDelete:
					fromJson.PhotoCommentDelete = PhotoCommentDelete.FromJson(resObj);
					fromJson.UserId = fromJson.PhotoCommentDelete.DeleterId;

					break;
				case GroupUpdateType.AudioNew:
					fromJson.Audio = resObj;

					break;
				case GroupUpdateType.VideoNew:
					fromJson.Video = resObj;

					break;
				case GroupUpdateType.VideoCommentNew:
				case GroupUpdateType.VideoCommentEdit:
				case GroupUpdateType.VideoCommentRestore:
					fromJson.VideoComment = VideoComment.FromJson(resObj);
					fromJson.UserId = fromJson.VideoComment.FromId;

					break;
				case GroupUpdateType.VideoCommentDelete:
					fromJson.VideoCommentDelete = VideoCommentDelete.FromJson(resObj);
					fromJson.UserId = fromJson.VideoCommentDelete.DeleterId;

					break;
				case GroupUpdateType.WallPostNew:
				case GroupUpdateType.WallRepost:
					fromJson.WallPost = WallPost.FromJson(resObj);
					fromJson.UserId = fromJson.WallPost.FromId > 0 ? fromJson.WallPost.FromId : null;

					break;
				case GroupUpdateType.WallReplyNew:
				case GroupUpdateType.WallReplyEdit:
				case GroupUpdateType.WallReplyRestore:
					fromJson.WallReply = WallReply.FromJson(resObj);
					fromJson.UserId = fromJson.WallReply.FromId;

					break;
				case GroupUpdateType.WallReplyDelete:
					fromJson.WallReplyDelete = WallReplyDelete.FromJson(resObj);
					fromJson.UserId = fromJson.WallReplyDelete.DeleterId;

					break;
				case GroupUpdateType.BoardPostNew:
				case GroupUpdateType.BoardPostEdit:
				case GroupUpdateType.BoardPostRestore:
					fromJson.BoardPost = BoardPost.FromJson(resObj);
					fromJson.UserId = fromJson.BoardPost.FromId > 0 ? fromJson.BoardPost.FromId : (long?) null;

					break;
				case GroupUpdateType.BoardPostDelete:
					fromJson.BoardPostDelete = BoardPostDelete.FromJson(resObj);

					break;
				case GroupUpdateType.MarketCommentNew:
				case GroupUpdateType.MarketCommentEdit:
				case GroupUpdateType.MarketCommentRestore:
					fromJson.MarketComment = MarketComment.FromJson(resObj);
					fromJson.UserId = fromJson.MarketComment.FromId;

					break;
				case GroupUpdateType.MarketCommentDelete:
					fromJson.MarketCommentDelete = MarketCommentDelete.FromJson(resObj);
					fromJson.UserId = fromJson.MarketCommentDelete.DeleterId;

					break;
				case GroupUpdateType.GroupLeave:
					fromJson.GroupLeave = GroupLeave.FromJson(resObj);
					fromJson.UserId = fromJson.GroupLeave.IsSelf == true ? fromJson.GroupLeave.UserId : null;

					break;
				case GroupUpdateType.GroupJoin:
					fromJson.GroupJoin = GroupJoin.FromJson(resObj);
					fromJson.UserId = fromJson.GroupJoin.UserId;

					break;
				case GroupUpdateType.UserBlock:
					fromJson.UserBlock = UserBlock.FromJson(resObj);
					fromJson.UserId = fromJson.UserBlock.AdminId;

					break;
				case GroupUpdateType.UserUnblock:
					fromJson.UserUnblock = UserUnblock.FromJson(resObj);
					fromJson.UserId = fromJson.UserUnblock.ByEndDate == true ? null : fromJson.UserUnblock.AdminId;

					break;
				case GroupUpdateType.PollVoteNew:
					fromJson.PollVoteNew = PollVoteNew.FromJson(resObj);
					fromJson.UserId = fromJson.PollVoteNew.UserId;

					break;
				case GroupUpdateType.GroupChangePhoto:
					fromJson.GroupChangePhoto = GroupChangePhoto.FromJson(resObj);
					fromJson.UserId = fromJson.GroupChangePhoto.UserId;

					break;
				case GroupUpdateType.GroupOfficersEdit:
					fromJson.GroupOfficersEdit = GroupOfficersEdit.FromJson(resObj);
					fromJson.UserId = fromJson.GroupOfficersEdit.AdminId;

					break;
			}

			return fromJson;
		}
	}
}