using AutoMapper;

namespace MatrimonyApiService.Commons.converters;
public class Base64ToByteArrayConverter : IValueConverter<string?, byte[]?>
{
    public byte[]? Convert(string? sourceMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(sourceMember))
            return null;

        try
        {
            string base64String = sourceMember;
            if (sourceMember.Contains(","))
            {
                base64String = sourceMember.Split(',')[1];
            }

            return System.Convert.FromBase64String(base64String);
        }
        catch
        {
            // Handle or log the error as needed
            return null;
        }
    }
}
