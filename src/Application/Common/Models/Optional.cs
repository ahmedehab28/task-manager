namespace Application.Common.Models
{
    public readonly struct Optional<T>
    {
        private readonly T _value; 
        public bool IsAssigned { get; }
        public T Value => IsAssigned ? _value : throw new InvalidOperationException("No value present.");
        public Optional(T? value) 
        {
            IsAssigned = true;
            _value = value!;
        }
        public static implicit operator Optional<T>(T value) => new Optional<T>(value); 
        public override string ToString() => IsAssigned ? (Value?.ToString() ?? "null") : "<absent>";
    }
}
