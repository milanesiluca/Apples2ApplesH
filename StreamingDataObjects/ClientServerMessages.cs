namespace StreamingDataObjects
{
    public class ClientServerMessages<T>
    {
        
        public int Category { get; private set; } 
        public T Message { get; private set; }

        public ClientServerMessages(int category, T message) { 
            Category = category;
            Message = message;
        }
        
    }
}
