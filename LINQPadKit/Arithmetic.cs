using LINQPad.Controls;
using NStandard;
using System.Collections;
using System.Runtime.InteropServices;

namespace LINQPadKit;

public partial class Arithmetic<T> : Table where T : unmanaged
{
    public bool Decoded { get; }
    public int ChunkSize { get; }

    public Arithmetic(T obj, bool decoded) : base(
        decoded
            ? GetDecodedMemory(obj, 32)
            : GetOriginMemory(obj, 32)
        )
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            throw new NotSupportedException("Only Windows is supported.");
        }

        Decoded = decoded;
        ChunkSize = 32;
    }

    public Arithmetic(T obj, bool decoded, int chunk) : base(
        decoded
            ? GetDecodedMemory(obj, chunk)
            : GetOriginMemory(obj, chunk)
        )
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            throw new NotSupportedException("Only Windows is supported.");
        }

        Decoded = decoded;
        ChunkSize = chunk;
    }

    private static TableCell ParseCell(bool isHeader, bool value)
    {
        return new TableCell(isHeader, new Span(value ? "1" : "0"));
    }

    private unsafe static bool[] GetBits(T obj)
    {
        var ptr = (byte*)&obj;
        var size = Marshal.SizeOf<T>();

        var bytes = new byte[size];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = ptr[i];
        }

        var bits = new BitArray(bytes);
        return bits.OfType<bool>().ToArray();
    }

    private static IEnumerable<int> GetIndices(int start, int length, bool reverse)
    {
        if (reverse)
        {
            for (int i = start; i < start + length; i++)
            {
                yield return length - 1 - i;
            }
        }
        else
        {
            for (int i = start; i < start + length; i++)
            {
                yield return i;
            }
        }
    }

    private static void StyleIndex(TableRow row)
    {
        row.CellStyles["width"] = "12px";
        row.CellStyles["text-align"] = "center";
        row.CellStyles["font-size"] = "9px";
    }

    private static void StyleHeader(TableRow row)
    {
        row.CellStyles["width"] = "12px";
        row.CellStyles["text-align"] = "center";
    }

    private static void Style(TableRow row)
    {
        row.CellStyles["text-align"] = "center";
    }

    private static TableRow[] GetDecodedMemory(T obj, int chunk)
    {
        if (obj is short) return DecodeInteger(obj, true);
        if (obj is int) return DecodeInteger(obj, true);
        if (obj is long) return DecodeInteger(obj, true);
#if NET7_0_OR_GREATER
        if (obj is Int128) return DecodeInteger(obj, true);
#endif

        if (obj is ushort) return DecodeInteger(obj, false);
        if (obj is uint) return DecodeInteger(obj, false);
        if (obj is ulong) return DecodeInteger(obj, false);
#if NET7_0_OR_GREATER
        if (obj is UInt128) return DecodeInteger(obj, false);
#endif

        if (obj is float) return DecodeFloating(obj, 8, 23);
        if (obj is double) return DecodeFloating(obj, 11, 52);
        if (obj is decimal) return DecodeDecimal(obj);

        return GetOriginMemory(obj, chunk);
    }

#if NET7_0_OR_GREATER
    private static UInt128 GetBitsValue(bool[] bits)
    {
        var _bits = new BitArray(bits);
        var bytes = new byte[16];
        _bits.CopyTo(bytes, 0);
        return BitConverterEx.ToUInt128(bytes, 0);
    }
#else
    private static decimal GetBitsValue(bool[] bits)
    {
        var _bits = new BitArray(bits);
        if (bits.Length <= 8)
        {
            var bytes = new byte[8];
            _bits.CopyTo(bytes, 0);
            return BitConverter.ToUInt64(bytes, 0);
        }
        else
        {
            var bytes = new byte[12];
            _bits.CopyTo(bytes, 0);
            var high = BitConverter.ToInt32(bytes, 0);
            var mid = BitConverter.ToInt32(bytes, 4);
            var low = BitConverter.ToInt32(bytes, 8);

            return new decimal(high, mid, low, false, 0);
        }
    }
