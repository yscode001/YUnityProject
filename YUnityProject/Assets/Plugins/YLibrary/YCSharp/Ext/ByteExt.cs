using System.Text;

namespace YCSharp
{
    public static class ByteExt
    {
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
    }
}