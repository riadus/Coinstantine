using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
	[RegisterInterfaceAsDynamic]
	public class TranslationMapper : IMapper<TranslationDTO, Translation>
    {
		public Translation Map(TranslationDTO source)
		{
			return new Translation
			{
				Id = 0,
				Key = source.Key,
				Language = source.Language,
				Value = source.Text
			};
		}

		public TranslationDTO MapBack(Translation source)
		{
			throw new NotImplementedException();
		}
	}
}
