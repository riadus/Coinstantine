using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class BuyingReceiptMapper : IMapper<BuyingReceiptDTO, BuyingReceipt>
    {
        public BuyingReceipt Map(BuyingReceiptDTO source)
        {
            return new BuyingReceipt
            {
                AmountBought = source.AmountBought,
                Value = source.Value,
                TransactionHash = source.TransactionReceipt?.TransactionHash,
                CumulativeGasUsed = source.TransactionReceipt?.CumulativeGasUsed,
                GasUsed = source.TransactionReceipt?.GasUsed,
                PurchaseDate = source.PurchaseDate
            };
        }

        public BuyingReceiptDTO MapBack(BuyingReceipt source)
        {
            throw new NotImplementedException();
        }
    }
}
