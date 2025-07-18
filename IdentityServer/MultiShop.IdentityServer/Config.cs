// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MultiShop.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("ResourceCatalog")
            {
                Scopes = {"CatalogFullPermission","CatalogReadPermission"}
            },
            new ApiResource("ResourceDiscount")
            {
                Scopes = {"DiscountFullPermission"}
            }, 
            new ApiResource("ResourceOrder")
            {
                Scopes = {"OrderFullPermission"}
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            new ApiResource("ResourceCargo")
            {
                Scopes = {"CargoFullPermission",""}
            },
            new ApiResource("ResourceBasket")
            {
                Scopes = {"BasketFullPermission"}
            }
            ,
             new ApiResource("ResourceOcelot")
            {
                Scopes = {"OcelotFullPermission"}
            },
             new ApiResource("ResourceComment")
            {
                Scopes = {"CommentFullPermission"}
            },
             new ApiResource("ResourcePayment")
            {
                Scopes = {"PaymentFullPermission"}
            },
             new ApiResource("ResourceImages")
            {
                Scopes = {"ImagesFullPermission"}
            }

        };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("CatalogFullPermission","Full auhority for catalog operations"),
                new ApiScope("CatalogReadPermission","Reading authority for catalog operations"),
                new ApiScope("DiscountFullPermission","Full auhority for discount"),
                new ApiScope("OrderFullPermission","Full auhority for order operations"),
                new ApiScope("CargoFullPermission","Full auhority for cargo operations"),
                new ApiScope("BasketFullPermission","Full auhority for basket operations"),
                new ApiScope("OcelotFullPermission","Full auhority for ocelot operations"),
                new ApiScope("CommentFullPermission","Full auhority for comment operations"),
                new ApiScope("PaymentFullPermission","Full auhority for payment operations"),
                new ApiScope("ImagesFullPermission","Full auhority for image operations"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //Visitor
                new Client { 
                    ClientId = "MultiShopVisitorId",
                    ClientName="MultiShopVisitorUser",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowedScopes = { "CatalogReadPermission", "CatalogFullPermission", "OcelotFullPermission","CommentFullPermission","ImagesFullPermission", IdentityServerConstants.LocalApi.ScopeName },
                    AllowAccessTokensViaBrowser = true
                },
                //Manager
                new Client
                {
                    ClientId = "MultiShopManagerId",
                    ClientName="MultiShopManagerUser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowedScopes = {"CatalogFullPermission", "CatalogReadPermission", "BasketFullPermission","OcelotFullPermission", "PaymentFullPermission","CommentFullPermission", "ImagesFullPermission" 
                    ,
                    IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "MultiShopAdminId",
                    ClientName = "MultiShopAdminUser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowedScopes = { "CatalogFullPermission", "CatalogReadPermission", 
                        "DiscountFullPermission", "OrderFullPermission",
                        "CargoFullPermission","BasketFullPermission","OcelotFullPermission","CommentFullPermission","PaymentFullPermission",
                        "ImagesFullPermission",
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile},    
                    AccessTokenLifetime = 600
                    
                }
            };
    }
}