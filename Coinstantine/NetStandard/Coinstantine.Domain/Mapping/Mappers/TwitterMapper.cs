using System;
using Coinstantine.Data;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class TwitterMapper : IMapper<TwitterProfile, TwitterProfileDTO>
    {
        public TwitterProfileDTO Map(TwitterProfile source)
        {
            return new TwitterProfileDTO
            {
                ScreenName = source.ScreenName,
                TweetId = source.TweetId,
                TwitterId = source.TwitterId,
                Username = source.Username,
                CreationDate = source.CreationDate,
                NumberOfFollower = source.Followers
            };
        }

        public TwitterProfile MapBack(TwitterProfileDTO source)
        {
            return new TwitterProfile
            {
                ScreenName = source.ScreenName,
                TweetId = source.TweetId,
                TwitterId = source.TwitterId,
                Username = source.Username,
                CreationDate = source.CreationDate,
                Followers = source.NumberOfFollower
            };
        }
    }
}
