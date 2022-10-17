namespace TO_DO_API.Models
{
    public class TodoItemDTO
    {
        /// <summary>
        /// class exist to avoid overposting 
        /// Omitting some properties to reduce payload
        /// </summary>
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
