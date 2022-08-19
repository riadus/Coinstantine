using Coinstantine.Data;
using Coinstantine.UnitTests.Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Coinstantine.UnitTests
{
    public class TwitterAirdropRequirementTests
    {
        [Test]
        public void No_Twitter_Account_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Test]
        public void No_Twitter_Account_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = false,
                HasAccountApplies = false
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile(), new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Test]
        public void Having_Twitter_Account_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account" } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_Enough_Followers_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumFollowers = 100,
                MinimumFollowersApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", Followers = 10 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_Enough_Followers_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", Followers = 10 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Test]
        public void Having_Enough_Followers_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumFollowers = 100,
                MinimumFollowersApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", Followers = 200 } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Test]
        public void Not_Having_An_Account_Created_Early_Enough_When_Needed_Should_Fail()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumCreationDate = new DateTime(2018, 1, 1),
                MinimumCreationDateApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.False(result);
        }

        [Test]
        public void Not_Having_An_Account_Created_Early_Enough_When_Not_Needed_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2018, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }

        [Test]
        public void Having_An_Account_Created_Early_Enough_Should_Succeed()
        {
            var requirementConstructorBuilder = new RequirementToLambdaBuilder().Build();
            var requirementConstructor = new RequirementsConstructorBuilder()
                .WithRequirementToLambda(requirementConstructorBuilder)
                .Build();
            var twitterReq = new TwitterAirdropRequirement
            {
                HasAccount = true,
                HasAccountApplies = true,
                MinimumCreationDate = new DateTime(2018, 1, 1),
                MinimumCreationDateApplies = true
            };
            var result = requirementConstructor.MeetsAllRequirement(new UserProfile { TwitterProfile = new TwitterProfile { Validated = true, Username = "@Account", CreationDate = new DateTime(2017, 7, 1) } }, new List<IAirdropRequirement> { twitterReq });
            Assert.True(result);
        }
    }
}
