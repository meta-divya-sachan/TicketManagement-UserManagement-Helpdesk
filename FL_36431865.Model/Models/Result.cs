using System;

namespace TicketManagement.Model
{
    public class Result<T>
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public string Id { get; set; }

        public int TotalCount { get; set; }
    }
}
