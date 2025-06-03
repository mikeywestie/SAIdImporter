using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

public class SouthAfricanID
{
    public string IDNumber { get; }
    public DateTime? DateOfBirth { get; }
    public string Gender { get; }
    public bool IsValid { get; }

    public SouthAfricanID(string idNumber)
    {
        IDNumber = idNumber.PadLeft(13, '0');

        DateTime parsedDob;
        bool isChecksumValid = ValidateChecksum(IDNumber);
        bool isDobValid = TryParseDOB(IDNumber.Substring(0, 6), out parsedDob);

        IsValid = isChecksumValid && isDobValid;
        DateOfBirth = IsValid ? parsedDob : (DateTime?)null;
        Gender = GenderFromID(idNumber);
    }



    private static bool ValidateChecksum(string id)
    {
        if (id.Length != 13 || !id.All(char.IsDigit))
            return false;

        int[] digits = id.Select(c => c - '0').ToArray();

        int sumOdd = digits.Where((d, i) => i % 2 == 0 && i < 12).Sum();

        string evenDigits = string.Concat(digits.Where((d, i) => i % 2 == 1 && i < 12));
        int evenNumber = int.Parse(evenDigits) * 2;

        int sumEven = evenNumber.ToString().Select(c => c - '0').Sum();

        int total = sumOdd + sumEven;
        int checksum = (10 - (total % 10)) % 10;

        return checksum == digits[12];
    }

    private static bool TryParseDOB(string dobPart, out DateTime dob)
    {
        dob = DateTime.MinValue;
        if (dobPart.Length != 6)
            return false;

        // Extract year, month, day as integers
        int year = int.Parse(dobPart.Substring(0, 2));
        int month = int.Parse(dobPart.Substring(2, 2));
        int day = int.Parse(dobPart.Substring(4, 2));

        // Infer century assuming age between 16 and 84
        int currentYear = DateTime.Now.Year % 100;
        int century = (year <= currentYear) ? 2000 : 1900;
        year += century;

        try
        {
            dob = new DateTime(year, month, day);
            int age = DateTime.Now.Year - dob.Year;
            if (age < 16 || age > 84)
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GenderFromID(string id)
    {
        if (id.Length < 10)
            return "Unknown";

        int genderNum = int.Parse(id.Substring(6, 4));
        return genderNum < 5000 ? "Female" : "Male";
    }
}
