using System;
using System.Threading.Tasks;
namespace StigSchmidtNielsson.Utils.Extensions {
    public static class TaskExtensions {
        public static void HandleExceptions(this Task task, Action<Exception> Handler) {
            task.ContinueWith(t => {
                if (t != null && t.Exception != null) {
                    var aggException = t.Exception.Flatten();
                    foreach (var exception in aggException.InnerExceptions) {
                        Handler(exception);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    //TODO: test
}