#endif

    private static TableRow[] DecodeInteger(T obj, bool signed)
    {
        var bits = GetBits(obj);

        if (signed)
        {
            var sign = bits[bits.Length - 1];
            var value = bits[..(bits.Length - 1)];

            return
            [
                new TableRow(
                [
                    new TableCell(true, new Span("Sign")),
                    new TableCell(true, new Span("Value"))
                    {
                        ColSpan = bits.Length - 1,
                    },
                ]).Pipe(StyleHeader),

                new TableRow(
                [
                    ..
                    from n in GetIndices(0, bits.Length, true)
                    select new TableCell(true, new Span($"{n}"))
                ]).Pipe(StyleIndex),

                new TableRow(
                [
                    ParseCell(false, sign),
                    ..
                    from c in value.Reverse()
                    select ParseCell(false, c)
                ]).Pipe(Style),

                new TableRow(
                [
                    new TableCell(true, new Span(sign ? "-" : "+")),
                    new TableCell(true, new Span($"{GetBitsValue(value)}"))
                    {
                        ColSpan = bits.Length - 1,
                    },
                ]).Pipe(StyleHeader),
            ];
        }
        else
        {
            return
            [
                new TableRow(
                [
                    new TableCell(true, new Span("Value"))
                    {
                        ColSpan = bits.Length,
                    },
                ]).Pipe(StyleHeader),

                new TableRow(
                [
                    ..
                    from n in GetIndices(0, bits.Length, true)
                    select new TableCell(true, new Span($"{n}"))
                ]).Pipe(StyleIndex),

                new TableRow(
                [
                    ..
                    from c in bits.Reverse()
                    select ParseCell(false, c),
                ]).Pipe(Style),

                new TableRow(
                [
                    new TableCell(true, new Span($"{GetBitsValue(bits)}"))
                    {
                        ColSpan = bits.Length,
                    },
                ]).Pipe(StyleHeader),
            ];
        }
    }

    private static TableRow[] DecodeFloating(T obj, int elength, int mlength)
    {
        var bits = GetBits(obj);

        var sign = bits[bits.Length - 1];
        var exponent = bits[(mlength)..(mlength + elength)];
        var mantissa = bits[..mlength];

        return
        [
            new TableRow(
            [
                new TableCell(true, new Span("Sign")),
                new TableCell(true, new Span("Exponent"))
                {
                    ColSpan = elength,
                },
                new TableCell(true, new Span("Mantissa"))
                {
                    ColSpan = mlength,
                },
            ]).Pipe(StyleHeader),

            new TableRow(
            [
                ..
                from n in GetIndices(0, bits.Length, true)
                select new TableCell(true, new Span($"{n}"))
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ParseCell(false, sign),
                ..
                from c in exponent.Reverse()
                select ParseCell(false, c),
                ..
                from c in mantissa.Reverse()
                select ParseCell(false, c),
            ]).Pipe(Style),

            new TableRow(
            [
                new TableCell(true, new Span(sign ? "-" : "+")),
                new TableCell(true, new Span($"{GetBitsValue(exponent)}"))
                {
                    ColSpan = elength,
                },
                new TableCell(true, new Span($"{GetBitsValue(mantissa)}"))
                {
                    ColSpan = mlength,
                },
            ]).Pipe(StyleHeader),
        ];
    }

    private static TableRow[] DecodeDecimal(T obj)
    {
        var bits = GetBits(obj);

        var scale = bits[16..24];
        var sign = bits[31];
        var high = bits[32..64];
        var low = bits[64..96];
        var mid = bits[96..];
        bool[] value = [.. low, .. mid, .. high];

        return
        [
            new TableRow(
            [
                new TableCell(true, new Span("Scale"))
                {
                    ColSpan = 8,
                },
                new TableCell(true, new Span("Reserved"))
                {
                    ColSpan = 16,
                },
                new TableCell(true, new Span("Sign")),
                new TableCell(true, new Span("Reserved"))
                {
                    ColSpan = 7,
                },
            ]).Pipe(StyleHeader),

            new TableRow(
            [
                ..
                from n in RangeEx.Create(16, 8).Reverse()
                select new TableCell(true, new Span($"{n}")),
                ..
                from n in RangeEx.Create(0, 16).Reverse()
                select new TableCell(true, new Span($"{n}")),
                ..
                from n in RangeEx.Create(24, 8).Reverse()
                select new TableCell(true, new Span($"{n}")),
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ..
                from c in scale.Reverse()
                select ParseCell(false, c),
                ..
                from n in RangeEx.Create(0, 16)
                select ParseCell(false, false),
                ParseCell(false, sign),
                ..
                from n in RangeEx.Create(0, 7)
                select ParseCell(false, false),
            ]).Pipe(Style),

            new TableRow(
            [
                new TableCell(true, new Span($"{GetBitsValue(scale)}"))
                {
                    ColSpan = 8,
                },
                new TableCell(true, new Span(""))
                {
                    ColSpan = 16,
                },
                new TableCell(true, new Span(sign ? "-" : "+")),
                new TableCell(true, new Span(""))
                {
                    ColSpan = 7,
                },
            ]).Pipe(StyleHeader),

            // High
            new TableRow(
            [
                new TableCell(true, new Span("High"))
                {
                    ColSpan = 32,
                },
            ]).Pipe(StyleHeader),

            new TableRow(
            [
                ..
                from n in RangeEx.Create(32, 32).Reverse()
                select new TableCell(true, new Span($"{n}")),
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ..
                from c in high.Reverse()
                select ParseCell(false, c),
            ]).Pipe(Style),

            new TableRow(
            [
                new TableCell(true, new Span($"{GetBitsValue(high)}"))
                {
                    ColSpan = 32,
                },
            ]).Pipe(StyleHeader),

            // Mid
            new TableRow(
            [
                new TableCell(true, new Span("Mid"))
                {
                    ColSpan = 32,
                },
            ]).Pipe(StyleHeader),

            new TableRow(
            [
                ..
                from n in RangeEx.Create(96, 32).Reverse()
                select new TableCell(true, new Span($"{n}")),
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ..
                from c in mid.Reverse()
                select ParseCell(false, c),
            ]).Pipe(Style),

            new TableRow(
            [
                new TableCell(true, new Span($"{GetBitsValue(mid)}"))
                {
                    ColSpan = 32,
                },
            ]).Pipe(StyleHeader),

            // Low
            new TableRow(
            [
                new TableCell(true, new Span("Low"))
                {
                    ColSpan = 32,
                },
            ]).Pipe(StyleHeader),

            new TableRow(
            [
                ..
                from n in RangeEx.Create(64, 32).Reverse()
                select new TableCell(true, new Span($"{n}")),
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ..
                from c in low.Reverse()
                select ParseCell(false, c),
            ]).Pipe(Style),

            new TableRow(
            [
                new TableCell(true, new Span($"{GetBitsValue(low)}"))
                {
                    ColSpan = 64,
                },
            ]).Pipe(StyleHeader),

            // High & Mid & Low
            new TableRow(
            [
                new TableCell(true, new Span($"Value = {GetBitsValue(value)}"))
                {
                    ColSpan = 64,
                },
            ]).Pipe(StyleHeader),
        ];
    }

    private static TableRow[] GetOriginMemory(T obj, int chunkSize)
    {
        var bits = GetBits(obj);

#if NET6_0_OR_GREATER
        var chunks = bits.Chunk(chunkSize);
        return
        [
            ..
            (
                from pair in chunks.Pairs()
                let i = pair.Index
                let chunk = pair.Value
                select new TableRow[]
                {
                    new TableRow(
                    [
                        ..
                        from n in GetIndices(i * chunkSize, chunk.Length, false)
                        select new TableCell(true, new Span($"{n}"))
                    ]).Pipe(StyleIndex),

                    new TableRow(
                    [
                        ..
                        from c in chunk
                        select ParseCell(false, c)
                    ]).Pipe(Style),
                }
            ).SelectMany(x => x)
        ];
#else
        return
        [
            new TableRow(
            [
                ..
                from n in GetIndices(0, bits.Length, false)
                select new TableCell(true, new Span($"{n}"))
            ]).Pipe(StyleIndex),

            new TableRow(
            [
                ..
                from c in bits
                select ParseCell(false, c)
            ]).Pipe(Style),
        ];
#endif
    }
}
