namespace AliasEngine
{
    public class Alias
    {
        public string FirstPart
        {
            get; private set;
        }
        public string SecondPart
        {
            get; private set;
        }

        public Alias(string source)
        {
            var firstPart = source.Substring(0, source.IndexOf(" "));
            var secondPart = source.Substring(source.IndexOf(" ") + 1);

            FirstPart = firstPart;
            SecondPart = secondPart;
        }
    }
}
