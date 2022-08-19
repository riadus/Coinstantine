using System.Collections.Generic;
using Coinstantine.Data;
using Coinstantine.UnitTests.Builders;
using NUnit.Framework;

namespace Coinstantine.UnitTests
{
    public class TelegramAirdropRequirementTests
    {
        [Test]
        public void No_Telegram_Account_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { telegramReq });
            Assert.False(result);
        }

        [Test]
        public void No_Telegram_Account_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = false
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { telegramReq });
            Assert.True(result);
        }

        [Test]
        public void Having_Telegram_Account_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var telegramReq = new TelegramAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TelegramProfile = new TelegramProfile { Validated = true, Username = "@Account" } }, new List<IAirdropRequirement> { telegramReq });
            Assert.True(result);
        }
    }
}
