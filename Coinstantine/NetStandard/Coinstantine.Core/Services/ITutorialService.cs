using System;

namespace Coinstantine.Core.Services
{
    public interface ITutorialService
    {
		void StartTelegramTutorial(Action startAction);
    }
}
