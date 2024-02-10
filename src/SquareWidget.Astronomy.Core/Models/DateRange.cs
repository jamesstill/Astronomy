using System;

namespace SquareWidget.Astronomy.Core.Models
{
    public readonly struct DateRange
    {
        public readonly DateOnly StartDate { get; }
        public readonly DateOnly EndDate { get; }

        public DateRange(DateOnly startDate, DateOnly endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate), "End date must be greater than or equal to start date.");
            }

            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}
