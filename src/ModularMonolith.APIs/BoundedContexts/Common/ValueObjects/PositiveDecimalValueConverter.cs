﻿namespace U2U.ModularMonolith.BoundedContexts.Common.ValueObjects;

public sealed class PositiveDecimalValueConverter
: ValueConverter<PositiveDecimal, decimal>
{
  public PositiveDecimalValueConverter()
  : base(
    pd => pd.Value,
    value => new PositiveDecimal(value)
    ) { }
}
