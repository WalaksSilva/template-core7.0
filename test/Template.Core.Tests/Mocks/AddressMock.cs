﻿using Bogus;
using Template.API.ViewModels.Address;

namespace Template.Core.Tests.Mocks
{
    public static class AddressMock
    {
        public static Faker<AddressViewModel> AddressViewModelFaker =>
            new Faker<AddressViewModel>("pt_BR")
            .CustomInstantiator(x => new AddressViewModel
            (
                id: x.Random.Number(1, 10),
                cep: x.Address.ZipCode(),
                street: x.Address.StreetName(),
                streetFull: x.Address.StreetAddress(),
                uf: x.Address.State()
            ));
    }
}
