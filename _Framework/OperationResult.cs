namespace _Framework
{
    public class OperationResult
    {
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }


        public OperationResult Success(string message = OperationMessage.Success)
        {
            IsSucceeded = true;
            Message = message;
            return this;
        }

        public OperationResult Failed(string message)
        {
            IsSucceeded = false;
            Message = message;
            return this;
        }
    }
}