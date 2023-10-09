using System;
using System.IO;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;

namespace LSLib;
public static class LZ4Wrapper
{
    public static byte[] LZ4FrameCompress(byte[] data)
    {
        using var source = new MemoryStream(data);
        using var encoder = LZ4Stream.Encode(source);
        using var target = new MemoryStream();
        encoder.CopyTo(target);
        return target.ToArray();
    }

    public static byte[] LZ4FrameDecompress(byte[] data)
    {
        using var source = new MemoryStream(data);
        using var decoder = LZ4Stream.Decode(source);
        using var target = new MemoryStream();
        decoder.CopyTo(target);
        return target.ToArray();
    }

    public static Span<byte> LZ4BlockCompress(byte[] data, LZ4Level level = LZ4Level.L00_FAST)
    {
        int maxOutputSize = LZ4Codec.MaximumOutputSize(data.Length);
        byte[] output = new byte[maxOutputSize];
        int outputSize = LZ4Codec.Encode(data, output, level);
        return output.AsSpan(0, outputSize);
    }

    public static Span<byte> LZ4BlockDecompress(byte[] data, int outputSize)
    {
        byte[] output = new byte[outputSize];
        LZ4Codec.Decode(data, output);
        return output.AsSpan(0, outputSize);
    }
}
