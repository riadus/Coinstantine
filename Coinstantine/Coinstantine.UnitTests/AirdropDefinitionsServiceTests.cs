using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.UnitTests.Builders;
using NUnit.Framework;

namespace Coinstantine.UnitTests
{
    public class AirdropDefinitionsServiceTests
    {
        [Test]
        public async Task Full_Airdrop_Should_Return_Full_Status()
        {
            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .Build();

            var airdropDefinition = new AirdropDefinition
            {
                MaxLimit = 10,
                NumberOfSubscribers = 10
            };

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.Full, status);
        }

        [Test]
        public async Task Expired_Airdrop_Should_Return_Expired_Status()
        {
            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .Build();

            var airdropDefinition = new AirdropDefinition
            {
                ExpirationDate = DateTime.Now.AddDays(-1)
            };

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.Expired, status);
        }

        [Test]
        public async Task Not_Started_Airdrop_Should_Return_Not_Started_Status()
        {
            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .Build();

            var airdropDefinition = new AirdropDefinition
            {
                StartDate = DateTime.Now.AddDays(1)
            };

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.NotStarted, status);
        }

        [Test]
        public async Task Almost_Full_Airdrop_Should_Return_SoonToBeFull_Status()
        {
            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .Build();

            var airdropDefinition = new AirdropDefinition
            {
                MaxLimit = 10,
                NumberOfSubscribers = 9
            };

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.SoonToBeFull, status);
        }

        [Test]
        public async Task Ok_Airdrop_Should_Return_Ok_Status()
        {
            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .Build();

            var airdropDefinition = new AirdropDefinition
            {
                MaxLimit = 10,
                NumberOfSubscribers = 8,
                StartDate = DateTime.Now.AddDays(-1),
                ExpirationDate = DateTime.Now.AddDays(1),
            };

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.Ok, status);
        }

        [Test]
        public async Task Already_Subscribed_User_Airdrop_Should_Return_AlreadySubscribed_Status()
        {
            var userProfile = new UserProfile
            {
                Id = 1
            };
            var airdropDefinition = new AirdropDefinition
            {
                AirdropId = 1
            };
            var aidropUser = new UserAirdrop
            {
                UserId = 1,
                AidropDefinitionIds = new List<int> { airdropDefinition.AirdropId }
            };
            var unitOfWork = new MockUnitOfWork();
            await unitOfWork.UserAirdrops.SaveAsync(aidropUser).ConfigureAwait(false);

            var airdropDefinitionService = new AirdropDefinitionsServiceBuilder()
                                                .WithUnitOfWork(unitOfWork)
                                                .WithUserProfile(userProfile)
                                                .Build();

            var status = await airdropDefinitionService.GetStatus(airdropDefinition);
            Assert.AreEqual(AirdropStatus.AlreadySubscribed, status);
        }
    }
}
