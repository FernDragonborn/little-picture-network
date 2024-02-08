using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LittlePictureNetworkBackend.Data;

public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (x, y) => x.DayNumber == y.DayNumber,
        dateOnly => dateOnly.GetHashCode())
    { }
}
