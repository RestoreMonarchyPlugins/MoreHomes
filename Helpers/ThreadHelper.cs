using Rocket.Core.Logging;
using Rocket.Core.Utils;
using System;
using System.Threading;

namespace RestoreMonarchy.MoreHomes.Helpers
{
    public class ThreadHelper
    {
		public static void RunAsynchronously(Action action, string exceptionMessage = null)
		{
			ThreadPool.QueueUserWorkItem((_) => 
			{
				try
				{
					action.Invoke();
				}
				catch (Exception e)
				{
                    RunSynchronously(() => 
					{
						Logger.LogException(e, exceptionMessage);
					}, 0f);
				}
			});
		}

		public static void RunSynchronously(Action action, float delaySeconds = 0f)
		{
			TaskDispatcher.QueueOnMainThread(action, delaySeconds);
		}
	}
}
