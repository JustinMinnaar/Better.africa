using System;
using BetterAfrica.Shared;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     A person of a particular type to be included in the membership.
    /// </summary>
    public class BMemberDependency : BContractDependency
    {
        /// <summary></summary>
        public int MemberId { get; set; }

        /// <summary></summary>
        public virtual BMember Member { get; set; }

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            Person.BeforeSave(errors);
        }
    }
}