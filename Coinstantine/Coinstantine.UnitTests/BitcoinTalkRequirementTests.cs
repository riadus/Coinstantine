using System;
using System.Collections.Generic;
using Coinstantine.Data;
using Coinstantine.UnitTests.Builders;
using NUnit.Framework;

namespace Coinstantine.UnitTests
{
    public class BitcoinTalkRequirementTests
    {
        [Test]
        public void No_BitcoinTalk_Account_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void No_BitcoinTalk_Account_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = false
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_BitcoinTalk_Account_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account" } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_Enough_Posts_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumPosts = 100,
                MinimumPostsApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_Enough_Posts_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_Enough_Posts_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumPosts = 100,
                MinimumPostsApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Posts = 200 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_Enough_Activity_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumActivity = 100,
                MinimumActivityApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_Enough_Activity_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 10 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_Enough_Activity_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumActivity = 100,
                MinimumActivityApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Activity = 200 } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_A_Rank_High_Enough_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumPosition = BitcoinTalkRank.JrMember,
                MinimumRankApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.Newbie } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_A_Rank_High_Enough_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.Newbie } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_A_Rank_High_Enough_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumPosition = BitcoinTalkRank.JrMember,
                MinimumRankApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.SrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_The_Exact_Required_Rank_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                ExactPosition = BitcoinTalkRank.Newbie,
                ExactRankApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.JrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_The_Exact_Required_Rank_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.SrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_The_Exact_Required_Rank_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumPosition = BitcoinTalkRank.JrMember,
                MinimumRankApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", Position = BitcoinTalkRank.JrMember } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_An_Account_Created_Early_Enough_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumCreationDate = new DateTime(2018, 1, 1),
                MinimumCreationDateApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistredDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_An_Account_Created_Early_Enough_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistredDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }

        [Test]
        public void Having_An_Account_Created_Early_Enough_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var bitcoinTalkReq = new BitcoinTalkAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumCreationDate = new DateTime(2018, 1, 1),
                MinimumCreationDateApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { BitcoinTalkProfile = new BitcoinTalkProfile { Validated = true, Username = "@Account", RegistredDate = new DateTime(2017, 7, 1) } }, new List<IAirdropRequirement> { bitcoinTalkReq });
            Assert.True(result);
        }
    }
}
