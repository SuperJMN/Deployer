﻿using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Zafiro.Core.Pending
{
    public static class ResultExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this Result<T> result)
        {
            return result.Match(arg => new[] { result.Value }, _ => Enumerable.Empty<T>());
        }

        public static T Handle<T>(this Result<T> result, Func<string, T> convertToSuccess)
        {
            var mapError = result.MapError(convertToSuccess);
            return mapError.Match(s => s, s => s);
        }

        public static T Handle<T, E>(this Result<T, E> result, Func<E, T> convertToSuccess)
        {
            var mapError = result.MapError(convertToSuccess);
            return mapError.Match(s => s, s => s);
        }
    }
}
