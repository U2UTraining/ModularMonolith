﻿namespace ModularMonolithBoundedContexts.Common.ValueObjects;

public sealed class NonEmptyStringConverter
: ValueConverter<NonEmptyString, string>
{
  public NonEmptyStringConverter()
  : base(
    nes => nes.Value,
    value => new NonEmptyString(value)
  )
  { }
}
