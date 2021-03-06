using NUnit.Framework;
using VkNet.Model.RequestParams;

namespace VkNet.Tests.Categories.Audio
{
	public class AudioSearchTest : BaseTest
	{
		[Test]
		public void SearchTest()
		{
			Url = "https://api.vk.com/method/audio.search";

			Json = @"{
					  'response': {
					    'count': 99066,
					    'items': [
					      {
					        'id': 456418873,
					        'owner_id': 371745467,
					        'artist': 'John Powell',
					        'title': 'Test Drive',
					        'duration': 156,
					        'date': 1503612601,
					        'url': 'https://cs1-46v4.vkuseraudio.net/p2/a9065affc6e6a4.mp3?extra=BBoeyU-fEBRAxHXnrM0MMnK7JcEQOIWWrOaEKN9-LUtOq9CsfkYOUZSD-wYzjJS9nKW_FUs5HiKgbZ7fYi5prpoUSFr4Lf8HGL1t7ex__cogR55wK0d0FQ5z4e8usrJjA2LwOE7AH0QeRqP4',
					        'album': {
					          'id': 290166,
					          'owner_id': -2000290166,
					          'title': 'How To Train Your Dragon (How To Train Your Dragon)',
					          'access_key': '556ad161df148ca1e3',
					          'thumb': {
					            'photo_34': 'https://sun1-12.userapi.com/c636816/v636816452/7f6df/fDiWOOXDe5U.jpg',
					            'photo_68': 'https://sun1-16.userapi.com/c636816/v636816452/7f6de/qe6RCqItpd0.jpg',
					            'photo_135': 'https://sun1-17.userapi.com/c636816/v636816452/7f6dd/Zk-fVXZONwI.jpg',
					            'photo_270': 'https://sun1-4.userapi.com/c636816/v636816452/7f6dc/9JeS618go-E.jpg',
					            'photo_300': 'https://sun1-19.userapi.com/c636816/v636816452/7f6db/1DfxBKXD2gM.jpg',
					            'photo_600': 'https://sun1-4.userapi.com/c636816/v636816452/7f6da/aQ5ZY1cMQx0.jpg',
					            'width': 600,
					            'height': 600
					          }
					        },
					        'is_licensed': true,
					        'is_hq': true,
					        'track_genre_id': 7,
					        'access_key': '8551ed14c8ff6b1392'
					      }
					    ]
					  }
					}";

			var result = Api.Audio.Search(new AudioSearchParams
			{
				Query = "test",
				Count = 1
			});

			Assert.IsNotEmpty(result);
		}
	}
}