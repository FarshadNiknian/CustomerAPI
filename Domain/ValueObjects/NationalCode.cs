using Domain.Common;

namespace Domain.ValueObjects;

public sealed record NationalCode 
{
    public string Value { get; }

    private NationalCode(string value) => Value = value;

    public static NationalCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("کد ملی الزامی است.");
        }

        value = value.Trim();

        if (!IsValid(value))
        {
            throw new DomainException("فرمت یا رقم کنترلی کد ملی نامعتبر است.");
        }

        return new NationalCode(value);
    }

    public static NationalCode CreateUnsafe(string value) => new(value);

    private static bool IsValid(ReadOnlySpan<char> code)
    {
        //check length
        if (code.Length != 10)
        {
            return false;
        }

       //check is numner
        for (int i = 0; i < 10; i++)
        {
            if (!char.IsAsciiDigit(code[i]))
            {
                return false;
            }
        }

        //check duplicate
        bool allSame = true;
        for (int i = 1; i < 10; i++)
        {
            if (code[i] != code[0])
            {
                allSame = false;
                break;
            }
        }
        if (allSame)
        {
            return false;
        }

        //checkSum
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (code[i] - '0') * (10 - i);
        }

        int remainder = sum % 11;
        int checkDigit = code[9] - '0';

        return remainder < 2
            ? checkDigit == remainder
            : checkDigit == 11 - remainder;
    }

    public override string ToString() => Value;
}
