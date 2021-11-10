using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Zafiro.Core.Pending
{
    public static class MaybeExtensions
    {
        public static Maybe<T> Do<T>(this Maybe<T> self, Action<T> action)
        {
            self.Execute(action);
            return self;
        }
    }
}