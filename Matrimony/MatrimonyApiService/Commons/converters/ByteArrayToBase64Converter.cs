using AutoMapper;

namespace MatrimonyApiService.Commons.converters;
public class ByteArrayToBase64Converter : IValueConverter<byte[]?, string?>
{
    public string? Convert(byte[]? sourceMember, ResolutionContext context)
    {
        if (sourceMember == null || sourceMember.Length == 0)
            return null;

        string base64 = System.Convert.ToBase64String(sourceMember);
        return $"data:image/jpeg;base64,{base64}";
    }
}
