// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class TradeAddress
{
    public CodeType Postcode { get; set; }

    public TextType LineOne { get; set; }

    public TextType LineTwo { get; set; }

    public TextType LineThree { get; set; }

    public TextType LineFour { get; set; }

    public TextType LineFive { get; set; }

    public TextType CityName { get; set; }

    public IDType CountryCode { get; set; }

    public TextType CountryName { get; set; }

    public IDType CountrySubDivisionCode { get; set; }

    public TextType CountrySubDivisionName { get; set; }

    public CodeType TypeCode { get; set; }
}