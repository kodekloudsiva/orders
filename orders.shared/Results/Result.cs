using orders.shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }

        public Error? Error { get; }
        public List<Error>? Errors { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(Error error)
        {
            IsSuccess = false;
            Error = error;
            Errors = new List<Error> { error };
        }

        private Result(List<Error> errors)
        {
            IsSuccess = false;
            Errors = errors ?? new List<Error>();
            Error = Errors.FirstOrDefault();
        }

        // Returns a successful result with a value
        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        // Returns a failed result with a single error
        public static Result<T> Failure(Error error)
        {
            return new Result<T>(error);
        }

        // Returns a failed result with a list of errors
        public static Result<T> Failure(List<Error> errors)
        {
            return new Result<T>(errors);
        }
    }

    public record Error(string Code, string Message, string? Field = null);
}
