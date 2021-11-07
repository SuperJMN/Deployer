using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Zafiro.Core.Pending
{
    public static class CombineExtensions
    {
        public static Task<Result<TResult>> With<T1, T2, TResult>(this Result<T1> a,
            Result<T2> b,
            Func<T1, T2, Task<TResult>> map)
        {
            var mapSuccess =
                a.BindError(el1 => b
                        .MapError(el2 => string.Join(Result.ErrorMessagesSeparator, el1, el2))
                        .Bind(_ => Result.Failure<T1>(el1)))
                    .Bind(x => b
                        .Map(y => map(x, y))
                        .MapError(el => el));

            return mapSuccess;
        }

        public static Task<Result> WithBind<T1, T2>(this Result<T1> a,
            Result<T2> b,
            Func<T1, T2, Task<Result>> map)
        {
            var mapSuccess =
                a.BindError(el1 => b
                        .MapError(el2 => string.Join(Result.ErrorMessagesSeparator, el1, el2))
                        .Bind(_ => Result.Failure<T1>(el1)))
                    .Bind(x => b
                        .Bind(y => map(x, y))
                        .MapError(el => el));

            return mapSuccess;
        }

        public static Task<Result<TResult>> WithBind<T1, T2, TResult>(this Result<T1> a,
            Result<T2> b,
            Func<T1, T2, Task<Result<TResult>>> map)
        {
            var mapSuccess =
                a.BindError(el1 => b
                        .MapError(el2 => string.Join(Result.ErrorMessagesSeparator, el1, el2))
                        .Bind(_ => Result.Failure<T1>(el1)))
                    .Bind(x => b
                        .Bind(y => map(x, y))
                        .MapError(el => el));

            return mapSuccess;
        }

        public static Task<Result<TResult, E>> WithBind<T1, T2, E, TResult>(this Result<T1, E> a,
            Result<T2, E> b,
            Func<T1, T2, Task<Result<TResult, E>>> map, Func<E, E, E> combineError)
        {
            var mapSuccess =
                a.BindError(el1 => b
                        .MapError(el2 => combineError(el1, el2))
                        .Bind(_ => Result.Failure<T1, E>(el1)))
                    .Bind(x => b
                        .Bind(y => map(x, y))
                        .MapError(el => el));

            return mapSuccess;
        }

        public static Task<Result<TResult, E>> With<T1, T2, E, TResult>(this Result<T1, E> a,
            Result<T2, E> b,
            Func<T1, T2, Task<TResult>> map, Func<E, E, E> combineError)
        {
            var mapSuccess =
                a.BindError(el1 => b
                        .MapError(el2 => combineError(el1, el2))
                        .Bind(_ => Result.Failure<T1, E>(el1)))
                    .Bind(x => b
                        .Map(y => map(x, y))
                        .MapError(el => el));

            return mapSuccess;
        }

        public static Result<T, E2> BindError<T, E, E2>(this Result<T, E> self,
            Func<E, Result<T, E2>> map)
        {
            if (self.IsSuccess)
            {
                return Result.Success<T, E2>(self.Value);
            }

            return map(self.Error);
        }

        public static Result<T> BindError<T>(this Result<T> self,
            Func<string, Result<T>> map)
        {
            if (self.IsSuccess)
            {
                return Result.Success(self.Value);
            }

            return map(self.Error);
        }
    }
}