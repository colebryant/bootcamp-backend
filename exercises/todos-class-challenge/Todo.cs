using System;

namespace Todos
{
    public class Todo
    {
        public string Text {get; set;}
        public bool IsComplete  {get; set;}
        public DateTime DueDate {get; set;}

        public Todo() {
            Text = "";
            IsComplete = false;
            DueDate = DateTime.Today;
        }

        public void PrintTodo() {
            Console.WriteLine($"Todo: {Text} IsComplete: {IsComplete} Due: {DueDate.ToShortDateString()}");
        }

        public string AsStringRepresentation() {
            return $"Todo: {Text}. Done? {IsComplete}. Due on: {DueDate.ToShortDateString()}";
        }

        public override string ToString() {
            string result = AsStringRepresentation();
            return result;

            // In the real world, I would not go to the trouble of having an AsStringRepresentation()
            //  method. I would just have the following code in the ToString() method:

            // return $"Todo: {Text} done? {IsComplete} due on: {DueDate} Shh...{Secret}";
        }
    }
}