namespace HLParserService.Service
{
    public interface IHL7Parser<T>
    {
        string Encode(T objToEncode);
        T Parse(string hl7Message);
    }
}
