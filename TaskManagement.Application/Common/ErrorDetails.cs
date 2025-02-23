﻿namespace TaskManagement.Domain.Common
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
    }
}
