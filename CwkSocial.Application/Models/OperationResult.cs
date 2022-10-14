﻿namespace CwkSocial.APPLICATION.Models
{
    public class OperationResult<T>
    {
        public T PayLoad { get; set; }
        public bool IsError { get; set; }
        public List<Error> Errors { get; } = new List<Error>();
    }
}