using System;
using System.Windows;
using System.Windows.Threading;

namespace RunOnMainThread
{
    public static class MainThreadDispatcher
    {
        public static void RunOnMainThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
