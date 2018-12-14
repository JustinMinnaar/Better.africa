using System;
using BetterAfrica.Shared;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     A person of a particular type to be included in the member.
    /// </summary>
    public class CMemberDependency : CPersonDependency
    {
        /// <summary></summary>
        public int MemberId { get; set; }

        /// <summary></summary>
        public virtual CMember Member { get; set; }

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            Person.BeforeSave(errors);
        }
    }
}