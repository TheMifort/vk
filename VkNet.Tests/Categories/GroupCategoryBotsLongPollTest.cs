using System;
using System.Linq;
using NUnit.Framework;
using VkNet.Categories;
using VkNet.Enums;
using VkNet.Exception;
using VkNet.Model.RequestParams;

namespace VkNet.Tests.Categories
{
	[TestFixture]
	public class GroupCategoryBotsLongPollTest : BaseTest
	{
		private GroupsCategory GetMockedGroupCategory(string url, string json)
		{
			Json = json;
			Url = url;

			return new GroupsCategory(Api);
		}

		private string GetFullResponse(string updateJson)
		{
			return $"{{'ts': '713','updates': [{updateJson}]}}";
		}

		[Test]
		public void GetBotsLongPollHistory_Failed1()
		{
			const string json = "{\"failed\":1, \"ts\":10}";
			var groupCategory = GetMockedGroupCategory("https://vk.com", json);

			Assert.Throws<BotsLongPollOutdateException>(() => groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			}));
		}

		[Test]
		public void GetBotsLongPollHistory_Failed1Ts()
		{
			const string json = "{\"failed\":1, \"ts\":10}";
			var groupCategory = GetMockedGroupCategory("https://vk.com", json);

			const int ts = 10;

			try
			{
				groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
				{
					Key = "test",
					Server = "https://vk.com",
					Ts = 0,
					Wait = 10
				});

				Assert.Fail();
			}
			catch (BotsLongPollOutdateException exception)
			{
				Assert.AreEqual(ts, exception.Ts);
			}
		}

		[Test]
		public void GetBotsLongPollHistory_Failed2()
		{
			const string json = "{\"failed\":2}";
			var groupCategory = GetMockedGroupCategory("https://vk.com", json);

			Assert.Throws<BotsLongPollKeyExpiredException>(() => groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			}));
		}

		[Test]
		public void GetBotsLongPollHistory_Failed3()
		{
			const string json = "{\"failed\":3}";
			var groupCategory = GetMockedGroupCategory("https://vk.com", json);

			Assert.Throws<BotsLongPollInfoLostException>(() => groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			}));
		}

		[Test]
		public void GetBotsLongPollHistory_BoardPostNew()
		{
			const string json = @"{
  'type': 'board_post_new',
  'object': {
    'id': 3,
    'from_id': 123,
    'date': 1533404752,
    'text': 'test',
    'topic_owner_id': -1234,
    'topic_id': 38446232
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.BoardPost.FromId);
			Assert.AreEqual(text, update.BoardPost.Text);
			Assert.AreEqual(-groupId, update.BoardPost.TopicOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_BoardPostNewFirst()
		{
			const string json = @"{
  'type': 'board_post_new',
  'object': {
    'id': 2,
    'from_id': -1234,
    'date': 1533404708,
    'text': 'test',
    'topic_owner_id': -1234,
    'topic_id': 6
  },
  'group_id': 1234
}";

			const int groupId = 1234;
			const string text = "test";
			const int topicId = 6;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(-groupId, update.BoardPost.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.BoardPost.Text);
			Assert.AreEqual(-groupId, update.BoardPost.TopicOwnerId);
			Assert.AreEqual(topicId, update.BoardPost.TopicId);
		}

		[Test]
		public void GetBotsLongPollHistory_BoardPostEditTest()
		{
			const string json = @"{
  'type': 'board_post_edit',
  'object': {
    'id': 2,
    'from_id': -1234,
    'date': 1533404708,
    'text': 'test1',
    'topic_owner_id': -1234,
    'topic_id': 38446232
  },
  'group_id': 1234
}";

			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(-groupId, update.BoardPost.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.BoardPost.Text);
			Assert.AreEqual(-groupId, update.BoardPost.TopicOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_BoardPostRestoreTest()
		{
			const string json = @"{
  'type': 'board_post_restore',
  'object': {
    'id': 3,
    'from_id': 123,
    'date': 1533404752,
    'text': 'test',
    'topic_owner_id': -1234,
    'topic_id': 38446232
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.BoardPost.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.BoardPost.Text);
			Assert.AreEqual(-groupId, update.BoardPost.TopicOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_BoardPostDeleteTest()
		{
			const string json = @"{
  'type': 'board_post_delete',
  'object': {
    'topic_owner_id': -1234,
    'id': 3,
    'topic_id': 6
  },
  'group_id': 1234
}";

			const int groupId = 1234;
			const int topicId = 6;
			const int id = 3;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.BoardPostDelete.TopicOwnerId);
			Assert.AreEqual(topicId, update.BoardPostDelete.TopicId);
			Assert.AreEqual(id, update.BoardPostDelete.Id);
		}

		[Test]
		public void GetBotsLongPollHistory_GroupChangePhotoTest()
		{
			const string json = @"{
  'type': 'group_change_photo',
  'object': {
    'user_id': 123,
    'photo': {
      'id': 4444,
      'album_id': -6,
      'owner_id': -1234,
      'user_id': 100,
      'sizes': [
        {
          'type': 's',
          'url': 'https://sun1-2.userapi.com/c830609/v830609207/15e4db/SSBTALZRXxo.jpg',
          'width': 75,
          'height': 75
        },
        {
          'type': 'm',
          'url': 'https://sun1-15.userapi.com/c830609/v830609207/15e4dc/7bcKr5iiVis.jpg',
          'width': 130,
          'height': 130
        },
        {
          'type': 'x',
          'url': 'https://sun1-3.userapi.com/c830609/v830609207/15e4dd/WnLOfTavZ_Q.jpg',
          'width': 604,
          'height': 604
        },
        {
          'type': 'y',
          'url': 'https://sun1-3.userapi.com/c830609/v830609207/15e4de/JC1mkuarBog.jpg',
          'width': 807,
          'height': 807
        },
        {
          'type': 'z',
          'url': 'https://sun1-16.userapi.com/c830609/v830609207/15e4df/PnfylXt-aRs.jpg',
          'width': 1080,
          'height': 1080
        },
        {
          'type': 'w',
          'url': 'https://sun1-16.userapi.com/c830609/v830609207/15e4e0/TBkOlLB4R5g.jpg',
          'width': 1254,
          'height': 1254
        },
        {
          'type': 'o',
          'url': 'https://sun1-20.userapi.com/c830609/v830609207/15e4e1/8-OfmMzEIGU.jpg',
          'width': 130,
          'height': 130
        },
        {
          'type': 'p',
          'url': 'https://sun1-4.userapi.com/c830609/v830609207/15e4e2/j7V4wbUan7U.jpg',
          'width': 200,
          'height': 200
        },
        {
          'type': 'q',
          'url': 'https://sun1-12.userapi.com/c830609/v830609207/15e4e3/u7z-FuP33jg.jpg',
          'width': 320,
          'height': 320
        },
        {
          'type': 'r',
          'url': 'https://sun1-13.userapi.com/c830609/v830609207/15e4e4/RIvHkxHK9eo.jpg',
          'width': 510,
          'height': 510
        }
      ],
      'text': '',
      'date': 1533589819,
      'post_id': 12
    }
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const int id = 4444;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.GroupChangePhoto.UserId);
			Assert.AreEqual(-groupId, update.GroupChangePhoto.Photo.OwnerId);
			Assert.AreEqual(id, update.GroupChangePhoto.Photo.Id);
		}

		[Test]
		public void GetBotsLongPollHistory_GroupJoinTest()
		{
			const string json = @"{
  'type': 'group_join',
  'object': {
    'user_id': 321,
    'join_type': 'request'
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;
			const GroupJoinType joinType = GroupJoinType.Request;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.GroupJoin.UserId);
			Assert.AreEqual(joinType, update.GroupJoin.JoinType);
		}

		[Test]
		public void GetBotsLongPollHistory_GroupLeaveTest()
		{
			const string json = @"{
  'type': 'group_leave',
  'object': {
    'user_id': 321,
    'self': 0
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.GroupLeave.UserId);
			Assert.IsFalse(update.GroupLeave.IsSelf);
		}

		[Test]
		public void GetBotsLongPollHistory_GroupLeaveSelfTest()
		{
			const string json = @"{
  'type': 'group_leave',
  'object': {
    'user_id': 321,
    'self': 1
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.GroupLeave.UserId);
			Assert.IsTrue(update.GroupLeave.IsSelf);
		}

		[Test]
		public void GetBotsLongPollHistory_GroupOfficersEditTest()
		{
			const string json = @"{
  'type': 'group_officers_edit',
  'object': {
    'admin_id': 123,
    'user_id': 321,
    'level_old': 3,
    'level_new': 2
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int adminId = 123;
			const int groupId = 1234;
			const GroupOfficerLevel oldLevel = GroupOfficerLevel.Admin;
			const GroupOfficerLevel newLevel = GroupOfficerLevel.Editor;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(adminId, update.UserId);
			Assert.AreEqual(userId, update.GroupOfficersEdit.UserId);
			Assert.AreEqual(oldLevel, update.GroupOfficersEdit.LevelOld);
			Assert.AreEqual(newLevel, update.GroupOfficersEdit.LevelNew);
		}

		[Test]
		public void GetBotsLongPollHistory_UserBlockTest()
		{
			const string json = @"{
  'type': 'user_block',
  'object': {
    'admin_id': 123,
    'user_id': 321,
    'unblock_date': 0,
    'reason': 0,
    'comment': 'test'
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;
			const int adminId = 123;
			const string comment = "test";
			const GroupUserBlockReason reason = GroupUserBlockReason.Other;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(adminId, update.UserId);
			Assert.AreEqual(userId, update.UserBlock.UserId);
			Assert.AreEqual(adminId, update.UserBlock.AdminId);
			Assert.AreEqual(comment, update.UserBlock.Comment);
			Assert.AreEqual(reason, update.UserBlock.Reason);
			Assert.IsNull(update.UserBlock.UnblockDate);
		}

		[Test]
		public void GetBotsLongPollHistory_UserBlockTemporaryTest()
		{
			const string json = @"{
  'type': 'user_block',
  'object': {
    'admin_id': 123,
    'user_id': 321,
    'unblock_date': 1533589200,
    'reason': 4,
    'comment': 'test'
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;
			const int adminId = 123;
			const string comment = "test";
			const GroupUserBlockReason reason = GroupUserBlockReason.MessagesOffTopic;
			var unblockDate = new DateTime(2018, 8, 6, 21, 0, 0);

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(adminId, update.UserId);
			Assert.AreEqual(userId, update.UserBlock.UserId);
			Assert.AreEqual(adminId, update.UserBlock.AdminId);
			Assert.AreEqual(comment, update.UserBlock.Comment);
			Assert.AreEqual(reason, update.UserBlock.Reason);
			Assert.AreEqual(unblockDate, update.UserBlock.UnblockDate);
		}

		[Test]
		public void GetBotsLongPollHistory_UserUnblockTest()
		{
			const string json = @"{
  'type': 'user_unblock',
  'object': {
    'admin_id': 123,
    'user_id': 321,
    'by_end_date': 0
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;
			const int adminId = 123;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(adminId, update.UserId);
			Assert.AreEqual(userId, update.UserUnblock.UserId);
			Assert.AreEqual(adminId, update.UserUnblock.AdminId);
			Assert.IsFalse(update.UserUnblock.ByEndDate);
		}

		[Test]
		public void GetBotsLongPollHistory_UserUnblockByEndDateTest()
		{
			const string json = @"{
  'type': 'user_unblock',
  'object': {
    'admin_id': 123,
    'user_id': 321,
    'by_end_date': 1
  },
  'group_id': 1234
}";

			const int userId = 321;
			const int groupId = 1234;
			const int adminId = 123;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserUnblock.UserId);
			Assert.AreEqual(adminId, update.UserUnblock.AdminId);
			Assert.IsTrue(update.UserUnblock.ByEndDate);
		}

		[Test]
		public void GetBotsLongPollHistory_MarketCommentNewTest()
		{
			const string json = @"{
  'type': 'market_comment_new',
  'object': {
    'id': 1,
    'from_id': 123,
    'date': 1533405772,
    'text': 'test',
    'market_owner_id': -1234,
    'item_id': 1686058
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.MarketComment.FromId);
			Assert.AreEqual(text, update.MarketComment.Text);
			Assert.AreEqual(-groupId, update.MarketComment.MarketOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_MarketCommentEditTest()
		{
			const string json = @"{
  'type': 'market_comment_edit',
  'object': {
    'id': 1,
    'from_id': 123,
    'date': 1533405772,
    'text': 'test1',
    'market_owner_id': -1234,
    'item_id': 1686058
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.MarketComment.FromId);
			Assert.AreEqual(text, update.MarketComment.Text);
			Assert.AreEqual(-groupId, update.MarketComment.MarketOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_MarketCommentRestoreTest()
		{
			const string json = @"{
  'type': 'market_comment_restore',
  'object': {
    'id': 1,
    'from_id': 123,
    'date': 1533405772,
    'text': 'test1',
    'market_owner_id': -1234,
    'item_id': 1686058
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.MarketComment.FromId);
			Assert.AreEqual(text, update.MarketComment.Text);
			Assert.AreEqual(-groupId, update.MarketComment.MarketOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_MarketCommentDeleteTest()
		{
			const string json = @"{
  'type': 'market_comment_delete',
  'object': {
    'owner_id': -1234,
    'id': 1,
    'deleter_id': 123,
    'item_id': 4444
  },
  'group_id': 1234
}";

			const int deleterId = 123;
			const int groupId = 1234;
			const int itemId = 4444;
			const int id = 1;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.MarketCommentDelete.OwnerId);
			Assert.AreEqual(deleterId, update.MarketCommentDelete.DeleterId);
			Assert.AreEqual(itemId, update.MarketCommentDelete.ItemId);
			Assert.AreEqual(id, update.MarketCommentDelete.Id);
		}

		[Test]
		public void GetBotsLongPollHistory_MessageNewTest()
		{
			const string json = @"{
	'type': 'message_new',
	'object': {
		'date': 1533397795,
		'from_id': 123,
		'id': 829,
		'out': 0,
		'peer_id': 123,
		'text': 'test',
		'conversation_message_id': 791,
		'fwd_messages': [],
		'important': false,
		'random_id': 0,
		'attachments': [],
		'is_hidden': false
	},
	'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.Message.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.Message.Text);
		}

		[Test]
		public void GetBotsLongPollHistory_MesageEditTest()
		{
			const string json = @"{
  'type': 'message_edit',
  'object': {
    'date': 1533397838,
    'from_id': 123,
    'id': 791,
    'out': 1,
    'peer_id': 123,
    'text': 'test1',
    'conversation_message_id': 791,
    'fwd_messages': [],
    'update_time': 1533397838,
    'important': false,
    'random_id': 0,
    'attachments': [],
    'is_hidden': false
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.Message.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.Message.Text);
		}

		[Test]
		public void GetBotsLongPollHistory_MessageReplyTest()
		{
			const string json = @"{
  'type': 'message_reply',
  'object': {
    'date': 1533397818,
    'from_id': 123,
    'id': 830,
    'out': 1,
    'peer_id': 123,
    'text': 'test',
    'conversation_message_id': 792,
    'fwd_messages': [],
    'important': false,
    'random_id': 0,
    'attachments': [],
    'admin_author_id': 123,
    'is_hidden': false
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.Message.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.Message.Text);
		}

		[Test]
		public void GetBotsLongPollHistory_MessageAllowTest()
		{
			const string json = @"{
  'type': 'message_allow',
  'object': {
    'user_id': 123,
    'key': '123456'
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;
			const string key = "123456";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.MessageAllow.UserId);
			Assert.AreEqual(key, update.MessageAllow.Key);
			Assert.AreEqual(groupId, update.GroupId);
		}

		[Test]
		public void GetBotsLongPollHistory_MessageDenyTest()
		{
			const string json = @"{
  'type': 'message_deny',
  'object': {
    'user_id': 123
  },
  'group_id': 1234
}";

			const int userId = 123;
			const int groupId = 1234;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.MessageDeny.UserId);
			Assert.AreEqual(groupId, update.GroupId);
		}

		[Test]
		public void GetBotsLongPollHistory_PhotoNewTest()
		{
			const string json = @"{
  'type': 'photo_new',
  'object': {
    'id': 123456,
    'album_id': 1234,
    'owner_id': -1234,
    'user_id': 100,
    'sizes': [
      {
        'type': 's',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a000/lUOsa955-jY.jpg',
        'width': 75,
        'height': 75
      },
      {
        'type': 'm',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a001/iuKZIHc3wZQ.jpg',
        'width': 130,
        'height': 130
      },
      {
        'type': 'x',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a002/d6OlNoaopNU.jpg',
        'width': 604,
        'height': 604
      },
      {
        'type': 'y',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a003/_iE5hSNLl9U.jpg',
        'width': 807,
        'height': 807
      },
      {
        'type': 'z',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a004/33LbnCgpeIc.jpg',
        'width': 1024,
        'height': 1024
      },
      {
        'type': 'o',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a005/5IQIvtzM6VA.jpg',
        'width': 130,
        'height': 130
      },
      {
        'type': 'p',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a006/V6YM6vKahbw.jpg',
        'width': 200,
        'height': 200
      },
      {
        'type': 'q',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a007/ijJML7x7aKo.jpg',
        'width': 320,
        'height': 320
      },
      {
        'type': 'r',
        'url': 'https://pp.userapi.com/c840134/v840134869/5a008/rqmyhuQ57ic.jpg',
        'width': 510,
        'height': 510
      }
    ],
    'text': '',
    'date': 1533399738
  },
  'group_id': 1234
}";
			const int groupId = 1234;
			const int photoId = 123456;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(-groupId, update.Photo.OwnerId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(photoId, update.Photo.Id);
		}
		[Test]
		public void GetBotsLongPollHistory_PhotoCommentNewTest()
		{
			const string json = @"{
  'type': 'photo_comment_new',
  'object': {
    'id': 4,
    'from_id': 123,
    'date': 1533399764,
    'text': 'test',
    'photo_owner_id': -1234,
    'photo_id': 123456
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";
			const int photoId = 123456;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.PhotoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.PhotoComment.Text);
			Assert.AreEqual(-groupId, update.PhotoComment.PhotoOwnerId);
			Assert.AreEqual(photoId, update.PhotoComment.PhotoId);
		}
		[Test]
		public void GetBotsLongPollHistory_PhotoCommentEditTest()
		{
			const string json = @"{
  'type': 'photo_comment_edit',
  'object': {
    'id': 4,
    'from_id': 123,
    'date': 1533399764,
    'text': 'test1',
    'photo_owner_id': -1234,
    'photo_id': 456239020
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.PhotoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.PhotoComment.Text);
			Assert.AreEqual(-groupId, update.PhotoComment.PhotoOwnerId);
		}
		[Test]
		public void GetBotsLongPollHistory_PhotoCommentRestoreTest()
		{
			var json =
				@"{
  'type': 'photo_comment_restore',
  'object': {
    'id': 4,
    'from_id': 123,
    'date': 1533399764,
    'text': 'test1',
    'photo_owner_id': -1234,
    'photo_id': 456239020
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.PhotoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.PhotoComment.Text);
			Assert.AreEqual(-groupId, update.PhotoComment.PhotoOwnerId);
		}
		[Test]
		public void GetBotsLongPollHistory_PhotoCommentDeleteTest()
		{
			const string json = @"{
  'type': 'photo_comment_delete',
  'object': {
    'owner_id': -1234,
    'id': 4,
    'deleter_id': 12345,
    'photo_id': 123456,
    'user_id': 123
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int deleterId = 12345;
			const int groupId = 1234;
			const int photoId = 123456;
			const int id = 4;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(deleterId, update.UserId);
			Assert.AreEqual(deleterId, update.PhotoCommentDelete.DeleterId);
			Assert.AreEqual(userId, update.PhotoCommentDelete.UserId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.PhotoCommentDelete.OwnerId);
			Assert.AreEqual(photoId, update.PhotoCommentDelete.PhotoId);
			Assert.AreEqual(id, update.PhotoCommentDelete.Id);
		}

		[Test]
		public void GetBotsLongPollHistory_PollVoteNewTest()
		{
			const string json = @"{
  'type': 'poll_vote_new',
  'object': {
    'owner_id': -1234,
    'poll_id': 4444,
    'option_id': 3333,
    'user_id': 123
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const int optionId = 3333;
			const int pollId = 4444;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.PollVoteNew.UserId);
			Assert.AreEqual(optionId, update.PollVoteNew.OptionId);
			Assert.AreEqual(pollId, update.PollVoteNew.PollId);
		}

		[Test]
		public void GetBotsLongPollHistory_VideoNewTest()
		{
			const string json = @"{
  'type': 'video_new',
  'object': {
    'id': 4444,
    'owner_id': -1234,
    'title': 'Test',
    'duration': 14,
    'description': '',
    'date': 1533402376,
    'comments': 0,
    'views': 1,
    'width': 680,
    'height': 720,
    'photo_130': 'https://pp.userapi.com/c849028/v849028892/43296/cQDGL421aic.jpg',
    'photo_320': 'https://pp.userapi.com/c849028/v849028892/43294/uhE7yWEUJ6Y.jpg',
    'photo_800': 'https://pp.userapi.com/c849028/v849028892/43293/dEXbARrZQuE.jpg',
    'repeat': 1,
    'first_frame_800': 'https://pp.userapi.com/c846320/v846320892/b3572/wVgFPd4YBsc.jpg',
    'first_frame_320': 'https://pp.userapi.com/c846320/v846320892/b3573/803qAiFud4o.jpg',
    'first_frame_160': 'https://pp.userapi.com/c846320/v846320892/b3574/cnHZIE4htwc.jpg',
    'first_frame_130': 'https://pp.userapi.com/c846320/v846320892/b3575/Vuo9kROpA5o.jpg',
    'can_edit': 1,
    'can_add': 1
  },
  'group_id': 1234
}";
			const int groupId = 1234;
			const int id = 4444;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.Video.OwnerId);
			Assert.AreEqual(id, update.Video.Id);
		}
		[Test]
		public void GetBotsLongPollHistory_VideoCommentNewTest()
		{
			const string json = @"{
  'type': 'video_comment_new',
  'object': {
    'id': 1,
    'from_id': 123,
    'date': 1533402417,
    'text': 'test',
    'video_owner_id': -1234,
    'video_id': 4444
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const int videoId = 4444;
			const string text = "test";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.VideoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.VideoComment.Text);
			Assert.AreEqual(-groupId, update.VideoComment.VideoOwnerId);
			Assert.AreEqual(videoId, update.VideoComment.VideoId);
		}
		[Test]
		public void GetBotsLongPollHistory_VideoCommentEditTest()
		{
			const string json = @"{
  'type': 'video_comment_edit',
  'object': {
    'id': 1,
    'from_id': 123,
    'date': 1533402417,
    'text': 'test1',
    'video_owner_id': -1234,
    'video_id': 4444
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.VideoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.VideoComment.Text);
			Assert.AreEqual(-groupId, update.VideoComment.VideoOwnerId);
		}
		[Test]
		public void GetBotsLongPollHistory_VideoCommentRestoreTest()
		{
			const string json = "{\r\n  \"type\": \"video_comment_restore\",\r\n  \"object\": {\r\n    \"id\": 1,\r\n    \"from_id\": 123,\r\n    \"date\": 1533402417,\r\n    \"text\": \"test1\",\r\n    \"video_owner_id\": -1234,\r\n    \"video_id\": 4444\r\n  },\r\n  \"group_id\": 1234\r\n}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.VideoComment.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.VideoComment.Text);
			Assert.AreEqual(-groupId, update.VideoComment.VideoOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_VideoCommentDeleteTest()
		{
			var json =
				@"{
  'type': 'video_comment_delete',
  'object': {
    'owner_id': -1234,
    'id': 4,
    'deleter_id': 12345,
    'video_id': 123456
  },
  'group_id': 1234
}";
			const int groupId = 1234;
			const int deleterId = 12345;
			const int videoId = 123456;
			const int id = 4;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(deleterId, update.UserId);
			Assert.AreEqual(deleterId, update.VideoCommentDelete.DeleterId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.VideoCommentDelete.OwnerId);
			Assert.AreEqual(videoId, update.VideoCommentDelete.VideoId);
			Assert.AreEqual(id, update.VideoCommentDelete.Id);
		}

		[Test]
		public void GetBotsLongPollHistory_WallPostNewTest()
		{
			const string json = @"{
  'type': 'wall_post_new',
  'object': {
    'id': 6,
    'from_id': 123,
    'owner_id': -1234,
    'date': 1533403316,
    'marked_as_ads': 0,
    'post_type': 'post',
    'text': 'test',
    'can_edit': 1,
    'created_by': 123,
    'can_delete': 1,
    'comments': {
      'count': 0
    }
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.WallPost.FromId);
			Assert.AreEqual(-groupId, update.WallPost.OwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_WallReplyNewTest()
		{
			const string json = @"{
  'type': 'wall_reply_new',
  'object': {
    'id': 9,
    'from_id': 123,
    'date': 1533403427,
    'text': 'test',
    'post_owner_id': -1234,
    'post_id': 6
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test";
			const int postId = 6;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.WallReply.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.WallReply.Text);
			Assert.AreEqual(-groupId, update.WallReply.PostOwnerId);
			Assert.AreEqual(postId, update.WallReply.PostId);
		}

		[Test]
		public void GetBotsLongPollHistory_WallReplyEditTest()
		{
			const string json = @"{
  'type': 'wall_reply_edit',
  'object': {
    'id': 9,
    'from_id': 123,
    'date': 1533403427,
    'text': 'test1',
    'post_owner_id': -1234,
    'post_id': 6
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.WallReply.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.WallReply.Text);
			Assert.AreEqual(-groupId, update.WallReply.PostOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_WallReplyRestoreTest()
		{
			const string json = @"{
  'type': 'wall_reply_restore',
  'object': {
    'id': 9,
    'from_id': 123,
    'date': 1533403427,
    'text': 'test1',
    'post_owner_id': -1234,
    'post_id': 6
  },
  'group_id': 1234
}";
			const int userId = 123;
			const int groupId = 1234;
			const string text = "test1";

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(userId, update.UserId);
			Assert.AreEqual(userId, update.WallReply.FromId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(text, update.WallReply.Text);
			Assert.AreEqual(-groupId, update.WallReply.PostOwnerId);
		}

		[Test]
		public void GetBotsLongPollHistory_WallReplyDeleteTest()
		{
			const string json = @"{
  'type': 'wall_reply_delete',
  'object': {
    'owner_id': -1234,
    'id': 9,
    'deleter_id': 12345,
    'post_id': 6
  },
  'group_id': 1234
}";
			const int groupId = 1234;
			const int deleterId = 12345;
			const int postId = 6;
			const int id = 9;

			var groupCategory = GetMockedGroupCategory("https://vk.com", GetFullResponse(json));

			var botsLongPollHistory = groupCategory.GetBotsLongPollHistory(new BotsLongPollHistoryParams
			{
				Key = "test",
				Server = "https://vk.com",
				Ts = 0,
				Wait = 10
			});

			var update = botsLongPollHistory.Updates.First();

			Assert.AreEqual(deleterId, update.UserId);
			Assert.AreEqual(deleterId, update.WallReplyDelete.DeleterId);
			Assert.AreEqual(groupId, update.GroupId);
			Assert.AreEqual(-groupId, update.WallReplyDelete.OwnerId);
			Assert.AreEqual(postId, update.WallReplyDelete.PostId);
			Assert.AreEqual(id, update.WallReplyDelete.Id);
		}
	}
}