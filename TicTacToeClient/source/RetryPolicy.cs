using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Client.source
{
    public static class RetryPolicy
    {
        /// <summary>
        /// Retry on transient error using exponential backoff algorithm.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionLogic"></param>
        public async static void ExponentialRetry(Action action, Func<Exception, bool> exceptionLogic)
        {
            const int MAX_RETRIES = 3;
            const int MAX_WAIT_INTERVAL = 60000;
            int retries = 0;
            bool retry = false;

            do
            {
                try
                {
                    int waitTime = Math.Min(GetWaitTimeExp(retries), MAX_WAIT_INTERVAL);

                    await Task.Delay(waitTime);

                    action();
                }
                catch (Exception ex)
                {
                    retry = exceptionLogic(ex);
                }
            } while (retry && (retries++ < MAX_RETRIES));
        }

        /// <summary>
        /// Returns the next wait interval, in milliseconds, using an exponential backoff algorithm.
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private static int GetWaitTimeExp(int retryCount)
        {
            return (int)(Math.Pow(2, retryCount) * 100);
        }
    }
}
