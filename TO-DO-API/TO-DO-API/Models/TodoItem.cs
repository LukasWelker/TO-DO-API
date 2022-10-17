namespace TO_DO_API.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        //durch ein Fragezeichen wird der Standardwert eines Rückgabetyps auf null anstatt 0 gesetzt
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? Secret { get; set; }
    }
   
   
}
