namespace ReactiveServices.Configuration
{
    public class DependencyResolverId
    {
        private DependencyResolverId(string value)
        {
            Value = value;
        }
        private readonly string Value;

        public static DependencyResolverId FromString(string value)
        {
            return new DependencyResolverId(value);
        }

        public static bool operator ==(DependencyResolverId x, DependencyResolverId y)
        {
            if ((x == null) && (y == null)) return true;
            if ((x == null) || (y == null)) return false;
            return x.Value == y.Value;
        }

        public static bool operator !=(DependencyResolverId x, DependencyResolverId y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is DependencyResolverId)) return false;
            return ((DependencyResolverId)obj).Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "DependencyResolverId#" + Value;
        }
    }
}