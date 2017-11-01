using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientProxyCommon
{
    public static class ThreadShell
    {
        public static Task Run(Func<Task> action)
        {
            return Task.Run(() => {
                Task.Factory.StartNew(action);
                return Task.CompletedTask;
            });
        }

        public static Task Run(Action action)
        {
            return Task.Run(() => Task.Factory.StartNew(action));
        }

        public static Task LongRun(Func<Task> action)
        {
            return Task.Run(() => {
                Task.Factory.StartNew(action,
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);

                return Task.CompletedTask;
            });
        }

        public static Task LongRun(Func<Task> action, CancellationTokenSource cancellationTokenSource)
        {
            return Task.Run(() => {
                Task.Factory.StartNew(action,
                    cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);

                return Task.CompletedTask;
            });
        }

        public static Task LongRun(Action action)
        {
            return Task.Run(() => {
                Task.Factory.StartNew(action,
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            });
        }
    }
}
