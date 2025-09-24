using System.Collections.Generic;
namespace TaskSchedulerDariaSt.Models
{

    // Changes for serialization were added with the help of GPT.
    // Rest of the code - from the course.
    public class Node<T>
    {
        private T value;
        private Node<T>? next;

        // GPT help: parameterless constructor required for (de)serialization
        public Node() { }

        public Node(T value)
        {
            this.value = value;
            this.next = null;
        }
        public Node(T value, Node<T> next) 
        {
            this.value = value;
            this.next = next;
        }

        // GPT help: public properties mapped to the same private fields
        public T? Value
        {
            get => this.value;
            set => this.value = value!;
        }
        public Node<T>? Next
        {
            get => this.next;
            set => this.next = value;
        }

        public T GetValue() => this.value;
        public Node<T>? GetNext() => this.next;
        public void SetValue(T value)
        {
            this.value = value;
        }
        public void SetNext(Node<T>? next)
        {
            this.next = next;
        }
        public override string ToString() => this.value?.ToString();
        public bool HasNext() 
        {
            return (this.next != null);
        }

    }
}

