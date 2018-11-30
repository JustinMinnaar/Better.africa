using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    public abstract class Contract<T> : BaseEntity where T : Contract<T>
    {
        public string Number { get; set; }

        #region SignDate

        public DateTime? SignDate { get; set; }

        public string SignDateError
        {
            get
            {
                if (SignDate == null || SignDate < new DateTime(2018, 12, 1))
                    return "Sign date is required and must be on or after 2018-12-01";

                return null;
            }
        }

        public T WithSignDate(int dd, int mm, int yy)
        {
            SignDate = new DateTime(yy, mm, 1);
            return (T)this;
        }

        #endregion SignDate

        #region InceptionDate

        public DateTime? InceptionDate { get; set; }

        public string InceptionDateError
        {
            get
            {
                var isNull = (InceptionDate == null);
                var isOutOfRange = (InceptionDate < new DateTime(2018, 12, 1) || InceptionDate > new DateTime(2028, 1, 1));
                if (isNull || isOutOfRange)
                    return "Inception date is required and must be on or after 2018-12-01 and before 2028-12-01.";

                if (SignDate != null)
                {
                    if (SignDate.Value.Day <= 7)
                    {
                        if (InceptionDate < SignDate.Value.FirstDayOfMonth())
                            return "Inception date cannot be before the month in which the contract was signed, when sign date is before the 8th.";
                    }
                    else
                    {
                        if (InceptionDate < SignDate.Value.FirstDayOfNextMonth())
                            return "Inception date cannot be before the month following the month in which the contract was signed, when sign date is after the 7th.";
                    }

                    if (InceptionDate > SignDate.Value.FirstDayOfMonth().AddMonths(6))
                        Errors.Add(nameof(InceptionDate), "Inception Date cannot be more than six months after the sign date as this would suggest an error when capturing the contract.");
                }

                return null;
            }
        }

        public T WithInceptionDate(int mm, int yy)
        {
            InceptionDate = new DateTime(yy, mm, 1);
            return (T)this;
        }

        #endregion InceptionDate

        #region BeforeSave

        public override void BeforeSave(EntityErrors errors)
        {
            base.BeforeSave(errors);

            errors.Add(nameof(SignDate), SignDateError);
            errors.Add(nameof(InceptionDate), InceptionDateError);
        }

        #endregion BeforeSave
    }
}