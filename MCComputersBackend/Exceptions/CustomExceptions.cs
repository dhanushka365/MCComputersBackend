namespace MCComputersBackend.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message)
        {
        }

        public BusinessLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int id) 
            : base($"{entityName} with ID {id} was not found.")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }

    public class InsufficientStockException : BusinessLogicException
    {
        public InsufficientStockException(string productName, int requestedQuantity, int availableStock)
            : base($"Insufficient stock for product '{productName}'. Requested: {requestedQuantity}, Available: {availableStock}")
        {
        }
    }
}
