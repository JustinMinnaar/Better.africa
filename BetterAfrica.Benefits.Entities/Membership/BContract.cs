using BetterAfrica.Shared;
using Knights.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterAfrica.Benefits.Entities
{
    public class BContract : BaseEntity
    {
        public int Number { get; set; }

        public Guid? AgentId { get; set; }

        public virtual BPerson Agent { get; set; }

        public string AgentError
        {
            get
            {
                if (AgentId == null) return "An Agent is required.";
                return null;
            }
        }

        public override string ToString()
        {
            return $"{Number} (" +
                $"signed: {SignDate?.ToShortDateString()} " +
                $"inception: {InceptionDate?.ToShortDateString()})";
        }

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

        #endregion SignDate

        #region InceptionDate

        public DateTime? InceptionDate { get; set; }

        public string InceptionDateError
        {
            get
            {
                if (SignDate == null) return "Sign Date is required to calculate InceptionDate";

                var isNull = (InceptionDate == null);
                var minDate = new DateTime(2018, 12, 1);
                var maxDate = SignDate.Value.AddMonths(6);
                var isOutOfRange = (InceptionDate < minDate || InceptionDate > maxDate);
                if (isNull || isOutOfRange)
                    return $"Inception date is required and must be on or after {minDate.ToShortDateString()} and before {maxDate.ToShortDateString()}.";

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

        #endregion InceptionDate

        #region BeforeSave

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            errors.Add(nameof(Agent), AgentError);
            errors.Add(nameof(SignDate), SignDateError);
            errors.Add(nameof(InceptionDate), InceptionDateError);
        }

        #endregion BeforeSave
    }

    public static class ContractHelpers
    {
        public static T WithAgent<T>(this T contract, BPerson agent)
            where T : BContract
        {
            contract.AgentId = agent.Id;
            contract.Agent = agent;
            return (T)contract;
        }

        public static T WithSignDate<T>(this T contract, int yy, int mm, int dd)
            where T : BContract
        {
            contract.SignDate = new DateTime(yy, mm, dd);
            return (T)contract;
        }

        public static T WithInceptionDate<T>(this T contract, int yy, int mm)
            where T : BContract
        {
            contract.InceptionDate = new DateTime(yy, mm, 1);
            return (T)contract;
        }
    }
}