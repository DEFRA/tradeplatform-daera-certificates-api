// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IModelMapper<in TSource, out TDestination>
{
    TDestination Map(TSource source);
}