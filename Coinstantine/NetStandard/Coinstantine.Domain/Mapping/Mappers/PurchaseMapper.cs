using System;
using System.Linq;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;
using Nethereum.Util;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class PurchaseMapper : IMapper<PurchaseDTO, Purchase>
    {
        private readonly IMapperFactory _mapperFactory;

        public PurchaseMapper(IMapperFactory mapperFactory)
        {
            _mapperFactory = mapperFactory;
        }

        public Purchase Map(PurchaseDTO source)
        {
            var mapper = _mapperFactory.GetMapper<BuyingReceiptDTO, BuyingReceipt>();
            var purchase = new Purchase
            {
                ActualPurchase = mapper.Map(source.ActualPurchase),
            };

            return purchase;
        }

        public PurchaseDTO MapBack(Purchase source)
        {
            throw new NotImplementedException();
        }
    }
}
