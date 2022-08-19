using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces.Airdrops
{
    public interface IAirdropDefinitionsService
    {
        Task<bool> SyncAirdropDefinitions();
        Task<IEnumerable<AirdropDefinition>> GetAirdropDefinitions();
        Task<AirdropStatus> GetStatus(AirdropDefinition airdropDefinition);
        Task<UserAirdrop> GetUserAirdrop();
        Task<(bool Success, FailReason FailReason)> SubscribeToAirdrop(AirdropDefinition airdropDefinition);
    }

    public enum AirdropStatus
    {
        Ok,
        Full,
        NotStarted,
        Expired,
        AlreadySubscribed,
        RequirementNotMet,
        SoonToBeFull
    }
}
