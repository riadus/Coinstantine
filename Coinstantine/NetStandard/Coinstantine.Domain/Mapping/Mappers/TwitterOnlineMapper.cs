using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class TwitterOnlineMapper : IMapper<TwitterProfileOnline, TwitterProfile>
    {
        public TwitterProfile Map(TwitterProfileOnline source)
        {
            return new TwitterProfile
            {
                ScreenName = source.ScreenName,
                CreationDate = source.CreationDate,
                Followers = source.NumberOfFollower,
                TwitterId = source.TwitterId,
                Validated = source.Validated,
                ValidationDate = source.ValidationDate
            };
        }

        public TwitterProfileOnline MapBack(TwitterProfile source)
        {
            throw new NotImplementedException();
        }
    }
}
