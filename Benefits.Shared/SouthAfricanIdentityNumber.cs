using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knights.Core.Common;

namespace Benefits.Shared
{
    public class SouthAfricanIdentityNumber
    {
        public static SouthAfricanIdentityNumber Random(int minYear, int maxYear, bool SaCitizen = true)
        {
            var r = new Random();
            var year = r.Next(minYear, maxYear + 1);
            var month = r.Next(1, 12);
            var day = r.Next(1, DateTime.DaysInMonth(year, month) + 1);
            var gender = r.Next(0, 10);
            var middle = r.Next(0, 10000);
            var citizen = SaCitizen ? 0 : r.Next(0, 10);

            var id = new SouthAfricanIdentityNumber
            {
                Number = $"{(year % 100):00}{month:00}{day:00}{gender:0}{middle:000}{citizen:0}"
            };

            var checksum = id.GetControlDigit();
            id.Number += $"{checksum:0}";

            if (id.IsValid) return id;

            throw new NotImplementedException();
        }

        public string Number { get; set; }

        public DateTime? Birthdate
        {
            get
            {
                try
                {
                    if (Number == null) return null;

                    var number = Number.ToDigitsOnly();
                    if (Number.Length < 6) return null;

                    var yy = int.Parse(number.Substring(0, 2));
                    if (yy > 28) yy = 1900 + yy; else yy = 2000 + yy;

                    var mm = int.Parse(number.Substring(2, 2));
                    var dd = int.Parse(number.Substring(4, 2));

                    var date = new DateTime(yy, mm, dd);

                    return date;
                }
                catch (ArgumentOutOfRangeException) { return null; }
            }
        }

        public bool IsMale => Number != null && Number.Length == 13 && "56789".Contains(Number[6]);

        public bool IsFemale => Number != null && Number.Length == 13 && "01234".Contains(Number[6]);

        public char Citizenship => Number != null && Number.Length == 13 ? Number[10] : ' ';

        public bool IsSouthAfricanCitizen => Citizenship == '0';

        public bool ControlDigitIsValid => GetControlDigit() == Number[12];

        public bool IsValid => ControlDigitIsValid && Birthdate != null;

        //
        // Stored in a property 'ParseIdString'. Returns: the valid digit between 0 and 9, or
        // -1 if the method fails.

        /// <summary>
        ///     Calculates the valid digit expected for the ID number specified.
        ///     This method assumes that the 13-digit id number has valid digits in position 0 through 11.
        /// </summary>
        /// <returns>space if it fails, otherwise the digit from 0 to 9.</returns>
        private char GetControlDigit()
        {
            int d = -1;
            try
            {
                int a = 0;
                for (int i = 0; i < 6; i++)
                {
                    a += int.Parse(Number[2 * i].ToString());
                }
                int b = 0;
                for (int i = 0; i < 6; i++)
                {
                    b = b * 10 + int.Parse(Number[2 * i + 1].ToString());
                }
                b *= 2;
                int c = 0;
                do
                {
                    c += b % 10;
                    b = b / 10;
                }
                while (b > 0);
                c += a;
                d = 10 - (c % 10);
                if (d == 10) d = 0;
            }
            catch { return ' '; }

            var digit = (char)(d + (int)'0');

            return digit;
        }
    }
}