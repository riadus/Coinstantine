using System;
using Coinstantine.Common;
using Coinstantine.Data;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class UserProfileMapper : IMapper<ApiUser, UserProfile>
    {
        private readonly IMapper<BitcoinTalkUserOnline, BitcoinTalkProfile> _bctMapper;
        private readonly IMapper<TelegramProfileOnline, TelegramProfile> _telegramMapper;
        private readonly IMapper<TwitterProfileOnline, TwitterProfile> _twitterMapper;

        public UserProfileMapper(IMapper<BitcoinTalkUserOnline, BitcoinTalkProfile> bctMapper,
                                 IMapper<TelegramProfileOnline, TelegramProfile> telegramMapper,
                                 IMapper<TwitterProfileOnline, TwitterProfile> twitterMapper)
        {
            _bctMapper = bctMapper;
            _telegramMapper = telegramMapper;
            _twitterMapper = twitterMapper;
        }

        public UserProfile Map(ApiUser source)
        {
            var userProfile = new UserProfile
            {
                Username = source.Username,
                Email = source.Email
            };

            if(source.BctProfile != null)
            {
                userProfile.BitcoinTalkProfile = _bctMapper.Map(source.BctProfile);
            }

            if(source.TwitterProfile != null)
            {
                userProfile.TwitterProfile = _twitterMapper.Map(source.TwitterProfile);
            }

            if(source.Telegram != null)
			{
                userProfile.TelegramProfile = _telegramMapper.Map(source.Telegram);
			}

            if(!source.BlockchainInfo?.Address.IsNullOrEmpty() ?? true)
            {
                userProfile.BlockchainInfo = source.BlockchainInfo;
            }   

            return userProfile;
        }

        public ApiUser MapBack(UserProfile source)
        {
            throw new NotImplementedException();
        }
    }
}
